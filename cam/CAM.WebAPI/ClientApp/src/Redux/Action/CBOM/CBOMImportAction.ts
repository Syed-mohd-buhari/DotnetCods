import { setNotification } from "../NotificationAction"; // assuming this already exists
import { NotifyType } from "../../Reducer/NotificationReducer";
import setLoader from "../LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { rootStore } from "../../Store/rootStore";
import { CBOMApi } from "../../../Business/CBOMBusiness";

export async function ImportCBOMExcel(file: File) {
  let api = new CBOMApi();
  let res: boolean | undefined;
  setLoader("ADD", "CBOMImportStatus");

  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.CBOMImportStatus(file)
    );
    setLoader("REMOVE", "CBOMImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "CBOMImportStatus");
}
