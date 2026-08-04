import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BuildBagApi } from "../../../Business/BuildBagsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_BUILD_BAG,
  GET_EDIT_BUILD_BAG,
  BuildBagDtoUpdate,
  BuildBagEdit,
} from "../../../Model/BuildBag";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetBuildBagEditResource(id: number) {
  setLoader("ADD", "GetBuildBagEditResource");

  let api = new BuildBagApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<BuildBagDtoUpdate>
  >(() => api.buildBagGetUpdateResourceBuildBag(id));
  let rtn = { BuildBagDtoEdit: createResource } as BuildBagEdit;
  rootStore.dispatch({ type: GET_EDIT_BUILD_BAG, payload: rtn });
  setLoader("REMOVE", "GetBuildBagEditResource");

  return rtn.BuildBagDtoEdit;
}

export async function EditBuildBag(data: BuildBagDtoUpdate, forced?: boolean) {
  setLoader("ADD", "EditBuildBag");
  let api = new BuildBagApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.buildBagPut(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as BuildBagEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_BUILD_BAG, payload: rtn });
  setLoader("REMOVE", "EditBuildBag");
  return rtn;
}
