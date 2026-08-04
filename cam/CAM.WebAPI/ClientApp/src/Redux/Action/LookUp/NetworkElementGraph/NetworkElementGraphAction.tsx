import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkElementGraphApi } from "../../../../Business/NetworkElementGraphBusiness";
import setLoader from "../../LoaderAction";
import { ResultDto, NetworkElementGraph } from "../../../../Model/CommonModels";
import { AssestOverviewByMarketQueryObjectGrid } from "../../../../Model/NetworkElementAsPlanned";
import { FileResult, ReturnFile } from "../../../../Model/Common";
import { rootStore } from "../../../Store/rootStore";
import { setNotification } from "../../NotificationAction";
import { NotifyType } from "../../../Reducer/NotificationReducer";

export async function GetNetworkElementGraphAllOpco() {
  setLoader("ADD", "GetNetworkElementGraphAllOpco");

  let api = new NetworkElementGraphApi();
  let opco = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementGraphGetAllOpcos()
  );
  let rtn = {
    ResultDtoCreate: opco,
  };
  setLoader("REMOVE", "GetNetworkElementGraphAllOpco");
  return rtn;
}

export async function GetNetworkElementGraph(
  data: AssestOverviewByMarketQueryObjectGrid
) {
  setLoader("ADD", "GetNetworkElementGraph");
  let api = new NetworkElementGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementGraph(data)
  );
  setLoader("REMOVE", "GetNetworkElementGraph");
  return result?.data;
}

export async function GetAssetOverviewByMarketReport(
  queryFilter?: AssestOverviewByMarketQueryObjectGrid
) {
  setLoader("ADD", "GetAssetOverviewByMarketReport");

  let res: ReturnFile | undefined;
  let api = new NetworkElementGraphApi();
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.AssetOverviewByMarketExport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetAssetOverviewByMarketReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetAssetOverviewByMarketReport");
}
