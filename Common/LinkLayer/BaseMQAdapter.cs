﻿using Apache.NMS;
using Apache.NMS.ActiveMQ;
//using Apache.NMS.Stomp;
using Apache.NMS.ActiveMQ.Commands;
using Common.HandlerLayer;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace Common.LinkLayer
{
    public enum DestinationFeature
    {
        Topic,
        VirtualTopic,
        MirroredQueues,
        Queue
    }

    public struct MessageField
    {
        public string Name;
        public string Value;
    }

    /// <summary>
    ///收到一筆MQMessage並完成資料處理時的事件參數類別
    /// </summary>
    public class MQMessageHandleFinishedEventArgs : EventArgs
    {
        private string _errorMessage;
        private DataRow _MessageRow;
        public MQMessageHandleFinishedEventArgs(string errorMessage, DataRow MessageRow)
        {
            _errorMessage = errorMessage;
            _MessageRow = MessageRow;
        }
        public string errorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        public DataRow MessageRow
        {
            get { return _MessageRow; }
            set { _MessageRow = value; }
        }
    }

    /// <summary>
    ///非同步發送MQMessage完成時的事件參數類別
    /// </summary>
    public class MQMessageAsynSendFinishedEventArgs : EventArgs
    {
        private string _errorMessage;
        public MQMessageAsynSendFinishedEventArgs(string errorMessage)
        {
            _errorMessage = errorMessage;
        }
        public string errorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
    [Serializable]

    public abstract class BaseMQAdapter : Common.LinkLayer.IMQAdapter
    {
        protected readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string _Uri = string.Empty;
        protected string _ListenName = string.Empty;
        protected string _SendName = string.Empty;
        protected string _UserName = string.Empty;
        protected string _PassWord = string.Empty;
        protected string _MacAddress = string.Empty;
        protected bool _UseSharedConnection = true;
        protected DBAction _SenderDBAction = DBAction.None;
        protected DBAction _ReceiverDBAction = DBAction.None;
        protected string _MirroredQueuePrefix = "MIRROR.";
        protected string _VirtualTopic = "VTCON.*.";
        protected DestinationFeature _DestinationFeature = DestinationFeature.Topic;

        //使用_UseSharedConnection=true共享連線時下面的_Factory和_Connection會一直是null
        protected IConnectionFactory _Factory = null;
        protected IConnection _Connection = null;
        protected ISession _Session = null;
        protected IMessageConsumer _Consumer = null;
        protected int _VirtualTopicConsumers = 1;  //當_DestinationFeature=DestinationFeature.Virtual_Topic則採用Virtual Topics 機制(Queue+Topic),多個Consumer的預設數量
        protected List<IMessageConsumer> _ListVirtualTopicConsumer = new List<IMessageConsumer>();

        protected IMessageProducer _Producer = null;

        protected string _MessageID;

        protected SynchronizationContext _UISyncContext;

        protected TopicTypeHandler _Handler;

        protected bool _IsEventInUIThread = false;             //觸發事件時是否回到UI Thread預設為false
        protected bool _UseSSL = false;

        protected Timer HeartBeatTimer;
        protected int _HeartBeatInterval = 60;
        protected int _sendAmounnts = 0;
        protected double _MessageTimeOut = 0;
        protected string _Selector = "";
        protected bool _IsDurableConsumer = false;
        protected string _ClientID = "";

        public delegate void MQMessageHandleFinishedEventHandler(object sender, MQMessageHandleFinishedEventArgs e);
        List<MQMessageHandleFinishedEventHandler> MQMessageHandleFinishedEventDelegates = new List<MQMessageHandleFinishedEventHandler>();
        private event MQMessageHandleFinishedEventHandler _MQMessageHandleFinished;
        public event MQMessageHandleFinishedEventHandler MQMessageHandleFinished
        {
            add
            {
                _MQMessageHandleFinished += value;
                MQMessageHandleFinishedEventDelegates.Add(value);
            }
            remove
            {
                _MQMessageHandleFinished -= value;
                MQMessageHandleFinishedEventDelegates.Remove(value);
            }
        }

        public delegate void MQMessageAsynSendFinishedEventHandler(object sender, MQMessageAsynSendFinishedEventArgs e);
        List<MQMessageAsynSendFinishedEventHandler> MQMessageAsynSendFinishedEventDelegates = new List<MQMessageAsynSendFinishedEventHandler>();
        private event MQMessageAsynSendFinishedEventHandler _MQMessageAsynSendFinished;
        public event MQMessageAsynSendFinishedEventHandler MQMessageAsynSendFinished
        {
            add
            {
                _MQMessageAsynSendFinished += value;
                MQMessageAsynSendFinishedEventDelegates.Add(value);
            }
            remove
            {
                _MQMessageAsynSendFinished -= value;
                MQMessageAsynSendFinishedEventDelegates.Remove(value);
            }
        }

        /// <summary>
        /// 收到一筆MQMessage並完成資料處理時事件
        /// </summary>
        protected virtual void OnMQMessageHandleFinished(object state)
        {
            MQMessageHandleFinishedEventArgs e = state as MQMessageHandleFinishedEventArgs;
            if (_MQMessageHandleFinished != null)
            {
                _MQMessageHandleFinished(this, e);
            }
        }
        /// <summary>
        /// 非同步發送Message完成時事件
        /// </summary>
        protected virtual void OnMQMessageSendFinished(object state)
        {
            MQMessageAsynSendFinishedEventArgs e = state as MQMessageAsynSendFinishedEventArgs;
            if (_MQMessageAsynSendFinished != null)
            {
                _MQMessageAsynSendFinished(this, e);
            }
        }

        public string Uri
        {
            set { _Uri = value; }
            get { return _Uri; }
        }

        public DestinationFeature DestinationFeature
        {
            set { _DestinationFeature = value; }
            get { return _DestinationFeature; }
        }

        public string ListenName
        {
            set { _ListenName = value; }
            get { return _ListenName; }
        }

        public string SendName
        {
            set { _SendName = value; }
            get { return _SendName; }
        }


        public string UserName
        {
            set { _UserName = value; }
        }

        public string PassWord
        {
            set { _PassWord = value; }
        }
        public string MacAddress
        {
            set { _MacAddress = value; }
            get { return _MacAddress; }
        }
        public int CurrentSendAmounts
        {
            get { return _sendAmounnts; }
        }
        public bool UseSharedConnection
        {
            set { _UseSharedConnection = value; }
            get { return _UseSharedConnection; }
        }
        public DBAction SenderDBAction
        {
            set { _SenderDBAction = value; }
            get { return _SenderDBAction; }
        }
        public DBAction ReceiverDBAction
        {
            set { _ReceiverDBAction = value; }
            get { return _ReceiverDBAction; }
        }
        public string MirroredQueuePrefix
        {
            set { _MirroredQueuePrefix = value; }
            get { return _MirroredQueuePrefix; }
        }

        public string VirtualTopic
        {
            set { _VirtualTopic = value; }
            get { return _VirtualTopic; }
        }

        public int VirtualTopicConsumers
        {
            set { _VirtualTopicConsumers = value; }
            get { return _VirtualTopicConsumers; }
        }
        public string MessageID
        {
            get { return _MessageID; }
            set
            {
                _MessageID = value;
            }
        }
        /// <summary>
        /// 觸發事件時是否回到UI Thread(預設false)
        /// </summary>
        public bool IsEventInUIThread
        {
            get { return _IsEventInUIThread; }
            set { _IsEventInUIThread = value; }
        }
        public bool IsDurableConsumer
        {
            get { return _IsDurableConsumer; }
            set { _IsDurableConsumer = value; }
        }
        public bool UseSSL
        {
            get { return _UseSSL; }
            set { _UseSSL = value; }
        }

        /// <summary>
        /// 心跳訊息間隔(秒)
        /// </summary>
        public int HeartBeatInterval
        {
            set { _HeartBeatInterval = value; }
            get { return _HeartBeatInterval; }
        }
        public double MessageTimeOut
        {
            set { _MessageTimeOut = value; }
            get { return _MessageTimeOut; }
        }
        public string Selector
        {
            get { return _Selector; }
            set
            {
                _Selector = value;
            }
        }       
        public SynchronizationContext UISyncContext
        {
            get { return _UISyncContext; }
        }
        public TopicTypeHandler Handler
        {
            get { return _Handler; }
            set { _Handler = value; }
        }

        public BaseMQAdapter()
        {
        }

        public BaseMQAdapter(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName)
        {
            _Uri = Uri;
            _DestinationFeature = DestinationFeature;
            _ListenName = ListenName;
            _SendName = SendName;
        }
        public BaseMQAdapter(string Uri, DestinationFeature DestinationFeature, string ListenName, string SendName, string UserName, string Pwd)
        {
            _Uri = Uri;
            _DestinationFeature = DestinationFeature;
            _ListenName = ListenName;
            _SendName = SendName;
            _UserName = UserName;
            _PassWord = Pwd;
        }

        public bool CheckActiveMQAlive()
        {
            string urls;
            string ports;
            string url = "";
            urls = _Uri.Split(new char[] { ':' })[0];
            ports = _Uri.Split(new char[] { ':' })[1];
            //代表url只有1個IP
            if (urls.IndexOf(",") == -1)
            {
                bool result = false;
                //代表只有1個port
                if (ports.IndexOf(",") == -1)
                {
                    url = "tcp://" + urls + ":" + ports;
                    _Factory = new ConnectionFactory(url);
                    try
                    {
                        if (_UserName != "" && _PassWord != "")
                        {
                            _Connection = _Factory.CreateConnection(_UserName, _PassWord);
                        }
                        else
                        {
                            _Connection = _Factory.CreateConnection();
                        }
                        result = true;
                    }
                    catch (NMSException ex)
                    {
                        if (log.IsErrorEnabled) log.Error("IsActiveMQAlive() Error", ex);
                        result = false;
                    }
                }
                //代表多個port
                else
                {
                    foreach( string port in ports.Split(new char[] { ',' }))
                    {
                        url = "tcp://" + urls + ":" + port;
                        _Factory = new ConnectionFactory(url);
                        try
                        {
                            if (_UserName != "" && _PassWord != "")
                            {
                                _Connection = _Factory.CreateConnection(_UserName, _PassWord);
                            }
                            else
                            {
                                _Connection = _Factory.CreateConnection();
                            }
                            result = true;
                            break;
                        }
                        catch (NMSException ex)
                        {
                            if (log.IsErrorEnabled) log.Error("IsActiveMQAlive() Error", ex);
                            continue;
                        }
                    }
                }
                return result;
            }
            //代表url有多個IP
            else
            {
                bool result = false;
                List<string> lstUrl = urls.Split(new char[] { ',' }).ToList<string>();
                int i = 0;
                foreach (string _url in lstUrl)
                {
                    //代表只有1個port
                    if (ports.IndexOf(",") == -1)
                    {
                        url = "tcp://" + _url + ":" + ports;
                    }
                    //代表多個port
                    else
                    {
                        string[] lstPort = ports.Split(new char[] { ',' });
                        url = "tcp://" + _url + ":" + ports.Split(new char[] { ',' })[i];
                        i++;
                    }
                    _Factory = new ConnectionFactory(url);
                    try
                    {
                        if (_UserName != "" && _PassWord != "")
                        {
                            _Connection = _Factory.CreateConnection(_UserName, _PassWord);
                        }
                        else
                        {
                            _Connection = _Factory.CreateConnection();
                        }
                        result = true;
                        break;
                    }
                    catch (NMSException ex)
                    {
                        if (log.IsErrorEnabled) log.Error("IsActiveMQAlive() Error", ex);
                        continue;
                    }
                }
                return result;
            }
        }
        public void Start(string ClientID = "", bool IsDurableConsumer = false)
        {
            string SingleUrl = "";
            string Urls = "";
            _IsDurableConsumer = IsDurableConsumer;
            _ClientID = ClientID;
            string urls;
            string ports;
            urls = _Uri.Split(new char[] { ':' })[0];
            ports = _Uri.Split(new char[] { ':' })[1];
            //代表只有1個IP
            if (urls.IndexOf(",") == -1)
            {
                SingleUrl = urls;
            }
            //代表多個IP
            else
            {
                SingleUrl = urls.Split(new char[] { ',' })[0];
                Urls = urls;
            }
            if (!SingleUrl.Equals("") && SingleUrl.IndexOf(":") > -1)
            {
                string ip = SingleUrl.Substring(0, SingleUrl.IndexOf(":"));
                _MacAddress = Util.GetMacAddress();
            }
            // Example connection strings:
            //failover:tcp://localhost:61616?initialReconnectDelay=2000&maxReconnectAttempts=2
            try
            {
                if (_UseSharedConnection)
                {
                    if (Urls.Equals(""))
                    {
                        AMQSharedConnection.Open(SingleUrl, ports, _UserName, _PassWord, _UseSSL, _IsDurableConsumer, _ClientID);
                    }
                    else
                    {
                        AMQSharedConnection.Open(Urls, ports, _UserName, _PassWord, _UseSSL, _IsDurableConsumer, _ClientID);
                    }
                    _Session = AMQSharedConnection.GetConnection().CreateSession(AcknowledgementMode.AutoAcknowledge);
                    _Connection = AMQSharedConnection.GetConnection();
                    //_Connection.ConnectionResumedListener += _Connection_ConnectionResumedListener;
                    //_Connection.ExceptionListener += _Connection_ExceptionListener;
                    StartListener();
                    StartSender();
                    _UISyncContext = SynchronizationContext.Current;
                    //InitialHeartBeat();
                }
                else
                {
                    //if (_Connection == null)
                    //{
                    if (Urls.Equals(""))
                    {
                        _Factory = new ConnectionFactory(Util.GetMQFailOverConnString(SingleUrl, ports, _UseSSL));
                    }
                    else
                    {
                        _Factory = new ConnectionFactory(Util.GetMQFailOverConnString(Urls, ports, _UseSSL));
                    }
                    try
                    {
                        if (_UserName != "" && _PassWord != "")
                        {
                            _Connection = _Factory.CreateConnection(_UserName, _PassWord);
                        }
                        else
                        {
                            _Connection = _Factory.CreateConnection();
                        }
                        //_Connection.ConnectionResumedListener += _Connection_ConnectionResumedListener;
                        //_Connection.ExceptionListener += _Connection_ExceptionListener;
                    }
                    catch (Apache.NMS.NMSConnectionException ex)
                    {
                        if (log.IsErrorEnabled) log.Error("BaseMQAdapter Start() Error", ex);
                        throw ex;
                    }
                    try
                    {
                        if (IsDurableConsumer && !string.IsNullOrEmpty(ClientID)) _Connection.ClientId = ClientID;
                        _Connection.Start();
                    }
                    catch (BrokerException ex)
                    {
                        if (log.IsErrorEnabled) log.Error("BaseMQAdapter Start() Error", ex);
                        throw ex;
                    }
                    _Session = _Connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                    StartListener();
                    StartSender();
                    _UISyncContext = SynchronizationContext.Current;
                    //InitialHeartBeat();
                    //}
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error("BaseMQAdapter Start() Error", ex);
                throw ex;
            }
        }

        public void Close()
        {
            try
            {
                if (_Session != null)
                {
                    _Producer = null;
                    _Consumer = null;
                    _Session.Close();
                    _Session = null;
                }
                if (!_UseSharedConnection)
                {
                    if (_Connection != null)
                    {
                        if (_Connection.IsStarted)
                        {
                            _Connection.Stop();
                            _Connection.Close();
                            _Connection = null;
                        }
                    }
                }
                EndHeartBeat();
            }
            catch (ConnectionClosedException ex)
            {
                if (log.IsErrorEnabled) log.Error("BaseMQAdapter Close() Error", ex);
                throw ex;
            }
        }

        public void Restart(string ClientID = "", bool IsDurableConsumer = false)
        {
            try
            {
                Close();
                Start(ClientID, IsDurableConsumer);
                //InitialHeartBeat();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void RemoveAllEvents()
        {
            foreach (MQMessageHandleFinishedEventHandler eh in MQMessageHandleFinishedEventDelegates)
            {
                _MQMessageHandleFinished -= eh;
            }
            MQMessageHandleFinishedEventDelegates.Clear();
            foreach (MQMessageAsynSendFinishedEventHandler eh in MQMessageAsynSendFinishedEventDelegates)
            {
                _MQMessageAsynSendFinished -= eh;
            }
            MQMessageAsynSendFinishedEventDelegates.Clear();
        }

        public void listener_messageReceivedEventHandler(IMessage message)
        {
            if (message.Properties.Count < 1)
            {
                if (log.IsWarnEnabled) log.Warn("MQMessage Format is InCorrect");
            }
            else
            {
                processMQMessage(message);
            }
        }

        public abstract void processMQMessage(IMessage message);

        public bool SendMQMessage(string MessageIDTag, List<MessageField> SingleMqMessage, int DelayedPerWhenNumber = 0, int DelayedMillisecond = 0)
        {
            bool isSend = false;
            string ErrorMsg = "";
            try
            {
                this._MessageID = System.Guid.NewGuid().ToString();
                //註解發送筆數資訊
                //SendCountMessage(MessageIDTag, _MessageID, 1);
                if (_Producer != null)
                {
                    IMessage msg = _Producer.CreateMessage();
                    //if (_DestinationFeature == DestinationFeature.Queue)
                    //{
                    //    ITemporaryQueue replyDest = _Session.CreateTemporaryQueue();
                    //    msg.NMSCorrelationID = this._MessageID;
                    //    msg.NMSReplyTo = replyDest;
                    //}
                    msg.Properties.SetString(MessageIDTag, this._MessageID);
                    //MacAddress(99)
                    if (!string.IsNullOrEmpty(_MacAddress))
                    {
                        msg.Properties.SetString("99", _MacAddress);
                    }
                    //加入總筆數tag
                    msg.Properties.SetString("10038", "1");
                    foreach (MessageField prop in SingleMqMessage)
                    {
                        msg.Properties.SetString(prop.Name, prop.Value);
                    }
                    if ((_Session as Apache.NMS.ActiveMQ.Session).Started)
                    {
                        TimeSpan TS = _MessageTimeOut == 0 ? TimeSpan.Zero : TimeSpan.FromDays(_MessageTimeOut);
                        _Producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                        isSend = true;
                        _sendAmounnts += 1;
                        if (DelayedPerWhenNumber > 0 && DelayedMillisecond > 0)
                        {
                            SlowDownProducer(DelayedPerWhenNumber, DelayedMillisecond);
                        }
                    }
                    else
                    {
                        //throw new Exception("Network connection or ActiveMQService Has been closed!");
                        if (log.IsInfoEnabled) log.Info("Network connection or ActiveMQService Has been closed!");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "BaseMQAdapter SendMQMessage() Error(" + ex.Message + ")";
                if (log.IsErrorEnabled) log.Error(ErrorMsg, ex);
                System.Environment.Exit(-1);
            }
            finally
            {
                if (_UISyncContext != null && IsEventInUIThread)
                {
                    _UISyncContext.Post(OnMQMessageSendFinished, new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
                else
                {
                    OnMQMessageSendFinished(new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
            }
            return isSend;
        }

        public bool SendMQMessage(string MessageIDTag, List<List<MessageField>> MultiMqMessage, int DelayedPerWhenNumber = 0, int DelayedMillisecond = 0)
        {
            bool isSend = false;
            string ErrorMsg = "";
            try
            {
                this._MessageID = System.Guid.NewGuid().ToString();
                //註解發送筆數資訊
                //SendCountMessage(MessageIDTag, _MessageID, MultiMqMessage.Count);
                if (_Producer != null)
                {
                    foreach (List<MessageField> SingleMqMessage in MultiMqMessage)
                    {
                        IMessage msg = _Producer.CreateMessage();
                        //if (_DestinationFeature == DestinationFeature.Queue)
                        //{
                        //    ITemporaryQueue replyDest = _Session.CreateTemporaryQueue();
                        //    msg.NMSCorrelationID = this._MessageID;
                        //    msg.NMSReplyTo = replyDest;
                        //}
                        msg.Properties.SetString(MessageIDTag, this._MessageID);
                        //MacAddress(99)
                        if (!string.IsNullOrEmpty(_MacAddress))
                        {
                            msg.Properties.SetString("99", _MacAddress);
                        }
                        //加入總筆數tag
                        msg.Properties.SetString("10038", MultiMqMessage.Count().ToString());
                        foreach (MessageField prop in SingleMqMessage)
                        {
                            msg.Properties.SetString(prop.Name, prop.Value);
                        }
                        if ((_Session as Apache.NMS.ActiveMQ.Session).Started)
                        {
                            TimeSpan TS = _MessageTimeOut == 0 ? TimeSpan.Zero : TimeSpan.FromDays(_MessageTimeOut);
                            _Producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                            isSend = true;
                            _sendAmounnts += 1;
                            if (DelayedPerWhenNumber > 0 && DelayedMillisecond > 0)
                            {
                                SlowDownProducer(DelayedPerWhenNumber, DelayedMillisecond);
                            }
                        }
                        else
                        {
                            //throw new Exception("Network connection or ActiveMQService Has been closed!");
                            if (log.IsInfoEnabled) log.Info("Network connection or ActiveMQService Has been closed!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "SendMQMessage: Error(" + ex.Message + ")";
                if (log.IsErrorEnabled) log.Error(ErrorMsg, ex);
                System.Environment.Exit(-1);
            }
            finally
            {
                if (_UISyncContext != null && IsEventInUIThread)
                {
                    _UISyncContext.Post(OnMQMessageSendFinished, new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
                else
                {
                    OnMQMessageSendFinished(new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
            }
            return isSend;
        }

        public void SendAsynMQMessage(string MessageIDTag, List<List<MessageField>> MultiMqMessage, int DelayedPerWhenNumber = 0, int DelayedMillisecond = 0)
        {
            ThreadStart SendThreadStart = new ThreadStart(
                delegate ()
                {
                    lock (this)
                    {
                        this._MessageID = System.Guid.NewGuid().ToString();
                        //註解發送筆數資訊
                        //SendCountMessage(MessageIDTag, _MessageID, MultiMqMessage.Count);
                        SendAsyn(_Producer, MessageIDTag, MultiMqMessage, DelayedPerWhenNumber, DelayedMillisecond);
                    }
                });
            Thread SendThread = new Thread(SendThreadStart);
            SendThread.Start();
        }
        public bool SendFile(string FileName, string FilePath , string ID = "")
        {
            bool isSend = false;
            string ErrorMsg = "";
            try
            {
                if (_Producer != null)
                {
                    var bytes = default(byte[]);
                    using (StreamReader sr = new StreamReader(FilePath))
                    {
                        using (var memstream = new MemoryStream())
                        {
                            var buffer = new byte[512];
                            var bytesRead = default(int);
                            while ((bytesRead = sr.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                memstream.Write(buffer, 0, bytesRead);
                            bytes = memstream.ToArray();
                        }
                    }

                    IBytesMessage msg = _Producer.CreateBytesMessage(bytes);
                    msg.Properties.SetString("id", ID);
                    msg.Properties.SetString("filename", FileName);

                    //IStreamMessage msg1 = _Producer.CreateStreamMessage();
                    //msg1.Properties.SetString("id", ID);
                    //msg1.Properties.SetString("filename", FileName);
                    //msg1.WriteBytes(bytes);

                    if ((_Session as Apache.NMS.ActiveMQ.Session).Started)
                    {
                        TimeSpan TS = _MessageTimeOut == 0 ? TimeSpan.Zero : TimeSpan.FromDays(_MessageTimeOut);
                        _Producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                        //_Producer.Send(msg1, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                        isSend = true;
                    }
                    else
                    {
                        if (log.IsInfoEnabled) log.Info("Network connection or ActiveMQService Has been closed!");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "BaseMQAdapter SendFile: Error(" + ex.Message + ")";
                if (log.IsErrorEnabled) log.Error(ErrorMsg, ex);
                System.Environment.Exit(-1);
            }
            finally
            {
                if (_UISyncContext != null && IsEventInUIThread)
                {
                    _UISyncContext.Post(OnMQMessageSendFinished, new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
                else
                {
                    OnMQMessageSendFinished(new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
            }
            return isSend;
        }
        public bool SendFile(string FileName, byte[] FileBytes, string ID = "")
        {
            bool isSend = false;
            string ErrorMsg = "";
            try
            {
                if (_Producer != null)
                {
                    IBytesMessage msg = _Producer.CreateBytesMessage(FileBytes);
                    msg.Properties.SetString("id", ID);
                    msg.Properties.SetString("filename", FileName);

                    //IStreamMessage msg1 = _Producer.CreateStreamMessage();
                    //msg1.Properties.SetString("id", ID);
                    //msg1.Properties.SetString("filename", FileName);
                    //msg1.Properties.SetString("length", FileBytes.Length.ToString());
                    //msg1.WriteBytes(FileBytes);

                    if ((_Session as Apache.NMS.ActiveMQ.Session).Started)
                    {
                        TimeSpan TS = _MessageTimeOut == 0 ? TimeSpan.Zero : TimeSpan.FromDays(_MessageTimeOut);
                        _Producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                        //_Producer.Send(msg1, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                        isSend = true;
                    }
                    else
                    {
                        if (log.IsInfoEnabled) log.Info("Network connection or ActiveMQService Has been closed!");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "BaseMQAdapter SendFile: Error(" + ex.Message + ")";
                if (log.IsErrorEnabled) log.Error(ErrorMsg, ex);
                System.Environment.Exit(-1);
            }
            finally
            {
                if (_UISyncContext != null && IsEventInUIThread)
                {
                    _UISyncContext.Post(OnMQMessageSendFinished, new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
                else
                {
                    OnMQMessageSendFinished(new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
            }
            return isSend;
        }
        public void ReStartListener(string ListenName)
        {
            try
            {
                if (ListenName != "" && _Session != null)
                {
                    if (_Consumer != null)
                    {
                        _Consumer.Close();
                    }
                    if (_DestinationFeature == DestinationFeature.Topic)
                    {
                        if (_Selector.Equals(""))
                        {
                            if (_IsDurableConsumer)
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_ListenName), _Connection.ClientId, null, false);
                            }
                            else
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(ListenName));
                            }
                        }
                        else
                        {
                            if (_IsDurableConsumer)
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_ListenName), _Connection.ClientId, _Selector, false);
                            }
                            else
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(ListenName), _Selector);
                            }
                        }
                        _Consumer.Listener += new MessageListener(listener_messageReceivedEventHandler);
                    }
                    else if (_DestinationFeature == DestinationFeature.VirtualTopic)
                    {
                        IMessageConsumer[] QueueConsumer = new IMessageConsumer[_VirtualTopicConsumers];
                        for (int i = 0; i < _VirtualTopicConsumers; i++)
                        {
                            if (_Selector.Equals(""))
                            {
                                //QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", Util.GetMacAddress()) + _ListenName));
                                QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", _ClientID) + _ListenName));
                            }
                            else
                            {
                                //QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", Util.GetMacAddress()) + _ListenName), _Selector);
                                QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", _ClientID) + _ListenName), _Selector);
                            }
                            QueueConsumer[i].Listener += new MessageListener(listener_messageReceivedEventHandler);
                            _ListVirtualTopicConsumer.Add(QueueConsumer[i]);
                        }
                    }
                    else if (_DestinationFeature == DestinationFeature.MirroredQueues)
                    {
                        if (_IsDurableConsumer)
                        {
                            if (_Selector.Equals(""))
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_MirroredQueuePrefix + _ListenName), _Connection.ClientId, null, false);
                            }
                            else
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_MirroredQueuePrefix + _ListenName), _Connection.ClientId, null, false);
                            }
                        }
                        else
                        {
                            if (_Selector.Equals(""))
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(_MirroredQueuePrefix + ListenName));
                            }
                            else
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(_MirroredQueuePrefix + ListenName), _Selector);
                            }
                        }
                        _Consumer.Listener += new MessageListener(listener_messageReceivedEventHandler);
                    }
                    else if (_DestinationFeature == DestinationFeature.Queue)
                    {
                        if (_Selector.Equals(""))
                        {
                            _Consumer = _Session.CreateConsumer(new ActiveMQQueue(ListenName));
                        }
                        else
                        {
                            _Consumer = _Session.CreateConsumer(new ActiveMQQueue(ListenName), _Selector);
                        }
                        _Consumer.Listener += new MessageListener(listener_messageReceivedEventHandler);
                    }
                }
            }
            catch (Exception exception)
            {
                if (log.IsErrorEnabled) log.Error("BaseMQAdapter ReStartListener() Error", exception);
            }
        }

        public void ReStartSender(string SendName)
        {
            try
            {
                if (SendName != "" && _Session != null)
                {
                    //if (_Producer != null)
                    //{
                    //    _Producer.Close();
                    //}
                    if (_DestinationFeature == DestinationFeature.Topic || _DestinationFeature == DestinationFeature.VirtualTopic)
                    {
                        _Producer = _Session.CreateProducer(new ActiveMQTopic(SendName));
                        //_Producer = _Session.CreateProducer(new Topic(SendName));
                        _Producer.DisableMessageID = true;
                        _Producer.DisableMessageTimestamp = true;
                    }
                     else if (_DestinationFeature == DestinationFeature.Queue || _DestinationFeature == DestinationFeature.MirroredQueues)
                    {
                        _Producer = _Session.CreateProducer(new ActiveMQQueue(SendName));
                        _Producer.DisableMessageID = true;
                        _Producer.DisableMessageTimestamp = true;
                    }
                }
            }
            catch (Exception exception)
            {
                if (log.IsErrorEnabled) log.Error("BaseMQAdapter ReStartSender() Error", exception);
            }
        }

        public void CloseSharedConnection()
        {
            try
            {
                if (_UseSharedConnection)
                {
                    AMQSharedConnection.Close();
                }
            }
            catch (Exception exception)
            {
                if (log.IsErrorEnabled) log.Error("BaseMQAdapter CloseSharedConnection() Error", exception);
            }
        }

        protected void SendAsyn(IMessageProducer Producer, string MessageIDTag, List<List<MessageField>> MultiMqMessage, int DelayedPerWhenNumber = 0, int DelayedMillisecond = 0)
        {
            string ErrorMsg = "";
            try
            {
                if (_Producer != null)
                {
                    foreach (List<MessageField> SingleMqMessage in MultiMqMessage)
                    {
                        IMessage msg = Producer.CreateMessage();
                        //if (_DestinationFeature == DestinationFeature.Queue)
                        //{
                        //    ITemporaryQueue replyDest = _Session.CreateTemporaryQueue();
                        //    msg.NMSCorrelationID = this._MessageID;
                        //    msg.NMSReplyTo = replyDest;
                        //}
                        msg.Properties.SetString(MessageIDTag, this._MessageID);
                        //MacAddress(99)
                        if (!string.IsNullOrEmpty(_MacAddress))
                        {
                            msg.Properties.SetString("99", _MacAddress);
                        }
                        //加入總筆數tag
                        msg.Properties.SetString("10038", MultiMqMessage.Count().ToString());
                        foreach (MessageField prop in SingleMqMessage)
                        {
                            msg.Properties.SetString(prop.Name, prop.Value);
                        }
                        if ((_Session as Apache.NMS.ActiveMQ.Session).Started)
                        {
                            TimeSpan TS = _MessageTimeOut == 0 ? TimeSpan.Zero : TimeSpan.FromDays(_MessageTimeOut);
                            _Producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                            _sendAmounnts += 1;
                            if (DelayedPerWhenNumber > 0 && DelayedMillisecond > 0)
                            {
                                SlowDownProducer(DelayedPerWhenNumber, DelayedMillisecond);
                            }
                        }
                        else
                        {
                            //throw new Exception("Network connection or ActiveMQService Has been closed!");
                            if (log.IsInfoEnabled) log.Info("Network connection or ActiveMQService Has been closed!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "BaseMQAdapter SendAsyn() Error(" + ex.Message + ")";
                if (log.IsErrorEnabled) log.Error(ErrorMsg, ex);
                System.Environment.Exit(-1);
            }
            finally
            {
                if (_UISyncContext != null && IsEventInUIThread)
                {
                    _UISyncContext.Post(OnMQMessageSendFinished, new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
                else
                {
                    OnMQMessageSendFinished(new MQMessageAsynSendFinishedEventArgs(ErrorMsg));
                }
            }
        }
        /// <summary>
        /// 發送筆數的Message
        /// </summary>
        /// <param name="MessageIDTag"></param>
        /// <param name="MessageID"></param>
        /// <param name="MessageCount"></param>
        private bool SendCountMessage(string MessageIDTag, string MessageID, int MessageCount)
        {
            bool isSend = false;
            string ErrorMsg = "";
            try
            {
                if (_Producer != null)
                {
                    IMessage msg = _Producer.CreateMessage();
                    //if (_DestinationFeature == DestinationFeature.Queue)
                    //{
                    //    ITemporaryQueue replyDest = _Session.CreateTemporaryQueue();
                    //    msg.NMSCorrelationID = this._MessageID;
                    //    msg.NMSReplyTo = replyDest;
                    //}
                    List<MessageField> MessageCountRow = new List<MessageField>();
                    MessageField MessageMessageIDField = new MessageField();
                    MessageMessageIDField.Name = MessageIDTag;
                    MessageMessageIDField.Value = MessageID;
                    MessageCountRow.Add(MessageMessageIDField);
                    MessageField MessageCountRowField = new MessageField();
                    MessageCountRowField.Name = "10038";
                    MessageCountRowField.Value = MessageCount.ToString();
                    MessageCountRow.Add(MessageCountRowField);
                    foreach (MessageField prop in MessageCountRow)
                    {
                        msg.Properties.SetString(prop.Name, prop.Value);
                    }
                    //MacAddress(99)
                    if (!string.IsNullOrEmpty(_MacAddress))
                    {
                        msg.Properties.SetString("99", _MacAddress);
                    }
                    if ((_Session as Apache.NMS.ActiveMQ.Session).Started)
                    {
                        TimeSpan TS = _MessageTimeOut == 0 ? TimeSpan.Zero : TimeSpan.FromDays(_MessageTimeOut);
                        _Producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                        isSend = true;
                    }
                    else
                    {
                        //throw new Exception("Network connection or ActiveMQService Has been closed!");
                        if (log.IsInfoEnabled) log.Info("Network connection or ActiveMQService Has been closed!");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "BaseMQAdapter SendCountMessage: Error happened(" + ex.Message + ")";
                if (log.IsErrorEnabled) log.Error(ErrorMsg, ex);
                System.Environment.Exit(-1);
            }
            return isSend;
        }

        private void StartListener()
        {
            try
            {
                if (_ListenName != "" && _Session != null)
                {
                    if (_DestinationFeature == DestinationFeature.Topic)
                    {
                        if (_Selector.Equals(""))
                        {
                            if (_IsDurableConsumer)
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_ListenName), _Connection.ClientId, null, false);
                            }
                            else
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(_ListenName));
                            }
                        }
                        else
                        {
                            if (_IsDurableConsumer)
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_ListenName), _Connection.ClientId, _Selector, false);
                            }
                            else
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(_ListenName), _Selector);
                            }
                        }
                        _Consumer.Listener += new MessageListener(listener_messageReceivedEventHandler);
                    }
                    else if (_DestinationFeature == DestinationFeature.VirtualTopic)
                    {
                        IMessageConsumer[] QueueConsumer = new IMessageConsumer[_VirtualTopicConsumers];
                        for (int i = 0; i < _VirtualTopicConsumers; i++)
                        {
                            if (_Selector.Equals(""))
                            {
                                //QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", Util.GetMacAddress()) + _ListenName));
                                QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", _ClientID) + _ListenName));
                            }
                            else
                            {
                                // QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", Util.GetMacAddress()) + _ListenName), _Selector);
                                QueueConsumer[i] = _Session.CreateConsumer(new ActiveMQQueue(_VirtualTopic.Replace("*", _ClientID) + _ListenName), _Selector);
                            }
                            QueueConsumer[i].Listener += new MessageListener(listener_messageReceivedEventHandler);
                            _ListVirtualTopicConsumer.Add(QueueConsumer[i]);
                        }
                    }
                    else if (_DestinationFeature == DestinationFeature.MirroredQueues)
                    {
                        if (_IsDurableConsumer)
                        {
                            if (_Selector.Equals(""))
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_MirroredQueuePrefix + _ListenName), _Connection.ClientId, null, false);
                            }
                            else
                            {
                                _Consumer = _Session.CreateDurableConsumer(new ActiveMQTopic(_MirroredQueuePrefix + _ListenName), _Connection.ClientId, null, false);
                            }
                        }
                        else
                        {
                            if (_Selector.Equals(""))
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(_MirroredQueuePrefix + _ListenName));
                            }
                            else
                            {
                                _Consumer = _Session.CreateConsumer(new ActiveMQTopic(_MirroredQueuePrefix + _ListenName), _Selector);
                            }
                        }
                        _Consumer.Listener += new MessageListener(listener_messageReceivedEventHandler);
                    }
                    else if (_DestinationFeature == DestinationFeature.Queue)
                    {
                        if (_Selector.Equals(""))
                        {
                            _Consumer = _Session.CreateConsumer(new ActiveMQQueue(_ListenName));
                        }
                        else
                        {
                            _Consumer = _Session.CreateConsumer(new ActiveMQQueue(_ListenName), _Selector);
                        }
                        _Consumer.Listener += new MessageListener(listener_messageReceivedEventHandler);
                    }
                }
            }
            catch (Exception exception)
            {
                if (log.IsErrorEnabled) log.Error("BaseMQAdapter StartListener() Error", exception);
            }
        }

        private void StartSender()
        {
            try
            {
                if (_SendName != "" && _Session != null)
                {
                    if (_DestinationFeature == DestinationFeature.Topic || _DestinationFeature == DestinationFeature.VirtualTopic)
                    {
                        _Producer = _Session.CreateProducer(new ActiveMQTopic(_SendName));
                        //_Producer = _Session.CreateProducer(new Topic(_SendName));
                        _Producer.DisableMessageID = true;
                        _Producer.DisableMessageTimestamp = true;
                    }
                    else if (_DestinationFeature == DestinationFeature.Queue || _DestinationFeature == DestinationFeature.MirroredQueues)
                    {
                        _Producer = _Session.CreateProducer(new ActiveMQQueue(_SendName));
                        _Producer.DisableMessageID = true;
                        _Producer.DisableMessageTimestamp = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error("BaseMQAdapter StartSender() Error", ex);
            }
        }

        private void InitialHeartBeat()
        {
            try
            {
                TimerCallback TCB = new TimerCallback(state => { SetHeartBeat(); });
                HeartBeatTimer = new Timer(TCB, DateTime.Now, 0, 1000 * _HeartBeatInterval);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EndHeartBeat()
        {
            HeartBeatTimer = null;
        }

        private void SetHeartBeat()
        {
            string ErrorMsg = "";
            try
            {
                if (_Producer != null)
                {
                    IMessage msg = _Producer.CreateMessage();
                    //MacAddress(99)
                    if (!string.IsNullOrEmpty(_MacAddress))
                    {
                        msg.Properties.SetString("99", _MacAddress);
                    }
                    msg.Properties.SetString("0", "HeartBeat");
                    if ((_Session as Apache.NMS.ActiveMQ.Session).Started)
                    {
                        TimeSpan TS = _MessageTimeOut == 0 ? TimeSpan.Zero : TimeSpan.FromDays(_MessageTimeOut);
                        _Producer.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Highest, TS);
                    }
                    else
                    {
                        //throw new Exception("Network connection or ActiveMQService Has been closed!");
                        if (log.IsInfoEnabled) log.Info("Network connection or ActiveMQService Has been closed!");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "BaseMQAdapter SetHeartBeat: Error happened(" + ex.Message + ")";
                if (log.IsErrorEnabled) log.Error(ErrorMsg, ex);
                System.Environment.Exit(-1);
            }
        }
        private void SlowDownProducer(int DelayedPerWhenNumber, int DelayedMillisecond)
        {
            if (_sendAmounnts % DelayedPerWhenNumber == 0)
            {
                Thread.Sleep(DelayedMillisecond);
            }
        }
        private void SlowDownProducer1()
        {
            //int iSlowDownNum = this.SenderDBAction == DBAction.Query ? 100 : this.SenderDBAction == DBAction.Add ? 8 : this.SenderDBAction == DBAction.Update ? 8 : this.SenderDBAction == DBAction.Delete ? 20 :
            //                   30;
            //if (_sendAmounnts % iSlowDownNum == 0)
            //{
            //    Thread.Sleep(10);
            //    //Thread.Sleep(20);
            //}


            int iSlowDownNum;
            if (this.SenderDBAction == DBAction.Query)
            {
                iSlowDownNum = 50;
                if (_sendAmounnts % iSlowDownNum == 0)
                {
                    Thread.Sleep(25);
                }
            }
            else if (this.SenderDBAction == DBAction.Add)
            {
                iSlowDownNum = 12;
                if (_sendAmounnts % iSlowDownNum == 0)
                {
                    Thread.Sleep(10);
                }
            }
            else if (this.SenderDBAction == DBAction.Update)
            {
                iSlowDownNum = 10;
                if (_sendAmounnts % iSlowDownNum == 0)
                {
                    Thread.Sleep(10);
                }

            }
            else if (this.SenderDBAction == DBAction.Delete)
            {
                iSlowDownNum = 20;
                if (_sendAmounnts % iSlowDownNum == 0)
                {
                    Thread.Sleep(10);
                }
            }
            else if (this.SenderDBAction == DBAction.None)
            {
                iSlowDownNum = 30;
                if (_sendAmounnts % iSlowDownNum == 0)
                {
                    Thread.Sleep(10);
                }
            }
        }
        private void _Connection_ConnectionResumedListener()
        {
            string ConnActiveUrl = (_Connection as Apache.NMS.ActiveMQ.Connection).ITransport.RemoteAddress.AbsoluteUri;
            if (log.IsErrorEnabled) log.ErrorFormat("Connection has performed fault-tolerant switch to {0}", ConnActiveUrl);
        }
        private void _Connection_ExceptionListener(Exception exception)
        {
            string ConnActiveUrl = (_Connection as Apache.NMS.ActiveMQ.Connection).ITransport.RemoteAddress.AbsoluteUri;
            if (log.IsErrorEnabled) log.ErrorFormat("Connection has performed fault-tolerant switch to {0}", ConnActiveUrl);
        }
    }
}
