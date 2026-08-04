import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BenefitApi } from "../../../../Business/LookUp/BenefitsBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetBenefitsCreateResource() {
	setLoader("ADD", "GetBenefitsCreateResource");

	let api = new BenefitApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.benefitGetCreateResourceBenefit());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_BENEFITS", payload: rtn });
	setLoader("REMOVE", "GetBenefitsCreateResource");
}

export async function CreatBenefits(data: TipologicaGridDto) {
	setLoader("ADD", "CreatBenefits");
	let api = new BenefitApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.benefitCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_BENEFITS", payload: rtn });
	setLoader("REMOVE", "CreatBenefits");
	return rtn;
}
