import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PassThroughSoftwareReportApi } from "../../../Business/Report/PassThroughSoftwareBusiness";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetPassThroughSoftwareReportImportStatus(
  file: File,
  verticalId: Array<number>
) {
  let api = new PassThroughSoftwareReportApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetPassThroughSoftwareReportImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.PassThroughSoftwareReportImport(file, undefined, verticalId)
    );
    setLoader("REMOVE", "GetPassThroughSoftwareReportImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPassThroughSoftwareReportImportStatus");
}
