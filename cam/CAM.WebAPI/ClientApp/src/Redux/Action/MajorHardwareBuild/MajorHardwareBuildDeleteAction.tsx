import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorHardwareBuildApi } from "../../../Business/MajorHardwareBuildBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_MAJOR_HARDWARE_BUILD, RESTORE_MAJOR_HARDWARE_BUILD } from "../../../Model/MajorHardwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function deleteMajorHardwareBuild(id: number) {
	let api = new MajorHardwareBuildApi();
	setLoader("ADD", "deleteMajorHardwareBuild");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorHardwareBuildDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_MAJOR_HARDWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "deleteMajorHardwareBuild");
	return rtn;
}

export async function RestoreMajorHardwareBuild(id: number) {
	setLoader("ADD", "RestoreMajorHardwareBuild");
	let api = new MajorHardwareBuildApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorHardwareBuildRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_MAJOR_HARDWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "RestoreMajorHardwareBuild");
	return rtn;
}

export async function DeleteDeepMajorHardwareBuild(id: number) {
	let api = new MajorHardwareBuildApi();
	setLoader("ADD", "DeleteDeepMajorHardwareBuild");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorHardwareBuildDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_MAJOR_HARDWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "DeleteDeepMajorHardwareBuild");
	return rtn;
}

export async function GetRelatedRecordsMajorHardwareBuild(id: number) {
	let api = new MajorHardwareBuildApi();
	setLoader("ADD", "GetRelatedRecordsMajorHardwareBuild");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorHardwareBuildGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsMajorHardwareBuild");
	return rtn;
}
