import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SharedLookUpApi } from "../../../../Business/LookUp/SharedLookUpBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSharedLookUpEditResource(id: number) {
  setLoader("ADD", "GetSharedLookUpEditResource");

  let api = new SharedLookUpApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() =>
    api.sharedLookUpGetUpdateResourceSharedLookUp(sessionStorage.sharedName, id)
  );
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_SHARED_LOOKUP", payload: rtn });
  setLoader("REMOVE", "GetSharedLookUpEditResource");

  return rtn;
}

export async function EditSharedLookUp(data: TipologicaGridDto) {
  setLoader("ADD", "EditSharedLookUp");
  let api = new SharedLookUpApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.sharedLookUpPut(sessionStorage.sharedName, data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SHARED_LOOKUP", payload: rtn });
  setLoader("REMOVE", "EditSharedLookUp");
  return rtn;
}
