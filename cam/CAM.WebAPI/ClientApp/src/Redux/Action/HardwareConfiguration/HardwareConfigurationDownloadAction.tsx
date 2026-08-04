import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { HardwareConfigurationApi } from "../../../Business/HardwareConfigurationBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { HardwareConfigurationQueryObjectGrid } from "../../../Model/HardwareConfiguration";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetHardwareConfigurationReport(queryFilter?: HardwareConfigurationQueryObjectGrid) {
	setLoader("ADD", "GetHardwareConfigurationReport");

	let api = new HardwareConfigurationApi();
	let res: ReturnFile | undefined;
	try {
			res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
				api.hardwareConfigurationExportReport(
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
		setLoader("REMOVE", "GetHardwareConfigurationReport");

		return result;
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetHardwareConfigurationReport");
}


