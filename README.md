24/10 
Idag öppnade jag uppgiften och läste igenom den för att förstå hur jag skulle gå tillväga och planera mitt arbete. 

 

26/10 
Idag gick jag igenom min tidigare uppgift för att börja bygga upp den här på samma sätt. 
Jag började med att skapa en switch-meny för användarval och skapade sedan klasserna Vehicle, Car och Mc. 

 

27/10 
Implementerade klasserna ParkingSpot och ParkingGarage. 
Jag lärde mig hur man kan loopa genom listor och hantera objekt på ett enklare sätt, vilket gjorde koden mycket mer flexibel. 

 

28/10 
Färdigställde metoder för att kunna parkera och hämta ut fordon. 
Jag lade till HasSpace() för att kontrollera om det finns plats, och RemoveVehicle() för att ta bort ett fordon. 
Jag fick problem med att hitta rätt plats baserat på registreringsnummer, men löste det med FirstOrDefault() och FindSpotIndexByLicense(). 

 

29/10 
Började lägga till funktionen MoveVehicle. 
Jag stötte på problem med indexeringen att plats 10 i listan motsvarar index 9 i koden. 
Löste det genom att minska newSpotNumber-- för att få rätt resultat. 

 

30/10 
Började med filhantering i JSON. 
Jag implementerade FileSaving med SaveGarage() och LoadGarage() för att spara data vid  avslut och läsa in den vid start. 
Jag fick problem med serialisering av objekt som använder arv, men löste det med en egen VehicleConverter som hanterar både Car och Mc. 

 

31/10 
Lade till config.json för att styra inställningar som antal platser, priser och fordonstyper. 
Det gör att programmet kan ändras utan att man behöver uppdatera koden. 
Det var lite avancerat, men jag förstod hur JSON kan användas för att lagra den typen av data. 

 

01/11 
Började implementera Spectre.Console för att göra gränssnittet snyggare och mer användarvänligt. 
Jag lade till färger, rubriker och en meny med piltangenter. 

02/11 
Gjorde klart en parkeringskarta med Spectre.Console som visar varje plats i färg 
(grön = ledig, gul = delvis fylld, röd = full) och regnummer för fordonen. 
Lade även till en progressbar som visar hur stor del av parkeringen som är upptagen. 
Efter många tester fungerar allt stabilt data sparas och laddas korrekt. 

Slutsats 

Jag har känt både frustration och stolthet under det här projektet. 
Det har tagit mycket tid och många sena kvällar att få allt att fungera, men jag är väldigt nöjd med resultatet. Jag är lite besviken på att jag inte hann klart med VG-delen, men samtidigt stolt över hur mycket jag lärt mig. 
Det har varit mycket nytt särskilt filhantering, Spectre.Console och JSON.
