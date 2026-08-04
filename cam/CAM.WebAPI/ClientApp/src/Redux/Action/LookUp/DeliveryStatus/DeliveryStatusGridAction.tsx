import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { DeliveryStatusApi } from "../../../../Business/LookUp/DeliveryStatusBusiness";
import { TipologicaGridDtoCombinationRule, QueryResultDtoOfTipologicaGridDtoCombinationRule, TipologicheQueryObjectGrid, LookUpGrid, TipologicheQueryObjectGridCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDeliveryStatusGrid(queryFilter?: TipologicheQueryObjectGridCombinationRule) {
	setLoader("ADD", "GetDeliveryStatusGrid");

	let result: QueryResultDtoOfTipologicaGridDtoCombinationRule | null | undefined;
	let api = new DeliveryStatusApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoCombinationRule>>(() =>
				api.deliveryStatusGetDeliveryStatus(
					queryFilter?.projectStatusCombinationRule,
					queryFilter?.rule,
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.sortBy,
					queryFilter?.isSortAscending,
					queryFilter?.page,
					queryFilter?.pageSize,
					queryFilter?.lastModifiedStartDate,
					queryFilter?.lastModifiedEndDate,
					queryFilter?.principalId,
					queryFilter?.deleted,
					queryFilter?.orphan,
					queryFilter?.lastModifiedBy
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoCombinationRule>>(() => api.deliveryStatusGetDeliveryStatus());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoCombinationRule;
		rootStore.dispatch({ type: "GET_GRID_DELIVERY_STATUS", payload: rtn as TipologicaGridDtoCombinationRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_DELIVERY_STATUS", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetDeliveryStatusGrid");
}

export async function GetDeliveryStatusGridALL() {
	setLoader("ADD", "GetDeliveryStatusGridALL");

	let result: QueryResultDtoOfTipologicaGridDtoCombinationRule | null | undefined;
	let api = new DeliveryStatusApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoCombinationRule>>(() => api.deliveryStatusGetDeliveryStatus());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoCombinationRule;
		rootStore.dispatch({ type: "GET_GRID_DELIVERY_STATUS_ALL", payload: rtn as TipologicaGridDtoCombinationRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_DELIVERY_STATUS_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetDeliveryStatusGridALL");
}

export async function GetFilterColumDeliveryStatus(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGridCombinationRule) {
	// setLoader("ADD", "GetFilterColumDeliveryStatus");

	let result: FilterValueDto[] | undefined;
	let api = new DeliveryStatusApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.deliveryStatusGetFilterResult(
				columName,
				columValue,
				queryFilter?.projectStatusCombinationRule,
				queryFilter?.rule,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.sortBy,
				queryFilter?.isSortAscending,
				queryFilter?.page,
				queryFilter?.pageSize,
				queryFilter?.lastModifiedStartDate,
				queryFilter?.lastModifiedEndDate,
				queryFilter?.principalId,
				queryFilter?.deleted,
				queryFilter?.orphan,
				queryFilter?.lastModifiedBy
			)
		);
	} else {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.deliveryStatusGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_DELIVERY_STATUS", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumDeliveryStatus");
}
