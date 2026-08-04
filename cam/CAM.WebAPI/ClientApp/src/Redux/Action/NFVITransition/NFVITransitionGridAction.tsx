import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { NFVITransitionApi } from "../../../Business/NFVITransitionBusiness";
import { NFVITransitionGrid, NFVITransitionQueryObjectGrid, GET_FILTER_NFVI_TRAMSITION, GET_GRID_NFVI_TRAMSITION, QueryResultDtoOfNFVITransitionDtoGrid } from "../../../Model/NFVITransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNFVITransitionGrid(queryFilter?: NFVITransitionQueryObjectGrid) {
	setLoader("ADD", "GetNFVITransitionGrid");

	let result: QueryResultDtoOfNFVITransitionDtoGrid | null | undefined;
	let api = new NFVITransitionApi();

	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfNFVITransitionDtoGrid>>(() =>
			api.nFVITransitionGetVNFTransition(
				queryFilter ?? {}
			)
		);

		let rtn = { NFVITransitionGridResult: result, filter: null } as NFVITransitionGrid;
		rootStore.dispatch({ type: GET_GRID_NFVI_TRAMSITION, payload: rtn as NFVITransitionGrid });
	} catch (error) {
		rootStore.dispatch({ type: GET_GRID_NFVI_TRAMSITION, payload: { NFVITransitionGridResult: result, filter: null } as NFVITransitionGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetNFVITransitionGrid");
}

export async function GetFilterColumNFVITransition(columName: string, columValue: string, queryFilter?: NFVITransitionQueryObjectGrid) {

	let result: FilterValueDto[] | undefined;
	let api = new NFVITransitionApi();
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.nFVITransitionGetFilterResult(
				queryFilter ?? {}, columName,
				columValue,
			)
		);
	let rtn = { filter: result, NFVITransitionGridResult: null } as NFVITransitionGrid;
	rootStore.dispatch({ type: GET_FILTER_NFVI_TRAMSITION, payload: rtn });
}
