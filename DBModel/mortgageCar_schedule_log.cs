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
    
    public partial class mortgageCar_schedule_log
    {
        public decimal pk { get; set; }
        public Nullable<decimal> schedule_pk { get; set; }
        public string status { get; set; }
        public string supervisor { get; set; }
        public string order_nbr { get; set; }
        public Nullable<System.DateTime> request_date { get; set; }
        public string request_time { get; set; }
        public string est_time { get; set; }
        public string complete_time { get; set; }
        public string location { get; set; }
        public string cus_name { get; set; }
        public string tel { get; set; }
        public string driverName { get; set; }
        public string licensePlate { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
        public string updated_by { get; set; }
    }
}
