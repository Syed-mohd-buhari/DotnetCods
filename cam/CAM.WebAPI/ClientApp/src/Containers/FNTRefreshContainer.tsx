import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { GetTSRReport } from "../Redux/Action/TSRReport/TSRReportDownloadAction";
import { GetFNTRefresh } from "../Redux/Action/FNTReport/TemsFNTReportImportAction";
import { Alert } from "react-bootstrap";
import { formatDateWithTime } from "../Hook/Common";
import { DropdownInputComponent } from "../Components/FormField";

interface Props {
  endTime?: string | undefined;
  redirect?: string;
  modal?: {
    isModal: boolean | false;
    setRefreshFntModal(flag: boolean): any;
    closeModalSetup(changed?: boolean): any;
    refreshFntMsg?(obj: any): any;
  };
  opcoRes?: any;
  opcoLastTime?: any;
}

const FNTRefreshDataReport = (props: Props) => {
  const [show, setShow] = useState(false);
  const [showMsg, setShowMsg] = useState("");
  const [selectedOpco, setSelectedOpco] = useState<any>(null);
  const [showMsgType, setShowMsgType] = useState("");
  const dataRefreshCall = async (type: string) => {
    const payload = {
      LocationOfHardwareAsset: selectedOpco ? [selectedOpco?.key] : [],
    };
    let result = await GetFNTRefresh(payload);
    if (result) {
      props.modal &&
        props.modal.refreshFntMsg &&
        props.modal.refreshFntMsg(result);
      props.modal && props.modal.closeModalSetup(true);
    }
  };
  return (
    <div
      className={`${
        props?.redirect === "fntReport" ? "mt-3" : "pageContainer"
      }`}
      style={{
        height: props?.redirect !== "fntReport" ? "20rem" : "",
        alignContent: "center",
      }}
    >
      <div className="d-flex mx-3">
        <div className="col-3 pl-0 mt-2">
          <DropdownInputComponent
            label={"Please Select Opco"}
            placeholderText="Select"
            labelCSS="mb-0 text-left"
            inputCSS="labelForm voda-bold mb-2"
            isSearchable={true}
            isClearable={true}
            value={selectedOpco ?? null}
            onChange={(e: any) => setSelectedOpco(e)}
            options={props?.opcoRes ?? []}
          />
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
            <span className="voda-bold align-center">{`${
              props?.opcoLastTime?.filter(
                (res) => res?.key === selectedOpco?.key
              )?.[0]
                ? "Last Refresh :  " +
                  formatDateWithTime(
                    props?.opcoLastTime?.filter(
                      (res) => res?.key === selectedOpco?.key
                    )?.[0]?.value
                  )
                : ""
            }`}</span>
          ) : (
            ""
          )}
          <div className="d-flex ">
            <button
              className="download-to-excel mrl-10 grid-main-btn"
              onClick={() => dataRefreshCall("fnt")}
            >
              Refresh FNT Data
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={() =>
                props.modal && props.modal.setRefreshFntModal(false)
              }
            >
              Close
            </button>
          </div>
        </div>
      ) : null}
    </div>
  );
};

export default FNTRefreshDataReport;
