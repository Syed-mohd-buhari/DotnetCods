import React, { useEffect, useRef, useState } from "react";
import { useSelector } from "react-redux";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { SelectFilterType, SelectGridType } from "../../Hook/CommonRenderGrid/GridRender";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import { RootState } from "../../Redux/Store/rootStore";
import { RenderDetail, QueryObjectGrid } from "../../Model/Common";
import { NFVICCompatibilityDtoGrid, NFVICCompatibilityQueryDto } from "../../Model/NfvicCompatible";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";



interface Props {
    data: NFVICCompatibilityDtoGrid[] | undefined;
    renderGrid: RenderDetail[];
  }
  
  const NFVICCompatibilityGrid: React.FC<Props> = ({ data, renderGrid }) => {
    const thRefs = useRef<Array<HTMLTableCellElement | null>>([]);
    const setThRef = (ref: HTMLTableCellElement | null, index: number) => {
      thRefs.current[index] = ref;
      calculateBodyWidths(thRefs, 0, 1, 2);
    };
  
    const visibleColumns = renderGrid
      .filter((col) => col.show)
      .sort((a, b) => a.order - b.order);
  
    useEffect(() => {
      calculateBodyWidths(thRefs, 0, 1, 2);
    }, [data]);
  
    return (
      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 flex-row" style={{ position: "relative" }}>
          <table className="table-responsive" tabIndex={-1}>
            <thead>
              <tr className="intestazione">
                {visibleColumns.map((col, index) => (
                  <th
                    key={index}
                    ref={(ref) => setThRef(ref, index)}
                    className="text-center"
                  >
                   {LabelsDictionary[col.propertyName]?.Full}
                    {console.log(LabelsDictionary[col.propertyName]?.Full)}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {data?.map((row, rowIndex) => (
                <tr key={rowIndex} className="dati">
                  {visibleColumns.map((col, colIndex) =>
                    SelectGridType(
                      row[col.propertyName as keyof NFVICCompatibilityDtoGrid],
                      col.propertyName,
                      col.type,
                      "",
                      undefined,
                      undefined,
                      undefined,
                      colIndex,
                      thRefs,
                      setThRef
                    )
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    );
  };
  
  export default NFVICCompatibilityGrid;
