import { rootStore } from "../Store/rootStore";

export function setStateModal(isOpen: boolean) {
    rootStore.dispatch({ type: "MODAL_STATE", payload: isOpen })
}
