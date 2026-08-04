import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import ModalConfirm from "../Components/ModalConfirm";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import Select from "react-select";
import { useAuth } from "../Hook/useAuth";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { CustomGridRender } from "../Model/Common";
import {
  SoftwareConfigurationDtoCreate,
  SoftwareConfigurationDtoGrid,
  SoftwareConfigurationDtoUpdate,
  SoftwareConfigurationQueryObjectGrid,
} from "../Model/SoftwareConfiguration";
import setLoader from "../Redux/Action/LoaderAction";
import { GetNetworkElementAsIsCreateResource } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsCreateAction";
import {
  DeleteDeepNetworkElementAsIs,
  GetRelatedRecordsNetworkElementAsIs,
  RestoreNetworkElementAsIs,
} from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsDeleteAction";
import { GetSoftwareConfigurationReport } from "../Redux/Action/SoftwareConfiguration/SoftwareConfigurationDownloadAction";
import { GetNetworkElementAsIsEditResource } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsEditAction";

import { dictionaryToArray } from "../Hook/Dictionary";

import {
  GetFilterColumSoftwareConfiguration,
  GetFilterOpcoSoftwareConfiguration,
  GetFilterOemSoftwareConfiguration,
  GetFilterEleSoftwareConfiguration,
  GetDropdownFilterSoftwareConfiguration,
} from "../Redux/Action/SoftwareConfiguration/SoftwareConfigurationGridAction";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import SoftwareConfigurationsTable from "./SoftwareConfigurationsTable";
import { useTheme } from "../Context/ThemeContext";
import DarkMode from "../Components/DarkMode/DarkMode";

export let paginationQuery: SoftwareConfigurationQueryObjectGrid = {
  softwarewareconfigurationid: [],
  opCo: [],
  oem: [],
  networkFunction: [],
  nodeType: [],
  elementDeploymentName: [],
  location: [],
  systemTypeId: [],
  softwareReleaseInformationSystemLevel: [],
  softwareProductNumberSystemLevel: [],
  softwareProductionDate: undefined,
  softwareInstallDate: undefined,
  hardwareSolution: [],
  platform: [],
  hardwareType: [],
  otherHardwareInfo: [],
  hardwareAcquisition: [],
  manualOverride: [],
  hardwareSystemId: [],
  dataAcquisitionDate: undefined,
  dataAcquisitionMethod: [],
  elementManager: [],
  elementManagerExportFileFormat: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
};

