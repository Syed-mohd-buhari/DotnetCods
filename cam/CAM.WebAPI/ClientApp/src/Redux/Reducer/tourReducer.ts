import {
  TourState,
  TourActionTypes,
  START_TOUR,
  STOP_TOUR,
  START_GUIDE_TOUR,
  END_GUIDE_TOUR,
  NEXT_STEP,
  SET_STEP_INDEX,
  SET_TOUR_STEPS,
} from "../../Model/tourTypes";

const initialState: TourState = {
  run: false,
  steps: [],
  stepIndex: 0,
};

export const tourReducer = (state = initialState, action: any): TourState => {
  switch (action.type) {
    case START_TOUR:
      return { ...state, steps: action.payload, run: true, stepIndex: 0 };
    case STOP_TOUR:
      return { ...state, run: false, stepIndex: 0 };
    case NEXT_STEP:
      return { ...state, stepIndex: state.stepIndex + 1 };
    case SET_STEP_INDEX:
      return { ...state, stepIndex: action.payload };
    case SET_TOUR_STEPS:
      return { ...state, steps: action.payload };
    default:
      return state;
  }
};

interface TourGuideState {
  startGuideTour: boolean;
}

const initialState1: TourGuideState = {
  startGuideTour: false,
};

export const tourGuideReducer = (
  state = initialState1,
  action: any
): TourGuideState => {
  switch (action.type) {
    case START_GUIDE_TOUR:
      return { ...state, startGuideTour: true };
    case END_GUIDE_TOUR:
      return { ...state, startGuideTour: false };
    default:
      return state;
  }
};

export default tourReducer;
