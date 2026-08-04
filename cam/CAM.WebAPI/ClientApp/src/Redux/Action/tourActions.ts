import { Step } from "react-joyride";
import {
  TourStep,
  TourActionTypes,
  START_TOUR,
  STOP_TOUR,
  START_GUIDE_TOUR,
  END_GUIDE_TOUR,
  NEXT_STEP,
  SET_STEP_INDEX,
  SET_TOUR_STEPS,
} from "../../Model/tourTypes";

export const startTour = (steps: TourStep[]): any => ({
  type: START_TOUR,
  payload: steps,
});

export const stopTour = (): any => ({
  type: STOP_TOUR,
});

export const nextStep = (): any => ({
  type: NEXT_STEP,
});

export const startGuideTour = () => ({
  type: START_GUIDE_TOUR,
});

export const endGuideTour = () => ({
  type: END_GUIDE_TOUR,
});

export const setStepIndex = (index: number): any => ({
  type: SET_STEP_INDEX,
  payload: index,
});

export const setTourSteps = (steps: Step[]) => ({
  type: SET_TOUR_STEPS,
  payload: steps,
});
