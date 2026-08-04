import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SystemTypeApi } from "../../../Business/SystemTypeBusiness";
import {
  DataRemediationDto,
  ResultDataRemediationDto,
  ResultDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../Model/CommonModels";
import {
  ConstraintInfoDto,
  SystemTypeReleatedMajorEntity,
  LifecycleConstraintDto,
} from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSystemSolutionName(
  nameOEM?: string,
  majorHardwareBuilds?: number,
  systemTypeBuilds?: number,
  majorHardwareList?: Array<number>
) {
  let api = new SystemTypeApi();
  let systemSolutionName = await ApiCallWithErrorHandling<Promise<ResultDto>>(
    () =>
      api.systemTypeGetSystemSolutionName(
        nameOEM,
        majorHardwareBuilds,
        systemTypeBuilds,
        majorHardwareList
      )
  );

  return systemSolutionName?.data;
}

export async function GetAllSubdomainAndVerticalResponsibles(
  majorHardwareBuildId?: number,
  majorSoftwareBuildId?: number
) {
  let api = new SystemTypeApi();
  let res = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemTypeGetAllSubdomainAndVerticalResponsibles(
      majorHardwareBuildId,
      majorSoftwareBuildId
    )
  );

  return res;
}

export async function GetCostraintInfo(
  systemTypeBuild?: number,
  majorHardwareBuilds?: Array<number>
) {
  setLoader("ADD", "GetCostraintInfo");

  let api = new SystemTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ConstraintInfoDto>>(() =>
    api.systemTypeGetCostraintInfo(systemTypeBuild, majorHardwareBuilds)
  );
  setLoader("REMOVE", "GetCostraintInfo");

  return result;
}

export async function GetVodafoneNameResource(majorSWId?: number) {
  let api = new SystemTypeApi();
  let result = await ApiCallWithErrorHandling<
    Promise<{ [key: string]: string }>
  >(() => api.systemTypeGetVodafoneNameResource(majorSWId));

  return result;
}

export async function GetVodafoneNameResourceWizardMode(majorSWId?: number) {
  setLoader("ADD", "GetVodafoneNameResource");

  let api = new SystemTypeApi();
  let result = await ApiCallWithErrorHandling<
    Promise<{ [key: string]: string }>
  >(() => api.systemTypeGetVodafoneNameResourceWizardMode(majorSWId));
  setLoader("REMOVE", "GetVodafoneNameResource");

  return result;
}

export async function GetAssetCategoryReleated(value: number) {
  setLoader("ADD", "GetAssetCategoryReleated");

  let api = new SystemTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemTypeGetAssetCategoryReleated(value)
  );
  setLoader("REMOVE", "GetAssetCategoryReleated");

  return result?.data as SystemTypeReleatedMajorEntity;
}

export async function GetMinorDateFromMajorEntity(
  systemTypeId?: number,
  majorHardwareIds?: Array<number>
) {
  setLoader("ADD", "GetMinorDateFromMajorEntity");

  let api = new SystemTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto | null>>(() =>
    api.systemTypeGetMinorDateFromMajorEntity(systemTypeId, majorHardwareIds)
  );
  setLoader("REMOVE", "GetMinorDateFromMajorEntity");
  // console.log("the result of GetMinorDateFromMajorEntity is",result);
  return result;
}

export async function ApplyDataRimediationSystemType(data: DataRemediationDto) {
  let api = new SystemTypeApi();
  setLoader("ADD", "ApplyDataRimediationSystemType");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfResultDataRemediationDto>
  >(() => api.systemTypeApplyDataRemediation(data));
  if (result && !result.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    return result.data as ResultDataRemediationDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  }
  setLoader("REMOVE", "ApplyDataRimediationSystemType");
}
