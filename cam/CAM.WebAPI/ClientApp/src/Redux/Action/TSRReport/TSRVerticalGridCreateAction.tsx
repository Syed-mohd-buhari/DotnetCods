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
import {
  LookUpCreate,
  QueryResultDtoOfTSRReportDtoGrid,
  TSRReportVerticalQueryObjectGrid,
  TemsTSRReportVerticalCreate,
  TemsTSRReportVerticalGrid,
} from "../../../Model/TSRReport";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
// import { useDispatch } from 'react-redux'

export async function GetTSRReportVerticalCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetTSRReportVerticalCreateResource");
  let api = new TSRReportApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<QueryResultDtoOfTSRReportDtoGrid>
  >(() => api.TSRReportVerticalGetCreateResource());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_TSR_VERTICAL", payload: rtn });
  setLoader("REMOVE", "GetTSRReportVerticalCreateResource");
}

export async function CreatedVerticalTSRReport(
  data: TSRReportVerticalQueryObjectGrid
) {
  setLoader("ADD", "CreatedVerticalTSRReport");
  let api = new TSRReportApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TSRReportVerticalCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_TSR_VERTICAL", payload: rtn });
  setLoader("REMOVE", "CreatedVerticalTSRReport");
  return rtn;
}
