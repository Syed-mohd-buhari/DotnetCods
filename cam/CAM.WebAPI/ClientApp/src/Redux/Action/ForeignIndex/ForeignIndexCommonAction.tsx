import { ProductLifecycleConstraintsApi } from "../../../Business/ProductLifecycleConstraintBusiness";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SystemTypeApi } from "../../../Business/SystemTypeBusiness";
import { DataRemediationDto, ResultDataRemediationDto, ResultDto, ResultDtoOfResultDataRemediationDto } from "../../../Model/CommonModels";
import { ConstraintInfoDto, LifecycleConstraintDto, LifecycleConstraintQueryDto } from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { ForeignIndexApi } from '../../../Business/ForeignIndexBusiness';
import { ResultDtoOfForeignIndexDto, ForeignIndexDto } from '../../../Model/ForeignIndexModel';

export async function GetDesignComponentOrphans(id?: number) {
	let api = new ForeignIndexApi();
	setLoader("ADD", "GetDesignComponentOrphans");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfForeignIndexDto>>(() => api.foreignIndexGetOrphanDesignComponents(id));
	if (result && !result.warning) {
		rootStore.dispatch({ type: "GET_ORPHANS_FOREIGN_INDEX", payload: result.data });
		setLoader("REMOVE", "GetDesignComponentOrphans");
		return result as ResultDtoOfForeignIndexDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetDesignComponentOrphans");
		return result as ResultDtoOfForeignIndexDto;
	}
}

export async function GetSystemTypeOrphans(data: ForeignIndexDto) {
	let api = new ForeignIndexApi();
	setLoader("ADD", "GetSystemTypeOrphans");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfForeignIndexDto>>(() => api.foreignIndexGetOrphanSystemTypes(data));
	if (result && !result.warning) {
		rootStore.dispatch({ type: "GET_ORPHANS_FOREIGN_INDEX", payload: result.data });
		setLoader("REMOVE", "GetSystemTypeOrphans");
		return result as ResultDtoOfForeignIndexDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetSystemTypeOrphans");
		return result as ResultDtoOfForeignIndexDto;
	}
}

export async function GetGroupsForeignIndex(data: ForeignIndexDto) {
	let api = new ForeignIndexApi();
	setLoader("ADD", "GetGroupsForeignIndex");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfForeignIndexDto>>(() => api.foreignIndexGetGroups(data));
	if (result && !result.warning) {
		rootStore.dispatch({ type: "GET_ORPHANS_FOREIGN_INDEX", payload: result.data });
		setLoader("REMOVE", "GetGroupsForeignIndex");
		return result as ResultDtoOfForeignIndexDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetGroupsForeignIndex");
		return result as ResultDtoOfForeignIndexDto;
	}
}

export async function GetImpactCheckForeignIndex(data: ForeignIndexDto) {
	let api = new ForeignIndexApi();
	setLoader("ADD", "GetGroupsForeignIndex");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfForeignIndexDto>>(() => api.foreignIndexImpactCheck(data));
	if (result && !result.warning) {
		rootStore.dispatch({ type: "GET_ORPHANS_FOREIGN_INDEX", payload: result.data });
		setLoader("REMOVE", "GetGroupsForeignIndex");
		return result as ResultDtoOfForeignIndexDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "GetGroupsForeignIndex");
		return result as ResultDtoOfForeignIndexDto;
	}
}

export async function ApplyDataRefactoring(data: ForeignIndexDto) {
	let api = new ForeignIndexApi();
	setLoader("ADD", "ApplyDataRefactoring");
	let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfForeignIndexDto>>(() => api.foreignIndexApply(data));
	if (result && !result.warning) {
		rootStore.dispatch({ type: "GET_ORPHANS_FOREIGN_INDEX", payload: result.data });
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "ApplyDataRefactoring");
		return result as ResultDtoOfForeignIndexDto;
	} else {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
		setLoader("REMOVE", "ApplyDataRefactoring");
		return result as ResultDtoOfForeignIndexDto;
	}
}

export async function CancelForeignIndex(data: ForeignIndexDto) {
	let api = new ForeignIndexApi();
	setLoader("ADD", "CancelForeignIndex");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.foreignIndexCancelForeignIndex(data));
	if (result && result.warning) {
		rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	}
	setLoader("REMOVE", "CancelForeignIndex");

}

export async function ResetForeignIndex() {
	let api = new ForeignIndexApi();
	setLoader("ADD", "ResetForeignIndex");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.foreignIndexResetForeignIndex());
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	setLoader("REMOVE", "ResetForeignIndex");

}
