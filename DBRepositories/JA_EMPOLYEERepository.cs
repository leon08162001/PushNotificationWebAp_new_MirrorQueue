using DataAccess;
using System.Data.Entity;

namespace DBRepositories
{
    public class JA_EMPOLYEERepository<JA_EMPOLYEE> : SpecificEntityRepository<JA_EMPOLYEE> where JA_EMPOLYEE : class, new()
    {
        public JA_EMPOLYEERepository(DbContext DbContext) : base(DbContext)
        {
        }
    }
}
