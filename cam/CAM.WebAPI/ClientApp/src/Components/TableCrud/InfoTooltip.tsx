import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import { Link } from "react-router-dom";
import Box from "@mui/material/Box";
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import Glossary from "../Glossary";

interface Props {
  tooltipText?: string;
  columName?: string;
  shortenedName?: string;
  spanClassName?: string;
  hideFilter?: boolean;
  action?: {
    settingVisibility: () => void;
  };
}
const InfoTooltip: React.FC<Props> = (props) => {
  const [longName, setLongName] = useState<boolean>(false);
  const [isModalVisible, setModalVisible] = useState<boolean>(false);
  const [glossaryView, setGlossaryView] = useState<boolean>(false);

  const name = longName
    ? props.columName
    : props.shortenedName || props.columName;

  const checkName = (val) => {
    let splitName = val.split("empty");
    return isNaN(parseInt(splitName[1])) ? val : " ";
  };

  return (
    <div className="divFilter">
      <label
        style={{ cursor: props.hideFilter ? "default" : "pointer" }}
        className={`filterColor ${props.spanClassName} ${
          isModalVisible ? "active" : "inactive"
        }`}
        onClick={() => setModalVisible(true)}
        tabIndex={0}
        onKeyDown={(e) => {
          if (e.key === "Enter") {
            e.stopPropagation();
            setModalVisible(true);
          }
        }}
      >
        {name?.includes("empty") ? checkName(name) : name}
      </label>
      {isModalVisible ? (
        <Modal
          show={isModalVisible}
          onHide={() => setModalVisible(false)}
          // backdrop="static"
          backdropClassName="backdropConfirm"
          dialogClassName="dialogConfirm"
          className="modalConfirm"
          keyboard={false}
          centered
        >
          <Modal.Header>
            <Modal.Title>
              <div className="">{props.columName}</div>
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Please
            {/* replace redirect to popup */}
            <a
              // href={
              //   window.location.origin + "/glossary/" + props.columName?.trim()
              // }
              // target="_blank"
              onClick={() => {
                setGlossaryView(true);
                setModalVisible(false);
              }}
              className="custom-link"
            >
              {" "}
              click here{" "}
            </a>
            to go to the glossary page
            {/* <div className="col-12">
              {props.children != undefined ? (
                props.children
              ) : (
                <div
                  className="tooltipDiv"
                  dangerouslySetInnerHTML={{
                    __html: props.tooltipText ?? "",
                  }}
                ></div>
              )}
            </div> */}
          </Modal.Body>
          <Modal.Footer>
            <div className="col-12 justify-content-end mt-4 d-flex footerModal">
              <button
                className="  voda-bold btn btn-danger px-4 btnHeader"
                onClick={() => setModalVisible(false)}
                type="button"
              >
                Close
              </button>
            </div>
          </Modal.Footer>
        </Modal>
      ) : null}

      {glossaryView ? (
        <Dialog
          open={glossaryView}
          onClose={() => setGlossaryView(false)}
          aria-labelledby="alert-dialog-title"
          aria-describedby="alert-dialog-description"
          maxWidth="xl"
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
              <IconButton
                aria-label="close"
                onClick={() => {
                  setGlossaryView(false);
                }}
              >
                <IoClose size={25} />
              </IconButton>
            </Box>

            <Glossary name={props.columName?.trim()} />
          </DialogContent>
        </Dialog>
      ) : (
        ""
      )}
    </div>
  );
};
export default InfoTooltip;
