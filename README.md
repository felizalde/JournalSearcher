
# Journal Searcher


## 1- Steps to run the containers

```
docker-compose up -d
```

## 2- Create postgres server

Before run the site, you will need to set up the journals database.


1. Go to [PgAdmin](http://localhost:5050/) -> http://localhost:5050/
    Password is `admin` (very complex password)

2. Create a new server called `journals` and in the Connection tab set the follows settings:

```
    hostname/address = postgres
    port: 5432
    username: journalsAdmin
    password: JuanRomanRiquelme
```

3. Create database called `JournalsRecommender`

4. Copy backup file into the container

```
docker cp SRC_FILE pgadmin_container:/tmp/JournalsRecommender.backup

-- SRC_FILE: Is the location of the backup file (e.g: C:/temp/JournalsRecommender.backup )

```

5. Restore database:
    - Right-Click over the db `JournalsRecommender`
    - Click on `Restore..`
    - Set as Format = Custom or tar, and FileName = /tmp/JournalsRecommender.backup
    - Click on Restore.

## 3- Open Swagger and generate the index

1. Go to [Swagger](http://localhost:5000/swagger/index.html) => http://localhost:5000/swagger/index.html

2. Generate a token using `api/auth` and some of the follows credentials:

| username  | password  |  role |
|---|---|---|
| efealde@gmail.com  |  Riquelme1. | Admin  |
| nicoleiva08@gmail.com  |  Riquelme1. | Admin  |
| diegoalonso1709@gmail.com  |  Riquelme1. | Admin  |
| guest1@jr10.com  |  Riquelme1. | User  |
| guest2@jr10.com  |  Riquelme1. | User  |

_*Create the index, and run the scrappers requires Admin privileges_

3. Add the token generated to the Authorize button.

4. Run `api/index/create` to create the index.

5. Run `api/index/fill` to fill the index. The last version in the backup file is 2. So, you can use 2 as the version number.


## 4- Run the [site](http://localhost:5000/) and make you first search.


---

---

### How to create db backup and get the file outside of the docker container

1. Go to pgAdmin, and create the backup. Save it into folder `/tmp/JournalsRecommender.backup`

2. Copy the backup generated to the host machine using:

```
docker cp pgadmin_container:/tmp/db-backup/{FILENAME} {DEST_FOLDER}

-- FILENAME: replace with the name of the backup.
-- DEST_FOLDER: is the destination folder into the host machine.
```


