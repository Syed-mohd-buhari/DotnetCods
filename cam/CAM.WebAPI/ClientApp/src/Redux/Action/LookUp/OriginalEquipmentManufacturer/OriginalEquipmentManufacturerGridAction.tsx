import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { OriginalEquipmentManufacturerApi } from "../../../../Business/LookUp/OriginalEquipmentManufacturerBusiness";
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

export async function GetOriginalEquipmentManufacturerGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetOriginalEquipmentManufacturerGrid");

  let api = new OriginalEquipmentManufacturerApi();
  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.originalEquipmentManufacturerGetOriginalEquipmentManufacturer(
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
          queryFilter?.description,
          queryFilter?.isPlatformSoftware
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.originalEquipmentManufacturerGetOriginalEquipmentManufacturer()
      );
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_ORIGINAL_EQUIPMENT_MANUFACTURER",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_ORIGINAL_EQUIPMENT_MANUFACTURER",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetOriginalEquipmentManufacturerGrid");
}

export async function GetOriginalEquipmentManufacturerGridALL() {
  setLoader("ADD", "GetOriginalEquipmentManufacturerGridALL");

  let api = new OriginalEquipmentManufacturerApi();
  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() =>
      api.originalEquipmentManufacturerGetOriginalEquipmentManufacturer()
    );
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_ORIGINAL_EQUIPMENT_MANUFACTURER_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_ORIGINAL_EQUIPMENT_MANUFACTURER_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetOriginalEquipmentManufacturerGridALL");
}

export async function GetFilterColumOriginalEquipmentManufacturer(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumOriginalEquipmentManufacturer");

  let result: FilterValueDto[] | undefined;
  let api = new OriginalEquipmentManufacturerApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.originalEquipmentManufacturerGetFilterResult(
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
        queryFilter?.description,
        queryFilter?.isPlatformSoftware
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.originalEquipmentManufacturerGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({
    type: "GET_FILTER_ORIGINAL_EQUIPMENT_MANUFACTURER",
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumOriginalEquipmentManufacturer");
}
