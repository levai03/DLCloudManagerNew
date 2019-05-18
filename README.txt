A program tartalmaz egy SETUP fájlt, amivel elvégezhetõ a telepítés. A programnak vannak bizonyos alapértelmezett kezdõ paraméterei, amik jelen pillanatban még nem változtathatók. Ilyen például az alapértelmezett elérési út megadása.
A Google Drive használata miatt feltétlenül szükséges egy aktív felhasználói fiók megléte. A program jelenleg tartalmaz egy credentials.json fájlt, ami a davidteszt100@gmail.com (jelszó: 123abc...) fiókhoz enged hozzáférést, így az elsõ hónapokban a felhasználónak nincs különösebb teendõje. A tesztelést és az értékelést követõen a fiókot megszûntetem, így szükség lesz egy saját fiókra és ott a projekt engedélyezésére. Ennek lépései a következõk:
1.	Jelentkezzünk be a Google Drive fiókunkba és navigáljunk a https://console.developers.google.com/ oldalra.
2.	A Credentials alatt készítsünk egy új OAuth client ID típusú credentialst.
3.	Válasszuk ki az újonnan létrehozott credentialt és töltsük le a JSON fájlt. 
4.	Nevezzük át credentials.json-ra a letöltött fájlt és írjuk felül vele az alkalmazás mappájában lévõ ugyanolyan nevû dokumentumot. 
A második lépésben szükséges ugyanilyen metodikával elvégezni a Dropbox oldalán az engedélyezést. A teszteléshez a Google bejelentkezést használtam. Lépései:
1.	Jelentkezzünk be Dropboxra és menjünk a https://www.dropbox.com/developers/apps 
2.	Hozzunk létre egy új appot! Fontos, hogy a Dropbox apit és ezen belül a Full Dropbox lehetõséget válasszuk!
3.	Szükségünk lesz egy access tokenre, amit generálni is tudunk a létrehozott app Settings részében Ezek után szükségünk lesz 
