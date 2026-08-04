import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorSoftwareBuildApi } from "../../../Business/MajorSoftwareBuildsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_MAJOR_SOFTWARE_BUILD, RESTORE_MAJOR_SOFTWARE_BUILD } from "../../../Model/MajorSoftwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function deleteMajorSoftwareBuild(id: number) {
	setLoader("ADD", "deleteMajorSoftwareBuild");
	let api = new MajorSoftwareBuildApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorSoftwareBuildDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_MAJOR_SOFTWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "deleteMajorSoftwareBuild");
	return rtn;
}
export async function DeleteDeepMajorSoftwareBuild(id: number) {
	let api = new MajorSoftwareBuildApi();
	setLoader("ADD", "DeleteDeepMajorSoftwareBuild");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorSoftwareBuildDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_MAJOR_SOFTWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "DeleteDeepMajorSoftwareBuild");
	return rtn;
}

export async function RestoreMajorSoftwareBuild(id: number) {
	setLoader("ADD", "RestoreMajorSoftwareBuild");
	let api = new MajorSoftwareBuildApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorSoftwareBuildRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_MAJOR_SOFTWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "RestoreMajorSoftwareBuild");
	return rtn;
}

export async function GetRelatedRecordsMajorSoftwareBuild(id: number) {
	let api = new MajorSoftwareBuildApi();
	setLoader("ADD", "GetRelatedRecordsMajorSoftwareBuild");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorSoftwareBuildGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsMajorSoftwareBuild");
	return rtn;
}
