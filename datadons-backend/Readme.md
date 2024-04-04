## Running

To run the backend, run the following commands:

- insure you are in the datadons-backend directory `cd datadons-backend`

```sh
dotnet run
```

for swagger visit `https://localhost:5001/swagger`

## DB instructs

When changing models and contexts run the following commands:

```sh
dotnet ef migrations add MyNewChanges
dotnet ef database update
```

this changes the database to match the new models and contexts.

# COMPILE FOR HOST!!
    
```sh
dotnet publish -c Release -o ./publish
```
then move sqlite into publish folder

```sh
cp  ./DonsDb.sqlite ./publish/DonsDb.sqlite
```
## Testing with Swagger

-[x] /api/GetVersion
-[ ] /api/AddUser
-[ ] /api/GetUser/{id}
-[ ] /api/GetAllUsers
-[ ] /api/{userId}/add-driver
-[ ] /api/{userId}/driver
-[ ] /api/{userId}/setAsDriver
-[x] /api/GetTrip/{id}
-[x] /api/GetAllTrips
-[x] /api/AddTrip
-[x] /api/UpdateTrip
-[x] /api/GetAllTripsBy/{driverID}
-[ ] /api/Trips/search