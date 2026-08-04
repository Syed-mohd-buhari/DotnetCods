import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ExodusAtGlanceGraphApi } from "../../../../Business/ExodusAtGlanceBusiness";
import setLoader from "../../LoaderAction";
import { ResultDto, ExodusAtGlanceGraph } from "../../../../Model/CommonModels";

export async function GetExodusAtGlanceGraphApiResource(
  isPageLoad?: boolean,
  filterObj: any = {},
  isAssetLevel?: boolean
) {
  setLoader("ADD", "GetExodusAtGlanceGraphApiResource");

  let api = new ExodusAtGlanceGraphApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ExodusAtGlanceGraphGetResource(isPageLoad, filterObj, isAssetLevel)
  );
  let rtn = {
    ResultDtoCreate: result,
  };
  setLoader("REMOVE", "GetExodusAtGlanceGraphApiResource");
  return rtn;
}

export async function GetExodusAtGlanceGraph(
  data: ExodusAtGlanceGraph,
  isAssetLevel?: boolean
) {
  setLoader("ADD", "GetExodusAtGlanceGraph");
  let api = new ExodusAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ExodusAtGlanceGraphGet(data, isAssetLevel)
  );
  let rtn = { ResultDtoCreate: result };

  setLoader("REMOVE", "GetExodusAtGlanceGraph");
  return rtn;
}

export async function GetOpcoWisePercentage(
  isPageLoad: boolean,
  data: ExodusAtGlanceGraph,
  isEOS?: boolean,
  isAssetLevel?: boolean
) {
  let api = new ExodusAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetOpcoWisePercentage(isPageLoad, data, isEOS, isAssetLevel)
  );
  return result;
}

export async function GetExodusAtGlancePAGraph(
  data: ExodusAtGlanceGraph,
  isToggleOn?: boolean
) {
  let api = new ExodusAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ExodusAtGlancePAGraph(data, isToggleOn)
  );
  return result;
}
