import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { OrganizationInfoApi } from "../../../Business/OrganizationInfoBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { OrganizationInfoQueryObjectGrid } from "../../../Model/OrganizationInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetOrganizationInfoReport(
  queryFilter?: OrganizationInfoQueryObjectGrid
) {
  setLoader("ADD", "GetOrganizationInfoReport");

  let res: ReturnFile | undefined;
  let api = new OrganizationInfoApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.organizationInfoExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetOrganizationInfoReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetOrganizationInfoReport");
}
