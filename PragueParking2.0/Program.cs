using Spectre.Console;
using PragueParking.core;

namespace PragueParking2._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = Config.Load();
            var garage = FileSaving.LoadGarage();

            if (garage == null || garage.spots.Count == 0)
            {
                garage = new ParkingGarage(config.TotalSpots);
            }



            bool running = true;
            while (running)
            {
                Console.Clear();

                
                AnsiConsole.Write(
                        new Panel(
                        new Markup("[bold cyan]Prague Parking 2.0[/]\n[grey]Välj ett alternativ med piltangenterna[/]"))
                        .Border(BoxBorder.Rounded)
                        .Header("[yellow]Huvudmeny[/]")
                        .HeaderAlignment(Justify.Center)
                );

                
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[white]Vad vill du göra?[/]")
                        .PageSize(40)
                        .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
                        .AddChoices(new[]
                        {
                            "1. Parkera fordon",
                            "2. Flytta fordon",
                            "3. Hämta ut fordon",
                            "4. Sök efter fordon",
                            "5. Visa parkeringsplats",
                            "0. Avsluta"
                        }));

                switch (choice)
                {
                    case "1. Parkera fordon":
                        Parkera_UI(garage);
                        break;

                    case "2. Flytta fordon":
                        MoveVehicle_UI(garage);
                        break;

                    case "3. Hämta ut fordon":
                        PickUpVehicle_UI(garage);
                        break;

                    case "4. Sök efter fordon":
                        SearchVehicle_UI(garage);
                        break;

                    case "5. Visa parkeringsplats":
                        ShowGarage_UI(garage);
                        break;

                    case "0. Avsluta":
                        FileSaving.SaveGarage(garage);
                        running = false;
                        break;
                }
            }

            AnsiConsole.MarkupLine("[bold green]Ha det så bra![/]");
        }
        

        static void Parkera_UI(ParkingGarage garage)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline cyan]Parkera fordon[/]\n");

          
            var typeInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj [green]fordonstyp[/]:")
                    .AddChoices("bil", "mc")
            );

           
            var licensePlate = AnsiConsole.Prompt(
                new TextPrompt<string>("Ange registreringsnummer (max 10 tecken):")
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return ValidationResult.Error("[red]Får inte vara tomt[/]");
                        if (input.Trim().Length > 10)
                            return ValidationResult.Error("[red]För långt[/]");
                        return ValidationResult.Success();
                    })
            ).Trim().ToUpper();

            
            Vehicle v = null;
            if (typeInput == "bil")
                v = new Car(licensePlate);
            else if (typeInput == "mc")
                v = new Mc(licensePlate);

            bool parked = garage.ParkVehicle(v);

           
            FileSaving.SaveGarage(garage);

            if (parked)
            {
                AnsiConsole.MarkupLine("\n[bold green]Fordonet parkerades![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[bold red]Det fanns ingen plats för detta fordon.[/]");
            }
            PauseReturn();




        }

        static void MoveVehicle_UI(ParkingGarage garage)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline cyan]Flytta fordon[/]\n");

            var licensePlate = AnsiConsole.Prompt(
                new TextPrompt<string>("Ange registreringsnummer att flytta:")
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return ValidationResult.Error("[red]Får inte vara tomt[/]");
                        return ValidationResult.Success();
                    })
            ).Trim().ToUpper();

            int maxSpots = garage.spots.Count;

            var newSpotNumber = AnsiConsole.Prompt(
                new TextPrompt<int>($"Ange nytt platsnummer (1-{maxSpots}):")
                    .Validate(num =>
                    {
                        if (num < 1 || num > maxSpots)
                            return ValidationResult.Error("[red]Ogiltigt platsnummer[/]");
                        return ValidationResult.Success();
                    })
            );

            bool success = garage.MoveVehicle(licensePlate, newSpotNumber - 1);

            if (success)
            {
                FileSaving.SaveGarage(garage);
                AnsiConsole.MarkupLine("\n[bold green]Fordonet har flyttats.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[bold red]Kunde inte flytta fordonet.[/]");
            }

            PauseReturn();

        }

        static void PickUpVehicle_UI(ParkingGarage garage)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline cyan]Hämta ut fordon[/]\n");

            var licensePlate = AnsiConsole.Prompt(
                new TextPrompt<string>("Ange registreringsnummer för fordonet du vill hämta ut:")
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return ValidationResult.Error("[red]Får inte vara tomt[/]");
                        return ValidationResult.Success();
                    })
            ).Trim().ToUpper();

            bool removed = garage.RemoveVehicle(licensePlate);

            if (removed)
            {
                FileSaving.SaveGarage(garage);
                AnsiConsole.MarkupLine("\n[bold green]Fordonet är uthämtat och borttaget ur garaget.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[bold red]Fordonet hittades inte.[/]");
            }

            PauseReturn();

        }

        static void SearchVehicle_UI(ParkingGarage garage)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline cyan]Sök fordon[/]\n");

            var licensePlate = AnsiConsole.Prompt(
                new TextPrompt<string>("Ange registreringsnummer att söka efter:")
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return ValidationResult.Error("[red]Får inte vara tomt[/]");
                        return ValidationResult.Success();
                    })
            ).Trim().ToUpper();

            int index = garage.FindSpotIndexByLicense(licensePlate);

            if (index == -1)
            {
                AnsiConsole.MarkupLine("[red]Fordon {0} hittades inte i garaget.[/]", licensePlate);
            }
            else
            {
                var spotNumber = index + 1;
                AnsiConsole.MarkupLine("[green]Fordon {0} står på plats {1}.[/]", licensePlate, spotNumber);
            }

            PauseReturn();
        }

        static void ShowGarage_UI(ParkingGarage garage)
        {
            AnsiConsole.Clear();
            garage.ShowGrid(); 
          

        }

            static void PauseReturn()
        {
            AnsiConsole.MarkupLine("\n[grey]Tryck på valfri tangent för att återgå till menyn...[/]");
            Console.ReadKey(true);
        }

    }
        
    }