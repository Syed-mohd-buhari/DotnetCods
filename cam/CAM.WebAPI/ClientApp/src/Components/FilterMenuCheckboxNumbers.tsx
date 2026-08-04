import React, { useState, useEffect } from "react";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { cancelDelayClose, delayClose, lowerFirstLetter } from "../Hook/Common";
interface Props {
  action: {
    closeAll(): any;
    updateCount(property: string): string[];
    getFiltriAttivi(id: string, checked: boolean, property: string): any;
    orderBy(property: string, isAscending: boolean): any;
    getFilters(property: string, text: string): any;
  };
  filterData?: FilterValueDto[];
  property: string;
  overrideProperty: string;
  count: string[];
  orderAscending?: boolean;
  propertyInOrder?: string;
  FiltriAttivi: any[] | undefined;
}

const FilterMenuCheckboxNumbers: React.FC<Props> = (props) => {
  const inOrder: boolean =
    lowerFirstLetter(props.property) == props.propertyInOrder;
  const classNameAscending =
    inOrder && props.orderAscending ? "selectedOrder" : "";
  const classNameDescending =
    inOrder && !props.orderAscending ? "selectedOrder" : "";

  const [data, setData] = useState<FilterValueDto[] | undefined>([]);
  const [searchText, setSearchText] = useState<string>("");

  useEffect(() => {
    props.action.getFilters(props.property, "");
  }, []);

  useEffect(() => {
    if (props.filterData != undefined) {
      setData(props.filterData);
    }
  }, [props.filterData]);

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

  function _filtraFiltri(text: string) {
    setSearchText(text);
    props.action.getFilters(props.property, text);
    if (props.count.filter((x) => x == props.property).length < 1) {
      props.action.updateCount(props.property);
    }
  }

  const _showAll = () => {
    props.action.getFilters(props.property, "");
    props.action.updateCount(props.property);
  };

  const handleChange = (id: string | undefined, event: any) => {
    const checked = event.target.checked;
    id &&
      props.action.getFiltriAttivi(
        id,
        checked,
        props.overrideProperty == "" ? props.property : props.overrideProperty
      );

    if (props.FiltriAttivi?.length === 0) {
      props.action.getFilters(props.property, searchText);
    }
  };

  return (
    <div
      className="filterMenu pb-2"
      onMouseEnter={cancelDelayClose}
      onMouseLeave={() => delayClose(props.action.closeAll)}
    >
      <div className="col-12 row mx-0 px-2 mt-2">
        <label className="w-100 filterMenu text-l">ORDER</label>
        <div className="col-6 pl-0 pr-1">
          <button
            className={`w-100 btn btn-light btnOrderBy ${classNameAscending}`}
            onClick={() => props.action.orderBy(props.property, true)}
          >
            Ascending (A-Z)
          </button>
        </div>
        <div className="col-6 pr-0 pl-1">
          <button
            className={`w-100 btn btn-light btnOrderBy ${classNameDescending}`}
            onClick={() => props.action.orderBy(props.property, false)}
          >
            Descending (Z-A)
          </button>
        </div>
      </div>
      <div className="col-12 row mx-0 px-2 mt-2">
        <label className="w-100 filterMenu text-l">FILTERS</label>
        <input
          type="text"
          className="filterMenu w-100"
          placeholder="Search..."
          onChange={(e) => setSearchText(e.target.value)}
        ></input>
      </div>
      <div className="col-12 row mx-0 px-2 mt-2 listaCheckbox">
        {data != undefined && data !== null && data.length !== 0 ? (
          data
            .sort(
              (a, b) => parseFloat(a.text ?? "0") - parseFloat(b.text ?? "0")
            )
            .filter((x) =>
              x.text.toLowerCase().includes(searchText.toLowerCase())
            )
            .map((item, index) => {
              const filtroAttivo =
                props.FiltriAttivi &&
                props.FiltriAttivi.find((x) => x == item.value) != null
                  ? true
                  : false;
              return item.text != "" && item.value != null ? (
                <div
                  className="w-100 d-flex flex-row mx-0 mt-1 filterCheckboxDiv"
                  key={item.value}
                  tabIndex={1}
                >
                  <input
                    type="checkbox"
                    className="filterCheckbox mr-2"
                    defaultChecked={filtroAttivo}
                    onChange={(e) => handleChange(item.value, e)}
                    placeholder="Search..."
                    id={item.value}
                  ></input>
                  <label
                    htmlFor={item.value}
                    className="voda-regular mb-0 text-l"
                    style={{ display: "inline" }}
                  >
                    {item.text}
                  </label>
                </div>
              ) : null;
            })
        ) : (
          <label>NO VALUES</label>
        )}
      </div>
      {/* <div className="col-12 row mx-0 d-flex justify-content-center">
                {props.count.filter(x => x == props.property).length > 0
                    ? null
                    : <button onClick={() => _showAll()} type="button" className="btn btn-link btnMore">More..</button>
                }
            </div> */}
    </div>
  );
};

export default FilterMenuCheckboxNumbers;
