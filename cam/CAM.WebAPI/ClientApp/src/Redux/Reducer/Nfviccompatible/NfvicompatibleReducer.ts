import {
  NFVICCompatibilityGrid,
  GET_GRID_NFVIC_COMPATIBILITY,
  GET_FILTER_NFVIC_COMPATIBILITY,
  GET_VMWARE_DROPDOWN,
  VmWareDropdowns,
} from "../../../Model/NfvicCompatible";

const initialState: NFVICCompatibilityGrid = {
  NFVICCompatibilityGridResult: null,
  filter: null,
  dropdowns: null, // Can be null, or we can set a default empty object if needed
};

export const NFVICCompatibleReducer = (
  state = initialState,
  action: { type: string; payload: Partial<NFVICCompatibilityGrid> }
): NFVICCompatibilityGrid => {
  switch (action.type) {
    case GET_GRID_NFVIC_COMPATIBILITY:
      return {
        ...state,
        NFVICCompatibilityGridResult: action.payload.NFVICCompatibilityGridResult ?? null,
      };

    case GET_FILTER_NFVIC_COMPATIBILITY:
      return {
        ...state,
        filter: action.payload.filter ?? null,
      };

    case GET_VMWARE_DROPDOWN:
      return {
        ...state,
        dropdowns: action.payload.dropdowns ?? null, // Default to null if no dropdowns are present
      };

    default:
      return state;
  }
};
