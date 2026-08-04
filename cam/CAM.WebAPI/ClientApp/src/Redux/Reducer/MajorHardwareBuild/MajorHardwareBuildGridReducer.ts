import {
  GET_GRID_MAJOR_HARDWARE_BUILD,
  GET_FILTER_MAJOR_HARDWARE_BUILD,
  MajorHardwareBuildGrid,
} from "../../../Model/MajorHardwareBuild";

const initState: MajorHardwareBuildGrid = {
  MajorHardwareBuildGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const MajorHardwareBuildGridReducer = (
  state = initState,
  action: { type: string; payload: MajorHardwareBuildGrid }
) => {
  switch (action.type) {
    case GET_GRID_MAJOR_HARDWARE_BUILD: {
      return {
        ...state,
        MajorHardwareBuildGridResult:
          action.payload.MajorHardwareBuildGridResult,
      };
    }
    case GET_FILTER_MAJOR_HARDWARE_BUILD:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
