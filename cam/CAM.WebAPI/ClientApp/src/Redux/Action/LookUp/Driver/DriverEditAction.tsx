import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DriverApi } from "../../../../Business/LookUp/DriverBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetDriverEditResource(id: number) {
	setLoader("ADD", "GetDriverEditResource");

	let api = new DriverApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.driverGetUpdateResourceDriver(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_DRIVER", payload: rtn });
	setLoader("REMOVE", "GetDriverEditResource");

	return rtn;
}

export async function EditDriver(data: TipologicaGridDto) {
	setLoader("ADD", "EditDriver");
	let api = new DriverApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.driverPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_DRIVER", payload: rtn });
	setLoader("REMOVE", "EditDriver");
	return rtn;
}
