import React from "react";
import { Route, Navigate } from "react-router-dom";

const PublicRoute: React.FC<{
  component: React.FC<{ onLogin: Function }>;
  path: string;
  exact: boolean;
  isAuth: boolean;
  onLogin: any;
}> = (props) => {
  const condition = props.isAuth;

  return condition ? (
    <Navigate to="/" />
  ) : (
    <Route
      path={props.path}
      element={React.createElement(props.component, { onLogin: props.onLogin })}
    />
  );
};
export default PublicRoute;
