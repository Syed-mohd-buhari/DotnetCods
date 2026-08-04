import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTSRReportImportStatus(
  file: File,
  mode: Array<number>,
  verticalId: Array<number>
) {
  let api = new TSRReportApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetTSRReportImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.TSRReportImport(file, mode, verticalId)
    );
    setLoader("REMOVE", "GetTSRReportImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetTSRReportImportStatus");
}

export async function GetTSRRefresh(body?: any) {
  let api = new TSRReportApi();
  let res: any;
  setLoader("ADD", "GetTSRRefresh");
  try {
    res = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.TSRRefresh(body)
    );
    setLoader("REMOVE", "GetTSRRefresh");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetTSRRefresh");
}

export async function GetLatestRefreshStatus(filterId: number) {
  let api = new TSRReportApi();
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
