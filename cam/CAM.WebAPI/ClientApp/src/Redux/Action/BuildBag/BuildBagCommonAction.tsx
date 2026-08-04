import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BuildBagApi } from "../../../Business/BuildBagsBusiness";
import {
  DataRemediationDto,
  ResultDto,
  ResultDataRemediationDto,
  ResultDtoOfResultDataRemediationDto,
} from "../../../Model/CommonModels";
import {
  BuildBagToCloneDto,
  ResultDtoOfBuildBagToCloneDto,
  CloneBuildBagDto,
  BuildBagDtoCreate,
  BuildBagDtoUpdate,
} from "../../../Model/BuildBag";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetSoftwareComponentToClone(data: number) {
  let api = new BuildBagApi();
  setLoader("ADD", "GetSoftwareComponentToClone");
  let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.buildBagGetSoftwareComponentToClone(data)
  );
  if (result && !result.warning) {
    // rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
    setLoader("REMOVE", "GetSoftwareComponentToClone");
    return result as any;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetSoftwareComponentToClone");
  }
}

export async function GetViewBagAndComponent(id: number) {
  let api = new BuildBagApi();
  setLoader("ADD", "GetViewBagAndComponent");
  let result = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.getViewBagAndComponent(id)
  );
  if (result && !result.warning) {
    // rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
    setLoader("REMOVE", "GetViewBagAndComponent");
    return result as any;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "GetViewBagAndComponent");
  }
}

export async function SoftwareComponentUpgrade(data: BuildBagDtoUpdate) {
  let api = new BuildBagApi();
  setLoader("ADD", "SoftwareComponentUpgrade");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.softwareComponentUpgrade(data)
  );
  if (result && !result.warning) {
    setLoader("REMOVE", "SoftwareComponentUpgrade");
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    return result as ResultDto;
  } else {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
    setLoader("REMOVE", "SoftwareComponentUpgrade");
  }
}
