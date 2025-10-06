Josefines Toll Calculator

Lösning
Lösningen är byggd som ett backend-REST API byggt med ASP.NET och en frontend-klient byggd med React (Vite).

Mockdata
Jag valda att börja med en json-fil med mockdata som man sedan kunde ha gjort om till en databas. Min json fil ligger i mappen Data i
#TollCalculatorAPI". 

Services
Jag har skapat två services för API:et
- UserRepository service:
En service för att hämta och fylla mina klasser med min json-data.

- TollCalculator service:
En service för att räkna ut avgifter per datum.

Jag stoppade in orginal logiken för avgift uträkningen i den här servicen. Jag ändrade om en del, jag tog bort funktionen för att räkna ut avgift per dag och ersatte den med en funktionen "PopulateFeesForVehicle" som räknar ut avgift per datum och sätter den uträknade avgiften i min "Fee" variabel i min VehicleDate klass. 

Jag ändrade dom andra original funktionerna från TollCalculator lite grann för att göra dom mer läsvänliga.

Controllers
Har en UserController som anropas av React klienten. Min UserController populerar mina användare med dess data inklusive avgift per datum samt lite sammanslagna avgifter som sedan skickas till klienten.

React klient
I min react klient har jag lite komponenter för en typisk sidlayout (Header, Footer, Nav m.m) och användarbild, samt mina "websidor" (Pages).

CSS
Jag tänkte först använda mig av Tailwind CSS men stötte på patrull med installeringen så valde istället att skriva CSS:en själv. 
CSS:en för siten ligger i "APP.css"
I Början av App.css finns färg-gränsnittet jag vale som SS variabler.

Användarbilder
Jag laddade ner några gratis användarbilder från nätet som jag stoppade in i mappen "Img" i mappen "Assets". Finns två små komponenter (UserImages och UserImage) som hanterar visning av bilderna.

Förbättringspotential
Saker jag hade velat implementera:
⦁	En knapp och komponent för att visa tidigare månaders debiterade datum i VehicleDetails.jsx
⦁	En snyggare design för fordonslänkarna i User.jsx
⦁	En översättning av typ av fordon till svenska och beskrivningar om att vissa fordonstyper är avgiftsfria.
⦁	En inloggningssida för användaren.





