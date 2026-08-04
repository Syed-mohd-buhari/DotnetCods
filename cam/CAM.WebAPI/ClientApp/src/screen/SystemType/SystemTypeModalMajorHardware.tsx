import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import FilterMenuOnlyOrder from "../../Components/FilterMenuOnlyOrder";
import {
  toggleState,
  filterObjectArrayWithObjectArray,
  stringIsNullOrEmpty,
} from "../../Hook/Common";
import { MajorHardwareBuildMainSystemTypeDto } from "../../Model/SystemTypeModel";
import { GetMajorHardwareBuildGrid } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildGridAction";
import {
  MajorHardwareBuildQueryObjectGrid,
  MajorHardwareBuildDtoGrid,
} from "../../Model/MajorHardwareBuild";
import { useLocation, useNavigate } from "react-router-dom";
import ModalConfirm from "../../Components/ModalConfirm";
import { stateConfirm, DataModalConfirm } from "../../Model/Common";
import Loader from "../../Components/Loader";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";

interface Props {
  data: MajorHardwareBuildMainSystemTypeDto[];
  action: {
    GetMajorHardwareFromList(items: MajorHardwareBuildMainSystemTypeDto[]): any;
    setChanged(val: boolean): any;
  };
  changed: boolean;
  tab: string;
}

