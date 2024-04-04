```
27/7/2023

AUTHORS:
Henry
Jamie
```
## technology 
- Dotnet backend, hosted on azure

## setup
```bash
dotnet new webapi -o datadons-backend
cd datadons-backend
dotnet add package Microsoft.EntityFrameworkCore.InMemory
code -r ../datadons-backend
```
## Requirements
- users
	- Classic user backend requirements
		- UserID
		- Username
		- Password (hashed)
		- Email
		- Authentication (TBC) [[Safety]]
		- PH
		- preferences 
			- EG: no pets, masks exc...
	- split into two classes, drivers and riders
		- users can switch between drivers and riders easily
	- stores reviews gained and reviews got
		- reviewID
		- reviews are classed as `outgoing` or `incoming`
		- rating
		- description
		- set complements
			- eg like Uber - driving/communication/navigation exc
		- bad review system
			- many bad reviews will result in some sort of flag
				- TODO, staff to review? Community based?

## Driver
- car details
	- make
	- model
	- colour
	- Number plate
- Drivers licence
- regular trips
	- Trips DB
		- DriverID
		- Reoccurring Dates (TODO)
		- startpoint
		- endpoint

## Rider
- all rider information is in user... no extra information needed


## Trips
*different to reoccurring trips*
- driver id
- date-time
- max riders
	- isFull
- list of riderID
	- rider can book more than one seat per riderID
- price
- startPoint
- endPoint
- duration
	- eg driver starts ride
	- for stats

