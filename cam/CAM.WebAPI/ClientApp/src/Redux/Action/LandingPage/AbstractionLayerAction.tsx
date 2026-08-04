import { AbstractionLayerInfoApi } from "../../../Business/AbstractionLayerBusiness";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetAllPaSWUpgrade(id: number) {
  setLoader("ADD", "GetAllPaSWUpgrade");

  let api = new AbstractionLayerInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.PaSWUpgradeGet(id)
  );
  setLoader("REMOVE", "GetAllPaSWUpgrade");

  return response?.data;
}

export async function GetEosAndEomMileStones(id: number) {
  setLoader("ADD", "GetEosAndEomMileStones");

  let api = new AbstractionLayerInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.EosAndEomMileStonesGet(id)
  );
  setLoader("REMOVE", "GetEosAndEomMileStones");

  return response;
}
export async function GetUserPrefrenceDetails(id: number) {
  setLoader("ADD", "GetUserPrefrenceDetails");

  let api = new AbstractionLayerInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.UserPrefrenceDetailsGet(id)
  );
  setLoader("REMOVE", "GetUserPrefrenceDetails");

  return response;
}

export async function DoingSectionPADetails(id: number) {
  setLoader("ADD", "DoingSectionPADetails");

  let api = new AbstractionLayerInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.DoingSectionPADetailsGet(id)
  );
  setLoader("REMOVE", "DoingSectionPADetails");

  return response?.data;
}
export async function DoingSectionForEomAndEos(id: number) {
  setLoader("ADD", "DoingSectionForEomAndEos");

  let api = new AbstractionLayerInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.DoingSectionForEomAndEosGet(id)
  );
  setLoader("REMOVE", "DoingSectionForEomAndEos");

  return response;
}
export async function AchievementsDetails(id: number) {
  setLoader("ADD", "AchievementsDetails");

  let api = new AbstractionLayerInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.AchievementsDetailsGet(id)
  );
  setLoader("REMOVE", "AchievementsDetails");

  return response;
}
export async function SignPostPaDetails(id: number) {
  setLoader("ADD", "SignPostPaDetails");

  let api = new AbstractionLayerInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.SignPostPaDetailsGet(id)
  );
  setLoader("REMOVE", "SignPostPaDetails");

  return response;
}
