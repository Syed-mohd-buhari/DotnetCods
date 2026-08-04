import React from "react";
import { Container, createRoot } from "react-dom/client";
import "./Css/bootstrap.min.css";
import "./index.css";
import "../src/Css/App.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import { Provider } from "react-redux";
import { rootStore } from "./Redux/Store/rootStore";
import { BrowserRouter } from "react-router-dom";
import "react-bootstrap-range-slider/dist/react-bootstrap-range-slider.css";
// --- MSAL migration ---
import { MsalProvider } from "@azure/msal-react";
import { EventType } from "@azure/msal-browser";
import { msalInstance, msalReady } from "./authProvider";
// ----------------------

const container = document.getElementById("root") as Container;

// Restore the active account on reload (e.g. an existing sessionStorage session).
const existingAccounts = msalInstance.getAllAccounts();
if (existingAccounts.length > 0) {
  msalInstance.setActiveAccount(existingAccounts[0]);
}

// Keep the active account current after login / token acquisition.
msalInstance.addEventCallback((event) => {
  const payload: any = event.payload;
  if (
    (event.eventType === EventType.LOGIN_SUCCESS ||
      event.eventType === EventType.ACQUIRE_TOKEN_SUCCESS) &&
    payload?.account
  ) {
    msalInstance.setActiveAccount(payload.account);
  }
});

// initialize() must resolve before rendering (msal-browser v3+ requirement),
// otherwise useMsal()/loginRedirect() throw an "uninitialized" error.
msalReady.then(() => {
  createRoot(container).render(
    <Provider store={rootStore}>
      <MsalProvider instance={msalInstance}>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </MsalProvider>
    </Provider>
  );
});

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
