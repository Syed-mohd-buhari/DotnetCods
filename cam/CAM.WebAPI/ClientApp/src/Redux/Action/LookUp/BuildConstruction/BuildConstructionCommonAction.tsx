import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BuildConstructionApi } from "../../../../Business/LookUp/BuildConstructionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import setLoader from "../../LoaderAction";

export async function GetRuleFromBuildCostruction(id: number) {
	setLoader("ADD", "GetRuleFromBuildCostruction");

	let api = new BuildConstructionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.buildConstructionGetFromBuildCostruction(id));
	setLoader("REMOVE", "GetRuleFromBuildCostruction");

	return result?.data as number;
}
