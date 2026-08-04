import React, { useEffect, useState } from "react";
import DatePicker from "react-datepicker";
import { useSelector } from "react-redux";
import Select from "react-select";
import ModalConfirm from "../../../Components/ModalConfirm";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import { dictionaryToArray } from "../../../Hook/Dictionary";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import {
  VolteKPIDtoUpdate,
  VolteKPIQueryObjectGrid,
} from "../../../Model/VolteKpi/VolteKPI";
import setLoader from "../../../Redux/Action/LoaderAction";
import { setNotification } from "../../../Redux/Action/NotificationAction";
import { CreatVolteKPI } from "../../../Redux/Action/VolteKPI/VolteKPICreateAction";
import { EditVolteKPI } from "../../../Redux/Action/VolteKPI/VolteKPIEditAction";
import { NotifyType } from "../../../Redux/Reducer/NotificationReducer";
import { RootState, rootStore } from "../../../Redux/Store/rootStore";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { useTheme } from "../../../Context/ThemeContext";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    openModal(): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    setQuery(val: VolteKPIQueryObjectGrid): any;
  };
  edit: boolean;
  keyTab?: string;
  query: VolteKPIQueryObjectGrid;
}

const DashBoard: React.FC<Props> = (props) => {
  const { validation, setValidation, confirmForm, setChanged, setInputValue } =
    useFormTableCrud<VolteKPIDtoUpdate>(CreatVolteKPI, EditVolteKPI);

  const Grid = (state: RootState) =>
    state.volteKPIGridReducer.VolteKPIGridResult;
  const GridDto = useSelector(Grid);
  const [setted, setSetted] = useState<boolean>(false);

  const [date, setDate] = useState<Date>(new Date());
  const [volteKPITypesOption, setVolteKPITypesOption] = useState<any>();
  const [opcoOption, setOpcoOption] = useState<any>();

  const [volteKPITypesSelected, setVolteKPITypesSelected] = useState<any>([]);

  const [opcoSelected, setOpcoSelected] = useState<any>([]);

  useEffect(() => {
    const listOfVolteKPITypesOption: any[] | undefined =
      GridDto?.volteKPITypeResource
        ? dictionaryToArray(GridDto?.volteKPITypeResource)
        : undefined;
    const listOfOpco: any[] | undefined = GridDto?.opCoResource
      ? dictionaryToArray(GridDto?.opCoResource)
      : undefined;

    setVolteKPITypesOption(listOfVolteKPITypesOption);
    setOpcoOption(listOfOpco);

    if (listOfVolteKPITypesOption) {
      const defaultSelected =
        listOfVolteKPITypesOption.filter((el) => el.key !== 4) ?? [];
      setVolteKPITypesSelected(defaultSelected);
    }
    if (listOfOpco && !setted) {
      setOpcoSelected(listOfOpco);
    }
    if (
      listOfVolteKPITypesOption &&
      listOfVolteKPITypesOption?.length > 0 &&
      listOfOpco &&
      listOfOpco?.length > 0
    ) {
      setSetted(true);
    }
  }, [GridDto?.opCoResource, GridDto?.volteKPITypeResource]);

  const onChangeDate = (newDate) => {
    //Rimuovi Validazione
    if (validation?.property?.includes("date")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("date");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    setDate(newDate);
  };

  const validazioneClient = () => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      volteKPITypesSelected?.length === 0 ||
      volteKPITypesSelected == undefined ||
      volteKPITypesSelected == null
    ) {
      addInvalidProperty("volteKPITypes");
    }
    if (
      opcoSelected?.length === 0 ||
      opcoSelected == undefined ||
      opcoSelected == null
    ) {
      addInvalidProperty("opCo");
    }
    if (date == undefined || date == null) {
      addInvalidProperty("date");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const onViewPress = () => {
    const isValid = validazioneClient().response;
    if (isValid) {
      let copy = { ...props.query } as VolteKPIQueryObjectGrid;

      //Setto la data
      copy.month = date.getMonth() + 1;
      copy.year = date.getFullYear();

      // Setto volteKPITypes
      let copyOfVolteKPITypes = [] as Array<number>;
      volteKPITypesSelected?.map((el) => {
        copyOfVolteKPITypes.push(el.key);
      });
      copy.volteKPITypes = copyOfVolteKPITypes;

      // Setto opco
      let copyOfopco = [] as Array<number>;
      opcoSelected?.map((el) => {
        copyOfopco.push(el.key);
      });
      copy.opCo = copyOfopco;

      props.action.setQuery(copy);
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered!",
          notifyType: NotifyType.warning,
        })
      );
    }
  };

  const change = (property: string, value: any) => {
    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    // Cambio volteKPITypes o opCo
    property === "volteKPITypes"
      ? setVolteKPITypesSelected(value)
      : property === "opCo" && setOpcoSelected(value);
  };

  const { darkMode } = useTheme();
  const customStyles = {
    option: (provided, state) => ({
      ...provided,
      backgroundColor: darkMode ? "blue" : "white", // Change 'blue' to your desired background color
      color: darkMode ? "white" : "black", // Change 'white' to your desired text color
    }),
  };

  return (
    <div className="w-100 pb-3">
      <ModalConfirm data={confirmForm} />
      <form id="formSoftwareBuild" onChange={() => setChanged(true)}>
        <div className="row col-12 px-0 mx-0">
          <div className="col-4 px-2">
            <div className="form-group col-12">
              <label className="themeText voda-bold w-100">
                Please Select KPI<span className="red">*</span>
                <div className="d-flex" style={{ color: "black" }}>
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      // styles={customStyles}
                      options={volteKPITypesOption}
                      value={volteKPITypesSelected}
                      onChange={(e) => change("volteKPITypes", e)}
                      onBlur={() => setInputValue("")}
                      isMulti
                      isSearchable
                      isClearable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option["key"].toString()}
                    />
                  </div>
                </div>
              </label>
              {validation &&
              validation.response == false &&
              validation.property?.includes("volteKPITypes") ? (
                <label className="validation">
                  *This field cannot be empty
                </label>
              ) : null}
            </div>
          </div>
          <div className="col-4 px-2">
            <div className="form-group col-12">
              <label className="themeText voda-bold   w-100">
                Please Select Month<span className="red">*</span>
                <div className="d-flex">
                  <div
                    className="w-100"
                    style={{ position: "relative", top: "-5px" }}
                  >
                    <DatePicker
                      selected={date}
                      onChange={(newDate, e) => {
                        e.preventDefault();
                        onChangeDate(newDate);
                      }}
                      dateFormat="MMMM yyyy"
                      showMonthYearPicker
                      className="inputForm w-100"
                      maxDate={new Date()}
                    />
                  </div>
                </div>
              </label>
              {validation &&
              validation.response == false &&
              validation.property?.includes("date") ? (
                <label className="validation">
                  *This field cannot be empty
                </label>
              ) : null}
            </div>
          </div>
          <div className="col-4 px-2">
            <div className="form-group col-12">
              <label className="themeText voda-bold   w-100">
                Please Select Opco<span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100" style={{ color: "black" }}>
                    <Select
                      menuPosition={"fixed"}
                      // styles={customStyles}
                      options={opcoOption}
                      value={opcoSelected}
                      onChange={(e) => e && change("opCo", e)}
                      onBlur={() => setInputValue("")}
                      isMulti
                      isSearchable
                      isClearable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option.key}
                    />
                  </div>
                </div>
              </label>
              {validation &&
              validation.response == false &&
              validation.property?.includes("opCo") ? (
                <label className="validation">
                  *This field cannot be empty
                </label>
              ) : null}
            </div>
          </div>
          <div className="col-12 px-2">
            <div className="col-12 px-3 d-flex flex-row flex-mode">
              <button
                type="button"
                className="btn mt-4 mr-5 clearBtn"
                onClick={onViewPress}
              >
                View Dashboard
              </button>
              <button
                type="button"
                className="btn mt-4 clearBtn"
                onClick={() => {
                  props.action.openModal();
                }}
              >
                Add Monthly Value
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};

export default DashBoard;
