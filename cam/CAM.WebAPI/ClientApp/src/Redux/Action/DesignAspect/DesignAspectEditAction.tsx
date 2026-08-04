import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../../Business/DesignAspectsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_DESIGN_ASPECT,
  GET_EDIT_DESIGN_ASPECT,
  DesignAspectDtoUpdate,
  DesignAspectEdit,
} from "../../../Model/DesignAspects";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDesignAspectEditResource(id: number) {
  setLoader("ADD", "GetDesignAspectEditResource");
  let api = new DesignAspectApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<DesignAspectDtoUpdate>
  >(() => api.designAspectGetUpdateResourceDesignAspect(id));
  let rtn = { DesignAspectDtoEdit: createResource } as DesignAspectEdit;
  rootStore.dispatch({ type: GET_EDIT_DESIGN_ASPECT, payload: rtn });
  setLoader("REMOVE", "GetDesignAspectEditResource");
}

export async function EditDesignAspect(
  data: DesignAspectDtoUpdate,
  forced?: boolean
) {
  let api = new DesignAspectApi();
  setLoader("ADD", "EditDesignAspect");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designAspectPut(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as DesignAspectEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_DESIGN_ASPECT, payload: rtn });
  setLoader("REMOVE", "EditDesignAspect");
  return rtn;
}

export async function GetUpdateDAMigration(
  paId: number,
  dcId: number,
  targetDcfId: number
) {
  setLoader("ADD", "GetUpdateDAMigration");
  const api = new DesignAspectApi();

  try {
    
    const result = await ApiCallWithErrorHandling<Promise<string>>(() =>
      api.getUpdateDAMigration(paId, dcId, targetDcfId)
    );

    
    const parsed =
      typeof result === "string" ? JSON.parse(result) : result;

    const payload = {
      updateDaMigrationData: parsed ?? null,
    };

    rootStore.dispatch(
      setNotification({
        message: "DA Migration data fetched successfully",
        notifyType: NotifyType.success,
      })
    );

    return payload;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Failed to fetch Update DA Migration records",
        notifyType: NotifyType.error,
      })
    );
    return { updateDaMigrationData: null };
  } finally {
    setLoader("REMOVE", "GetUpdateDAMigration");
  }
}
