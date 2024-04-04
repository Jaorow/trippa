import React, { useState, useRef, useEffect } from "react";
import {
  View,
  Text,
  FlatList,
  StyleSheet,
  Platform,
  Modal,
  TouchableOpacity,
  Image,
  TextInput,
  Button
} from "react-native";
import { AntDesign } from "@expo/vector-icons";
import { FontAwesome5 } from "@expo/vector-icons";
import MapView, { Marker } from "react-native-maps";
import { FontAwesome } from "@expo/vector-icons";
import { getAllTrips } from "./services/ApiHandler";
import DateTimePicker from "@react-native-community/datetimepicker";
import { GooglePlacesAutocomplete } from "react-native-google-places-autocomplete";
import { ScrollView } from "react-native-gesture-handler";
import { ForeignObject } from "react-native-svg";
import AsyncStorage from "@react-native-async-storage/async-storage";

// * MAP FUNCTIONALITY * //
function MyMapComponent({ startLocation, endLocation }) {
  const { startLat, startLng } = startLocation;
  const { endLat, endLng } = endLocation;

  // Calculate the center point
  const centerLat = (startLat + endLat) / 2;
  const centerLng = (startLng + endLng) / 2;

  // Calculate the delta values for padding (adjust these values as needed)
  var latitudeDelta = Math.abs(startLat - endLat) * 2;
  var longitudeDelta = Math.abs(startLng - endLng) * 2;
  if (latitudeDelta == 0) {
    latitudeDelta += 0.00001;
  }
  if (startLng == endLat) {
    longitudeDelta += 0.00001;
  }

  // determine platform for custom pin
  const pinImage =
    Platform.OS === "ios"
      ? require("./assets/pin-ios.png")
      : require("./assets/pin-android.png");

  return (
    <MapView
      style={{ flex: 1, zIndex: -1, width: "100%", height: "100%" }}
      initialRegion={{
        latitude: centerLat,
        longitude: centerLng,
        latitudeDelta,
        longitudeDelta,
      }}
      zoomEnabled={false}
      scrollEnabled={false}
      mapType="standard" // standard, satellite, hybrid, terrain
      // TODO: in settings we can store a user cookie for settings,
      //. we could change this value easily depending on the cookie
    >
      {/* <Marker
        coordinate={{ latitude: startLat, longitude: startLng }}
        title="Start Location"
      />
      <Marker
        coordinate={{ latitude: endLat, longitude: endLng }}
        title="End Location"

      /> */}
      <Marker
        coordinate={{ latitude: startLat, longitude: startLng }}
        title="Start Location"
        centerOffset={{ x: 1, y: -8 }}
      >
        <Image source={pinImage} resizeMode="contain" style={styles.pinImage} />
      </Marker>
      <Marker
        coordinate={{ latitude: endLat, longitude: endLng }}
        title="Start Location"
        centerOffset={{ x: 1, y: -8 }}
      >
        <Image source={pinImage} resizeMode="contain" style={styles.pinImage} />
      </Marker>
    </MapView>
  );
}

// * DATE TIME FUNCTIONALITY * //
function formatDateTime(dateTimeString) {
  if (dateTimeString === undefined) {
    return null;
  }
  const date = new Date(dateTimeString);
  const formattedTime = format12HourTime(date);
  const formattedDate = `${date.getDate()}${getDaySuffix(date.getDate())}`;
  return `${formattedTime} on ${formattedDate}`;
}

function format12HourTime(date) {
  if (date === undefined) {
    return null;
  }
  const hour = date.getHours();
  const minute = date.getMinutes().toString().padStart(2, '0');
  const ampm = hour >= 12 ? "PM" : "AM";
  const formattedHour = hour % 12 === 0 ? 12 : hour % 12;
  return `${formattedHour}:${minute}${ampm}`;
}


// Helper function to get the day suffix (e.g., "st", "nd", "rd", "th")
function getDaySuffix(day) {
  if (day >= 11 && day <= 13) {
    return "th";
  }
  const lastDigit = day % 10;
  switch (lastDigit) {
    case 1:
      return "st";
    case 2:
      return "nd";
    case 3:
      return "rd";
    default:
      return "th";
  }
}


