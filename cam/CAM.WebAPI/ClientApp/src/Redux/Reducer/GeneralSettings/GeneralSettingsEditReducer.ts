import {
    EDIT_GENERAL_SETTINGS,
    GET_EDIT_GENERAL_SETTINGS,
    GeneralSettingsEdit,
  } from "../../../Model/GeneralSettingsModal";
  
  const initState: GeneralSettingsEdit = {
    GeneralSettingsDtoEdit: null,
    ResultDtoEdit: null,
  };
  
  export const GeneralSettingsEditReducer = (
    state = initState,
    action: { type: string; payload: GeneralSettingsEdit }
  ): GeneralSettingsEdit => {
    switch (action.type) {
      case EDIT_GENERAL_SETTINGS:
        return {
          ...state,
          ResultDtoEdit: action.payload.ResultDtoEdit,
        };
  
      case GET_EDIT_GENERAL_SETTINGS:
        return {
          ...state,
          GeneralSettingsDtoEdit: action.payload.GeneralSettingsDtoEdit,
        };
  
      default:
        return state;
    }
  };
  