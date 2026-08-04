import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TeamsApi } from "../../../Business/TeamManagementBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { QueryDtoforTeam } from "../../../Model/TeamManagement";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function DownloadTeamManagementReport(query: QueryDtoforTeam) {
  setLoader("ADD", "DownloadTeamManagementReport");
  let api = new TeamsApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.teamsExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "DownloadTeamManagementReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadTeamManagementReport");
}
