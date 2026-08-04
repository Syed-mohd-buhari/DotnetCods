import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TestInfoApi } from "../../../Business/TestInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_TEST_INFO,
  GET_EDIT_TEST_INFO,
  TestInfoDtoUpdate,
  TestInfoEdit,
} from "../../../Model/TestInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTestInfoEditResource(id: number) {
  setLoader("ADD", "GetTestInfoEditResource");

  let api = new TestInfoApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TestInfoDtoUpdate>
  >(() => api.testInfoGetUpdateResourceTestInfo(id));
  let rtn = { TestInfoDtoEdit: createResource } as TestInfoEdit;
  rootStore.dispatch({ type: GET_EDIT_TEST_INFO, payload: rtn });
  setLoader("REMOVE", "GetTestInfoEditResource");

  return rtn;
}

export async function EditTestInfo(data: TestInfoDtoUpdate, forced?: boolean) {
  setLoader("ADD", "EditTestInfo");
  let api = new TestInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.testInfoEdit(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as TestInfoEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_TEST_INFO, payload: rtn });
  setLoader("REMOVE", "EditTestInfo");
  return rtn;
}
