import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubNetworkBoundaryApi } from "../../../../Business/LookUp/SubnetworkBoundryBusiness";
import { FileResult, ReturnFile } from "../../../../Model/Common";
import { SubNetworkBoundaryQueryDto } from "../../../../Model/LookUp/SubnetworkBoundry";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSubnetworkBoundaryReport(
  queryFilter?: SubNetworkBoundaryQueryDto
) {
  setLoader("ADD", "GetSubnetworkBoundaryReport");

  let api = new SubNetworkBoundaryApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.subnetworkBoundaryExportReport(queryFilter ?? {})
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetSubnetworkBoundaryReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    setLoader("REMOVE", "GetSubnetworkBoundaryReport");
  }
}
