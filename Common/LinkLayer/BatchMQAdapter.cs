﻿using Apache.NMS;
using Common.TopicMessage;
using Common.Utility;
//using Apache.NMS.ActiveMQ;
//using Apache.NMS.ActiveMQ.Commands;
//using Apache.NMS.Stomp;
//using Apache.NMS.Stomp.Commands;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common.LinkLayer
{
    /// <summary>
    /// MQ處理完批次資料的事件參數類別
    /// </summary>
    public class MQBatchFinishedEventArgs : EventArgs
    {
        private string _errorMessage;
        private DataTable _BatchResultTable;
        public MQBatchFinishedEventArgs()
        {
            _errorMessage = "";
        }
        public MQBatchFinishedEventArgs(string errorMessage, DataTable BatchResultTable)
        {
            _errorMessage = errorMessage;
            _BatchResultTable = BatchResultTable;
        }
        public string errorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        public DataTable BatchResultTable
        {
            get { return _BatchResultTable; }
            set { _BatchResultTable = value; }
        }
    }
    /// <summary>
    /// MessageHeader's Count與MessageBody's DataRow Count不符合事件參數類別
    /// </summary>
    public class MQBatchMismatchedEventArgs : EventArgs
    {
        private string _MismatchedMessage;
        public MQBatchMismatchedEventArgs()
        {
            _MismatchedMessage = "";
        }
        public MQBatchMismatchedEventArgs(string MismatchedMessage)
        {
            _MismatchedMessage = MismatchedMessage;
        }
        public string MismatchedMessage
        {
            get { return _MismatchedMessage; }
            set { _MismatchedMessage = value; }
        }
    }
    [Serializable]

    public class BatchMQAdapter : BaseMQAdapter
    {
        // Delegate
        public delegate void MQBatchFinishedEventHandler(object sender, MQBatchFinishedEventArgs e);
        List<MQBatchFinishedEventHandler> MQBatchFinishedEventDelegates = new List<MQBatchFinishedEventHandler>();
        private event MQBatchFinishedEventHandler _MQBatchFinished;
        public event MQBatchFinishedEventHandler MQBatchFinished
        {
            add
            {
                _MQBatchFinished += value;
                MQBatchFinishedEventDelegates.Add(value);
            }
            remove
            {
                _MQBatchFinished -= value;
                MQBatchFinishedEventDelegates.Remove(value);
            }
        }

        protected delegate void MQBatchMismatchedEventHandler(object sender, MQBatchMismatchedEventArgs e);
        List<MQBatchMismatchedEventHandler> MQBatchMismatchedEventDelegates = new List<MQBatchMismatchedEventHandler>();
        private event MQBatchMismatchedEventHandler _MQBatchMismatched;
        protected event MQBatchMismatchedEventHandler MQBatchMismatched
        {
            add
            {
                _MQBatchMismatched += value;
                MQBatchMismatchedEventDelegates.Add(value);
            }
            remove
            {
                _MQBatchMismatched -= value;
                MQBatchMismatchedEventDelegates.Remove(value);
            }
        }

        IApplicationContext applicationContext = ContextRegistry.GetContext();
        Config config;

        /// <summary>
        /// MQ完成所有批次資料處理時事件
        /// </summary>
        protected virtual void OnMQBatchFinished(object state)
        {
            MQBatchFinishedEventArgs e = state as MQBatchFinishedEventArgs;
            if (_MQBatchFinished != null)
            {
                _MQBatchFinished(this, e);
            }
        }
        /// <summary>
        /// MessageHeader's Count與MessageBody's DataRow Count不符合時事件(每次在接收訊息一開始呼叫ClearTimeOutMQReceivedMessage時觸發)
        /// </summary>
        /// <param name="state"></param>
        protected virtual void OnMQBatchMismatched(object state)
        {
            MQBatchMismatchedEventArgs e = state as MQBatchMismatchedEventArgs;
            if (_MQBatchMismatched != null)
            {
                _MQBatchMismatched(this, e);
            }
        }

        protected bool _IsBatchFinished = false;
        protected Type _DataType;
        protected Dictionary<string, string> _DicTagType = new Dictionary<string, string>();

        //註解紀錄傳送筆數資訊的Dictionary
        //protected Dictionary<string, MessageHeader> DicMessageHeader = new Dictionary<string,MessageHeader>();
        protected Dictionary<string, MessageBody> DicMessageBody = new Dictionary<string, MessageBody>();

        private static BatchMQAdapter singleton;

        public BatchMQAdapter() : base() { config = (Config)applicationContext.GetObject("Config"); this.MQBatchMismatched += new MQBatchMismatchedEventHandler(BatchMQAdapter_MQBatchMismatched); }

        public BatchMQAdapter(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName)
            : base(Uri, DestinationFeature, ListenName, SendName) { config = (Config)applicationContext.GetObject("Config"); this.MQBatchMismatched += new MQBatchMismatchedEventHandler(BatchMQAdapter_MQBatchMismatched); }

        public BatchMQAdapter(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName, string UserName, string Pwd)
            : base(Uri, DestinationFeature, ListenName, SendName, UserName, Pwd) { config = (Config)applicationContext.GetObject("Config"); this.MQBatchMismatched += new MQBatchMismatchedEventHandler(BatchMQAdapter_MQBatchMismatched); }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static BatchMQAdapter getSingleton()
        {
            if (singleton == null)
            {
                singleton = new BatchMQAdapter();
            }
            return singleton;
        }

        public static BatchMQAdapter getSingleton(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName)
        {
            if (singleton == null)
            {
                singleton = new BatchMQAdapter(Uri, DestinationFeature, ListenName, SendName);
            }
            return singleton;
        }

        public static BatchMQAdapter getSingleton(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName, string UserName, string Pwd)
        {
            if (singleton == null)
            {
                singleton = new BatchMQAdapter(Uri, DestinationFeature, ListenName, SendName, UserName, Pwd);
            }
            return singleton;
        }

        public Type DataType
        {
            set { _DataType = value; }
            get { return _DataType; }
        }

        public bool IsBatchFinished
        {
            get { return _IsBatchFinished; }
        }

        public override void RemoveAllEvents()
        {
            base.RemoveAllEvents();
            foreach (MQBatchFinishedEventHandler eh in MQBatchFinishedEventDelegates)
            {
                _MQBatchFinished -= eh;
            }
            MQBatchFinishedEventDelegates.Clear();
            foreach (MQBatchMismatchedEventHandler eh in MQBatchMismatchedEventDelegates)
            {
                _MQBatchMismatched -= eh;
            }
            MQBatchMismatchedEventDelegates.Clear();
        }

        public override void processMQMessage(IMessage message)
        {
            try
            {
                ClearTimeOutMQReceivedMessage();
                string _ErrMsg = "";
                //接收檔案
                if (message.Properties.Contains("filename"))
                {
                    IBytesMessage msg = message as IBytesMessage;
                    DataTable MessageDT = new DataTable();
                    MessageDT.TableName = "file";
                    try
                    {
                        foreach (object key in msg.Properties.Keys)
                        {
                            MessageDT.Columns.Add(key.ToString(), typeof(System.String));
                        }
                        MessageDT.Columns.Add("content", typeof(byte[]));
                        //匯入檔案內容到Datatable
                        DataRow MessageRow;
                        MessageRow = MessageDT.NewRow();
                        foreach (object key in msg.Properties.Keys)
                        {
                            MessageRow[key.ToString()] = msg.Properties[key.ToString()];
                        }
                        MessageRow["content"] = msg.Content;
                        MessageDT.Rows.Add(MessageRow);
                        RunOnMQMessageHandleFinished(_ErrMsg, MessageRow);
                        if (this.Handler != null)
                        {
                            this.Handler.WorkItemQueue.Enqueue(MessageDT);
                        }
                        _IsBatchFinished = true;
                        RunOnMQBatchFinished(_ErrMsg, MessageDT);
                        _IsBatchFinished = false;
                    }
                    catch (Exception ex1)
                    {
                        _ErrMsg = ex1.Message;
                        RunOnMQMessageHandleFinished(_ErrMsg, null);
                        _IsBatchFinished = true;
                        RunOnMQBatchFinished(_ErrMsg, MessageDT);
                        _IsBatchFinished = false;
                        if (log.IsErrorEnabled) log.Error(ex1.Message, ex1);
                    }
                }
                //接收文字訊息
                else
                {
                    Dictionary<string, string> MQMessageDictionary = new Dictionary<string, string>();
                    foreach (object key in message.Properties.Keys)
                    {
                        MQMessageDictionary.Add(key.ToString(), message.Properties[key.ToString()] == null ? null : message.Properties[key.ToString()].ToString());
                    }
                    if (MQMessageDictionary.Keys.Count == 0)
                    {
                        return;
                    }
                    //0.檢查是否為HeartBeat訊息,若是則忽略不處理
                    if (MQMessageDictionary.ContainsKey("0"))
                    {
                        return;
                    }
                    //1.檢查是否有指定TagType,以便與傳過來的TagData作驗證用
                    if (_DataType == null)
                    {
                        _ErrMsg = "not yet assigned Tag Type of Tag Data";
                        if (log.IsInfoEnabled) log.Info(_ErrMsg);
                        RunOnMQMessageHandleFinished(_ErrMsg, null);
                        return;
                    }
                    _DicTagType = Util.ConvertTagClassConstants(_DataType);
                    //2.驗證MQ傳過來的TagData的tag正確性(與指定的TagType)
                    if(MQMessageDictionary.ContainsKey("__AMQ_CID"))
                    {
                        MQMessageDictionary.Remove("__AMQ_CID");
                    }
                    foreach (string key in MQMessageDictionary.Keys)
                    {
                        if (!_DicTagType.ContainsKey(key))
                        {
                            _ErrMsg = string.Format("Tag Data's Tag[{0}] Not in the assigned type[{1}]", key, _DataType.Name);
                            if (log.IsInfoEnabled) log.Info(_ErrMsg);
                            RunOnMQMessageHandleFinished(_ErrMsg, null);
                            return;
                        }
                    }
                    string MessageID = _DataType.GetField("MessageID") == null ? "710" : _DataType.GetField("MessageID").GetValue(_DataType).ToString();
                    //3.驗證資料內容的Message總筆數
                    string TotalRecords = _DataType.GetField("TotalRecords") == null ? "10038" : _DataType.GetField("TotalRecords").GetValue(_DataType).ToString();
                    if (MQMessageDictionary.ContainsKey(TotalRecords))
                    {
                        //驗證筆數資料正確性
                        //如果筆數不是數值
                        int iTotalRecords;
                        if (!int.TryParse(MQMessageDictionary[TotalRecords].ToString(), out iTotalRecords))
                        {
                            _ErrMsg = "TotalRecords value must be digit";
                            if (log.IsInfoEnabled) log.Info(_ErrMsg);
                            RunOnMQMessageHandleFinished(_ErrMsg, null);
                            return;
                        }
                    }
                    //驗證MessageID是否存在
                    if (!MQMessageDictionary.ContainsKey(MessageID))
                    {
                        _ErrMsg = "MessageID Of Message in MessageBody is not exist";
                        if (log.IsInfoEnabled) log.Info(_ErrMsg);
                        RunOnMQMessageHandleFinished(_ErrMsg, null);
                        return;
                    }
                    //MessageID存在則檢查DicMessageBody內是否存在此MessageID,沒有則建立DataTable Schema並加入一筆MessageBody至DicMessageBody
                    if (!DicMessageBody.ContainsKey(MQMessageDictionary[MessageID].ToString()))
                    {
                        DataTable DT = new DataTable();
                        DT = Util.CreateTableSchema(_DicTagType, _DataType);
                        DicMessageBody.Add(MQMessageDictionary[MessageID].ToString(), new MessageBody(DT, System.DateTime.Now));
                    }
                    //匯入每筆message到屬於此MessageID的MessageBody
                    MessageBody MB = DicMessageBody[MQMessageDictionary[MessageID].ToString()];
                    DataRow MessageRow;
                    MessageRow = Util.AddMessageToRow(MQMessageDictionary, _DicTagType, _DataType, MB.Messages);
                    if (MessageRow != null)
                    {
                        _ErrMsg = "";
                        MB.Messages.Rows.Add(MessageRow);
                        RunOnMQMessageHandleFinished(_ErrMsg, MessageRow);
                    }
                    else
                    {
                        _ErrMsg = "Error happened when generate DataRow";
                        if (log.IsInfoEnabled) log.Info(_ErrMsg);
                        RunOnMQMessageHandleFinished(_ErrMsg, null);
                    }
                    if (DicMessageBody.ContainsKey(MQMessageDictionary[MessageID].ToString()) && MB.Messages.Rows.Count > 0)
                    {
                        int iTotalRecords = Convert.ToInt32(MB.Messages.Rows[0]["TotalRecords"].ToString());
                        //若此MessageID TotalRecords的筆數與在DicMessageBody的Messages筆數相同
                        if (iTotalRecords == DicMessageBody[MQMessageDictionary[MessageID].ToString()].Messages.Rows.Count)
                        {
                            _ErrMsg = "";
                            //DataTable ResultTable = DicMessageBody[MQMessageDictionary[MessageID].ToString()].Messages.Copy();
                            DataTable ResultTable = DicMessageBody[MQMessageDictionary[MessageID].ToString()].Messages;
                            if (ResultTable.Rows.Count > 0 && ResultTable.Columns.Contains("MacAddress") && !ResultTable.Rows[0].IsNull("MacAddress") && this.SendName.IndexOf("#") != -1)
                            {
                                this.ReStartSender(this.SendName.Replace("#", ResultTable.Rows[0]["MacAddress"].ToString()));
                            }
                            if (this.Handler != null)
                            {
                                this.Handler.WorkItemQueue.Enqueue(ResultTable);
                            }
                            _IsBatchFinished = true;
                            RunOnMQBatchFinished(_ErrMsg, ResultTable);
                            ClearGuidInDictionary(MQMessageDictionary[MessageID].ToString());
                            _IsBatchFinished = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 清除逾時的已接收的MQMessage
        /// </summary>
        public void ClearTimeOutMQReceivedMessage()
        {
            int TimeOut = Convert.ToInt32(config.MQReceivedMessageReservedSeconds);
            DateTime SysTime = System.DateTime.Now;
            foreach (string Guid in DicMessageBody.Keys.ToArray())
            {
                if ((SysTime - DicMessageBody[Guid].CreatedTime).Seconds >= TimeOut)
                {
                    MessageBody MB = DicMessageBody[Guid];
                    int iTotalRecords = Convert.ToInt32(MB.Messages.Rows[0]["TotalRecords"].ToString());
                    int BodyCount = MB.Messages.Rows.Count;
                    if (iTotalRecords != BodyCount)
                    {
                        string _ErrMsg = string.Format("Message Body Rows({0}) of Message ID:{1} is not match TotalRecords({2})", BodyCount, Guid, iTotalRecords);
                        if (log.IsInfoEnabled) log.Info(_ErrMsg);
                        OnMQBatchMismatched(new MQBatchMismatchedEventArgs(_ErrMsg));
                    }
                    DicMessageBody.Remove(Guid);
                }
            }
        }
        /// <summary>
        /// 清除Dictionary裏指定的Guid
        /// </summary>
        /// <param name="Guid"></param>
        public void ClearGuidInDictionary(string Guid)
        {
            DicMessageBody.Remove(Guid);
        }

        void BatchMQAdapter_MQBatchMismatched(object sender, MQBatchMismatchedEventArgs e)
        {
            if (log.IsInfoEnabled) log.Info(e.MismatchedMessage);
        }

        private void RunOnMQMessageHandleFinished(string ErrorMessage, DataRow MessageRow)
        {
            if (UISyncContext != null && IsEventInUIThread)
            {
                UISyncContext.Post(OnMQMessageHandleFinished, new MQMessageHandleFinishedEventArgs(ErrorMessage, MessageRow));
            }
            else
            {
                OnMQMessageHandleFinished(new MQMessageHandleFinishedEventArgs(ErrorMessage, MessageRow));
            }
        }

        private void RunOnMQBatchFinished(string ErrorMessage, DataTable BatchResultTable)
        {
            if (UISyncContext != null && IsEventInUIThread)
            {
                UISyncContext.Post(OnMQBatchFinished, new MQBatchFinishedEventArgs(ErrorMessage, BatchResultTable));
            }
            else
            {
                OnMQBatchFinished(new MQBatchFinishedEventArgs(ErrorMessage, BatchResultTable));
            }
        }
    }
}
