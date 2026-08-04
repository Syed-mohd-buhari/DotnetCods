import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SoftwareComponentApi } from "../../../Business/SoftwareComponentBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { SoftwareComponentQueryObjectGrid } from "../../../Model/SoftwareComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSoftwareComponentReport(queryFilter?: SoftwareComponentQueryObjectGrid) {
	setLoader("ADD", "GetSoftwareComponentReport");

	let api = new SoftwareComponentApi();
	let res: ReturnFile | undefined;
	try {
			res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
				api.softwareComponentExportReport(
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
		setLoader("REMOVE", "GetSoftwareComponentReport");

		return result;
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetSoftwareComponentReport");
}


