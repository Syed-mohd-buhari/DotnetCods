import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  BundleUpgradeInitiativeApiFetchParamCreator,
  BundleUpgradeInitiativeApi,
} from "../../../Business/BundleUpgradeInitiativeBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_BUNDLE_UPGRADE_INIZIATIVE,
  BundleUpgradeInitiativeCreate,
  BundleUpgradeInitiativeDtoCreate,
  GET_CREATE_BUNDLE_UPGRADE_INIZIATIVE,
  BundleUpgradeInitiativeDto,
} from "../../../Model/BundleUpgradeIniziative";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetBundleUpgradeInitiativeCreateResource() {
  setLoader("ADD", "GetBundleUpgradeInitiativeCreateResource");
  // const dispach = useDispatch();
  // ActionCenter<>
  // let test = await  ActionCenter<Promise<BundleUpgradeInitiativeDtoCreate>>(() => api.bundleUpgradeInitiativeGetCreateResourceBundleUpgradeInitiative());
  let api = new BundleUpgradeInitiativeApi();

  let createResource = await ApiCallWithErrorHandling<
    Promise<BundleUpgradeInitiativeDtoCreate>
  >(() =>
    api.bundleUpgradeInitiativeGetCreateResourceBundleUpgradeInitiative()
  );
  let rtn = {
    ResultDtoCreate: null,
    BundleUpgradeInitiativeDtoCreate: createResource,
  } as BundleUpgradeInitiativeCreate;
  rootStore.dispatch({
    type: GET_CREATE_BUNDLE_UPGRADE_INIZIATIVE,
    payload: rtn,
  });
  setLoader("REMOVE", "GetBundleUpgradeInitiativeCreateResource");
}

export async function CreatBundleUpgradeInitiative(
  data: BundleUpgradeInitiativeDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatBundleUpgradeInitiative");
  let api = new BundleUpgradeInitiativeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.bundleUpgradeInitiativeCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    BundleUpgradeInitiativeDtoCreate: null,
  } as BundleUpgradeInitiativeCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn });
  setLoader("REMOVE", "CreatBundleUpgradeInitiative");
  return rtn;
}
