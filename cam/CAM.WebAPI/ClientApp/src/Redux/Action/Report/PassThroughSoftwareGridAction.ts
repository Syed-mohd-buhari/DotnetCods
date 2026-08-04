import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { PassThroughSoftwareReportApi } from "../../../Business/Report/PassThroughSoftwareBusiness";
import { PassThroughReportApi } from "../../../Business/Report/PassThroughReportBusiness";

import {
  PassThroughSoftwareReportDtoGrid,
  PassThroughSoftwareReportGrid,
  PassThroughSoftwareReportQueryObjectGrid,
  QueryResultDtoOfPassThroughSoftwareReportDtoGrid,
  GET_GRID_PASSTHROUGH_SOFTWARE_REPORT,
  GET_FILTER_PASSTHROUGH_SOFTWARE_REPORT,
} from "../../../Model/Report/PassThroughSoftwareReportExport";
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

export async function GetPassThroughSoftwareReportGrid(
  queryFilter?: PassThroughSoftwareReportQueryObjectGrid
) {
  setLoader("ADD", "GetPassThroughSoftwareReportGrid");

  let result:
    | QueryResultDtoOfPassThroughSoftwareReportDtoGrid
    | null
    | undefined;
  let api = new PassThroughSoftwareReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPassThroughSoftwareReportDtoGrid>
    >(() => api.PassThroughSoftwareReportGetReport(queryFilter ?? {}));

    let rtn = {
      PassThroughSoftwareReportGridResult: result,
      filter: null,
    } as PassThroughSoftwareReportGrid;
    rootStore.dispatch({
      type: GET_GRID_PASSTHROUGH_SOFTWARE_REPORT,
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
      PassThroughSoftwareReportGridResult: null,
      filter: null,
    } as PassThroughSoftwareReportGrid;
    rootStore.dispatch({
      type: GET_GRID_PASSTHROUGH_SOFTWARE_REPORT,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetPassThroughSoftwareReportGrid");
}

export async function GetFilterColumPassThroughSoftwareReport(
  columName: string,
  columValue: string,
  queryFilter?: PassThroughSoftwareReportQueryObjectGrid
) {
  let api = new PassThroughSoftwareReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.PassThroughSoftwareReportGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    PassThroughSoftwareReportGridResult: null,
  } as PassThroughSoftwareReportGrid;
  rootStore.dispatch({
    type: GET_FILTER_PASSTHROUGH_SOFTWARE_REPORT,
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
