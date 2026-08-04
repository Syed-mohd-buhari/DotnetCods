import {
  removeAccessToken,
  returnExpTokenPeriod,
  setAccessToken,
} from "../Redux/Action/AuthenticationAction";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { rootStore } from "../Redux/Store/rootStore";
import { refreshToken } from "./getAzureBusiness";

const ENV_TOKEN = process.env.REACT_APP_ENV_TOKEN;

const checkTokenExpirationMiddleware = (): boolean => {
  let today: any = new Date();
  let expired: any = new Date(
    JSON.parse(localStorage.getItem(`${ENV_TOKEN}_expiredDate`)!)
  );
  let diffMs = expired - today;
  // milliseconds between now & Christmas
  // var diffDays = Math.floor(diffMs / 86400000); // days
  // var diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
  let diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000);
  if (diffMins <= 2) {
    refreshToken()
      .then((res) => {
        if (res.data.success) {
          setAccessToken(
            res.data.token,
            returnExpTokenPeriod(),
            res.data.refreshToken.token
          );
          return true;
        } else {
          return false;
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }
  return false;
};

export { checkTokenExpirationMiddleware };
