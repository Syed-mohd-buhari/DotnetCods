import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentFamilyApi } from "../../../Business/DesignComponentFamilyBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { DesignComponentFamilyQueryObjectGrid } from "../../../Model/DesignComponentFamily";
import setLoader from "../LoaderAction";

export async function GetDesignComponentFamilyReport(queryFilter?: DesignComponentFamilyQueryObjectGrid) {
	setLoader("ADD", "GetDesignComponentFamilyReport");
	let res: ReturnFile | undefined;
	let api = new DesignComponentFamilyApi();

		res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
			api.designComponentFamilyExportReport(
				queryFilter || {}
			)
		);

	let fileNameBase = res?.FileName.split(";")[1] ?? "";
	var index = fileNameBase?.indexOf('"') + 1;
	var lastIndex = fileNameBase?.indexOf('"', index);
	let fileName = fileNameBase?.substring(index, lastIndex);
	let result = res?.File.then((x) => {
		setLoader("REMOVE", "GetDesignComponentFamilyReport");
		return { file: x, fileName: fileName } as FileResult;
	});
	setLoader("REMOVE", "GetDesignComponentFamilyReport");
	return result;
}
