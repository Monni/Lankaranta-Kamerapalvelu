# Lankaranta kamerapalvelu
## Asennus
### MySQL
Ohjelmiston kuvien hallinnointi tapahtuu MySQL tietokannassa. Tätä varten tietokanta on asennettava.
* [https://www.linux.fi/wiki/MySQL:n_k%C3%A4ytt%C3%B6%C3%B6notto] (Asennusohjeet Linuxille)
* [http://dev.mysql.com/doc/refman/5.7/en/windows-installation.html] (Asennusohjeet Windowsille)

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
```
```
```
```
```
```
```
```
```
```
```
```




### Konfiguroitavat asiat
