import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { OrganizationInfoApi } from "../../../Business/OrganizationInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_EDIT_ORGANIZATION_INFO,
  EDIT_ORGANIZATION_INFO,
  OrganizationInfoDtoUpdate,
  OrganizationInfoEdit,
} from "../../../Model/OrganizationInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetOrganizationInfoEditResource(id: number) {
  setLoader("ADD", "GetOrganizationInfoApiEditResource");

  let api = new OrganizationInfoApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<OrganizationInfoDtoUpdate>
  >(() => api.organizationInfoGetUpdateResourceOrganizationInfo(id));
  let rtn = { OrganizationInfoDtoEdit: createResource } as OrganizationInfoEdit;
  rootStore.dispatch({ type: GET_EDIT_ORGANIZATION_INFO, payload: rtn });
  setLoader("REMOVE", "GetOrganizationInfoApiEditResource");

  return rtn;
}

export async function EditResourceOrganizationInfoRefillData(id: number) {
  setLoader("ADD", "GetOrganizationInfoApiEditResource");

  let api = new OrganizationInfoApi();
  let rtn = await ApiCallWithErrorHandling<Promise<OrganizationInfoDtoUpdate>>(
    () => api.organizationInfoGetUpdateResourceOrganizationInfo(id)
  );
  setLoader("REMOVE", "GetOrganizationInfoApiEditResource");
  return rtn;
}

export async function EditOrganizationInfo(
  data: OrganizationInfoDtoUpdate,
  forced?: boolean
) {
  setLoader("ADD", "EditOrganizationInfo");
  let api = new OrganizationInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.organizationInfoEdit(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as OrganizationInfoEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_ORGANIZATION_INFO, payload: rtn });
  setLoader("REMOVE", "EditOrganizationInfo");
  return rtn;
}
