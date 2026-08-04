import React from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/Toggle.css";

const ImplementationStatusTable = (props) => {
  return (
    <>
      <div className="mt-4 row mx-0 col-12 d-flex justify-content-center">
        <div className="w-100">
          <table className="w-100">
            <thead>
              <tr className="intestazione">
                <th style={{ padding: "0 10px" }}>
                  <div className="flex-row startFlex">
                    Opco
                    {/* <span className="columFlex">
                      <img src={require("../../img/up.png")} className="w-8" />
                      <img
                        src={require("../../img/down.png")}
                        className="w-8"
                      />
                    </span> */}
                  </div>
                </th>

                <th style={{ padding: "0 10px" }}>
                  <div className="flex-row startFlex">
                    Design Component
                    {/* <span className="columFlex">
                      <img src={require("../../img/up.png")} className="w-8" />
                      <img
                        src={require("../../img/down.png")}
                        className="w-8"
                      />
                    </span> */}
                  </div>
                </th>
                <th style={{ padding: "0 10px" }}>
                  <div className="flex-row startFlex">
                    Node Count Approach
                    {/* <span className="columFlex">
                      <img src={require("../../img/up.png")} className="w-8" />
                      <img
                        src={require("../../img/down.png")}
                        className="w-8"
                      />
                    </span> */}
                  </div>
                </th>

                <th style={{ padding: "0 10px" }}>
                  <div className="flex-row startFlex">
                    No. of Nodes
                    {/* <span className="columFlex">
                      <img src={require("../../img/up.png")} className="w-8" />
                      <img
                        src={require("../../img/down.png")}
                        className="w-8"
                      />
                    </span> */}
                  </div>
                </th>

                <th style={{ padding: "0 10px" }}>
                  <div className="flex-row startFlex">
                    Network Elements
                    {/* <span className="columFlex">
                      <img src={require("../../img/up.png")} className="w-8" />
                      <img
                        src={require("../../img/down.png")}
                        className="w-8"
                      />
                    </span> */}
                  </div>
                </th>

                <th style={{ padding: "0 10px" }}>
                  <div className="flex-row startFlex">
                    Location
                    {/* <span className="columFlex">
                      <img src={require("../../img/up.png")} className="w-8" />
                      <img
                        src={require("../../img/down.png")}
                        className="w-8"
                      />
                    </span> */}
                  </div>
                </th>

                <th style={{ padding: "0 10px" }}>
                  <div className="flex-row startFlex">
                    Environment
                    {/* <span className="columFlex">
                      <img src={require("../../img/up.png")} className="w-8" />
                      <img
                        src={require("../../img/down.png")}
                        className="w-8"
                      />
                    </span> */}
                  </div>
                </th>
              </tr>
            </thead>
            <tbody>
              {props?.data?.map((item, index) => (
                <tr className="dati" key={index}>
                  <td>{item?.opcoName}</td>
                  <td
                    dangerouslySetInnerHTML={{
                      __html: item?.dcName ?? "---",
                    }}
                  ></td>
                  <td>{item?.nodeCountApproach}</td>
                  <td>
                    {item?.nodeCountApproach == "Element Count" &&
                    item?.numberofNodes == 0
                      ? "---"
                      : item?.numberofNodes ?? "---"}
                  </td>
                  <td>{item?.networkELement ?? "---"}</td>
                  <td>{item?.location ?? "---"}</td>
                  <td>{item?.enviroment ?? "---"}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
};

export default ImplementationStatusTable;
