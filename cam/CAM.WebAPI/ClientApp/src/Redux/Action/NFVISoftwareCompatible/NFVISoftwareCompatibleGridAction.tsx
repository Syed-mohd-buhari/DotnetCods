import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { NFVISwCompatibleApi } from "../../../Business/NFVISoftwareCompatibleBusiness";
import {
  GET_FILTER_NFVI_COMPATIBLE,
  GET_GRID_NFVI_COMPATIBLE,
  NFVISwCompatibleGrid,
  NFVISwCompatibleQueryObjectGrid,
  QueryResultDtoOfNFVISwCompatibleDtoGrid,
} from "../../../Model/NFVISoftwareCompatible";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNFVISwCompatibleGrid(
  queryFilter?: NFVISwCompatibleQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetNFVISwCompatibleGrid");

  let result: QueryResultDtoOfNFVISwCompatibleDtoGrid | null | undefined;
  let api = new NFVISwCompatibleApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfNFVISwCompatibleDtoGrid>
    >(() => api.NFVISoftwareCompatibleGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_NFVI_COMPATIBLE,
        payload: {
          NFVISwCompatibleGridResult: result,
          filter: null,
        } as NFVISwCompatibleGrid,
      });
    } else {
      setLoader("REMOVE", "GetNFVISwCompatibleGrid");

      return result?.items;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_NFVI_COMPATIBLE,
      payload: {
        NFVISwCompatibleGridResult: null,
        filter: null,
      } as NFVISwCompatibleGrid,
    });
  }
  setLoader("REMOVE", "GetNFVISwCompatibleGrid");
}

export async function GetFilterColumnNFVISwCompatible(
  columName: string,
  columValue: string,
  queryFilter?: NFVISwCompatibleQueryObjectGrid
) {
  let api = new NFVISwCompatibleApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.NFVISwCompatibleGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NFVISwCompatibleGridResult: null,
  } as NFVISwCompatibleGrid;
  rootStore.dispatch({ type: GET_FILTER_NFVI_COMPATIBLE, payload: rtn });

  return rtn;
}
