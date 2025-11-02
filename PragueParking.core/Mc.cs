

namespace PragueParking.core
{
    public class Mc : Vehicle
    {
        public Mc() 
        { 
            Size = 2;
            PricePerHour = 10;
           Type = "mc";
        }
        public Mc(string licensePlate) : base(licensePlate)
        {
            
            Size = 2;
            PricePerHour = 10;
           Type = "mc";
        }

    }
}
