using DataAccess;
using MoneySQContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;


namespace WebApiAp.Controller
{
    [RoutePrefix("api/MoneySQ/ZZ_APPLICATION")]
    public class ZZ_APPLICATIONController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
        [HttpPost]
        public IList GetAllApplications()
        {
            IList Result;
            SpecificEntityRepository<ZZ_APPLICATION> db = new SpecificEntityRepository<ZZ_APPLICATION>(new MoneySQEntities("MONEYSQ_Encrypt"));
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
        public string GetApplicantIDByID(string ID)
        {
            string applicantID = "";
            SpecificEntityRepository<ZZ_APPLICATION> db = new SpecificEntityRepository<ZZ_APPLICATION>(new MoneySQEntities("MONEYSQ_Encrypt"));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("idno_of_applicant", ID);
            List<ZZ_APPLICATION> applicants = db.Find("select * from[dbo].[ZZ_APPLICATION] where idno_of_applicant = @idno_of_applicant", dic);
            if (applicants.Count > 0)
            {
                applicantID = applicants[0].idno_of_applicant;
            }
            return applicantID;
        }
        public bool UpdateApplicantPushByID(string ID)
        {
            bool result = false;
            SpecificEntityRepository<ZZ_APPLICATION> db = new SpecificEntityRepository<ZZ_APPLICATION>(new MoneySQEntities("MONEYSQ_Encrypt"));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@idno_of_applicant", ID);
            List<ZZ_APPLICATION> applicants = db.Find("select * from[dbo].[ZZ_APPLICATION] where idno_of_applicant = @idno_of_applicant", dic);
            foreach(ZZ_APPLICATION ap in applicants)
            {
                ap.opr_id = ap.idno_of_applicant;
                ap.opr_name = ap.name_of_applicant;
                ap.opr_date = DateTime.Now;
                ap.enable_push = true;
                result = db.Update(ap);
            }
            return result;
        }
    }
}
