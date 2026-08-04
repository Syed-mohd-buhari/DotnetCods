import React from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { GetBPTPOCExport } from "../Redux/Action/BPTPOC/BPTPOCDownloadAction";

const BPTPOC = () => {
  const DownloadReport = async (flag: any) => {
    let result = await GetBPTPOCExport(flag);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };
  return (
    <div className="pageContainer">
      <div className="headerPage row mx-0 justify-content-between position-relative pt-2">
        <div>
          <h3 className="voda-bold text-left">BPT POC</h3>
        </div>
      </div>
      <div>
        <div className="d-flex justify-content-center">
          <button
            className="download-to-excel mrl-10"
            onClick={() => DownloadReport(1)}
          >
            Legacy File
          </button>
          <button
            className="download-to-excel mrl-10"
            onClick={() => DownloadReport(2)}
          >
            Non Legacy File
          </button>
          <button
            className="download-to-excel mrl-10"
            onClick={() => DownloadReport(3)}
          >
            Client File
          </button>
        </div>
      </div>
    </div>
  );
};

export default BPTPOC;
