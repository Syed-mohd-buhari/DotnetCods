import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DriverApi } from "../../../../Business/LookUp/DriverBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDriverCreateResource() {
	setLoader("ADD", "GetDriverCreateResource");

	let api = new DriverApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.driverGetCreateResourceDriver());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_DRIVER", payload: rtn });
	setLoader("REMOVE", "GetDriverCreateResource");
}

export async function CreatDriver(data: TipologicaGridDto) {
	setLoader("ADD", "CreatDriver");
	let api = new DriverApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.driverCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_DRIVER", payload: rtn });
	setLoader("REMOVE", "CreatDriver");
	return rtn;
}
