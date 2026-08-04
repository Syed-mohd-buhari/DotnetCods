import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import FilterMenuOnlyOrder from "../../Components/FilterMenuOnlyOrder";
import {
  toggleState,
  filterObjectArrayWithObjectArray,
} from "../../Hook/Common";
import { MajorHardwareBuildMainSystemTypeDto } from "../../Model/SystemTypeModel";
import { GetMajorHardwareBuildGrid } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildGridAction";
import {
  MajorHardwareBuildQueryObjectGrid,
  MajorHardwareBuildDtoGrid,
} from "../../Model/MajorHardwareBuild";

interface Props {
  data: MajorHardwareBuildMainSystemTypeDto[];
  resource: { key: number; value: string }[] | null | undefined;
  action: {
    GetMajorHardwareFromList(items: MajorHardwareBuildMainSystemTypeDto[]): any;
  };
}

const MajorHardwareThirdPartyComponent: React.FC<Props> = (props) => {
  const [isVisibleFiltri, setIsVisibleFiltri] = useState(0);

  const closeAll = () => {
    setIsVisibleFiltri(0);
  };

  const [dataIds, setDataIds] = useState<MajorHardwareBuildMainSystemTypeDto[]>(
    []
  );
  const [dataReload, setDataReload] =
    useState<
      { key: number; value: MajorHardwareBuildDtoGrid; isMain: boolean }[]
    >();
  const [filtriAttivi, setFiltriAttivi] =
    useState<MajorHardwareBuildQueryObjectGrid>();
  const [searchData, setSearchData] = useState<
    { key: number; value: MajorHardwareBuildDtoGrid }[]
  >([]);
  const [searchText, setSearchText] = useState("");
  const [toAssociateState, setToAssociateState] = useState<
    MajorHardwareBuildDtoGrid[]
  >([]);

  useEffect(() => {
    setDataIds(props.data);
  }, []);

  useEffect(() => {
    let ids: (number | undefined)[] =
      props.data?.map((x) => x.majorHardwareBuildId) ?? ([] as number[]);

    // GetMajorHardwareBuildGrid({ majorHardwareBuildId: ids, sortBy: filtriAttivi?.sortBy, isSortAscending: filtriAttivi?.isSortAscending } as MajorHardwareBuildQueryObjectGrid).then(result => {
    //     let dataRes = result.MajorHardwareBuildGridResult?.items?.map(s => {
    //         return { key: s.majorHardwareBuildId, value: s, isMain: props.data.find(x => x.majorHardwareBuildId == s.majorHardwareBuildId)?.isMain } as { key: number, value: MajorHardwareBuildDtoGrid, isMain: boolean }
    //     });
    //     setDataIds(props.data)
    //     setDataReload(dataRes);
    // })
  }, [props.data]);

  useEffect(() => {
    // GetMajorHardwareBuildGrid({ name: searchText, sortBy: filtriAttivi?.sortBy, isSortAscending: filtriAttivi?.isSortAscending } as MajorHardwareBuildQueryObjectGrid).then(result => {
    //     let dataRes = result.MajorHardwareBuildGridResult?.items?.map(s => {
    //         return { key: s.majorHardwareBuildId, value: s, isMain: props.data.find(x => x.majorHardwareBuildId == s.majorHardwareBuildId)?.isMain } as { key: number, value: MajorHardwareBuildDtoGrid, isMain: boolean }
    //     });
    //     if (dataRes != undefined && dataReload != undefined) {
    //         setSearchData(filterObjectArrayWithObjectArray(dataRes, dataReload));
    //     }
    // });
  }, [searchText]);

  const addRemove = (checked: boolean, item: MajorHardwareBuildDtoGrid) => {
    let copy = [...toAssociateState];

    if (!checked) {
      copy.push(item);
      setToAssociateState(copy);
    } else {
      const index = copy.findIndex(
        (x) => x.majorHardwareBuildId == item.majorHardwareBuildId
      );
      if (index !== -1) {
        copy.splice(index, 1);
      }
    }
    setToAssociateState(copy);
  };

  const associate = () => {
    if (dataReload != null && dataReload != undefined) {
      let copy = [...dataIds];
      let copyReload = [...dataReload];

      let toAssociate = toAssociateState.map((x) => {
        return {
          majorHardwareBuildId: x.majorHardwareBuildId,
          isMain: false,
        } as MajorHardwareBuildMainSystemTypeDto;
      });
      let toAssociateReload = toAssociateState.map((x) => {
        return { value: x, key: x.majorHardwareBuildId, isMain: false } as {
          key: number;
          value: MajorHardwareBuildDtoGrid;
          isMain: boolean;
        };
      });

      copy = copy.concat(toAssociate);

      copyReload = copyReload.concat(toAssociateReload);

      setDataIds(copy);

      setDataReload(copyReload);

      props.action.GetMajorHardwareFromList(copy);
    }

    if (toAssociateState != null && toAssociateState != undefined) {
      const associateCopy = [...toAssociateState];

      associateCopy.splice(0, associateCopy.length);
      setToAssociateState(associateCopy);
    }
  };

  const remove = (id: number) => {
    const copy = [...dataIds];
    const index = copy.findIndex((x) => x.majorHardwareBuildId == id);
    if (index !== -1) {
      copy.splice(index, 1);
    }
    setDataIds(copy);
    props.action.GetMajorHardwareFromList(copy);
  };

  useEffect(() => {
    let ids: (number | undefined)[] =
      props.data?.map((x) => x.majorHardwareBuildId) ?? ([] as number[]);

    // GetMajorHardwareBuildGrid({ majorHardwareBuildId: ids, sortBy: filtriAttivi?.sortBy, isSortAscending: filtriAttivi?.isSortAscending } as MajorHardwareBuildQueryObjectGrid).then(result => {
    //     let dataRes = result.MajorHardwareBuildGridResult?.items?.map(s => {
    //         return { key: s.majorHardwareBuildId, value: s, isMain: props.data.find(x => x.majorHardwareBuildId == s.majorHardwareBuildId)?.isMain } as { key: number, value: MajorHardwareBuildDtoGrid, isMain: boolean }
    //     });
    //     setDataIds(props.data)
    //     setDataReload(dataRes);
    // })
  }, [filtriAttivi]);

  const orderBy = (property: string, isAscending: boolean) => {
    let copy = { ...filtriAttivi } as MajorHardwareBuildQueryObjectGrid;
    copy.sortBy = property;
    copy.isSortAscending = isAscending;
    setFiltriAttivi(copy);
  };

  return (
    <div className="listaApparatiContainer mt-2 row mx-0 col-12 p-0 d-flex justify-content-center">
      <div className="col-12 mx-0 px-0 py-3">
        <table className="w-100">
          <thead>
            <tr className="intestazione">
              <th className="">
                <div
                  className="h-100 d-flex flex-row align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(1, isVisibleFiltri))
                  }
                >
                  <span>OEM</span>
                  <div className="pl-1">
                    <img
                      className="btnOrder2"
                      src={require("../../img/arrowDown.png")}
                    />
                  </div>
                </div>
                {isVisibleFiltri == 1 ? (
                  <FilterMenuOnlyOrder
                    property={"OriginalEquipmentManufacturerId"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className=" ">
                <div
                  className="h-100 d-flex flex-row align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(2, isVisibleFiltri))
                  }
                >
                  <span>HARDWARE SOLUTION</span>
                  <div className=" pl-1">
                    <img
                      className="btnOrder2"
                      src={require("../../img/arrowDown.png")}
                    />
                  </div>
                </div>
                {isVisibleFiltri == 2 ? (
                  <FilterMenuOnlyOrder
                    property={"HardwareSolution"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className=" ">
                <div
                  className="h-100 d-flex flex-row align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(3, isVisibleFiltri))
                  }
                >
                  <span>Platform</span>
                  <div className=" pl-1">
                    <img
                      className="btnOrder2"
                      src={require("../../img/arrowDown.png")}
                    />
                  </div>
                </div>
                {isVisibleFiltri == 3 ? (
                  <FilterMenuOnlyOrder
                    property={"Platform"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className=" ">
                <div
                  className="h-100 d-flex flex-row align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(4, isVisibleFiltri))
                  }
                >
                  <span>HARDWARE TYPE</span>
                  <div className=" pl-1">
                    <img
                      className="btnOrder2"
                      src={require("../../img/arrowDown.png")}
                    />
                  </div>
                </div>
                {isVisibleFiltri == 4 ? (
                  <FilterMenuOnlyOrder
                    property={"HardwareType"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className=" "></th>
            </tr>
          </thead>
          <tbody>
            {dataReload &&
              dataReload
                .sort((x, y) => {
                  return Number(y.isMain) - Number(x.isMain);
                })
                .map((item, index) => (
                  <tr
                    className={item.isMain ? "dati default" : "dati"}
                    key={item.key}
                  >
                    <td className=" ">
                      <a className="oem" tabIndex={-1}>
                        {item.value.originalEquipmentManufacturer}
                      </a>
                    </td>
                    <td className=" ">{item.value.hardwareSolution}</td>
                    <td className=" ">{item.value.platform}</td>
                    <td className=" ">{item.value.hardwareType}</td>
                    <td className=" ">
                      {item.isMain == false ? (
                        <div className="d-flex flex-row-reverse">
                          <button
                            type="button"
                            className="btn btn-link "
                            onClick={() => remove(item.key)}
                          >
                            <img
                              className="btnEdit"
                              src={require("../../img/delete.png")}
                            />
                          </button>
                        </div>
                      ) : null}
                    </td>
                  </tr>
                ))}
          </tbody>
        </table>
      </div>
      <div className="col-12 row mx-0 px-0">
        <div className="col-10 pl-0 pr-1">
          <input
            className="w-100 associateSearchbar"
            type="text"
            placeholder="Search.."
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
          ></input>
        </div>
        <div className="col-2 pl-1 pr-3">
          <button
            onClick={associate}
            disabled={toAssociateState.length === 0 ? true : false}
            className="w-100   voda-bold btn btn-danger px-4 associateSearchbar"
            type="button"
          >
            ASSOCIATE
          </button>
        </div>
        {searchText != "" ? (
          <div className="col-12 px-0 mt-4">
            <label className="voda-regular">Search results:</label>
            <div className="associateSearchbarContainer col-12 px-0">
              <table className="w-100">
                <tbody>
                  {searchData && searchData?.length > 0 ? (
                    searchData &&
                    searchData.map((item, index) => (
                      <SearchRow
                        array={toAssociateState}
                        key={item.key}
                        data={item.value}
                        action={{ addRemove }}
                      ></SearchRow>
                    ))
                  ) : (
                    <tr className="noresults">
                      <td colSpan={5} className="  text-danger">
                        THE SEARCH DID NOT RETURN ANY RESULTS
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        ) : null}
      </div>
    </div>
  );
};

export default MajorHardwareThirdPartyComponent;
interface PropsRow {
  data: MajorHardwareBuildDtoGrid;
  action: {
    addRemove(checked: boolean, item: MajorHardwareBuildDtoGrid): any;
  };
  array: MajorHardwareBuildDtoGrid[];
}

const SearchRow: React.FC<PropsRow> = (props) => {
  const [checked, setChecked] = useState<boolean>(true);
  const [disabled, setDisabled] = useState<boolean>(false);
  const data = props.data;

  const addOrRemove = () => {
    if (checked === true) {
      setChecked(false);
    } else if (checked === false) {
      setChecked(true);
    }
    props.action.addRemove(!checked, data);
  };

  useEffect(() => {
    if (
      props.array.filter(
        (x) => x.majorHardwareBuildId == props.data.majorHardwareBuildId
      ).length > 0
    ) {
      setChecked(false);
    } else {
      setChecked(true);
    }
  }, [props.array]);

  const handleChange = () => {
    return;
  };

  return (
    <tr className="dati" onClick={addOrRemove}>
      <td className="checkInputTable">
        <input
          type="checkbox"
          disabled={disabled}
          checked={!checked}
          onChange={(e) => handleChange()}
        />
      </td>
      <td className=" ">
        <a className="oem" tabIndex={-1}>{data.originalEquipmentManufacturer}</a>
      </td>
      <td className=" ">{data.hardwareSolution}</td>
      <td className=" ">{data.platform}</td>
      <td className=" ">{data.hardwareType}</td>
    </tr>
  );
};
