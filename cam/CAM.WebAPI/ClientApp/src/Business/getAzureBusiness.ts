import { ApiCallWithErrorHandling, BASE_PATH } from "./Common/CommonBusiness";
import setLoader from "../Redux/Action/LoaderAction";
import {
  getAccessToken,
  getRefreshToken,
} from "../Redux/Action/AuthenticationAction";
import { headerObj } from "./header";
import axios from "axios";

export interface USER_DATA {
  email: string;
  isAuthenicated: boolean;
}

export interface USER_DATA_RESPONSE {
  token: string;
}

export const getAzureData = (data: USER_DATA): any => {
  setLoader("ADD", "LOGIN");
  return axios.get(BASE_PATH + "/api/User/GenerateToken", {
    params: { email: data.email, isAuthenicated: data.isAuthenicated },
  });
};

export const refreshToken = async () => {
  const data = {
    token: getAccessToken(),
    refreshToken: getRefreshToken(),
  };

  return axios.post(BASE_PATH + "/api/User/RefreshToken", data, {
    headers: {
      ...headerObj,
    },
  });
};
