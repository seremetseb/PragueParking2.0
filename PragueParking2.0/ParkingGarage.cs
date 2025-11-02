using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace PragueParking2._0
{
    
    public class ParkingGarage
    {

        public List<ParkingSpot> spots { get; set; } = new List<ParkingSpot>();

        public ParkingGarage() { }

        public ParkingGarage(int NumberOfSpots = 100)
        {

            spots = new List<ParkingSpot>();
            for (int i = 0; i < NumberOfSpots; i++)
            {
                spots.Add(new ParkingSpot { SpotNumber = i + 1 });
            }
        }
        public bool ParkVehicle(Vehicle v)
        {
            foreach (var spot in spots)
            {
                if (spot.HasSpace(v)) 
                { 
                    spot.ParkVehicle(v);
                    AnsiConsole.MarkupLine("[green]{0} parkerades på plats {1}.[/]",v , spot.SpotNumber);
                    return true;
                }
            
            }
            AnsiConsole.MarkupLine("[red]Det finns inga lediga parkeringsplatser för detta fordon.[/]");
            return false;

        }

   
        public bool RemoveVehicle(string licensePlate)
        {
            foreach (var spot in spots)
            {
                var vehicle = spot.Vehicles.FirstOrDefault(v => (v.LicensePlate ?? "") == licensePlate);
                if (vehicle == null)
                {
                    continue;
                }

                DateTime arrival = vehicle.Arrival;
                DateTime departure = DateTime.Now;
                TimeSpan parkedTime = departure - arrival;

                int hours = (int)parkedTime.TotalHours;
                int minutes = parkedTime.Minutes;

                double totalMinutes = parkedTime.TotalMinutes;

                double cost = 0;

                if (totalMinutes > 10)
                {
                    double totalHoursRoundedUp = Math.Ceiling(parkedTime.TotalHours);
                    cost = totalHoursRoundedUp * vehicle.PricePerHour;
                }

                spot.RemoveVehicle(licensePlate);

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .Title("[bold yellow]ParkeringsKvitto[/]");

                table.AddColumn(new TableColumn("[bold white]Fält[/]").Centered());
                table.AddColumn(new TableColumn("[bold white]Värde[/]").Centered());
                table.AddRow("[grey]Fordon[/]", $"[bold white]{vehicle.Type}[/]");
                table.AddRow("[grey]Registreringsnummer[/]", $"[bold white]{vehicle.LicensePlate}[/]");
                table.AddRow("[grey]Parkeringsplats[/]", $"[bold white]{spot.SpotNumber}[/]");
                table.AddRow("[grey]Ankomsttid[/]", $"[bold white]{arrival}[/]");
                table.AddRow("[grey]Avgångstid[/]", $"[bold white]{departure}[/]");
                table.AddRow("[grey]Parkerad tid[/]", $"[bold white]{hours} timmar och {minutes} minuter[/]");
                table.AddRow("[grey]Total kostnad[/]", totalMinutes <= 10 ? "[green]0 CZK[/]" : $"[bold white]{cost} CZK[/]");

                var panel = new Panel(table)
                {
                    Border = BoxBorder.Rounded,
                    Header = new PanelHeader("[yellow]Tack för att du parkerade hos oss![/]"),
                    Padding = new Padding(1, 1, 1, 1)
                };

                AnsiConsole.Write(panel);

                return true;
            }

            AnsiConsole.MarkupLine("[bold red]Fordonet {0} finns inte i garaget.[/]", licensePlate);
            return false;
        }

        public int FindVehicleSpot(string licensePlate)
        {
            for (int i = 0; i < spots.Count; i++)
            {
                if (spots[i].Vehicles.Any(v => v.LicensePlate == licensePlate))
                {
                    return i;
                }
            }
            return -1;
        }

        public void FindVehicle(string licensePlate)
        {
            int index = FindSpotIndexByLicense(licensePlate);

            if (index == -1)
            {
                AnsiConsole.MarkupLine("[red]Fordonet med registreringsnummer {0} hittades inte på parkeringsplatsen.[/]", licensePlate);
            }
            else
            {
                AnsiConsole.MarkupLine("[green]Fordonet med registreringsnummer {0} står på plats {1}.[/]", licensePlate, index + 1);
            }

        }

        public bool MoveVehicle(string licensePlate, int newSpotNumber) 
        { 
            if (newSpotNumber < 0 || newSpotNumber >= spots.Count)
            {
               AnsiConsole.MarkupLine("[red]Ogiltigt platsnummer![/]");
                return false;
            }

            int fromIndex = FindSpotIndexByLicense(licensePlate);
            if (fromIndex == -1)
            {
                AnsiConsole.MarkupLine("[red]Fordonet med registreringsnummer {0} hittades inte på parkeringsplatsen.[/]", licensePlate);
                return false;
            }
            var fromSpot = spots[fromIndex];
            var toSpot = spots[newSpotNumber];

            var vehicle = fromSpot.Vehicles.First(v => v.LicensePlate == licensePlate);

            if (!toSpot.HasSpace(vehicle))
            {
                AnsiConsole.MarkupLine("[red]Det finns inte tillräckligt med utrymme på plats {0} för fordonet.[/]", newSpotNumber + 1);    
                return false;
            }

            fromSpot.Vehicles.Remove(vehicle);
            toSpot.Vehicles.Add(vehicle);

            AnsiConsole.MarkupLine("[green]Fordonet med registreringsnummer {0} har flyttats till plats {1}.[/]", licensePlate, newSpotNumber + 1); 
            return true;
        }

        public int FindSpotIndexByLicense(string licensePlate)
        {
            for (int i = 0; i < spots.Count; i++)
            {
                if (spots[i].Vehicles.Any(v => v.LicensePlate == licensePlate))
                {
                    return i;
                }
            }
            return -1;
        }

        public void ShowGrid()
        {
            int cols = 10;

            var cells = new List<string>();

            for (int i = 0; i < spots.Count; i++)
            {
                var spot = spots[i];
                
                if (spot.Vehicles.Count == 0)
                {
                    cells.Add($"[green]{spot.SpotNumber} (Ledigt)[/]");
                }
                else
                {
                    int usedSpace = spot.Vehicles.Sum(v => v.Size);
                    int capacity = spot.Capacity;

                    string color = usedSpace >= capacity ? "red" : "yellow";

                    string regs = string.Join(" | ", spot.Vehicles.Select(v => v.LicensePlate));

                    cells.Add($"[{color}]Plats {spot.SpotNumber}[/]\n[{color}]{regs}[/]");
                }
                
            }

            var grid = new Table()
                .Border(TableBorder.None)
                .HideHeaders()
                .Centered();

            for (int c = 0; c < cols; c++) 
            { 
                grid.AddColumn(new TableColumn(""));
            }

            for (int start = 0; start < cells.Count; start += cols)
            {
                var slice = cells.Skip(start).Take(cols).ToList();
                while (slice.Count < cols)
                {
                    slice.Add("");
                }
                grid.AddRow(slice.Select(s => new Markup(s)).ToArray());
            }

            var panel = new Panel(grid)
                .Header("[bold cyan]Parkeringskarta[/]")
                .Border(BoxBorder.Rounded)
                .Padding(1, 1, 1, 1);


            AnsiConsole.Clear();
            AnsiConsole.Write(panel);

            AnsiConsole.WriteLine();
            ShowSummary();

            AnsiConsole.MarkupLine("\n[grey]Tryck på valfri tangent för att återgå till menyn...[/]");
            Console.ReadKey();
        }

        public void ShowSummary()
        {
            int total = spots.Count;
            int empty = spots.Count(s => s.Vehicles.Count == 0);
            int occupied = total - empty;

            
            var summaryTable = new Table()
                .Centered()
                .Border(TableBorder.Heavy)
                .Title("[bold yellow]Parkeringsöversikt[/]");

            summaryTable.AddColumn("[bold]Totalt antal platser[/]");
            summaryTable.AddColumn("[bold green]Lediga[/]");
            summaryTable.AddColumn("[bold red]Upptagna[/]");

            summaryTable.AddRow(
                $"[white]{total}[/]",
                $"[green]{empty}[/]",
                $"[red]{occupied}[/]"
            );

           
            double fillPercent = total == 0 ? 0 : (double)occupied / total * 100;

            AnsiConsole.Write(summaryTable);

            AnsiConsole.WriteLine(); 
            AnsiConsole.MarkupLine($"[bold]Använda Parkeringsplatser:[/] {fillPercent:F1}%");

           
            AnsiConsole.Progress()
                .HideCompleted(true)
                .Columns(new ProgressColumn[]
                {
            new TaskDescriptionColumn(),    
            new ProgressBarColumn(),       
            new PercentageColumn()          
                })
                .Start(ctx =>
                {
                    var task = ctx.AddTask("[yellow]Använda parkeringsplatser[/]", maxValue: total);
                    task.Increment(occupied);
                    
                    System.Threading.Thread.Sleep(400);
                });

            AnsiConsole.WriteLine();
        }


        public void ShowParking()
        {
            foreach (var spot in spots)
            {
                Console.WriteLine(spot.ToString());
                    
            }
        }

    }
}
