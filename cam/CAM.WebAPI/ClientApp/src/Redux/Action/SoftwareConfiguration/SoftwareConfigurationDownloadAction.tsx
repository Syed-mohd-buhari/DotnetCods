import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SoftwareConfigurationApi } from "../../../Business/SoftwareConfigurationBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { SoftwareConfigurationQueryObjectGrid } from "../../../Model/SoftwareConfiguration";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSoftwareConfigurationReport(queryFilter?: SoftwareConfigurationQueryObjectGrid) {
	setLoader("ADD", "GetSoftwareConfigurationReport");

	let api = new SoftwareConfigurationApi();
	let res: ReturnFile | undefined;
	try {
			res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
				api.softwareConfigurationExportReport(
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
		setLoader("REMOVE", "GetSoftwareConfigurationReport");

		return result;
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetSoftwareConfigurationReport");
}


