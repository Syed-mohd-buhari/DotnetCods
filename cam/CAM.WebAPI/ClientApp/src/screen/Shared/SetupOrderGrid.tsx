import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  RenderDetail,
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../../Model/Common";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import ModalConfirm from "../../Components/ModalConfirm";
import { SelectGridType } from "../../Hook/CommonRenderGrid/GridRender";

interface Props {
  action: {
    SetChangedOrder(changed: boolean): any;
    SetDataOrder(data: { id: number; order: number }[]): any;
    setIsSetupOrder(value: boolean): any;
    SetIsConfirmOrder(value: boolean): any;
  };
  renderGrid: CustomGridRender | undefined;
  propertyOrder: string;
  data: Array<any> | undefined;
  isConfirmOrder: boolean;
  id?: string;
}

const SetupOrderGrid: React.FC<Props> = (props) => {
  const [renderGrid, setRenderGrid] = useState<RenderDetail[]>([]);
  const [data, setData] = useState<Array<any>>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);

  useEffect(() => {
    props.renderGrid && setRenderGrid(props.renderGrid.render);
    props.data && setData(props.data);
  }, []);

  useEffect(() => {
    if (props.isConfirmOrder) {
      setConfirm({
        title: "Cancel",
        message: "Are you sure you want to exit? unSaved change will be lost",
        button: "Exit",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => {
            setConfirm(stateConfirm);
            props.action.SetIsConfirmOrder(false);
          },
          confirm: () => {
            props.action.setIsSetupOrder(false);
            props.action.SetIsConfirmOrder(false);
          },
        },
      });
    }
  }, [props.isConfirmOrder]);

  const upIndex = (value: number | undefined) => {
    props.action.SetChangedOrder(true);
    let copy = [...data] as Array<any>;
    let index = copy.findIndex((x) => x[props.propertyOrder] === value);
    if (index !== -1) {
      let indexPre = copy.findIndex(
        (x) => x[props.propertyOrder] === copy[index][props.propertyOrder] - 1
      );
      copy[indexPre][props.propertyOrder] = copy[index][props.propertyOrder];
      copy[index][props.propertyOrder] = copy[index][props.propertyOrder] - 1;
      setData(copy);
    }
    rtnDataArray(copy);
    return;
  };

  const downIndex = (value: string | undefined) => {
    props.action.SetChangedOrder(true);
    let copy = [...data] as Array<any>;
    let index = copy.findIndex((x) => x[props.propertyOrder] === value);
    if (index !== -1) {
      let indexPre = copy.findIndex(
        (x) => x[props.propertyOrder] === copy[index][props.propertyOrder] + 1
      );
      copy[indexPre][props.propertyOrder] = copy[index][props.propertyOrder];
      copy[index][props.propertyOrder] = copy[index][props.propertyOrder] + 1;
      setData(copy);
    }
    rtnDataArray(copy);
    return;
  };

  const rtnDataArray = (data: Array<any>) => {
    let array = [] as { id: number; order: number }[];
    data.map((item, i) => {
      array.push({
        id: props.id != undefined ? item[props.id] : item.id,
        order: item[props.propertyOrder],
      });
    });
    props.action.SetDataOrder(array);
    return;
  };

  return (
    <div className="listaApparatiContainer row mx-0 col-12 p-0 d-flex justify-content-center">
      <ModalConfirm data={confirm} />
      <div className="mx-0 px-0 " style={{ overflow: "scroll" }}>
        <table className="w-100" tabIndex={-1}>
          <thead>
            <tr className="intestazione">
              {renderGrid
                .sort((a, b) => a.order - b.order)
                .filter((x) => x.show)
                .map((item, i) => (
                  <th className=" " key={item.propertyName}>
                    <div className="h-100 d-flex flex-row align-items-center divFilter">
                      <label className="mb-0 ml-1 p-1">
                        {(LabelsDictionary[item.propertyName] &&
                          LabelsDictionary[item.propertyName].Short) ??
                          item.propertyName}
                      </label>
                    </div>
                  </th>
                ))}
              {renderGrid && renderGrid.length ? (
                <th className="customWidth"></th>
              ) : (
                ""
              )}
            </tr>
          </thead>
          <tbody>
            {data &&
              data
                .sort((a, b) =>
                  (a[props.propertyOrder] ?? 0) < (b[props.propertyOrder] ?? 0)
                    ? -1
                    : 0
                )
                .map((item, i) => (
                  <tr className="dati" key={item.id}>
                    {renderGrid
                      .sort((a, b) => a.order - b.order)
                      .filter((x) => x.show)
                      .map((td, i) =>
                        SelectGridType(
                          item[td.propertyName],
                          td.propertyName,
                          td.type
                        )
                      )}
                    <td className=" ">
                      <div className="d-flex flex-row justify-content-end">
                        <div className="d-flex flex-column">
                          <button
                            type="button"
                            className="btn btn-link h-50 py-1 chevron"
                            disabled={item[props.propertyOrder] === 1}
                            onClick={() => upIndex(item[props.propertyOrder])}
                          >
                            <img
                              className="btnEdit"
                              src={require("../../img/up.png")}
                              alt="up"
                            />
                          </button>
                          <button
                            type="button"
                            className="btn btn-link h-50 py-1 chevron"
                            disabled={item[props.propertyOrder] === data.length}
                            onClick={() => downIndex(item[props.propertyOrder])}
                          >
                            <img
                              className="btnEdit"
                              src={require("../../img/down.png")}
                              alt="down"
                            />
                          </button>
                        </div>
                      </div>
                    </td>
                  </tr>
                ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default SetupOrderGrid;
