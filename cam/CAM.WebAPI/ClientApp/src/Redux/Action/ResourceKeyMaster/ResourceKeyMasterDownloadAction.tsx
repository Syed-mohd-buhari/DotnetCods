import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ResourceKeyMasterApi } from "../../../Business/ResourceKeyMasterBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { ResourceKeyMasterQueryObjectGrid } from "../../../Model/ResourceKeyMaster";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetResourceKeyMasterReport(queryFilter?: ResourceKeyMasterQueryObjectGrid) {
	setLoader("ADD", "GetResourceKeyMasterReport");

	let api = new ResourceKeyMasterApi();
	let res: ReturnFile | undefined;
	try {
			res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
				api.resourceKeyMasterExportReport(
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
		setLoader("REMOVE", "GetResourceKeyMasterReport");

		return result;
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetResourceKeyMasterReport");
}
