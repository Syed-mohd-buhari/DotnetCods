import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFDesignComponentApi } from "../../../../Business/LookUp/VNFDesignComponentBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVNFDesignComponentCreateResource() {
	setLoader("ADD", "GetVNFDesignComponentCreateResource");

	let api = new VNFDesignComponentApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.vNFDesignComponentGetCreateResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_VNF_DESIGN_COMPONENT", payload: rtn });
	setLoader("REMOVE", "GetVNFDesignComponentCreateResource");
}

export async function CreatVNFDesignComponent(data: TipologicaGridDto) {
	setLoader("ADD", "CreatVNFDesignComponent");
	let api = new VNFDesignComponentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFDesignComponentCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_VNF_DESIGN_COMPONENT", payload: rtn });
	setLoader("REMOVE", "CreatVNFDesignComponent");
	return rtn;
}
