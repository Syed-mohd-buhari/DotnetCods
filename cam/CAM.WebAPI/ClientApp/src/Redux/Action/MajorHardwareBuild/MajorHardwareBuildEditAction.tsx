import { fail } from "assert";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorHardwareBuildApi } from "../../../Business/MajorHardwareBuildBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { EDIT_MAJOR_HARDWARE_BUILD, GET_EDIT_MAJOR_HARDWARE_BUILD, MajorHardwareBuildDtoUpdate, MajorHardwareBuildEdit } from "../../../Model/MajorHardwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetMajorHardwareBuildEditResource(id: number) {
	setLoader("ADD", "GetMajorHardwareBuildEditResource");

	let api = new MajorHardwareBuildApi();
	let createResource = await ApiCallWithErrorHandling<Promise<MajorHardwareBuildDtoUpdate>>(() => api.majorHardwareBuildGetUpdateResourceMajorHardwareBuild(id));
	let rtn = { MajorHardwareBuildDtoEdit: createResource } as MajorHardwareBuildEdit;
	rootStore.dispatch({ type: GET_EDIT_MAJOR_HARDWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "GetMajorHardwareBuildEditResource");

	return rtn.MajorHardwareBuildDtoEdit;
}

export async function EditMajorHardwareBuild(data: MajorHardwareBuildDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditMajorHardwareBuild");
	let api = new MajorHardwareBuildApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorHardwareBuildPut(data, forced));
	let rtn = { ResultDtoEdit: result } as MajorHardwareBuildEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_MAJOR_HARDWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "EditMajorHardwareBuild");
	return rtn;
}
