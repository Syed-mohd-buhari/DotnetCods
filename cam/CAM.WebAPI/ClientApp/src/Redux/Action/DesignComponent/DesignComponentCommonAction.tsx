import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentApi } from "../../../Business/DesignComponentBusiness";
import {
  DataRemediationDto,
  ResultDataRemediationDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../Model/CommonModels";
import {
  ImpactServiceBoundaryChanged,
  ResultDtoOfImpactServiceBoundaryChanged,
  ResultDtoOfListOfSystemAndServiceBoundary,
  ResultDtoOfServiceBoundaryChanged,
} from "../../../Model/DesignComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { ResultDtoOfBoolean } from "../../../Model/Common";
import { UnUsedSubnetworkBoundary } from "./../../../Model/DesignComponent";

export async function ApplyDataRimediationDesignComponent(
  data: DataRemediationDto
) {
  let api = new DesignComponentApi();
  setLoader("ADD", "ApplyDataRimediationDesignComponent");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfResultDataRemediationDto>
  >(() => api.designComponentApplyDataRemediation(data));
  if (result && !result.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "ApplyDataRimediationDesignComponent");
    return result.data as ResultDataRemediationDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "ApplyDataRimediationDesignComponent");
}

export async function GetSystemTypeAndServiceBoundary(data: number) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetSystemTypeAndServiceBoundary");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfServiceBoundaryChanged>
  >(() => api.designComponentGetSystemType(data));
  if (result && !result.warning) {
    setLoader("REMOVE", "GetSystemTypeAndServiceBoundary");
    return result.data!;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetSystemTypeAndServiceBoundary");
}

export async function GetSubnetworkBoundaries(data: number) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetSubnetworkBoundary");
  let result = await ApiCallWithErrorHandling<
    Promise<UnUsedSubnetworkBoundary[]>
  >(() => api.designComponentGetSubnetworkBoundaries(data));
  if (result) {
    setLoader("REMOVE", "GetSubnetworkBoundary");
    return result;
  } else {
    // rootStore.dispatch(
    //   setNotification({
    //     message: "Warning !",
    //     notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    //   })
    // );
  }
  setLoader("REMOVE", "GetSubnetworkBoundary");
}

export async function GetSystemTypeAndServiceBoundaryDestructured(
  msOem?: number,
  msst?: number,
  mhOem?: number
) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetSystemTypeAndServiceBoundaryDestructured");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfServiceBoundaryChanged>
  >(() => api.designComponentGetSystemType2(msOem, msst, mhOem));
  if (result && !result.warning) {
    setLoader("REMOVE", "GetSystemTypeAndServiceBoundaryDestructured");
    return result.data!;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetSystemTypeAndServiceBoundaryDestructured");
}

export async function GetCheckDcfExist(
  systemTypeId?: number,
  supportedAllService?: boolean,
  serviceBoundaryId?: number[]
) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetCheckDcfExist");
  let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfBoolean>>(() =>
    api.designComponentGetDesignComponentFamilyExist(
      systemTypeId,
      supportedAllService,
      serviceBoundaryId ?? []
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetCheckDcfExist");
    return result.data as boolean;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetCheckDcfExist");
}
export async function GetCheckDcfExistDestructured(
  hwOemId: number,
  swAppType: string,
  swOemId: number,
  serviceBoundaryId: number[],
  platformId: number,
  supportedAllService?: boolean
) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetCheckDcfExistDestructured");
  let result = await ApiCallWithErrorHandling<Promise<ResultDtoOfBoolean>>(() =>
    api.designComponentGetDesignComponentFamilyExistDestructured(
      hwOemId,
      swAppType,
      swOemId,
      serviceBoundaryId,
      platformId,
      supportedAllService
    )
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "GetCheckDcfExistDestructured");
    return result.data as boolean;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetCheckDcfExistDestructured");
}

export async function GetImpactChangeDesignComponent(
  serviceBoundaryId: number[],
  systemTypeId?: number
) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetImpactChangeDesignComponent");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfImpactServiceBoundaryChanged>
  >(() => api.designComponentGetImpact(serviceBoundaryId, systemTypeId));
  if (result && !result.warning) {
    setLoader("REMOVE", "GetImpactChangeDesignComponent");
    return result.data as ImpactServiceBoundaryChanged;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetImpactChangeDesignComponent");
}

export async function GetImpactedAreasOfEditDc(
  designComponent?: number,
  systemType?: number,
  serviceBoundaryId?: number[]
) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetImpactedAreasOfEditDc");
  let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.impactedAreasOfEditDc(
      designComponent,
      systemType,
      serviceBoundaryId ?? undefined
    )
  );
  if (result && result.warning) {
    setLoader("REMOVE", "GetImpactedAreasOfEditDc");
    return result.data;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "GetImpactedAreasOfEditDc");
}

export async function GetSubNetworkBoundariesDestructured(data: number) {
  let api = new DesignComponentApi();
  setLoader("ADD", "GetSubNetworkBoundariesDestructured");
  let result = await ApiCallWithErrorHandling<
    Promise<UnUsedSubnetworkBoundary[]>
  >(() => api.designComponentGetSubNetworkBoundariesDestructured(data));
  if (result) {
    setLoader("REMOVE", "GetSubNetworkBoundariesDestructured");
    return result;
  } else {
    // rootStore.dispatch(
    //   setNotification({
    //     message: result?.info ?? "",
    //     notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    //   })
    // );
  }
  setLoader("REMOVE", "GetSubNetworkBoundariesDestructured");
}
export async function IsDCHasNfxiBuildConstruction(designComponentId: number) {
  setLoader("ADD", "IsDCHasNfxiBuildConstruction");

  let api = new DesignComponentApi();

  let data = await ApiCallWithErrorHandling<Promise<ResultDtoOfBoolean>>(() =>
    api.IsDCHasNfxiBuildConstruction(designComponentId)
  );
  setLoader("REMOVE", "IsDCHasNfxiBuildConstruction");

  return data
}