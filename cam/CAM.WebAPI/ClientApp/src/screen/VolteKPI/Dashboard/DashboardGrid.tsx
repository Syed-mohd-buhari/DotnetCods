import React, { SetStateAction } from "react";
import { useSelector } from "react-redux";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/VolteKPI.css";
import { RenderDetail } from "../../../Model/Common";
import {
  VolteKPIDtoGrid,
  VolteKPIQueryObjectGrid,
} from "../../../Model/VolteKpi/VolteKPI";
import { RootState } from "../../../Redux/Store/rootStore";

interface Props {
  action: {
    openModalEdit(idKPIType: number): any;
    Delete(id: number | undefined, apiType?: string, orphan?: boolean): any;
    Edit(parameters: VolteKPIQueryObjectGrid): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<VolteKPIQueryObjectGrid> | undefined): any;
  };
  orphanColor?: boolean;
  renderGrid: RenderDetail[];
  pagination: VolteKPIQueryObjectGrid | undefined;
}

const DashboardGrid: React.FC<Props> = (props) => {
  const Grid = (state: RootState) =>
    state.volteKPIGridReducer.VolteKPIGridResult;
  const gridDto = useSelector(Grid);

  const onEdit = async (idKPI, idKPIType) => {
    const parameters = {} as VolteKPIQueryObjectGrid;
    parameters.volteKPIId = [idKPI];
    await props.action.Edit(parameters);
    props.action.openModalEdit(idKPIType);
  };

  //CHIAMATA AL PARENT AL CAMBIO FILTRI
  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 tableOverflow p-0 mb-3">
      <div className="mx-0 px-0 col-12">
        <table className="table table-borderless mb-1">
          <thead>
            <tr className="intestazione">
              <th className="customVolteKPIHead text-center">
                <div className="h-100 d-flex flex-row align-items-center divFilter">
                  <label className="mx-auto">Program</label>
                </div>
              </th>
              {gridDto?.items &&
                gridDto?.items[0]?.opCoColumns?.map((item, i) => (
                  <th
                    className="  customVolteKPIHead text-center borderCustom minWidthKPI"
                    key={item + i}
                  >
                    <div className="h-100 d-flex flex-row align-items-center justify-content-center divFilter  ">
                      <label>{item}</label>
                    </div>
                  </th>
                ))}
              <th className=" ">
                <div className="divFilter reports "></div>
              </th>
            </tr>
          </thead>
          <tbody>
            {gridDto?.items &&
              gridDto?.items?.map((item, index) => (
                <tr className="dati" key={`HW${index}`}>
                  <td className="  borderCustom " key={`HW${index}${index}`}>
                    {item.program}
                  </td>
                  {item.opCoList?.map((el, idx) => (
                    <td
                      key={Math.random() + idx}
                      className="  text-center borderCustom"
                      style={{
                        backgroundColor: el.backgroundColor,
                        color: "white",
                        cursor: `${item.type !== 4 && "pointer"}`,
                      }}
                      onClick={() => {
                        item.type !== 4 &&
                          onEdit(el.volteKPIId, el.volteKPIType);
                      }}
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

export default DashboardGrid;
