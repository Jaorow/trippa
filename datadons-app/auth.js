import React, { useState } from "react";
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  Keyboard,
  TouchableWithoutFeedback,
  Image,
  KeyboardAvoidingView,
} from "react-native";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { getUserName, AddUser } from "./services/ApiHandler";
import logo from "./assets/logo.png";
import login from "./assets/login.png";

function LoginRegister({ onLoginSuccess }) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [passwordVerification, setPasswordVerification] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [isRegistering, setIsRegistering] = useState(false);
  const [page, setPage] = useState("welcome"); // 'welcome', 'login', or 'register'

  const handleLogin = async () => {
    try {
      const result = await getUserName(username);

      if (
        result &&
        result.username === username &&
        password === result.password
      ) {
        AsyncStorage.setItem("user", username);
        console.log("Login successful");
        onLoginSuccess();
        // navigateToHome();
      } else {
        // TODO: better error handling
        alert("Username or password is incorrect");
      }
    } catch (error) {
      console.error("Error fetching username:", error);
    }
  };

  const handleRegister = async () => {
    const newUser = {
      username: username,
      password: password,
      phone: phoneNumber,
    };

    try {
      const result = await getUserName(username);

      if (result.username === username) {
        alert("Username already exists");
      } else {
        await AddUser(newUser); // Assuming AddUser returns a promise
        console.log("User created");
        setIsRegistering(!isRegistering);
        setUsername("");
        setPassword("");
        setPhoneNumber("");

        AsyncStorage.setItem("user", username);
        console.log("Login successful");
        onLoginSuccess();
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const navigateToLogin = () => setPage("login");
  const navigateToRegister = () => setPage("register");
  const navigateToWelcome = () => setPage("welcome");

  //     return (
  //         // TODO: add login embellishments (logo, when passwords dont match, taken user name exc...)
  //         <TouchableWithoutFeedback onPress={() => Keyboard.dismiss()}>
  //         <View style={styles.container}>
  //             {isRegistering ? (

  //                 // REGISTER
  // <View style={styles.container}>
  //     <Text style={styles.header}>Register</Text>
  //     <TextInput
  //         style={styles.input}
  //         placeholder="Username"
  //         autoCapitalize="none"
  //         onChangeText={(text) => setUsername(text)}
  //         value={username}
  //     />
  //     <TextInput
  //         style={styles.input}
  //         placeholder="Phone number"
  //         keyboardType="numeric"
  //         onChangeText={(text) => setPhoneNumber(text)}
  //         value={phoneNumber}
  //     />
  //     <TextInput
  //         style={styles.input}
  //         placeholder="Password"
  //         secureTextEntry={true}
  //         autoCapitalize="none"
  //         onChangeText={(text) => setPassword(text)}
  //         value={password}
  //     />
  //     <TextInput
  //         style={styles.input}
  //         placeholder="Password"
  //         secureTextEntry={true}
  //         autoCapitalize="none"
  //         onChangeText={(text) => setPasswordVerification(text)}
  //         value={passwordVerification}
  //     />

  //     <TouchableOpacity
  //         style={styles.button}
  //         onPress={isRegistering ? handleRegister : handleLogin}
  //     >
  //         <Text style={styles.buttonText}>
  //             Register
  //         </Text>
  //     </TouchableOpacity>
  //     <TouchableOpacity
  //         onPress={() => setIsRegistering(!isRegistering)}
  //     >
  //         <Text style={styles.toggleText}>
  //             Already have an account? Login
  //         </Text>
  //     </TouchableOpacity>
  // </View>

  //             ) : (

  //                 // LOGIN
  //                 <View style={styles.container}>
  //                     <View style={styles.circle}></View>
  //                     <Text style={styles.header2}>Hello, Welcome!</Text>
  //                     <Text style={styles.header}>Login</Text>
  //                     <TextInput
  //                         style={styles.input}
  //                         placeholder="Username"
  //                         autoCapitalize="none"
  //                         onChangeText={(text) => setUsername(text)}
  //                         value={username}
  //                     />
  //                     <TextInput
  //                         style={styles.input}
  //                         placeholder="Password"
  //                         secureTextEntry={true}
  //                         autoCapitalize="none"
  //                         onChangeText={(text) => setPassword(text)}
  //                         value={password}
  //                     />
  //                     <TouchableOpacity
  //                         style={styles.button}
  //                         onPress={handleLogin}
  //                     >
  //                         <Text style={styles.buttonText}>
  //                             Login
  //                         </Text>
  //                     </TouchableOpacity>
  //                     <TouchableOpacity
  //                         onPress={() => setIsRegistering(!isRegistering)}
  //                     >
  //                         <Text style={styles.toggleText}>
  //                             Don't have an account? Register
  //                         </Text>
  //                     </TouchableOpacity>
  //                 </View>
  //             )}
  //         </View>
  //         </TouchableWithoutFeedback>
  //     );
  // }

  return (
    <TouchableWithoutFeedback onPress={() => Keyboard.dismiss()}>
      <View style={styles.container}>
        {page === "welcome" && (
          <View style={styles.container}>
            <Image source={logo} style={styles.logo} />
            <View style={styles.circle}></View>
            <Text style={styles.headerwelcome}>Welcome to Trippa!</Text>
            <View style={styles.welcomeButtonContainer}>
              <TouchableOpacity style={styles.button} onPress={navigateToLogin}>
                <Text style={styles.buttonText}>Login</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={styles.button}
                onPress={navigateToRegister}
              >
                <Text style={styles.buttonText}>Register</Text>
              </TouchableOpacity>
            </View>
          </View>
        )}

        {page === "login" && (
          // LOGIN COMPONENT
          // ... (Login component code here)
          <KeyboardAvoidingView
            behavior={Platform.OS === "ios" ? "padding" : null}
            style={styles.container}
          >
            <View style={styles.container}>
              <Image source={login} style={styles.login} />
              <View style={styles.circle}></View>
              <Text style={styles.header3}>Welcome Back!</Text>
              <Text style={styles.headerlogin}>Login</Text>

              {/* <View> */}
              <TextInput
                style={styles.input}
                placeholder="Username"
                autoCapitalize="none"
                onChangeText={(text) => setUsername(text)}
                value={username}
              />
              <TextInput
                style={styles.input}
                placeholder="Password"
                secureTextEntry={true}
                autoCapitalize="none"
                onChangeText={(text) => setPassword(text)}
                value={password}
              />
              {/* <View> */}

              <TouchableOpacity
                style={[styles.button, styles.loginButton]}
                onPress={handleLogin}
              >
                <Text style={styles.buttonText}>Login</Text>
              </TouchableOpacity>
              <TouchableOpacity onPress={() => setPage("register")}>
                <Text style={styles.toggleText}>
                  Don't have an account? Register
                </Text>
              </TouchableOpacity>
            </View>
          </KeyboardAvoidingView>
        )}

        {page === "register" && (
          // REGISTER COMPONENT
          // ... (Register component code here)
          <KeyboardAvoidingView
            behavior={Platform.OS === "ios" ? "padding" : null}
            style={styles.container}
          >
            <View style={styles.container}>
              <Image source={login} style={styles.login} />
              <View style={styles.circle}></View>
              <Text style={styles.header3}>Ready to Roll?</Text>
              <Text style={styles.headerlogin}>Register</Text>
              <TextInput
                style={styles.inputregister}
                placeholder="Username"
                autoCapitalize="none"
                onChangeText={(text) => setUsername(text)}
                value={username}
              />
              <TextInput
                style={styles.inputregister}
                placeholder="Phone number"
                keyboardType="numeric"
                onChangeText={(text) => setPhoneNumber(text)}
                value={phoneNumber}
              />
              <TextInput
                style={styles.inputregister}
                placeholder="Password"
                secureTextEntry={true}
                autoCapitalize="none"
                onChangeText={(text) => setPassword(text)}
                value={password}
              />
              <TextInput
                style={styles.inputregister}
                placeholder="Password"
                secureTextEntry={true}
                autoCapitalize="none"
                onChangeText={(text) => setPasswordVerification(text)}
                value={passwordVerification}
              />

              <TouchableOpacity style={styles.button2} onPress={handleRegister}>
                <Text style={styles.buttonText}>Register</Text>
              </TouchableOpacity>
              <TouchableOpacity onPress={() => setPage("login")}>
                <Text style={styles.toggleText2}>
                  Already have an account? Login
                </Text>
              </TouchableOpacity>
            </View>
          </KeyboardAvoidingView>
        )}
      </View>
    </TouchableWithoutFeedback>
  );
}

const highlight_color = "#007c3e";

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100%",
  },
  header: {
    fontSize: 24,
    marginBottom: 20,
    position: "absolute",
    top: "40%",
    zIndex: 1,
  },
  headerwelcome: {
    fontSize: 36,
    marginBottom: 20,
    position: "absolute",
    top: "55%",
    zIndex: 1,
    color: "white",
  },
  headerlogin: {
    fontSize: 36,
    marginBottom: 20,
    position: "absolute",
    top: "50%",
    zIndex: 1,
    color: "white",
  },
  header3: {
    fontSize: 28, // Adjust this value as per your preference for the size
    marginBottom: 20,
    position: "absolute",
    top: "14%", // You can adjust this to move it a bit down if needed
    left: "-4%", // You can adjust this to move it a bit right if needed
    color: "black",
  },
  input: {
    width: 300,
    height: 40,
    color: "white",
    borderColor: "white",
    borderWidth: 1,
    paddingHorizontal: 10,
    marginBottom: 5,
    borderRadius: 5,
    top: "25%",
  },
  inputregister: {
    width: 300,
    height: 40,
    color: "white",
    borderColor: "white",
    borderWidth: 1,
    paddingHorizontal: 10,
    marginBottom: 5,
    borderRadius: 5,
    top: "28%",
  },
  inputContainer: {
    top: "24%",
  },
  button: {
    backgroundColor: "white",
    width: 200,
    height: 50,
    justifyContent: "center",
    alignItems: "center",
    borderRadius: 5,
    marginBottom: 30,
  },
  button2: {
    backgroundColor: "white",
    width: 200,
    height: 50,
    justifyContent: "center",
    alignItems: "center",
    borderRadius: 5,
    marginBottom: 30,
    top: "29%",
  },
  loginButton: {
    top: "30%",
  },

  buttonText: {
    color: "black",
    fontSize: 16,
  },
  welcomeButtonContainer: {
    justifyContent: "center",
    alignItems: "center",
    marginTop: "110%",
  },
  toggleText: {
    marginTop: 10,
    color: "white",
    top: "870%",
  },
  toggleText2: {
    marginTop: 10,
    color: "white",
    top: "700%",
  },
  circle: {
    width: 1500,
    height: 1500,
    borderRadius: 1500 / 2,
    borderColor: "#1f5533",
    borderWidth: 4,
    top: "45%",
    backgroundColor: highlight_color,
    position: "absolute",
    bottom: 0,
  },
  logo: {
    width: "40%",
    height: "40%",
    position: "absolute",
    top: 20,
    resizeMode: "contain",
  },
  login: {
    width: "100%",
    height: "50%",
    position: "absolute",
    top: "3%",
    left: "20%",
    resizeMode: "contain",
  },
  temp: {
    color: "red",
  },
});

export default LoginRegister;
