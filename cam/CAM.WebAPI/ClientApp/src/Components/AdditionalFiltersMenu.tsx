import React, { useEffect, useState } from "react";
import "../Css/NavBar.css";
import "../Css/index.css";
import { QueryObjectGrid } from "../Model/Common";
import { cancelDelayClose, delayClose } from "../Hook/Common";
import setLoader from "../Redux/Action/LoaderAction";

export interface Props {
  query: QueryObjectGrid;
  orphanColored?: boolean;
  orphanRemoved?: boolean;
  action: {
    setQuery(query: QueryObjectGrid): any;
    setIsVisible(value: boolean): any;
    getGrid(query: QueryObjectGrid): any;
    setOrphanColor?(value: boolean): any;
  };
}

const AdditionalFiltersMenu: React.FC<Props> = (props) => {
  //const { tipologicaPermesso } = useAuth();
  const setAdditionalFilters = (property: string, checked: boolean) => {
    let copy = { ...props.query } as QueryObjectGrid;
    copy[property] = checked;
    // props.action.getGrid(copy).then(x => {   })
    setLoader("ADD", "getGrid");
    props.action.getGrid(copy).then((x) => {
      setLoader("REMOVE", "getGrid");
    });
    props.action.setQuery(copy);
  };

  return (
    <div
      className="filterMenuOrphans pl-3 pt-2"
      onMouseEnter={cancelDelayClose}
      onMouseLeave={() =>
        delayClose(() => props.action.setIsVisible(false), 2000)
      }
    >
      {/* <label className="w-100 filterMenu">Data Filters</label> */}
      <div className="form-group">
        {props.orphanRemoved !== true ? (
          <label className="w-100 d-flex align-items-center fz-16 gray-light">
            <input
              type="checkbox"
              onChange={(e) => setAdditionalFilters("orphan", e.target.checked)}
              checked={props.query.orphan}
              className="inputForm checkboxList mb-1 mr-1"
            />
            Filter on Orphans
          </label>
        ) : null}
        {/* {tipologicaPermesso && (
					<label className="labelForm voda-bold   w-100 d-flex align-items-center">
						<input type="checkbox" onChange={(e) => setAdditionalFilters("deleted", e.target.checked)} checked={props.query.deleted} className="inputForm checkboxList mb-1 mr-1" />
						Deleted
					</label>
				)} */}
      </div>
      {props.action.setOrphanColor && props.orphanRemoved !== true ? (
        <>
          {/* <label className="w-100 filterMenu  ">Visual Filters</label> */}
          <div className="form-group">
            <label className="w-100 d-flex align-items-center fz-16 gray-light">
              <input
                type="checkbox"
                onChange={(e) =>
                  props.action.setOrphanColor &&
                  props.action.setOrphanColor(e.target.checked)
                }
                checked={props.orphanColored}
                className="inputForm checkboxList mb-1 mr-1"
              />
              Highlight Orphans
            </label>
          </div>
        </>
      ) : null}
    </div>
  );
};

export default AdditionalFiltersMenu;
