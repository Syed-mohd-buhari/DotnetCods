import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { GeneralSettingsApi } from "../../../Business/GeneralSettingsBusiness";
import {
  GeneralSettingsGrid,
  QueryResultDtoOfGeneralSettingsDtoGrid,
  GET_GRID_GENERAL_SETTINGS,
  GeneralSettingsQueryObjectGrid,
} from "../../../Model/GeneralSettingsModal";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

// Accept optional query: GeneralSettingsQueryObjectGrid
export async function GetGeneralSettingsGrid(
  query?: GeneralSettingsQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetGeneralSettingsGrid");

  let result: QueryResultDtoOfGeneralSettingsDtoGrid | null | undefined;
  let api = new GeneralSettingsApi();

  try {
    const finalQuery: GeneralSettingsQueryObjectGrid = query ?? {
      sortBy: "",
      isSortAscending: true,
      page: 1,
      pageSize: 20,
      deleted: false,
      orphan: false,
    };

    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfGeneralSettingsDtoGrid>
    >(() => api.generalSettingsGetGrid(finalQuery));

    if (returnValues !== true) {
      rootStore.dispatch({
        type: GET_GRID_GENERAL_SETTINGS,
        payload: {
          GeneralSettingsGridResult: result,
          filter: null,
        } as GeneralSettingsGrid,
      });
    } else {
      setLoader("REMOVE", "GetGeneralSettingsGrid");
      return result?.items;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Failed to fetch General Settings",
        notifyType: NotifyType.error,
      })
    );

    rootStore.dispatch({
      type: GET_GRID_GENERAL_SETTINGS,
      payload: { GeneralSettingsGridResult: null, filter: null } as GeneralSettingsGrid,
    });
  }

  setLoader("REMOVE", "GetGeneralSettingsGrid");
}
