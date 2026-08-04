import { setNotification } from "../NotificationAction"; // assuming this already exists
import { NotifyType } from "../../Reducer/NotificationReducer";
import setLoader from "../LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { rootStore } from "../../Store/rootStore";
import { VBOMInfoApi } from "../../../Business/VBOMInfoBusiness";

export async function ImportVBOMInfoExcel(file: File) {
  let api = new VBOMInfoApi();
  let res: boolean | undefined;
  setLoader("ADD", "VBOMInfoImportStatus");

  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.VBOMInfoImportStatus(file)
    );
    console.log("Api Response:", res);
    setLoader("REMOVE", "VBOMInfoImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "VBOMInfoImportStatus");
}
