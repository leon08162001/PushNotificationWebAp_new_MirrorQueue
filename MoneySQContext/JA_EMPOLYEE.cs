//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoneySQContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class JA_EMPOLYEE
    {
        [Key, Column(Order = 0)]
        public string company_code { get; set; }
        [Key, Column(Order = 1)]
        public short empolyee_no { get; set; }
        public string social_security_number { get; set; }
        public string empolyee_name { get; set; }
        public string empolyee_last_name { get; set; }
        public string empolyee_first_name { get; set; }
        public string employee_english_name { get; set; }
        public string empolyee_english_last_name { get; set; }
        public string empolyee_english_first_name { get; set; }
        public string in_services_status { get; set; }
        public System.DateTime on_board_date { get; set; }
        public Nullable<System.DateTime> leaving_date { get; set; }
        public string cell_phone { get; set; }
        public string home_phone_no { get; set; }
        public string office_phone_no { get; set; }
        public string office_phone_extension { get; set; }
        public string email { get; set; }
        public string mailing_address { get; set; }
        public string division_code { get; set; }
        public Nullable<System.DateTime> division_effective_date { get; set; }
        public string job_title_code { get; set; }
        public Nullable<System.DateTime> job_effective_date { get; set; }
        public string opr_id { get; set; }
        public string opr_name { get; set; }
        public System.DateTime opr_date { get; set; }
        public string opr_ip_address { get; set; }
        public string opr_gps_address { get; set; }
        public Nullable<bool> enable_push { get; set; }
    }
}
