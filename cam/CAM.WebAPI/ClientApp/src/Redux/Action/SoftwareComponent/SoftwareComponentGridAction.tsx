import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { SoftwareComponentApi } from "../../../Business/SoftwareComponentBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  GET_GRID_SOFTWARE_COMPONENT,
  NetworkElementAsIsGrid,
  SoftwareComponentGrid,
  SoftwareComponentQueryObjectGrid,
  QueryResultDtoOfSoftwareComponentDtoGrid,
} from "../../../Model/SoftwareComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSoftwareComponentGrid(
  queryFilter?: SoftwareComponentQueryObjectGrid
) {
  setLoader("ADD", "GetSoftwareComponentGrid");
  let api = new SoftwareComponentApi();

  let result: QueryResultDtoOfSoftwareComponentDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfSoftwareComponentDtoGrid>
    >(() => api.softwareComponentGetSoftwareComponent(queryFilter ?? {}));

    let rtn = {
      SoftwareComponentGridResult: result,
      filter: null,
    } as SoftwareComponentGrid;

    rootStore.dispatch({ type: GET_GRID_SOFTWARE_COMPONENT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_SOFTWARE_COMPONENT,
      payload: {
        SoftwareComponentGridResult: null,
        filter: null,
      } as SoftwareComponentGrid,
    });
  }
  setLoader("REMOVE", "GetSoftwareComponentGrid");
}

export async function GetFilterColumNetworkElementAsIs(
  columName: string,
  columValue: string,
  queryFilter?: SoftwareComponentQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SoftwareComponentApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.networkElementAsIsGetFilterResult(
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

export async function GetFilterColumSoftwareComponent(
  columName: string,
  columValue: string,
  queryFilter?: SoftwareComponentQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SoftwareComponentApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.softwareComponentGetFilterResult(
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
