import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsIsApi } from "../../../Business/NetworkElementAsIsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_NETWORK_ELEMENT_AS_IS,
  GET_CREATE_NETWORK_ELEMENT_AS_IS,
  NetworkElementAsIsCreate,
  NetworkElementAsIsDtoCreate,
} from "../../../Model/NetworkElementAsIs";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNetworkElementAsIsCreateResource() {
  // const dispach = useDispatch();
  setLoader("ADD", "GetNetworkElementAsIsCreateResource");
  let api = new NetworkElementAsIsApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<NetworkElementAsIsDtoCreate>
  >(() => api.networkElementAsIsGetCreateResourceNetworkElementAsIs());
  let rtn = {
    ResultDtoCreate: null,
    NetworkElementAsIsDtoCreate: createResource,
  } as NetworkElementAsIsCreate;
  rootStore.dispatch({ type: GET_CREATE_NETWORK_ELEMENT_AS_IS, payload: rtn });
  setLoader("REMOVE", "GetNetworkElementAsIsCreateResource");
}

export async function CreatNetworkElementAsIs(
  data: NetworkElementAsIsDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatNetworkElementAsIs");
  let api = new NetworkElementAsIsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsIsCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    NetworkElementAsIsDtoCreate: null,
  } as NetworkElementAsIsCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_NETWORK_ELEMENT_AS_IS, payload: rtn });
  setLoader("REMOVE", "CreatNetworkElementAsIs");
  return rtn;
}

export async function GetNetworkElementAsIsImportStatus(file: File) {
  let api = new NetworkElementAsIsApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetNetworkElementAsIsImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.networkElementAsIsImportStatus(file)
    );
    setLoader("REMOVE", "GetNetworkElementAsIsImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetNetworkElementAsIsImportStatus");
}
