import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EndOfSupportContractApi } from "../../../../Business/LookUp/EndOfSupportContract";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetEndOfSupportContractEditResource(id: number) {
	setLoader("ADD", "GetEndOfSupportContractEditResource");

	let api = new EndOfSupportContractApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.endOfSupportContractGetUpdateResourceActivityStatus(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_END_OF_SUPPORT_CONTRACT", payload: rtn });
	setLoader("REMOVE", "GetEndOfSupportContractEditResource");

	return rtn;
}

export async function EditEndOfSupportContract(data: TipologicaGridDto) {
	setLoader("ADD", "EditEndOfSupportContract");
	let api = new EndOfSupportContractApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.endOfSupportContractPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_END_OF_SUPPORT_CONTRACT", payload: rtn });
	setLoader("REMOVE", "EditEndOfSupportContract");
	return rtn;
}
