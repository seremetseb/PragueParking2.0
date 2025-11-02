using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PragueParking2._0
{
    public class VehicleTypeConfig 
    { 
        public string Type { get; set; } = "";
        public int Size { get; set; }
        public int PricePerHour { get; set; }
    }
    public class ConfigData 
    { 
        public int TotalSpots { get; set; } = 100;
        public List<VehicleTypeConfig> VehicleTypes { get; set; } = new();
        public int FreeMinutes { get; set; } = 10;

    }

    public static class Config
    {
        private static readonly string configFilePath = "config.json";

        public static ConfigData Load()
        {
            if (!File.Exists(configFilePath))
            {
                var defaultConfig = CreatDefaultConfig();
                Save(defaultConfig);
                return defaultConfig;
            }

            string json = File.ReadAllText(configFilePath);

            var cfg = JsonSerializer.Deserialize<ConfigData>(json);

            if (cfg == null)
            {
                var fallbackConfig = CreatDefaultConfig();
                Save(fallbackConfig);
                return fallbackConfig;

            }
            return cfg;
        }
    

    public static void Save(ConfigData cfg)
     {
            var json = JsonSerializer.Serialize(cfg, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFilePath, json);

     }

        private static ConfigData CreatDefaultConfig()
        {
            return new ConfigData
            {
                TotalSpots = 100,
                FreeMinutes = 10,
                VehicleTypes = new List<VehicleTypeConfig>
                {
                    new VehicleTypeConfig { Type = "Car", Size = 4, PricePerHour = 20 },
                    new VehicleTypeConfig { Type = "Motorcycle", Size = 2, PricePerHour = 10 },

                }
            };

        }
    }
}