const ModalMajorHardware: React.FC<Props> = (props) => {
  const navigate = useNavigate();
  const location: any = useLocation();
  const [isVisibleFiltri, setIsVisibleFiltri] = useState(0);
  const [filtriAttivi, setFiltriAttivi] =
    useState<MajorHardwareBuildQueryObjectGrid>();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [isLoading, setIsLoading] = useState(true);
  const closeAll = () => {
    setIsVisibleFiltri(0);
  };

  //LISTA SOPRA
  const [dataAssociated, setDataAssociated] =
    useState<
      { key: number; value: MajorHardwareBuildDtoGrid; isMain: boolean }[]
    >();

  //RICERCA
  const [searchText, setSearchText] = useState("");

  //SELEZIONATI DA ASSOCIARE
  const [toAssociateState, setToAssociateState] = useState<
    MajorHardwareBuildDtoGrid[]
  >([]);

  //RISULTATI RICERCA
  const [searchResults, setSearchResults] =
    useState<MajorHardwareBuildDtoGrid[]>();

  //RESET SEARCH TEXT AL CAMBIO TAB
  useEffect(() => {
    setSearchText("");
  }, [props.tab]);

  //UPDATE FROM PROPS
  useEffect(() => {
    let ids: (number | undefined)[] =
      props.data?.map((x) => x.majorHardwareBuildId) ?? ([] as number[]);
    GetMajorHardwareBuildGrid(filtriAttivi, true, true).then((results) => {
      if (results) {
        let toAdd = results.filter((x) => {
          return ids.indexOf(x.majorHardwareBuildId) !== -1;
        });
        let cast = toAdd.map((x) => {
          return {
            key: x.majorHardwareBuildId,
            value: x,
            isMain: props.data.find(
              (z) => z.majorHardwareBuildId == x.majorHardwareBuildId
            )?.isMain,
          } as {
            key: number;
            value: MajorHardwareBuildDtoGrid;
            isMain: boolean;
          };
        });
        setDataAssociated(cast);
      }
    });
  }, [props.data]);

  const onChangeSearch = async (text: string) => {
    setIsLoading(true);
    setSearchText(text);
    await GetMajorHardwareBuildGrid(
      {
        name: text,
        sortBy: stringIsNullOrEmpty(filtriAttivi?.sortBy)
          ? "originalEquipmentManufacturerId"
          : filtriAttivi?.sortBy,
        isSortAscending: stringIsNullOrEmpty(filtriAttivi?.sortBy)
          ? true
          : filtriAttivi?.isSortAscending,
      } as MajorHardwareBuildQueryObjectGrid,
      true,
      true
    ).then((results) => {
      if (results) {
        let ids: (number | undefined)[] =
          props.data?.map((x) => x.majorHardwareBuildId) ?? ([] as number[]);
        let toAdd = results.filter((x) => {
          return ids.indexOf(x.majorHardwareBuildId) === -1;
        });
        setSearchResults(toAdd);
      }
      setIsLoading(false);
    });
  };

  const addRemove = (checked: boolean, item: MajorHardwareBuildDtoGrid) => {
    props.action.setChanged(true);
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
    props.action.setChanged(true);
    if (dataAssociated != null && dataAssociated != undefined) {
      let copy = [...props.data];
      let toAssociate = toAssociateState.map((x) => {
        return {
          majorHardwareBuildId: x.majorHardwareBuildId,
          isMain: false,
        } as MajorHardwareBuildMainSystemTypeDto;
      });
      copy = copy.concat(toAssociate);
      props.action.GetMajorHardwareFromList(copy);
      setToAssociateState([]);

      if (searchResults) {
        let ids: (number | undefined)[] =
          copy.map((x) => x.majorHardwareBuildId) ?? ([] as number[]);
        let toAdd = searchResults.filter((x) => {
          return ids.indexOf(x.majorHardwareBuildId) === -1;
        });
        setSearchResults(toAdd);
      }
    }
  };

  const remove = (id: number) => {
    props.action.setChanged(true);
    const copy = [...props.data];
    const index = copy.findIndex((x) => x.majorHardwareBuildId == id);
    if (index !== -1) {
      copy.splice(index, 1);
    }
    props.action.GetMajorHardwareFromList(copy);
    setConfirm(stateConfirm);
  };

  const orderBy = async (property: string, isAscending: boolean) => {
    let copy = { ...filtriAttivi } as MajorHardwareBuildQueryObjectGrid;
    copy.name = searchText;
    copy.sortBy = property;
    copy.isSortAscending = isAscending;
    setFiltriAttivi(copy);

    let ordered = dataAssociated?.sort((a, b) =>
      ((a.value[property] ?? "") as string).toLowerCase() >
      ((b.value[property] ?? "") as string).toLowerCase()
        ? !isAscending
          ? -1
          : 1
        : !isAscending
        ? 1
        : -1
    );
    setDataAssociated(ordered);

    setIsLoading(true);
    await GetMajorHardwareBuildGrid(copy, true, true).then((results) => {
      if (results) {
        let ids: (number | undefined)[] =
          props.data?.map((x) => x.majorHardwareBuildId) ?? ([] as number[]);
        let toAdd = results.filter((x) => {
          return ids.indexOf(x.majorHardwareBuildId) === -1;
        });
        setSearchResults(toAdd);
      }
      setIsLoading(false);
    });
  };

  const navigateToMajorHardware = (stateVal: number | undefined) => {
    let location = {
      pathname: "/majorhardware",
      search: "id=" + stateVal,
      state: {
        id: stateVal,
        tab: "MajorHardwareBuild",
        prevPage: "systemtype",
      },
    };
    navigate(location);
  };

  const ConfirmNavigate = (state: number | undefined) => {
    if (props.changed == true) {
      state &&
        setConfirm({
          title: "Confirm",
          message:
            "Are you sure you want to quit? Unsaved changes will be lost.",
          button: "Exit",
          item: 0,
          isOpen: true,
          actions: {
            cancel: () => setConfirm(stateConfirm),
            confirm: () => navigateToMajorHardware(state),
          },
        });
    } else {
      state && navigateToMajorHardware(state);
    }
  };

  const ConfirmRemove = (state: number | undefined) => {
    if (props.changed == true) {
      state &&
        setConfirm({
          title: "Confirm",
          message: "Are you sure you want to remove?",
          button: "Confirm",
          item: 0,
          isOpen: true,
          actions: {
            cancel: () => setConfirm(stateConfirm),
            confirm: () => remove(state),
          },
        });
    } else {
      state && navigateToMajorHardware(state);
    }
  };

  return (
    <div className="listaApparatiContainer mt-2 row mx-0 col-12 p-0 d-flex justify-content-center">
      <ModalConfirm data={confirm} />
      <div className="col-12 mx-0 px-0 py-3">
        <table className="w-100" style={{ minHeight: "inherit" }}>
          <thead>
            <tr className="intestazione">
              <th className="plr-10">
                <div
                  className="h-100 d-flex align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(1, isVisibleFiltri))
                  }
                >
                  <span className="fz-14">Equipment Manufacturer</span>
                  <div className="pl-1">
                    <div className="column-flex pointer">
                      <img
                        className="arrow-f"
                        src={require("../../img/top.jpg")}
                      />
                      <img
                        className="arrow-f"
                        src={require("../../img/bottom.jpg")}
                      />
                    </div>
                  </div>
                </div>
                {isVisibleFiltri == 1 ? (
                  <FilterMenuOnlyOrder
                    property={"originalEquipmentManufacturerId"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className="plr-10">
                <div
                  className="h-100 d-flex align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(2, isVisibleFiltri))
                  }
                >
                  <span className="fz-14">Hardware Solution</span>
                  <div className="pl-1">
                    <div className="column-flex pointer">
                      <img
                        className="arrow-f"
                        src={require("../../img/top.jpg")}
                      />
                      <img
                        className="arrow-f"
                        src={require("../../img/bottom.jpg")}
                      />
                    </div>
                  </div>
                </div>
                {isVisibleFiltri == 2 ? (
                  <FilterMenuOnlyOrder
                    property={"hardwareSolution"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className="plr-10">
                <div
                  className="h-100 d-flex align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(3, isVisibleFiltri))
                  }
                >
                  <span className="fz-14">Platform</span>
                  <div className="pl-1">
                    <div className="column-flex pointer">
                      <img
                        className="arrow-f"
                        src={require("../../img/top.jpg")}
                      />
                      <img
                        className="arrow-f"
                        src={require("../../img/bottom.jpg")}
                      />
                    </div>
                  </div>
                </div>
                {isVisibleFiltri == 3 ? (
                  <FilterMenuOnlyOrder
                    property={"platform"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className="plr-10">
                <div
                  className="h-100 d-flex align-items-center divFilter"
                  onClick={() =>
                    setIsVisibleFiltri(toggleState(4, isVisibleFiltri))
                  }
                >
                  <span className="fz-14">Hardware Type</span>
                  <div className="pl-1">
                    <div className="column-flex pointer">
                      <img
                        className="arrow-f"
                        src={require("../../img/top.jpg")}
                      />
                      <img
                        className="arrow-f"
                        src={require("../../img/bottom.jpg")}
                      />
                    </div>
                  </div>
                </div>
                {isVisibleFiltri == 4 ? (
                  <FilterMenuOnlyOrder
                    property={"hardwareType"}
                    action={{ closeAll, orderBy }}
                  ></FilterMenuOnlyOrder>
                ) : null}
              </th>
              <th className="plr-10"></th>
            </tr>
          </thead>
          <tbody>
            {dataAssociated &&
              dataAssociated
                .sort((x, y) => {
                  return Number(y.isMain) - Number(x.isMain);
                })
                .map((item, index) => (
                  <tr key={item.key} className="dati">
                    <td className=" ">
                      <a
                        className="oem"
                        onClick={() =>
                          ConfirmNavigate(item.value.majorHardwareBuildId)
                        }
                      >
                        {item.value.originalEquipmentManufacturer}
                      </a>
                    </td>
                    <td className=" ">{item.value.hardwareSolution}</td>
                    <td className=" ">{item.value.platform}</td>
                    <td className=" ">
                      <label
                        className="labelForm   w-100 mb-0"
                        dangerouslySetInnerHTML={{
                          __html: item.value.hardwareType ?? "",
                        }}
                      ></label>
                    </td>
                    <td className=" ">
                      {item.isMain == false ? (
                        <div className="d-flex flex-row-reverse">
                          <button
                            type="button"
                            className="btn btn-link "
                            onClick={() => ConfirmRemove(item.key)}
                          >
                            <img
                              className="btnEdit"
                              src={require("../../img/chain.png")}
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
      <div className="col-12 row mx-0 px-0 ">
        <label className="labelForm voda-bold w-100 mt-20 ">
          Associate Additional HW Solution<span className="red">*</span>
        </label>
        <div className="col-10 pl-0 pr-1">
          <input
            className="w-100 associateSearchbar"
            type="text"
            placeholder="Search.."
            value={searchText}
            onChange={(e) => onChangeSearch(e.target.value)}
          ></input>
        </div>
        <div className="col-2 pl-1 pr-3">
          <button
            onClick={associate}
            disabled={toAssociateState.length === 0 ? true : false}
            className="w-100 voda-bold btn cancel px-4 btnHeader fz-14"
            type="button"
          >
            Associate
          </button>
        </div>
        {searchText !== "" ? (
          isLoading ? (
            <div className="associateSearchbarContainer col-12 px-0">
              <Loader show={true} isFullScreen={false} />
            </div>
          ) : (
            <div className="col-12 px-0 mt-4">
              <label className="voda-bold">Search results:</label>
              <div className="associateSearchbarContainer col-12 px-0">
                <table className="w-100">
                  <tbody>
                    {searchResults && searchResults?.length > 0 ? (
                      searchResults.map((item, index) => (
                        <SearchRow
                          array={toAssociateState}
                          keyId={
                            item.majorHardwareBuildId?.toString() +
                            index.toString()
                          }
                          data={item}
                          action={{ addRemove, ConfirmNavigate }}
                        ></SearchRow>
                      ))
                    ) : (
                      <tr className="noresults">
                        <td colSpan={5} className="text-danger">
                          THE SEARCH DID NOT RETURN ANY RESULTS
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>
          )
        ) : null}
      </div>
    </div>
  );
};

export default ModalMajorHardware;
interface PropsRow {
  data: MajorHardwareBuildDtoGrid;
  action: {
    addRemove(checked: boolean, item: MajorHardwareBuildDtoGrid): any;
    ConfirmNavigate(id: number | undefined): any;
  };
  array: MajorHardwareBuildDtoGrid[];
  keyId: string;
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
    <tr className="dati" key={props.keyId} onClick={addOrRemove}>
      <td className="checkInputTable">
        <input
          type="checkbox"
          disabled={disabled}
          checked={!checked}
          onChange={(e) => handleChange()}
        />
      </td>
      <td className=" ">
        <a
          className="oem"
          onClick={() =>
            props.action.ConfirmNavigate(data.majorHardwareBuildId)
          }
        >
          {data.originalEquipmentManufacturer}
        </a>
      </td>
      <td className=" ">{data.hardwareSolution}</td>
      <td className=" ">{data.platform}</td>
      <td className=" ">
        <label
          className="labelForm w-100 mb-0"
          dangerouslySetInnerHTML={{
            __html: data.hardwareType ?? "",
          }}
        ></label>
      </td>
    </tr>
  );
};
