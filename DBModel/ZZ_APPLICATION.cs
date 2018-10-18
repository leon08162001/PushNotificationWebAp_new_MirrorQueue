using System;
using System.ComponentModel.DataAnnotations;

namespace DBModels
{
    public class ZZ_APPLICATION
    {
        [Key]
        public virtual string company_code { get; set; } // nvarchar(10), not null
        [Key]
        public virtual string application_no { get; set; } // nvarchar(50), not null
        public virtual string application_no_front { get; set; } // nvarchar(50), null
        public virtual string application_category { get; set; } // nvarchar(3), not null
        public virtual DateTime? date_of_application { get; set; } // date, null
        public virtual string status_of_application { get; set; } // nvarchar(3), not null
        public virtual string data_created_method { get; set; } // nvarchar(3), not null
        public virtual string type_of_applicant { get; set; } // nvarchar(3), null
        public virtual string idno_of_applicant { get; set; } // nvarchar(100), null
        public virtual string name_of_applicant { get; set; } // nvarchar(255), null
        public virtual DateTime? birthday { get; set; } // date, null
        public virtual string sex_code { get; set; } // nvarchar(3), null
        public virtual DateTime? issue_date { get; set; } // date, null
        public virtual string issue_category { get; set; } // nvarchar(3), null
        public virtual string name_of_father { get; set; } // nvarchar(50), null
        public virtual string name_of_mother { get; set; } // nvarchar(50), null
        public virtual string name_of_spouse { get; set; } // nvarchar(50), null
        public virtual string birthplace { get; set; } // nvarchar(50), null
        public virtual string military_category { get; set; } // nvarchar(50), null
        public virtual string identity_card_no { get; set; } // nvarchar(50), null
        public virtual string nhi_ic_card_no { get; set; } // nvarchar(50), null
        public virtual string issue_city { get; set; } // nvarchar(2), null
        public virtual DateTime? setup_date { get; set; } // date, null
        public virtual string currency_type { get; set; } // nvarchar(3), null
        public virtual decimal? amt_for_applying { get; set; } // decimal(12,2), null
        public virtual short? period_for_applying { get; set; } // smallint, null
        public virtual string programid_for_applying { get; set; } // nvarchar(255), null
        public virtual short? empolyeeno_of_service_staff { get; set; } // smallint, null
        public virtual short? empolyeeno_for_performance { get; set; } // smallint, null
        public virtual string divisioncode_for_performance { get; set; } // nvarchar(10), null
        public virtual string permanent_address { get; set; } // nvarchar(255), null
        public virtual string permanent_address_zipcode { get; set; } // nvarchar(5), null
        public virtual string permanent_address_city { get; set; } // nvarchar(20), null
        public virtual string permanent_address_town { get; set; } // nvarchar(20), null
        public virtual string permanent_address_street { get; set; } // nvarchar(20), null
        public virtual string permanent_address_li { get; set; } // nvarchar(20), null
        public virtual string permanent_address_lin { get; set; } // nvarchar(6), null
        public virtual string permanent_address_section { get; set; } // nvarchar(6), null
        public virtual string permanent_address_lane { get; set; } // nvarchar(6), null
        public virtual string permanent_address_alley { get; set; } // nvarchar(6), null
        public virtual string permanent_address_no { get; set; } // nvarchar(6), null
        public virtual string permanent_address_floor { get; set; } // nvarchar(6), null
        public virtual string permanent_address_room { get; set; } // nvarchar(6), null
        public virtual string mailing_address { get; set; } // nvarchar(255), null
        public virtual string mailing_address_zipcode { get; set; } // nvarchar(5), null
        public virtual string mailing_address_city { get; set; } // nvarchar(20), null
        public virtual string mailing_address_town { get; set; } // nvarchar(20), null
        public virtual string mailing_address_street { get; set; } // nvarchar(20), null
        public virtual string mailing_addresss_li { get; set; } // nvarchar(20), null
        public virtual string mailing_addresss_lin { get; set; } // nvarchar(6), null
        public virtual string mailing_address_section { get; set; } // nvarchar(6), null
        public virtual string mailing_address_lane { get; set; } // nvarchar(6), null
        public virtual string mailing_address_alley { get; set; } // nvarchar(6), null
        public virtual string mailing_address_no { get; set; } // nvarchar(6), null
        public virtual string mailing_address_floor { get; set; } // nvarchar(6), null
        public virtual string mailing_address_room { get; set; } // nvarchar(6), null
        public virtual string email { get; set; } // nvarchar(255), null
        public virtual string cell_phone { get; set; } // nvarchar(40), null
        public virtual string job_title { get; set; } // nvarchar(255), null
        public virtual string education_level_cd { get; set; } // nvarchar(3), null
        public virtual string living_condition_cd { get; set; } // nvarchar(3), null
        public virtual string monthly_income_code { get; set; } // nvarchar(3), null
        public virtual string marital_status_cd { get; set; } // nvarchar(3), null
        public virtual string digital_certificate_status { get; set; } // nvarchar(3), null
        public virtual string consent_letter_sign_status { get; set; } // nvarchar(3), null
        public virtual string applicant_bankcode { get; set; } // nvarchar(3), null
        public virtual string applicant_bankbranch_code { get; set; } // nvarchar(4), null
        public virtual string applicant_bank_account { get; set; } // nvarchar(50), null
        public virtual decimal? suggested_interest_rate { get; set; } // decimal(5,4), null
        public virtual decimal? suggested_amt { get; set; } // decimal(12,2), null
        public virtual string suggested_contract_type { get; set; } // nvarchar(3), null
        public virtual string suggested_opinion { get; set; } // nvarchar(2000), null
        public virtual decimal? approved_interest_rate { get; set; } // decimal(5,4), null
        public virtual decimal? approved_amt { get; set; } // decimal(12,2), null
        public virtual string approved_contract_type { get; set; } // nvarchar(3), null
        public virtual DateTime? datetime_of_acceptence { get; set; } // datetime2(7), null
        public virtual short? case_officer_employeeno { get; set; } // smallint, null
        public virtual string opr_id { get; set; } // nvarchar(100), not null
        public virtual string opr_name { get; set; } // nvarchar(255), not null
        public virtual DateTime opr_date { get; set; } // datetime2(7), not null
        public virtual string opr_ip_address { get; set; } // nvarchar(40), not null
        public virtual string opr_gps_address { get; set; } // nvarchar(40), null
    }

}