function SearchGoogleAutoComplete(props) {

    return (
      <GooglePlacesAutocomplete
        placeholder="Search"
        minLength={2} // minimum length of text to search
        autoFocus={true}
        returnKeyType={"search"} // Can be left out for default return key
        listViewDisplayed={"auto"} // true/false/undefined
        fetchDetails={true}
        onPress={(data, details = null) => {
          // 'details' is provided when fetchDetails = true
          props.notifyChange(
            details.geometry.location,
            details.formatted_address
          );
        }}
        query={{
          key: "AIzaSyDrwiWWzU9dTML6CrMVHgEx8ZrcRFunoa8",
          language: "en",
          location: "-40.900557,174.885971",
          radius: "1500000",
          components: "country:NZ",
        }}
        nearbyPlacesAPI="GooglePlacesSearch"
        debounce={300}
        styles={{
          container: { width: 300 }, // Check this line for width styles
          textInputContainer: { width: "100%" }, // Check this line for width styles
        }}
      />
    );
  }
  function haversine_distance(passenger, lat, long) {
    var R = 6371.0710; // Radius of the Earth in kms
    var rlat1 = passenger.lat * (Math.PI/180); // Convert degrees to radians
    var rlat2 = lat * (Math.PI/180); // Convert degrees to radians
    var difflat = rlat2-rlat1; // Radian difference (latitudes)
    var difflon = (long-passenger.lng) * (Math.PI/180); // Radian difference (longitudes)

    var d = 2 * R * Math.asin(Math.sqrt(Math.sin(difflat/2)*Math.sin(difflat/2)+Math.cos(rlat1)*Math.cos(rlat2)*Math.sin(difflon/2)*Math.sin(difflon/2)));
    return d;
  }

//* address functionality
function convertAddressApiJson(start, end) {
  // this was interesting to code...
  // FIXME: bit of a hacky way to do this, but it works
  // pass in start and end address json from google api
  // returns the highest address point, eg auckland to whangari would return auckland to whangari
  // however auckland to auckland would return address to address
  function findHighestMismatchElement(list1, list2) {
    let index = -1;
    for (let i = list1.length - 1; i >= 0; i--) {
      if (list1[i] !== list2[i]) {
        index = i;
        break;
      }
    }
    return index;
  }  // console.log(start);
  var startComp = start.split(",");
  var endComp = end.split(",");

  var index = findHighestMismatchElement(startComp, endComp);

  // return [startComp[index], endComp[index]];
  // TODO: change this to return index, just need to split the 2nd index, split off the post code...
  return startComp[0];
}

function algorithmToSortArray(primArr,arr, compare = defaultCompare) {
  const { length } = arr;
  let minIndex;
  for (let i = 0; i < length - 1; i++) {
      minIndex = i;
      for (let j = i; j < length; j++) {
          if (compare(arr[minIndex], arr[j]) === Compare.BIGGER_THAN) {
              minIndex = j;
          }
      }
      if (i !== minIndex) {
          swap(arr, i, minIndex);
          swap(primArr, i, minIndex)
      }
  }
  return primArr;
}

function swap(arr, a, b) {
  const temp = arr[a];
  arr[a] = arr[b];
  arr[b] = temp;
}
const Compare = {
  LESS_THAN: -1,
  BIGGER_THAN: 1
};

function defaultCompare(a, b) {
  if (a === b) {
      return 0;
  }
  return a < b ? Compare.LESS_THAN : Compare.BIGGER_THAN;
}

