import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetMapInfoApi } from "../../../../Business/LookUp/AssetMapInfoBusiness";
import { FileResult, ReturnFile } from "../../../../Model/Common";
import {
  AssetMapInfoGrid,
  AssetMapInfoQueryObjectGrid,
  QueryResultDtoOfAssetMapInfoDtoGrid,
} from "../../../../Model/LookUp/AssetMapInfo";
import { NotifyType } from "../../../Reducer/NotificationReducer";

import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetAssetMapInfoExport(
  queryFilter?: AssetMapInfoQueryObjectGrid
) {
  setLoader("ADD", "GetAssetMapInfoExport");

  let res: ReturnFile | undefined;
  let api = new AssetMapInfoApi();

  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.AssetMapInfoExport(queryFilter ?? {}, "excel")
    );

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);

    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });

    setLoader("REMOVE", "GetAssetMapInfoExport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }

  setLoader("REMOVE", "GetAssetMapInfoExport");
}
