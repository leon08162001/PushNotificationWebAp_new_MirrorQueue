//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBModels
{
    using System;
    
    public partial class sp_GetRetentionHistoryByLoanNo_Result
    {
        public string status { get; set; }
        public string refinance_amt { get; set; }
        public string redeem { get; set; }
        public string redeem_item { get; set; }
        public string interest_flag { get; set; }
        public string interest { get; set; }
        public string valuation_amt { get; set; }
        public Nullable<System.DateTime> valuation_date { get; set; }
        public string valuation_staff { get; set; }
        public string new_credit_limit_flag { get; set; }
        public string new_credit_limit_amt { get; set; }
        public string remarks { get; set; }
        public string approved_by { get; set; }
        public string risk_level { get; set; }
        public Nullable<System.DateTime> last_upd_date { get; set; }
        public string last_upd_user { get; set; }
    }
}
