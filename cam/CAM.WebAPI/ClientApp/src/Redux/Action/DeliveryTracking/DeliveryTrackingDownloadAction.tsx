import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DeliveryTrackingApi } from "../../../Business/DeliveryTrackingBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { DeliveryTrackingQueryObjectGrid } from "../../../Model/DeliveryTracking";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDeliveryTrackingReport(
  queryFilter?: DeliveryTrackingQueryObjectGrid
) {
  setLoader("ADD", "GetDeliveryTrackingReport");

  let api = new DeliveryTrackingApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.DeliveryTrackingExportReport(queryFilter ?? {})
    );

    console.log("res => ", res);
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetDeliveryTrackingReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetDeliveryTrackingReport");
  }
}
