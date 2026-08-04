import { authProvider } from "../../authProvider";
import { rootStore } from "../Store/rootStore";
import { verifyPermesso } from "./AuthenticationCheckAction";
const ENV_TOKEN = process.env.REACT_APP_ENV_TOKEN;

const tokenKey = `${ENV_TOKEN}_token`;
const periodKey = `${ENV_TOKEN}_period`;
const refreshTokenKey = `${ENV_TOKEN}_refresh`;
const expiredDateKey = `${ENV_TOKEN}_expiredDate`;

export function setAccessToken(
  token: string,
  tokenPeriod?: number,
  refreshToken?: string
) {
  if (tokenPeriod) {
    const currentDate = new Date();
    const expiredDate = new Date(currentDate.getTime() + tokenPeriod! * 60000);
    localStorage.setItem(periodKey, JSON.stringify(tokenPeriod!));
    localStorage.setItem(expiredDateKey, JSON.stringify(expiredDate));
  }

  localStorage.setItem(tokenKey, "Bearer " + token);
  localStorage.setItem(refreshTokenKey, refreshToken!);

  // verifyPermesso();

  rootStore.dispatch({
    type: "AUTH_REGISTERED",
    payload: false,
  });
}

export const setErrorMessage = (errorMessage: string) => {
  localStorage.setItem("ERROR_MESSAGE", errorMessage!);
  // rootStore.dispatch({
  //   type: "AUTH_REGISTERED",
  // });
};

export function getAccessToken(): string {
  return localStorage.getItem(tokenKey) ?? "";
}

export function getRefreshToken() {
  return localStorage.getItem(refreshTokenKey) ?? "";
}

export async function AquiredTokenSilent() {
  const accessTokenRequest = {
    scopes: [
      "https://userscam.onmicrosoft.com/api/read",
      "https://userscam.onmicrosoft.com/api/write",
    ],
  };

  let accessToken = await authProvider.acquireTokenSilent(accessTokenRequest);
  let token = "Bearer " + accessToken.accessToken;
  await localStorage.setItem("token", token);
  rootStore.dispatch({ type: "ACCESS_TOKEN_ASSIGN", payload: token });
  return token;
}

export function removeAccessToken() {
  localStorage.clear();
  sessionStorage.clear();
  rootStore.dispatch({ type: "ACCESS_TOKEN_ASSIGN", payload: "" });
  rootStore.dispatch({ type: "AUTH_NOT_REGISTERED", payload: null });
}

export const returnExpTokenPeriod = () => {
  return +localStorage.getItem(periodKey)!;
};

//import { authProvider } from "../../authProvider";
//import { NotifyType } from "../Reducer/NotificationReducer";
//import { rootStore } from "../Store/rootStore";
//import { setNotification } from "./NotificationAction";

//export async function setAccessToken() {
//    try {
//        console.log("setAccessToken")
//        const accessTokenRequest = {
//            scopes: ["https://camusers.onmicrosoft.com/api/demo.read", "https://camusers.onmicrosoft.com/api/demo.write", "offline_access"]
//        }
//        var SavedToken = getAccessToken();
//        if (SavedToken == null || SavedToken == '') {
//            let accessToken = await authProvider.acquireTokenSilent(accessTokenRequest);
//            if (accessToken.accessToken != null || accessToken.accessToken !== undefined || accessToken.accessToken !== "" || accessToken.accessToken !== '') {
//                console.log("setToken");
//                let token = "Bearer " + accessToken.accessToken;
//                await localStorage.setItem('token', token);
//                rootStore.dispatch({ type: "ACCESS_TOKEN_ASSIGN", payload: token });
//                // setTimeout(() => {
//                //     window.location.reload();
//                // }, 500);
//            }
//        } else {
//            rootStore.dispatch({ type: "ACCESS_TOKEN_ASSIGN", payload: SavedToken });
//        }
//    } catch (error) {
//        console.log(error);
//        rootStore.dispatch(setNotification({ message: "To use the application it is necessary to accept Microsoft's third-party cookies", notifyType: NotifyType.noNotify }));
//        setAccessToken();
//    }
//}
//export function getAccessToken() {
//    return localStorage.getItem("token") ?? "";
//}

//export async function AquiredTokenSilent() {
//    const accessTokenRequest = {
//        scopes: ["https://camusers.onmicrosoft.com/api/demo.read", "https://camusers.onmicrosoft.com/api/demo.write", "offline_access"]
//    }

//    let accessToken = await authProvider.acquireTokenSilent(accessTokenRequest);
//    let token = "Bearer " + accessToken.accessToken;
//    await localStorage.setItem('token', token);
//    rootStore.dispatch({ type: "ACCESS_TOKEN_ASSIGN", payload: token });
//    return token;
//}

//export function removeAccessToken() {
//    localStorage.setItem("token", "");
//    rootStore.dispatch({ type: "ACCESS_TOKEN_ASSIGN", payload: '' });
//}
