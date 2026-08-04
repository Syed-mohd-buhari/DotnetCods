import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EndOfSupportContractApi } from "../../../../Business/LookUp/EndOfSupportContract";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetEndOfSupportContractCreateResource() {
	setLoader("ADD", "GetEndOfSupportContractCreateResource");

	let api = new EndOfSupportContractApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.endOfSupportContractGetCreateResourceActivityStatus());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_END_OF_SUPPORT_CONTRACT", payload: rtn });
	setLoader("REMOVE", "GetEndOfSupportContractCreateResource");
}

export async function CreatEndOfSupportContract(data: TipologicaGridDto) {
	setLoader("ADD", "CreatEndOfSupportContract");
	let api = new EndOfSupportContractApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.endOfSupportContractCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_END_OF_SUPPORT_CONTRACT", payload: rtn });
	setLoader("REMOVE", "CreatEndOfSupportContract");
	return rtn;
}
