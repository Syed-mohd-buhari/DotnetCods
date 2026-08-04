import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ServicePlanApi } from "../../../Business/ServicePlanBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { ServicePlanQueryObjectGrid } from "../../../Model/ServicePlan";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetServicePlanReport(
  queryFilter?: ServicePlanQueryObjectGrid
) {
  setLoader("ADD", "GetServicePlanReport");

  let res: ReturnFile | undefined;
  let api = new ServicePlanApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.ServicePlanExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetServicePlanReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetServicePlanReport");
}
