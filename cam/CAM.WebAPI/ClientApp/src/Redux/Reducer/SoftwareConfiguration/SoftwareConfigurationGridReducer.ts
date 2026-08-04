import {
  GET_GRID_SOFTWARE_CONFIGURATION,
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  SoftwareConfigurationGrid,
  GET_FILTER_OPCO,
  GET_FILTER_OEM,
  GET_FILTER_ELE,
} from "../../../Model/SoftwareConfiguration";

const initState: SoftwareConfigurationGrid = {
  SoftwareConfigurationGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

//Added newly for New Network Element

export const SoftwareConfigurationGridReducer = (
  state = initState,
  action: { type: string; payload: SoftwareConfigurationGrid }
) => {
  switch (action.type) {
    case GET_GRID_SOFTWARE_CONFIGURATION: {
      return {
        ...state,
        SoftwareConfigurationGridResult:
          action.payload.SoftwareConfigurationGridResult,
      };
    }
    case GET_FILTER_NETWORK_ELEMENT_AS_IS:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};

export const SoftwareConfigurationOpcoReducer = (
  state = initState,
  action: { type: string; payload: SoftwareConfigurationGrid }
) => {
  switch (action.type) {
    case GET_FILTER_OPCO:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};

export const SoftwareConfigurationOemReducer = (
  state = initState,
  action: { type: string; payload: SoftwareConfigurationGrid }
) => {
  switch (action.type) {
    case GET_FILTER_OEM:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};

export const SoftwareConfigurationEleReducer = (
  state = initState,
  action: { type: string; payload: SoftwareConfigurationGrid }
) => {
  switch (action.type) {
    case GET_FILTER_ELE:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
