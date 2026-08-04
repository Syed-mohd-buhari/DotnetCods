import { setNotification } from "../NotificationAction"; // assuming this already exists
import { NotifyType } from "../../Reducer/NotificationReducer";
import setLoader from "../LoaderAction";
import { NFVICCompatibleApi } from "../../../Business/NFVICompatibilityBusiness";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { rootStore } from "../../Store/rootStore";
import { NFVISwCompatibleApi } from "../../../Business/NFVISoftwareCompatibleBusiness";


export async function importNFVISoftwareCompatibleExcel(file: File) {
    let api = new NFVISwCompatibleApi();
  let res: boolean | undefined;
  setLoader("ADD", "NFVISwImportStatus");

  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.nfvicImportStatus(file)
    );
    console.log("Api Response:", res);
    setLoader("REMOVE", "NFVISwImportStatus");
    return res;
  }catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "NFVISwImportStatus");
};
