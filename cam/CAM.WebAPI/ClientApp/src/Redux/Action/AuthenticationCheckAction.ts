import { AuthCheckApi } from "../../Business/AuthCheckBusiness";
import { rootStore } from "../Store/rootStore";
import { removeAccessToken } from "./AuthenticationAction";
import setLoader from "./LoaderAction";

export async function verifyPermesso() {
  setLoader("ADD", "verifyPermesso");
  const api = new AuthCheckApi();

  try {
    let response = await api.authCheckCheck();
    if (response.status === 401) {
      removeAccessToken();
      setLoader("REMOVE", "verifyPermesso");
    } else {
      let role = await response.json();

      if (role.role) {
        rootStore.dispatch({ type: "AUTH_REGISTERED", payload: true });
        rootStore.dispatch({ type: "AUTH_ROLE", payload: role.role });
        rootStore.dispatch({
          type: "AAD_LOGIN_SUCCESS",
          payload: { account: { name: role.username, userid: role.userid } },
        });
      }
      if (role.userPagePrefrenceDetails) {
        rootStore.dispatch({
          type: "AUTH_USER_PREFERENCE",
          payload: role.userPagePrefrenceDetails,
        });
      }
      if (role.pagesize > 0) {
        rootStore.dispatch({
          type: "UPDATE_PAGE_SIZE",
          payload: role.pagesize,
        });
      }
      if (role.mode) {
        rootStore.dispatch({
          type: "ADD_OPERATING_MODE",
          payload: role.mode,
        });
      }
      setLoader("REMOVE", "verifyPermesso");
    }
  } catch (error: any) {
    if (error?.status === 401) {
      removeAccessToken();
      setLoader("REMOVE", "verifyPermesso");
    }
  }
  setLoader("REMOVE", "verifyPermesso");
}
