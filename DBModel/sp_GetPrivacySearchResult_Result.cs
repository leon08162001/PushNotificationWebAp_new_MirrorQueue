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
    
    public partial class sp_GetPrivacySearchResult_Result
    {
        public int pk { get; set; }
        public Nullable<bool> agree { get; set; }
        public string cus_name { get; set; }
        public string idnum { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string callin_date { get; set; }
        public Nullable<bool> is_print { get; set; }
        public string lst_print_date { get; set; }
        public string printed_by { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
    }
}
