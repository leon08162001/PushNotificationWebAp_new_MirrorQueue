﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using TIBCO.EMS;

namespace Common.LinkLayer
{
    [Serializable]

    public class GenericEMSAdapter : BaseEMSAdapter
    {
        protected Type _DataType;
        protected Dictionary<string, string> _DicDataType = new Dictionary<string, string>();
        protected DataTable MessageDT = new DataTable(); //將EMSMessage資料轉換成DataTable所使用的DataTable

        private static GenericEMSAdapter singleton;

        public GenericEMSAdapter() : base() { }

        public GenericEMSAdapter(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName)
            : base(Uri, DestinationFeature, ListenName, SendName) { }

        public GenericEMSAdapter(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName, string UserName, string Pwd)
            : base(Uri, DestinationFeature, ListenName, SendName, UserName, Pwd) { }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static GenericEMSAdapter getSingleton()
        {
            if (singleton == null)
            {
                singleton = new GenericEMSAdapter();
            }
            return singleton;
        }

        public static GenericEMSAdapter getSingleton(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName)
        {
            if (singleton == null)
            {
                singleton = new GenericEMSAdapter(Uri, DestinationFeature, ListenName, SendName);
            }
            return singleton;
        }

        public static GenericEMSAdapter getSingleton(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName, string UserName, string Pwd)
        {
            if (singleton == null)
            {
                singleton = new GenericEMSAdapter(Uri, DestinationFeature, ListenName, SendName, UserName, Pwd);
            }
            return singleton;
        }

        public Type DataType
        {
            set { _DataType = value; }
            get { return _DataType; }
        }

        public override void processEMSMessage(Message message)
        {
            string Message = "";
            string _ErrMsg = "";
            Dictionary<string, string> EMSMessageDictionary = new Dictionary<string, string>();
            System.Collections.IEnumerator PropertyNames = message.PropertyNames;
            PropertyNames.Reset();
            while (PropertyNames.MoveNext())
            {
                string key = PropertyNames.Current.ToString();
                if (key.Equals("JMSXDeliveryCount"))
                {
                    continue;
                }
                EMSMessageDictionary.Add(key, message.GetStringProperty(key));
            }
            if (EMSMessageDictionary.Keys.Count == 0)
            {
                return;
            }
            foreach (string key in EMSMessageDictionary.Keys)
            {
                Message += key + "=" + EMSMessageDictionary[key] + ";";
            }
            //0.檢查是否為HeartBeat訊息,若是則忽略不處理
            if (EMSMessageDictionary.ContainsKey("0"))
            {
                return;
            }
            //1.檢查是否有指定DataType,以便與傳過來的FixData作驗證用
            if (_DataType == null)
            {
                _ErrMsg = "not yet assigned Fix Tag Type of Fix Data";
                if (log.IsInfoEnabled) log.Info(_ErrMsg);
                if (UISyncContext != null && IsEventInUIThread)
                {
                    UISyncContext.Post(OnEMSMessageHandleFinished, new EMSMessageHandleFinishedEventArgs(_ErrMsg, null));
                }
                else
                {
                    OnEMSMessageHandleFinished(new EMSMessageHandleFinishedEventArgs(_ErrMsg, null));
                }
                return;
            }
            _DicDataType = ConvertFixTagClassConstants(_DataType);
            //2.驗證EMS傳過來的FixData的tag正確性(與指定的DataType)
            foreach (string key in EMSMessageDictionary.Keys)
            {
                if (!_DicDataType.ContainsKey(key))
                {
                    _ErrMsg = string.Format("Fix Data's Tag[{0}] Not in the assigned type[{1}]", key, _DataType.Name);
                    if (log.IsInfoEnabled) log.Info(_ErrMsg);
                    if (UISyncContext != null && IsEventInUIThread)
                    {
                        UISyncContext.Post(OnEMSMessageHandleFinished, new EMSMessageHandleFinishedEventArgs(_ErrMsg, null));
                    }
                    else
                    {
                        OnEMSMessageHandleFinished(new EMSMessageHandleFinishedEventArgs(_ErrMsg, null));
                    }
                    return;
                }
            }

            //建立DataTable Schema
            if (MessageDT.Columns.Count < 1)
            {
                CreateTableSchema(_DicDataType);
            }
            //匯入每筆message到DataTable
            DataRow MessageRow;
            AddMessageToTable(EMSMessageDictionary, out MessageRow);
            if (MessageRow != null && MessageRow.Table.Columns.Contains("MacAddress") && !MessageRow.IsNull("MacAddress") && this.SendName.IndexOf("#") != -1)
            {
                this.ReStartSender(this.SendName.Replace("#", MessageRow["MacAddress"].ToString()));
            }
            //觸發每筆EMSMessage資料處理完成事件
            if (UISyncContext != null && IsEventInUIThread)
            {
                UISyncContext.Post(OnEMSMessageHandleFinished, new EMSMessageHandleFinishedEventArgs(_ErrMsg, MessageRow));
            }
            else
            {
                OnEMSMessageHandleFinished(new EMSMessageHandleFinishedEventArgs(_ErrMsg, MessageRow));
            }
        }

