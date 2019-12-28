using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using MySql.Data.MySqlClient;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

/// <summary>
/// ��Ʀs�������
/// </summary>
namespace DataAccess.DB
{
    [Serializable()]
    public class Dac : IDisposable
    {
        private dbkind Datatype;
        private IDbConnection DbCon;
        private IDbCommand Cmd;
        private IDbDataParameter Para;
        private IDataAdapter IDa;
        private int _CommandTimeOut = 600;
        private string _ConnectionConfigName = "Default";
        private SqlDatabase db;

        /// <summary>
        /// ����SQL���OTimeOut���ɶ�(��)
        /// </summary>
        public int CommandTimeOut
        {
            get
            {
                return _CommandTimeOut;
            }
            set
            {
                _CommandTimeOut = value;
            }
        }
        /// <summary>
        /// �պA�ɸ�Ʈw�s�u�W��
        /// </summary>
        public string ConnectionConfigName
        {
            get
            {
                return _ConnectionConfigName;
            }
            set
            {
                _ConnectionConfigName = value;
            }
        }

        public Dac(dbkind dbkind)
        {
            Setdbkind(dbkind);
        }

        public Dac(dbkind dbkind, string ConnectionConfigName)
        {
            Setdbkind(dbkind);
            this._ConnectionConfigName = ConnectionConfigName;
        }

        private void Setdbkind(dbkind dbkind)
        {
            Datatype = dbkind;
            if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
            {
                DbCon = new OleDbConnection();
                Cmd = new OleDbCommand();
                Para = new OleDbParameter();
                IDa = new OleDbDataAdapter();
            }
            else if (dbkind == dbkind.SQL_Server)
            {
                DbCon = new SqlConnection();
                Cmd = new SqlCommand();
                Para = new SqlParameter();
                IDa = new SqlDataAdapter();
            }
            else if (dbkind == dbkind.Oracle)
            {
                DbCon = new OracleConnection();
                Cmd = new OracleCommand();
                (Cmd as OracleCommand).FetchSize = (Cmd as OracleCommand).FetchSize * 8;
                Para = new OracleParameter();
                IDa = new OracleDataAdapter();
            }
            else if (dbkind == dbkind.MySql)
            {
                DbCon = new MySqlConnection();
                Cmd = new MySqlCommand();
                Para = new MySqlParameter();
                IDa = new MySqlDataAdapter();
            }
        }
        /// <summary>
        /// �}�Ҹ�Ʈw
        /// </summary>
        /// <param name="ConnectionStr">���oconnection string</param>
        private void Open(string ConnectionStr)
        {
            if (ConnectionStr == "") { ConnectionStr = ConfigurationManager.ConnectionStrings[this.ConnectionConfigName].ConnectionString; }
            DbCon.ConnectionString = ConnectionStr;
            db = new SqlDatabase(DbCon.ConnectionString);
            DbCon.Open();
        }

        /// <summary>
        /// <see cref="Dac"/> class�غc�l
        /// </summary>
        /// <param name="UserInfo">The user info.</param>
        //public Dac(UserInformation UserInfo)
        //{
        //  userInfo=UserInfo;
        //}