function Search(){
    const [tripsData, setTripsData] = useState([]);
    const [date, setDate] = useState('');
    const [numPeople, setNumPeople] = useState('');
    const [startLocation, setStartLocation] = useState(null);
    const [endLocation, setEndLocation] = useState(null);
    const [startAddress, setStartAddress] = useState("");
    const [endAddress, setEndAddress] = useState("");
    const [isDateModalVisible, setDateModalVisible] = useState(false);
    const [isNumPeopleModalVisible, setNumPeopleModalVisible] = useState(false);
    const [dateTime, setDateTime] = useState(new Date()); // this will preset it to "today's" date
    const [isDateTimePickerVisible, setDateTimePickerVisible] = useState(false);
    const [sortedTrips, setsortedTrips] = useState([]);
    const [isModalVisible, setModalVisible] = useState(false);

    useEffect(() => {
      // This code will run when the component is mounted (similar to componentDidMount in class components)
      getTrips();
    }, []);

    const toggleModal = () => {
      setModalVisible(!isModalVisible);
    };


    const handleStartLocationChange = (location, address) => {
        setStartLocation(location);
        setStartAddress(address);
      };
    
      const handleEndLocationChange = (location, address) => {
        setEndLocation(location);
        setEndAddress(address);
      };
    const handleItemPress = (item) => {
        // setSelectedItem(item);
      };

    function getTrips(){

        getAllTrips().then((trips) => {
        console.log(trips.length + " Trip(s) fetched");
        setTripsData(trips);
      });
    }
    function sortTrips(trips){
      const distList = [];
      const tripList = [];
      const d = new Date(dateTime);
      const date = d.getFullYear()+'/'+(d.getMonth()+1)+'/'+d.getDate();
      trips.forEach(trip => {
        const dt = new Date(trip.dateTime);
        const dateT = dt.getFullYear()+'/'+(dt.getMonth()+1)+'/'+dt.getDate();
        console.log(numPeople <= trip.maxRiders && date == dateT)
        if (numPeople <= trip.maxRiders && date == dateT){
          // check date is correct, check driver has space for passengers.
          const dStart = haversine_distance(startLocation, trip.startLatitude, trip.startLongitude);
          const dEnd = haversine_distance(endLocation, trip.endLatitude, trip.endLongitude);
          console.log(startLocation, trip.startLatitude, trip.startLongitude);
          console.log(endLocation, trip.endLatitude, trip.endLongitude);
          console.log(dStart, dEnd)
          if ((trip.detourRange / 1000) > dStart && trip.detourRange /1000 > dEnd){
            tripList.push(trip)
            distList.push(dStart + dEnd)
          }
        }
        
      });
      const sortedTrips = algorithmToSortArray(tripList, distList)
      return sortedTrips
    }
    const handleSearch = () => {

        if (startLocation != null && endLocation != null){
            const sort = sortTrips(tripsData);
            setsortedTrips(sort);
            toggleModal();
        }

      };
    return (
        <View style={styles.container}>

      <View style={styles.locationContainer}>
          <Text>Start Location:</Text>
          <SearchGoogleAutoComplete
            styles={{
              container: { width: 300, zIndex: 9999 },
              textInputContainer: { width: "100%" },
              listView: { backgroundColor: "white" },
            }}
            notifyChange={handleStartLocationChange}
          />
        </View>
        <View style={styles.locationContainer1}>
          <Text>End Location:</Text>
          <SearchGoogleAutoComplete
            styles={{
              container: { width: 300, zIndex: 9999 },
              textInputContainer: { width: "100%" },
              listView: { backgroundColor: "white" },
            }}
            notifyChange={handleEndLocationChange}
          />
        </View>
          {/* <View style={styles.buttonContainer}> */}

        <Modal
        animationType="slide"
        transparent={false}
        visible={isModalVisible}
        onRequestClose={toggleModal}
      >
        <View style={styles.modalContainer}>
          {/* FlatList inside the modal */}
          {sortedTrips.length > 0 ? (
                      <FlatList
                      style={{marginTop: 50}}
                      data={sortedTrips}
                      renderItem={({ item }) => (
                        <TouchableOpacity
                          key={item.tripID}
                          onPress={() => handleItemPress(item)}
                        >
                          <View style={styles.tripCard}>
                            <MyMapComponent
                              startLocation={{
                                startLat: item.startLatitude,
                                startLng: item.startLongitude,
                              }}
                              endLocation={{
                                endLat: item.endLatitude,
                                endLng: item.endLongitude,
                              }}
                            />
                            {/* <Text style={styles.dateTime}>
                              {formatDateTime(item.dateTime)}
                            </Text> */}
                            <Text style={styles.riderInfo}>
                              <FontAwesome5
                                name="car-side"
                                size={14}
                                color={highlight_color}
                              />
                              {item.currentRiders}/{item.maxRiders}
                            </Text>

                            <View style={styles.cardLocation}>
                              <Text style={styles.price}>${item.price}</Text>
                              <Text style={styles.location}>
                                {/* {convertAddressApiJson(item.startLocation, item.endLocation)[0]} */}
                                {convertAddressApiJson(
                                  item.startLocation,
                                  item.endLocation
                                )}
                                <AntDesign
                                  name="arrowright"
                                  size={13}
                                  color={highlight_color}
                                />
                                {/* {convertAddressApiJson(item.startLocation, item.endLocation)[1]} */}
                                {convertAddressApiJson(
                                  item.endLocation,
                                  item.startLocation
                                )}
                              </Text>
                            </View>
                          </View>
                        </TouchableOpacity>
                      )}
                      keyExtractor={(item) => item.tripID}
                      vertical
                      showsVerticalScrollIndicator={false}
                    />
          ) : (
            <View style={styles.noTripContainer}>
              <Text style={styles.noTrip}>No trips found</Text>
            </View>
          )} 
          <TouchableOpacity onPress={toggleModal} style={[styles.button, styles.closeButton]}>
            <Text style={styles.buttonText}>Close</Text>
          </TouchableOpacity>   
          </View>
          </Modal>
          {/* Date time picker */}
          <View style={styles.dateTimeContainer}>
            <Text>Select Date</Text>
            <TouchableOpacity onPress={() => setDateTimePickerVisible(true)}>
              <Text style={styles.dateEmoji}>&#128198;</Text>
            </TouchableOpacity>
            {isDateTimePickerVisible && (
                <DateTimePicker
                value={dateTime}
                mode="datetime"
                is24Hour={false}
                display="default"
                onChange={(event, selectedDate)=>{
                  setDateTimePickerVisible(false); // Hide the picker after selecting a date
                  const currentDate = selectedDate || dateTime;
                  console.log(currentDate);
                  setDateTime(currentDate);
                }}
                />
            )}
        </View>
        {/* number of riders*/}
        <View style={styles.numRidersContainer}>
          <Text>No. of Riders</Text>
        <TextInput
            style={styles.inputRiders}
            value={numPeople}
            onChangeText={text => setNumPeople(text)}
            keyboardType="numeric"
            maxLength={1}
            defaultValue="1"
            placeholder="1"
        />
          </View>
        <TouchableOpacity onPress={handleSearch} style={styles.button}>
          <Text style={styles.buttonText}>Search</Text>
        </TouchableOpacity>
        </View>
    )
};
const paddingValue = 3;
const highlight_color = "#007c3e";

