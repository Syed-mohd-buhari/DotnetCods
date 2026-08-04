import React, { useState, useEffect } from "react";
import DatePicker from "react-datepicker";
import { formatTime, lowerFirstLetter } from "../Hook/Common";

interface Props {
  action: {
    closeAll(): any;
    setDateToChildren(data: any, property: string, target: string): any;
    orderBy(property: string, isAscending: boolean): any;
  };
  property: string;
  orderAscending?: boolean;
  propertyInOrder?: string;
  from: Date | undefined;
  to: Date | undefined;
}

const FilterMenuDateRange: React.FC<Props> = (props) => {
  const [min, setMin] = useState<string>("");
  const [max, setMax] = useState<string>("");
  const [from, setFrom] = useState<string>("");
  const [to, setTo] = useState<string>("");
  const inOrder: boolean =
    lowerFirstLetter(props.property) == props.propertyInOrder;
  const classNameAscending =
    inOrder && props.orderAscending ? "selectedOrder" : "";
  const classNameDescending =
    inOrder && !props.orderAscending ? "selectedOrder" : "";

  useEffect(() => {
    setFrom(formatTime(props.from) ?? "");
    setTo(formatTime(props.to) ?? "");
  }, []);

  useEffect(() => {
    setFrom(formatTime(props.from) ?? "");
    setMin(formatTime(props.from) ?? "");
  }, [props.from]);

  useEffect(() => {
    const tables = document.getElementsByTagName("table");

    for (let i = 0; i < tables.length; i++) {
      tables[i].addEventListener("scroll", props.action.closeAll);
    }

    return () => {
      for (let i = 0; i < tables.length; i++) {
        tables[i].removeEventListener("scroll", props.action.closeAll);
      }
    };
  }, [props.action]);

  useEffect(() => {
    setTo(formatTime(props.to) ?? "");
    setMax(formatTime(props.to) ?? "");
  }, [props.to]);

  const changeHandler = (e: any, target: string) => {
    // console.log("e => ", e);
    // const dateUpdated = new Date(e);
    // const DateString = ` ${dateUpdated.getFullYear()}/${
    //   dateUpdated.getMonth() + 1
    // }/${dateUpdated.getDate()}`;
    // //let date = e;
    // props.action.setDateToChildren(DateString, props.property, target);

    console.log("e => ", e);
    const dateUpdated = new Date(e);
    const DateString = ` ${dateUpdated.getFullYear()}/${
      dateUpdated.getMonth() + 1
    }/${dateUpdated.getDate()}`;
    if (target === "from") {
      setFrom(DateString);
    }

    if (target === "to") {
      setTo(DateString);
    }
  };

  const getResultOfCheckedFilters = () => {
    if (from !== "" && to !== "") {
      props.action.setDateToChildren(
        { from: from, to: to },
        props.property,
        "both"
      );
      return;
    }

    props.action.closeAll();
    if (from !== "" && from) {
      props.action.setDateToChildren(from, props.property, "from");
      return;
    }

    if (to !== "" && to) {
      props.action.setDateToChildren(to, props.property, "to");
      return;
    }
  };

  return (
    <div className="filterMenu pb-2">
      <div className="col-12 row mx-0 px-2 mt-2">
        <label className="w-100 filterMenu">ORDER</label>
        <div className="col-6 pl-0 pr-1">
          <button
            className={`w-100 btn btn-light btnOrderBy ${classNameAscending}`}
            type="button"
            onClick={() => props.action.orderBy(props.property, true)}
          >
            Ascending
          </button>
        </div>
        <div className="col-6 pr-0 pl-1">
          <button
            className={`w-100 btn btn-light btnOrderBy ${classNameDescending}`}
            type="button"
            onClick={() => props.action.orderBy(props.property, false)}
          >
            Descending
          </button>
        </div>
      </div>
      <div className="col-12 row mx-0 px-2 mt-2 picker">
        <label className="w-75 filterMenu">From</label>

        <input
          type="date"
          className="filterMenu w-100"
          placeholder="DD/MM/YYYY"
          defaultValue={from}
          // value={from}
          max={to}
          onChange={(e) => changeHandler(e.target.value, "from")}
        ></input>
      </div>
      <div className="col-12 row mx-0 px-2 mt-2 picker">
        <label className="w-75 filterMenu">Up To</label>
        <input
          type="date"
          className="filterMenu w-100"
          defaultValue={to}
          placeholder="DD/MM/YYYY"
          min={from}
          // value={to}
          onChange={(e) => changeHandler(e.target.value, "to")}
        ></input>
      </div>
      <div
        className="col-12 row mx-0 px-2 mt-2 pt-2"
        style={{ borderTop: "1px solid #A4A4A4" }}
      >
        <div className="col-6 pl-0 pr-1">
          <button
            className={`w-100 btn btn-light btnConfirmCancelFilters `}
            style={{ border: "1px solid #A4A4A4" }}
            onClick={() => props.action.closeAll()}
          >
            Close
          </button>
        </div>
        <div className="col-6 pr-0 pl-1">
          <button
            className={`w-100 btn btn-light btnConfirmCancelFilters `}
            style={{ background: "#dd0000", color: "#fff", fontWeight: "bold" }}
            onClick={() => getResultOfCheckedFilters()}
          >
            OK
          </button>
        </div>
      </div>
    </div>
  );
};

export default FilterMenuDateRange;
