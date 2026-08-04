import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PassThroughReportApi } from "../../../Business/Report/PassThroughReportBusiness";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetPassThroughReportImportStatus(
  file: File,
  verticalId: Array<number>
) {
  let api = new PassThroughReportApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetPassThroughReportImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.PassThroughReportImport(file, undefined, verticalId)
    );
    setLoader("REMOVE", "GetPassThroughReportImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPassThroughReportImportStatus");
}
