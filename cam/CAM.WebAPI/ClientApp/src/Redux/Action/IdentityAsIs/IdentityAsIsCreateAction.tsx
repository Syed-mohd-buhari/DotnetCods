import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { IdentityAsIsApi } from "../../../Business/IdentityAsIs";
import { ResultDto } from "../../../Model/CommonModels";
import { getResourceObject } from "../../../Model/Common";
import {
  CREATE_IDENTITYASIS,
  GET_CREATE_IDENTiTYASIS,
  IdentityAsIsCreate,
  IdentityAsIsDtoCreate,
} from "../../../Model/LookUp/Identities";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetIdentityAsIsCreateResource() {
  setLoader("ADD", "GetIdentityAsIsCreateResource");

  let api = new IdentityAsIsApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<IdentityAsIsDtoCreate>
  >(() => api.IdentityAsIsGetCreateResourceIdentityAsIs());
  console.log(createResource, "actions");
  let rtn = {
    ResultDtoCreate: null,
    IdentityAsIsDtoCreate: { ...createResource },
  } as IdentityAsIsCreate;
  rootStore.dispatch({ type: GET_CREATE_IDENTiTYASIS, payload: rtn });
  setLoader("REMOVE", "GetIdentityAsIsCreateResource");

  return rtn.IdentityAsIsDtoCreate;
}
export async function GetIdentityAsIsResourceKey(obj: getResourceObject) {
  setLoader("ADD", "GetIdentityAsIsResourceKey");
  let api = new IdentityAsIsApi();
  let getKey = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getResourceKeyForIdentityAsIs(obj)
  );
  setLoader("REMOVE", "GetIdentityAsIsResourceKey");
  return getKey;
}
export async function CreatIdentityAsIs(
  data: IdentityAsIsDtoCreate,
  forced?: boolean
) {
  let api = new IdentityAsIsApi();
  setLoader("ADD", "CreatIdentityAsIs");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IdentityAsIsCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    IdentityAsIsDtoCreate: null,
  } as IdentityAsIsCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: GET_CREATE_IDENTiTYASIS, payload: rtn });

  setLoader("REMOVE", "CreatIdentityAsIs");
  return rtn;
}
export async function GetIdentityAsIsImportStatus(file: File) {
  let api = new IdentityAsIsApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetIdentityAsIsImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.identityAsIsImportStatus(file)
    );
    console.log("Api Response:", res);
    setLoader("REMOVE", "GetIdentityAsIsImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetIdentityAsIsImportStatus");
}
