import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentApiFetchParamCreator, DesignComponentApi } from "../../../Business/DesignComponentBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DesignComponentDtoUpdate, DesignComponentEdit, EDIT_DESIGN_COMPONENT, GET_EDIT_DESIGN_COMPONENT } from "../../../Model/DesignComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDesignComponentEditResource(id: number) {
	setLoader("ADD", "GetDesignComponentEditResource");

	// const dispach = useDispatch();
	let api = new DesignComponentApi();
	let createResource = await ApiCallWithErrorHandling<Promise<DesignComponentDtoUpdate>>(() => api.designComponentGetUpdateResourceDesignComponent(id));
	let rtn = { DesignComponentDtoEdit: createResource } as DesignComponentEdit;
	rootStore.dispatch({ type: GET_EDIT_DESIGN_COMPONENT, payload: rtn });
	setLoader("REMOVE", "GetDesignComponentEditResource");

	return rtn;
}

export async function EditDesignComponent(data: DesignComponentDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditDesignComponent");
	let api = new DesignComponentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentPut(data, forced));
	let rtn = { ResultDtoEdit: result } as DesignComponentEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_DESIGN_COMPONENT, payload: rtn });
	setLoader("REMOVE", "EditDesignComponent");
	return rtn;
}
