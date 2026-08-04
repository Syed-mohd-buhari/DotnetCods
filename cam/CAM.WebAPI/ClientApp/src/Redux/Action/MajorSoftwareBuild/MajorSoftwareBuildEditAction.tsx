import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorSoftwareBuildApi } from "../../../Business/MajorSoftwareBuildsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { EDIT_MAJOR_SOFTWARE_BUILD, GET_EDIT_MAJOR_SOFTWARE_BUILD, MajorSoftwareBuildDtoUpdate, MajorSoftwareBuildEdit } from "../../../Model/MajorSoftwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetMajorSoftwareBuildEditResource(id: number) {
	setLoader("ADD", "GetMajorSoftwareBuildEditResource");

	let api = new MajorSoftwareBuildApi();
	let createResource = await ApiCallWithErrorHandling<Promise<MajorSoftwareBuildDtoUpdate>>(() => api.majorSoftwareBuildGetUpdateResourceMajorSoftwareBuild(id));
	let rtn = { MajorSoftwareBuildDtoEdit: createResource } as MajorSoftwareBuildEdit;
	rootStore.dispatch({ type: GET_EDIT_MAJOR_SOFTWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "GetMajorSoftwareBuildEditResource");

	return rtn.MajorSoftwareBuildDtoEdit;
}

export async function EditMajorSoftwareBuild(data: MajorSoftwareBuildDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditMajorSoftwareBuild");
	let api = new MajorSoftwareBuildApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorSoftwareBuildPut(data, forced));
	let rtn = { ResultDtoEdit: result } as MajorSoftwareBuildEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_MAJOR_SOFTWARE_BUILD, payload: rtn });
	setLoader("REMOVE", "EditMajorSoftwareBuild");
	return rtn;
}
