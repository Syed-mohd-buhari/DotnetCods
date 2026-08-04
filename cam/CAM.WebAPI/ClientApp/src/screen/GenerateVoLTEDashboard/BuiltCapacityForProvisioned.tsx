import React, { useEffect, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { VolteKPIReportRow } from "../../Model/Report/ReportVolteKPIModel";

interface Props {
  data: VolteKPIReportRow[] | undefined;
}

const BuiltCapacityForProvisioned: React.FC<Props> = (props) => {
  const [data, setData] = useState<VolteKPIReportRow[] | undefined>([]);
  const [months, setMonths] = useState<string[]>([]);

  //CARICAMENTO INIZIALE
  useEffect(() => {
    setData(props.data);
  }, []);

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
  }, [props.data]);

  useEffect(() => {
    manageMonths();
  }, [data]);

  const manageMonths = () => {
    const listOfMonths: string[] = [];
    data?.forEach((el) => {
      el.monthValues?.forEach((item) => {
        item.monthYearLabel &&
          !listOfMonths.includes(item.monthYearLabel) &&
          listOfMonths.push(item.monthYearLabel);
      });
    });
    setMonths(listOfMonths);
  };

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 d-flex justify-content-center">
      <div className="col-12 mx-0 px-0 py-3">
        <table className="w-100 table-borderless">
          <thead>
            <tr className="intestazione">
              <th
                className="text-center customVolteKPIHead"
                style={{ fontSize: "14px", minWidth: 80 }}
                colSpan={3}
              ></th>
              <th
                className="customVolteKPIHead borderCustom custom-padding"
                style={{ fontSize: "14px" }}
                colSpan={3}
              >
                Q1
              </th>
              <th
                className="customVolteKPIHead borderCustom custom-padding"
                style={{ fontSize: "14px" }}
                colSpan={3}
              >
                Q2
              </th>
              <th
                className="customVolteKPIHead borderCustom custom-padding"
                style={{ fontSize: "14px" }}
                colSpan={3}
              >
                Q3
              </th>
              <th
                className="customVolteKPIHead borderCustom custom-padding"
                style={{ fontSize: "14px" }}
                colSpan={3}
              >
                Q4
              </th>
            </tr>
            <tr className="intestazione">
              <th colSpan={3} className="customVolteKPIHead ptb-15"></th>
              {months.map((el, idx) => (
                <th
                  key={el + idx}
                  className="customVolteKPIHead borderCustom text-center px-4 ptb-15"
                  style={{ fontSize: "14px" }}
                >
                  {el}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {data?.map((el, id) => (
              <tr
                className="dati"
                key={
                  el?.monthValues && el?.opCo && el?.monthValues + el?.opCo + id
                }
              >
                <td colSpan={3} className="dati text-center py-1">
                  {el.opCo}
                </td>
                {el.monthValues?.map((el, i) => (
                  <td
                    key={
                      el?.month && el?.quarter && el?.month + i + el?.quarter
                    }
                    className="dati text-center py-1"
                  >
                    {el.value}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default BuiltCapacityForProvisioned;
