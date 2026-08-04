import { setNotification } from "../NotificationAction"; // assuming this already exists
import { NotifyType } from "../../Reducer/NotificationReducer";
import setLoader from "../LoaderAction";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { rootStore } from "../../Store/rootStore";
import { ProjectPlanApi } from "../../../Business/ProjectPlanBusiness";

// export async function ImportProjectPlan(file: File) {
//   let api = new ProjectPlanApi();
//   let res: boolean | undefined;
//   setLoader("ADD", "ImportProjectPlanStatus");

//   try {
//     res = await ApiCallWithErrorHandling<Promise<boolean>>(() =>
//       api.bptReportImport(file)
//     );
//     setLoader("REMOVE", "ProjectPlanImportStatus");
//     return res;
//   } catch (error) {
//     rootStore.dispatch(
//       setNotification({
//         message: "Fail to fetch Import file status.",
//         notifyType: NotifyType.error,
//       })
//     );
//   }
//   setLoader("REMOVE", "ProjectPlanImportStatus");
// }

export async function GetProjectPlanRefreshStatus() {
  let api = new ProjectPlanApi();
  let res: any;
  setLoader("ADD", "GetProjectPlanRefreshStatus");
  try {
    res = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.projectPlanRefreshStatus()
    );
    rootStore.dispatch(
      setNotification({
        message: !res?.warning ? "Project Plan refresh data successfully" : "",
        notifyType: res?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetProjectPlanRefreshStatus");
    return res;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch Import file status.",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetProjectPlanRefreshStatus");
}
