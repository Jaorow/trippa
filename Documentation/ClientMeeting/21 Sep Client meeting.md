## Conal's Notes on Trippr
#### Desired features
- Driver registration:
	- Check if car is real
		- cross reference with some database
	- Check if licenses are valid (via API)
	- Check if car registration is valid (via API)
		- https://www.carjam.co.nz/dev:category?t=api
	- Make a request to join a trip rather than proceeding straight to payment
		- Driver can check the user booking the trip i.e. can deny based on poor reviews
	- Make driver's not see form for drivers available
	- Login using gmail, facebook etc.

- Trips:
	- Show detour to pick up distance with a checkbox for none
	- Add messaging
	- Add reviews
		- showing rating out of 5 stars
		- show user reviews
	- Creating trips
		- Specify AM or PM

#### Notes on design
- When creating a trip, a non-driver user should be redirected to "register/sign up as driver" page rather than proceeding straight to "fill out form" page

### Team 26 (WindWheels) Design notes
- Messaging between users - group chat with all drivers and passengers
- Using google API for maps and map searching

- When registering they have bio where they can write comments about themselves, API to automatically fill out address when searching in search bar. 
- Sort button at the top, (sort by distance price etc) Car details seats avaiilable and reviews about the driver when looking at trips When registering as driver, choose car brand colour and brand seats etc. Able to show the trip on the map when click on it.
- When user first opens app they can see all the rides on a single map. 
- Can click on marker that is on map that allows the driver to jump to google maps Button that goes to current location on map (e.g in google maps, recenter and current position)
- Messaging - group chat with all drivers and passengers. History page of user reviews when clicking on their profiles similar to reviews of a restaurant
### Team 30 Design notes
**User Experience**
- Sign in page
	- log in using phone number/email
	- sign in using google/facebook
	- forget password button
	- sign up button
- Trip page
	- tags to show preferences e.g. pet friendly. 