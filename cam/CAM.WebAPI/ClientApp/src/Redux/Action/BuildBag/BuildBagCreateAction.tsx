import { BuildBagApi } from "../../../Business/BuildBagsBusiness";
import {
  CREATE_BUILD_BAG,
  GET_CREATE_BUILD_BAG,
  BuildBagCreate,
  BuildBagDtoCreate,
} from "../../../Model/BuildBag";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import setLoader from "../LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../Model/CommonModels";
// import { useDispatch } from 'react-redux'

export async function GetBuildBagCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetBuildBagCreateResource");

  let api = new BuildBagApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<BuildBagDtoCreate>
  >(() => api.buildBagGetCreateResourceBuildBag());
  let rtn = {
    ResultDtoCreate: null,
    BuildBagDtoCreate: createResource,
  } as BuildBagCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({ type: GET_CREATE_BUILD_BAG, payload: rtn });
  }
  setLoader("REMOVE", "GetBuildBagCreateResource");

  return rtn.BuildBagDtoCreate;
}

export async function CreatBuildBag(data: BuildBagDtoCreate, forced?: boolean) {
  setLoader("ADD", "CreatBuildBag");
  let api = new BuildBagApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.buildBagCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    BuildBagDtoCreate: null,
  } as BuildBagCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_BUILD_BAG, payload: rtn });
  setLoader("REMOVE", "CreatBuildBag");
  return rtn;
}
