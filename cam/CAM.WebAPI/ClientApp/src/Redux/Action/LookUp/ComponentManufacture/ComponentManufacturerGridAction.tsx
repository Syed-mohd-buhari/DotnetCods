import {
    ApiCallWithErrorHandling,
    FilterValueDto,
  } from "../../../../Business/Common/CommonBusiness";
  import { ComponentManufacturerApi } from "../../../../Business/LookUp/ComponentManufactureBuisness";
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
  
  export async function GetCompoentManufacturerGrid(
    queryFilter?: TipologicheQueryObjectGrid
  ) {
    setLoader("ADD", "GetOriginalEquipmentManufacturerGrid");
    const api = new ComponentManufacturerApi();
    let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  
    try {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.componentManufacturerGetOriginalEquipmentManufacturer(
          queryFilter ?? {}
        )
      );
  
      const rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
      rootStore.dispatch({
        type: "GET_GRID_COMPONENT_MANUFACTURER",
        payload: rtn,
      });
    } catch (error) {
      rootStore.dispatch({
        type: "GET_GRID_COMPONENT_MANUFACTURER",
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
  
  export async function GetComponentManufacturerGridALL() {
    setLoader("ADD", "GetOriginalEquipmentManufacturerGridALL");
    const api = new ComponentManufacturerApi();
    let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  
    try {
      const body: TipologicheQueryObjectGrid = {
        sortBy: "",
        isSortAscending: false,
        page: 1,
        pageSize: 10,
        componentManufacturerId: [],
        componentManufacturer: [],
        componentName: [],
        lastModifiedBy: [],
      };
  
      result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(
        () => api.componentManufacturerGetOriginalEquipmentManufacturer(body)
      );
  
      const rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
      rootStore.dispatch({
        type: "GET_GRID_COMPONENT_MANUFACTURER_ALL",
        payload: rtn,
      });
    } catch (error) {
      rootStore.dispatch({
        type: "GET_GRID_COMPONENT_MANUFACTURER_ALL",
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
  
  
  export async function GetFilterColumComponentManufacturer(
    columnName: string,
    columnValue: string,
    queryFilter?: TipologicheQueryObjectGrid
  ) {
    // setLoader("ADD", "GetFilterColumOriginalEquipmentManufacturer");
    const api = new ComponentManufacturerApi();
    let result: FilterValueDto[] | undefined;
  
    // Safely handle optional queryFilter
    const f = queryFilter ?? {};
  
    const body = {
      sortBy: f.sortBy ?? "",
      isSortAscending: f.isSortAscending ?? true,
      page: f.page ?? 1,
      pageSize: f.pageSize ?? 10,
      lastModified: f.lastModifiedStartDate,
      principalId: f.principalId ?? 0,
      deleted: f.deleted ?? false,
      orphan: f.orphan ?? false,
      lastModifiedBy: f.lastModifiedBy ?? [],
      lastModifiedValue: {
        startDate: f.lastModifiedValue?.startDate ?? f.lastModifiedStartDate,
        endDate: f.lastModifiedValue?.endDate ?? f.lastModifiedEndDate,
      },
      componentManufacturerId: f.componentManufacturerId ?? [],
      componentManufacturer: f.componentManufacturer ?? [],
      componentName: f.componentName ?? [],
    };
    try {
      result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
        api.componentManufacturerGetFilterResult(
          body,
          columnName,
          columnValue
        )
      );
    } catch (error) {
      // Optionally handle error
    }
  
    const rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
    rootStore.dispatch({
      type: "GET_FILTER_COMPONENT_MANUFACTURER",
      payload: rtn,
    });
  
    // setLoader("REMOVE", "GetFilterColumOriginalEquipmentManufacturer");
  }
  