import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { MajorHardwareBuildApi } from "../../../Business/MajorHardwareBuildBusiness";
import {
  DataRemediationDto,
  ResultDto,
  ResultDataRemediationDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../Model/CommonModels";
import { IdentityAsIsApi } from "../../../Business/IdentityAsIs";

import {
  CREATE_MAJOR_HARDWARE_BUILD,
  GET_CREATE_MAJOR_HARDWARE_BUILD,
  MajorHardwareBuildCreate,
  MajorHardwareBuildDtoCreate,
} from "../../../Model/MajorHardwareBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function ApplyDataRimediationMajorHardware(
  data: DataRemediationDto
) {
  let api = new MajorHardwareBuildApi();
  setLoader("ADD", "ApplyDataRimediationMajorHardware");
  let result = await ApiCallWithErrorHandling<
    Promise<ResultDtoOfResultDataRemediationDto>
  >(() => api.majorHardwareBuildApplyDataRemediation(data));
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
  setLoader("REMOVE", "ApplyDataRimediationMajorHardware");
}
export async function GetAssetsByOpcoIdAndDcfId(opcoId: number,dcfId:number) {
  setLoader("ADD", "GetAssetsByOpcoIdAndDcfId");

  let api = new IdentityAsIsApi();

  let data = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetAssetsByOpcoIdAndDcfId(opcoId,dcfId)
  );
  setLoader("REMOVE", "GetAssetsByOpcoIdAndDcfId");

  return data
}