        /// <summary>
        /// 轉換MsgType.cs中Tag Class的常數名稱及常數值為Dictionary
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        protected Dictionary<string, string> ConvertFixTagClassConstants(Type DataType)
        {
            Dictionary<string, string> DicFix = new Dictionary<string, string>();
            FieldInfo[] fieldInfos = DataType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (FieldInfo fi in fieldInfos)
            {
                if (fi.IsLiteral && !fi.IsInitOnly)
                {
                    DicFix.Add(fi.GetValue(DataType).ToString(), fi.Name);
                }
            }
            return DicFix;
        }

        /// <summary>
        /// 建立存放EMSMessge資料內容的DataTable Schema
        /// </summary>
        /// <param name="DicDataType">指定的DataType</param>
        private void CreateTableSchema(Dictionary<string, string> DicDataType)
        {
            MessageDT.Reset();
            string MessageID = _DataType.GetField("MessageID") == null ? "710" : _DataType.GetField("MessageID").GetValue(_DataType).ToString();
            string TotalRecords = _DataType.GetField("TotalRecords") == null ? "10038" : _DataType.GetField("TotalRecords").GetValue(_DataType).ToString();
            foreach (string key in DicDataType.Keys)
            {
                //必須為非requestID的tag及非總筆數的tag才建立欄位(停用下列程式碼)
                //if (!key.Equals(MessageID) && !key.Equals(TotalRecords))
                //必須為非總筆數的tag才建立欄位
                if (!key.Equals(TotalRecords))
                {
                    string FiledName = DicDataType[key].ToString();
                    MessageDT.Columns.Add(FiledName, typeof(System.String));
                }
            }
        }
        /// <summary>
        /// 將每筆EMSMessage加入至DataTable
        /// </summary>
        /// <param name="DicEMSMessage">每筆EMSMessage的Dictionary資料形式</param>
        /// <param name="MessagRow"></param>
        private void AddMessageToTable(Dictionary<string, string> DicEMSMessage, out DataRow MessagRow)
        {
            try
            {
                string MessageID = _DataType.GetField("MessageID") == null ? "710" : _DataType.GetField("MessageID").GetValue(_DataType).ToString();
                string TotalRecords = _DataType.GetField("TotalRecords") == null ? "10038" : _DataType.GetField("TotalRecords").GetValue(_DataType).ToString();
                DataRow tmpMessagRow = MessageDT.NewRow();
                foreach (string key in DicEMSMessage.Keys)
                {
                    //必須為非requestID的tag及非總筆數的tag才設定欄位值(停用下列程式碼)
                    //if (!key.Equals(MessageID) && !key.Equals(TotalRecords))
                    //必須為非總筆數的tag才設定欄位值
                    if (!key.Equals(TotalRecords))
                    {
                        tmpMessagRow[_DicDataType[key].ToString()] = DicEMSMessage[key] == null ? DicEMSMessage[key] : DicEMSMessage[key].ToString();
                    }
                }
                MessageDT.Rows.Add(tmpMessagRow);
                MessagRow = tmpMessagRow;
            }
            catch (Exception ex)
            {
                MessagRow = null;
                //_ErrMsg += ex.Message + ";";
                if (log.IsErrorEnabled) log.Error("GenericEMSAdapter AddMessageToTable: ", ex);
            }
        }
    }
}
