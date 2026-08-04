import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngineeringApi } from "../../../Business/LcmEngineeringBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { LcmEngineringQueryObjectGrid } from "../../../Model/LcmEngineering";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetLcmEngineeringReport(
  queryFilter?: LcmEngineringQueryObjectGrid
) {
  let api = new LcmEngineeringApi();
  let res: ReturnFile | undefined;
  setLoader("ADD", "GetLcmEngineeringReport");
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.lcmEngineeringExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetLcmEngineeringReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetLcmEngineeringReport");
}

export async function GetLcmEngineeringArchiveReport(
  queryFilter?: LcmEngineringQueryObjectGrid
) {
  let api = new LcmEngineeringApi();
  let res: ReturnFile | undefined;
  setLoader("ADD", "GetLcmEngineeringArchiveReport");
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.lcmEngineeringArchivedExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetLcmEngineeringArchiveReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetLcmEngineeringArchiveReport");
}

export async function GetLcmEngineeringImportStatus(file: File) {
  let api = new LcmEngineeringApi();
  let res: boolean | undefined;
  setLoader("ADD", "GetLcmEngineeringImportStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
      api.lcmEngineeringImportStatus(file)
    );
    setLoader("REMOVE", "GetLcmEngineeringImportStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetLcmEngineeringImportStatus");
}
