import React, { useEffect, useState } from "react";
import { Tab, Tabs } from "react-bootstrap";
import DatePicker from "react-datepicker";
import { useSelector } from "react-redux";
import Select from "react-select";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { dictionaryToArray } from "../Hook/Dictionary";
import { useAuth } from "../Hook/useAuth";
import { useFormTableCrud } from "../Hook/useFormTableCrud";
import { useResourceTableCrudForVolteKPI } from "../Hook/useResourceTableCrudForVolteKPI";
import { VolteKPIReportRow } from "../Model/Report/ReportVolteKPIModel";
import {
  VolteKPIDtoGrid,
  VolteKPIQueryObjectGrid,
} from "../Model/VolteKpi/VolteKPI";
import { DownloadVolteKPIReport } from "../Redux/Action/Report/ReportDownloadVolteKPIAction";
import { GetReportVolteKPIGrid } from "../Redux/Action/Report/ReportVolteKPIAction";
import { EditVolteKPI } from "../Redux/Action/VolteKPI/VolteKPIEditAction";
import { RootState } from "../Redux/Store/rootStore";
import BuiltCapacityForProvisioned from "../screen/GenerateVoLTEDashboard/BuiltCapacityForProvisioned";
import BuiltCapacityForRegistered from "../screen/GenerateVoLTEDashboard/BuiltCapacityForRegistered";
import { CommonValidation } from "../screen/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { useTheme } from "../Context/ThemeContext";

export let paginationQuery = {
  year: new Date().getFullYear(),
};

export const getColor = (text: string): string => {
  switch (text) {
    case "expired":
      return "red";
    case "on expiration":
      return "orange";
    case "on support":
      return "green";
    default:
      return "";
  }
};

