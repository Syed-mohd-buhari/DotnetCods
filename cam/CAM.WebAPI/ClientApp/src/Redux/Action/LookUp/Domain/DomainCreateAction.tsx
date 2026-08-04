import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SystemNamesApi } from "../../../../Business/LookUp/DomainBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { LookUpCreate, LookUpForSystemNamesCreate } from "../../../../Model/LookUp/LookUpGenericModel";
import { SystemNamesDto } from "../../../../Model/LookUp/Domain";

// import { useDispatch } from 'react-redux'

export async function GetSystemNameCreateResource() {
	setLoader("ADD", "GetSystemNameCreateResource");

	let api = new SystemNamesApi();
	let createResource = await ApiCallWithErrorHandling<Promise<SystemNamesDto>>(() => api.systemNamesGetCreateResourceSystemNames());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpForSystemNamesCreate;
	rootStore.dispatch({ type: "GET_CREATE_SYSTEM_NAME", payload: rtn });
	setLoader("REMOVE", "GetSystemNameCreateResource");
}

export async function CreateSystemName(data: SystemNamesDto) {
	setLoader("ADD", "CreateSystemName");
	let api = new SystemNamesApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemNamesCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpForSystemNamesCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_SYSTEM_NAME", payload: rtn });
	setLoader("REMOVE", "CreateSystemName");
	return rtn;
}

