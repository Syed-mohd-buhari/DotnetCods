import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VNFTransitionApi } from "../../../Business/VNFTransitionBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { VNFTransitionQueryObjectGrid } from "../../../Model/VNFTransition";
import setLoader from "../LoaderAction";

export async function GetVNFTransitionReport(queryFilter?: VNFTransitionQueryObjectGrid) {
	setLoader("ADD", "GetVNFTransitionReport");

	let res: ReturnFile | undefined;
	let api = new VNFTransitionApi();

	res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
		api.vNFTransitionExportReport(
			queryFilter ?? {}
		)
	);

	let fileNameBase = res?.FileName.split(";")[1] ?? "";
	var index = fileNameBase?.indexOf('"') + 1;
	var lastIndex = fileNameBase?.indexOf('"', index);
	let fileName = fileNameBase?.substring(index, lastIndex);
	let result = res?.File.then((x) => {
		return { file: x, fileName: fileName } as FileResult;
	});
	setLoader("REMOVE", "GetVNFTransitionReport");

	return result;
}
