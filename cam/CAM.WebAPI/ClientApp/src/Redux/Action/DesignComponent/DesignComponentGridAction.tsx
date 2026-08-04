import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { DesignComponentApi } from "../../../Business/DesignComponentBusiness";
import {
  DesignComponentGrid,
  DesignComponentQueryObjectGrid,
  GET_FILTER_DESIGN_COMPONENT,
  GET_GRID_DESIGN_COMPONENT,
  QueryResultDtoOfDesignComponentDtoGrid,
} from "../../../Model/DesignComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDesignComponentGrid(
  queryFilter?: DesignComponentQueryObjectGrid
) {
  let result: QueryResultDtoOfDesignComponentDtoGrid | null | undefined;
  let api = new DesignComponentApi();
  setLoader("ADD", "GetDesignComponentGrid");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfDesignComponentDtoGrid>
    >(() => api.designComponentGetDesignComponent(queryFilter || {}));
    let rtn = {
      DesignComponentGridResult: result,
      filter: null,
    } as DesignComponentGrid;
    rootStore.dispatch({
      type: GET_GRID_DESIGN_COMPONENT,
      payload: rtn as DesignComponentGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: GET_GRID_DESIGN_COMPONENT,
      payload: {
        DesignComponentGridResult: result,
        filter: null,
      } as DesignComponentGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetDesignComponentGrid");
}

export async function GetFilterColumDesignComponent(
  columName: string,
  columValue: string,
  queryFilter?: DesignComponentQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new DesignComponentApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.designComponentGetFilterResult(queryFilter || {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    DesignComponentGridResult: null,
  } as DesignComponentGrid;
  rootStore.dispatch({ type: GET_FILTER_DESIGN_COMPONENT, payload: rtn });
}
