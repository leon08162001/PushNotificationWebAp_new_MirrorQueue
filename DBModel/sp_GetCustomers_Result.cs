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
    
    public partial class sp_GetCustomers_Result
    {
        public decimal pk { get; set; }
        public string order_nbr { get; set; }
        public string country { get; set; }
        public Nullable<int> seq { get; set; }
        public string customer_type { get; set; }
        public string nickname { get; set; }
        public string eng_name { get; set; }
        public Nullable<bool> is_pay { get; set; }
        public string id_num { get; set; }
        public Nullable<System.DateTime> date_of_birth { get; set; }
        public string phone_mobile { get; set; }
        public string phone_home { get; set; }
        public string phone_office { get; set; }
        public string add_1 { get; set; }
        public string add_2 { get; set; }
        public string refno { get; set; }
        public string lst_upd_user { get; set; }
        public Nullable<System.DateTime> lst_upd_date { get; set; }
        public bool is_shareholder { get; set; }
        public string shareholder_contributions { get; set; }
        public string shareholder_contributions_shares { get; set; }
    }
}