        /// <summary>
        /// ������Ʈw�s�u
        /// </summary>
        private void CloseDB()
        {
            if (DbCon.State == ConnectionState.Open)
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// �����Command����,�å[�J��ƫ��O��Return Value(�D�n�Ω����w�x�{�ǩҼv�T���C��)
        /// </summary>
        /// <example>
        /// �H�U���ϥ�BuildIntCommand���d�ҵ{���G
        /// <code>
        /// // Create parameter array
        ///	SqlParameter[] parameters =
        ///	{
        ///		new SqlParameter( "@LOGIN_ID", Sqldbkind.NVarChar, 50 ),	// 0
        ///		new SqlParameter( "@DOMAIN", Sqldbkind.NVarChar, 50 ),	// 1
        ///	};
        ///	
        ///	// Set parameter values and directions
        ///	parameters[ 0 ].Value = LOGINID;
        ///	parameters[ 1 ].Value = DOMAIN;
        ///
        ///	SqlCommand cmd =BuildIntCommand("spCustomProcedure",parameters);
        /// </code>
        /// </example>
        /// <param name="StoredProcName">�w�s�{�ǦW��</param>
        /// <param name="Parameters">�w�s�{�Ǫ��Ѽư}�C</param>
        /// <returns></returns>
        private IDbCommand BuildIntCommand(string StoredProcName, IDataParameter[] Parameters, bool hasReturnValue = false)
        {
            IDbCommand command = BuildQueryCommand(StoredProcName, Parameters);
            if (hasReturnValue)
            {
                IDbDataParameter Parameter = Para;
                Parameter.ParameterName = "ReturnValue";
                Parameter.DbType = DbType.Int32;
                Parameter.Size = 4;
                Parameter.Direction = ParameterDirection.ReturnValue;
                Parameter.Precision = 0;
                Parameter.Scale = 0;
                Parameter.SourceColumn = string.Empty;
                Parameter.SourceVersion = DataRowVersion.Default;
                Parameter.Value = null;
                command.Parameters.Add(Parameter);
            }
            return command;
        }


        /// <summary>
        /// �����Command����
        /// </summary>
        /// <example>
        /// �H�U���ϥ�BuildQueryCommand���d�ҵ{���G
        /// <code>
        /// // Create parameter array
        ///	SqlParameter[] parameters =
        ///	{
        ///		new SqlParameter( "@LOGIN_ID", Sqldbkind.NVarChar, 50 ),	// 0
        ///		new SqlParameter( "@DOMAIN", Sqldbkind.NVarChar, 50 ),	// 1
        ///	};
        ///	
        ///	// Set parameter values and directions
        ///	parameters[ 0 ].Value = LOGINID;
        ///	parameters[ 1 ].Value = DOMAIN;
        ///
        ///	SqlCommand cmd =BuildIntCommand("spQueryProcedure",parameters);
        /// </code>
        /// </example>
        /// <param name="StoredProcName">�w�s�{�ǦW��</param>
        /// <param name="Parameters">�w�s�{�ǰѼư}�C</param>
        /// <returns></returns>
        private IDbCommand BuildQueryCommand(string StoredProcName, IDataParameter[] Parameters)
        {
            IDbCommand command = Cmd;
            command.CommandText = StoredProcName;
            command.Connection = DbCon;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = CommandTimeOut;
            command.Parameters.Clear();
            if (Parameters != null)
            {
                foreach (IDataParameter Parameter in Parameters)
                {
                    command.Parameters.Add(Parameter);
                }
            }
            return command;
        }


        /// <summary>
        /// ����w�x�{�ǨöǦ^�v�T���C��
        /// </summary>
        /// <example>
        /// �H�U���ϥ�RunProcedure���d�ҵ{���G
        /// <code>
        /// // Create parameter array
        ///	SqlParameter[] parameters =
        ///	{
        ///		new SqlParameter( "@LOGIN_ID", Sqldbkind.NVarChar, 50 ),	// 0
        ///		new SqlParameter( "@DOMAIN", Sqldbkind.NVarChar, 50 ),	// 1
        ///	};
        ///	
        ///	// Set parameter values and directions
        ///	parameters[ 0 ].Value = LOGINID;
        ///	parameters[ 1 ].Value = DOMAIN;
        ///
        ///int rowsAffected=0; 
        ///BuildIntCommand("spUpdateProcedure",parameters,"Default",rowsAffected);
        ///if(rowsAffected==0)
        ///		ShowErrorMessage("���楢��");
        ///	else 
        ///		ShowErrorMessage("���榨�\");
        /// </code>
        /// </example>
        /// <param name="StoredProcName">�w�s�{�ǦW��</param>
        /// <param name="Parameters">�w�s�{�ǰѼư}�C</param>
        /// <param name="Connection">Conncetion�W��</param>
        /// <param name="rowsAffected">�v�T���C��</param>
        public void RunProcedure(string StoredProcName, IDataParameter[] Parameters, string Connection, out int rowsAffected)
        {
            IDbCommand command = BuildIntCommand(StoredProcName, Parameters);
            try
            {
                Open(Connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                rowsAffected = (int)command.Parameters["ReturnValue"];
                CloseDB();
            }
        }

        public void RunProcedure(string StoredProcName, IDataParameter[] Parameters, string Connection, out int rowsAffected, bool IsUseTransaction)
        {
            IDbCommand command = BuildIntCommand(StoredProcName, Parameters);
            IDbTransaction trans;
            try
            {
                Open(Connection);
                if (IsUseTransaction)
                {
                    trans = DbCon.BeginTransaction();
                    try
                    {
                        command.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception iex)
                    {
                        trans.Rollback();
                        throw iex;
                    }
                }
                else
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                rowsAffected = (int)command.Parameters["ReturnValue"];
                CloseDB();
            }
        }

        /// <summary>
        /// ����w�x�{�ǨöǦ^DataReaader����
        /// </summary>
        /// <example>
        /// �H�U���ϥ�RunProcedure���d�ҵ{���G
        /// <code>
        /// // Create parameter array
        ///	SqlParameter[] parameters =
        ///	{
        ///		new SqlParameter( "@LOGIN_ID", Sqldbkind.NVarChar, 50 ),	// 0
        ///		new SqlParameter( "@DOMAIN", Sqldbkind.NVarChar, 50 ),	// 1
        ///	};
        ///	
        ///	// Set parameter values and directions
        ///	parameters[ 0 ].Value = LOGINID;
        ///	parameters[ 1 ].Value = DOMAIN;
        ///
        ///	SqlDataReader cmd =RunProcedure("spQueryProcedure",parameters,"Default");
        /// </code>
        /// </example>
        /// <param name="StoredProcName">�w�s�{�ǦW��</param>
        /// <param name="Parameters">�w�s�{�ǰѼư}�C</param>
        /// <param name="Connection">Connection�W��</param>
        /// <returns></returns>
        public IDataReader RunProcedure(string StoredProcName, IDataParameter[] Parameters, string Connection, bool hasReturnValue = false)
        {
            IDbCommand command = BuildIntCommand(StoredProcName, Parameters, hasReturnValue);
            command.CommandType = CommandType.StoredProcedure;
            IDataReader sdr;
            try
            {
                Open(Connection);
                sdr = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                //CloseDB();
            }
            return sdr;
        }

        /// <summary>
        /// ����w�x�{�ǨöǦ^DataTable����
        /// </summary>
        /// <example>
        /// �H�U���ϥ�RunProcedure���d�ҵ{���G
        /// <code>
        /// // Create parameter array
        ///	SqlParameter[] parameters =
        ///	{
        ///		new SqlParameter( "@LOGIN_ID", Sqldbkind.NVarChar, 50 ),	// 0
        ///		new SqlParameter( "@DOMAIN", Sqldbkind.NVarChar, 50 ),	// 1
        ///	};
        ///	
        ///	// Set parameter values and directions
        ///	parameters[ 0 ].Value = LOGINID;
        ///	parameters[ 1 ].Value = DOMAIN;
        ///
        ///	DataTable cmd =RunProcedure("spQueryProcedure",parameters,"Default");
        /// </code>
        /// </example>
        /// <param name="StoredProcName">�w�s�{�ǦW��</param>
        /// <param name="Parameters">�w�s�{�ǰѼư}�C</param>
        /// <param name="TableName">DataTable�W��</param>
        /// <param name="Connection">Connection�W��</param>
        /// <returns></returns>
        public DataTable RunProcedure(string StoredProcName, IDataParameter[] Parameters, string TableName, string Connection)
        {
            DataTable dt = new DataTable(TableName);
            DbDataAdapter Da;
            try
            {
                Open(Connection);
                if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                {
                    Da = (OleDbDataAdapter)IDa;
                    Da.SelectCommand = (OleDbCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(dt);
                }
                else if (Datatype == dbkind.SQL_Server)
                {
                    Da = (SqlDataAdapter)IDa;
                    Da.SelectCommand = (SqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(dt);
                }
                else if (Datatype == dbkind.Oracle)
                {
                    Da = (OracleDataAdapter)IDa;
                    Da.SelectCommand = (OracleCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(dt);
                }
                else if (Datatype == dbkind.MySql)
                {
                    Da = (MySqlDataAdapter)IDa;
                    Da.SelectCommand = (MySqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                CloseDB();
            }
            return dt;
        }

        public DataTable RunProcedure(string StoredProcName, IDataParameter[] Parameters, string TableName, string Connection, bool IsUseTransaction)
        {
            DataTable dt = new DataTable(TableName);
            DbDataAdapter Da;
            IDbTransaction trans;
            try
            {
                Open(Connection);
                if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                {
                    Da = (OleDbDataAdapter)IDa;
                    Da.SelectCommand = (OleDbCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(dt);
                }
                else if (Datatype == dbkind.SQL_Server)
                {
                    Da = (SqlDataAdapter)IDa;
                    Da.SelectCommand = (SqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    if (IsUseTransaction)
                    {
                        trans = DbCon.BeginTransaction();
                        Da.SelectCommand.Transaction = (DbTransaction)trans;
                        try
                        {
                            Da.Fill(dt);
                            trans.Commit();
                        }
                        catch (Exception iex)
                        {
                            trans.Rollback();
                            throw iex;
                        }
                    }
                    else
                    {
                        Da.Fill(dt);
                    }
                }
                else if (Datatype == dbkind.Oracle)
                {
                    Da = (OracleDataAdapter)IDa;
                    Da.SelectCommand = (OracleCommand)BuildQueryCommand(StoredProcName, Parameters);
                    if (IsUseTransaction)
                    {
                        trans = DbCon.BeginTransaction();
                        Da.SelectCommand.Transaction = (DbTransaction)trans;
                        try
                        {
                            Da.Fill(dt);
                            trans.Commit();
                        }
                        catch (Exception iex)
                        {
                            trans.Rollback();
                            throw iex;
                        }
                    }
                    else
                    {
                        Da.Fill(dt);
                    }
                }
                else if (Datatype == dbkind.MySql)
                {
                    Da = (MySqlDataAdapter)IDa;
                    Da.SelectCommand = (MySqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                CloseDB();
            }
            return dt;
        }

        /// <summary>
        /// ����w�x�{�ǨñN���G�[�J�{����Dataset��
        /// </summary>
        /// <example>
        /// �H�U���ϥ�RunProcedure���d�ҵ{���G
        /// <code>
        /// // Create parameter array
        ///	SqlParameter[] parameters =
        ///	{
        ///		new SqlParameter( "@LOGIN_ID", Sqldbkind.NVarChar, 50 ),	// 0
        ///		new SqlParameter( "@DOMAIN", Sqldbkind.NVarChar, 50 ),	// 1
        ///	};
        ///	
        ///	// Set parameter values and directions
        ///	parameters[ 0 ].Value = LOGINID;
        ///	parameters[ 1 ].Value = DOMAIN;
        ///
        ///	DataSet Ds = new DataSet();
        ///	RunProcedure("spQueryProcedure",parameters,Ds,"Table1","Default");
        /// </code>
        /// </example>
        /// <param name="StoredProcName">�w�s�{�ǦW��</param>
        /// <param name="Parameters">�w�s�{�ǰѼư}�C</param>
        /// <param name="Ds">DataSet����</param>
        /// <param name="TableName">DataTable�W��</param>
        /// <param name="Connection">Connection�W��</param>
        public void RunProcedure(string StoredProcName, IDataParameter[] Parameters, ref DataSet Ds, string TableName, string Connection)
        {
            DbDataAdapter Da;
            try
            {
                Open(Connection);
                if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                {
                    Da = new OleDbDataAdapter();
                    Da.SelectCommand = (OleDbCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(Ds, TableName);
                }
                else if (Datatype == dbkind.SQL_Server)
                {
                    Da = new SqlDataAdapter();
                    Da.SelectCommand = (SqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(Ds, TableName);
                }
                else if (Datatype == dbkind.Oracle)
                {
                    Da = new OracleDataAdapter();
                    Da.SelectCommand = (OracleCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(Ds, TableName);
                }
                else if (Datatype == dbkind.MySql)
                {
                    Da = new MySqlDataAdapter();
                    Da.SelectCommand = (MySqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(Ds, TableName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                CloseDB();
            }
        }

        public void RunProcedure(string StoredProcName, IDataParameter[] Parameters, ref DataSet Ds, string TableName, string Connection, bool IsUseTransaction)
        {
            DbDataAdapter Da;
            IDbTransaction trans;
            try
            {
                Open(Connection);
                if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                {
                    Da = new OleDbDataAdapter();
                    Da.SelectCommand = (OleDbCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(Ds, TableName);
                }
                else if (Datatype == dbkind.SQL_Server)
                {
                    Da = new SqlDataAdapter();
                    Da.SelectCommand = (SqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    if (IsUseTransaction)
                    {
                        trans = DbCon.BeginTransaction();
                        Da.SelectCommand.Transaction = (DbTransaction)trans;
                        try
                        {
                            Da.Fill(Ds, TableName);
                            trans.Commit();
                        }
                        catch (Exception iex)
                        {
                            trans.Rollback();
                            throw iex;
                        }
                    }
                    else
                    {
                        Da.Fill(Ds, TableName);
                    }
                }
                else if (Datatype == dbkind.Oracle)
                {
                    Da = new OracleDataAdapter();
                    Da.SelectCommand = (OracleCommand)BuildQueryCommand(StoredProcName, Parameters);
                    if (IsUseTransaction)
                    {
                        trans = DbCon.BeginTransaction();
                        Da.SelectCommand.Transaction = (DbTransaction)trans;
                        try
                        {
                            Da.Fill(Ds, TableName);
                            trans.Commit();
                        }
                        catch (Exception iex)
                        {
                            trans.Rollback();
                            throw iex;
                        }
                    }
                    else
                    {
                        Da.Fill(Ds, TableName);
                    }
                }
                else if (Datatype == dbkind.MySql)
                {
                    Da = new MySqlDataAdapter();
                    Da.SelectCommand = (MySqlCommand)BuildQueryCommand(StoredProcName, Parameters);
                    Da.Fill(Ds, TableName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                CloseDB();
            }
        }

        /// <summary>
        /// ����d�ߡA�öǦ^�d�ߩҶǦ^�����G���Ĥ@�Ӹ�ƦC���Ĥ@�Ӹ�Ʀ�C�|������L����Ʀ�θ�ƦC�C 
        /// </summary>
        /// <example>
        ///	�@�몺 RunSQL �d�ߥi�H�p�U�C���d�Ү榡�ơG 
        /// <code>
        /// string strSQL = "SELECT COUNT(*) FROM dbo.region";
        /// Int32 count = 0 ; //���o��Ƶ���
        /// RunSQL(strSQL,"Default",count);
        /// </code>
        /// </example>
        /// <param name="strSQL">SQL �y�k</param>
        /// <param name="Connection">Connection�W��</param>
        /// <param name="Returnobj">�^�ǵ��G����</param>
        /// <param name="values">�Ѽƭ�</param>
        public void RunSQL(string strSQL, string Connection, out object Returnobj, object[] values = null)
        {
            //IDbCommand command = Cmd;
            //Cmd.CommandText = strSQL;
            //Cmd.Connection = DbCon;
            IDbCommand command;
            try
            {
                Open(Connection);
                command = db.GetSqlStringCommand(strSQL);
                BuildParameters(ref command, values);
                command.Connection = DbCon;
                Returnobj = command.ExecuteScalar();
            }
            catch (MySqlException ex) { Returnobj = null; throw new Exception(ex.Message, ex); }
            catch (OracleException ex) { Returnobj = null; throw new Exception(ex.Message, ex); }
            catch (SqlException ex) { Returnobj = null; throw new Exception(ex.Message, ex); }
            catch (Exception ex) { Returnobj = null; throw new Exception(ex.Message, ex); }
            finally
            {
                CloseDB();
            }
        }
        /// <summary>
        /// ����SQL Command�öǦ^DataTable
        /// </summary>
        ///<example>
        ///	�@�몺 RunSQL ���d�ҡG
        ///<code>
        ///		string strSQL = "SELECT * FROM dbo.region";
        ///		DataTable dt = RunSQL(strSQL,"region");
        ///</code>
        ///</example>
        /// <param name="strSQL">SQL�y�k</param>
        /// <param name="TableName">DataTable�W��</param>
        /// <param name="Connection">Connection�W��</param>
        /// <param name="values">�Ѽƭ�</param>
        /// <returns></returns>
        public DataTable RunSQL(string strSQL, string TableName, string Connection, object[] values = null)
        {
            DataTable dt = new DataTable(TableName);
            DbDataAdapter Da;
            try
            {
                Open(Connection);
                IDbCommand command = db.GetSqlStringCommand(strSQL);
                BuildParameters(ref command, values);
                command.Connection = DbCon;
                if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                {
                    //Da = new OleDbDataAdapter(strSQL, (OleDbConnection)DbCon);
                    Da = new OleDbDataAdapter((OleDbCommand)command);
                    Da.Fill(dt);
                }
                else if (Datatype == dbkind.SQL_Server)
                {
                    //Da = new SqlDataAdapter(strSQL, (SqlConnection)DbCon);
                    Da = new SqlDataAdapter((SqlCommand)command);
                    Da.Fill(dt);
                }
                else if (Datatype == dbkind.Oracle)
                {
                    //Da = new OracleDataAdapter(strSQL, (OracleConnection)DbCon);
                    Da = new OracleDataAdapter((OracleCommand)command);
                    (Da.SelectCommand as OracleCommand).FetchSize = (Da.SelectCommand as OracleCommand).FetchSize * 8;
                    Da.Fill(dt);
                }
                else if (Datatype == dbkind.MySql)
                {
                    //Da = new MySqlDataAdapter(strSQL, (MySqlConnection)DbCon);
                    Da = new MySqlDataAdapter((MySqlCommand)command);
                    Da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                CloseDB();
            }
            return dt;
        }

        /// <summary>
        /// ����SQL Command�ñNDataTable�[�J��{����DataSet
        /// </summary>
        ///<example>
        ///	�@�몺 RunSQL ���d�ҡG
        ///<code>
        ///		string strSQL = "SELECT * FROM dbo.region";
        ///		DataSet Ds= DataSet();
        ///		RunSQL(strSQL,Ds,"region","Default");
        ///</code>
        ///</example>
        /// <param name="strSQL">SQL�y�k</param>
        /// <param name="Ds">DataSet����</param>
        /// <param name="TableName">DataTable�W��</param>
        /// <param name="Connection">Connection�W��</param>
        /// <param name="values">�Ѽƭ�</param>
        public void RunSQL(string strSQL, ref DataSet Ds, string TableName, string Connection, object[] values = null)
        {
            try
            {
                Open(Connection);
                DbDataAdapter Da;
                IDbCommand command = db.GetSqlStringCommand(strSQL);
                BuildParameters(ref command, values);
                command.Connection = DbCon;
                if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                {
                    //Da = new OleDbDataAdapter(strSQL, (OleDbConnection)DbCon);
                    Da = new OleDbDataAdapter((OleDbCommand)command);
                    Da.Fill(Ds, TableName);
                }
                else if (Datatype == dbkind.SQL_Server)
                {
                    //Da = new SqlDataAdapter(strSQL, (SqlConnection)DbCon);
                    Da = new SqlDataAdapter((SqlCommand)command);
                    Da.Fill(Ds, TableName);
                }
                else if (Datatype == dbkind.Oracle)
                {
                    // Da = new OracleDataAdapter(strSQL, (OracleConnection)DbCon);
                    Da = new OracleDataAdapter((OracleCommand)command);
                    (Da.SelectCommand as OracleCommand).FetchSize = (Da.SelectCommand as OracleCommand).FetchSize * 8;
                    Da.Fill(Ds, TableName);
                }
                else if (Datatype == dbkind.MySql)
                {
                    //Da = new MySqlDataAdapter(strSQL, (MySqlConnection)DbCon);
                    Da = new MySqlDataAdapter((MySqlCommand)command);
                    Da.Fill(Ds, TableName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                CloseDB();
            }
        }
        /// <summary>
        /// ����SQL Command�öǦ^�v�T���C��
        /// </summary>
        ///<example>
        ///	�@�몺 RunSQL ���d�ҡG
        ///<code>
        ///		string strSQL = "Delete FROM dbo.region where regionid=1";
        ///		int RowsAffected=0;
        ///		RunSQL(strSQL,RowsAffected,"Default");
        ///</code>
        ///</example>
        /// <param name="strSQL">T-SQL�y�k</param>
        /// <param name="RowsAffected">���v�T���C��</param>
        /// <param name="Connection">�s�u�r��</param>
        /// <param name="values">�Ѽƭ�</param>
        public void RunSQL(string strSQL, out int RowsAffected, string Connection, object[] values = null)
        {
            RowsAffected = 0;
            try
            {
                Open(Connection);
                IDbCommand command = db.GetSqlStringCommand(strSQL);
                BuildParameters(ref command, values);
                command.Connection = DbCon;
                if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                {
                    //OleDbCommand command = new OleDbCommand(strSQL, (OleDbConnection)DbCon);
                    RowsAffected = command.ExecuteNonQuery();
                }
                else if (Datatype == dbkind.SQL_Server)
                {
                    //SqlCommand command = new SqlCommand(strSQL, (SqlConnection)DbCon);
                    RowsAffected = command.ExecuteNonQuery();
                }
                else if (Datatype == dbkind.Oracle)
                {
                    //OracleCommand command = new OracleCommand(strSQL, (OracleConnection)DbCon);
                    (command as OracleCommand).FetchSize = (command as OracleCommand).FetchSize * 8;
                    RowsAffected = command.ExecuteNonQuery();
                }
                else if (Datatype == dbkind.MySql)
                {
                    //MySqlCommand command = new MySqlCommand(strSQL, (MySqlConnection)DbCon);
                    RowsAffected = command.ExecuteNonQuery();
                }
                if (RowsAffected == 0) RowsAffected = 1;
            }
            catch (MySqlException ex) { RowsAffected = 0; throw new Exception(ex.Message, ex); }
            catch (OracleException ex) { RowsAffected = 0; throw new Exception(ex.Message, ex); }
            catch (SqlException ex) { RowsAffected = 0; throw new Exception(ex.Message, ex); }
            catch (Exception ex) { RowsAffected = 0; throw new Exception(ex.Message, ex); }
            finally
            {
                CloseDB();
            }
        }


        /// <summary>
        /// ����SQL Command�öǦ^���\�Υ���
        /// </summary>
        ///<example>
        ///	�@�몺 RunSQL ���d�ҡG
        ///<code>
        ///		string strSQL = "Delete FROM dbo.region where regionid=1";
        ///		
        ///		if(RunSQL(strSQL))
        ///			ShowErrorMessage("���榨�\");
        ///		else 
        ///			ShowErrorMessage("���楢��");
        ///</code>
        ///</example>
        /// <param name="strSQL">The STR SQL.</param>
        /// <param name="Connection">�s�u�r��</param>
        /// <returns></returns>
        public bool RunSQL(string strSQL, string Connection)
        {
            int i = 0;
            RunSQL(strSQL, out i, Connection);
            return (i > 0);
        }
        public bool BulkAddToTable(DataTable DT, string TableName, string Connection)
        {
            bool IsInsertOk = false;
            try
            {
                Open(Connection);
                if (Datatype == dbkind.SQL_Server)
                {
                    DataRow[] rowArray = DT.Select();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)DbCon))
                    {
                        bulkCopy.DestinationTableName = TableName;
                        try
                        {
                            // Write the array of rows to the destination.
                            bulkCopy.WriteToServer(rowArray);
                            IsInsertOk = true;
                        }
                        catch (Exception ex)
                        {
                            IsInsertOk = false;
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
            }
            catch (MySqlException ex) { throw new Exception(ex.Message, ex); }
            catch (OracleException ex) { throw new Exception(ex.Message, ex); }
            catch (SqlException ex) { throw new Exception(ex.Message, ex); }
            catch (Exception ex) { throw new Exception(ex.Message, ex); }
            finally
            {
                CloseDB();
            }
            return IsInsertOk;
        }
        /// <summary>
        /// ����SQL Command�öǦ^���\�Υ���
        /// </summary>
        ///<example>
        ///	�@�몺 RunSQL ���d�ҡG
        ///<code>
        ///		string strSQL = "Delete FROM dbo.region where regionid=1";
        ///		
        ///		if(RunSQL(strSQL,"Default"))
        ///			ShowErrorMessage("���榨�\");
        ///		else 
        ///			ShowErrorMessage("���楢��");
        ///</code>
        ///</example>
        /// <param name="strSQL">The STR SQL.</param>
        /// <param name="strConnection">The connectionstring.</param>
        /// <param name="values">�Ѽƭ�</param>
        /// <returns></returns>
        public bool ExecuteSQL(string strSQL, string strConnection, object[] values = null)
        {
            int i = 0;
            RunSQL(strSQL, out i, strConnection, values);
            return (i > 0);
        }

        /// <summary>
        /// Generates the condition.
        /// </summary>
        /// <example>
        /// GenerateCondition�d�ҡG
        ///	<code>
        ///		DataTable dt = RunSQL("select * from dbo.region","region");
        ///		strSQL = "select * from dbo.employee where " + 
        ///		GenerateCondition("regionid",FilterCollection.FilterType.Equal,"en",true); 
        ///	</code>
        /// </example>
        /// <param name="FieldName">���W��</param>
        /// <param name="Filter">�z��覡</param>
        /// <param name="KeyWord">��</param>
        /// <param name="AddQuote">�ȬO�_�ϥγ�޸�</param>
        /// <returns></returns>
        public string GenerateCondition(string FieldName, FilterCollection.FilterType Filter, string KeyWord, bool AddQuote)
        {
            string FileType = "=";
            switch (Filter.ToString())
            {
                case "LessEqual":
                    FileType = "<=";
                    break;
                case "EqualThan":
                    FileType = ">=";
                    break;
                case "NotEqual":
                    FileType = "<>";
                    break;
                case "Less":
                    FileType = "<";
                    break;
                case "Than":
                    FileType = ">";
                    break;
                case "Like":
                    FileType = "like";
                    break;
            }

            if (!AddQuote)
                return string.Format("{0} {1} {2}", FieldName, FileType, KeyWord);
            else
            {
                if (FileType == "like")
                    return string.Format("{0} {1} '%{2}%'", FieldName, FileType, KeyWord);
                else
                    return string.Format("{0} {1} '{2}'", FieldName, FileType, KeyWord);
            }

        }

        /// <summary>
        /// ���� Transact-SQL ���
        /// </summary>
        /// <example>
        /// Transaction�d�ҡG
        ///	<code>
        ///		if(Transaction("insert into region('1','en')"
        ///							,"delete from region where regionid=2"))
        ///			ShowErrorMessage("���榨�\");
        ///		else 
        ///			ShowErrorMessage("���楢��");
        ///	</code>
        /// </example>
        /// <param name="strSQLS">The STR SQLS.</param>
        /// <returns></returns>
        public bool Transaction(string ConnectionStr, params string[] strSQLS)
        {
            IDbTransaction transaction = null;
            try
            {
                Open(ConnectionStr);
                transaction = DbCon.BeginTransaction();
                foreach (string strSQL in strSQLS)
                {
                    if (strSQL != "" && strSQL != null)
                    {
                        if (Datatype == dbkind.Access || Datatype == dbkind.Excel)
                            new OleDbCommand(strSQL, (OleDbConnection)DbCon, (OleDbTransaction)transaction).ExecuteNonQuery();
                        else if (Datatype == dbkind.SQL_Server)
                            new SqlCommand(strSQL, (SqlConnection)DbCon, (SqlTransaction)transaction).ExecuteNonQuery();
                        else if (Datatype == dbkind.Oracle)
                        {
                            transaction = ((OracleConnection)DbCon).BeginTransaction();
                            new OracleCommand(strSQL, (OracleConnection)DbCon).ExecuteNonQuery();
                        }
                        else if (Datatype == dbkind.MySql)
                            new MySqlCommand(strSQL, (MySqlConnection)DbCon, (MySqlTransaction)transaction).ExecuteNonQuery();
                    }
                }
                transaction.Commit();
                return true;
            }
            catch (OracleException ex)
            {
                if (transaction != null) transaction.Rollback();
                return false;
                throw new Exception(ex.Message, ex);
            }
            catch (SqlException ex)
            {
                if (transaction != null) transaction.Rollback();
                return false;
                throw new Exception(ex.Message, ex);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
                throw new Exception(ex.Message, ex.GetBaseException());
            }
            finally
            {
                CloseDB();
            }
        }

        /// <summary>
        /// Inserts the access log.
        /// </summary>
        /// <param name="strSQL">The STR SQL.</param>
        /// <returns></returns>
        private string InsertAccessLog(string strSQL)
        {
            return "true";
            //return(userInfo.CurrentPath!=null 
            //  && (userInfo.CurrentPath.ToLower()!="default.aspx" 
            //  || userInfo.CurrentPath.ToLower()!="banner.aspx" 
            //  || userInfo.CurrentPath.ToLower()!="middle.aspx" 
            //  || userInfo.CurrentPath.ToLower()!="treeview.aspx"
            //  || userInfo.CurrentPath.ToLower()!="basepage.cs"))?(string.Format(@"INSERT INTO [AccessLog]" + "\r\n" +
            //  @"           ([UserId]" + "\r\n" +
            //  @"           ,[SQLCmd]" + "\r\n" +
            //  @"           ,[AppPath]" + "\r\n" +
            //  @"           ,[LoginId]" + "\r\n" +
            //  @"           ,[Createdate])" + "\r\n" +
            //  @"     VALUES(" + "\r\n" +
            //  @"           '{0}'" + "\r\n" +
            //  @"           ,'{1}'" + "\r\n" +
            //  @"           ,'{2}'" + "\r\n" +
            //  @"           ,'{3}'" + "\r\n" +
            //  @"           ,getdate())",userInfo.UserID,strSQL.Replace("'","''"),userInfo.CurrentPath,userInfo.PageLoginId)):"";
        }
        #region IDisposable ����

        public void Dispose()
        {
            if (DbCon != null)
            {
                DbCon.Close();
                DbCon.Dispose();
                DbCon = null;
            }
        }

        #endregion
        private void BuildParameters(ref IDbCommand cmd, object[] values)
        {
            string sqlParamChar = string.Empty;
            switch (Datatype)
            {
                case dbkind.SQL_Server:
                case dbkind.Access:
                case dbkind.Excel:
                    sqlParamChar = "@";
                    break;
                case dbkind.Oracle:
                    sqlParamChar = ":";
                    break;
                case dbkind.MySql:
                    sqlParamChar = "?";
                    break;
            }
            List<string> paramNames = BetweenStringsList(cmd.CommandText, sqlParamChar, " ");
            if (values != null && values.Length > 0 && values.Length == paramNames.Count)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = paramNames[i];
                    parameter.Value = values[i];
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        private static List<string> BetweenStringsList(string text, string start, string end)
        {
            List<string> matched = new List<string>();

            int indexStart = 0;
            int indexEnd = 0;

            bool exit = false;
            while (!exit)
            {
                indexStart = text.IndexOf(start);

                if (indexStart != -1)
                {
                    indexEnd = indexStart + text.Substring(indexStart).IndexOf(end);

                    matched.Add(text.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length));

                    text = text.Substring(indexEnd + end.Length);
                }
                else
                {
                    exit = true;
                }
            }
            return matched;
        }
    }
}