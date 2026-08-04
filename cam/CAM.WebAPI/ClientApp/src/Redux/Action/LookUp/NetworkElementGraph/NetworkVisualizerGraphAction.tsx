import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkVisualizerGraphApi } from "../../../../Business/NetworkVisualizerBusiness";
import setLoader from "../../LoaderAction";
import {
  ResultDto,
  NetworkVisualizerGraph,
} from "../../../../Model/CommonModels";

export async function GetNetworkVisualizerGraphApiResource() {
  setLoader("ADD", "GetNetworkVisualizerGraphApiResource");

  let api = new NetworkVisualizerGraphApi();
  let opco = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkVisualizerGraphGetResource()
  );
  let rtn = {
    ResultDtoCreate: opco,
  };
  setLoader("REMOVE", "GetNetworkVisualizerGraphApiResource");
  return rtn;
}

export async function GetNetworkVisualizerGraph(data: NetworkVisualizerGraph) {
  setLoader("ADD", "GetNetworkVisualizerGraph");
  let api = new NetworkVisualizerGraphApi();

  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementGraph(data)
  );
  let rtn = { ResultDtoCreate: result };

  setLoader("REMOVE", "GetNetworkVisualizerGraph");
  return rtn;
}
