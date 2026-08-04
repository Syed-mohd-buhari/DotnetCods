import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { GetTSRReport } from "../Redux/Action/TSRReport/TSRReportDownloadAction";
import { GetTSRRefresh } from "../Redux/Action/TSRReport/TSRReportImportAction";
import { Alert } from "react-bootstrap";
import { formatDateWithTime } from "../Hook/Common";

interface Props {
  endTime?: string | undefined;
  redirect?: string;
  modal?: {
    isModal: boolean | false;
    setRefreshTsrModal(flag: boolean): any;
    closeModalSetup(changed?: boolean): any;
  };
}

const TSRRefreshDataReport = (props: Props) => {
  const [show, setShow] = useState(false);
  const [showMsg, setShowMsg] = useState("");
  const [showMsgType, setShowMsgType] = useState("");
  const DataRefreshCall = async (type: string) => {
    if (type === "all") {
      let result = await GetTSRRefresh({});
      if (result) {
        setShowMsg(result?.info);
        setShowMsgType(result.warning ? "danger" : "success");
        setShow(true);
        props.modal && props.modal.closeModalSetup(false);
      }
    } else {
      let result = await GetTSRRefresh({ countryWhereAssetIsLocated: [type] });
      if (result) {
        setShowMsg(result?.info);
        setShowMsgType(result.warning ? "danger" : "success");
        setShow(true);
        props.modal && props.modal.closeModalSetup(false);
      }
    }
  };

  return (
    <div
      className={`${
        props?.redirect === "tsrReport" ? "mt-3" : "pageContainer"
      }`}
      style={{
        height: props?.redirect !== "tsrReport" ? "20rem" : "",
        alignContent: "center",
      }}
    >
      {show && (
        <div className="mt-2">
          <Alert
            variant={showMsgType ?? "info"}
            onClose={() => setShow(false)}
            dismissible
          >
            <p>{showMsg}</p>
          </Alert>
        </div>
      )}
      <div className="headerPage row mx-0 justify-content-center mb-5">
        <div className="d-flex flex-row align-items-center">
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => DataRefreshCall("uk")}
          >
            Refresh UK Data
          </button>
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => DataRefreshCall("group")}
          >
            Refresh Group Data
          </button>
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={() => DataRefreshCall("all")}
          >
            Refresh Both Data
          </button>
        </div>
      </div>
      {props.modal && props.modal.isModal ? (
        <div
          className={`col-12 ${
            props?.endTime !== undefined
              ? "justify-content-between"
              : "justify-content-end"
          } d-flex footerModal px-3`}
        >
          {props?.endTime !== undefined ? (
            <span className="voda-bold align-center">{`Last Refresh : ${formatDateWithTime(
              props.endTime
            )}`}</span>
          ) : (
            ""
          )}
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            type="button"
            onClick={() => props.modal && props.modal.setRefreshTsrModal(false)}
          >
            Close
          </button>
        </div>
      ) : null}
    </div>
  );
};

export default TSRRefreshDataReport;
