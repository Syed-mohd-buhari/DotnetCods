import React, { useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import Paginate from "../Components/PaginationComponent";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import Select from "react-select";
import { useAuth } from "../Hook/useAuth";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender } from "../Model/Common";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
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
import {
  GetSoftwareConfigurationGrid,
  GetSubFuntionFilter,
} from "../Redux/Action/SoftwareConfiguration/SoftwareConfigurationGridAction";
import { RootState } from "../Redux/Store/rootStore";
import SoftwareConfigurationGrid from "../screen/SoftwareConfiguration/SoftwareConfigurationGrid";
import NetworkElementAsIsModal from "../screen/NetworkElementAsIs/NetworkElementAsIsModal";
import SetupColumns from "../screen/Shared/SetupColumns";
import { dictionaryToArray } from "../Hook/Dictionary";
import {
  GetReportPATAllExports,
  GetReportPATGrid,
  GetVendorsOfPredefinedFilter,
} from "../Redux/Action/Report/PlannedActivityTrackerAction";

import { GetSWOem } from "../Redux/Action/SoftwareConfiguration/SoftwareConfigurationGridAction";
import { useGenerateLcmDbHardware } from "../Hook/ReportLcmDatabaseOverrideHook/useGenerateLcmDbHardware";
import {
  GetFilterColumSoftwareConfiguration,
  GetFilterOpcoSoftwareConfiguration,
  GetFilterOemSoftwareConfiguration,
  GetFilterEleSoftwareConfiguration,
} from "../Redux/Action/SoftwareConfiguration/SoftwareConfigurationGridAction";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import SubFuntionFilterGrid from "../screen/SoftwareConfiguration/SubFuntionFilterGrid";
import SubFunctionAreaContainer from "./SubFunctionAreaContainer";

interface Props {
  Oem: any;
  Opco: any;
  Ele: any;
}

