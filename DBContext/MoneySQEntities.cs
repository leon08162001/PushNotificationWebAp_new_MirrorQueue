﻿using System;
using System.Configuration;
using System.Data.Entity;

namespace DBContext
{
    public partial class MoneySQEntities : DbContext
    {
        public MoneySQEntities(String ConnectionName)
        {
            string ConnStr = ConfigurationManager.ConnectionStrings[ConnectionName].ToString();
            string DecryptConn = Utility.DBConnection.GetEntityServerPlainConnString(ConnStr);
            DbContext DbContext = new DbContext(DecryptConn);
            base.Database.Connection.ConnectionString = DbContext.Database.Connection.ConnectionString;
            //base.Database.Connection.ConnectionString = Utility.DBConnection.GetSqlServerPlainConnString(ConnStr);
        }
    }
}
