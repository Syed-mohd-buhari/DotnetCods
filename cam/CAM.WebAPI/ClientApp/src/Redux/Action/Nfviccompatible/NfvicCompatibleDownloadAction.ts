import { NFVICCompatibleApi } from "../../../Business/NFVICompatibilityBusiness";
import { NFVICompatibilityReportRequest } from "../../../Model/NfvicCompatible";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";

export async function GetNFVICompatibleReport(
  vmVarId: number,
  queryFilter: NFVICompatibilityReportRequest
): Promise<FileResult | undefined> {
  setLoader("ADD", "GetNFVICompatibleReport");
  const api = new NFVICCompatibleApi();

  try {
    const res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.NFVICompatibleExportReport(vmVarId, queryFilter)
    );

    if (!res) throw new Error("No file returned");

    const fileNameHeader = res.FileName?.split(";")[1] ?? "";
    const start = fileNameHeader.indexOf('"') + 1;
    const end = fileNameHeader.indexOf('"', start);
    const fileName = fileNameHeader.substring(start, end);

    const file = await res.File;
    return { file, fileName };
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Failed to download NFVI Report",
        notifyType: NotifyType.error,
      })
    );
  } finally {
    setLoader("REMOVE", "GetNFVICompatibleReport");
  }
}
