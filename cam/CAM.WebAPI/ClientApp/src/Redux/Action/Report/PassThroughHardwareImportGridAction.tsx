import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PassThroughHardwareReportApi } from "../../../Business/Report/PassThroughHardwareBusiness";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetPassThroughHardwareReportImportStatus(
  file: File,
  verticalId: Array<number>
) {
  let api = new PassThroughHardwareReportApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetPassThroughHardwareReportImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.PassThroughHardwareReportImport(file, undefined, verticalId)
    );
    setLoader("REMOVE", "GetPassThroughHardwareReportImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPassThroughHardwareReportImportStatus");
}
