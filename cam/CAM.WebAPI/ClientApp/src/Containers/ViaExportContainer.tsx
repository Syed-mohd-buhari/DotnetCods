import React, { useEffect, useState } from "react";
import { Tabs, Tab, Modal, Dropdown } from "react-bootstrap";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import Paginate from "../Components/PaginationComponent";
import { GetViaExportHardwareGrid } from "../Redux/Action/ViaExport/ViaExportHardwareGridAction";
import {
  DownloadViaReport,
  DownloadCSV,
} from "../Redux/Action/ViaExport/ViaExportDownloadAction";
import { SkipManager, ViaExportQuery } from "../Model/ViaExport/ViaExport";
import { GetViaExportSoftwareGrid } from "../Redux/Action/ViaExport/ViaExportSoftwareGridAction";
import { useViaExportHardware } from "../Hook/useViaExport/useViaExportHardware";
import { useViaExportSoftware } from "../Hook/useViaExport/useViaExportSoftware";
import SetupColumns from "../screen/Shared/SetupColumns";
import { CustomGridRender } from "../Model/Common";
import ViaExportSoftware from "../screen/ViaExport/ViaExportSoftware";
import ViaExportHardware from "../screen/ViaExport/ViaExportHardware";
import { ViaExport } from "../Model/ViaExport/ViaExport";
import { ReportViaQueryAllDto } from "../Model/ViaExport/ExportViaExport";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import ModalConfirm from "../Components/ModalConfirm";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { useAuth } from "./../Hook/useAuth";

export let paginationQueryHardware: ViaExportQuery = {
  meSourceAssetId: [],
  meName: [],
  meType: [],
  lcmProdName: [],
  vendor: [],
  eoslContractDate: undefined,
  eeoslContractDate: undefined,
  locationName: [],
  meDescription: [],
  meStatus: [],
  meIpAddress: [],
  meVirtualFlg: [],
  organisationName: [],
  osName: [],
  osEoslContractDate: undefined,
  osEeoslContractDate: undefined,
  meCniBcServiceFlg: [],
  meCriticality: [],
  meDeploymentType: [],
  meEnvironment: [],
  meExternalConnectionFlg: [],
  meFqdn: [],
  meGdprRelevantFlg: [],
  meInstallationDate: undefined,
  meLastMajorUpgradeDate: undefined,
  meLastScanDate: undefined,
  meMacAddress: [],
  meSerialNumber: [],
  meServiceType: [],
  meCyberarkIntegrationFlg: [],
  meIdmIntegrationFlg: [],
  meSecurityTier: [],
  me2FaIntegrationFlg: [],
  meSiemIntegrationFlg: [],
  supportContract: [],
  lcmStatus: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  skipAmount: undefined,
  skipManager: [] as SkipManager[],
};

export let paginationQuerySoftware: ViaExportQuery = {
  meSourceAssetId: [],
  meName: [],
  meType: [],
  lcmProdName: [],
  vendor: [],
  eoslContractDate: undefined,
  eeoslContractDate: undefined,
  locationName: [],
  meDescription: [],
  meStatus: [],
  meIpAddress: [],
  meVirtualFlg: [],
  organisationName: [],
  osName: [],
  osEoslContractDate: undefined,
  osEeoslContractDate: undefined,
  meCniBcServiceFlg: [],
  meCriticality: [],
  meDeploymentType: [],
  meEnvironment: [],
  meExternalConnectionFlg: [],
  meFqdn: [],
  meGdprRelevantFlg: [],
  meInstallationDate: undefined,
  meLastMajorUpgradeDate: undefined,
  meLastScanDate: undefined,
  meMacAddress: [],
  meSerialNumber: [],
  meServiceType: [],
  meCyberarkIntegrationFlg: [],
  meIdmIntegrationFlg: [],
  meSecurityTier: [],
  me2FaIntegrationFlg: [],
  meSiemIntegrationFlg: [],
  supportContract: [],
  lcmStatus: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
};

