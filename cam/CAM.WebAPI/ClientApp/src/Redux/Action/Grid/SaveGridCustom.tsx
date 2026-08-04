import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  DesignComponentApiFetchParamCreator,
  DesignComponentApi,
} from "../../../Business/DesignComponentBusiness";
import { GridApi } from "../../../Business/SaveGrid";
import { ResultDto, SaveGrid } from "../../../Model/CommonModels";
import {
  CREATE_DESIGN_COMPONENT,
  DesignComponentCreate,
  DesignComponentDtoCreate,
  GET_CREATE_DESIGN_COMPONENT,
} from "../../../Model/DesignComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function SaveCustomGridRender(data: SaveGrid) {
  setLoader("REMOVE", "SaveCustomGridRender");
  let api = new GridApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.gridSave(data)
  );
  let rtn = { ResultDtoCreate: result };
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  setLoader("REMOVE", "SaveCustomGridRender");
  return rtn;
}
export async function DeleteCustomGridRender(data: string) {
  setLoader("REMOVE", "DeleteCustomGridRender");
  let api = new GridApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.gridDelete(data)
  );
  let rtn = { ResultDtoCreate: result };
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  setLoader("REMOVE", "DeleteCustomGridRender");
  return rtn;
}
