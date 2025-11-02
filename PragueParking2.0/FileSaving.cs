using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using PragueParking.core;

namespace PragueParking2._0
{
    public static class FileSaving
    {
        private static readonly string filePath = "garage.json";

        public static void SaveGarage(ParkingGarage garage)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
              
            };
           string json= JsonSerializer.Serialize(garage, options);
              File.WriteAllText(filePath, json);
        }

        public static ParkingGarage LoadGarage()
        {
            if (!File.Exists(filePath))
            {
                return new ParkingGarage(100);
            }

            string json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                Converters = { new VehicleConverter() }
            };

            ParkingGarage loadedGarage = JsonSerializer.Deserialize<ParkingGarage>(json, options);

            if (loadedGarage == null)
            {
                return new ParkingGarage(100);
            }

            if (loadedGarage.spots == null)
            { 
                loadedGarage.spots = new List<ParkingSpot>();
            }
            return loadedGarage;
        }


    }

    public class VehicleConverter : JsonConverter<Vehicle>
    { 
    
        public override Vehicle Read(ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                string license = root.GetProperty("LicensePlate").GetString();
                string type = root.GetProperty("Type").GetString();
                DateTime arrival = root.GetProperty("Arrival").GetDateTime();

                Vehicle v;
                if (string.Equals(type, "Car", StringComparison.OrdinalIgnoreCase))
                {
                    v = new Car(license);
                }
                else if (string.Equals(type, "Mc", StringComparison.OrdinalIgnoreCase))
                {
                    v = new Mc(license);
                }
                else
                {
                    throw new NotSupportedException($"Vehicle type '{type}' is not supported.");
                }

                v.Arrival = arrival;

                return v;

            }


        }
    
    
    

    public override void Write(Utf8JsonWriter writer, Vehicle value, JsonSerializerOptions options)
        {
           writer.WriteStartObject();
           writer.WriteString("LicensePlate", value.LicensePlate);
           writer.WriteString("Type", value.Type);
           writer.WriteString("Arrival", value.Arrival);
           writer.WriteNumber("PricePerHour", value.PricePerHour);
           writer.WriteNumber("Size", value.Size);

            writer.WriteEndObject();
        }
    }

}
