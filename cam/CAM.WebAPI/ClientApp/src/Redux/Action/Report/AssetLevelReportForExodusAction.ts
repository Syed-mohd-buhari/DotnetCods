import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { AssetLevelReportForExodusApi } from "../../../Business/Report/ExodusGraphicLevelBuisness";
import {
  GetAssetLevelReportForExodusDTO,
  AssetLevelReportForExodusResponse,
  AssetLevelExodusGrid,
  GET_GRID_ASSET_LEVEL_EXODUS,
  GET_OPCO_DCF_DROPDOWN,
  OpcoDcfDropdownResponse,
} from "../../../Model/Report/AssetLevelReportForExodus";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetAssetLevelReportForExodusGrid(
  queryFilter?: GetAssetLevelReportForExodusDTO
) {
  setLoader("ADD", "GetAssetLevelExodusGrid");
  let result: AssetLevelReportForExodusResponse | null | undefined;
  let api = new AssetLevelReportForExodusApi();
  try {
    result = await ApiCallWithErrorHandling(() =>
      api.GetAssetLevelReportForExodus(queryFilter ?? {})
    );

    let rtn = {
      AssetLevelExodusResult: result,
      filter: null,
    } as AssetLevelExodusGrid;
    rootStore.dispatch({ type: GET_GRID_ASSET_LEVEL_EXODUS, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_ASSET_LEVEL_EXODUS,
      payload: {
        AssetLevelExodusResult: null,
        filter: null,
      } as AssetLevelExodusGrid,
    });
  }
  setLoader("REMOVE", "GetAssetLevelExodusGrid");
}

export async function GetOpcoAndPlannedDcfDropdown(
  body: Record<string, any> = {}
) {
  setLoader("ADD", "GetOpcoDcfDropdown");
  let result: OpcoDcfDropdownResponse | null | undefined;
  let api = new AssetLevelReportForExodusApi();
  try {
    result = await ApiCallWithErrorHandling(() =>
      api.GetOpcoAndPlannnedDcfDropdown(body)
    );
    rootStore.dispatch({
      type: GET_OPCO_DCF_DROPDOWN,
      payload: { OpcoDcfDropdownResult: result } as AssetLevelExodusGrid,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_OPCO_DCF_DROPDOWN,
      payload: { OpcoDcfDropdownResult: null } as AssetLevelExodusGrid,
    });
  }
  setLoader("REMOVE", "GetOpcoDcfDropdown");
}
