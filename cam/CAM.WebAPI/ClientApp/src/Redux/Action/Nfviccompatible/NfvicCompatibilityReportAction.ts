// NfvicCompatibilityReportAction.ts

import { NFVICCompatibleApi } from "../../../Business/NFVICompatibilityBusiness";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { setNotification } from "../NotificationAction";
import { NotifyType } from "../../Reducer/NotificationReducer";
import setLoader from "../LoaderAction";
import { NFVICompatibilityReportRequest } from "../../../Model/NfvicCompatible";

export async function GetNFVICompatibilityReport(
  vmVarId: number,
  requestBody: NFVICompatibilityReportRequest
) {
  setLoader("ADD", "GetNFVICompatibilityReport");

  const api = new NFVICCompatibleApi();

  try {
    const result = await ApiCallWithErrorHandling(() =>
      api.nfvicCompatibleGetReport(vmVarId, requestBody)
    );

    setLoader("REMOVE", "GetNFVICompatibilityReport");

    // ✅ Return only the inner `data.data` (i.e., NFVICompatibilityReportData)
    return result?.data?.data;
  } catch (error) {
    setLoader("REMOVE", "GetNFVICompatibilityReport");

    setNotification({
      message: "Failed to fetch NFVI Compatibility Report",
      notifyType: NotifyType.error,
    });
  }
}
