A program tartalmaz egy SETUP f�jlt, amivel elv�gezhet� a telep�t�s. A programnak vannak bizonyos alap�rtelmezett kezd� param�terei, amik jelen pillanatban m�g nem v�ltoztathat�k. Ilyen p�ld�ul az alap�rtelmezett el�r�si �t megad�sa.
A Google Drive haszn�lata miatt felt�tlen�l sz�ks�ges egy akt�v felhaszn�l�i fi�k megl�te. A program jelenleg tartalmaz egy credentials.json f�jlt, ami a davidteszt100@gmail.com (jelsz�: 123abc...) fi�khoz enged hozz�f�r�st, �gy az els� h�napokban a felhaszn�l�nak nincs k�l�n�sebb teend�je. A tesztel�st �s az �rt�kel�st k�vet�en a fi�kot megsz�ntetem, �gy sz�ks�g lesz egy saj�t fi�kra �s ott a projekt enged�lyez�s�re. Ennek l�p�sei a k�vetkez�k:
1.	Jelentkezz�nk be a Google Drive fi�kunkba �s navig�ljunk a https://console.developers.google.com/ oldalra.
2.	A Credentials alatt k�sz�ts�nk egy �j OAuth client ID t�pus� credentialst.
3.	V�lasszuk ki az �jonnan l�trehozott credentialt �s t�lts�k le a JSON f�jlt. 
4.	Nevezz�k �t credentials.json-ra a let�lt�tt f�jlt �s �rjuk fel�l vele az alkalmaz�s mapp�j�ban l�v� ugyanolyan nev� dokumentumot. 
A m�sodik l�p�sben sz�ks�ges ugyanilyen metodik�val elv�gezni a Dropbox oldal�n az enged�lyez�st. A tesztel�shez a Google bejelentkez�st haszn�ltam. L�p�sei:
1.	Jelentkezz�nk be Dropboxra �s menj�nk a https://www.dropbox.com/developers/apps 
2.	Hozzunk l�tre egy �j appot! Fontos, hogy a Dropbox apit �s ezen bel�l a Full Dropbox lehet�s�get v�lasszuk!
3.	Sz�ks�g�nk lesz egy access tokenre, amit gener�lni is tudunk a l�trehozott app Settings r�sz�ben Ezek ut�n sz�ks�g�nk lesz 
