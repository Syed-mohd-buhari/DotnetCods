import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NFVITransitionApi } from "../../../Business/NFVITransitionBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { NFVITransitionQueryObjectGrid } from "../../../Model/NFVITransition";
import setLoader from "../LoaderAction";

export async function GetNFVITransitionReport(queryFilter?: NFVITransitionQueryObjectGrid) {
	setLoader("ADD", "GetNFVITransitionReport");

	let res: ReturnFile | undefined;
	let api = new NFVITransitionApi();

		res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
			api.nFVITransitionExportReport(
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
	setLoader("REMOVE", "GetNFVITransitionReport");

	return result;
}
