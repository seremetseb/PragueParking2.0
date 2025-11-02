using System;
using System.Collections.Generic;
using System.Linq;

namespace PragueParking2._0
{
    public class ParkingSpot
    {
        public int SpotNumber { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public int Capacity { get; set; } =4;

        public bool HasSpace(Vehicle v)
        { 
            int UsedSpace = Vehicles.Sum(vehicle => vehicle.Size);
            return UsedSpace + v.Size <= Capacity;


        }

        public bool ParkVehicle(Vehicle v)
        {
            if (HasSpace(v))
            {
                Vehicles.Add(v);
                return true;
            }
            return false;
        }

        public void RemoveVehicle(string licensePlate)
        { 
            var vehicle = Vehicles.FirstOrDefault(v => v.LicensePlate == licensePlate);
            if (vehicle != null)
            {
                Vehicles.Remove(vehicle);
            } 

        }

        public override string ToString()
        {
            if (Vehicles.Count == 0)
                return $"Plats {SpotNumber}: (Ledigt)";
            var parts = Vehicles.Select(v => $"{v.Type}#{v.LicensePlate}");
            return $"Plats {SpotNumber}: {string.Join("|", parts)}";
        }

    }
}