const ViaExportContainer: React.FC = (props) => {
  //PAGE
  const [keyTabs, setKeyTabs] = useState("hardware");
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const { readonly, isPermesso } = useAuth();

  //DTO HARDWARE
  const [dataHardware, setDataHardware] = useState<ViaExport[] | undefined>([]);
  let GridDtoHardware = useSelector(
    (state: RootState) =>
      state.viaExportHardwareGridReducer.ViaExportHardwareGridResult
  );

  //DTO SOFTWARE
  const [dataSoftware, setDataSoftware] = useState<ViaExport[] | undefined>([]);
  let GridDtoSoftware = useSelector(
    (state: RootState) =>
      state.viaExportSoftwareGridReducer.ViaExportSoftwareGridResult
  );

  const { queryHardware, setQueryHardware, nextHardware, backHardware } =
    useViaExportHardware(
      paginationQueryHardware,
      isPermesso ? GetViaExportHardwareGrid : undefined
      // GridDtoHardware?.skipManager ?? ([] as SkipManager[])
    );
  const { querySoftware, setQuerySoftware, nextSoftware, backSoftware } =
    useViaExportSoftware(
      paginationQuerySoftware,
      isPermesso ? GetViaExportSoftwareGrid : undefined
    );

  const [queryAll, setQueryAll] = useState<ReportViaQueryAllDto>({});

  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);

  const [renderGridStateHw, setRenderGridStateHw] = useState<
    CustomGridRender | undefined
  >();

  const [renderGridStateSw, setRenderGridStateSw] = useState<
    CustomGridRender | undefined
  >();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1

  const InvocheDownload = async () => {
    setConfirm({
      title: "Export",
      message: "export_form",
      button: "Continue",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: async (type: string) => {
          console.log(type);
          if (!type) {
            rootStore.dispatch(
              setNotification({
                message: "Select Your Export Type!",
                notifyType: NotifyType.warning,
              })
            );
          }

          if (type === "excel") {
            let result = await DownloadViaReport(queryAll);
            if (result !== undefined) {
              let url = window.URL.createObjectURL(result.file);
              let a = document.createElement("a");
              a.href = url;
              a.download = result.fileName;
              a.click();
              setConfirm(stateConfirm);
            }
          }

          if (type === "csv") {
            let result = await DownloadCSV(queryAll, keyTabs);
            if (result !== undefined) {
              let url = window.URL.createObjectURL(result.file);
              let a = document.createElement("a");
              a.href = url;
              a.download = result.fileName;
              a.click();
              setConfirm(stateConfirm);
            }
          }
        },
      },
    } as DataModalConfirm);
  };

  useEffect(() => {
    let copy = { ...queryAll } as ReportViaQueryAllDto;
    if (queryHardware && querySoftware && isPermesso) {
      copy.queryHardware = queryHardware;
      copy.querySoftware = querySoftware;
      setQueryAll(copy);
    }
  }, [queryHardware, querySoftware]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDtoHardware != undefined) {
      setDataHardware(GridDtoHardware?.items);
      let copy = { ...GridDtoHardware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateHw(copy);
    }
  }, [GridDtoHardware]);

  useEffect(() => {
    if (GridDtoSoftware != undefined) {
      setDataSoftware(GridDtoSoftware?.items);
      let copy = { ...GridDtoSoftware?.gridRender } as
        | CustomGridRender
        | undefined;
      setRenderGridStateSw(copy);
    }
  }, [GridDtoSoftware]);

  const closeModalSetup = (changed: boolean) => {
    GetViaExportHardwareGrid(queryHardware).then((x) =>
      GetViaExportSoftwareGrid(querySoftware).then((x) =>
        setIsVisibleModalSetup(false)
      )
    );
  };

  const onApiCallChange = (hwQuery, swQuery) => {
    if (hwQuery !== null) {
      setQueryHardware(hwQuery);
    }
    if (swQuery !== null) {
      setQuerySoftware(swQuery);
    }
  };

  return (
    <div className="pageContainer">
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
          </div>
        </Modal.Header>
        <Modal.Body className="plr-30">
          <SetupColumns
            renderGrid={
              keyTabs === "hardware" ? renderGridStateHw : renderGridStateSw
            }
            action={{ closeModalSetup: () => setIsVisibleModalSetup(false) }}
          ></SetupColumns>
        </Modal.Body>
      </Modal>

      <ModalConfirm data={confirm}></ModalConfirm>

      <div className="headerPage row mx-0 justify-content-between">
        <h3 className="voda-bold">Vai Export</h3>
        <div className="d-flex">
          <button
            className="download-to-excel mrl-10 grid-main-btn
            "
            onClick={() => InvocheDownload()}
          >
            {/* <img src={require("../img/excel.png")} /> */}
            Download to Excel
          </button>
          <Dropdown
            className="d-inline more-options grid-main-btn
"
          >
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu
              className="grid-main-btn
"
            >
              <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                Manage Table Content
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>

          {/* <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            type="button"
            onClick={() => InvocheDownload()}
          >
            <img
              alt="Download"
              width="26"
              style={{ marginBottom: "2px" }}
              src={require("../img/excel.png")}
            />
          </button>
          <button
            className="  voda-bold btn btn-danger ml-2 px-4 btnHeader"
            onClick={() => setIsVisibleModalSetup(true)}
            type="button"
          >
            <img
              style={{ height: 24, marginBottom: "3px" }}
              src={require("../img/settings.png")}
              alt="setting"
            />
          </button> */}
        </div>
      </div>
      <Tabs
        defaultActiveKey="hardware"
        id="viaExport"
        activeKey={keyTabs}
        onSelect={(x) => setKeyTabs(x || "")}
      >
        <Tab eventKey="hardware" title="Hardware">
          {keyTabs === "hardware" && (
            <>
              <ViaExportHardware
                data={dataHardware}
                pagination={queryHardware}
                renderGrid={renderGridStateHw?.render ?? []}
                action={{
                  Filter: setQueryHardware,
                }}
              ></ViaExportHardware>
              <Paginate
                pagination={{
                  page: queryHardware.page,
                  pageSize: queryHardware.pageSize,
                }}
                totalItems={GridDtoHardware?.totalItems}
                actions={{ next: nextHardware, back: backHardware }}
              />
            </>
          )}
        </Tab>
        <Tab eventKey="software" title="Software">
          {keyTabs === "software" && (
            <>
              <ViaExportSoftware
                data={dataSoftware}
                pagination={querySoftware}
                renderGrid={renderGridStateSw?.render ?? []}
                action={{
                  Filter: setQuerySoftware,
                }}
              ></ViaExportSoftware>
              <Paginate
                pagination={{
                  page: querySoftware.page,
                  pageSize: querySoftware.pageSize,
                }}
                totalItems={GridDtoSoftware?.totalItems}
                actions={{ next: nextSoftware, back: backSoftware }}
              />
            </>
          )}
        </Tab>
      </Tabs>
    </div>
  );
};

export default ViaExportContainer;
