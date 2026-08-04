import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { PassThroughHardwareReportApi } from "../../../Business/Report/PassThroughHardwareBusiness";
import { PassThroughReportApi } from "../../../Business/Report/PassThroughReportBusiness";

import {
  PassThroughHardwareReportDtoGrid,
  PassThroughHardwareReportGrid,
  PassThroughHardwareReportQueryObjectGrid,
  QueryResultDtoOfPassThroughHardwareReportDtoGrid,
  GET_GRID_PASSTHROUGH_HARDWARE_REPORT,
  GET_FILTER_PASSTHROUGH_HARDWARE_REPORT,
} from "../../../Model/Report/PassThroughHardwareReportExport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";
import { SharedLookUpApi } from "../../../Business/LookUp/SharedLookUpBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";
import { ReportSoftwareApi } from "../../../Business/Report/ReportSoftwareBusiness";
import { RiTentLine } from "react-icons/ri";

export async function GetPassThroughHardwareReportGrid(
  queryFilter?: PassThroughHardwareReportQueryObjectGrid
) {
  setLoader("ADD", "GetPassThroughHardwareReportGrid");

  let result:
    | QueryResultDtoOfPassThroughHardwareReportDtoGrid
    | null
    | undefined;
  let api = new PassThroughHardwareReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPassThroughHardwareReportDtoGrid>
    >(() => api.PassThroughHardwareReportGetReport(queryFilter ?? {}));

    let rtn = {
      PassThroughHardwareReportGridResult: result,
      filter: null,
    } as PassThroughHardwareReportGrid;
    rootStore.dispatch({
      type: GET_GRID_PASSTHROUGH_HARDWARE_REPORT,
      payload: rtn,
    });
    // return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      PassThroughHardwareReportGridResult: null,
      filter: null,
    } as PassThroughHardwareReportGrid;
    rootStore.dispatch({
      type: GET_GRID_PASSTHROUGH_HARDWARE_REPORT,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetPassThroughHardwareReportGrid");
}

export async function GetFilterColumPassThroughHardwareReport(
  columName: string,
  columValue: string,
  queryFilter?: PassThroughHardwareReportQueryObjectGrid
) {
  let api = new PassThroughHardwareReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.PassThroughHardwareReportGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    PassThroughHardwareReportGridResult: null,
  } as PassThroughHardwareReportGrid;
  rootStore.dispatch({
    type: GET_FILTER_PASSTHROUGH_HARDWARE_REPORT,
    payload: rtn,
  });
}

export async function GetSharedLookUpGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SharedLookUpApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.sharedLookUpGetSharedLookUp(
          queryFilter?.sortBy,
          queryFilter?.isSortAscending,
          queryFilter?.page,
          queryFilter?.pageSize,
          queryFilter?.lastModifiedStartDate,
          queryFilter?.lastModifiedEndDate,
          queryFilter?.principalId,
          queryFilter?.deleted,
          queryFilter?.orphan,
          queryFilter?.lastModifiedBy,
          queryFilter?.id,
          queryFilter?.description
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.sharedLookUpGetSharedLookUp());
    }

    return result;
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SHARED_LOOKUP",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
}

export async function GetTSRReportVerticalGrid(
  queryFilter?: TipologicheQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetTSRReportVerticalGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new PassThroughReportApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.tsrReportGetVerticalGrid(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.tsrReportGetVerticalGrid({}));
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_TSR_VERTICAL",
      payload: rtn as TipologicaGridDto,
    });
    setLoader("REMOVE", "GetTSRReportVerticalGrid");

    return rtn;
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_TSR_VERTICAL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetTSRReportVerticalGrid");
}
