﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess
{
    /// <summary>
    /// 靜態化資料庫物件模型存取類別
    /// </summary>
    public static class StaticCommonEntityRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DbContext _DbContext;
        /// <summary>
        /// 初始化資料庫環境物件(使用其他存取方法前須先執行取初始化作業)
        /// </summary>
        /// <param name="DbContext"></param>
        public static void InitDBContext(DbContext DbContext)
        {
            _DbContext = DbContext;
        }
        /// <summary>
        /// 取得某資料表物件模型的資料筆數
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetCount<T>() where T : class
        {
            DbSet<T> _dbset = _DbContext.Set<T>();
            return _dbset.AsQueryable().Count();
        }
        /// <summary>
        /// 取得符合物件模型篩選條件的資料筆數(以指定語述詞方式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int GetCount<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            DbSet<T> _dbset = _DbContext.Set<T>();
            return _dbset.Where(predicate).Count();
        }
        /// <summary>
        /// 取得某資料表物件模型所有資料並排序(排序為選擇性參數)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAll<T>(params Expression<Func<T, IComparable>>[] SortKeys) where T : class
        {
            DbSet<T> _dbset = _DbContext.Set<T>();
            List<T> query = _dbset.AsEnumerable().ToList();
            try
            {
                if (SortKeys != null && SortKeys.Any())
                {
                    query.SortBy<T>(SortKeys);
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return query;
        }
        /// <summary>
        /// 取得某資料表物件模型所有資料並排序(排序為選擇性參數)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSort"></param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string strSort = null) where T : class
        {
            DbSet<T> _dbset = _DbContext.Set<T>();
            List<T> query = _dbset.AsEnumerable().ToList();
            try
            {
                if (strSort != null && strSort.Any())
                {
                    query = query.ApplySort(strSort);
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return query;
        }
        /// <summary>
        /// 以指定陳述詞方式尋找符合物件模型篩選條件的資料並排序(排序為選擇性參數)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<T> Find<T>(Expression<Func<T, bool>> WherePredicate, params Expression<Func<T, IComparable>>[] SortKeys) where T : class
        {
            DbSet<T> dbset = _DbContext.Set<T>();
            List<T> query = dbset.Where(WherePredicate).AsEnumerable().ToList();
            try
            {
                if (SortKeys != null && SortKeys.Any())
                {
                    query.SortBy<T>(SortKeys);
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return query;
        }
        /// <summary>
        /// 以指定陳述詞方式尋找符合物件模型篩選條件的資料並排序(排序為選擇性參數)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WherePredicate"></param>
        /// <param name="strSort"></param>
        /// <returns></returns>
        public static List<T> Find<T>(Expression<Func<T, bool>> WherePredicate, string strSort = null) where T : class
        {
            DbSet<T> dbset = _DbContext.Set<T>();
            List<T> query = dbset.Where(WherePredicate).AsEnumerable().ToList();
            try
            {
                if (strSort != null && strSort.Any())
                {
                    query = query.ApplySort(strSort);
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return query;
        }
        /// <summary>
        /// 以設定物件模型欄位值方式尋找符合物件模型篩選條件的資料並排序(排序為選擇性參數)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TEntitySet"></param>
        /// <returns></returns>
        public static List<T> Find<T>(List<T> TEntitySet, params Expression<Func<T, IComparable>>[] SortKeys) where T : class
        {
            List<T> query = new List<T>();
            if (TEntitySet.Count() > 0)
            {
                T Entity = TEntitySet[0];
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                try
                {
                    _DbContext.Configuration.ValidateOnSaveEnabled = false;
                    DbSet<T> dbset = _DbContext.Set<T>();
                    ObjectContext objectContext = ((IObjectContextAdapter)_DbContext).ObjectContext;
                    ObjectSet<T> set = objectContext.CreateObjectSet<T>();
                    string tableName = set.EntitySet.ElementType.Name;
                    List<EdmMember> members = set.EntitySet.ElementType.Members.ToList();
                    StringBuilder sSQLBuilder = new StringBuilder("SELECT * FROM " + tableName + " where ");
                    foreach (EdmMember member in members)
                    {
                        if (_DbContext.Entry(Entity).Property(member.Name).CurrentValue != null && (!_DbContext.Entry(Entity).Property(member.Name).CurrentValue.ToString().Equals("") && !_DbContext.Entry(Entity).Property(member.Name).CurrentValue.ToString().Equals("0")))
                        {
                            sSQLBuilder.Append(member.Name + "=@" + member.Name + " and ");
                            SqlParameters.Add(new SqlParameter(member.Name, _DbContext.Entry(Entity).Property(member.Name).CurrentValue));
                        }
                    }
                    string sSQL = sSQLBuilder.ToString();
                    if (sSQL.EndsWith(" where "))
                    {
                        sSQL = sSQL.Substring(0, sSQL.Length - " where ".Length);
                    }
                    if (sSQL.EndsWith(" and "))
                    {
                        sSQL = sSQL.Substring(0, sSQL.Length - " and ".Length);
                    }
                    //query = _DbContext.Database.SqlQuery<T>(sSQL, SqlParameters.ToArray()).ToList();
                    query = _DbContext.Set<T>().SqlQuery(sSQL, SqlParameters.ToArray()).ToList();
                    if (SortKeys != null && SortKeys.Any())
                    {
                        query.SortBy<T>(SortKeys);
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                    throw ex;
                }
            }
            return query;
        }
        /// <summary>
        /// 以設定物件模型欄位值方式尋找符合物件模型篩選條件的資料並排序(排序為選擇性參數)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TEntitySet"></param>
        /// <param name="strSort"></param>
        /// <returns></returns>
        public static List<T> Find<T>(List<T> TEntitySet, string strSort = null) where T : class
        {
            List<T> query = new List<T>();
            if (TEntitySet.Count() > 0)
            {
                T Entity = TEntitySet[0];
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                try
                {
                    _DbContext.Configuration.ValidateOnSaveEnabled = false;
                    DbSet<T> dbset = _DbContext.Set<T>();
                    ObjectContext objectContext = ((IObjectContextAdapter)_DbContext).ObjectContext;
                    ObjectSet<T> set = objectContext.CreateObjectSet<T>();
                    string tableName = set.EntitySet.ElementType.Name;
                    List<EdmMember> members = set.EntitySet.ElementType.Members.ToList();
                    StringBuilder sSQLBuilder = new StringBuilder("SELECT * FROM " + tableName + " where ");
                    foreach (EdmMember member in members)
                    {
                        if (_DbContext.Entry(Entity).Property(member.Name).CurrentValue != null && (!_DbContext.Entry(Entity).Property(member.Name).CurrentValue.ToString().Equals("") && !_DbContext.Entry(Entity).Property(member.Name).CurrentValue.ToString().Equals("0")))
                        {
                            sSQLBuilder.Append(member.Name + "=@" + member.Name + " and ");
                            SqlParameters.Add(new SqlParameter(member.Name, _DbContext.Entry(Entity).Property(member.Name).CurrentValue));
                        }
                    }
                    string sSQL = sSQLBuilder.ToString();
                    if (sSQL.EndsWith(" where "))
                    {
                        sSQL = sSQL.Substring(0, sSQL.Length - " where ".Length);
                    }
                    if (sSQL.EndsWith(" and "))
                    {
                        sSQL = sSQL.Substring(0, sSQL.Length - " and ".Length);
                    }
                    //query = _DbContext.Database.SqlQuery<T>(sSQL, SqlParameters.ToArray()).ToList();
                    query = _DbContext.Set<T>().SqlQuery(sSQL, SqlParameters.ToArray()).ToList();
                    if (strSort != null && strSort.Any())
                    {
                        query = query.ApplySort(strSort);
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                    throw ex;
                }
            }
            return query;
        }
        /// <summary>
        /// 下一段Select SQL指令尋找符合物件模型篩選條件的資料(以帶SQL含參數指令方式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static List<T> Find<T>(string Sql, Dictionary<string, object> Parameters) where T : class
        {
            List<T> query = new List<T>();
            using (var cmd = _DbContext.Database.Connection.CreateCommand())
            {
                try
                {
                    if (CheckQueryStatement(new string[] { Sql }))
                    {
                        cmd.CommandText = Sql;
                        foreach (KeyValuePair<string, object> param in Parameters)
                        {
                            DbParameter dbParameter = cmd.CreateParameter();
                            dbParameter.ParameterName = param.Key;
                            dbParameter.Value = param.Value;
                            cmd.Parameters.Add(dbParameter);
                        }
                        SqlParameter[] Params = cmd.Parameters.Cast<SqlParameter>().ToArray();
                        cmd.Parameters.Clear();
                        //query = _DbContext.Database.SqlQuery<T>(cmd.CommandText, Params).ToList();
                        query = _DbContext.Set<T>().SqlQuery(cmd.CommandText, Params).ToList();
                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                    throw ex;
                }
            }
            return query;
        }
        /// <summary>
        /// 下多個Select SQL指令尋找符合物件模型篩選條件的資料(以帶SQL含參數指令方式)
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="ParametersList"></param>
        /// <param name="RecordSetNum"></param>
        /// <param name="OutTypes"></param>
        /// <returns></returns>
        public static IList FindMultiRecordSet(string Sql, Dictionary<string, object> ParametersList, int RecordSetNum, Type[] OutTypes)
        {
            //dynamic[] AllQuerys = new dynamic[] { };
            List<object> AllQuerys = new List<object>();
            try
            {
                Sql = Sql.EndsWith(";") ? Sql.Substring(0, Sql.Length - 1) : Sql;
                string[] arrySql = Sql.Split(';');
                if (arrySql.Length == RecordSetNum && RecordSetNum == OutTypes.Length && CheckQueryStatement(arrySql))
                {
                    //AllQuerys = new dynamic[RecordSetNum];
                    _DbContext.Database.Connection.Open();
                    DbCommand cmd = _DbContext.Database.Connection.CreateCommand();
                    cmd.CommandText = Sql;
                    cmd.CommandType = CommandType.Text;
                    foreach (KeyValuePair<string, object> param in ParametersList)
                    {
                        DbParameter inParameter = cmd.CreateParameter();
                        inParameter.ParameterName = param.Key;
                        inParameter.Value = param.Value;
                        cmd.Parameters.Add(inParameter);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        for (int i = 0; i < RecordSetNum; i++)
                        {
                            //AllQuerys[i] = Util.DataReaderToObjectList(OutTypes[i], reader);
                            AllQuerys.Add(Util.DataReaderToObjectList(OutTypes[i], reader));
                            reader.NextResult();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _DbContext.Database.Connection.Close();
            }
            return AllQuerys;
        }
        /// <summary>
        /// 執行無回傳資料集的預存程序(依預存程序需求帶入輸入輸出參數及預存程序回傳值)
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="OutValue"></param>
        /// <param name="InParameters"></param>
        /// <param name="OutParameters"></param>
        public static void ExecuteProcedure(string ProcedureName, out object OutValue, Dictionary<string, object> InParameters = null, Dictionary<string, object> OutParameters = null)
        {
            OutValue = 0;
            //Had to go this route since EF Code First doesn't support output parameters
            //returned from sprocs very well at this point
            try
            {
                _DbContext.Database.Connection.Open();
                DbCommand cmd = _DbContext.Database.Connection.CreateCommand();
                cmd.CommandText = ProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                if (InParameters != null)
                {
                    foreach (KeyValuePair<string, object> param in InParameters)
                    {
                        DbParameter inParameter = cmd.CreateParameter();
                        inParameter.ParameterName = param.Key;
                        inParameter.Value = param.Value;
                        cmd.Parameters.Add(inParameter);
                    }
                }
                var outParam = cmd.CreateParameter();
                if (OutParameters != null && OutParameters.Count() == 1)
                {
                    foreach (KeyValuePair<string, object> param in OutParameters)
                    {
                        outParam.ParameterName = param.Key;
                        outParam.Value = param.Value;
                        outParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outParam);
                    }
                }
                var reader = cmd.ExecuteReader();
                //Access output variable after reader is closed
                OutValue = (outParam.Value == null) ? outParam.Value : Convert.ToInt32(outParam.Value);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _DbContext.Database.Connection.Close();
            }
        }
        /// <summary>
        /// 執行回傳單一資料集的預存程序(依預存程序需求帶入輸入輸出參數及預存程序回傳值)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ProcedureName"></param>
        /// <param name="OutValue"></param>
        /// <param name="InParameters"></param>
        /// <param name="OutParameters"></param>
        /// <returns></returns>
        public static List<T> ExecuteProcedure<T>(string ProcedureName, out object OutValue, Dictionary<string, object> InParameters = null, Dictionary<string, object> OutParameters = null) where T : new()
        {
            List<T> query = new List<T>();
            OutValue = 0;
            //Had to go this route since EF Code First doesn't support output parameters
            //returned from sprocs very well at this point
            try
            {
                _DbContext.Database.Connection.Open();
                DbCommand cmd = _DbContext.Database.Connection.CreateCommand();
                cmd.CommandText = ProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                if (InParameters != null)
                {
                    foreach (KeyValuePair<string, object> param in InParameters)
                    {
                        DbParameter inParameter = cmd.CreateParameter();
                        inParameter.ParameterName = param.Key;
                        inParameter.Value = param.Value;
                        cmd.Parameters.Add(inParameter);
                    }
                }
                var outParam = cmd.CreateParameter();
                if (OutParameters != null && OutParameters.Count() == 1)
                {
                    foreach (KeyValuePair<string, object> param in OutParameters)
                    {
                        outParam.ParameterName = param.Key;
                        outParam.Value = param.Value;
                        outParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outParam);
                    }
                }
                using (var reader = cmd.ExecuteReader())
                {
                    query = Util.DataReaderToObjectList<T>(reader);
                }
                //Access output variable after reader is closed
                OutValue = (outParam.Value == null) ? outParam.Value : Convert.ToInt32(outParam.Value);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _DbContext.Database.Connection.Close();
            }
            return query;
        }
        /// <summary>
        /// 執行回傳多個資料集的預存程序(依預存程序需求帶入輸入輸出參數及預存程序回傳值)
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="RecordSetNum"></param>
        /// <param name="OutTypes"></param>
        /// <param name="OutValue"></param>
        /// <param name="InParameters"></param>
        /// <param name="OutParameters"></param>
        /// <returns></returns>
        public static IList ExecuteProcedure(string ProcedureName, int RecordSetNum, Type[] OutTypes, out object OutValue, Dictionary<string, object> InParameters = null, Dictionary<string, object> OutParameters = null)
        {
            OutValue = 0;
            //dynamic[] AllQuerys = new dynamic[] { };
            List<object> AllQuerys = new List<object>();
            //Had to go this route since EF Code First doesn't support output parameters
            //returned from sprocs very well at this point
            try
            {
                if (RecordSetNum == OutTypes.Length)
                {
                    //AllQuerys = new dynamic[RecordSetNum];
                    _DbContext.Database.Connection.Open();
                    DbCommand cmd = _DbContext.Database.Connection.CreateCommand();
                    cmd.CommandText = ProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (InParameters != null)
                    {
                        foreach (KeyValuePair<string, object> param in InParameters)
                        {
                            DbParameter inParameter = cmd.CreateParameter();
                            inParameter.ParameterName = param.Key;
                            inParameter.Value = param.Value;
                            cmd.Parameters.Add(inParameter);
                        }
                    }
                    var outParam = cmd.CreateParameter();
                    if (OutParameters != null && OutParameters.Count() == 1)
                    {
                        foreach (KeyValuePair<string, object> param in OutParameters)
                        {
                            outParam.ParameterName = param.Key;
                            outParam.Value = param.Value;
                            outParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(outParam);
                        }
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        for (int i = 0; i < RecordSetNum; i++)
                        {
                            //AllQuerys[i] = Util.DataReaderToObjectList(OutTypes[i], reader);
                            AllQuerys.Add(Util.DataReaderToObjectList(OutTypes[i], reader));
                            reader.NextResult();
                        }
                    }
                    //Access output variable after reader is closed
                    OutValue = (outParam.Value == null) ? outParam.Value : Convert.ToInt32(outParam.Value);
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _DbContext.Database.Connection.Close();
            }
            return AllQuerys;
        }
        public static bool Add<T>(T entity) where T : class
        {
            bool result = false;
            DbSet<T> dbset = _DbContext.Set<T>();
            try
            {
                dbset.Add(entity);
                _DbContext.SaveChanges();
                result = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return result;
        }
        public static bool Add<T>(List<T> TEntitySet) where T : class
        {
            bool result = false;
            _DbContext.Configuration.AutoDetectChangesEnabled = false;
            _DbContext.Configuration.ValidateOnSaveEnabled = false;
            DbSet<T> dbset = _DbContext.Set<T>();
            dbset.AddRange(TEntitySet);
            try
            {
                _DbContext.SaveChanges();
                result = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            _DbContext.Configuration.AutoDetectChangesEnabled = true;
            _DbContext.Configuration.ValidateOnSaveEnabled = true;
            return result;
        }
        public static bool Update<T>(T entity) where T : class
        {
            bool result = false;
            DbSet<T> dbset = _DbContext.Set<T>();
            dbset.Attach(entity);
            _DbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            try
            {
                _DbContext.SaveChanges();
                result = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return result;
        }
        public static bool Update<T>(List<T> TEntitySet) where T : class
        {
            bool result = false;
            _DbContext.Configuration.AutoDetectChangesEnabled = false;
            _DbContext.Configuration.ValidateOnSaveEnabled = false;
            DbSet<T> dbset = _DbContext.Set<T>();
            foreach (T Entity in TEntitySet)
            {
                dbset.Attach(Entity);
                _DbContext.Entry(Entity).State = EntityState.Modified;
            }
            try
            {
                _DbContext.SaveChanges();
                result = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            _DbContext.Configuration.AutoDetectChangesEnabled = true;
            _DbContext.Configuration.ValidateOnSaveEnabled = true;
            return result;
        }
        public static bool Delete<T>(T entity) where T : class
        {
            bool result = false;
            DbSet<T> dbset = _DbContext.Set<T>();
            dbset.Attach(entity);
            _DbContext.Entry(entity).State = EntityState.Deleted;
            try
            {
                _DbContext.SaveChanges();
                result = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return result;
        }
        public static bool Delete<T>(List<T> TEntitySet) where T : class
        {
            bool result = false;
            _DbContext.Configuration.AutoDetectChangesEnabled = false;
            _DbContext.Configuration.ValidateOnSaveEnabled = false;
            DbSet<T> dbset = _DbContext.Set<T>();
            foreach (T Entity in TEntitySet)
            {
                dbset.Attach(Entity);
                _DbContext.Entry(Entity).State = EntityState.Deleted;
            }
            try
            {
                _DbContext.SaveChanges();
                result = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            _DbContext.Configuration.AutoDetectChangesEnabled = true;
            _DbContext.Configuration.ValidateOnSaveEnabled = true;
            return result;
        }
        public static bool CheckPKExist<T>(Dictionary<string, object> PkValueList) where T : class
        {
            bool isPKExist = false;
            List<SqlParameter> SqlParameters = new List<SqlParameter>();
            try
            {
                DbSet<T> dbset = _DbContext.Set<T>();
                ObjectContext objectContext = ((IObjectContextAdapter)_DbContext).ObjectContext;
                ObjectSet<T> set = objectContext.CreateObjectSet<T>();
                string tableName = set.EntitySet.ElementType.Name;
                List<EdmMember> PkMembers = set.EntitySet.ElementType.KeyMembers.ToList();

                if (CheckPrimaryKeyIdentity(PkMembers.Select(r => r.Name.ToLowerInvariant()).ToArray(), PkValueList.Keys.Select(r => r.ToLowerInvariant()).ToArray()))
                {
                    StringBuilder sSQLBuilder = new StringBuilder("SELECT COUNT(*) FROM " + tableName + " where ");
                    foreach (KeyValuePair<string, object> param in PkValueList)
                    {
                        sSQLBuilder.Append(param.Key + "=@" + param.Key + " and ");
                        SqlParameters.Add(new SqlParameter(param.Key, param.Value));
                    }
                    string sSQL = sSQLBuilder.ToString();
                    if (sSQL.EndsWith(" where "))
                    {
                        sSQL = sSQL.Substring(0, sSQL.Length - " where ".Length);
                    }
                    if (sSQL.EndsWith(" and "))
                    {
                        sSQL = sSQL.Substring(0, sSQL.Length - " and ".Length);
                    }
                    isPKExist = _DbContext.Database.SqlQuery<int>(sSQL, SqlParameters.ToArray()).First() > 0 ? true : false;
                }
                else
                {
                    isPKExist = false;
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                throw ex;
            }
            return isPKExist;
        }
        /// <summary>
        /// 檢查多個SQL指令是否都是查詢語法
        /// </summary>
        /// <param name="Sqls"></param>
        /// <returns></returns>
        private static bool CheckQueryStatement(string[] Sqls)
        {
            bool result = true;
            foreach (string Sql in Sqls)
            {
                if (!Sql.Trim().ToLower().StartsWith("select "))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        private static bool CheckPrimaryKeyIdentity(string[] FirstPKs, string[] SecondPKs)
        {
            bool result = false;
            if (FirstPKs.Length != SecondPKs.Length)
            {
                result = false;
            }
            else
            {
                result = FirstPKs.SequenceEqual(SecondPKs);
            }

            return result;
        }
    }
}
