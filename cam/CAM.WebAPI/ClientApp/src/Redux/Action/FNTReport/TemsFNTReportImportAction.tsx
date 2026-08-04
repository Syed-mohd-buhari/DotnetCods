import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { FNTReportApi } from "../../../Business/FNTReportBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTemsFNTReportImportStatus(
  file: File,
  mode: Array<number>,
  verticalId: Array<number>
) {
  let api = new FNTReportApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetTemsFNTReportImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.TemsFNTReportImport(file, mode, verticalId)
    );
    setLoader("REMOVE", "GetTemsFNTReportImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetTemsFNTReportImportStatus");
}

export async function GetFNTRefresh(body?: any) {
  let api = new FNTReportApi();
  let res: any;
  setLoader("ADD", "GetFNTRefresh");
  try {
    res = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.FNTRefresh(body)
    );
    setLoader("REMOVE", "GetFNTRefresh");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetFNTRefresh");
}

export async function GetLatestRefreshStatus(filterId: number) {
  let api = new FNTReportApi();
  let res: any;
  setLoader("ADD", "GetLatestRefreshStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.TSRRefreshStatus(filterId)
    );
    setLoader("REMOVE", "GetLatestRefreshStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetLatestRefreshStatus");
}
