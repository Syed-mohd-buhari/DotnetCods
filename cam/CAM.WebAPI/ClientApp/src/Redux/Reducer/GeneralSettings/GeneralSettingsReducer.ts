import {
  GET_GRID_GENERAL_SETTINGS,
  GET_EDIT_GENERAL_SETTINGS,
  EDIT_GENERAL_SETTINGS,
  GeneralSettingsGrid,
  GeneralSettingsEdit,
} from "../../../Model/GeneralSettingsModal";
import { AnyAction } from "redux";

// Default state
const initialState: GeneralSettingsGrid & GeneralSettingsEdit = {
  GeneralSettingsGridResult: null,
  filter: null,
  GeneralSettingsDtoEdit: null,
  ResultDtoEdit: null,
};

// Reducer
export function GeneralSettingsReducer(
  state: GeneralSettingsGrid & GeneralSettingsEdit = initialState,
  action: AnyAction
): GeneralSettingsGrid & GeneralSettingsEdit {
  switch (action.type) {
    case GET_GRID_GENERAL_SETTINGS:
      return {
        ...state,
        GeneralSettingsGridResult: action.payload.GeneralSettingsGridResult,
        filter: action.payload.filter,
      };

    case GET_EDIT_GENERAL_SETTINGS:
      return {
        ...state,
        GeneralSettingsDtoEdit: action.payload.GeneralSettingsDtoEdit,
        ResultDtoEdit: action.payload.ResultDtoEdit,
      };

    case EDIT_GENERAL_SETTINGS:
      return {
        ...state,
        ResultDtoEdit: action.payload.ResultDtoEdit,
      };

    default:
      return state;
  }
}
