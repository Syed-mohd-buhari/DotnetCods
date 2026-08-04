import React, { useEffect, useState } from "react";
import { Form, Modal } from "react-bootstrap";
import "../Css/NavBar.css";
import "../Css/index.css";
import { DataModalConfirm } from "../Model/Common";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export interface Props {
  data: DataModalConfirm | any;
}

const ModalConfirm: React.FC<Props | any> = (props) => {
  const [isVisible, setVisible] = useState(false);
  const data = props.data;
  const [exportType, setExportType] = useState<string>("excel");
  const [modeType, setDbMode] = useState<string>("normal");

  useEffect(() => {
    setVisible(data.isOpen);
  }, [data]);

  return (
    <Dialog
      open={isVisible}
      onClose={() => data.actions.cancel()}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
      maxWidth="lg"
      scroll="body"
      fullWidth={true}
      slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
    >
      <DialogTitle className="d-flex justify-content-center">
        <div className="col-12">{data.title}</div>
      </DialogTitle>
      <IconButton
        aria-label="close"
        onClick={() => data.actions.cancel()}
        sx={{
          position: "absolute",
          right: 8,
          top: 8,
          color: (theme) => theme.palette.grey[500],
        }}
      >
        <IoClose size={25} />
      </IconButton>
      <DialogContent>
        {data.message !== "export_form" ? (
          <div className="col-12">
            <span
              className="my-4 voda-bold"
              dangerouslySetInnerHTML={{
                __html:
                  data.message === "" || data.message == null
                    ? "---"
                    : data.message,
              }}
            ></span>
            {props.showHyperLink && (
              <p>
                ( please follow the user onboarding process via the ARC portal)
                to guide the user to request for the access via ARC portal? The
                ARC portal on the text will be
                <a
                  target="_blank"
                  href="https://collaborate.vodafone.com/sites/TDO-UAM/default.aspx"
                >
                  a hyperlink Home - TDO-UAM (vodafone.com) ?
                </a>
              </p>
            )}
          </div>
        ) : null}

        {data.message === "Please select operating mode." ? (
          <div className="col-12">
            {/* <label>
              Training
              <input
                type="radio"
                id="training"
                value="Training"
                name="group3"
                onChange={() => {
                  setDbMode("training");
                }}
                checked={modeType === "training"}
              />
            </label> */}

            <Form.Check
              inline
              label="Training"
              value="Training"
              name="group3"
              type="radio"
              id="Training"
              checked={modeType === "training" ? true : false}
              onChange={() => {
                setDbMode("training");
              }}
            />
            <Form.Check
              inline
              label="Normal"
              value="Normal"
              name="group3"
              type="radio"
              checked={modeType === "normal" ? true : false}
              id="Normal"
              onChange={() => {
                setDbMode("normal");
              }}
            />
          </div>
        ) : null}

        {data.message === "export_form" ? (
          <div className="col-12">
            <Form.Check
              inline
              label="Excel"
              value="excel"
              name="group1"
              type="radio"
              id="Excel"
              checked={exportType === "excel" ? true : false}
              onChange={() => setExportType("excel")}
            />
            <Form.Check
              inline
              label="CSV"
              value="CSV"
              id="CSV"
              name="group1"
              type="radio"
              checked={exportType === "csv" ? true : false}
              onChange={() => setExportType("csv")}
            />
          </div>
        ) : null}
        <div className="col-12"></div>
      </DialogContent>
      <div className="col-12 justify-content-end mt-4 d-flex footerModal">
        {!data.onlyOneButton && (
          <button
            className={`voda-bold btn px-4 btnHeader ${
              data.footerAlert ? "btn-danger" : "btn-link cancel"
            }`}
            onClick={() => data.actions.cancel()}
            type="button"
          >
            {props.data.cancelText != undefined
              ? props.data.cancelText
              : "Close"}
          </button>
        )}

        {data?.button && (
          <button
            className="voda-bold btn btn-danger ml-3 px-4 btnHeader"
            onClick={() =>
              data.actions.confirm(
                data.message === "export_form" ? exportType! : modeType!
              )
            }
            type="button"
          >
            {data?.button}
          </button>
        )}

        {props.data.buttonSecond && props.data.buttonSecond !== null ? (
          <button
            className="voda-bold btn btn-danger ml-3 px-4"
            onClick={() =>
              data.actions.confirmSecond && data.actions.confirmSecond()
            }
            type="button"
          >
            {data.buttonSecond}
          </button>
        ) : null}
      </div>
    </Dialog>
  );
};

export default ModalConfirm;
