import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BundleUpgradeInitiativeApi } from "../../../Business/BundleUpgradeInitiativeBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { BundleUpgradeInitiativeQueryObjectGrid } from "../../../Model/BundleUpgradeIniziative";
import setLoader from "../LoaderAction";

export async function GetBundleUpgradeInitiativeReport(queryFilter?: BundleUpgradeInitiativeQueryObjectGrid) {
	let res: ReturnFile | undefined;
	let api = new BundleUpgradeInitiativeApi();

	setLoader("ADD", "GetBundleUpgradeInitiativeReport");

		res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
			api.bundleUpgradeInitiativeExportReport(
				queryFilter || {} 
			)
		);
	
	let fileNameBase = res?.FileName.split(";")[1] ?? "";
	var index = fileNameBase?.indexOf('"') + 1;
	var lastIndex = fileNameBase?.indexOf('"', index);
	let fileName = fileNameBase?.substring(index, lastIndex);
	let result = res?.File.then((x) => {
		setLoader("REMOVE", "GetBundleUpgradeInitiativeReport");

		return { file: x, fileName: fileName } as FileResult;
	});
	setLoader("REMOVE", "GetBundleUpgradeInitiativeReport");

	return result;
}
