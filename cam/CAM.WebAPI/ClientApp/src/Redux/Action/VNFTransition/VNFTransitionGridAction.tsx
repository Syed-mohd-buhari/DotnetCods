import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { VNFTransitionApi } from "../../../Business/VNFTransitionBusiness";
import { VNFTransitionGrid, VNFTransitionQueryObjectGrid, GET_FILTER_VNF_TRANSITION, GET_GRID_VNF_TRANSITION, QueryResultDtoOfVNFTransitionDtoGrid } from "../../../Model/VNFTransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVNFTransitionGrid(queryFilter?: VNFTransitionQueryObjectGrid) {
	setLoader("ADD", "GetVNFTransitionGrid");

	let result: QueryResultDtoOfVNFTransitionDtoGrid | null | undefined;
	let api = new VNFTransitionApi();

	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfVNFTransitionDtoGrid>>(() =>
			api.vNFTransitionGetVNFTransition(
				queryFilter ?? {}
			)
		);

		let rtn = { VNFTransitionGridResult: result, filter: null } as VNFTransitionGrid;
		rootStore.dispatch({ type: GET_GRID_VNF_TRANSITION, payload: rtn as VNFTransitionGrid });
	} catch (error) {
		rootStore.dispatch({ type: GET_GRID_VNF_TRANSITION, payload: { VNFTransitionGridResult: result, filter: null } as VNFTransitionGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetVNFTransitionGrid");
}

export async function GetFilterColumVNFTransition(columName: string, columValue: string, queryFilter?: VNFTransitionQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumVNFTransition");

	let result: FilterValueDto[] | undefined;
	let api = new VNFTransitionApi();

	result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
		api.vNFTransitionGetFilterResult(
			queryFilter ?? {}, columName,
			columValue,
		)
	);

	let rtn = { filter: result, VNFTransitionGridResult: null } as VNFTransitionGrid;
	rootStore.dispatch({ type: GET_FILTER_VNF_TRANSITION, payload: rtn });
	// setLoader("REMOVE", "GetFilterColumVNFTransition");
}
