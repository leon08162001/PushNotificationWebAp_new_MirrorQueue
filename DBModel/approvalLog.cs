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
    
    public partial class approvalLog
    {
        public decimal logID { get; set; }
        public string order_nbr { get; set; }
        public Nullable<int> status { get; set; }
        public string last_upd_user { get; set; }
        public Nullable<System.DateTime> last_upd_date { get; set; }
    }
}
