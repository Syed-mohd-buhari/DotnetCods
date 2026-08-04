import React from "react";

import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BuildConstructionApi } from "../../../../Business/LookUp/BuildConstructionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDtoRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetBuildConstructionEditResource(id: number) {
	setLoader("ADD", "GetBuildConstructionEditResource");

	let api = new BuildConstructionApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoRule>>(() => api.buildConstructionGetUpdateResourceBuildConstruction(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_BUILD_CONSTRUCTION", payload: rtn });
	setLoader("REMOVE", "GetBuildConstructionEditResource");

	return rtn;
}

export async function EditBuildConstruction(data: TipologicaGridDtoRule) {
	setLoader("ADD", "EditBuildConstruction");
	let api = new BuildConstructionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.buildConstructionPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDtoRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_BUILD_CONSTRUCTION", payload: rtn });
	setLoader("REMOVE", "EditBuildConstruction");
	return rtn;
}
