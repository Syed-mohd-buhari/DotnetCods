import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { MainOrganisationApi } from "../../../../Business/LookUp/MainOrganisationBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetMainOrganisationCreateResource() {
  setLoader("ADD", "GetMainOrganisationCreateResource");

  let api = new MainOrganisationApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.mainOrganisationGetCreateResourceMainOrganisation());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_MAIN_ORGANISATION", payload: rtn });
  setLoader("REMOVE", "GetMainOrganisationCreateResource");
}

export async function CreatMainOrganisation(data: TipologicaGridDto) {
  setLoader("ADD", "CreatMainOrganisation");
  let api = new MainOrganisationApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.mainOrganisationCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_MAIN_ORGANISATION", payload: rtn });
  setLoader("REMOVE", "CreatMainOrganisation");
  return rtn;
}
