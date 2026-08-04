import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import { Tab, Tabs } from "react-bootstrap";

import "../Css/NetworkElement.css";
import "../Css/NavBar.css";
import "../Css/index.css";
import { RelatedRecordsResultDto } from "../Model/CommonModels";

export interface Props {
  show: boolean;
  data: RelatedRecordsResultDto | undefined;
  deleteAllButton?: string;
  deleteButton?: string;
  impactsArea?: boolean;
  headerTitle?: string;
  action: {
    closeModal(): any;
    confirm?(deleteOnlyPlannedActivity: boolean): any;
  };
}

const ModalRelated: React.FC<Props> = (props) => {
  const [keyTabs, setKeyTabs] = useState<string>();

  useEffect(() => {
    if (props.data?.dataRelatedList)
      props.data?.dataRelatedList[0].table != undefined &&
        setKeyTabs(props.data?.dataRelatedList[0].table);
  }, [props.data]);

  const confirm = (deleteOnlyPlannedActivity) => {
    if (props.action.confirm) {
      props.action.confirm(deleteOnlyPlannedActivity);
    }

    if (props.impactsArea) {
      return;
    }
    props.action.closeModal();
  };

  let exitButtonClass = [" ", "voda-bold", "btn", "ml-3", "px-4", "btnHeader"];

  if (!props.deleteAllButton) exitButtonClass.push("btn-danger");

  return (
    <Modal
      show={props.show}
      // backdrop="static"
      backdropClassName="backdropRelatedModal"
      dialogClassName="dialogRelatedModal"
      className="dialogRelatedModal"
      keyboard={false}
      size="lg"
      centered
      onHide={props.action.closeModal}
    >
      <Modal.Header className="d-flex justify-content-center">
        <div className="col-12">
          <h4 className="mb-0">
            {props.deleteAllButton && !props.impactsArea
              ? "The record is not orphaned :"
              : props.impactsArea
              ? "Are you sure want to change ?"
              : props.headerTitle !== "" && props.headerTitle !== undefined
              ? props.headerTitle
              : "Delete Entry"}
          </h4>
        </div>
      </Modal.Header>
      <Modal.Body>
        <div className="col-12">
          <div className="d-flex flex-column">
            <span
              className="mt-1"
              style={{ color: "#333333", fontSize: "18px" }}
            >
              {/* The Record is not orphaned. */}
              The {props.data?.entityName}
              <label
                className="voda-bold"
                dangerouslySetInnerHTML={{
                  __html: `"${props.data?.recordName ?? ""}"`,
                }}
              ></label>{" "}
              It's related to:
            </span>
          </div>
          {/* <div className="col-12 "> */}
          <Tabs activeKey={keyTabs} onSelect={(x) => setKeyTabs(x || "")}>
            {props.data?.dataRelatedList?.map((item, idx: number) => (
              <Tab className="mt-4" eventKey={item.table} title={item.table}>
                <div
                  style={{
                    maxHeight: 350,
                    minHeight: 200,
                    overflow: "scroll",
                    overflowX: "hidden",
                    border: "1px solid #E5E5E5",
                    borderRadius: "10px",
                    padding: "10px",
                  }}
                >
                  <ul className="px-2" style={{ listStyle: "none" }}>
                    {item?.values?.map((el, idx) => (
                      <li
                        key={el + idx}
                        dangerouslySetInnerHTML={{ __html: el + "," }}
                      ></li>
                    ))}
                  </ul>
                </div>
              </Tab>
            ))}
          </Tabs>
          {/* </div> */}
          {/* <span className="mt-4">are related to it. </span> */}
        </div>
      </Modal.Body>
      <Modal.Footer>
        <div
          className={
            props.deleteAllButton
              ? "col-12 justify-content-around d-flex footerModal"
              : "col-12 justify-content-end d-flex footerModal"
          }
        >
          <button
            className={exitButtonClass.join(" ")}
            onClick={() => props.action.closeModal()}
            type="button"
          >
            Close
          </button>
          {props.deleteButton ? (
            <button
              className="  voda-bold btn btn-danger ml-3 px-4 btnHeader"
              onClick={() => confirm(true)}
              type="button"
            >
              {props.deleteButton}
            </button>
          ) : null}
          {props.deleteAllButton ? (
            <button
              className="  voda-bold btn btn-danger ml-3 px-4 btnHeader"
              onClick={() => confirm(false)}
              type="button"
            >
              {props.deleteAllButton}
            </button>
          ) : null}
        </div>
      </Modal.Footer>
    </Modal>
  );
};

export default ModalRelated;
