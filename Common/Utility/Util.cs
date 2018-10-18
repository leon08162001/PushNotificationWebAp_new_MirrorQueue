using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Net.Http;
using System.Web;
using System.ServiceModel.Channels;

namespace Common.Utility
{
    public class Util
    {
        [DllImport("Iphlpapi.dll")]
        static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);
        [DllImport("Ws2_32.dll")]
        static extern Int32 inet_addr(string ipaddr);


        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 轉換MsgType.cs中Tag Class的常數名稱及常數值為Dictionary
        /// </summary>
        /// <param name="TagType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertTagClassConstants(Type TagType)
        {
            Dictionary<string, string> DicTag = new Dictionary<string, string>();
            try
            {
                FieldInfo[] fieldInfos = TagType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
                PropertyInfo[] propertyInfos = TagType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
                if (fieldInfos.Length > 0)
                {
                    foreach (FieldInfo fi in fieldInfos)
                    {
                        if (fi.IsLiteral && !fi.IsInitOnly)
                        {
                            DicTag.Add(fi.GetValue(TagType).ToString(), fi.Name);
                        }
                    }
                }
                if (propertyInfos.Length > 0)
                {
                    foreach (PropertyInfo property in propertyInfos)
                    {
                        if (property.MemberType == MemberTypes.Property)
                        {
                            DicTag.Add(property.Name, property.Name);
                        }
                    }
                    DicTag.Add("99", "MacAddress");
                    DicTag.Add("710", "MessageID");
                    DicTag.Add("10038", "TotalRecords");
                    DicTag.Add("9999", "Sequence");
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error("Util AddMessageToRow: ", ex);
            }
            return DicTag;
        }
        /// <summary>
        /// 建立存放MQMessge資料內容的DataTable Schema
        /// </summary>
        /// <param name="DicTagType">指定的TagType</param>
        public static DataTable CreateTableSchema(Dictionary<string, string> DicTagType, Type TagType)
        {
            DataTable MessageDT = new DataTable();
            foreach (string key in DicTagType.Keys)
            {
                string FiledName = DicTagType[key].ToString();
                MessageDT.Columns.Add(FiledName, typeof(System.String));
            }
            return MessageDT;
        }
        /// <summary>
        /// 將每筆MQMessage加入至DataTable
        /// </summary>
        /// <param name="DicMQMessage">每筆MQMessage的Dictionary資料形式</param>
        /// <param name="MessagRow"></param>
        public static DataTable AddMessageToTable(Dictionary<string, string> DicMQMessage, out DataRow MessagRow, Dictionary<string, string> DicTagType, Type TagType, DataTable MessageDT)
        {
            try
            {
                DataRow tmpMessagRow = MessageDT.NewRow();
                foreach (string key in DicMQMessage.Keys)
                {
                    tmpMessagRow[DicTagType[key].ToString()] = DicMQMessage[key] == null ? DicMQMessage[key] : DicMQMessage[key].ToString();
                }
                MessageDT.Rows.Add(tmpMessagRow);
                MessagRow = tmpMessagRow;
            }
            catch (Exception ex)
            {
                MessagRow = null;
                if (log.IsErrorEnabled) log.Error("Util AddMessageToTable: ", ex);
            }
            return MessageDT;
        }
        public static DataRow AddMessageToRow(Dictionary<string, string> DicMQMessage, Dictionary<string, string> DicTagType, Type TagType, DataTable MessageDT)
        {
            DataRow tmpMessagRow = MessageDT.NewRow();
            try
            {
                foreach (string key in DicMQMessage.Keys)
                {
                    tmpMessagRow[DicTagType[key].ToString()] = DicMQMessage[key] == null ? DicMQMessage[key] : DicMQMessage[key].ToString();
                }
            }
            catch (Exception ex)
            {
                tmpMessagRow = null;
                if (log.IsErrorEnabled) log.Error("Util AddMessageToRow: ", ex);
            }
            return tmpMessagRow;
        }
        /// <summary>
        /// 將Fix字串轉換成Dictionary
        /// </summary>
        /// <param name="Message">字串型態資料</param>
        /// <param name="DataSplitChar">資料分隔字元</param>
        /// <param name="DataMapChar">field和value分隔字元</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToMessageMapForFix(string Message, string DataSplitChar, string DataMapChar)
        {
            Dictionary<string, string> MessageMap = new Dictionary<string, string>();
            try
            {
                IEnumerable<string> IEnumData = Message.Split(DataSplitChar.ToCharArray()).AsEnumerable();
                string TempLostData = "";
                foreach (string Data in IEnumData)
                {
                    if (Data.Contains(DataMapChar))
                    {
                        int fixTag;
                        string LeftStr = Data.Substring(0, Data.IndexOf("="));
                        if (int.TryParse(LeftStr, out fixTag))
                        {
                            if (TempLostData != "")
                            {
                                MessageMap[MessageMap.ElementAt(MessageMap.Count - 1).Key] += TempLostData;
                                TempLostData = "";
                            }
                            string[] AryKeyValue = Data.Split(DataMapChar.ToCharArray());
                            if (AryKeyValue.Length == 2 && AryKeyValue[0] != "" && AryKeyValue[1] != "")
                            {
                                MessageMap.Add(AryKeyValue[0], AryKeyValue[1]);
                            }
                            else
                            {
                                TempLostData = "";
                                for (int i = 1; i < AryKeyValue.Length; i++)
                                {
                                    TempLostData += AryKeyValue[i] + "=";
                                }
                                TempLostData = TempLostData.Substring(0, TempLostData.Length - 1);
                                MessageMap.Add(AryKeyValue[0], TempLostData);
                                TempLostData = "";
                            }
                        }
                        else
                        {
                            TempLostData += " " + Data;
                        }
                    }
                    else
                    {
                        TempLostData += " " + Data;
                        continue;
                    }
                }
                if (TempLostData != "")
                {
                    MessageMap[MessageMap.ElementAt(MessageMap.Count - 1).Key] += TempLostData;
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error("Util AddMessageToRow: ", ex);
            }
            return MessageMap;
        }

        /// <summary>
        /// 將Json字串轉換成Dictionary
        /// </summary>
        /// <param name="Message">字串型態資料</param>
        /// <param name="DataSplitChar">資料分隔字元</param>
        /// <param name="DataMapChar">field和value分隔字元</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ToMessageMapForJson(string sJson)
        {
            List<Dictionary<string, string>> MessageMapList = new List<Dictionary<string, string>>();
            Dictionary<string, string> MessageMap = new Dictionary<string, string>();
            try
            {
                var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };
                var json = JValue.Parse(sJson);
                if (json.Type == JTokenType.Object)
                {
                    MessageMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(sJson, settings);
                    MessageMapList.Add(MessageMap);
                }
                else if (json.Type == JTokenType.Array)
                {
                    MessageMapList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(sJson, settings);
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error("Util AddMessageToRow: ", ex);
            }
            return MessageMapList;
        }
        /// <summary>
        /// 取得MacAddress
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        /// <summary>
        /// 根據IP取得MacAddress
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string GetMacAddress(string IPAddress = null)
        {
            string strMacAddress = string.Empty;
            StringBuilder SBMacAddress = new StringBuilder();
            try
            {
                Int32 remote = inet_addr(IPAddress);

                Int64 macinfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macinfo, ref length);

                string temp = System.Convert.ToString(macinfo, 16).PadLeft(12, '0').ToUpper();

                int x = 12;
                for (int i = 0; i < 6; i++)
                {
                    if (i == 5) { SBMacAddress.Append(temp.Substring(x - 2, 2)); }
                    else { SBMacAddress.Append(temp.Substring(x - 2, 2) + ":"); }
                    x -= 2;
                }
                strMacAddress = SBMacAddress.ToString();
            }
            catch (Exception ex)
            {
                strMacAddress = string.Empty;
                if (log.IsErrorEnabled) log.Error("Util AddMessageToRow: ", ex);
            }
            return strMacAddress;
        }

        public static string GetEMSFailOverConnString(string ConnString)
        {
            StringBuilder FailOverConnString = new StringBuilder("");
            if (ConnString.IndexOf(",") == -1)
            {
                FailOverConnString.Append("tcp://" + ConnString + ",tcp://" + ConnString);
            }
            else
            {
                string sPort = ConnString.IndexOf(":") == -1 ? "7222" : ConnString.Substring(ConnString.IndexOf(":") + 1);
                string sUrls = ConnString.IndexOf(":") == -1 ? ConnString : ConnString.Substring(0, ConnString.IndexOf(":"));
                List<string> urls = sUrls.Split(new char[] { ',' }).ToList<string>();
                foreach (string url in urls)
                {
                    FailOverConnString.Append("tcp://" + url + ":" + sPort + ",");
                }
                if (FailOverConnString.Length > 0)
                {
                    FailOverConnString.Remove(FailOverConnString.Length - 1, 1);
                }
            }
            return FailOverConnString.ToString();
        }

        public static string GetMQFailOverConnString(string Urls, string Ports, bool useSSL = false)
        {
            string OtherFailOverOptions = "?transport.randomize=true&amp;transport.trackMessages=true&amp;transport.priorityBackup=true";
            StringBuilder FailOverConnString = new StringBuilder("");
            //代表只有1個IP
            if (Urls.IndexOf(",") == -1)
            {
                string port;
                //代表只有1個port
                if (Ports.IndexOf(",") == -1)
                {
                    port = Ports;
                    if (useSSL)
                    {
                        FailOverConnString.Append("failover:ssl://" + Urls + ":" + port);
                        FailOverConnString.Append("?transport.acceptInvalidBrokerCert=true");
                        //FailOverConnString.Append("&amp;transport.clientCertFilename=amq-client.ts");
                        //FailOverConnString.Append("&amp;transport.clientCertPassword=880816");
                        //FailOverConnString.Append("&amp;transport.serverName=60.248.159.60");
                        FailOverConnString.Append("&amp;transport.needClientAuth=false");
                    }
                    else
                    {
                        FailOverConnString.Append("failover:tcp://" + Urls + ":" + port);
                    }
                }
                //代表多個port
                else
                {
                    int portsCount = Ports.Split(new char[] { ',' }).Length;
                    for (int i = 0; i < portsCount; i++)
                    {
                        if (useSSL)
                        {
                            FailOverConnString.Append("ssl://" + Urls + ":" + Ports.Split(new char[] { ',' })[i]);
                            FailOverConnString.Append("?transport.acceptInvalidBrokerCert=true");
                            FailOverConnString.Append("&amp;transport.needClientAuth=false");
                            FailOverConnString.Append(",");
                        }
                        else
                        {
                            FailOverConnString.Append("tcp://" + Urls + ":" + Ports.Split(new char[] { ',' })[i] + ",");
                        }
                    }
                    if (FailOverConnString.Length > 0)
                    {
                        FailOverConnString.Insert(0, "failover:(");
                        FailOverConnString.Remove(FailOverConnString.Length - 1, 1);
                        FailOverConnString.Append(")");
                        FailOverConnString.Append(OtherFailOverOptions);
                    }
                }
            }
            //代表多個IP
            else
            {
                string sPort = "";
                List<string> urls = Urls.Split(new char[] { ',' }).ToList<string>();
                int i = 0;
                foreach (string url in urls)
                {
                    //代表只有1個port
                    if (Ports.IndexOf(",") == -1)
                    {
                        sPort = string.IsNullOrEmpty(Ports) ? "61616" : Ports;
                    }
                    else
                    {
                        sPort = Ports.Split(new char[] { ',' })[i];
                        i++;
                    }
                    if (useSSL)
                    {
                        FailOverConnString.Append("ssl://" + url + ":" + sPort);
                        FailOverConnString.Append("?transport.acceptInvalidBrokerCert=true");
                        FailOverConnString.Append("&amp;transport.needClientAuth=false");
                        FailOverConnString.Append(",");
                    }
                    else
                    {
                        FailOverConnString.Append("tcp://" + url + ":" + sPort + ",");
                    }
                }
                if (FailOverConnString.Length > 0)
                {
                    FailOverConnString.Insert(0, "failover:(");
                    FailOverConnString.Remove(FailOverConnString.Length - 1, 1);
                    FailOverConnString.Append(")");
                    FailOverConnString.Append(OtherFailOverOptions);
                }
            }
            return FailOverConnString.ToString();
        }

        public static string GetClientIp(HttpRequestMessage request = null)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    }
}
