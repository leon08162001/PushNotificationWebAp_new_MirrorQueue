using Common.HandlerLayer;
using Common.LinkLayer;
using DataAccess;
using DBContext;
using DBModels;
using MQDemo.Messager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Ciphers;
using Utility;

namespace MQDemoSubscriber
{
    /// <summary>
    /// 呼叫WorkThreads工作的中介class(類似MVC裡的Controller,處理view(winform)和model(WorkThreads)的互動)
    /// </summary>
    public class TopicController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static MQServer_Receive_Response_Test _MQServer_Receive_Response_Test;
        //public static TibcoServer_Receive_Response_Test _TibcoServer_Receive_Response_Test;
        public static EMSServer_Receive_Response_Test _EMSServer_Receive_Response_Test;

        public static void HandleTopic(MQServer_Receive_Response_Test ActiveForm, IMQAdapter IMQAdapter)
        {
            WorkThreads.IsEventInUIThread = true;
            _MQServer_Receive_Response_Test = ActiveForm;
            Amib.Threading.Action WorkItem = new Amib.Threading.Action(() =>
            {
                while (!IMQAdapter.Handler.WorkItemQueue.Exit)
                {
                    try
                    {
                        //DataTable MessageDT = TopicTypeHandler.WorkItemQueue.Dequeue();
                        DataTable MessageDT = IMQAdapter.Handler.WorkItemQueue.Dequeue();
                        if (MessageDT != null && !IMQAdapter.Handler.WorkItemQueue.Exit)
                        {
                            if (MessageDT.TableName.Equals("file"))
                            {
                                //存檔
                                if (MessageDT.Rows.Count > 0)
                                {
                                    DataRow dr = MessageDT.Rows[0];
                                    string FielName = dr["filename"].ToString();
                                    File.Delete(@"D:\temp\" + FielName);
                                    File.WriteAllBytes(@"D:\temp\"+ FielName, (dr["content"] as byte[]));
                                }
                            }
                            else
                            {
                                WorkThreads.WaitOrContinueByMaxThreads(IMQAdapter.Handler);
                                Func<TopicType, DataTable, DataTable> DoDBTask = new Func<TopicType, DataTable, DataTable>(DBFactory.DoDBTask);
                                Func<Type, DataTable, List<List<MessageField>>> ConvertTagToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                Func<String, List<List<MessageField>>, int, int, bool> SendMQMessage = new Func<String, List<List<MessageField>>, int, int, bool>(IMQAdapter.SendMQMessage);
                                WorkThreads.DoTask<TopicType, DataTable, Type, String, DataTable, List<List<MessageField>>>(IMQAdapter, DoDBTask, ConvertTagToMessage, SendMQMessage, IMQAdapter.Handler.TopicType, MessageDT, IMQAdapter.Handler.ResponseTag, "710");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (log.IsErrorEnabled) log.Error("HandleTopic(): inner loop exception ", ex);
                    }
                }
            });
            IMQAdapter.Handler.IsCallHandleTopic = true;
            IMQAdapter.Handler.WorkDispatcher.QueueWorkItem(WorkItem);
        }
        public static void HandleTopic(EMSServer_Receive_Response_Test ActiveForm, IEMSAdapter IEMSAdapter)
        {
            WorkThreads.IsEventInUIThread = true;
            _EMSServer_Receive_Response_Test = ActiveForm;
            Amib.Threading.Action WorkItem = new Amib.Threading.Action(() =>
            {
                while (!IEMSAdapter.Handler.WorkItemQueue.Exit)
                {
                    try
                    {
                        //DataTable MessageDT = TopicTypeHandler.WorkItemQueue.Dequeue();
                        DataTable MessageDT = IEMSAdapter.Handler.WorkItemQueue.Dequeue();
                        if (MessageDT != null && !IEMSAdapter.Handler.WorkItemQueue.Exit)
                        {
                            if (MessageDT.TableName.Equals("file"))
                            {
                                //存檔
                                if (MessageDT.Rows.Count > 0)
                                {
                                    DataRow dr = MessageDT.Rows[0];
                                    string FielName = dr["filename"].ToString();
                                    File.Delete(@"D:\temp\" + FielName);
                                    File.WriteAllBytes(@"D:\temp\" + FielName, (dr["content"] as byte[]));
                                }
                            }
                            else
                            {
                                WorkThreads.WaitOrContinueByMaxThreads(IEMSAdapter.Handler);
                                Func<TopicType, DataTable, DataTable> DoDBTask = new Func<TopicType, DataTable, DataTable>(DBFactory.DoDBTask);
                                Func<Type, DataTable, List<List<MessageField>>> ConvertTagToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                Func<String, List<List<MessageField>>, int, int, bool> SendEMSMessage = new Func<String, List<List<MessageField>>, int, int, bool>(IEMSAdapter.SendEMSMessage);
                                WorkThreads.DoTask<TopicType, DataTable, Type, String, DataTable, List<List<MessageField>>>(IEMSAdapter, DoDBTask, ConvertTagToMessage, SendEMSMessage, IEMSAdapter.Handler.TopicType, MessageDT, IEMSAdapter.Handler.ResponseTag, "710");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (log.IsErrorEnabled) log.Error("HandleTopic(): inner loop exception ", ex);
                    }
                }
            });
            IEMSAdapter.Handler.IsCallHandleTopic = true;
            IEMSAdapter.Handler.WorkDispatcher.QueueWorkItem(WorkItem);
        }
        public static void HandleTopicForJson<T>(MQServer_Receive_Response_Test ActiveForm, IMQAdapter IMQAdapter) where T : class
        {
            WorkThreads.IsEventInUIThread = true;
            _MQServer_Receive_Response_Test = ActiveForm;
            Amib.Threading.Action WorkItem = new Amib.Threading.Action(() =>
            {
                while (!IMQAdapter.Handler.WorkItemQueue.Exit)
                {
                    try
                    {
                        DataTable MessageDT = IMQAdapter.Handler.WorkItemQueue.Dequeue();
                        if (MessageDT != null && !IMQAdapter.Handler.WorkItemQueue.Exit)
                        {
                            if (MessageDT.TableName.Equals("file"))
                            {
                                //存檔
                                if (MessageDT.Rows.Count > 0)
                                {
                                    DataRow dr = MessageDT.Rows[0];
                                    string FileName = dr["filename"].ToString();
                                    File.Delete(@"D:\temp\" + FileName);
                                    File.WriteAllBytes(@"D:\temp\" + FileName, (dr["content"] as byte[]));
                                }
                            }
                            else
                            {
                                WorkThreads.WaitOrContinueByMaxThreads(IMQAdapter.Handler);
                                var settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    MissingMemberHandling = MissingMemberHandling.Ignore
                                };

                                string JSONresult = JsonConvert.SerializeObject(MessageDT);

                                //List<T> Customer = JsonConvert.DeserializeObject<List<T>>(JSONresult, settings);
                                List<loanApplication_customer> Customer = JsonConvert.DeserializeObject<List<loanApplication_customer>>(JSONresult, settings);
                                //SpecificEntityRepositoryEntityRepository<T> Entities = new SpecificEntityRepositoryEntityRepository<T>(new LAS_TWEntities());
                                //StaticCommonEntityRepository.InitDBContext(new LAS_TWEntities());
                                SpecificEntityRepository<loanApplication_customer> Entities = new SpecificEntityRepository<loanApplication_customer>(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
                                //StaticCommonEntityRepository.InitDBContext(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
                                if (IMQAdapter.ReceiverDBAction == DBAction.Query)
                                {
                                    //List<loanApplication_customer> a = StaticCommonEntityRepository.GetAll<loanApplication_customer>(r => r.pk, r => r.seq, r => r.nickname);
                                    //bool IsExist = StaticCommonEntityRepository.CheckPKExist<loanApplication_customer>(new Dictionary<string, object> { { "PK", "21" } });
                                    bool IsExist = Entities.CheckPKExist(new Dictionary<string, object> { { "PK", "21" } });
                                    Dictionary<string, object> updateInParams = new Dictionary<string, object>();
                                    updateInParams.Add("pk", " 21");
                                    updateInParams.Add("nickname", " 李乃興");

                                    Dictionary<string, object> InParams = new Dictionary<string, object>();
                                    InParams.Add("nickname", "王勝達");
                                    //Dictionary<string, object> OutParams = new Dictionary<string, object>();
                                    //OutParams.Add("SalesID", 0);
                                    object outValue;
                                    //Entities.ExecuteProcedure("sp_UpdateCustomers", out outValue, updateInParams, null);
                                    //StaticCommonEntityRepository.ExecuteProcedure("sp_UpdateCustomers", out outValue, updateInParams, null);

                                    //List<loanApplication_customer> Result1 = Entities.ExecuteProcedure<loanApplication_customer>("sp_GetCustomers", out outValue, InParams, null);
                                    var Result = Entities.ExecuteProcedure("sp_GetCustomers", 1, new Type[] { typeof(loanApplication_customer) }, out outValue, InParams, null);
                                    var Result2 = Entities.ExecuteProcedure("sp_GetCustomers2", 2, new Type[] { typeof(loanApplication_customer), typeof(comUser) }, out outValue, InParams, null);
                                    var Result3 = Entities.ExecuteProcedure("sp_GetCustomers3", 3, new Type[] { typeof(loanApplication_customer), typeof(comUser), typeof(UserActivityLog) }, out outValue, InParams, null);
                                    List<loanApplication_customer> a = Result3[0] as List<loanApplication_customer>;

                                    //var Result = StaticCommonEntityRepository.ExecuteProcedure("sp_GetCustomers",1, new Type[] { typeof(loanApplication_customer)}, out outValue, InParams, null);
                                    //var Result2 = StaticCommonEntityRepository.ExecuteProcedure("sp_GetCustomers2",2, new Type[] { typeof(loanApplication_customer), typeof(comUser) }, out outValue, InParams, null);
                                    //var Result3 = StaticCommonEntityRepository.ExecuteProcedure("sp_GetCustomers3", 3, new Type[] { typeof(loanApplication_customer), typeof(comUser), typeof(UserActivityLog) }, out outValue, InParams, null);

                                    Dictionary<string, object> ParamsList = new Dictionary<string, object>();
                                    ParamsList.Add("seq", "2");

                                    //var ResultSets = StaticCommonEntityRepository.FindMultiRecordSet("select * from [dbo].[loanApplication_customer] where seq=@seq order by customer_type;", ParamsList, 1, new Type[] { typeof(loanApplication_customer) });

                                    //List<NewCustomers> Customers = StaticCommonEntityRepository.Find<NewCustomers>("select pk,order_nbr orderno,country from [dbo].[loanApplication_customer] where nickname = @nickname", new Dictionary<string, object> { { "nickname", "李乃興" } });
                                    //List<T> Customers1 = StaticCommonEntityRepository.Find<T>(Customer, p => (p as loanApplication_customer).customer_type);
                                    //List<T> Customers1 = StaticCommonEntityRepository.Find<T>(Customer);

                                    //List<T> Customers1 = StaticCommonEntityRepository.Find<T>(Customer, "customer_type");
                                    Entities.Find(Customer);
                                    //Query<T>(Customer);

                                    WorkThreads.WaitOrContinueByMaxThreads(IMQAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IMQAdapter, ConvertTableToMessage, MessageDT, "710");
                                }
                                else if (IMQAdapter.ReceiverDBAction == DBAction.Add)
                                {
                                    //StaticCommonEntityRepository.Add<T>(Customer);
                                    Entities.Add(Customer);
                                    //Add<T>(Customer);
                                    WorkThreads.WaitOrContinueByMaxThreads(IMQAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IMQAdapter, ConvertTableToMessage, MessageDT, "710");
                                }
                                else if (IMQAdapter.ReceiverDBAction == DBAction.Update)
                                {
                                    //StaticCommonEntityRepository.Update<T>(Customer);
                                    Entities.Update(Customer);
                                    //Update<T>(Customer);
                                    WorkThreads.WaitOrContinueByMaxThreads(IMQAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IMQAdapter, ConvertTableToMessage, MessageDT, "710");
                                }
                                else if (IMQAdapter.ReceiverDBAction == DBAction.Delete)
                                {
                                    //StaticCommonEntityRepository.Delete<T>(Customer);
                                    Entities.Delete(Customer);
                                    //Delete<T>(Customer);
                                    WorkThreads.WaitOrContinueByMaxThreads(IMQAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IMQAdapter, ConvertTableToMessage, MessageDT, "710");
                                }

                                //對android傳來的簽章資料驗章且分散資料庫儲存 begin
                                CertificateDataVerifySaveDBTest<T>(MessageDT);
                                //if (typeof(T) == typeof(SignMessage))
                                //{
                                //    List<SignMessage> SignMessage = JsonConvert.DeserializeObject<List<SignMessage>>(JSONresult, settings);
                                //    bool IsOK = CipherHelper.RsaVerifyData(DecEncCode.AESDecrypt(SignMessage[0].cipherText), SignMessage[0].sign, SignMessage[0].publickey);
                                //}

                                //int userCount = TopicController.GetCount<comUser>();
                                //comUser newUser = new comUser();
                                //newUser.user_id = "katepan" + userCount;
                                //newUser.country = "TWN";
                                //newUser.first_name = "Kate";
                                //newUser.last_name = "Pan";
                                //newUser.dept = "CEO";
                                //newUser.role = "GEN";
                                //newUser.password = "F9-EC-8F-24-07-18-B8-C5-AD-71-6B-6F-E9-4D-14-E6";
                                //newUser.active = true;
                                //newUser.show_report = true;
                                //newUser.access_report = true;
                                //newUser.access_print = true;
                                //newUser.limited_user = false;
                                //newUser.advanced_user = true;
                                //newUser.last_upd_user = "leonlee";
                                //newUser.last_upd_date = DateTime.Now;
                                //bool result = TopicController.InsertUser(newUser);
                                //對android傳來的簽章資料驗章且分散資料庫儲存 end
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (log.IsErrorEnabled) log.Error("HandleTopicForJson(): inner loop exception ", ex);
                    }
                }
            });
            IMQAdapter.Handler.IsCallHandleTopic = true;
            IMQAdapter.Handler.WorkDispatcher.QueueWorkItem(WorkItem);
        }
        public static void HandleTopicForJson<T>(EMSServer_Receive_Response_Test ActiveForm, IEMSAdapter IEMSAdapter) where T : class, new()
        {
            WorkThreads.IsEventInUIThread = true;
            _EMSServer_Receive_Response_Test = ActiveForm;
            Amib.Threading.Action WorkItem = new Amib.Threading.Action(() =>
            {
                while (!IEMSAdapter.Handler.WorkItemQueue.Exit)
                {
                    try
                    {
                        DataTable MessageDT = IEMSAdapter.Handler.WorkItemQueue.Dequeue();
                        if (MessageDT != null && !IEMSAdapter.Handler.WorkItemQueue.Exit)
                        {
                            if (MessageDT.TableName.Equals("file"))
                            {
                                //存檔
                                if (MessageDT.Rows.Count > 0)
                                {
                                    DataRow dr = MessageDT.Rows[0];
                                    string FielName = dr["filename"].ToString();
                                    File.Delete(@"D:\temp\" + FielName);
                                    File.WriteAllBytes(@"D:\temp\" + FielName, (dr["content"] as byte[]));
                                }
                            }
                            else
                            {
                                WorkThreads.WaitOrContinueByMaxThreads(IEMSAdapter.Handler);
                                var settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    MissingMemberHandling = MissingMemberHandling.Ignore
                                };

                                string JSONresult = JsonConvert.SerializeObject(MessageDT);
                                //List<T> Customer = JsonConvert.DeserializeObject<List<T>>(JSONresult, settings);
                                List<loanApplication_customer> Customer = JsonConvert.DeserializeObject<List<loanApplication_customer>>(JSONresult, settings);
                                //SpecificEntityRepositoryEntityRepository<T> Entities = new SpecificEntityRepositoryEntityRepository<T>(new LAS_TWEntities());
                                //StaticCommonEntityRepository.InitDBContext(new LAS_TWEntities());
                                SpecificEntityRepository<loanApplication_customer> Entities = new SpecificEntityRepository<loanApplication_customer>(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
                                //StaticCommonEntityRepository.InitDBContext(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
                                if (IEMSAdapter.ReceiverDBAction == DBAction.Query)
                                {
                                    //List<loanApplication_customer> a = StaticCommonEntityRepository.GetAll<loanApplication_customer>(r => r.pk, r => r.seq, r => r.nickname);
                                    //bool IsExist = StaticCommonEntityRepository.CheckPKExist<loanApplication_customer>(new Dictionary<string, object> { { "PK", "21" } });
                                    bool IsExist = Entities.CheckPKExist(new Dictionary<string, object> { { "PK", "21" } });
                                    Dictionary<string, object> updateInParams = new Dictionary<string, object>();
                                    updateInParams.Add("pk", " 21");
                                    updateInParams.Add("nickname", " 李乃興");

                                    Dictionary<string, object> InParams = new Dictionary<string, object>();
                                    InParams.Add("nickname", "王勝達");
                                    //Dictionary<string, object> OutParams = new Dictionary<string, object>();
                                    //OutParams.Add("SalesID", 0);
                                    object outValue;
                                    //Entities.ExecuteProcedure("sp_UpdateCustomers", out outValue, updateInParams, null);
                                    //StaticCommonEntityRepository.ExecuteProcedure("sp_UpdateCustomers", out outValue, updateInParams, null);

                                    //List<loanApplication_customer> Result1 = Entities.ExecuteProcedure<loanApplication_customer>("sp_GetCustomers", out outValue, InParams, null);
                                    var Result = Entities.ExecuteProcedure("sp_GetCustomers", 1, new Type[] { typeof(loanApplication_customer) }, out outValue, InParams, null);
                                    var Result2 = Entities.ExecuteProcedure("sp_GetCustomers2", 2, new Type[] { typeof(loanApplication_customer), typeof(comUser) }, out outValue, InParams, null);
                                    var Result3 = Entities.ExecuteProcedure("sp_GetCustomers3", 3, new Type[] { typeof(loanApplication_customer), typeof(comUser), typeof(UserActivityLog) }, out outValue, InParams, null);
                                    List<loanApplication_customer> a = Result3[0] as List<loanApplication_customer>;

                                    //var Result = StaticCommonEntityRepository.ExecuteProcedure("sp_GetCustomers",1, new Type[] { typeof(loanApplication_customer)}, out outValue, InParams, null);
                                    //var Result2 = StaticCommonEntityRepository.ExecuteProcedure("sp_GetCustomers2",2, new Type[] { typeof(loanApplication_customer), typeof(comUser) }, out outValue, InParams, null);
                                    //var Result3 = StaticCommonEntityRepository.ExecuteProcedure("sp_GetCustomers3", 3, new Type[] { typeof(loanApplication_customer), typeof(comUser), typeof(UserActivityLog) }, out outValue, InParams, null);

                                    Dictionary<string, object> ParamsList = new Dictionary<string, object>();
                                    ParamsList.Add("seq", "2");

                                    //var ResultSets = StaticCommonEntityRepository.FindMultiRecordSet("select * from [dbo].[loanApplication_customer] where seq=@seq order by customer_type;", ParamsList, 1, new Type[] { typeof(loanApplication_customer) });

                                    //List<NewCustomers> Customers = StaticCommonEntityRepository.Find<NewCustomers>("select pk,order_nbr orderno,country from [dbo].[loanApplication_customer] where nickname = @nickname", new Dictionary<string, object> { { "nickname", "李乃興" } });
                                    //List<T> Customers1 = StaticCommonEntityRepository.Find<T>(Customer, p => (p as loanApplication_customer).customer_type);
                                    //List<T> Customers1 = StaticCommonEntityRepository.Find<T>(Customer);

                                    //List<T> Customers1 = StaticCommonEntityRepository.Find<T>(Customer, "customer_type");
                                    Entities.Find(Customer);
                                    //Query<T>(Customer);

                                    WorkThreads.WaitOrContinueByMaxThreads(IEMSAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IEMSAdapter, ConvertTableToMessage, MessageDT, "710");
                                }
                                else if (IEMSAdapter.ReceiverDBAction == DBAction.Add)
                                {
                                    //StaticCommonEntityRepository.Add<T>(Customer);
                                    Entities.Add(Customer);
                                    //Add<T>(Customer);
                                    WorkThreads.WaitOrContinueByMaxThreads(IEMSAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IEMSAdapter, ConvertTableToMessage, MessageDT, "710");
                                }
                                else if (IEMSAdapter.ReceiverDBAction == DBAction.Update)
                                {
                                    //StaticCommonEntityRepository.Update<T>(Customer);
                                    Entities.Update(Customer);
                                    //Update<T>(Customer);
                                    WorkThreads.WaitOrContinueByMaxThreads(IEMSAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IEMSAdapter, ConvertTableToMessage, MessageDT, "710");
                                }
                                else if (IEMSAdapter.ReceiverDBAction == DBAction.Delete)
                                {
                                    //StaticCommonEntityRepository.Delete<T>(Customer);
                                    Entities.Delete(Customer);
                                    //Delete<T>(Customer);
                                    WorkThreads.WaitOrContinueByMaxThreads(IEMSAdapter.Handler);
                                    Func<Type, DataTable, List<List<MessageField>>> ConvertTableToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTableToMessage);
                                    WorkThreads.DoConvertSendTaskForJason(IEMSAdapter, ConvertTableToMessage, MessageDT, "710");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (log.IsErrorEnabled) log.Error("HandleTopicForJson(): inner loop exception ", ex);
                    }
                }
            });
            IEMSAdapter.Handler.IsCallHandleTopic = true;
            IEMSAdapter.Handler.WorkDispatcher.QueueWorkItem(WorkItem);
        }
        //public static void HandleTopic(TibcoServer_Receive_Response_Test ActiveForm, TopicTypeHandler TopicTypeHandler, ITibcoAdapter ITibcoAdapter)
        //{
        //    WorkThreads.IsEventInUIThread = true;
        //    _TibcoServer_Receive_Response_Test = ActiveForm;
        //    Amib.Threading.Action WorkItem = new Amib.Threading.Action(() =>
        //    {
        //        while (!TopicTypeHandler.WorkItemQueue.Exit)
        //        {
        //            try
        //            {
        //                DataTable MessageDT = TopicTypeHandler.WorkItemQueue.Dequeue();
        //                if (MessageDT != null && !TopicTypeHandler.WorkItemQueue.Exit)
        //                {
        //                    WorkThreads.WaitOrContinueByMaxThreads(TopicTypeHandler);
        //                    Func<TopicType, DataTable, DataTable> DoDBTask = new Func<TopicType, DataTable, DataTable>(DBFactory.DoDBTask);
        //                    Func<Type, DataTable, List<List<MessageField>>> ConvertTagToMessage = new Func<Type, DataTable, List<List<MessageField>>>(FixFactory.ConvertTagToMessage);
        //                    Action<String, List<List<MessageField>>> SendTibcoMessage = new Action<string, List<List<MessageField>>>(ITibcoAdapter.SendTibcoMessage);
        //                    WorkThreads.DoTask<TopicType, DataTable, Type, String, DataTable, List<List<MessageField>>>(ITibcoAdapter, TopicTypeHandler, DoDBTask, ConvertTagToMessage, SendTibcoMessage, TopicTypeHandler.TopicType, MessageDT, TopicTypeHandler.ResponseTag, "710");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                if (log.IsErrorEnabled) log.Error("HandleTopic(): inner loop exception ", ex);
        //            }
        //        }
        //    });
        //    TopicTypeHandler.IsCallHandleTopic = true;
        //    TopicTypeHandler.WorkDispatcher.QueueWorkItem(WorkItem);
        //}

        private static List<T> Query<T>(List<T> TEntitySet) where T : class
        {
            List<T> query = new List<T>();
            if (TEntitySet.Count() > 0)
            {
                T Entity = TEntitySet[0];
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                using (LAS_TWEntities DB = new LAS_TWEntities())
                {
                    try
                    {
                        DB.Configuration.ValidateOnSaveEnabled = false;
                        DbSet<T> EntitySet = DB.Set<T>();
                        ObjectContext objectContext = ((IObjectContextAdapter)DB).ObjectContext;
                        ObjectSet<T> set = objectContext.CreateObjectSet<T>();
                        string tableName = set.EntitySet.ElementType.Name;
                        List<EdmMember> members = set.EntitySet.ElementType.Members.ToList();
                        List<string> keyNames = set.EntitySet.ElementType.KeyMembers.Select(k => k.Name).ToList<string>();
                        StringBuilder sSQLBuilder = new StringBuilder("SELECT * FROM " + tableName + " where ");
                        foreach (EdmMember member in members)
                        {
                            if (DB.Entry(Entity).Property(member.Name).CurrentValue != null && (!DB.Entry(Entity).Property(member.Name).CurrentValue.ToString().Equals("") && !DB.Entry(Entity).Property(member.Name).CurrentValue.ToString().Equals("0")))
                            {
                                sSQLBuilder.Append(member.Name + "=@" + member.Name + " and ");
                                SqlParameters.Add(new SqlParameter(member.Name, DB.Entry(Entity).Property(member.Name).CurrentValue));
                            }
                        }
                        string sSQL = sSQLBuilder.ToString();
                        if (sSQL.EndsWith("where "))
                        {
                            sSQL = sSQL.Substring(0, sSQL.Length - "where ".Length);
                        }
                        if (sSQL.EndsWith("and "))
                        {
                            sSQL = sSQL.Substring(0, sSQL.Length - "and ".Length);
                        }
                        //var query = EntitySet.SqlQuery(sSQL, SqlParameters.ToArray()).ToList<T>();
                        query = DB.Database.SqlQuery<T>(sSQL, SqlParameters.ToArray()).ToList();
                    }
                    catch (Exception ex)
                    {
                        if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                    }
                }
            }
            return query;
        }

        private static void Update<T>(List<T> TEntitySet) where T : class
        {
            using (LAS_TWEntities DB = new LAS_TWEntities())
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DB.Configuration.ValidateOnSaveEnabled = false;
                DbSet<T> EntitySet = DB.Set<T>();
                foreach (T Entity in TEntitySet)
                {
                    EntitySet.Attach(Entity);
                    DB.Entry(Entity).State = EntityState.Modified;
                }
                try
                {
                    DB.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                }
                DB.Configuration.AutoDetectChangesEnabled = true;
                DB.Configuration.ValidateOnSaveEnabled = true;

            }
        }

        private static void Add<T>(List<T> TEntitySet) where T : class
        {
            using (LAS_TWEntities DB = new LAS_TWEntities())
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DB.Configuration.ValidateOnSaveEnabled = false;
                DbSet<T> EntitySet = DB.Set<T>();
                EntitySet.AddRange(TEntitySet);
                try
                {
                    DB.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                }
                EntitySet.RemoveRange(TEntitySet);
                DB.Configuration.AutoDetectChangesEnabled = true;
                DB.Configuration.ValidateOnSaveEnabled = true;

            }
        }

        private static void Delete<T>(List<T> TEntitySet) where T : class
        {
            using (LAS_TWEntities DB = new LAS_TWEntities())
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DB.Configuration.ValidateOnSaveEnabled = false;
                DbSet<T> EntitySet = DB.Set<T>();
                foreach (T Entity in TEntitySet)
                {
                    EntitySet.Attach(Entity);
                    DB.Entry(Entity).State = EntityState.Deleted;
                }
                try
                {
                    DB.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                }
                DB.Configuration.AutoDetectChangesEnabled = true;
                DB.Configuration.ValidateOnSaveEnabled = true;

            }
        }

        /// <summary>
        /// 註冊WorkThreads內的事件
        /// </summary>
        public static void RegisterWorkThreadsEvent()
        {
            WorkThreads.DoDBTaskFinished += new WorkThreads.DoDBTaskFinishedEventHandler(WorkThreads_DoDBTaskFinished);
            WorkThreads.ConvertTagToMessageFinished += new WorkThreads.ConvertTagToMessageFinishedEventHandler(WorkThreads_ConvertTagToMessageFinished);
            WorkThreads.SendMessageToClientFinished += new WorkThreads.SendMessageToClientFinishedEventHandler(WorkThreads_SendMessageToClientFinished);
            WorkThreads.WorkThreadsErrorHappened += new WorkThreads.WorkThreadsErrorHappenedEventHandler(WorkThreads_WorkThreadsErrorHappened);
        }
        /// <summary>
        /// 移除WorkThreads內的事件
        /// </summary>
        public static void UnregisterWorkThreadsEvent()
        {
            WorkThreads.DoDBTaskFinished -= WorkThreads_DoDBTaskFinished;
            WorkThreads.ConvertTagToMessageFinished -= WorkThreads_ConvertTagToMessageFinished;
            WorkThreads.SendMessageToClientFinished -= WorkThreads_SendMessageToClientFinished;
            WorkThreads.WorkThreadsErrorHappened -= WorkThreads_WorkThreadsErrorHappened;
        }
        private static string GetRequestID(DataTable ListMessageData)
        {
            string RequestID = "";
            if (ListMessageData.Rows.Count > 0)
            {
                if (ListMessageData.Columns.Contains("MessageID"))
                {
                    RequestID = ListMessageData.Rows[0]["MessageID"].ToString();
                }
            }
            return RequestID;
        }
        static void WorkThreads_DoDBTaskFinished(object sender, WorkThreadsEventArgs<DataTable> e)
        {
            try
            {
                string RequestID = GetRequestID(e.ListMessageData);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = string.Format("{0}", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ffff"));
                if (!RequestID.Equals(""))
                {
                    //if (log.IsInfoEnabled) log.InfoFormat("{0}(Request ID:{1}) DoDBTaskFinished(Count:{2})", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.ListMessageData.Rows.Count);
                    lvi.SubItems.Add(string.Format("{0}(Request ID:{1}) DoDBTaskFinished(Count:{2})", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.ListMessageData.Rows.Count));
                }
                else
                {
                    //if (log.IsInfoEnabled) log.InfoFormat("{0} DoDBTaskFinished(Count:{1})", Enum.GetName(typeof(TopicType), e.TopicType), e.ListMessageData.Rows.Count);
                    lvi.SubItems.Add(string.Format("{0} DoDBTaskFinished(Count:{1})", Enum.GetName(typeof(TopicType), e.TopicType), e.ListMessageData.Rows.Count));
                }
                ExecuteListView(lvi, e.CurrentSendAmounts);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }
        static void WorkThreads_ConvertTagToMessageFinished(object sender, WorkThreadsEventArgs<DataTable> e)
        {
            try
            {
                string RequestID = GetRequestID(e.ListMessageData);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = string.Format("{0}", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ffff"));
                if (!RequestID.Equals(""))
                {
                    //if (log.IsInfoEnabled) log.InfoFormat("{0}(Request ID:{1}) ConvertTagToMessageFinished(Count:{2})", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.ListMessageData.Rows.Count);
                    lvi.SubItems.Add(string.Format("{0}(Request ID:{1}) ConvertTagToMessageFinished(Count:{2})", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.ListMessageData.Rows.Count));
                }
                else
                {
                    //if (log.IsInfoEnabled) log.InfoFormat("{0} ConvertTagToMessageFinished(Count:{1})", Enum.GetName(typeof(TopicType), e.TopicType), e.ListMessageData.Rows.Count);
                    lvi.SubItems.Add(string.Format("{0} ConvertTagToMessageFinished(Count:{1})", Enum.GetName(typeof(TopicType), e.TopicType), e.ListMessageData.Rows.Count));
                }
                ExecuteListView(lvi, e.CurrentSendAmounts);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }
        static void WorkThreads_SendMessageToClientFinished(object sender, WorkThreadsEventArgs<DataTable> e)
        {
            try
            {
                string RequestID = GetRequestID(e.ListMessageData);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = string.Format("{0}", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ffff"));
                if (!RequestID.Equals(""))
                {
                    //if (log.IsInfoEnabled) log.InfoFormat("{0}(Request ID:{1}) SendMessageToClientFinished(Count:{2})", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.ListMessageData.Rows.Count);
                    lvi.SubItems.Add(string.Format("{0}(Request ID:{1}) SendMessageToClientFinished(Count:{2})", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.ListMessageData.Rows.Count));
                }
                else
                {
                    //if (log.IsInfoEnabled) log.InfoFormat("{0} SendMessageToClientFinished(Count:{1})", Enum.GetName(typeof(TopicType), e.TopicType), e.ListMessageData.Rows.Count);
                    lvi.SubItems.Add(string.Format("{0} SendMessageToClientFinished(Count:{1})", Enum.GetName(typeof(TopicType), e.TopicType), e.ListMessageData.Rows.Count));
                }
                ExecuteListView(lvi, e.CurrentSendAmounts);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }
        static void WorkThreads_WorkThreadsErrorHappened(object sender, WorkThreadsErrorHappenedEventArgs<DataTable> e)
        {
            try
            {
                string RequestID = GetRequestID(e.ListMessageData);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = string.Format("{0}", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ffff"));
                if (!RequestID.Equals(""))
                {
                    if (log.IsInfoEnabled) log.InfoFormat("{0}(Request ID:{1}) WorkThreadsErrorHappened Error Message:{2}", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.errorMessage);
                    lvi.SubItems.Add(string.Format("{0}(Request ID:{1}) WorkThreadsErrorHappened Error Message:{2}", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.errorMessage));
                }
                else
                {
                    if (log.IsInfoEnabled) log.InfoFormat("{0}(Request ID:{1}) WorkThreadsErrorHappened Error Message:{2}", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.errorMessage);
                    lvi.SubItems.Add(string.Format("{0}(Request ID:{1}) WorkThreadsErrorHappened Error Message:{2}", Enum.GetName(typeof(TopicType), e.TopicType), RequestID, e.errorMessage));
                }
                ExecuteListView(lvi, e.CurrentSendAmounts);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        static void ExecuteListView(ListViewItem lvi, int CurrentSendAmounts)
        {
            if (_MQServer_Receive_Response_Test != null)
            {
                _MQServer_Receive_Response_Test.LvSystemInfo.Items.Add(lvi);
                _MQServer_Receive_Response_Test.txtMessageCount.Text = _MQServer_Receive_Response_Test.LvSystemInfo.Items.Count.ToString();
                //if (_MQServer_Receive_Response_Test.LvSystemInfo.Items.Count > 10000 && DateTime.Now.Second == 0)
                //{
                //    _MQServer_Receive_Response_Test.LvSystemInfo.Items.Clear();
                //}
            }
            //if (_TibcoServer_Receive_Response_Test != null) 
            //{ 
            //    _TibcoServer_Receive_Response_Test.LvSystemInfo.Items.Add(lvi);
            //    if (_TibcoServer_Receive_Response_Test.LvSystemInfo.Items.Count > 10000 && DateTime.Now.Second == 0)
            //    {
            //        _TibcoServer_Receive_Response_Test.LvSystemInfo.Items.Clear();
            //    }
            //}
            if (_EMSServer_Receive_Response_Test != null)
            {
                _EMSServer_Receive_Response_Test.LvSystemInfo.Items.Add(lvi);
                _EMSServer_Receive_Response_Test.txtMessageCount.Text = _EMSServer_Receive_Response_Test.LvSystemInfo.Items.Count.ToString();
                //if (_EMSServer_Receive_Response_Test.LvSystemInfo.Items.Count > 10000 && DateTime.Now.Second == 0)
                //{
                //    _EMSServer_Receive_Response_Test.LvSystemInfo.Items.Clear();
                //}
            }

            //if (CurrentSendAmounts >= 10000 && DateTime.Now.Second == 0)
            //{
            //    if (_MQServer_Receive_Response_Test != null) { _MQServer_Receive_Response_Test.LvSystemInfo.Items.Clear(); }
            //    if (_TibcoServer_Receive_Response_Test != null) { _TibcoServer_Receive_Response_Test.LvSystemInfo.Items.Clear(); }
            //    if (_EMSServer_Receive_Response_Test != null) { _EMSServer_Receive_Response_Test.LvSystemInfo.Items.Clear(); }
            //}
        }
        public static bool InsertUser(comUser NewUser)
        {
            bool result = false;
            SpecificEntityRepository<comUser> Entities = new SpecificEntityRepository<comUser>(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
            result = Entities.Add(NewUser);
            return result;
        }
        public static int GetCount<T>() where T : class
        {
            SpecificEntityRepository<T> Entities = new SpecificEntityRepository<T>(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
            return Entities.GetCount();
        }
        private static void CertificateDataVerifySaveDBTest<T>(DataTable MessageDT) where T : class
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            string JSONresult = JsonConvert.SerializeObject(MessageDT);
            //對android傳來的簽章資料驗章
            if (typeof(T) == typeof(SignMessage))
            {
                List<SignMessage> SignMessage = JsonConvert.DeserializeObject<List<SignMessage>>(JSONresult, settings);
                bool IsOK = CipherHelper.RsaVerifyData(DecEncCode.AESDecrypt(SignMessage[0].cipherText), SignMessage[0].sign, SignMessage[0].publickey);
            }
            //對android傳來的簽章資料驗章
            //分散式帳本模型資料新增test(使用pub/sub) begin
            int userCount = TopicController.GetCount<comUser>();
            comUser newUser = new comUser();
            newUser.user_id = "katepan" + userCount;
            newUser.country = "TWN";
            newUser.first_name = "Kate";
            newUser.last_name = "Pan";
            newUser.dept = "CEO";
            newUser.role = "GEN";
            newUser.password = "F9-EC-8F-24-07-18-B8-C5-AD-71-6B-6F-E9-4D-14-E6";
            newUser.active = true;
            newUser.show_report = true;
            newUser.access_report = true;
            newUser.access_print = true;
            newUser.limited_user = false;
            newUser.advanced_user = true;
            newUser.last_upd_user = "leonlee";
            newUser.last_upd_date = DateTime.Now;
            bool result = TopicController.InsertUser(newUser);
            //分散式帳本模型資料新增test(使用pub/sub) end
        }
    }
}
