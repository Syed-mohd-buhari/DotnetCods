import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorSoftwareBuildApi } from "../../../Business/MajorSoftwareBuildsBusiness";
import { DataRemediationDto, ResultDto, ResultDataRemediationDto, ResultDtoOfResultDataRemediationDto } from "../../../Model/CommonModels";
import { MajorSoftwareBuildToCloneDto, ResultDtoOfMajorSoftwareBuildToCloneDto, CloneMajorSoftwareBuildDto } from "../../../Model/MajorSoftwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function ApplyDataRimediationMajorSoftware(data: DataRemediationDto) {
	let api = new MajorSoftwareBuildApi();
	setLoader("ADD", "ApplyDataRimediationMajorSoftware");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfResultDataRemediationDto>>(() => api.majorSoftwareBuildApplyDataRemediation(data));
	if (result && !result.warning) {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "ApplyDataRimediationMajorSoftware");
		return result.data as ResultDataRemediationDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "ApplyDataRimediationMajorSoftware");
	}
}

export async function GetMajorSoftwareToClone(data: number) {
	let api = new MajorSoftwareBuildApi();
	setLoader("ADD", "GetMajorSoftwareToClone");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfMajorSoftwareBuildToCloneDto>>(() => api.majorSoftwareBuildGetMajorSoftwareBuildToClone(data));
	if (result && !result.warning) {
		// rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetMajorSoftwareToClone");
		return result as ResultDtoOfMajorSoftwareBuildToCloneDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetMajorSoftwareToClone");
	}
}

export async function GetSystemTypeForAddMajorSW(data: any) {
	let api = new MajorSoftwareBuildApi();
	setLoader("ADD", "GetSystemTypeForAddMajorSW");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfMajorSoftwareBuildToCloneDto>>(() => api.majorSoftwareBuildGetSystemTypeForAddMajorSW(data));
	if (result && !result.warning) {
		// rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetSystemTypeForAddMajorSW");
		return result as ResultDtoOfMajorSoftwareBuildToCloneDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetSystemTypeForAddMajorSW");
	}
}

export async function GetMajorSoftwareClonePreSubmit(data: number, flag?: boolean) {
	let api = new MajorSoftwareBuildApi();
	setLoader("ADD", "GetMajorSoftwareClonePreSubmit");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorSoftwareBuildGetInfoMajorSoftwareBuildToClone(data, flag));
	if (result && !result.warning) {
		setLoader("REMOVE", "GetMajorSoftwareClonePreSubmit");
		return result as ResultDto;
	} else {
		if (result?.info != "systemtype" && result?.info != "designcomponent") {
			rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		}
		setLoader("REMOVE", "GetMajorSoftwareClonePreSubmit");
		return result as ResultDto;
	}
}
export async function CloneMajorSoftware(data: CloneMajorSoftwareBuildDto) {
	let api = new MajorSoftwareBuildApi();
	setLoader("ADD", "CloneMajorSoftware");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.majorSoftwareBuildCloneMajorSoftwareBuild(data));
	if (result && !result.warning) {
		setLoader("REMOVE", "CloneMajorSoftware");
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		return result as ResultDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "CloneMajorSoftware");
	}
}
