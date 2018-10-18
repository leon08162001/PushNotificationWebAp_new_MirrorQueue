using DataAccess;
using MoneySQContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;


namespace WebApiAp.Controller
{
    [RoutePrefix("api/MoneySQ/JA_EMPOLYEE")]
    public class JA_EMPOLYEEController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
        [HttpPost]
        public IList GetAllEmployees()
        {
            IList Result;
            SpecificEntityRepository<JA_EMPOLYEE> db = new SpecificEntityRepository<JA_EMPOLYEE>(new MoneySQEntities("MONEYSQ_Encrypt"));
            Result = db.GetAll();
            if (Result.Count == 0)
            {
                throw new LASException(LASErrror.ObjectNotFound);
            }
            else
            {
                return Result;
            }
        }
        public string GetEmployeeID(string ID)
        {
            string Id = "";
            SpecificEntityRepository<JA_EMPOLYEE> db = new SpecificEntityRepository<JA_EMPOLYEE>(new MoneySQEntities("MONEYSQ_Encrypt"));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("social_security_number", ID);
            List<JA_EMPOLYEE> emps = db.Find("select * from [dbo].[JA_EMPOLYEE] where social_security_number= @social_security_number", dic);
            if (emps.Count > 0)
            {
                Id = emps[0].social_security_number;
            }
            return Id;
        }
        public bool UpdateEmployeePushByID(string ID)
        {
            bool result = false;
            SpecificEntityRepository<JA_EMPOLYEE> db = new SpecificEntityRepository<JA_EMPOLYEE>(new MoneySQEntities("MONEYSQ_Encrypt"));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@social_security_number", ID);
            List<JA_EMPOLYEE> emps = db.Find("select * from [dbo].[JA_EMPOLYEE] where social_security_number= @social_security_number", dic);
            if (emps.Count > 0)
            {
                emps[0].opr_id = emps[0].empolyee_no.ToString();
                emps[0].opr_name = emps[0].empolyee_name;
                emps[0].opr_date = DateTime.Now;
                emps[0].enable_push = true;
                result = db.Update(emps[0]);
            }
            return result;
        }
    }
}
