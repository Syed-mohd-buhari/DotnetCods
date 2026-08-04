import { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import { ResultDto } from "../Model/CommonModels";
import { setStateModal } from "../Redux/Action/ModalAction";

export function useOperationTableCrud<ModelCreate, ModelEdit>(
  functionForNewResource: Function,
  functionForEditResource: Function,
  functionForDelete: (id: number, apiType?: string) => Promise<ResultDto>,
  functionCallbackDelete: Function,
  functionForRestore?: (id: number) => Promise<ResultDto>
) {
  const [isVisibleModal, setVisibleModal] = useState(false);
  const [edit, setEdit] = useState(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const navigate = useNavigate();
  const location: any = useLocation();
  const [localStateHistory, setLocalState] = useState<
    | {
        id: number | null;
        tab: string;
        prevPage: string;
        formDisabed?: boolean;
      }
    | undefined
  >();

  //APERTURE MODALI CARICAMENTO RISORSE
  const New = async () => {
    functionForNewResource().then((x) => {
      setEdit(false);
      setVisibleModal(true);
      setStateModal(true);
    });
  };

  const Edit = (id: number) => {
    functionForEditResource(id).then((x) => {
      setEdit(true);
      setVisibleModal(true);
      setStateModal(true);
    });
  };

  const ConfirmRestore = (id: number) => {
    functionForRestore &&
      functionForRestore(id).then((x) => {
        if (x.warning === false && functionCallbackDelete !== undefined) {
          functionCallbackDelete();
          setConfirm(stateConfirm);
        }
      });
  };

  const Restore = (id: number) => {
    setConfirm({
      title: "Restore",
      message: "Do you want to Restore this item?",
      button: "Restore",
      item: id,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => ConfirmRestore(id),
      },
    });
  };

  const Delete = (id: number, apiType?: string, orphan?: boolean) => {
    setConfirm({
      title:
        orphan === true
          ? "Delete orphan record"
          : orphan === false
          ? "Delete record in use"
          : "Delete Entry",
      message:
        orphan === true
          ? "This item is not currently in use in another entity, do you want to delete it?"
          : orphan === false
          ? "This item is currently in use in another entity, do you want to delete it?"
          : "Do you want to delete this item?",
      button: "Delete",
      item: id,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => ConfirmDelete(id, apiType),
      },
    });
  };

  const Cancel = () => {
    setConfirm({
      title: "Warning!",
      message:
        "Are you sure you want to cancel? If you cancel, your changes will be permanently lost.",
      button: "Do Nothing",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => closeModal(),
        confirm: () => CancelConfirm(),
      },
    });
  };

  const closeModal = (changed?: boolean) => {
    if (changed) {
      Cancel();
    } else {
      setConfirm(stateConfirm);
      setVisibleModal(false);
      setStateModal(false);
      if (location.state != null && location.state !== undefined) {
        let locationState = {
          pathname: location.pathname,
          search: "",
          state: undefined,
        };
        // navigate(locationState, { replace: true });
      }
    }
  };

  //Cancel MODAL CONFIRM
  const CancelConfirm = () => {
    setConfirm(stateConfirm);
  };

  //CONFIRM MODALE CONFIRM DELETE
  const ConfirmDelete = (id: number, apiType?: string) => {
    functionForDelete(id, apiType).then((x) => {
      if (x.warning === false && functionCallbackDelete !== undefined)
        functionCallbackDelete();
    });
  };

  return {
    New,
    Edit,
    isVisibleModal,
    edit,
    confirm,
    closeModal,
    Delete,
    localStateHistory,
    setLocalState,
    Restore,
  };
}
