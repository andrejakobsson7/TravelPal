# TravelPal
Slutprojekt i “Objektorienterad programmering, grund” vid Newton Yrkeshögskola, HT-23  

Jag har i detta projekt försökt jobba skapa en struktur där så lite kod som möjligt skall finnas i code-behinden bakom varje sida. 
Metoder som jag skapat har jag försökt göra så generella som möjligt, exempelvis genom att skicka med både värdet som ska valideras men också avsändaren (ex. Combobox) och sedan lägga dem i Manager-klasserna, så att alla sidor kan nyttja samma validering/logik därigenom. Det har tagit ganska mycket tid i anspråk att göra det, men resultatet blir att ändringar i metoder slår igenom överallt om jag ändrar på ett ställe, så det har varit värt det. 
Ju större programmet blev, desto tydligare blev det för mig vilken stor nytta man har av att samla det där istället för att ha det utspritt lite här och var bakom varje sida. 
Nackdelen är att metoderna kanske ser onödigt krångliga ut. En annan effekt av det har blivit att Manager-klasserna kanske gör fel saker. Som jag har använt dem nu säkerställer dem att alla Travels och Users som skapas har rätt struktur (vilket känns vettigt), men den skriver också ut felmeddelanden till användaren, vilket kanske borde göras av sidan. Jag har bråkat lite med själv kring vad som är bästa lösningen, men det blev minstkod genom att göra på detta sätt i alla fall. 

Min största utmaning med projektet var att få till en vattentät hantering när det gäller att ha en “databas”-lista med resor (i TravelManager-klassen) och en lista med resor hos användaren. 
Jag skulle föredra att bara ha en lista (“databasen”), för att minska risken för fel att man missar att ändra på båda ställena när det sker tillägg/ändringar/borttag. 
Om jag hade gjort om projektet skulle jag inte haft någon lista med resor på varje “User”, utan istället sparat ner “User” som en property på varje resa i “databasen”. 
För att få ut en specifik users alla resor hade jag då gått till “databasen” och frågat efter det därifrån. Nackdelen med den lösningen vore att det förmodligen blev tyngre och längre laddtider, då man behöver gå igenom alla resor i “databasen” för att få ut sina resor. Kanske vore den bästa lösningen något mellanting – med en lista med resor på varje användare och att man sparar ner “User” för varje resa. 

Om man hade haft user på varje resa hade det också varit möjligt att få till en lösning som fungerar hela vägen gällande att lägga till default-pass på resor, även när man ändrar användarens “location” och om admin ändrar i en resa för en user. Hade man haft tillgång till vilken user som skapat resan vid ändring, hade man kunnat utgå från den userns location och räkna ut vilket typ av pass som ska läggas till i alla lägen. 

Jämfört med uppgiftsbeskrivningen har jag gjort några förändringar: 

För att enklare kunna hålla ihop användarens reselista med “databasen” valde jag att lägga till ID-nummer för varje resa. På så vis hade jag enklare att veta vilken resa man ändrade/tog bort.  

För att slippa skicka med vilken resa man valt till/från sidor valde jag att skapa en property i TravelManager som håller reda på vilken resa jag valt. Det blev likt tänket med att hålla koll på vilken användare som är inloggad, som används i UserManager. 

Sammanfattningsvis så har det varit ett roligt och utmanande projekt och jag har lärt mig mycket genom att göra det. 

André Jakobsson
