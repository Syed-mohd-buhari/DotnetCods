import { Configuration } from "./configuration";
import * as portableFetch from "portable-fetch";
import { rootStore } from "../../Redux/Store/rootStore";
import {
  getAccessToken,
  removeAccessToken,
} from "../../Redux/Action/AuthenticationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { checkTokenExpirationMiddleware } from "../checkExpoek";

/**
 *
 * @export
 * @class BaseAPI
 */
export class BaseAPI {
  protected configuration: Configuration;

  constructor(
    configuration?: Configuration,
    protected basePath = BASE_PATH,
    protected fetch: FetchAPI = portableFetch
  ) {
    if (configuration) {
      this.configuration = configuration;
      this.basePath = configuration.basePath || this.basePath;
    }
    if (checkTokenExpirationMiddleware()) {
      let token = getAccessToken();
      this.configuration = { accessToken: token, apiKey: token };
    } else {
      let token = getAccessToken();
      this.configuration = { accessToken: token, apiKey: token };
    }
  }
}

export const PROD_BASE_PATH = window.location.origin.replace(/\/+$/, "");

//export const BASE_PATH = "http://localhost:5000";
export const BASE_PATH = process.env.REACT_APP_BASE_PATH;

/**
 *
 * @export
 * @interface FetchAPI
 */
export interface FetchAPI {
  (url: string, init?: any): Promise<Response>;
}

/**
 *
 * @export
 * @interface FetchArgs
 */
export interface FetchArgs {
  url: string;
  options: any;
}
/**
 *
 * @export
 * @class RequiredError
 * @extends {Error}
 */
export class RequiredError extends Error {
  name!: "RequiredError";
  constructor(public field: string, msg?: string) {
    super(msg);
  }
}

/**
 *
 * @export
 * @interface FilterValueDto
 */
export interface FilterValueDto {
  /**
   *
   * @type {string}
   * @memberof FilterValueDto
   */
  value: string;
  /**
   *
   * @type {string}
   * @memberof FilterValueDto
   */
  text: string;
}

export async function ApiCallWithErrorHandling<TDto>(arg: () => TDto) {
  //checkTokenExpirationMiddleware();
  try {
    let result = await arg();
    return result;
  } catch (error: any) {
    if (error?.status === 401) {
      console.log(401);
      //removeAccessToken();
      //document.location.reload();
      // rootStore.dispatch(
      //   setNotification({
      //     message: "You Are Not Authorized , Login again",
      //     notifyType: NotifyType.error,
      //   })
      // );
      removeAccessToken();

      // let test = await AquiredTokenSilent();
      // if (test != null) {
      //   setTimeout(() => {
      //     document.location.reload();
      //   }, 1000);
      // } else {
      //   try {
      //     let errorResult = (await error.json()) as ResultDto;
      //     rootStore.dispatch(
      //       setNotification({
      //         message: errorResult?.info ?? "",
      //         notifyType: errorResult?.warning
      //           ? NotifyType.error
      //           : NotifyType.success,
      //       })
      //     );
      //     rootStore.dispatch({ type: "AUTH_NOT_REGISTERED" });
      //   } catch (error) {
      //     rootStore.dispatch({ type: "AUTH_NOT_REGISTERED" });
      //   }
      // }
    }
    if (error.status === 400) {
      rootStore.dispatch(
        setNotification({
          message: "Bad Request",
          notifyType: NotifyType.error,
        })
      );
    }
  }
}

export function returnUniqueArray(arr: Array<any>) {
  const uniqueArray: any[] = [];
  for (let i = 0; i < arr.length; i++) {
    if (uniqueArray.indexOf(arr[i]) === -1) {
      uniqueArray.push(arr[i]);
    }
  }

  return uniqueArray;
}
