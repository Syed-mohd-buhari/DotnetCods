import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BenefitApi } from "../../../../Business/LookUp/BenefitsBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteBenefits(id: number) {
	setLoader("ADD", "deleteBenefits");
	let api = new BenefitApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.benefitDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_BENEFITS", payload: rtn });
	setLoader("REMOVE", "deleteBenefits");
	return rtn;
}

export async function DeleteDeepBenefits(id: number) {
	let api = new BenefitApi();
	setLoader("ADD", "DeleteDeepBenefits");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.benefitDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_BENEFITS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepBenefits");
	return rtn;
}

export async function GetRelatedRecordsBenefits(id: number) {
	let api = new BenefitApi();
	setLoader("ADD", "GetRelatedRecordsBenefits");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.benefitGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsBenefits");
	return rtn;
}
