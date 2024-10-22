import { CommonActions } from '@react-navigation/native';
import React from "react";


export const navigationRef = React.createRef();

export function navigate(name, params) {
  navigationRef.current?.navigate(name, params);
}

export function goBack() {
  navigationRef.current?.dispatch(CommonActions.goBack());
}
