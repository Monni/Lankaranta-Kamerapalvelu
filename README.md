# Lankaranta kamerapalvelu
## Tietoa ohjelmasta
Ohjelmaa on toteutettu tarpeesta luoda toimiva ja luotettava valvontakameraratkaisu nykyisen internetiin kytketyn, vaihtelevasti toimivan, riistakameran vastineeksi. Ohjelman tarkoitus on taltioida valvontakameralta saapuvat kuvat ja tallentaa näiden tiedot tietokantaan, minkä kautta niitä on helppo hallinnoida ja lähettää eteenpäin valittuihin sähköposteihin.
Päätelaitteeksi rakennetaan tablettimainen, pöydällä pidettävä laite, mikä käynnistyessään yhdistää automaattisesti projektia pyörittävälle palvelimelle kokoruutunäkymässä. Ohjelman käyttöliittymä on suunniteltu skaalautumaan kyseiselle päätelaitteelle.

!(https://github.com/Monni/aspnet-project/tree/master/WebApplication3/WebApplication3/demo2.JPG)[https://github.com/Monni/aspnet-project/tree/master/WebApplication3/WebApplication3/Demo2.JPG]

!(https://github.com/Monni/aspnet-project/tree/master/WebApplication3/WebApplication3/demo1.JPG)[https://github.com/Monni/aspnet-project/tree/master/WebApplication3/WebApplication3/demo1.JPG]

### Toteutetut toiminnalliset vaatimukset
* Päälle- ja pois päältä kytkettävä Live -tila, milloin päälläollessaan noudetaan uusimmat kuvat kameralta lähetysnopeuden mukaan
* Kuvatietojen ja sijaintien nouto tietokannalta
* Kaikkien kuvien nouto
* Kuvien nouto, missä liikettä on havaittu
* Kuvien nouto, missä liikettä ei ole havaittu
* Noudettujen kuvien katselmointi
* Yksittäisen kuvan valitseminen ja poisto tietokannasta
* Yksittäisen kuvan valitseminen ja lähetys haluttuihin sähköposteihin

### Toteuttamatta jääneet toiminnalliset vaatimukset
* Kuvien nouto valitun päivämäärän mukaan
* Indikaattori näyttämään loppukäyttäjälle, onko kamera verkossa

### Ei-toiminnalliset vaatimukset, reunaehdot
* Käyttäjän salasanaton todennus (token?)
* Useamman käyttäjän yhtäaikainen tuki, maksimissaan viidelle yhtäaikaiselle sisäänkirjautumiselle
* Kuvan automaattinen lähetys sähköposteihin, jos liikettä havaittu

### Jatkokehitys
Ohjelmisto on toteutettu tarpeeseen ja vaikka ollen jo käyttökuntoinen loppukäyttäjälle on tietoturvapuoli saatava ensin kuntoon. Näyttöpääte tulisi saada autentikoitua (kuvat ei kaikille) ilman salasanaa käytön helppouden vuoksi. Seuraavina toissijaisina kehitysaiheina tulee toteuttamatta jääneet toiminnalliset vaatimukset sekä käyttöliittymän viilaaminen "kaupallisemmaksi".

## Asennus
### MySQL
Ohjelmiston kuvien hallinnointi tapahtuu MySQL tietokannassa. Tätä varten tietokanta on asennettava.
*  (Asennusohjeet Linuxille) [https://www.linux.fi/wiki/MySQL:n_k%C3%A4ytt%C3%B6%C3%B6notto]
*  (Asennusohjeet Windowsille) [http://dev.mysql.com/doc/refman/5.7/en/windows-installation.html]

#### Käyttöönotto
Kirjaudu sisään MySQL -ohjelmaan pääkäyttäjänä (root)
```
$ mysql -u root -p
Enter password:
```
```
Welcome to the MySQL monitor. Commands end with ; or \g.
Your MySQL connection id is 3 to server version: 3.23.56
Type 'help;' or '\h' for help. Type '\c' to clear the buffer.
mysql>
```
Luo tietokanta nimellä lankaranta
```
mysql> CREATE DATABASE lankaranta;
Query OK, 1 row affected (0.01 sec)
```
Varmista, että tietokanta on luotu
```
mysql> SHOW DATABASES;
+------------+
| Database   |
+------------+
| lankaranta |
| mysql      |
| test       |
+------------+
3 rows in set (0.00 sec)
```
Siirry juuri luotuun kantaan
```
mysql> use lankaranta;
Reading table information for completion of table and column names
You can turn off this feature to get a quicker startup with -A

Database changed
mysql>
```
Luo uusi käyttäjä ja mahdollista sille pääsy lankaranta -kantaan.
```
mysql> GRANT ALL PRIVILEGES ON lankaranta.* TO aspnet@%
-> IDENTIFIED BY 'valitsemasi_salasana' WITH GRANT OPTION;
Query OK, 0 rows affected (0.00 sec)
```
Luo uusi pöytä lankaranta -kantaan
```
CREATE TABLE images
(
ID int NOT NULL AUTO_INCREMENT,
datetime datetime,
imagepath varchar(255),
movement boolean,
PRIMARY KEY (ID)
)
```
Luo toinen pöytä lankaranta -kantaan
```
CREATE TABLE latest
(
ID int DEFAULT=1,
datetime datetime,
imagepath varchar(255),
movement boolean
)
```
Tämän jälkeen siirry projektin web.configiin, missä muuta connectionStringsin sisältä löytyvät kentät "user id" sekä "password" vastaamaan tietokantaa luodessa syötettyjä tietoja.

Tietokanta ja tietokantayhteys ovat nyt valmiita käytettäväksi. 
#### Testidatan lisäys tietokantaan
Käynnistä projekti haluamallasi selaimella ja avaa web form nimeltään DatabaseClient.aspx.
Sivulle aukeaa client, millä lähetetään kuvat muine tietoineen tietokantaan. Klikkaa painiketta "Browse..." ja navigoi projektin juuresta löytyvään "Demo_pictures" -kansioon. Lähetä formilla kansiosta löytyvät kuvat tietokantaan. Demokuvissa voit itse määritellä onko kuvissa havaittu liikettä.
