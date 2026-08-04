import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { MajorHardwareBuildApi } from "../../../../Business/MajorHardwareBuildBusiness";
import {
  DataRemediationDto,
  ResultDto,
  ResultDataRemediationDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../../Model/CommonModels";
import {
  CREATE_MAJOR_HARDWARE_BUILD,
  GET_CREATE_MAJOR_HARDWARE_BUILD,
  MajorHardwareBuildCreate,
  MajorHardwareBuildDtoCreate,
} from "../../../../Model/MajorHardwareBuild";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

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
