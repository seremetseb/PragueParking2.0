

namespace PragueParking.core
{
    public class Car : Vehicle
    {

        public Car() 
        { 
            Size = 4;
            PricePerHour = 20;
            Type = "Car";
        }
        public Car(string licensePlate) : base(licensePlate)
        {
            Size = 4;
            PricePerHour = 20;
            Type = "Car";
        }

    }
}
