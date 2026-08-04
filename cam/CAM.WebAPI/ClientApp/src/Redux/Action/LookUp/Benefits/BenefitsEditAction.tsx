import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BenefitApi } from "../../../../Business/LookUp/BenefitsBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetBenefitsEditResource(id: number) {
	setLoader("ADD", "GetBenefitsEditResource");

	let api = new BenefitApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.benefitGetUpdateResourceBenefit(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_BENEFITS", payload: rtn });
	setLoader("REMOVE", "GetBenefitsEditResource");

	return rtn;
}

export async function EditBenefits(data: TipologicaGridDto) {
	setLoader("ADD", "EditBenefits");
	let api = new BenefitApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.benefitPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_BENEFITS", payload: rtn });
	setLoader("REMOVE", "EditBenefits");
	return rtn;
}
