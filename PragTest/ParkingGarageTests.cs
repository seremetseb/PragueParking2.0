using Microsoft.VisualStudio.TestTools.UnitTesting;
using PragueParking2._0;
using System.Linq;

namespace PragueParking2._0.Tests
{
    [TestClass]
    public class ParkingGarageTests
    {
        [TestMethod]
        public void ParkVehicle_Should_AddVehicle_ToSomeSpot()
        {
            // Arrange
            var garage = new ParkingGarage(10);
            var car = new Car("ABC123");

            // Act
            bool parked = garage.ParkVehicle(car);

            // Assert
            Assert.IsTrue(parked, "ParkVehicle borde returnera true om bilen fick plats.");

            // kolla att bilen verkligen ligger i någon ruta
            bool found = garage.spots.Any(spot =>
                spot.Vehicles.Any(v => v.LicensePlate == "ABC123"));

            Assert.IsTrue(found, "Bilen ABC123 borde ha lagts in i garaget.");
        }

        [TestMethod]
        public void RemoveVehicle_Should_RemoveVehicle_FromGarage()
        {
            // Arrange
            var garage = new ParkingGarage(10);
            var mc = new Mc("XYZ999");
            garage.ParkVehicle(mc);

            // Act
            bool removed = garage.RemoveVehicle("XYZ999");

            // Assert
            Assert.IsTrue(removed, "RemoveVehicle borde returnera true om fordonet fanns och togs bort.");

            bool stillThere = garage.spots.Any(spot =>
                spot.Vehicles.Any(v => v.LicensePlate == "XYZ999"));

            Assert.IsFalse(stillThere, "MC med reg XYZ999 ska inte längre finnas parkerad.");
        }
    }
}