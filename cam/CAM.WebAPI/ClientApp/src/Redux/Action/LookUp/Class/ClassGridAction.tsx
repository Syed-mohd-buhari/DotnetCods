import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { ClassApi } from "../../../../Business/LookUp/ClassBusiness";
import {
  ClassQueryObjectGrid,
  QueryResultDtoOfClassDtoGrid,
} from "../../../../Model/LookUp/Class";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetClassGrid(queryFilter?: ClassQueryObjectGrid) {
  setLoader("ADD", "GetClassGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new ClassApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfClassDtoGrid>
      >(() =>
        api.ClassGetClass(
          queryFilter?.id,
          queryFilter?.description,
          queryFilter?.categoryId,
          queryFilter?.sortBy,
          queryFilter?.isSortAscending,
          queryFilter?.page,
          queryFilter?.pageSize,
          queryFilter?.lastModifiedStartDate,
          queryFilter?.lastModifiedEndDate,
          queryFilter?.categoryDescription,
          queryFilter?.deleted,
          queryFilter?.orphan,
          queryFilter?.lastModifiedBy
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfClassDtoGrid>
      >(() => api.ClassGetClass());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_CLASS",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CLASS",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetClassGrid");
}

export async function GetClassGridALL() {
  setLoader("ADD", "GetClassGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new ClassApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfClassDtoGrid>
    >(() => api.ClassGetClass());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_CLASS_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CLASS_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetClassGridALL");
}

export async function GetFilterColumClass(
  columName: string,
  columValue: string,
  queryFilter?: ClassQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumClass");

  let result: FilterValueDto[] | undefined;
  let api = new ClassApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.ClassGetFilterResult(
        columName,
        columValue,
        queryFilter?.id,
        queryFilter?.description,
        queryFilter?.categoryId,
        queryFilter?.sortBy,
        queryFilter?.isSortAscending,
        queryFilter?.page,
        queryFilter?.pageSize,
        queryFilter?.lastModifiedStartDate,
        queryFilter?.lastModifiedEndDate,
        queryFilter?.principalId,
        queryFilter?.deleted,
        queryFilter?.orphan,
        queryFilter?.lastModifiedBy
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.ClassGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_CLASS", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumClass");
}
