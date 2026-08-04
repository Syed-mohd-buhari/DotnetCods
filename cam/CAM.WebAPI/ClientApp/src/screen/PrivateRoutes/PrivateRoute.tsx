import React from "react";
import { Route, Navigate } from "react-router-dom";

const PrivateRoute: React.FC<{
  component: React.FC;
  path: string;
  exact: boolean;
  isAuth: boolean;
}> = (props) => {
  return props.isAuth ? (
    <Route path={props.path} element={<props.component/>} />
  ) : (
    <Navigate to="/login" />
  );
};
export default PrivateRoute;
