import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ProjectPlanApi } from "../../../Business/ProjectPlanBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import setLoader from "../LoaderAction";

export async function GetProjectPlanReport(paId: string) {
  setLoader("ADD", "GetProjectPlanReport");
  let api = new ProjectPlanApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getProjectPlanReport(paId)
  );

  setLoader("REMOVE", "GetProjectPlanReport");
  return result;
}
