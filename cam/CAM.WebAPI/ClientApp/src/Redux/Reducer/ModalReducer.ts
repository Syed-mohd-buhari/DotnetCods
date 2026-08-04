const initialState = {
  isOpen: false,
};

export const ModalReducer = (
  state = initialState,
  action: { type: string; payload: boolean }
) => {
  switch (action.type) {
    case "MODAL_STATE":
      return { isOpen: action.payload };
    default:
      return state;
  }
};
