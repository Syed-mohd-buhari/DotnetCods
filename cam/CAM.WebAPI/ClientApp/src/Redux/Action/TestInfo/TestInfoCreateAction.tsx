import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  TestInfoApiFetchParamCreator,
  TestInfoApi,
} from "../../../Business/TestInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_CREATE_TEST_INFO,
  CREATE_TEST_INFO,
  TestInfoCreate,
  TestInfoDtoCreate,
} from "../../../Model/TestInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetTestInfoCreateResource() {
  setLoader("ADD", "GetTestInfoCreateResource");

  let api = new TestInfoApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TestInfoDtoCreate>
  >(() => api.testInfoGetCreateResourceTestInfo());
  let rtn = {
    ResultDtoCreate: null,
    TestInfoDtoCreate: createResource,
  } as TestInfoCreate;
  rootStore.dispatch({ type: GET_CREATE_TEST_INFO, payload: rtn });
  setLoader("REMOVE", "GetTestInfoCreateResource");

  return rtn.TestInfoDtoCreate;
}

export async function CreatTestInfo(data: TestInfoDtoCreate, forced?: boolean) {
  setLoader("ADD", "CreatTestInfo");
  let api = new TestInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.testInfoCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    TestInfoDtoCreate: null,
  } as TestInfoCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_TEST_INFO, payload: rtn });
  setLoader("REMOVE", "CreatTestInfo");
  return rtn;
}
