import React, { useEffect, useState, useRef } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { GlossaryApi } from "../Business/GlossaryBusiness";

import LabelsDictionary from "../Constant/LabelsAndDescriptions.json";
import Highlighter from "react-highlight-words";
import { useParams } from "react-router-dom";
import { Dropdown, Modal, NavItem } from "react-bootstrap";

import { paginate } from "./paginate";
import setLoader from "../Redux/Action/LoaderAction";
import ModalConfirm from "./ModalConfirm";
import GlossaryForm from "../screen/Glossary/GlossaryForm";
import { rootStore } from "../Redux/Store/rootStore";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import ThreeDot from "./TableCrud/ThreeDot";
import { useAuth } from "../Hook/useAuth";
const Glossary = ({ name }) => {
  const glossaryApi = new GlossaryApi();
  // const { name } = useParams();
  const { isPermesso } = useAuth();
  const [currentPage, setCurrentPage] = useState(1);
  const [searchTerm, SetSearchTerm] = useState("");
  const [originalData, setOriginalData] = useState<any[]>([]);
  const [openModal, setOpenModal] = useState(false);
  const [editItem, setEditItem] = useState(null);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [charsList, setCharsList] = useState<string[]>([
    "A",
    "B",
    "C",
    "D",
    "E",
    "F",
    "G",
    "H",
    "I",
    "J",
    "K",
    "L",
    "M",
    "N",
    "O",
    "P",
    "Q",
    "R",
    "S",
    "T",
    "U",
    "V",
    "W",
    "X",
    "Z",
  ]);
  const [searchPattern, setSearchPattern] = useState<string>("All");

  const [data, setData] = useState<any[]>([]);

  useEffect(() => {
    isPermesso && getGlossaryData();
  }, [isPermesso]);

  useEffect(() => {
    if (name && data?.length) {
      const myDiv = document.getElementById(name);
      console.log("my div =? ", myDiv);
      if (myDiv) {
        const previousDiv = myDiv.parentElement?.previousElementSibling;
        if (previousDiv) {
          previousDiv?.scrollIntoView({
            behavior: "smooth",
            block: "start",
          });
        }
      }
    }
  }, [name, data]);

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);
  const containerRef = useRef<HTMLTableElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (rowId, item, event) => {
    setDropdownStates(() => ({
      rowId,
      item,
    }));
    //
    const element = containerRef.current;
    if (element) {
      const rect = element.getBoundingClientRect();
      const width = Math.floor(rect.width);
      const height = rect.height;

      const left = event.clientX;
      const top = event.clientY;

      const windowWidth = width;
      const windowHeight = height;

      const maxLeft = windowWidth - 200;
      const maxTop = windowHeight - 300;

      setDropdownPosition({
        left: Math.min(left, maxLeft),
        top: Math.min(top, maxTop),
      });
    }
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current) {
        if (
          event.target.tagName === "IMG" &&
          event.target.classList.contains("dropdown_trigger")
        ) {
          return;
        }
        setDropdownStates({});
      }
    };

    const tables = document.getElementsByClassName("table-container");

    // Attach the event listener when the component mounts
    document.addEventListener("click", handleClickOutside);

    for (let i = 0; i < tables.length; i++) {
      tables[i].addEventListener("scroll", handleClickOutside);
    }

    // Clean up the event listener when the component unmounts
    return () => {
      document.removeEventListener("click", handleClickOutside);
      for (let i = 0; i < tables.length; i++) {
        tables[i].removeEventListener("scroll", handleClickOutside);
      }
    };
  }, []);

  const handelPageChange = (page) => {
    setCurrentPage(page);
  };

  const getGlossaryData = () => {
    setLoader("ADD", "Glossary");
    glossaryApi
      .glossaryGetGlossary()
      .then((res) => {
        console.log("data => ", data);
        setData(res);
        setOriginalData(res);
        setLoader("REMOVE", "Glossary");
      })
      .catch((error) => {
        setLoader("REMOVE", "Glossary");
      });
  };

  const onNew = () => {
    console.log("new");
    setEditItem(null);
    setOpenModal(true);
  };

  const onSubmitted = (addedItem: any) => {
    //debugger;
    setLoader("ADD", "Glossary");
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
          setLoader("REMOVE", "Glossary");
        })
        .catch((error) => {
          rootStore.dispatch(
            setNotification({
              message: error?.info ?? "",
              notifyType: NotifyType.error,
            })
          );
          setLoader("REMOVE", "Glossary");
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
          setLoader("REMOVE", "Glossary");
        })
        .catch((error) => {
          rootStore.dispatch(
            setNotification({
              message: error?.info ?? "",
              notifyType: NotifyType.error,
            })
          );
          setLoader("REMOVE", "Glossary");
        });
    }
  };

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
        setData((oldData) =>
          oldData?.filter((item: any) => item.glossaryItemsId !== id)
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

  const onSearch = (e: any) => {
    SetSearchTerm(e.target.value);
    setSearchPattern("All");

    if (e?.target?.value === "" && !e?.arget?.value) {
      setData(originalData);
      return;
    }

    const seachData = originalData.filter((item: any) =>
      item?.description
        ?.toLowerCase()
        ?.includes(e?.target?.value?.toLowerCase())
    );
    setData(seachData);
  };

  const onCharsSearch = (char: string) => {
    setSearchPattern(char);
    console.log(data);
    const filterUpdated = originalData.filter(
      (item) =>
        item?.header?.toLowerCase()?.substring(0, 1) ===
        char.toLocaleLowerCase()
    );

    setData(filterUpdated);
  };

  return (
    <div className={`${name ? "pageContainerModal" : "pageContainer"}`}>
      <div className="headerPage row mx-0 justify-content-between">
        <h3 className="voda-bold">Glossary</h3>
        <div style={{ justifyContent: "end", display: "flex" }}>
          <button
            className="voda-bold btn btn-danger btnHeader"
            onClick={() => onNew()}
            type="button"
          >
            <span className="fz-14 ">New Glossary</span>
          </button>
        </div>
      </div>

      <ModalConfirm data={confirm} />

      <Modal
        show={openModal}
        // backdrop="static"
        keyboard={false}
        size="lg"
        className="new-modal glossary-edit-modal"
        onHide={() => setOpenModal(false)}
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 mt-3">
            <h4>{editItem ? "Edit Glossary" : "Create Glossary"}</h4>
          </div>
        </Modal.Header>
        <Modal.Body style={{ padding: "0 2rem" }}>
          <GlossaryForm
            cancel={() => setOpenModal(false)}
            submit={onSubmitted}
            editItem={editItem}
          />
        </Modal.Body>
      </Modal>

      <div className="listaApparatiContainer mt-3 mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0">
          <div className="input-group mb-3">
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => onSearch(e)}
              className="form-control mb-2 w-100"
              placeholder="Search"
              aria-label="Recipient's username"
              aria-describedby="basic-addon2"
            />
          </div>
          <div>
            <ul className="chars-list">
              <li
                className={"All" === searchPattern ? "active" : ""}
                onClick={() => {
                  setData(originalData);
                  setSearchPattern("All");
                }}
              >
                All
              </li>
              {charsList.map((item, index) => (
                <li
                  className={item === searchPattern ? "active" : ""}
                  onClick={() => onCharsSearch(item)}
                  key={index}
                >
                  {item}
                </li>
              ))}
            </ul>
          </div>
          <div
            className="mx-0 px-0 table-container"
            style={{ position: "relative" }}
          >
            {dropdownStates["rowId"] && (
              <Dropdown
                className="d-inline mx-2"
                show={dropdownStates["rowId"] ? true : false}
                ref={dropdownRef}
                style={
                  dropdownStates["rowId"]
                    ? {
                        position: "fixed",
                        top: `${dropdownPosition.top - 50}px`,
                        left: `${dropdownPosition.left + 50}px`,
                        transform: "translate(-50%, -50%)",
                        zIndex: 9999,
                      }
                    : {
                        position: "absolute",
                        top: "0px",
                        left: "0px",
                        margin: "0px",
                        opacity: "0",
                      }
                }
              >
                <div
                  className={`${
                    dropdownStates["rowId"]
                      ? "dropdown-menu show"
                      : "dropdown-menu"
                  }`}
                >
                  <Dropdown.Item onClick={() => onEdit(dropdownStates["item"])}>
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() => onDelete(dropdownStates["rowId"])}
                  >
                    Delete
                  </Dropdown.Item>
                </div>
              </Dropdown>
            )}
            <table
              className="w-100"
              style={{ minHeight: "20px" }}
              ref={containerRef}
            >
              <thead>
                <tr className="intestazione plr-10">
                  <th
                    scope="col"
                    style={{
                      fontSize: "15px",
                      padding: "0 20px",
                      textAlign: "left",
                      background: "#bdbdc0",
                    }}
                  >
                    Header
                  </th>
                  <th
                    scope="col"
                    style={{
                      fontSize: "15px",
                      padding: "0 10px",
                      textAlign: "left",
                      background: "#bdbdc0",
                    }}
                  >
                    Description
                  </th>
                  <th
                    scope="col"
                    style={{
                      fontSize: "15px",
                      padding: "0 10px",
                      textAlign: "left",
                      background: "#bdbdc0",
                    }}
                  ></th>
                </tr>
              </thead>
              <tbody>
                {data
                  .filter((item) => item?.header?.trim() !== "" && item?.header)
                  .map((item: any) => {
                    return (
                      <tr
                        className="dati"
                        key={item?.glossaryItemsId}
                        onDoubleClick={(e) =>
                          toggleDropdown(item?.glossaryItemsId, item, e)
                        }
                      >
                        <td className="left" id={item?.header}>
                          <Highlighter
                            highlightClassName="YourHighlightClass"
                            searchWords={[name]}
                            autoEscape={true}
                            textToHighlight={item?.header}
                          />
                        </td>
                        <td>
                          <p
                            dangerouslySetInnerHTML={{
                              __html: item?.description,
                            }}
                          ></p>
                        </td>
                        <td className="actions">
                          {
                            <div className="d-inline mx-2 cursor-pointer">
                              <img
                                className="dropdown_trigger"
                                src={require("../img/options_dots.png")}
                                style={{ cursor: "pointer", padding: "10px" }}
                                onClick={(e) => {
                                  toggleDropdown(
                                    item?.glossaryItemsId,
                                    item,
                                    e
                                  );
                                }}
                              />
                            </div>
                          }
                        </td>
                      </tr>
                    );
                  })}
              </tbody>
            </table>
          </div>
          {data && data.length ? null : (
            <p className="alert alert-danger"> no data found !! </p>
          )}
        </div>
      </div>
    </div>
  );
};
export default Glossary;
