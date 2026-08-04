import { useContext } from "react";
import { ModalContext, ModalContextType } from "../Context/ModalContext";

export const useModal = (): ModalContextType => useContext(ModalContext);