const styles = StyleSheet.create({
    container: {
      flex: 1,
      alignItems: "center",
      paddingTop: 20,
      backgroundColor: "#f2f2f2",
    },
    modalContainer:{
      flex: 1,
      backgroundColor: "#f2f2f2",
    },
    dateEmoji:{
      fontSize: 50
    },
    noTripContainer: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
    },
    noTrip: {
      fontSize: 18,
    },
    buttonText: {
      color: "white",
      fontSize: 16,
      textAlign: "center",
    },
    input: {
      width: 300,
      height: 40,
      borderColor: "#ddd",
      borderWidth: 1,
      paddingHorizontal: 10,
      marginBottom: 10,
      borderRadius: 5,
      backgroundColor: "white",
      fontSize: 16,
    },
    inputRiders: {
      width: 40,
      height: 40,
      borderColor: "#ddd",
      borderWidth: 1,
      paddingHorizontal: 10,
      marginBottom: 10,
      borderRadius: 5,
      backgroundColor: "white",
      fontSize: 30,
    },
    button:{
      backgroundColor: highlight_color,
      paddingVertical: 10,
      paddingHorizontal: 20,
      borderRadius: 5,
      marginTop: 20,
      elevation: 2,
      shadowOffset: { width: 1, height: 1 },
      shadowColor: "#333",
      shadowOpacity: 0.3,
      shadowRadius: 2,
      width: "50%",
      alignSelf: "center", 
    },
    dateTimeContainer: {
      width: "100%",
      alignItems: "center",
      marginTop: 300,
    },
    numRidersContainer: {
      width: "100%",
      alignItems: "center",
      marginTop: 30,
    },
    locationContainer: {
      width: "100%",
      overflow: "visible",
      alignItems: "center",
      marginTop: 150,
      zIndex: 9999,
      position: "absolute",
    },
    locationContainer1: {
      width: "100%",
      overflow: "visible",
      alignItems: "center",
      marginTop: 250,
  
      zIndex: 9998,
      position: "absolute",
    },
    location: {
      position: "absolute",
      bottom: paddingValue,
      left: paddingValue,
  
      overflow: "hidden",
      backgroundColor: "white",
      borderWidth: 0.6,
      borderColor: "#ccc",
      borderRadius: 8,
      color: "black",
      padding: 4,
      fontSize: 12,
    },
    tripCard: {
      backgroundColor: "white",
      borderWidth: 0.6,
      borderColor: "#ccc",
      borderRadius: 8,
      overflow: "hidden",
      marginVertical: 8,
      height: 200,
      minWidth: "95%",
    },
    pinImage: {
      ...Platform.select({
        ios: {
          width: 50,
          height: 50,
        },
        android: {
          width: 30,
          height: 30,
        },
      }),
    },
    price: {
      position: "absolute",
      bottom: 25 + paddingValue,
      left: paddingValue,
      overflow: "hidden",
      backgroundColor: "green",
      color: "white",
      borderWidth: 0.6,
      borderColor: "#ccc",
      borderRadius: 8,
      padding: 4,
      fontSize: 12,
    },
    riderInfo: {
      position: "absolute",
      bottom: paddingValue,
      right: paddingValue,
      overflow: "hidden",
      backgroundColor: "white",
      borderWidth: 0.6,
      borderColor: "#ccc",
      borderRadius: 8,
      color: "black",
      padding: 4,
      fontSize: 12,
    },
    closeButton: {
      marginBottom: 50,
    }
  });
export default Search;