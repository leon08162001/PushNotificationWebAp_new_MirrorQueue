namespace ExtendedDBModels
{
    public class NewCustomers
    {
        public decimal pk { get; set; } //(decimal(18,0), not null)
        public string orderno { get; set; } //(nvarchar(20), null)
        public string country { get; set; } //(varchar(3), not null)
    }
}
