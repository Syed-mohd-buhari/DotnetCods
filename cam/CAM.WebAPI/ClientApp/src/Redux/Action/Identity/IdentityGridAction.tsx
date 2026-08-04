import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { IdentityApi } from "../../../Business/IdentityBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  GET_GRID_IDENTITY,
  NetworkElementAsIsGrid,
  IdentityGrid,
  IdentityQueryObjectGrid,
  QueryResultDtoOfIdentityDtoGrid,
} from "../../../Model/Identity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetIdentityGrid(queryFilter?: IdentityQueryObjectGrid) {
  setLoader("ADD", "GetIdentityGrid");
  let api = new IdentityApi();

  let result: QueryResultDtoOfIdentityDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfIdentityDtoGrid>
    >(() => api.identityGetIdentity(queryFilter ?? {}));

    let rtn = { IdentityGridResult: result, filter: null } as IdentityGrid;

    rootStore.dispatch({ type: GET_GRID_IDENTITY, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_IDENTITY,
      payload: { IdentityGridResult: null, filter: null } as IdentityGrid,
    });
  }
  setLoader("REMOVE", "GetIdentityGrid");
}

export async function GetFilterColumIdentity(
  columName: string,
  columValue: string,
  queryFilter?: IdentityQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new IdentityApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.identityGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_NETWORK_ELEMENT_AS_IS, payload: rtn });
}
