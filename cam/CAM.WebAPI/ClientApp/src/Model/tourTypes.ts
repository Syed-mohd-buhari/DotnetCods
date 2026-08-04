import { StepType } from "@reactour/tour";
import { Step } from "react-joyride";
import { Action } from "redux";

export interface TourStep {
  content: React.ReactNode;
  target: string;
  placement?: string;
  title?: string;
  styles?: any;
  locale?: any;
}

export interface TourState {
  run: boolean;
  steps: any[];
  stepIndex: number;
}

export interface StartTourAction extends Action<typeof START_TOUR> {
  payload: TourStep[];
}

export interface StopTourAction extends Action<typeof STOP_TOUR> {}

export interface NextStepAction extends Action<typeof NEXT_STEP> {}

export interface StepsTourAction extends Action<typeof SET_TOUR_STEPS> {
  payload: Step[];
}

export interface SetStepIndexAction extends Action<typeof SET_STEP_INDEX> {
  payload: number;
}

export interface StartGuideTourAction extends Action<typeof START_GUIDE_TOUR> {}

export interface EndGuideTourAction extends Action<typeof END_GUIDE_TOUR> {}

export type TourActionTypes =
  | StartTourAction
  | StopTourAction
  | NextStepAction
  | SetStepIndexAction
  | StartGuideTourAction
  | EndGuideTourAction
  | StepsTourAction;

export type RoutedStep = StepType & {
  route: string;
};

export const tourSteps: RoutedStep[] = [
  {
    selector: ".landing_lcm_tour",
    content: "Lcm Engineering",
    route: "/home",
  },
  {
    selector: ".landing_assets_tour",
    content: "Assets",
    route: "/home",
  },
];

export const JoyrideSteps = [
  {
    target: "#landing_lcm_tour",
    content: "Lcm Engineering Menu",
  },
  {
    target: "#landing_assets_tour",
    content: "Assets Menu",
  },
];

export const START_TOUR = "START_TOUR";
export const STOP_TOUR = "STOP_TOUR";
export const START_GUIDE_TOUR = "START_GUIDE_TOUR";
export const END_GUIDE_TOUR = "END_GUIDE_TOUR";
export const NEXT_STEP = "NEXT_STEP";
export const SET_STEP_INDEX = "SET_STEP_INDEX";
export const SET_TOUR_STEPS = "SET_TOUR_STEPS";
