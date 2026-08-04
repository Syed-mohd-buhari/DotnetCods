import React, { useState } from "react";

import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/Toggle.css";

const DCS = (props) => {
  const sortData = (type: string) => {
    if (type === "Acc") {
    }

    if (type === "Dec") {
    }
  };

  return (
    <div className="mt-3 row mx-0 col-12 d-flex justify-content-center">
      <div className="w-100">
        <table className="w-100">
          <thead>
            <tr className="intestazione">
              <th style={{ padding: "0 10px" }}>
                <div className="flex-row startFlex">
                  Design Component
                  <span className="columFlex">
                    <img src={require("../../img/up.png")} className="w-8" />
                    <img src={require("../../img/down.png")} className="w-8" />
                  </span>
                </div>
              </th>
              <th style={{ padding: "0 10px" }}>
                <div className="flex-row startFlex">
                  Vodafone Name
                  <span className="columFlex">
                    <img src={require("../../img/up.png")} className="w-8" />
                    <img src={require("../../img/down.png")} className="w-8" />
                  </span>
                </div>
              </th>
            </tr>
          </thead>
          <tbody>
            {props?.data?.dcs.map((item, index) => (
              <tr className="dati" key={index}>
                <td
                  dangerouslySetInnerHTML={{
                    __html: item?.dcName ?? "---",
                  }}
                ></td>
                <td>{item?.vodafoneName}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default DCS;