const SoftwareConfigurations: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [opco, setOpco] = useState<{ [key: string]: string }>({});
  const [oem, setOem] = useState<{ [key: string]: string }>({});
  const [ele, setEle] = useState<{ [key: string]: string }>({});
  const [viewReport, setViewReport] = useState<boolean>(false);
  const [selectedOpcoValue, setSelectedOpcoValue] = useState<string[]>([]);
  const [selectedOemValue, setSelectedOemValue] = useState<string[]>([]);
  const [selectedEleValue, setSelectedEleValue] = useState<string[]>([]);

  const { KPIAdmin, admin, pageSize } = useAuth();
  const [filterType, setFilterType] = useState<number | undefined>(0);
  const [selected, setSelected] = useState<
    { label: string; value: number }[] | undefined
  >(undefined);

  const [data, setData] = useState<SoftwareConfigurationDtoGrid[] | undefined>(
    []
  );

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [prevPage, setPrevPage] = useState<string>();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();

  const refresh = () => {
    setLoader("ADD", "GetSoftwareConfigurationGrid");
    closeModal();
    setLoader("REMOVE", "GetSoftwareConfigurationGrid");
  };

  const GetFilterDropdown = async () => {
    setLoader("ADD", "GetSoftwareConfigurationGrid");
    const result = await GetDropdownFilterSoftwareConfiguration();
    setLoader("REMOVE", "GetSoftwareConfigurationGrid");
    const newOpCoObj = {};
    const newOemObj = {};
    const newEleObj = {};
    if (result?.opCoResource) {
      result?.opCoResource?.forEach((item, index) => {
        newOpCoObj[index] = item;
      });
    }
    if (result?.oemResource) {
      result?.oemResource?.forEach((item, index) => {
        newOemObj[index] = item;
      });
    }
    if (result?.elementResource) {
      result?.elementResource?.forEach((item, index) => {
        newEleObj[index] = item;
      });
    }
    setOpco(newOpCoObj);
    setOem(newOemObj);
    setEle(newEleObj);
  };

  const GetOpco = async () => {
    setLoader("ADD", "GetSoftwareConfigurationGrid");
    const result = await GetFilterOpcoSoftwareConfiguration("opCo", "");
    setLoader("REMOVE", "GetSoftwareConfigurationGrid");
    const newObj = {};
    if (result) {
      result.forEach((item, index) => {
        newObj[index] = item.value;
      });
    }
    setOpco(newObj);
  };

  const GetOem = async () => {
    setLoader("ADD", "GetSoftwareConfigurationGrid");
    const result = await GetFilterOemSoftwareConfiguration("oem", "");
    setLoader("REMOVE", "GetSoftwareConfigurationGrid");
    const newObj = {};
    if (result) {
      result.forEach((item, index) => {
        newObj[index] = item.value;
      });
    }
    setOem(newObj);
  };

  const GetEle = async () => {
    setLoader("ADD", "GetSoftwareConfigurationGrid");
    const result = await GetFilterEleSoftwareConfiguration("elementName", "");
    setLoader("REMOVE", "GetSoftwareConfigurationGrid");
    const newObj = {};
    if (result) {
      result.forEach((item, index) => {
        newObj[index] = item.value;
      });
    }
    setEle(newObj);
  };

  const selectedOpco = (e: any) => {
    let arr: string[] = [];
    if (e) {
      arr.push(e.label);
    }
    setSelectedOpcoValue(arr);
    return;
  };

  const selectedOem = (e: any) => {
    let arr: string[] = [];
    if (e) {
      arr.push(e.label);
    }
    setSelectedOemValue(arr);

    return;
  };

  const selectedEle = (e: any) => {
    let arr: string[] = [];
    if (e) {
      arr.push(e.label);
    }
    setSelectedEleValue(arr);
    return;
  };

  const {
    New,
    Edit,
    isVisibleModal,
    edit,
    closeModal,
    Delete,
    localStateHistory,
    setLocalState,
    Restore,
  } = useOperationTableCrud<
    SoftwareConfigurationDtoUpdate,
    SoftwareConfigurationDtoCreate
  >(
    GetNetworkElementAsIsCreateResource,
    GetNetworkElementAsIsEditResource,
    DeleteDeepNetworkElementAsIs,
    refresh,
    RestoreNetworkElementAsIs
  );

  const CancelConfirm = () => {
    setConfirm(stateConfirm);
  };

  const EditAndDetail = (id: number, idDetail) => {
    setLocalState({
      id: id,
      tab: "plannedActivities",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(id);
    setDetailId(idDetail);
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
    // setLoader("REMOVE", "GetSoftwareConfigurationGrid");
    // GetOpco();
    // GetOem();
    // GetEle();
    GetFilterDropdown();
  }, []);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    //setLoader("ADD", "GetSoftwareConfigurationGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
      };

      if (localState?.id != null) {
        if (localState.idDetail && localState.idDetail != null) {
          EditAndDetail(localState?.id, localState.idDetail);
          return;
        }
        setLocalState(localState);
        Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
  }, []);

  const onViewReport = async () => {
    if (
      selectedOpcoValue.length == 0 &&
      selectedOemValue.length == 0 &&
      selectedEleValue.length == 0
    ) {
      setConfirm({
        title: "Alert",
        message: "Please select a filter to view the report",
        button: "Ok",
        item: 0,
        onlyOneButton: true,
        isOpen: true,
        actions: {
          confirm: () => CancelConfirm(),
          cancel: () => CancelConfirm(),
        },
      });
      setViewReport(false);
    } else {
      setViewReport(true);
    }
  };

  const ExportAll = async () => {
    let result = await GetSoftwareConfigurationReport();
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  return (
    <div className="pageContainer">
      <div className="headerPage row mx-0 justify-content-between position-relative">
        <div>
          <h3 className="voda-bold">Software Configurations</h3>
        </div>
        <ModalConfirm data={confirm} />
        {(KPIAdmin || admin) && (
          <div className="">
            <button
              disabled={true}
              className="px-4 btnHeader clearBtn disabledCursor br-20 voda-bold mrl-10"
              //onClick={() => ExportAll()}
            >
              <img
                src={
                  darkMode
                    ? require("../img/MS Excel_white.png")
                    : require("../img/MS Excel_black.png")
                }
                className="excel"
              />
              Export All Data
            </button>
            <button
              className="px-4 btnHeader clearBtn br-20 voda-bold"
              type="button"
              onClick={() => window.location.reload()}
            >
              Refresh
            </button>
          </div>
        )}
      </div>
      <div className="mt-4">
        <div className="row">
          <div className="col-6 px-2">
            <div className="form-group col-12">
              <label className="themeText voda-bold w-100 text-left">
                Please Select an OpCo
                {/* <span className="red">*</span> */}
                <div className="d-flex" style={{ color: "black" }}>
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100 text-left"
                      // styles={customStyles}
                      options={dictionaryToArray(opco).map((item) => ({
                        value: item.key,
                        label: item.value,
                      }))}
                      onChange={(e) => selectedOpco(e)}
                      isSearchable
                      isClearable
                      //getOptionLabel={(option) => option.value}
                      //getOptionValue={(option) => option.key.toString()}
                    />
                  </div>
                </div>
              </label>
            </div>
          </div>
          <div className="col-6 px-2">
            <div className="form-group col-12">
              <label className="themeText voda-bold w-100 text-left">
                Please Select an Oem
                {/* <span className="red">*</span> */}
                <div className="d-flex" style={{ color: "black" }}>
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100 text-left"
                      // styles={customStyles}
                      options={dictionaryToArray(oem).map((item) => ({
                        value: item.key,
                        label: item.value,
                      }))}
                      onChange={(e) => selectedOem(e)}
                      isSearchable
                      isClearable
                      //getOptionLabel={(option) => option.value}
                      //getOptionValue={(option) => option.key.toString()}
                    />
                  </div>
                </div>
              </label>
            </div>
          </div>

          <div className="col-12">
            <div className="row">
              <div className="col-6 px-2">
                <div className="form-group col-12">
                  <label className="themeText w-100 voda-bold text-left">
                    Please Select an Element Name
                    {/* <span className="red">*</span> */}
                    <div className="d-flex" style={{ color: "black" }}>
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          className="w-100 text-left"
                          // styles={customStyles}
                          options={dictionaryToArray(ele).map((item) => ({
                            value: item.key,
                            label: item.value,
                          }))}
                          //value={selected ?? []}
                          //onChange={setSelected}
                          onChange={(e) => selectedEle(e)}
                          isSearchable
                          isClearable
                          //labelledBy="Select"
                        />
                      </div>
                    </div>
                  </label>
                </div>
              </div>

              <div className="col-2 px-2">
                <div className="mt-20 fr">
                  <button
                    className="px-4 btnHeader clearBtn br-20 voda-bold"
                    onClick={() => onViewReport()}
                  >
                    View Report
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div>
          {viewReport && (
            <SoftwareConfigurationsTable
              Oem={selectedOemValue}
              Opco={selectedOpcoValue}
              Ele={selectedEleValue}
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default SoftwareConfigurations;
