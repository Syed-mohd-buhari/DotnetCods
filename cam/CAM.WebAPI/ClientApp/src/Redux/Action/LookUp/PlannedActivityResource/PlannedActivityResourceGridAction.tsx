import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityResourceApi } from "../../../../Business/LookUp/PlannedActivityResourceBusiness";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import {
  PlannedActivityResourceQueryObjectGrid,
  QueryResultDtoOfPlannedActivityResourceDtoGrid,
} from "../../../../Model/LookUp/PlannedActivityResource";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPlannedActivityResourceGrid(
  queryFilter?: PlannedActivityResourceQueryObjectGrid
) {
  setLoader("ADD", "GetPlannedActivityResourceGrid");

  let result: QueryResultDtoOfPlannedActivityResourceDtoGrid | null | undefined;
  let api = new PlannedActivityResourceApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfPlannedActivityResourceDtoGrid>
      >(() =>
        api.plannedActivityResourceGetPlannedActivityResource(
          queryFilter.plannedActivityResourceId,
          queryFilter.plannedActivityResourceDescription,
          queryFilter.jsonFormResource,
          queryFilter.exportable,
          queryFilter.designAspectExportable,
          queryFilter.plannedDesignComponentRequiredAddAsset,
          queryFilter.plannedDesignComponentRequiredEditAsset,
          queryFilter.forLcm,
          queryFilter.forDesignAspect,
          queryFilter.designAspectHardware,
          queryFilter.designAspectSoftware,
          queryFilter.activityDetailsDesignAspect,
          queryFilter.ruleDesignAspect,
          queryFilter.designAspectLabelSoftware,
          queryFilter.designAspectLabelHardware,
          queryFilter.forAddAsset,
          queryFilter.forEditAsset,
          queryFilter.lcmHardware,
          queryFilter.lcmSoftware,
          queryFilter.addAssetHardware,
          queryFilter.addAssetSoftware,
          queryFilter.editAssetHardware,
          queryFilter.editAssetSoftware,
          queryFilter.ruleActicvityDetails,
          queryFilter.ruleLinkedDc,
          queryFilter.ruleLinkedDcPlannedActivityTypeDescription,
          queryFilter.lcmLabelSoftware,
          queryFilter.lcmLabelHardware,
          queryFilter.addAssetLabelSoftware,
          queryFilter.addAssetLabelHardware,
          queryFilter.editAssetLabelSoftware,
          queryFilter.editAssetLabelHardware,
          queryFilter.activityDetailsAddAsset,
          queryFilter.activityDetailsEditAsset,
          queryFilter.activityDetailsLcm,
          queryFilter.activityDetailsForVirtualizedAddAsset,
          queryFilter.ruleAddAsset,
          queryFilter.ruleActicvityDetailsAddAsset,
          queryFilter.driverTextAddAsset,
          queryFilter.benefitTextAddAsset,
          queryFilter.planningRisksAddAsset,
          queryFilter.activityDetailsForVirtualizedEditAsset,
          queryFilter.ruleEditAsset,
          queryFilter.ruleActicvityDetailsEditAsset,
          queryFilter.driverTextEditAsset,
          queryFilter.benefitTextEditAsset,
          queryFilter.planningRisksAEditAsset,
          queryFilter.driverTextLcm,
          queryFilter.benefitTextLcm,
          queryFilter.planningRisksLcm,
          queryFilter.driverTextDesignAspect,
          queryFilter.benefitTextDesignAspect,
          queryFilter.planningRisksDesignAspect,
          queryFilter.onBareMetalAddAsset,
          queryFilter.onVirtualizedAddAsset,
          queryFilter.forCreateAddAsset,
          queryFilter.forEditAddAsset,
          queryFilter.onBareMetalEditAsset,
          queryFilter.onVirtualizedEditAsset,
          queryFilter.forCreateEditAsset,
          queryFilter.forEditEditAsset,
          queryFilter.sortBy,
          queryFilter.isSortAscending,
          queryFilter.page,
          queryFilter.pageSize,
          queryFilter.lastModifiedStartDate,
          queryFilter.lastModifiedEndDate,
          queryFilter.principalId,
          queryFilter.deleted,
          queryFilter.orphan,
          queryFilter.lastModifiedBy
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfPlannedActivityResourceDtoGrid>
      >(() => api.plannedActivityResourceGetPlannedActivityResource());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_PLANNED_ACTIVITY_RESOURCE",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_PLANNED_ACTIVITY_RESOURCE",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPlannedActivityResourceGrid");
}

export async function GetPlannedActivityResourceGridALL() {
  setLoader("ADD", "GetPlannedActivityResourceGridALL");

  let result: QueryResultDtoOfPlannedActivityResourceDtoGrid | null | undefined;
  let api = new PlannedActivityResourceApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPlannedActivityResourceDtoGrid>
    >(() => api.plannedActivityResourceGetPlannedActivityResource());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_PLANNED_ACTIVITY_RESOURCE_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_PLANNED_ACTIVITY_RESOURCE_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPlannedActivityResourceGridALL");
}

export async function GetFilterColumPlannedActivityResource(
  columName: string,
  columValue: string,
  queryFilter?: PlannedActivityResourceQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumPlannedActivityResource");
  let result: FilterValueDto[] | undefined;
  let api = new PlannedActivityResourceApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.plannedActivityResourceGetFilterResult(
        columName,
        columValue,
        queryFilter.plannedActivityResourceId,
        queryFilter.plannedActivityResourceDescription,
        queryFilter.jsonFormResource,
        queryFilter.exportable,
        queryFilter.designAspectExportable,
        queryFilter.plannedDesignComponentRequiredAddAsset,
        queryFilter.plannedDesignComponentRequiredEditAsset,
        queryFilter.forLcm,
        queryFilter.forDesignAspect,
        queryFilter.designAspectHardware,
        queryFilter.designAspectSoftware,
        queryFilter.activityDetailsDesignAspect,
        queryFilter.ruleDesignAspect,
        queryFilter.designAspectLabelSoftware,
        queryFilter.designAspectLabelHardware,
        queryFilter.forAddAsset,
        queryFilter.forEditAsset,
        queryFilter.lcmHardware,
        queryFilter.lcmSoftware,
        queryFilter.addAssetHardware,
        queryFilter.addAssetSoftware,
        queryFilter.editAssetHardware,
        queryFilter.editAssetSoftware,
        queryFilter.ruleActicvityDetails,
        queryFilter.ruleLinkedDc,
        queryFilter.ruleLinkedDcPlannedActivityTypeDescription,
        queryFilter.lcmLabelSoftware,
        queryFilter.lcmLabelHardware,
        queryFilter.addAssetLabelSoftware,
        queryFilter.addAssetLabelHardware,
        queryFilter.editAssetLabelSoftware,
        queryFilter.editAssetLabelHardware,
        queryFilter.activityDetailsAddAsset,
        queryFilter.activityDetailsEditAsset,
        queryFilter.activityDetailsLcm,
        queryFilter.activityDetailsForVirtualizedAddAsset,
        queryFilter.ruleAddAsset,
        queryFilter.ruleActicvityDetailsAddAsset,
        queryFilter.driverTextAddAsset,
        queryFilter.benefitTextAddAsset,
        queryFilter.planningRisksAddAsset,
        queryFilter.activityDetailsForVirtualizedEditAsset,
        queryFilter.ruleEditAsset,
        queryFilter.ruleActicvityDetailsEditAsset,
        queryFilter.driverTextEditAsset,
        queryFilter.benefitTextEditAsset,
        queryFilter.planningRisksAEditAsset,
        queryFilter.driverTextLcm,
        queryFilter.benefitTextLcm,
        queryFilter.planningRisksLcm,
        queryFilter.driverTextDesignAspect,
        queryFilter.benefitTextDesignAspect,
        queryFilter.planningRisksDesignAspect,
        queryFilter.onBareMetalAddAsset,
        queryFilter.onVirtualizedAddAsset,
        queryFilter.forCreateAddAsset,
        queryFilter.forEditAddAsset,
        queryFilter.onBareMetalEditAsset,
        queryFilter.onVirtualizedEditAsset,
        queryFilter.forCreateEditAsset,
        queryFilter.forEditEditAsset,
        queryFilter.sortBy,
        queryFilter.isSortAscending,
        queryFilter.page,
        queryFilter.pageSize,
        queryFilter.lastModifiedStartDate,
        queryFilter.lastModifiedEndDate,
        queryFilter.principalId,
        queryFilter.deleted,
        queryFilter.orphan,
        queryFilter.lastModifiedBy
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.plannedActivityResourceGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({
    type: "GET_FILTER_PLANNED_ACTIVITY_RESOURCE",
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumPlannedActivityResource");
}
