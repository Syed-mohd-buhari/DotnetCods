import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFDesignComponentApi } from "../../../../Business/LookUp/VNFDesignComponentBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVNFDesignComponentEditResource(id: number) {
	setLoader("ADD", "GetVNFDesignComponentEditResource");

	let api = new VNFDesignComponentApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.vNFDesignComponentGetUpdateResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_VNF_DESIGN_COMPONENT", payload: rtn });
	setLoader("REMOVE", "GetVNFDesignComponentEditResource");

	return rtn;
}

export async function EditVNFDesignComponent(data: TipologicaGridDto) {
	setLoader("ADD", "EditVNFDesignComponent");
	let api = new VNFDesignComponentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFDesignComponentPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_VNF_DESIGN_COMPONENT", payload: rtn });
	setLoader("REMOVE", "EditVNFDesignComponent");
	return rtn;
}
