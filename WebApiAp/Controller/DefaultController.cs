using DBContext;
using DBModels;
using DBRepositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

namespace WebApiAp.Controller
{
    [RoutePrefix("api/Default")]
    public class DefaultController : ApiController
    {

        public string Get()
        {
            return "Hello World";
        }
        [HttpPost]
        public IList GetLoanApplication_customer(string nickname)
        {
            IList Result;
            //throw new Exception("test error");
            Dictionary<string, object> InParams = new Dictionary<string, object>();
            InParams.Add("nickname", nickname);
            object outValue;
            //SpecificEntityRepository<loanApplication_customer> Entities = new SpecificEntityRepository<loanApplication_customer>(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
            //Result = Entities.ExecuteProcedure("sp_GetCustomers3", 3, new Type[] { typeof(loanApplication_customer), typeof(comUser), typeof(UserActivityLog) }, out outValue, InParams, null);
            // CommonEntityRepository Entities = new CommonEntityRepository(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
            //Result = Entities.ExecuteProcedure("sp_GetCustomers3", 3, new Type[] { typeof(loanApplication_customer), typeof(comUser), typeof(UserActivityLog) }, out outValue, InParams, null);
            loanApplication_customerRepository<loanApplication_customer> loanApplication_customer = new DBRepositories.loanApplication_customerRepository<loanApplication_customer>(new LAS_TWEntities("LAS_TWEntities_Encrypt"));
            Result = loanApplication_customer.ExecuteProcedure("sp_GetCustomers3", 3, new Type[] { typeof(loanApplication_customer), typeof(comUser), typeof(UserActivityLog) }, out outValue, InParams, null);

            //List<loanApplication_customer> a = Entities.GetAll<loanApplication_customer>("-seq");
            //a = Entities.GetAll<loanApplication_customer>(r => r.nickname, r => r.seq);

            //List<loanApplication_customer> a = loanApplication_customer.GetAll("-seq");
            //a = loanApplication_customer.GetAll(r => r.nickname, r => r.seq);

            if (Result.Count == 0)
            {
                throw new LASException(LASErrror.ObjectNotFound);
            }
            else
            {
                return Result;
            }
        }
    }
}
