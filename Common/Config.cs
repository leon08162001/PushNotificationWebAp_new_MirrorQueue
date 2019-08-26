using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace Common
{
    public class Config
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _path;
        public static Context Context;

        //Tibco RV Setting
        public string Tibco_service;
        public string Tibco_network;
        public string Tibco_daemon;
        public string TibcoReceivedMessageReservedSeconds = "30";

        //ActiveMQ&ApolloMQ Setting
        public string MQUserID;
        public string MQPwd;
        public string MQ_service;
        public string MQ_network;
        public bool MQ_useSSL = false;
        public string MQClientID;
        public string MQReceivedMessageReservedSeconds = "30";

        //Tibco EMS Setting
        public string EMSUserID;
        public string EMSPwd;
        public string EMS_service;
        public string EMS_network;
        public bool EMS_useSSL = false;
        public string EMSClientID;
        public List<string> EMS_CertsPath = new List<string>();
        public string EMSReceivedMessageReservedSeconds = "30";

        //Y77
        public string jefferiesExcuReport_Sender_Topic;
        public string jefferiesExcuReport_Listener_Topic;
        public string jefferiesExcuReportMaxThreads = "0";
        public bool isUsingThreadPoolThreadForY77 = true;

        //OTAExport
        public string otaExport_Sender_Topic;
        public string otaExport_Listener_Topic;
        public string otaExportMaxThreads = "0";
        public bool isUsingThreadPoolThreadForOTA = true;

        //OTA1Export
        public string ota1Export_Sender_Topic;
        public string ota1Export_Listener_Topic;
        public string ota1ExportMaxThreads = "0";
        public bool isUsingThreadPoolThreadForOTA1 = true;

        //OTA2Export
        public string ota2Export_Sender_Topic;
        public string ota2Export_Listener_Topic;
        public string ota2ExportMaxThreads = "0";
        public bool isUsingThreadPoolThreadForOTA2 = true;

        public Config(string path)
        {
            _path = AppDomain.CurrentDomain.BaseDirectory + path;
        }

        public void ReadParameter()
        {
            if (!File.Exists(_path))
            {
                if (log.IsErrorEnabled) log.Error("Config.cs: can't find " + _path);
                return;
            }
            using (StreamReader sr = new StreamReader(_path))
            {
                try
                {
                    while (sr.Peek() > 0)
                    {
                        string line = sr.ReadLine().Trim();

                        if (line == "")
                            continue;

                        if (line[0] == '#')
                            continue;

                        int seperator = line.IndexOf("=");

                        if (seperator <= 0)
                            return;

                        string config_name = line.Substring(0, seperator);

                        string config_value = line.Substring(seperator + 1, line.Length - seperator - 1);
                        config_name = config_name.ToUpper();

                        switch (config_name)
                        {
                            //Tibco Setting
                            case "TIBCO_SERVICE":
                                Tibco_service = config_value;
                                break;
                            case "TIBCO_NETWORK":
                                Tibco_network = config_value;
                                break;
                            case "TIBCO_DAEMON":
                                Tibco_daemon = config_value;
                                break;
                            case "TIBCORECEIVEDMESSAGERESERVEDSECONDS":
                            {
                                int TestValue;
                                TibcoReceivedMessageReservedSeconds = int.TryParse(config_value, out TestValue)
                                    ? TestValue.ToString()
                                    : TibcoReceivedMessageReservedSeconds;
                                break;
                            }
                            //ActiveMQ&ApolloMQ Setting
                            case "MQUSERID":
                                MQUserID = config_value;
                                break;
                            case "MQPWD":
                                MQPwd = config_value;
                                break;
                            case "MQ_SERVICE":
                                MQ_service = config_value;
                                break;
                            case "MQ_NETWORK":
                                MQ_network = config_value;
                                break;
                            case "MQ_USESSL":
                                MQ_useSSL = Convert.ToBoolean(config_value);
                                break;
                            case "MQCLIENTID":
                                MQClientID = config_value;
                                break;
                            case "MQRECEIVEDMESSAGERESERVEDSECONDS":
                            {
                                int TestValue;
                                MQReceivedMessageReservedSeconds = int.TryParse(config_value, out TestValue)
                                    ? TestValue.ToString()
                                    : MQReceivedMessageReservedSeconds;
                                break;
                            }

                            //Tibco EMS Setting
                            case "EMSUSERID":
                                EMSUserID = config_value;
                                break;
                            case "EMSPWD":
                                EMSPwd = config_value;
                                break;
                            case "EMS_SERVICE":
                                EMS_service = config_value;
                                break;
                            case "EMS_NETWORK":
                                EMS_network = config_value;
                                break;
                            case "EMS_USESSL":
                                EMS_useSSL = Convert.ToBoolean(config_value);
                                break;
                            case "EMSCLIENTID":
                                EMSClientID = config_value;
                                break;
                            case "EMS_CERTSPATH":
                                if (config_value.IndexOf(",") == -1)
                                {
                                    EMS_CertsPath.Add(config_value);
                                }
                                else
                                {
                                    EMS_CertsPath = config_value.Split(new char[] { ',' }).ToList<string>();
                                }
                                break;                      
                            case "EMSRECEIVEDMESSAGERESERVEDSECONDS":
                            {
                                int TestValue;
                                EMSReceivedMessageReservedSeconds = int.TryParse(config_value, out TestValue)
                                    ? TestValue.ToString()
                                    : EMSReceivedMessageReservedSeconds;
                                break;
                            }

                            //Y77
                            case "JEFFERIESEXCUREPORT_SENDER_TOPIC":
                                jefferiesExcuReport_Sender_Topic = config_value;
                                break;
                            case "JEFFERIESEXCUREPORT_LISTENER_TOPIC":
                                jefferiesExcuReport_Listener_Topic = config_value;
                                break;
                            case "JEFFERIESEXCUREPORTMAXTHREADS":
                            {
                                int ConfigValue;
                                jefferiesExcuReportMaxThreads = int.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue.ToString()
                                    : jefferiesExcuReportMaxThreads;
                                break;
                            }
                            case "ISUSINGTHREADPOOLTHREADFORY77":
                            {
                                bool ConfigValue;
                                isUsingThreadPoolThreadForY77 = bool.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue
                                    : isUsingThreadPoolThreadForY77;
                                break;
                            }

                            //OTAExport
                            case "OTAEXPORT_SENDER_TOPIC":
                                otaExport_Sender_Topic = config_value;
                                break;
                            case "OTAEXPORT_LISTENER_TOPIC":
                                otaExport_Listener_Topic = config_value;
                                break;
                            case "OTAEXPORTMAXTHREADS":
                            {
                                int ConfigValue;
                                otaExportMaxThreads = int.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue.ToString()
                                    : otaExportMaxThreads;
                                break;
                            }
                            case "ISUSINGTHREADPOOLTHREADFOROTA":
                            {
                                bool ConfigValue;
                                isUsingThreadPoolThreadForOTA = bool.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue
                                    : isUsingThreadPoolThreadForOTA;
                                break;
                            }

                            //OTA1Export
                            case "OTA1EXPORT_SENDER_TOPIC":
                                ota1Export_Sender_Topic = config_value;
                                break;
                            case "OTA1EXPORT_LISTENER_TOPIC":
                                ota1Export_Listener_Topic = config_value;
                                break;
                            case "OTA1EXPORTMAXTHREADS":
                            {
                                int ConfigValue;
                                ota1ExportMaxThreads = int.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue.ToString()
                                    : ota1ExportMaxThreads;
                                break;
                            }
                            case "ISUSINGTHREADPOOLTHREADFOROTA1":
                            {
                                bool ConfigValue;
                                isUsingThreadPoolThreadForOTA1 = bool.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue
                                    : isUsingThreadPoolThreadForOTA1;
                                break;
                            }

                            //OTA2Export
                            case "OTA2EXPORT_SENDER_TOPIC":
                                ota2Export_Sender_Topic = config_value;
                                break;
                            case "OTA2EXPORT_LISTENER_TOPIC":
                                ota2Export_Listener_Topic = config_value;
                                break;
                            case "OTA2EXPORTMAXTHREADS":
                            {
                                int ConfigValue;
                                ota2ExportMaxThreads = int.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue.ToString()
                                    : ota2ExportMaxThreads;
                                break;
                            }
                            case "ISUSINGTHREADPOOLTHREADFOROTA2":
                            {
                                bool ConfigValue;
                                isUsingThreadPoolThreadForOTA2 = bool.TryParse(config_value, out ConfigValue)
                                    ? ConfigValue
                                    : isUsingThreadPoolThreadForOTA2;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error("ReadParameter() Error", ex);
                }
            }
        }

    }
}
