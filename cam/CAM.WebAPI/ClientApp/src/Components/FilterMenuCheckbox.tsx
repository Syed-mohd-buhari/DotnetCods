import React, { useState, useEffect } from "react";
import { FilterValueDto } from "../Business/Common/CommonBusiness";
import { cancelDelayClose, delayClose, lowerFirstLetter } from "../Hook/Common";
import { useLocation, useNavigate } from "react-router-dom";

interface Props {
  action: {
    closeAll(): any;
    updateCount(property: string): string[];
    getFiltriAttivi(ids: string[], property: string): any;
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

const FilterMenuCheckbox: React.FC<Props> = (props) => {
  const [data, setData] = useState<FilterValueDto[] | undefined>([]);
  const [checkedFilter, setCheckedFilter] = useState<string[]>([]);
  const [isAllSelectorChecked, setIsAllSelectorChecked] =
    useState<boolean>(false);
  const [searchText, setSearchText] = useState<string>("");
  const inOrder: boolean =
    lowerFirstLetter(props.property) == props.propertyInOrder;
  const classNameAscending =
    inOrder && props.orderAscending ? "selectedOrder" : "";
  const classNameDescending =
    inOrder && !props.orderAscending ? "selectedOrder" : "";

  const location: any = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const dcfId = queryParams.get("dcfId");

  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    GetFilters();
  }, []);

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

  const GetFilters = async () => {
    setIsLoading(true);
    await props.action.getFilters(props.property, "");
    setIsLoading(false);
  };

  useEffect(() => {
    if (props.filterData != undefined && props.filterData != null) {
      setData(
        props.filterData.filter((x) => x.value !== null && x.text !== null)
      );
    }
  }, [props.filterData]);

  useEffect(() => {
    if (
      data?.filter((x) =>
        x.text?.toLowerCase().includes(searchText.toLowerCase())
      ).length == checkedFilter.length
    ) {
      setIsAllSelectorChecked(true);
    } else {
      setIsAllSelectorChecked(false);
    }
  }, [data, checkedFilter, searchText]);

  useEffect(() => {
    if (props.FiltriAttivi) {
      setCheckedFilter(props.FiltriAttivi);
    }
  }, [props.FiltriAttivi]);

  const handleChange = (id: string | undefined, event: any) => {
    const checked = event.target.checked;

    if (id === "ALL") {
      if (checked) {
        const allIdOfFilter = [] as string[];
        data
          ?.filter((x) =>
            x.text?.toLowerCase().includes(searchText.toLowerCase())
          )
          .map((filter) => {
            if (filter.value) {
              allIdOfFilter.push(filter.value);
            }
          });
        setCheckedFilter(allIdOfFilter);
      } else {
        setCheckedFilter([]);
      }
    } else {
      if (id !== null && id !== undefined) {
        let copy = [...checkedFilter] as string[];
        if (checked) {
          copy.push(id);
        } else {
          if (checkedFilter.includes(id)) {
            const idx = checkedFilter.indexOf(id);
            copy.splice(idx, 1);
          }
        }
        setCheckedFilter(copy);
      }
    }
  };

  const getResultOfCheckedFilters = () => {
    props.action.closeAll();
    props.action.getFiltriAttivi(
      checkedFilter,
      props.property !== "" && props.property
        ? props.property
        : props.overrideProperty
    );

    if (props.FiltriAttivi?.length === 0 && searchText.trim().length !== 0) {
      props.action.getFilters(props.property, searchText);
    }
  };

  return (
    <div
      className="filterMenu pb-2"
      onMouseEnter={cancelDelayClose}
      onMouseLeave={() => delayClose(props.action.closeAll, 3000)}
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
        />
      </div>
      {isLoading ? (
        <div className="col-12 row mx-auto px-2 mt-2 listaCheckbox">
          <img
            className="smallLoaderContainer mx-auto mt-2"
            alt="loader"
            src={require("../img/loader.gif")}
          />
        </div>
      ) : (
        <div className="col-12 row mx-0 px-2 mt-2 listaCheckbox">
          {data !== undefined &&
          data !== null &&
          data.filter((x) =>
            x.text?.toLowerCase().includes(searchText.toLowerCase())
          ).length !== 0 ? (
            <>
              <div
                className="w-100 d-flex mx-0 mt-1 filterCheckboxDiv"
                tabIndex={1}
              >
                <input
                  type="checkbox"
                  className="filterCheckbox mr-2"
                  checked={isAllSelectorChecked}
                  onChange={(e) => handleChange("ALL", e)}
                  placeholder="PLACE All"
                  id="selectAll"
                ></input>
                <label
                  htmlFor="selectAll"
                  className="voda-regular mb-0"
                  style={{ display: "inline" }}
                >
                  Select All
                </label>
              </div>

              {data &&
                data
                  ?.sort((a, b) =>
                    (a.text ?? "---").toLowerCase() >
                    (b.text ?? "---").toLowerCase()
                      ? 1
                      : -1
                  )
                  ?.filter((x) =>
                    (x.text ?? "---")
                      .toLowerCase()
                      ?.includes(searchText.toLowerCase())
                  )
                  .map((item, index) => {
                    const filtroAttivo =
                      checkedFilter.find((x) => x === item.value) != null
                        ? true
                        : false;
                    return item.text != "" &&
                      item.value != null &&
                      item.value != undefined ? (
                      <div
                        className="w-100 d-flex mx-0 mt-1 filterCheckboxDiv"
                        key={item.value + index + item.text}
                        tabIndex={1}
                      >
                        <input
                          type="checkbox"
                          className="filterCheckbox mr-2"
                          checked={filtroAttivo}
                          onChange={(e) => handleChange(item.value, e)}
                          placeholder="Search..."
                          id={item.value}
                        ></input>
                        <label
                          htmlFor={item.value}
                          className="voda-regular mb-0 text-l"
                          style={{ display: "inline" }}
                          dangerouslySetInnerHTML={{ __html: item.text }}
                        ></label>
                      </div>
                    ) : null;
                  })}
            </>
          ) : (
            <label>NO VALUES</label>
          )}
        </div>
      )}

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

export default FilterMenuCheckbox;
