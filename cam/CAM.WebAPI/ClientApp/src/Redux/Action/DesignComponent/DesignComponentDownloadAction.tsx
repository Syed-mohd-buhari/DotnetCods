import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentApi } from "../../../Business/DesignComponentBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { DesignComponentQueryObjectGrid } from "../../../Model/DesignComponent";
import setLoader from "../LoaderAction";

export async function GetDesignComponentReport(queryFilter?: DesignComponentQueryObjectGrid) {
	setLoader("ADD", "GetDesignComponentReport");
	let res: ReturnFile | undefined;
	let api = new DesignComponentApi();

		res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
			api.designComponentExportReport(
				queryFilter || {}
			)
		);

	let fileNameBase = res?.FileName.split(";")[1] ?? "";
	var index = fileNameBase?.indexOf('"') + 1;
	var lastIndex = fileNameBase?.indexOf('"', index);
	let fileName = fileNameBase?.substring(index, lastIndex);
	let result = res?.File.then((x) => {
		setLoader("REMOVE", "GetDesignComponentReport");
		return { file: x, fileName: fileName } as FileResult;
	});
	setLoader("REMOVE", "GetDesignComponentReport");
	return result;
}
