import { ProductLifecycleConstraintsApi } from "../../../Business/ProductLifecycleConstraintBusiness";
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
  LifecycleConstraintDto,
  LifecycleConstraintQueryDto,
} from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function SaveProductLifecycleConstraint(
  data: LifecycleConstraintDto
) {
  let api = new ProductLifecycleConstraintsApi();
  setLoader("ADD", "SaveProductLifecycleConstraint");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.productLifecycleConstraintsSaveLifecycleConstraint(data)
  );
  if (result && !result.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "SaveProductLifecycleConstraint");
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "SaveProductLifecycleConstraint");
  }
}

export async function GetProductLifecycleConstraintInfo(
  data: LifecycleConstraintDto
) {
  let ms = data.majorSoftwareBuildDto;
  let mh = data.majorHardwareBuildDto;
  let dataCast = {
    msLastTimeBuyNew: ms?.lastTimeBuyNew,
    msLastTimeBuyUpgrades: ms?.lastTimeBuyUpgrades,
    msLastTimeBuyExpansions: ms?.lastTimeBuyExpansions,
    msEndOfMaintenance: ms?.endOfMaintenance,
    msEndOfsupport: ms?.endOfsupport,
    mhLastTimeBuyNew: mh?.lastTimeBuyNew,
    mhLastTimeBuyUpgrades: mh?.lastTimeBuyUpgrades,
    mhLastTimeBuyExpansions: mh?.lastTimeBuyExpansions,
    mhEndOfMaintenance: mh?.endOfMaintenance,
    mhEndOfsupport: mh?.endOfsupport,
  } as LifecycleConstraintQueryDto;
  let api = new ProductLifecycleConstraintsApi();
  setLoader("ADD", "GetProductLifecycleConstraintInfo");
  let result = await ApiCallWithErrorHandling<Promise<ConstraintInfoDto>>(() =>
    api.productLifecycleConstraintsGetLifecycleCostraintInfo(dataCast)
  );

  setLoader("REMOVE", "GetProductLifecycleConstraintInfo");
  return result as ConstraintInfoDto;
}
