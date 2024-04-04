import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Modal, TouchableWithoutFeedback, Keyboard } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { navigationRef } from './NavigationService'; // Import the navigationRef
import { AddDriver, getUserId } from './services/ApiHandler'
import { FontAwesome } from "@expo/vector-icons";


function AccountScreen() {
  const [license, setLicense] = useState('');
  const [carModel, setModel] = useState('');
  const [carColor, setColor] = useState('');
  const [carMake, setMake] = useState('');
  const [carType, setType] = useState('');
  const [plateNumber, setPlateNumber] = useState('');
  
  const [isRegistering, setItRegistering] = useState(false);
  
  const [modalVisible, setModalVisible] = useState(false);
  
  const handleLogout = () => {
    // TODO: redirect to login...
    AsyncStorage.removeItem('user')
    .then(() => {
      navigationRef.current?.navigate('Home');
    })
    .catch((err) => {
      console.log(err);
    });
  };
  
  const registerDriver = async () => {
    try {
      // Create a driver (done by adding driver to user)
      const newDriver = {
        licenseNumber: license,
        carModel: carModel,
        carColor: carColor,
        carMake: carMake,
        carType: carType,
        plateNumber: plateNumber
      };
      const user = await AsyncStorage.getItem('user');
      const userid = await getUserId(user);
      
      const result = await AddDriver(userid, newDriver);
      console.log(result)
      // TODO: handle json parse error.....
      if (result === "returned data"){
        console.log("Driver Created");
        setItRegistering(!isRegistering);
        setLicense("");
        setModel("");
        setColor("");
        setMake("");
        setType("");
        setPlateNumber("");
      }
      if (result && result.license === license) {
        alert("License already in use");
      } else {
        console.log("Driver Created");
        setItRegistering(!isRegistering);
        setLicense("");
        setModel("");
        setColor("");
        setMake("");
        setType("");
        setPlateNumber("");
      }
    } catch (error) {
      // Handle the error here
      if (
        error instanceof Error &&
        error.message.includes(
          "The instance of entity type 'Driver' cannot be tracked because another instance with the same key value for {'UserId'} is already being tracked."
        )
      ) {
        // Handle the specific error here
        alert("USER IS A DRIVER ALREADY")

      }else{
      console.error(error);
      }
    } finally {
      console.log("Driver Created");
      setItRegistering(setItRegistering(false));
      setLicense("");
      setModel("");
      setColor("");
      setMake("");
      setType("");
      setPlateNumber("");
    }
  };
  

  return (
    <View style={styles.container}>
      <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
        <TouchableOpacity
          style={styles.button}
          onPress={handleLogout}
        >
          <Text style={styles.buttonText}>
              Logout
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={styles.button}
          onPress={() => setModalVisible(true)}
        >
          <Text style={styles.buttonText}>
            Register as a Driver
          </Text>
        </TouchableOpacity>
      </View>
      <Modal
        animationType="fade"
        transparent="false"
        visible={modalVisible}
        onRequestClose={() => {
          setModalVisible(!modalVisible);
        }}
      >
      <TouchableWithoutFeedback onPress={() => Keyboard.dismiss()}>
        <View
          style={modal.modalContainer}
          onPress={() => setModalVisible(!modalVisible)}
          
        >
          <View style={Modal.modalContent}>
            <Text style={styles.header}>Register as a Driver</Text>
            <TouchableOpacity
                style={modal.closeButton}
                onPress={() => setModalVisible(false)}
              >
                <FontAwesome
                  style={modal.closeButtonIcon}
                  name="close"
                  size={27}
                />
            </TouchableOpacity>
            <Text>Enter your details below</Text>
              <View style={Modal.viewBox}>
 
                <TextInput
                  style={styles.input}
                  placeholder="Car Model"
                  autoCapitalize="words"
                  onChangeText={(text) => setModel(text)}
                  value={carModel}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Car Colour"
                  autoCapitalize="words"
                  onChangeText={(text) => setColor(text)}
                  value={carColor}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Car Make"
                  autoCapitalize="words"
                  onChangeText={(text) => setMake(text)}
                  value={carMake}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Car Type"
                  autoCapitalize="words"
                  onChangeText={(text) => setType(text)}
                  value={carType}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Plate Number"
                  autoCapitalize="characters"
                  onChangeText={(text) => setPlateNumber(text)}
                  value={plateNumber}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Driver's Lincense"
                  autoCapitalize="characters"
                  onChangeText={(text) => setLicense(text)}
                  value={license}
                />
                <TouchableOpacity
                  style={styles.button}
                  onPress={registerDriver}
                >
                  <Text style={styles.buttonText}>
                    Submit
                  </Text>
                </TouchableOpacity>
              </View>
          </View>
        </View>
        </TouchableWithoutFeedback>
      </Modal>
    </View>
  );
}


const highlight_color = '#007c3e';

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '100%',
        color: "black",
    },
    header: {
        fontSize: 24,
        marginBottom: 20,
    },
    input: {
        width: 300,
        height: 40,
        borderColor: 'gray',
        borderWidth: 1,
        paddingHorizontal: 10,
        marginBottom: 10,
        color: "black",
        borderRadius: 5,
    },
    button: {
        backgroundColor: highlight_color,
        paddingVertical: 10,
        paddingHorizontal: 20,
        borderRadius: 5,
        marginBottom: 25
    },
    buttonText: {
        color: 'white',
        fontSize: 16,
    },
    toggleText: {
        marginTop: 10,
    },
});

const modal = StyleSheet.create({
  modalContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(0, 0, 0, 0)",
    flexDirection: "column",
    paddingBottom: "40%",
  },
  modalContent: {
    // backgroundColor: "white",
    padding: 20,
    borderRadius: 10,
    width: "100%",
    marginTop: 500,
    height: "100%",
    overflow: "hidden",
  },
  closeButton: {
    position: "absolute",
    top: -10,
    right: 0,
    zIndex: 1,
    width: 40,
    height: 40,
    alignSelf: "center",
    marginTop: 10,
    color: highlight_color,
    justifyContent: "center",

    // * to see size of button
    // borderWidth: 1,
    // borderColor: 'black',
  },
  closeButtonIcon: {
    alignSelf: "center",
    color: highlight_color,
  },
  closeButton: {
    position: "absolute",
    top: -10,
    right: 0,
    zIndex: 1,
    width: 40,
    height: 40,
    alignSelf: "center",
    marginTop: 10,
    color: highlight_color,
    justifyContent: "center",

    // * to see size of button
    // borderWidth: 1,
    // borderColor: 'black',
  },
  closeButtonIcon: {
    alignSelf: "center",
    color: highlight_color,
  },


})

export default AccountScreen;
