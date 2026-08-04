import { SET_MAJOR_SW_BUILD_INDEX } from "../../Action/TourGuide/tourAction";
import { SET_MAJOR_HW_BUILD_INDEX } from "../../Action/TourGuide/tourAction";
import { SET_SYSTEM_TYPE_INDEX } from "../../Action/TourGuide/tourAction";

interface TourState {
  systemTypeIndex: number | null;
  majorHwBuildIndex: number | null;
  majorSwBuildIndex: number | null;
}

const initialState: TourState = {
  majorSwBuildIndex: null,
  majorHwBuildIndex: null,
  systemTypeIndex: null,
};

const tourStepReducer = (state = initialState, action: any): TourState => {
  switch (action.type) {
    case SET_MAJOR_SW_BUILD_INDEX:
      return {
        ...state,
        majorSwBuildIndex: action.payload,
      };
    case SET_MAJOR_HW_BUILD_INDEX:
      return {
        ...state,
        majorHwBuildIndex: action.payload,
      };
    case SET_SYSTEM_TYPE_INDEX:
      return {
        ...state,
        systemTypeIndex: action.payload,
      };

    default:
      return state;
  }
};

export default tourStepReducer;
