import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentApiFetchParamCreator, DesignComponentApi } from "../../../Business/DesignComponentBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { CREATE_DESIGN_COMPONENT, DesignComponentCreate, DesignComponentDtoCreate, GET_CREATE_DESIGN_COMPONENT } from "../../../Model/DesignComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'
import { InizializeNewProductCreateDto } from "../../../Model/InizializeNewProduct";
import { InizializeNewProductApi } from "../../../Business/InizializeNewProductBusiness";

export async function CreateInizializeNewProduct(data: InizializeNewProductCreateDto) {
	setLoader("ADD", "CreateInizializeNewProduct");
	let api = new InizializeNewProductApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.inizializeNewProductCreate(data));
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	setLoader("REMOVE", "CreateInizializeNewProduct");
	return result as ResultDto;
}
