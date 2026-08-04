import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ReportLcmExportApi } from "../../../Business/Report/ReportLcmExportBusiness";
import {
  ReportSubBoundHardwareGrid,
  ReportSubBoundSoftwareGrid,
  ReportHwSwQueryObjectGrid,
  QueryResultDtoOfReportSubBoundHardwareDtoGrid,
  QueryResultDtoOfReportSubBoundSoftwareDtoGrid,
  GET_GRID_REPORT_SUBBOUND_HARDWARE,
  GET_FILTER_REPORT_SUBBOUND_HARDWARE,
  GET_GRID_REPORT_SUBBOUND_SOFTWARE,
  GET_FILTER_REPORT_SUBBOUND_SOFTWARE,
  ReportNetworkLevel2Grid,
  QueryResultDtoOfReportNetworkLevel2DtoGrid,
  QueryResultDtoOfReportHardwareConfigDtoGrid,
  ReportHardwareConfigGrid,
  GET_GRID_REPORT_HARDWARE_CONFIG,
  GET_FILTER_REPORT_HARDWARE_CONFIG,
  ReportHardwareConfigQueryObjectGrid,
  GET_GRID_REPORT_NETWORKLEVEL2,
  GET_FILTER_REPORT_NETWORKLEVEL2,
} from "../../../Model/Report/ReportLcmExportModel";
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
import {
  ReportQueryDto,
  GetDcfNamesQueryDto,
} from "../../../Model/Report/LcmExportReport";
import { ResultDto } from "../../../Model/CommonModels";
import { ReportQueryAllDto } from "../../../Model/Report/Export";

export async function GetDCFNames(queryFilter?: GetDcfNamesQueryDto) {
  setLoader("ADD", "GetDCFNames");

  let result: ResultDto | null | undefined;
  let api = new ReportLcmExportApi();
  try {
    result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.getDcfNames(queryFilter ?? {})
    );
    setLoader("REMOVE", "GetDCFNames");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetDCFNames");
}
export async function GetSupportedService() {
  setLoader("ADD", "GetSupportedService");

  let result: ResultDto | null | undefined;
  let api = new ReportLcmExportApi();
  try {
    result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.GetSupportedService()
    );
    setLoader("REMOVE", "GetSupportedService");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSupportedService");
}
export async function GetReportNetworkLevel2Grid(
  queryFilter?: ReportQueryAllDto
) {
  setLoader("ADD", "GetReportNetworkLevel2Grid");

  let result: QueryResultDtoOfReportNetworkLevel2DtoGrid | null | undefined;
  let api = new ReportLcmExportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfReportNetworkLevel2DtoGrid>
    >(() => api.reportNetworkLevel2GetReport(queryFilter ?? {}));

    let rtn = {
      ReportNetworkLevel2GridResult: result,
      filter: null,
    } as ReportNetworkLevel2Grid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_NETWORKLEVEL2,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      ReportNetworkLevel2GridResult: null,
      filter: null,
    } as ReportNetworkLevel2Grid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_NETWORKLEVEL2,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetReportNetworkLevel2Grid");
}

export async function GetFilterColumReportNetworkLevel2(
  columName: string,
  columValue: string,
  queryFilter?: ReportQueryDto
) {
  let api = new ReportLcmExportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.reportNetworkLevel2GetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    ReportNetworkLevel2GridResult: null,
  } as ReportNetworkLevel2Grid;
  rootStore.dispatch({
    type: GET_FILTER_REPORT_NETWORKLEVEL2,
    payload: rtn,
  });
}

export async function GetReportHardwareConfigGrid(
  queryFilter?: ReportQueryAllDto
) {
  setLoader("ADD", "GetReportHardwareConfigGrid");

  let result: QueryResultDtoOfReportHardwareConfigDtoGrid | null | undefined;
  let api = new ReportLcmExportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfReportHardwareConfigDtoGrid>
    >(() => api.reportHardwareConfigGetReport(queryFilter ?? {}));

    let rtn = {
      ReportHardwareConfigGridResult: result,
      filter: null,
    } as ReportHardwareConfigGrid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_HARDWARE_CONFIG,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      ReportHardwareConfigGridResult: null,
      filter: null,
    } as ReportHardwareConfigGrid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_HARDWARE_CONFIG,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetReportHardwareConfigGrid");
}

export async function GetFilterColumReportHardwareConfig(
  columName: string,
  columValue: string,
  queryFilter?: ReportHardwareConfigQueryObjectGrid
) {
  let api = new ReportLcmExportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.reportHardwareConfigGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    ReportHardwareConfigGridResult: null,
  } as ReportHardwareConfigGrid;
  rootStore.dispatch({
    type: GET_FILTER_REPORT_HARDWARE_CONFIG,
    payload: rtn,
  });
}

export async function GetReportSubBoundHardwareGrid(
  queryFilter?: ReportHwSwQueryObjectGrid
) {
  setLoader("ADD", "GetReportSubBoundHardwareGrid");

  let result: QueryResultDtoOfReportSubBoundHardwareDtoGrid | null | undefined;
  let api = new ReportLcmExportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfReportSubBoundHardwareDtoGrid>
    >(() => api.reportSubBoundHardwareGetReport(queryFilter ?? {}));

    let rtn = {
      ReportSubBoundHardwareGridResult: result,
      filter: null,
    } as ReportSubBoundHardwareGrid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_SUBBOUND_HARDWARE,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      ReportSubBoundHardwareGridResult: null,
      filter: null,
    } as ReportSubBoundHardwareGrid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_SUBBOUND_HARDWARE,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetReportSubBoundHardwareGrid");
}

export async function GetFilterColumReportSubBoundHardware(
  columName: string,
  columValue: string,
  queryFilter?: ReportHwSwQueryObjectGrid
) {
  let api = new ReportLcmExportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.reportSubBoundHardwareGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    ReportSubBoundHardwareGridResult: null,
  } as ReportSubBoundHardwareGrid;
  rootStore.dispatch({
    type: GET_FILTER_REPORT_SUBBOUND_HARDWARE,
    payload: rtn,
  });
}

export async function GetReportSubBoundSoftwareGrid(
  queryFilter?: ReportHwSwQueryObjectGrid
) {
  setLoader("ADD", "GetReportSubBoundSoftwareGrid");

  let result: QueryResultDtoOfReportSubBoundSoftwareDtoGrid | null | undefined;
  let api = new ReportLcmExportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfReportSubBoundSoftwareDtoGrid>
    >(() => api.reportSubBoundSoftwareGetReport(queryFilter ?? {}));

    let rtn = {
      ReportSubBoundSoftwareGridResult: result,
      filter: null,
    } as ReportSubBoundSoftwareGrid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_SUBBOUND_SOFTWARE,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      ReportSubBoundSoftwareGridResult: null,
      filter: null,
    } as ReportSubBoundSoftwareGrid;
    rootStore.dispatch({
      type: GET_GRID_REPORT_SUBBOUND_SOFTWARE,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetReportSubBoundSoftwareGrid");
}

export async function GetFilterColumReportSubBoundSoftware(
  columName: string,
  columValue: string,
  queryFilter?: ReportHwSwQueryObjectGrid
) {
  let api = new ReportLcmExportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.reportSubBoundSoftwareGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    ReportSubBoundSoftwareGridResult: null,
  } as ReportSubBoundSoftwareGrid;
  rootStore.dispatch({
    type: GET_FILTER_REPORT_SUBBOUND_SOFTWARE,
    payload: rtn,
  });
}
