import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { IdentityApi } from "../../../Business/IdentityBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { IdentityQueryObjectGrid } from "../../../Model/Identity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetIdentityReport(queryFilter?: IdentityQueryObjectGrid) {
	setLoader("ADD", "GetIdentityReport");

	let api = new IdentityApi();
	let res: ReturnFile | undefined;
	try {
			res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
				api.identityExportReport(
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
		setLoader("REMOVE", "GetIdentityReport");

		return result;
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetIdentityReport");
}