const GenerateVoLTEDashboard: React.FC = (props) => {
  //PAGE
  const [keyTabs, setKeyTabs] = useState("provisioned");
  const { KPIAdmin, admin } = useAuth();

  const Grid = (state: RootState) =>
    state.reportVolteKPIReducer.ReportVolteKPIGridResult;
  const GridDto = useSelector(Grid);

  //DTO Provisioned
  const [dataProvisioned, setDataProvisioned] = useState<VolteKPIReportRow[]>();
  //DTO Registered
  const [dataRegistered, setDataRegistered] = useState<VolteKPIReportRow[]>();
  const [opcoOption, setOpcoOption] = useState<any>();
  const [opcoSelected, setOpcoSelected] = useState<any>([]);
  const [setted, setSetted] = useState<boolean>(false);
  const { validation, setValidation, confirmForm, setChanged, setInputValue } =
    useFormTableCrud<VolteKPIDtoGrid>(GetReportVolteKPIGrid, EditVolteKPI);
  const { query, setQuery } = useResourceTableCrudForVolteKPI(
    paginationQuery,
    GetReportVolteKPIGrid
  );
  const [date, setDate] = useState<Date>(new Date());

  const InvocheDownload = async () => {
    const year = date.getFullYear();
    let listOfOpcoId = [] as Array<number>;
    opcoSelected?.map((el) => {
      listOfOpcoId.push(el.key);
    });

    let result = await DownloadVolteKPIReport(listOfOpcoId, year);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const { darkMode } = useTheme();

  const customStyles = {
    option: (provided, state) => ({
      ...provided,
      backgroundColor: darkMode ? "blue" : "white", // Change 'blue' to your desired background color
      color: darkMode ? "white" : "black", // Change 'white' to your desired text color
    }),
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto?.provisioned != undefined) {
      setDataProvisioned(GridDto?.provisioned);
    }
    if (GridDto?.registered != undefined) {
      setDataRegistered(GridDto?.registered);
    }

    if (GridDto?.opCoResource != undefined) {
      const listOfOpco: any = GridDto?.opCoResource
        ? dictionaryToArray(GridDto?.opCoResource)
        : undefined;
      setOpcoOption(listOfOpco);

      if (!setted && listOfOpco) {
        setOpcoSelected(listOfOpco);
        setSetted(true);
      }
    }
  }, [GridDto]);

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

  const onViewPress = () => {
    const isValid = validazioneClient().response;
    if (isValid) {
      const dataObj = {} as VolteKPIQueryObjectGrid;
      dataObj.year = date.getFullYear();
      let listOfOpcoId = [] as Array<number>;
      opcoSelected?.map((el) => {
        listOfOpcoId.push(el.key);
      });
      dataObj.opCo = listOfOpcoId;
      GetReportVolteKPIGrid(dataObj);
    }
  };

  const validazioneClient = () => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };
    if (date == undefined || date == null) {
      addInvalidProperty("date");
    }
    if (
      opcoSelected?.length === 0 ||
      opcoSelected == undefined ||
      opcoSelected == null
    ) {
      addInvalidProperty("opCo");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  return (
    <div className="pageContainer">
      <div className="headerPage row mx-0 justify-content-between position-relative pt-2">
        <div>
          <h3 className="voda-bold text-left">Generate VoLTE Dashboard</h3>
          {/* <h3 className="voda-bold">
            VoLTE + VoWiFi Built Capacity for what can be
            {keyTabs === "provisioned" ? " PROVISIONED" : " REGISTERED"} [k]
          </h3> */}
          <span className="sub_title">
            {`Built Capacity for what can be ${
              keyTabs === "provisioned" ? "PROVISIONED" : "REGISTERED"
            } `}
            VoLTE + VoWiFi Built Capacity for what can be
            {keyTabs === "provisioned" ? " PROVISIONED" : " REGISTERED"} [k]
          </span>
        </div>
        {(KPIAdmin || admin) && (
          <div className="d-flex justify-content-end">
            <button
              className="download-to-excel mrl-10"
              onClick={() => InvocheDownload()}
            >
              {/* <img src={require("../img/excel.png")} /> */}
              Download to Excel
            </button>
          </div>
        )}
      </div>

      <div className="mt-4 mb-5">
        <div className="row pr-0 mr-0">
          <div className="col-10 pl-3 pr-1">
            <div className="form-group col-12 pl-0">
              <label className="voda-bold w-100 text-left themeText">
                Please Select Opco<span className="red">*</span>
                <div className="d-flex" style={{ color: "black" }}>
                  <div className="w-100">
                    <Select
                      // styles={customStyles}
                      options={opcoOption}
                      menuPosition={"fixed"}
                      value={opcoSelected}
                      onChange={(e) => e && setOpcoSelected(e)}
                      onBlur={() => setInputValue("")}
                      isMulti
                      isSearchable
                      isClearable
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option.key}
                    />
                  </div>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("opCo") ? (
                    <label className="validation">
                      *This field cannot be empty
                    </label>
                  ) : null}
                </div>
              </label>
              {/* {validation &&
              validation.response == false &&
              validation.property?.includes("opCo") ? (
                <label className="validation">
                  *This field cannot be empty
                </label>
              ) : null} */}
            </div>
          </div>
          <div className="col-2 pr-0">
            <div className="form-group col-12 pr-0">
              <label className="voda-bold w-100 text-left themeText">
                For Fiscal Year<span className="red">*</span>
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
                      dateFormat="yyyy"
                      showYearPicker
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
              <div className="mb-4 p-0 ">
                <button
                  type="button"
                  className="btn clearBtn mrl-0 mb-35 btn-border"
                  onClick={() => onViewPress()}
                >
                  View Dashboard
                </button>
              </div>
            </div>
          </div>

          {/* <div className="col-6"></div> */}
        </div>

        <Tabs
          defaultActiveKey="hardware"
          id="report"
          activeKey={keyTabs}
          onSelect={(x) => setKeyTabs(x || "")}
        >
          <Tab eventKey="provisioned" title="Provisioned">
            <BuiltCapacityForProvisioned data={dataProvisioned} />
          </Tab>
          <Tab eventKey="registered" title="Registered">
            <BuiltCapacityForRegistered data={dataRegistered} />
          </Tab>
        </Tabs>
      </div>
    </div>
  );
};

//coment
export default GenerateVoLTEDashboard;
