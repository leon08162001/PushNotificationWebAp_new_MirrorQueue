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
    using System.Collections.Generic;
    
    public partial class refinanceRetentionReport
    {
        public decimal pk { get; set; }
        public Nullable<decimal> refinance_pk { get; set; }
        public string loan_id { get; set; }
        public string address { get; set; }
        public string p_type { get; set; }
        public string area_g { get; set; }
        public string area_n { get; set; }
        public string valuation { get; set; }
        public string loan_date { get; set; }
        public string transfer_to { get; set; }
        public string interest { get; set; }
        public string pmt { get; set; }
        public string loan_type { get; set; }
        public string terms { get; set; }
        public Nullable<System.DateTime> completed_date { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
    }
}
