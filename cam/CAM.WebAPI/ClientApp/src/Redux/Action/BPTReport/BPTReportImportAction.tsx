import { setNotification } from "../NotificationAction"; // assuming this already exists
import { NotifyType } from "../../Reducer/NotificationReducer";
import setLoader from "../LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { rootStore } from "../../Store/rootStore";
import { BPTReportApi } from "../../../Business/BPTReportBuisnes";

export async function ImportBPTReport(file: File) {
  let api = new BPTReportApi();
  let res: boolean | undefined;
  setLoader("ADD", "ImportBPTReportStatus");

  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.bptReportImport(file)
    );
    setLoader("REMOVE", "BPTReportImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "BPTReportImportStatus");
}

export async function GetLatestBptRefreshStatus() {
  let api = new BPTReportApi();
  let res: any;
  setLoader("ADD", "GetLatestBptRefreshStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.bptRefreshStatus()
    );
    rootStore.dispatch(
      setNotification({
        message: !res?.warning ? "BPT Refresh Data successfully" : "",
        notifyType: res?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetLatestBptRefreshStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetLatestBptRefreshStatus");
}
