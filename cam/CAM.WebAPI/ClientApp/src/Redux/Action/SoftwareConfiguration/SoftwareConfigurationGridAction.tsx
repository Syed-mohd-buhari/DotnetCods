import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { SoftwareConfigurationApi } from "../../../Business/SoftwareConfigurationBusiness";
import { PATReport } from "../../../Business/PlannedActivityReportTrackerBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  GET_GRID_SOFTWARE_CONFIGURATION,
  NetworkElementAsIsGrid,
  SoftwareConfigurationGrid,
  SoftwareConfigurationQueryObjectGrid,
  QueryResultDtoOfSoftwareConfigurationDtoGrid,
  SWOEM_MODEL,
  GET_FILTER_OPCO,
  GET_FILTER_OEM,
  GET_FILTER_ELE,
  GET_FILTER_SUB_FUNCTION_FILTER,
  GET_GRID_SUB_FUNCTION_FILTER,
  SubFunctionFilterGrid,
} from "../../../Model/SoftwareConfiguration";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSoftwareConfigurationGrid(
  queryFilter?: SoftwareConfigurationQueryObjectGrid
) {
  setLoader("ADD", "GetSoftwareConfigurationGrid");
  let api = new SoftwareConfigurationApi();

  let result: QueryResultDtoOfSoftwareConfigurationDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfSoftwareConfigurationDtoGrid>
    >(() =>
      api.softwareConfigurationGetSoftwareConfiguration(queryFilter ?? {})
    );

    let rtn = {
      SoftwareConfigurationGridResult: result,
      filter: null,
    } as SoftwareConfigurationGrid;

    rootStore.dispatch({ type: GET_GRID_SOFTWARE_CONFIGURATION, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_SOFTWARE_CONFIGURATION,
      payload: {
        SoftwareConfigurationGridResult: null,
        filter: null,
      } as SoftwareConfigurationGrid,
    });
  }
  setLoader("REMOVE", "GetSoftwareConfigurationGrid");
}

export async function GetSWOem() {
  setLoader("ADD", "GetSWOem");
  let api = new PATReport();

  let rtn = await ApiCallWithErrorHandling<Promise<SWOEM_MODEL>>(() =>
    api.getSWOem()
  );

  let result = {
    SWOemResources: rtn,
  } as SWOEM_MODEL;

  setLoader("REMOVE", "GetSWOem");
  return result;
}

export async function GetFilterColumSoftwareConfiguration(
  columName: string,
  columValue: string,
  queryFilter?: SoftwareConfigurationQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SoftwareConfigurationApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.softwareConfigurationGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_NETWORK_ELEMENT_AS_IS, payload: rtn });
}

export async function GetSubFuntionFilter(
  queryFilter: SoftwareConfigurationQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SoftwareConfigurationApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.softwareConfigurationSubFunctionFilter(queryFilter ?? {})
  );
  let rtn = {
    filter: null,
    SubFunctionFilterGridResult: result,
  } as SubFunctionFilterGrid;
  rootStore.dispatch({ type: GET_GRID_SUB_FUNCTION_FILTER, payload: rtn });
}

export async function GetDropdownFilterSoftwareConfiguration() {
  let result;
  let api = new SoftwareConfigurationApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.softwareConfigurationDropdownFilterResult()
  );

  return result;
}

export async function GetFilterOpcoSoftwareConfiguration(
  columName: string,
  columValue: string,
  queryFilter?: SoftwareConfigurationQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SoftwareConfigurationApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.softwareConfigurationGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_OPCO, payload: rtn });
  return result;
}

export async function GetFilterOemSoftwareConfiguration(
  columName: string,
  columValue: string,
  queryFilter?: SoftwareConfigurationQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SoftwareConfigurationApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.softwareConfigurationGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_OEM, payload: rtn });
  return result;
}

export async function GetFilterEleSoftwareConfiguration(
  columName: string,
  columValue: string,
  queryFilter?: SoftwareConfigurationQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SoftwareConfigurationApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.softwareConfigurationGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_ELE, payload: rtn });
  return result;
}
