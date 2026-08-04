import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { MainOrganisationApi } from "../../../../Business/LookUp/MainOrganisationBusiness";
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

export async function GetMainOrganisationGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetMainOrganisationGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new MainOrganisationApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.mainOrganisationGetMainOrganisation(
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
          queryFilter?.mainOrganisationId,
          queryFilter?.mainOrganisationDescription
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.mainOrganisationGetMainOrganisation());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_MAIN_ORGANISATION",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_MAIN_ORGANISATION",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetMainOrganisationGrid");
}

export async function GetMainOrganisationGridALL() {
  setLoader("ADD", "GetMainOrganisationGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new MainOrganisationApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.mainOrganisationGetMainOrganisation());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_MAIN_ORGANISATION_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_MAIN_ORGANISATION_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetMainOrganisationGridALL");
}

export async function GetFilterColumMainOrganisation(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumMainOrganisation");

  let result: FilterValueDto[] | undefined;
  let api = new MainOrganisationApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.mainOrganisationGetFilterResult(
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
        queryFilter?.mainOrganisationId,
        queryFilter?.mainOrganisationDescription
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.mainOrganisationGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_MAIN_ORGANISATION", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumMainOrganisation");
}
