import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMAtGlanceGraphApi } from "../../../../Business/LcmAtGlanceBusiness";
import setLoader from "../../LoaderAction";
import { ResultDto, LCMAtGlanceGraph } from "../../../../Model/CommonModels";

export async function GetLCMAtGlanceGraphApiResource(isPageLoad?: boolean) {
  setLoader("ADD", "GetLCMAtGlanceGraphApiResource");

  let api = new LCMAtGlanceGraphApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.LCMAtGlanceGraphGetResource(isPageLoad)
  );
  let rtn = {
    ResultDtoCreate: result,
  };
  setLoader("REMOVE", "GetLCMAtGlanceGraphApiResource");
  return rtn;
}

export async function GetLCMAtGlanceGraph(data: LCMAtGlanceGraph) {
  setLoader("ADD", "GetLCMAtGlanceGraph");
  let api = new LCMAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.LCMAtGlanceGraphGet(data)
  );
  let rtn = { ResultDtoCreate: result };

  setLoader("REMOVE", "GetLCMAtGlanceGraph");
  return rtn;
}

export async function GetOpcoWisePercentage(
  isPageLoad: boolean,
  data: LCMAtGlanceGraph,
  isEOS: boolean
) {
  let api = new LCMAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetOpcoWisePercentage(isPageLoad, data, isEOS)
  );
  return result;
}

export async function GetProductWisePercentage(
  isPageLoad: boolean,
  data: LCMAtGlanceGraph,
  isEOS: boolean
) {
  let api = new LCMAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetProductWisePercentage(isPageLoad, data, isEOS)
  );
  return result;
}

export async function GetSubnetworkWisePercentage(
  isPageLoad: boolean,
  data: LCMAtGlanceGraph,
  isEOS: boolean
) {
  let api = new LCMAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetSupportedServiceWisePercentage(isPageLoad, data, isEOS)
  );
  return result;
}

export async function GetOverAllPercentageBasedOnFilters(
  isPageLoad: boolean,
  data: LCMAtGlanceGraph,
  isEOS: boolean
) {
  let api = new LCMAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetOverAllPercentageBasedOnFilters(isPageLoad, data, isEOS)
  );
  return result;
}

export async function GetLCMAtGlancePAGraph(
  data: LCMAtGlanceGraph,
  isEOS: boolean
) {
  let api = new LCMAtGlanceGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.LCMAtGlancePAGraph(data, isEOS)
  );
  return result;
}

export async function GetLCMAtGlanceGraphOverAll() {
  setLoader("ADD", "GetLCMAtGlanceGraphOverAll");

  let api = new LCMAtGlanceGraphApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.LCMAtGlanceGraphOverAll()
  );
  let rtn = {
    ResultDtoCreate: result,
  };
  setLoader("REMOVE", "GetLCMAtGlanceGraphOverAll");
  return rtn;
}
