
namespace PragueParking2._0
{
    public class Vehicle
    {
        public string LicensePlate { get; set; }
        public int Size { get; set; }  
        public int PricePerHour { get; set; }
        public DateTime Arrival { get; set; } = DateTime.Now;
        public string Type { get; set; } // bil eller mc 

        public Vehicle() { }
        
        
        public Vehicle(string licensePLate)
        { 
            LicensePlate = licensePLate.ToUpper();
            Arrival = DateTime.Now;


        }

       public override string ToString() => $"{GetType().Name.ToUpper()}#{LicensePlate}";
        
            
        






    }
}
