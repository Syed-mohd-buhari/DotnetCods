import React, { SetStateAction, useEffect, useRef, useState } from "react";
import { toggleStateString } from "../../Hook/Common";
import Info from "./InfoTooltip";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import TourGuide from "../TourGuide";
import { endGuideTour, startGuideTour } from "../../Redux/Action/tourActions";
import { getSwBuildTourSteps } from "../../Constant/TourSteps";

interface Props {
  children?: any;
  propertyName: string;
  isVisibleFiltriString: string;
  spanClassName?: string;
  overridePropertyName?: string;
  hideFilter?: boolean;
  setupDuplicates?: boolean;
  apiType?: string;
  index?: number;
  action?: {
    checkFilter: (value: string) => boolean;
    resetFilter: (value: string) => void;
    settingVisibility: (value: SetStateAction<string>) => void;
  };
  freezeHeader?: boolean;
  renderChildren?: boolean;
}

let firstIndex, secondIndex, thirdIndex;

export const calculateWidths = (thRefs) => {
  if (thRefs.current) {
    const style = thRefs.current?.style;
    thRefs.current?.classList.remove("sticky-col");
    thRefs.current?.classList.remove("first-th");
    style.left = "";
    style.background = "";
    style.position = "";
    style.zIndex = "";
    style.borderRight = "";
    style.boxShadow = "";
    const totalGridElements =
      document.querySelectorAll<HTMLTableDataCellElement>(
        `thead > tr > th`
      ).length;
    if (
      thRefs.current?.cellIndex < 3 &&
      totalGridElements &&
      totalGridElements > 6
    ) {
      if (thRefs.current?.cellIndex === 0) {
        firstIndex = thRefs.current?.getBoundingClientRect().width;
        thRefs.current?.classList.add("sticky-col");
        style.background = "#bdbdc0";
        style.position = "sticky";
        style.zIndex = "1";
      }
      if (thRefs.current?.cellIndex === 1) {
        secondIndex = thRefs.current?.getBoundingClientRect().width;
        thRefs.current?.classList.add("sticky-col");
        style.left = `${firstIndex}px`;
        style.background = "#bdbdc0";
        style.position = "sticky";
        style.zIndex = "1";
      }
      if (thRefs.current?.cellIndex === 2) {
        thirdIndex = thRefs.current?.getBoundingClientRect().width;
        thRefs.current?.classList.add("sticky-col");
        style.left = `${firstIndex + secondIndex}px`;
        style.background = "#bdbdc0";
        style.position = "sticky";
        style.zIndex = "1";
        style.borderRight = "1px solid rgb(204, 204, 204)";
        style.boxShadow = "rgba(0, 0, 0, 0.1) 5px 0px 5px";
        style.filter = "drop-shadow(2px 0px 0px rgba(0,0,0,0.1))";
      }
    } else {
      thRefs.current?.classList.remove("sticky-col");
      thRefs.current?.classList.remove("first-th");
      style.left = "";
      style.background = "";
      style.position = "";
      style.zIndex = "";
      style.borderRight = "";
      style.boxShadow = "";
    }
  }
};
const TableCrudTh: React.FC<Props> = (props: any) => {
  const thRefs = useRef<HTMLTableCellElement | null>(null);

  useEffect(() => {
    calculateWidths(thRefs);
  }, [props]);

  // console.log(firstIndex, secondIndex, thirdIndex);
  const propertyName = props.overridePropertyName ?? props?.propertyName;

  const visibilityAction = () =>
    props.action.settingVisibility(
      toggleStateString(propertyName, props.isVisibleFiltriString)
    );

  const columNameFull =
    (LabelsDictionary[propertyName] && LabelsDictionary[propertyName].Full) ??
    "";

  const columNameShort =
    (LabelsDictionary[propertyName] && LabelsDictionary[propertyName].Short) ??
    propertyName;

  const columnDescription =
    (LabelsDictionary[props.propertyName] &&
      LabelsDictionary[props.propertyName].Description) ??
    "";

  const infoTooltipVisible =
    columnDescription != undefined ||
    (columnDescription != "" && columNameShort);

  return (
    <th
      key={props.propertyName}
      ref={props.freezeHeader ? thRefs : null}
      id={"tourGrid_" + props.propertyName}
    >
      {props.renderChildren ? (
        props.children
      ) : (
        <>
          <div className="h-100 d-flex align-items-center divFilter">
            {infoTooltipVisible ? (
              <Info
                spanClassName={props.spanClassName}
                hideFilter={props.hideFilter}
                columName={columNameFull}
                shortenedName={
                  props.overridePropertyName == null
                    ? columNameShort
                    : props.overridePropertyName
                }
                action={{ settingVisibility: visibilityAction }}
                tooltipText={columnDescription}
              />
            ) : (
              <label className={props.spanClassName}>{columNameFull}</label>
            )}
            {props.hideFilter === true ? null : props.action.checkFilter(
                props.propertyName
              ) ? (
              <button
                className="p-1 btn btn-link ml-1 resetFilter d-flex justify-content-center align-items-center"
                onClick={() =>
                  props.setupDuplicates == false
                    ? props.action.resetFilter(props.propertyName)
                    : null
                }
              >
                <img
                  title="Reset filter"
                  className="btnOrder2 mb-0"
                  src={require("../../img/resetFilter.png")}
                  alt="reset filter"
                />
              </button>
            ) : props.children === undefined ? null : (
              <div>
                <img
                  className={`btnOrder2 filterColor ${
                    props.isVisibleFiltriString === props.propertyName
                      ? "active"
                      : "inactive"
                  }`}
                  onClick={visibilityAction}
                  src={require("../../img/icon_filtter.png")}
                  alt="filter"
                  tabIndex={0}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      e.stopPropagation();
                      visibilityAction();
                    }
                  }}
                />
              </div>
            )}
          </div>

          {propertyName == props.isVisibleFiltriString ? props.children : null}
        </>
      )}
    </th>
  );
};
export default TableCrudTh;
