import React from "react";

import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/Toggle.css";

const ImplementationTable = (props) => {
  return (
    <div className="mt-3 row mx-0 col-12 d-flex justify-content-center">
      <div className="w-100">
        <table className="w-100">
          <thead>
            <tr className="intestazione">
              <th style={{ padding: "0 10px" }}>
                <div className="flex-row startFlex">
                  Opco
                  <span className="columFlex">
                    <img src={require("../../img/up.png")} className="w-8" />
                    <img src={require("../../img/down.png")} className="w-8" />
                  </span>
                </div>
              </th>

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
                  No. of Nodes
                  <span className="columFlex">
                    <img src={require("../../img/up.png")} className="w-8" />
                    <img src={require("../../img/down.png")} className="w-8" />
                  </span>
                </div>
              </th>
            </tr>
          </thead>
          <tbody>
            {props?.data?.opcosImplementations.map((item, index) => (
              <tr className="dati" key={index}>
                <td>{item?.opcoName}</td>
                <td
                  dangerouslySetInnerHTML={{
                    __html: item?.dcName ?? "---",
                  }}
                ></td>
                <td>{item?.productionNodesCount ?? 0}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ImplementationTable;
