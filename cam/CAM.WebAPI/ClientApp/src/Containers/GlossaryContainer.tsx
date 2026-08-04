import React, { useEffect, useState } from "react";
import GlossaryForm from "../screen/Glossary/GlossaryForm";
import { GlossaryApi } from "../Business/GlossaryBusiness";
import { Dropdown, Modal, NavItem } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import { rootStore } from "../Redux/Store/rootStore";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { setNotification } from "../Redux/Action/NotificationAction";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import ModalConfirm from "../Components/ModalConfirm";
import setLoader from "../Redux/Action/LoaderAction";
import { useAuth } from "../Hook/useAuth";
import ThreeDot from "../Components/TableCrud/ThreeDot";
import glossaryImg from "../img/plus_1.png";

const GlossaryContainer = () => {
  const { isPermesso } = useAuth();
  const glossaryApi = new GlossaryApi();
  const [glossaryData, setGlossaryData] = useState<any[]>([]);
  const [originalData, setOriginalData] = useState<any[]>([]);
  const [openModal, setOpenModal] = useState(false);
  const [editItem, setEditItem] = useState(null);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [searchTerm, SetSearchTerm] = useState("");

  useEffect(() => {
    isPermesso && getGlossaryData();
  }, [isPermesso]);

  const onEdit = (item: any) => {
    setEditItem(item);
    setOpenModal(true);
  };

  const onDelete = (id: number) => {
    setConfirm({
      title: "Confirm",
      message: "Are you shure to delete this record ?",
      button: "Ok",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => {
          setConfirm(stateConfirm);
        },
        confirm: () => {
          onDeleteRecord(id);
          setConfirm(stateConfirm);
        },
      },
    });
  };

  const onDeleteRecord = (id: number) => {
    glossaryApi
      .glossaryDelete(id)
      .then((res) => {
        rootStore.dispatch(
          setNotification({
            message: res?.info ?? "",
            notifyType: NotifyType.success,
          })
        );
        setGlossaryData((oldData) =>
          oldData?.filter((item) => item.glossaryItemsId !== id)
        );
      })
      .catch((error) => {
        rootStore.dispatch(
          setNotification({
            message: error?.info ?? "",
            notifyType: NotifyType.error,
          })
        );
      });
  };

  const getGlossaryData = () => {
    setLoader("ADD", "Glossary");
    glossaryApi
      .glossaryGetGlossary()
      .then((res) => {
        setGlossaryData(res);
        setOriginalData(res);
        setLoader("REMOVE", "Glossary");
      })
      .catch((error) => {
        setLoader("REMOVE", "Glossary");
      });
  };

  const onNew = () => {
    setEditItem(null);
    setOpenModal(true);
  };

  const onSubmitted = (addedItem: any) => {
    if (editItem) {
      glossaryApi
        .glossaryIsPut(addedItem)
        .then((res) => {
          getGlossaryData();
          setOpenModal(false);
          rootStore.dispatch(
            setNotification({
              message: res?.info ?? "",
              notifyType: NotifyType.success,
            })
          );
        })
        .catch((error) => {
          rootStore.dispatch(
            setNotification({
              message: error?.info ?? "",
              notifyType: NotifyType.error,
            })
          );
        });
    } else {
      glossaryApi
        .glossaryCreate(addedItem)
        .then((res) => {
          getGlossaryData();
          setOpenModal(false);
          rootStore.dispatch(
            setNotification({
              message: res?.info ?? "",
              notifyType: NotifyType.success,
            })
          );
        })
        .catch((error) => {
          rootStore.dispatch(
            setNotification({
              message: error?.info ?? "",
              notifyType: NotifyType.error,
            })
          );
        });
    }
  };

  const onSearch = (e: any) => {
    SetSearchTerm(e.target.value);

    if (e?.target?.value === "" && !e?.arget?.value) {
      setGlossaryData(originalData);
      return;
    }

    const seachData = glossaryData.filter((item) =>
      item?.description
        ?.toLowerCase()
        ?.includes(e?.target?.value?.toLowerCase())
    );
    setGlossaryData(seachData);
  };

  return (
    <div className="listaApparatiContainer p-40">
      <ModalConfirm data={confirm} />
      <Modal
        show={openModal}
        backdrop="static"
        keyboard={false}
        size="lg"
        className="new-modal"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0 mb-2">
            <div className="col-12 mt-3">
              <h4 className="pd-15">
                {editItem === "create" ? "Create Glossary" : "Edit Glossary"}
              </h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <GlossaryForm
            cancel={() => setOpenModal(false)}
            submit={onSubmitted}
            editItem={editItem}
          />
        </Modal.Body>
      </Modal>
      <button
        className="voda-bold btn btn-danger px-4 btnHeader addNewGlossary"
        onClick={() => onNew()}
        type="button"
        style={{ float: "right" }}
      >
        <img src={glossaryImg} className="img-15" />
        <span className="fz-14">New Glossary</span>
      </button>
      <input
        type="text"
        value={searchTerm}
        onChange={(e) => onSearch(e)}
        className="form-control mb-2 w-100"
        placeholder="Search"
        aria-label="Recipient's username"
        aria-describedby="basic-addon2"
      />
      <div
        className="listaApparatiContainer mt-3 mx-0 col-12 p-0 justify-content-center"
        style={{ height: "320px", overflow: "auto" }}
      >
        <div className="mx-0 px-0 py-3 flex-row">
          <table className="w-100 table-border">
            <thead>
              <tr className="intestazione">
                <th className="text-left plr-10">Id</th>
                <th className="text-left plr-10">Header</th>
                <th className="text-left plr-10 ">Description</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {glossaryData.map((item) => (
                <tr className="dati" key={item?.glossaryItemsId}>
                  <td>{item?.glossaryItemsId}</td>
                  <td>{item?.header}</td>
                  <td>{item?.description}</td>
                  <td className="actions">
                    <Dropdown className="d-inline mx-2">
                      <Dropdown.Toggle id="dropdown-autoclose-true">
                        <ThreeDot />
                      </Dropdown.Toggle>

                      <Dropdown.Menu>
                        <Dropdown.Item onClick={() => onEdit(item)}>
                          Edit
                        </Dropdown.Item>
                        <Dropdown.Item
                          onClick={() => onDelete(item?.glossaryItemsId)}
                        >
                          Delete
                        </Dropdown.Item>
                      </Dropdown.Menu>
                    </Dropdown>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default GlossaryContainer;
