import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubNetworkBoundaryApi } from "../../../../Business/LookUp/SubnetworkBoundryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import {
  LookUpCreateSubNetworkBoundary,
  SubNetworkBoundaryGridDto,
} from "../../../../Model/LookUp/SubnetworkBoundry";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSubNetworkBoundaryCreateResource() {
  setLoader("ADD", "GetSubNetworkBoundaryCreateResource");

  let api = new SubNetworkBoundaryApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.subNetworkBoundaryGetCreateResourceSubNetworkBoundary());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreateSubNetworkBoundary;
  rootStore.dispatch({ type: "GET_CREATE_SUBNETWORK_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "GetSubNetworkBoundaryCreateResource");
}

export async function CreatSubNetworkBoundary(data: SubNetworkBoundaryGridDto) {
  setLoader("ADD", "CreatSubNetworkBoundary");
  let api = new SubNetworkBoundaryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subNetworkBoundaryCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as LookUpCreateSubNetworkBoundary;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_SUBNETWORK_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "CreatSubNetworkBoundary");
  return rtn;
}

export async function GetServicesOfSubNetworkBoundaries(ids: Array<number>) {
  setLoader("ADD", "GetServicesOfSubNetworkBoundaries");

  let api = new SubNetworkBoundaryApi();

  let data = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.getServicesOfSubNetworkBoundaries(ids)
  );
  setLoader("REMOVE", "GetServicesOfSubNetworkBoundaries");
  return data;
}

export async function GetSNewSubnetworkBoundaryDescription(vodafoneId: number) {
  setLoader("ADD", "GetSNewSubnetworkBoundaryDescription");

  let api = new SubNetworkBoundaryApi();

  let data = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.subNetworkBoundaryGetSubnetworkDescription(vodafoneId)
  );
  setLoader("REMOVE", "GetSNewSubnetworkBoundaryDescription");
  return data;
}

export async function GetAllSWApplicationType() {
  setLoader("ADD", "GetAllSWApplicationType");

  let api = new SubNetworkBoundaryApi();

  let data = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.subNetworkBoundaryGetAllSWApplicationType()
  );
  setLoader("REMOVE", "GetAllSWApplicationType");
  return data;
}
