import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { SubDomainResponsibleApi } from "../../../../Business/LookUp/SubDomainResponsibleBusiness";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  TipologicheQueryObjectGrid,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSubDomainResponsibleGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetSubDomainResponsibleGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SubDomainResponsibleApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.subDomainResponsibleGetSubDomainResponsible(
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
      >(() => api.subDomainResponsibleGetSubDomainResponsible());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_SUB_DOMAIN_RESPONSIBLE",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUB_DOMAIN_RESPONSIBLE",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSubDomainResponsibleGrid");
}

export async function GetSubDomainResponsibleGridALL() {
  setLoader("ADD", "GetSubDomainResponsibleGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SubDomainResponsibleApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.subDomainResponsibleGetSubDomainResponsible());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_SUB_DOMAIN_RESPONSIBLE_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUB_DOMAIN_RESPONSIBLE_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSubDomainResponsibleGridALL");
}

export async function GetFilterColumSubDomainResponsible(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumSubDomainResponsible");

  let result: FilterValueDto[] | undefined;
  let api = new SubDomainResponsibleApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.subDomainResponsibleGetFilterResult(
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
        queryFilter?.id,
        queryFilter?.description
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.subDomainResponsibleGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({
    type: "GET_FILTER_SUB_DOMAIN_RESPONSIBLE",
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumSubDomainResponsible");
}
