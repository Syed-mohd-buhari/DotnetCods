import { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import { ResultDto } from "../Model/CommonModels";
import { setStateModal } from "../Redux/Action/ModalAction";
interface dispacthOperationKey {
  KeyCreateResource: string;
  KeyEditResource: string;
}

export function useOperationTableCrud<ModelCreate, ModelEdit>(
  functionForNewResource: Function,
  functionForEditResource: Function,
  functionForDelete: (id: number) => Promise<ResultDto>,
  functionCallbackDelete: Function,
  functionForRestore?: (id: number) => Promise<ResultDto>
) {
  const [isVisibleModal, setVisibleModal] = useState(false);
  const [edit, setEdit] = useState(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const navigate = useNavigate();
  const location: any = useLocation();
  const [localStateHistory, setLocalState] = useState<
    { id: number | null; tab: string; prevPage: string } | undefined
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

  const Delete = (id: number, orphan?: boolean) => {
    setConfirm({
      title:
        orphan === true
          ? "Delete orphan record"
          : orphan === false
          ? "Delete record in use"
          : "Delete",
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
        confirm: () => ConfirmDelete(id),
      },
    });
  };

  const Cancel = () => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to quit? Unsaved changes will be lost.",
      button: "Exit",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => closeModal(),
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
      if (
        location.state != null &&
        location.state !== undefined
      ) {
        let locationState = {
          pathname: location.pathname,
          search: "",
          state: undefined,
        };
        navigate(locationState, { replace: true });
        // setLocalState(undefined);
      }
    }
  };

  //Cancel MODAL CONFIRM
  const CancelConfirm = () => {
    setConfirm(stateConfirm);
  };

  //CONFIRM MODALE CONFIRM DELETE
  const ConfirmDelete = (id: number) => {
    functionForDelete(id).then((x) => {
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
