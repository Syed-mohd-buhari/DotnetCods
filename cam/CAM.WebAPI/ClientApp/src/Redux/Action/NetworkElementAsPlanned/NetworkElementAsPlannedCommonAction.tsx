import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsPlannedApi } from "../../../Business/NetworkElementAsPlannedBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NetworkElementAssociated } from "../../../Model/LcmEngineering";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDesignComponentFamilyFromOem(data: number) {
  setLoader("ADD", "GetDesignComponentFamilyFromOem");

  let api = new NetworkElementAsPlannedApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsPlannedGetDesignComponentList(data)
  );
  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetDesignComponentFamilyFromOem");

    return result?.data;
  }
  setLoader("REMOVE", "GetDesignComponentFamilyFromOem");
}

export async function GetNetworkElementAssociateds(
  designComponentId?: number | undefined,
  opcoId?: number | undefined
) {
  setLoader("ADD", "GetNetworkElementAssociateds");

  let api = new NetworkElementAsPlannedApi();
  let result = await ApiCallWithErrorHandling<
    Promise<NetworkElementAssociated[]>
  >(() =>
    api.networkElementAsPlannedGetNetworkElementassociated(
      designComponentId,
      opcoId
    )
  );
  if (result) {
    setLoader("REMOVE", "GetNetworkElementAssociateds");
    return result;
  }
  setLoader("REMOVE", "GetNetworkElementAssociateds");
}

export async function GetNetworkElementOpCo(
  body: {
    dcId?: number;
    opCoId?: number;
    lcmBagId?: number;
  } = {}
): Promise<ResultDto | null> {
  setLoader("ADD", "GetNetworkElementOpCo");
  try {
    let api = new NetworkElementAsPlannedApi();
    let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.networkElementAsPlannedPostOpCo(body)
    );
    return result ?? null;
  } finally {
    setLoader("REMOVE", "GetNetworkElementOpCo");
  }
}

export async function GetPlatformMigarteGetDesignComponentList(
  compareValue: string,
  dcfResource: Record<string, string>
) {
  setLoader("ADD", "GetDesignComponentFamilyFromDCID");

  const api = new NetworkElementAsPlannedApi();
  const result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsPlatformMigarteGetDesignComponentList(compareValue, dcfResource)
  );

  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetDesignComponentFamilyFromDCID");
     return (result as { key: number; value: string }[]) || [];;
  }

  setLoader("REMOVE", "GetDesignComponentFamilyFromDCID");
}




export async function GetAssetForPlatfromMigration(opcoId?: number,dcfId?: number,PaId?:number) {
  setLoader("ADD", "GetDesignComponentFamilyFromDCID");

  let api = new NetworkElementAsPlannedApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getAssetsForPlatfromMigration(opcoId,dcfId,PaId)
  );
  if (result?.warning) {
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: result?.warning ? NotifyType.error : NotifyType.success,
      })
    );
  } else {
    setLoader("REMOVE", "GetDesignComponentFamilyFromDCID");

    return result?.data;
  }
  setLoader("REMOVE", "GetDesignComponentFamilyFromDCID");
}
