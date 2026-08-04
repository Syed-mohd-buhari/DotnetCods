import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SystemTypeApi } from "../../../Business/SystemTypeBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { SystemTypeQueryObjectGrid } from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSystemTypeReport(queryFilter?: SystemTypeQueryObjectGrid) {
	setLoader("ADD", "GetSystemTypeReport");

	let res: ReturnFile | undefined;
	let api = new SystemTypeApi();
	try {
			res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
				api.systemTypeExportReport(
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
		setLoader("REMOVE", "GetSystemTypeReport");
		return result;
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetSystemTypeReport");
}
