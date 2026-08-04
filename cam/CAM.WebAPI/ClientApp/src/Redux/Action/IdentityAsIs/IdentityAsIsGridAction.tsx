import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {IdentityAsIsApi } from "../../../Business/IdentityAsIs";
import {
  GET_FILTER_IDENTITYASIS,
  GET_GRID_IDENTITYASIS,
 IdentityAsIsDtoGrid,
 IdentityAsIsGrid,
 IdentityAsIsQueryObjectGrid,
  QueryResultDtoOfIdentityAsIsDtoGrid,
} from "../../../Model/LookUp/Identities";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetIdentityAsIsGrid(
  queryFilter?:IdentityAsIsQueryObjectGrid,
  returnValues?: boolean,
  hideLoader?: boolean
) {
  if (hideLoader !== true) {
    setLoader("ADD", "GetMIdentityAsIsGrid");
  }
  let api = new IdentityAsIsApi();
  let result: QueryResultDtoOfIdentityAsIsDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfIdentityAsIsDtoGrid>
    >(() => api.IdentityAsIsGetIdentityAsIs(queryFilter ?? {}));

    let rtn = {
     IdentityAsIsGridResult: result,
      filter: null,
    } as IdentityAsIsGrid;
    if (returnValues !== true) {
      rootStore.dispatch({ type: GET_GRID_IDENTITYASIS, payload: rtn });
    } else {
      setLoader("REMOVE", "GetMIdentityAsIsGrid");
      return rtn.IdentityAsIsGridResult
        ?.items as IdentityAsIsDtoGrid[];
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_IDENTITYASIS,
      payload: {
       IdentityAsIsGridResult: null,
        filter: null,
      } as IdentityAsIsGrid,
    });
  }
  setLoader("REMOVE", "GetMIdentityAsIsGrid");
}

export async function GetFilterColumIdentityAsIs(
  columName: string,
  columValue: string,
  queryFilter?:IdentityAsIsQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new IdentityAsIsApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.IdentityAsIsGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
   IdentityAsIsGridResult: null,
  } as IdentityAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_IDENTITYASIS, payload: rtn });
}
