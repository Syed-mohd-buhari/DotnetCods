import { TSRReportApi } from "../../../Business/TSRReportBusiness";

import { FNTReportApi } from "../../../Business/FNTReportBusiness";
import {
  GET_FILTER_NON_TEMS_FNT_REPORT,
  GET_GRID_NON_TEMS_FNT_REPORT,
  QueryResultDtoOfFNTReportDtoGrid,
  NonTemsFNTReportGrid,
  FNTReportQueryObjectGrid,
} from "../../../Model/FNTReport";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";

export async function GetNonTemsFNTReportGrid(
  queryFilter?: FNTReportQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetNonTemsFNTReportGrid");

  let result: QueryResultDtoOfFNTReportDtoGrid | null | undefined;
  let api = new FNTReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfFNTReportDtoGrid>
    >(() => api.NonTemsFNTReportGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_NON_TEMS_FNT_REPORT,
        payload: {
          NonTemsFNTReportGridResult: result,
          filter: null,
        } as NonTemsFNTReportGrid,
      });
    } else {
      setLoader("REMOVE", "GetNonTemsFNTReportGrid");

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
      type: GET_GRID_NON_TEMS_FNT_REPORT,
      payload: {
        NonTemsFNTReportGridResult: null,
        filter: null,
      } as NonTemsFNTReportGrid,
    });
  }
  setLoader("REMOVE", "GetNonTemsFNTReportGrid");
}

export async function GetNonTemsFilterColumFNTReport(
  columName: string,
  columValue: string,
  queryFilter?: FNTReportQueryObjectGrid
) {
  let api = new FNTReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.NonTemsFNTReportGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NonTemsFNTReportGridResult: null,
  } as NonTemsFNTReportGrid;
  rootStore.dispatch({ type: GET_FILTER_NON_TEMS_FNT_REPORT, payload: rtn });

  return rtn;
}

// export async function GetTSRReportVerticalGrid(
//   queryFilter?: TipologicheQueryObjectGrid,
//   returnValues?: boolean
// ) {
//   setLoader("ADD", "GetTSRReportVerticalGrid");

//   let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
//   let api = new TSRReportApi();
//   try {
//     if (queryFilter !== null && queryFilter !== undefined) {
//       result = await ApiCallWithErrorHandling<
//         Promise<QueryResultDtoOfTipologicaGridDto>
//       >(() => api.tsrReportGetVerticalGrid(queryFilter));
//     } else {
//       result = await ApiCallWithErrorHandling<
//         Promise<QueryResultDtoOfTipologicaGridDto>
//       >(() => api.tsrReportGetVerticalGrid({}));
//     }
//     // if (result?.items?.length === 0 || result?.totalItems === undefined) {
//     //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
//     // }
//     let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
//     rootStore.dispatch({
//       type: "GET_GRID_TSR_VERTICAL",
//       payload: rtn as TipologicaGridDto,
//     });
//     setLoader("REMOVE", "GetTSRReportVerticalGrid");

//     return rtn;
//   } catch (error) {
//     rootStore.dispatch({
//       type: "GET_GRID_TSR_VERTICAL",
//       payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
//     });
//     rootStore.dispatch(
//       setNotification({
//         message: "Fail to fetch",
//         notifyType: NotifyType.error,
//       })
//     );
//   }
//   setLoader("REMOVE", "GetTSRReportVerticalGrid");
// }
