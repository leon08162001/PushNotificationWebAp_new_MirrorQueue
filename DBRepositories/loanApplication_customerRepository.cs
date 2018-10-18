using DataAccess;
using System.Data.Entity;

namespace DBRepositories
{
    public class loanApplication_customerRepository<loanApplication_customer> : SpecificEntityRepository<loanApplication_customer> where loanApplication_customer : class, new()
    {
        public loanApplication_customerRepository(DbContext DbContext) : base(DbContext)
        {
        }
    }
}
