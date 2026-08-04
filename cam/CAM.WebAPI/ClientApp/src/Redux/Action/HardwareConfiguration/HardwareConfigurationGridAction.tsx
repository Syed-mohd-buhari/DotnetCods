import { ApiCallWithErrorHandling, FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { HardwareConfigurationApi } from "../../../Business/HardwareConfigurationBusiness";
import { GET_FILTER_NETWORK_ELEMENT_AS_IS, GET_GRID_HARDWARE_CONFIGURATION, NetworkElementAsIsGrid,HardwareConfigurationGrid, HardwareConfigurationQueryObjectGrid, QueryResultDtoOfHardwareConfigurationDtoGrid } from "../../../Model/HardwareConfiguration";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";


export async function GetHardwareConfigurationGrid(queryFilter?: HardwareConfigurationQueryObjectGrid) {
	
	setLoader("ADD", "GetHardwareConfigurationGrid");
	let api = new HardwareConfigurationApi();

	let result: QueryResultDtoOfHardwareConfigurationDtoGrid | null | undefined;
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfHardwareConfigurationDtoGrid>>(() =>
			api.hardwareConfigurationGetHardwareConfiguration(
				queryFilter ?? {}
			)
		);
		
		let rtn = { HardwareConfigurationGridResult: result, filter: null } as HardwareConfigurationGrid;

		rootStore.dispatch({ type: GET_GRID_HARDWARE_CONFIGURATION, payload: rtn });
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
		rootStore.dispatch({ type: GET_GRID_HARDWARE_CONFIGURATION, payload: { HardwareConfigurationGridResult: null, filter: null } as HardwareConfigurationGrid });
	}
	setLoader("REMOVE", "GetHardwareConfigurationGrid");
}



export async function GetFilterColumHardwareConfiguration(columName: string, columValue: string, queryFilter?: HardwareConfigurationQueryObjectGrid) {
	let result: FilterValueDto[] | undefined;
	let api = new HardwareConfigurationApi();
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.hardwareConfigurationGetFilterResult(
				queryFilter ?? {}, columName,
				columValue
			)
		);
	let rtn = { filter: result, NetworkElementAsIsGridResult: null } as NetworkElementAsIsGrid;
	rootStore.dispatch({ type: GET_FILTER_NETWORK_ELEMENT_AS_IS, payload: rtn });
}
