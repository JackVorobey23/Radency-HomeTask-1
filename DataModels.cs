namespace HomeTask1
{
    public class Payer
    {
        public string name {get;set;}
        public decimal payment {get;set;}
        public DateTime date {get;set;}
        public long account_number {get;set;}

    }
    public class ServiceData
    {
        public string name {get;set;}
        public List<Payer> payers {get; set;}
        public decimal total {get;set;}
    }
    public class CityData
    {
        public string city {get;set;}
        public List<ServiceData> services {get; set;}
        public decimal total {get;set;}
    }
}