export let paginationQuery: SoftwareConfigurationQueryObjectGrid = {
  softwarewareconfigurationid: [],
  opCo: [],
  oem: [],
  elementName: [],
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

const SoftwareConfigurationsTable: React.FC<Props> = (props) => {
  //STATE CONFIRM
  if (props.Opco.length > 0) {
    paginationQuery.opCo = props.Opco;
  }
  if (props.Oem.length > 0) {
    paginationQuery.oem = props.Oem;
  }
  if (props.Ele.length > 0) {
    paginationQuery.elementName = props.Ele;
  }
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [orphanColor, setOrphanColor] = useState(false);
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [swoEm, setSwoEm] = useState<{ [key: string]: string }>({});
  const [opco, setOpco] = useState<{ [key: string]: string }>({});
  const [oem, setOem] = useState<{ [key: string]: string }>({});
  const [ele, setEle] = useState<{ [key: string]: string }>({});

  const [elementName, setElementName] = useState<{ [key: string]: string }>({});
  const [onSelect, setOnSelect] = useState<boolean>(false);
  const [viewReport, setViewReport] = useState<boolean>(false);
  const [selectedOpcoValue, setSelectedOpcoValue] = useState<string[]>([]);
  const [selectedOemValue, setSelectedOemValue] = useState<string[]>([]);
  const [selectedEleValue, setSelectedEleValue] = useState<string[]>([]);
  const [functionAreaFlag, setFunctionAreaFlag] = useState<boolean>(false);
  const [functionPaginationQuery, setFunctionPaginationQuery] = useState<any>(
    []
  );
  const { KPIAdmin, admin } = useAuth();
  const [filterType, setFilterType] = useState<number | undefined>(0);
  const [selected, setSelected] = useState<
    { label: string; value: number }[] | undefined
  >(undefined);

  // const {
  //     queryHardware: queryPAT,
  //     setQueryHardware: setQueryPAT,
  //     nextHardware: nextPAT,
  //     backHardware: backPAT,
  //   } = useGenerateLcmDbHardware(paginationQuery, GetReportPATGrid);
  //DTO
  const [data, setData] = useState<SoftwareConfigurationDtoGrid[] | undefined>(
    []
  );
  let GridDto = useSelector(
    (state: RootState) =>
      state.softwareConfigurationGridReducer.SoftwareConfigurationGridResult
  );

  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso } = useAuth();

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetSoftwareConfigurationGrid : undefined
  );

  // const result = useFilterTableCrud<SoftwareConfigurationQueryObjectGrid>(
  //   setQuery,
  //   GetFilterColumSoftwareConfiguration,
  //   query
  // );

  let filterDataOpco = useSelector(
    (state: RootState) => state.softwareConfigurationOpcoReducer.filter
  );
  let filterDataOem = useSelector(
    (state: RootState) => state.softwareConfigurationOemReducer.filter
  );
  let filterDataEle = useSelector(
    (state: RootState) => state.softwareConfigurationEleReducer.filter
  );

  // const result = rootStore.dispatch(GetFilterColumSoftwareConfiguration('opco','text'))

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetSoftwareConfigurationGrid");
    closeModal();
    GetSoftwareConfigurationGrid(query).then(() =>
      setLoader("REMOVE", "GetSoftwareConfigurationGrid")
    );
  };

  const getSwoEm = async () => {
    let result = await GetSWOem();
    setSwoEm(result?.SWOemResources!);
  };

  const GetOpco = async () => {
    // let result = useFilterTableCrud<SoftwareConfigurationQueryObjectGrid>(
    //   setQuery,
    //   GetFilterColumSoftwareConfiguration,
    //   query
    // );
    const result = GetFilterOpcoSoftwareConfiguration("opCo", "");
    //const result = () => dispatch(GetFilterColumSoftwareConfiguration('opco','text'))
    const res = filterDataOpco || [];

    //const finalValue = res.map((item,index)=>({index,value:item.value}))
    const newObj = {};
    res.forEach((item, index) => {
      newObj[index] = item.value;
    });
    setOpco(newObj);
  };

  const GetOem = async () => {
    const result = GetFilterOemSoftwareConfiguration("oem", "");
    const res = filterDataOem || [];
    const newObj = {};
    res.forEach((item, index) => {
      newObj[index] = item.value;
    });
    setOem(newObj);
  };

  const GetEle = async () => {
    const result = GetFilterEleSoftwareConfiguration("elementName", "");
    const res = filterDataEle || [];
    const newObj = {};
    res.forEach((item, index) => {
      newObj[index] = item.value;
    });
    setEle(newObj);
  };

  const selectedOpco = (e: any) => {
    let arr: string[] = [];
    if (e) {
      setOnSelect(true);
      arr.push(e.label);
    }
    setSelectedOpcoValue(arr);
    return;
  };

  const selectedOem = (e: any) => {
    let arr: string[] = [];
    if (e) {
      setOnSelect(true);
      arr.push(e.label);
    }
    setSelectedOemValue(arr);

    return;
  };

  const selectedEle = (e: any) => {
    let arr: string[] = [];
    if (e) {
      setOnSelect(true);
      arr.push(e.label);
    }
    setSelectedEleValue(arr);
    return;
  };

  const setFilterSelected = (e: any) => {
    if (e) {
      setFilterType(e.key);
      GetVendorsOfPredefinedFilter(e.key).then((res) => {
        const selectedId = dictionaryToArray(res).map((item) => +item.value);
        const selectedList = dictionaryToArray(swoEm)
          .filter((item) => selectedId.includes(+item.key))
          .map((el) => ({ label: el.value, value: el.key }));

        setSelected(selectedList);
      });
      return;
    }

    setFilterType(undefined);
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

  const resetQuery = () => {
    //  ;
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

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

  const EditNotDetail = (id: number) => {
    setLocalState({
      id: id,
      tab: "networkelement",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(id);
    // setDetailId(idDetail);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetSoftwareConfigurationGrid");
      //getSwoEm();
      // GetOpco();
      // GetOem();
      // GetEle();
      //GetElementName();
    }
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetSoftwareConfigurationGrid");
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
        let copy = { ...query } as SoftwareConfigurationQueryObjectGrid;
        copy.softwarewareconfigurationid = [];
        copy.softwarewareconfigurationid?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
        GetSoftwareConfigurationGrid(copy).then((x) =>
          setLoader("REMOVE", "GetSoftwareConfigurationGrid")
        );
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    } else {
      // GetSoftwareConfigurationGrid(paginationQuery).then((x) =>
      //   setLoader("REMOVE", "GetSoftwareConfigurationGrid")
      // );
    }
    setLoader("REMOVE", "GetSoftwareConfigurationGrid");
  }, []);

  useEffect(() => {
    if (props.Opco) {
      paginationQuery.opCo = props.Opco;
    }
    if (props.Oem) {
      paginationQuery.oem = props.Oem;
    }
    if (props.Ele) {
      paginationQuery.elementName = props.Ele;
    }
    setLoader("ADD", "GetSoftwareConfigurationGrid");
    let copy = { ...paginationQuery } as SoftwareConfigurationQueryObjectGrid;
    setQuery(copy);
    // GetSoftwareConfigurationGrid(copy).then((x) =>
    //   setLoader("REMOVE", "GetSoftwareConfigurationGrid")
    // );
  }, [props.Ele, props.Oem, props.Opco]);

  const closeModalSetup = (changed: boolean) => {
    GetSoftwareConfigurationGrid(query).then((x) =>
      setIsVisibleModalSetup(false)
    );
  };

  const InvocheDownload = async () => {
    let result = await GetSoftwareConfigurationReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const onDelete = async (id: number) => {
    const result = await GetRelatedRecordsNetworkElementAsIs(id);
    if (result.data != null) {
      setIsVisibleModalRelated(true);
      setRelatedRecord(result.data);
    } else {
      Delete(id);
    }
  };
  const onOpenFunction = async (item: SoftwareConfigurationDtoGrid) => {
    let copy = {
      ...paginationQuery,
      swConfigFunctionAreaId: [item.swConfigFunctionAreaId],
      opCo: [],
    } as SoftwareConfigurationQueryObjectGrid;
    // setQuery(copy);
    setFunctionPaginationQuery(copy);
    if (
      item.swConfigFunctionAreaId !== 0 &&
      item.swConfigFunctionAreaId !== undefined
    ) {
      // const result = await GetSubFuntionFilter(copy);
      setFunctionAreaFlag(true);
      // console.log(result);
    }
  };
  return (
    <>
      <Modal
        show={functionAreaFlag}
        onHide={() => setFunctionAreaFlag(false)}
        backdrop="static"
        keyboard={false}
        size="xl"
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12">
            <h4> View Sub Function Area</h4>
          </div>
        </Modal.Header>
        <Modal.Body>
          <SubFunctionAreaContainer
            paginationQuery={functionPaginationQuery}
            action={{
              closeModal: () => {
                functionPaginationQuery([]);
                setFunctionAreaFlag(false);
              },
            }}
          />
        </Modal.Body>
      </Modal>
      <div className="pageContainer">
        <div className="mt-4">
          <div className="row">
            <div className="col-12">
              <div>
                <div className="mt-30 fr d-flex">
                  <button
                    //className="px-4 reportBtn voda-bold"
                    className="px-4 btnHeader clearBtn br-20 voda-bold"
                    type="button"
                    onClick={() => InvocheDownload()}
                  >
                    Download to XLS
                  </button>

                  <Dropdown className="d-inline more-options">
                    <Dropdown.Toggle id="dropdown-autoclose-inside">
                      More Options
                    </Dropdown.Toggle>

                    <Dropdown.Menu>
                      <Dropdown.Item
                        onClick={() => setIsVisibleModalSetup(true)}
                      >
                        Manage Table Content
                      </Dropdown.Item>
                    </Dropdown.Menu>
                  </Dropdown>
                </div>
              </div>
            </div>
          </div>
          <ModalConfirm data={confirm} />
          <Modal
            show={isVisibleModalSetup}
            backdrop="static"
            keyboard={false}
            size="lg"
          >
            <Modal.Header className="d-flex justify-content-center">
              <div className="col-12 px-0">
                <div className="col-12">
                  <h4 className="mb-0 mt-1">Setup Grid Informations</h4>
                </div>
                {/* <ErrorNotification OnModal={true} /> */}
              </div>
            </Modal.Header>
            <Modal.Body className="plr-30">
              <SetupColumns
                renderGrid={renderGridState}
                action={{ closeModalSetup }}
                tab={""}
              ></SetupColumns>
            </Modal.Body>
          </Modal>
          <div className="mt-4">
            <SoftwareConfigurationGrid
              data={data}
              pagination={query}
              orphanColor={orphanColor}
              renderGrid={renderGridState?.render ?? []}
              action={{
                onDelete,
                EditNotDetail,
                EditAndDetail,
                Filter: setQuery,
                Restore,
                setIsFiltriAttivati,
                onOpenFunction,
              }}
            ></SoftwareConfigurationGrid>

            <Paginate
              pagination={{ page: query.page, pageSize: query.pageSize }}
              totalItems={GridDto?.totalItems}
              actions={{ next, back }}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default SoftwareConfigurationsTable;
