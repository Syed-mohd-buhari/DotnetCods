import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { RiskClusterApi } from "../../../../Business/LookUp/RiskClusterBusiness";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  TipologicheQueryObjectGrid,
  LookUpGrid,
  LookUpCreate,
  LookUpEdit,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { ResultDto } from "../../../../Model/CommonModels";

export async function GetRiskClusterGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetRiskClusterGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new RiskClusterApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.RiskClusterGetRiskCluster(
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
          queryFilter?.riskClusterId
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.RiskClusterGetRiskCluster());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_RISK_CLUSTER",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_RISK_CLUSTER",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetRiskClusterGrid");
}

export async function GetRiskClusterGridALL() {
  setLoader("ADD", "GetRiskClusterGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new RiskClusterApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.RiskClusterGetRiskCluster());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_RISK_CLUSTER_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_RISK_CLUSTER_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetRiskClusterGridALL");
}

export async function GetFilterColumRiskCluster(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumRiskCluster");

  let result: FilterValueDto[] | undefined;
  let api = new RiskClusterApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.riskClusterGetFilterResult(
        columName,
        columValue,
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
        queryFilter?.riskClusterId
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.riskClusterGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_PROBLEM_CATEGORY", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumRiskCluster");
}

export async function GetRiskClusterCreateResource() {
  setLoader("ADD", "GetRiskClusterCreateResource");

  let api = new RiskClusterApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.riskClusterGetCreateResourceRiskCluster());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_RISK_CLUSTER", payload: rtn });
  setLoader("REMOVE", "GetRiskClusterCreateResource");
}

export async function CreateRiskCluster(data: TipologicaGridDto) {
  setLoader("ADD", "CreateRiskCluster");
  let api = new RiskClusterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.riskClusterCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_PROBLEM_CATEGORY", payload: rtn });
  setLoader("REMOVE", "CreateRiskCluster");
  return rtn;
}

export async function deleteRiskCluster(id: number) {
  setLoader("ADD", "deleteRiskCluster");
  let api = new RiskClusterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.riskClusterDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_RISK_CLUSTER", payload: rtn });
  setLoader("REMOVE", "deleteRiskCluster");
  return rtn;
}

export async function GetRiskClusterEditResource(id: number) {
  setLoader("ADD", "GetRiskClusterEditResource");

  let api = new RiskClusterApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.riskClusterGetUpdateResourceRiskCluster(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_RISK_CLUSTER", payload: rtn });
  setLoader("REMOVE", "GetRiskClusterEditResource");

  return rtn;
}

export async function EditRiskCluster(data: TipologicaGridDto) {
  setLoader("ADD", "EditRiskCluster");
  let api = new RiskClusterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.riskClusterPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_RISK_CLUSTER", payload: rtn });
  setLoader("REMOVE", "EditRiskCluster");
  return rtn;
}
