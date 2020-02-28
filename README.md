# MovieRental
Project to manage small video rental store, powered by Kevin Omar Alvarez.

## Before run the project you would need

* SQLEXPRESS
* NET Core 3.1 (LTS)

> Note: If you want to change database connection go to `appsettings.Development.json`

## Database config

For the database I prefer to use migrations, so you only need to run the following command into the project folder to step it up `C:\...\MovieRental\MovieRental`

```console
> dotnet ef database update
```

> Note: If you use console method you need dotnet tool globally

The other way is to use de PM Console from VS2019

```console
PM> cd .\MovieRental
PM > Update-Database
```

The migration include seed data for Roles, Users, TransactionType and Movies. The name of the database created would be `MovieRental` after run the command. 

You can use the following users for the test:

* Administrator
    > User: admin@mr.dev
    > Pass: kevin20*
    
* Client
    > User: client@mr.dev
    > Pass: 1234

### API Authentication

The API use Json Web Token as Security mechanism, so you need to add the `Authorization` header to the request.

> Bearer {{Token provided on auth method}}

### Swagger UI

For the API doc you can check the available methods enter the following URL `https://localhost:44329/swagger/index.html`

### Postman Collection

For import the postman collection use the following link

> https://www.getpostman.com/collections/612aedeff1bc35cb4307
