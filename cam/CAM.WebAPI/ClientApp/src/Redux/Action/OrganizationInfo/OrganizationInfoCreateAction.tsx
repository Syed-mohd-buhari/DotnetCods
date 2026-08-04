import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  OrganizationInfoApiFetchParamCreator,
  OrganizationInfoApi,
} from "../../../Business/OrganizationInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_CREATE_ORGANIZATION_INFO,
  CREATE_ORGANIZATION_INFO,
  OrganizationInfoCreate,
  OrganizationInfoDtoCreate,
} from "../../../Model/OrganizationInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetOrganizationInfoCreateResource() {
  setLoader("ADD", "GetOrganizationInfoCreateResource");

  let api = new OrganizationInfoApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<OrganizationInfoDtoCreate>
  >(() => api.organizationInfoGetCreateResourceOrganizationInfo());
  let rtn = {
    ResultDtoCreate: null,
    OrganizationInfoDtoCreate: createResource,
  } as OrganizationInfoCreate;
  rootStore.dispatch({ type: GET_CREATE_ORGANIZATION_INFO, payload: rtn });
  setLoader("REMOVE", "GetOrganizationInfoCreateResource");

  return rtn.OrganizationInfoDtoCreate;
}

export async function CreateResourceOrganizationInfoRefillData() {
  setLoader("ADD", "GetOrganizationInfoCreateResource");

  let api = new OrganizationInfoApi();
  let result = await ApiCallWithErrorHandling<
    Promise<OrganizationInfoDtoCreate>
  >(() => api.organizationInfoGetCreateResourceOrganizationInfo());
  setLoader("REMOVE", "GetOrganizationInfoCreateResource");

  return result;
}

export async function CreatOrganizationInfo(
  data: OrganizationInfoDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatOrganizationInfo");
  let api = new OrganizationInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.organizationInfoCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    OrganizationInfoDtoCreate: null,
  } as OrganizationInfoCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_ORGANIZATION_INFO, payload: rtn });
  setLoader("REMOVE", "CreatOrganizationInfo");
  return rtn;
}
