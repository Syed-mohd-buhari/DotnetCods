import React, { useEffect, useState } from "react";
import { Alert, Dropdown, Form, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import ModalConfirm from "../Components/ModalConfirm";
import ModalRelated from "../Components/ModalRelated";
import Paginate from "../Components/PaginationComponent";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useAuth } from "../Hook/useAuth";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import {
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../Model/Common";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import {
  AuditDtoCreate,
  AuditDtoGrid,
  AuditDtoUpdate,
  AuditQueryObjectGrid,
} from "../Model/Audit";
import setLoader from "../Redux/Action/LoaderAction";
import { GetNetworkElementAsIsCreateResource } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsCreateAction";
import {
  DeleteDeepNetworkElementAsIs,
  GetRelatedRecordsNetworkElementAsIs,
  RestoreNetworkElementAsIs,
} from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsDeleteAction";
import { GetAuditReport } from "../Redux/Action/Audit/AuditDownloadAction";
import { GetNetworkElementAsIsEditResource } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsEditAction";
import {
  GetAuditApproveStatus,
  GetAuditGrid,
  GetAuditOverrideStatus,
  GetAuditRejectStatus,
} from "../Redux/Action/Audit/AuditGridAction";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import AuditGrid from "../screen/Audit/AuditGrid";
import NetworkElementAsIsModal from "../screen/NetworkElementAsIs/NetworkElementAsIsModal";
import SetupColumns from "../screen/Shared/SetupColumns";
import {
  CloneReportStatus,
  DeleteReport,
  DownloadGenericCSVReport,
  DownloadGenericReport,
  GetGenericReportGrid,
  PreviewReport,
  UpdateReportStatus,
} from "../Redux/Action/GenericReport/GenericReportCommonAction";
import {
  GenericReportDtoGrid,
  GenericReportQueryObjectGrid,
} from "../Model/GenericReport";
import PreviewReportGrid from "../screen/GenericReport/PreviewReportGrid";
import ReportListGrid from "../screen/GenericReport/ReportListGrid";
import ViewReport from "../screen/GenericReport/ViewReport";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

export let paginationQuery: GenericReportQueryObjectGrid = {
  sortBy: "",
  isSortAscending: true,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
  dynamicReportId: [],
  userId: [],
  jsonGridCustomizationData: [],
  published: [],
  isScheduled: undefined,
  creationUser: [],
  creationDate: undefined,
  modificationUser: [],
  modificationDate: undefined,
};

const ReportListContainer: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalPreview, setIsVisibleModalPreview] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [viewReportId, setViewReportId] = useState<number>();
  const [viewReportName, setViewReportName] = useState<string>("");
  const [orphanColor, setOrphanColor] = useState(false);
  const [isVisibleLegenda, setVisibleLegenda] = useState<boolean>(false);
  const [nonAdminLength, setNonAdminLength] = useState<number>();
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [cloneReportName, setCloneReportName] = useState("");
  const [cloneVisibleModal, setCloneVisibleModal] = useState(false);
  const [reportDetails, setReportDetails] = useState<any>();
  const [excelPopup, setExcelPopup] = useState(false);
  const [excelPopupData, setExcelPopupData] = useState<{
    id: number | undefined;
    type: string;
    mode: string;
    queryList: any;
  }>({
    id: undefined,
    type: "excel",
    mode: "disaggregated",
    queryList: undefined,
  });
  const {
    tipologicaPermesso,
    VerifyIsInRole,
    KPIAdmin,
    KPIEditor,
    admin,
    simpleUser,
    readonly,
    role,
    isPermesso,
    pageSize,
  } = useAuth();
  //DTO
  const [data, setData] = useState<GenericReportDtoGrid[] | undefined>([]);
  const Grid = (state: RootState) =>
    state.genericReportGridReducer.GenericReportGridResult;
  let GridDto = useSelector(Grid);
  // console.log("Tems GridDto", GridDto);
  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();

  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  // console.log("Tems renderPreviewGridState", renderPreviewGridState);

  const [renderPreviewGridState, setPreviewRenderGridState] = useState<any>();

  // console.log("Tems renderGridState", renderGridState);

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetGenericReportGrid : undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetGenericReportGrid");
    closeModal();
    GetGenericReportGrid(query).then(() =>
      setLoader("REMOVE", "GetGenericReportGrid")
    );
  };

  const {
    New,
    Edit,
    isVisibleModal,
    edit,
    confirm,
    closeModal,
    Delete,
    localStateHistory,
    setLocalState,
    Restore,
  } = useOperationTableCrud<AuditDtoUpdate, AuditDtoCreate>(
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
      // Get Non Admin Data Length
      const nonAdimData = GridDto?.items?.filter(
        (x) => x.published === "Published"
      );
      if (GridDto?.totalItems !== undefined && nonAdimData !== undefined) {
        const length = GridDto?.totalItems - nonAdimData?.length;
        setNonAdminLength(length);
      }
      //
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetAuditGrid");
    }
  }, [GridDto]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetGenericReportGrid");
    if (location.state != null && location.state !== undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
        idDetail: number | string;
        ids?: number[];
      };
      if (localState?.id != null) {
        setLocalState(localState);
        setRedirect(true);
        setFilterRedirect(true);
        let copy = { ...query } as GenericReportQueryObjectGrid;
        copy.principalId = localState?.id;
        setQuery(copy);
        GetGenericReportGrid(copy).then((x) => {
          setLoader("REMOVE", "GetGenericReportGrid");
        });
      }
      if (localState.prevPage && localState.prevPage != "") {
        setPrevPage(localState.prevPage);
      }
    }
    setLoader("REMOVE", "GetGenericReportGrid");
  }, []);

  const previewReport = async (reportId: number, reportName: string) => {
    setViewReportId(reportId);
    setViewReportName(reportName);
    setIsVisibleModalPreview(true);
  };

  const closeModalSetup = (changed: boolean) => {
    if (isVisibleModalPreview) {
      setIsVisibleModalSetup(false);
    } else {
      GetGenericReportGrid(query).then((x) => setIsVisibleModalSetup(false));
    }
  };

  const onDelete = async (reportId: number, reportName: string) => {
    setMyConfirm({
      title: "Delete Report",
      message: `Are you sure, do you want to delete the report "${reportName}"?`,
      button: "Delete",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setMyConfirm(stateConfirm),
        confirm: async () => {
          setMyConfirm(stateConfirm);
          const result = await DeleteReport(reportId);
          if (result.warning === true) {
            setShow(false);
            refresh();
          } else {
            setAlertStatus({
              message: result?.["info"] || "",
              class: "danger",
            });
            setShow(true);
          }
        },
      },
    });
  };

  const onPublish = async (reportId: number, reportName: string) => {
    setMyConfirm({
      title: "Publish Report",
      message: `Are you sure, do you want to publish the report "${reportName}"?`,
      button: "Publish",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setMyConfirm(stateConfirm),
        confirm: async () => {
          setMyConfirm(stateConfirm);
          const result = await UpdateReportStatus(reportId);
          if (result.warning === true) {
            setShow(false);
            refresh();
          } else {
            setAlertStatus({
              message: result?.["info"] || "",
              class: "danger",
            });
            setShow(true);
          }
        },
      },
    });
  };

  const onCloneReport = (reportDetails: any) => {
    setReportDetails(reportDetails);
    setCloneVisibleModal(true);
  };

  const callCloneReport = () => {
    let payload = {
      dynamicReportsId: reportDetails?.dynamicReportsId,
      reportName: cloneReportName,
    };
    setMyConfirm({
      title: "Clone",
      message: `Are you sure, do you want to clone the report "${reportDetails?.reportName}" as "${cloneReportName}?`,
      button: "Yes",
      item: "",
      cancelText: "No",
      isOpen: true,
      actions: {
        cancel: () => setMyConfirm(stateConfirm),
        confirm: async () => {
          setMyConfirm(stateConfirm);
          const result = await CloneReportStatus(payload);
          if (result.warning === true) {
            setShow(false);
            setCloneVisibleModal(false);
            setCloneReportName("");
            refresh();
          } else {
            setAlertStatus({
              message: result?.["info"] || "",
              class: "danger",
            });
            setShow(true);
          }
        },
      },
    });
  };

  const DownloadReport = async () => {
    const { id, mode, type, queryList } = excelPopupData;
    const payload = {
      dynamicReportId: [id],
      ViewMode: mode === "aggregated" ? 1 : 2,
    };
    if (type === "excel") {
      let result = await DownloadGenericReport(queryList ?? payload);
      if (result !== undefined) {
        let url = window.URL.createObjectURL(result.file);
        let a = document.createElement("a");
        a.href = url;
        a.download = result.fileName;
        a.click();
        onCloseModal();
      }
    }

    if (type === "csv") {
      let result = await DownloadGenericCSVReport(queryList ?? payload);
      if (result !== undefined) {
        let url = window.URL.createObjectURL(result.file);
        let a = document.createElement("a");
        a.href = url;
        a.download = result.fileName;
        a.click();
        onCloseModal();
      }
    }
  };

  const onOverride = async (overrideList) => {
    const result = await GetAuditOverrideStatus(overrideList);
    if (result.warning === true) {
      setShow(false);
      refresh();
    } else {
      setAlertStatus({
        message: result?.["info"] || "",
        class: "danger",
      });
      setShow(true);
    }
  };

  const onCloseModal = () => {
    closeModal();
    setIsVisibleModalPreview(false);
    setViewReportId(undefined);
    setExcelPopup(false);
    setExcelPopupData({
      id: undefined,
      type: "excel",
      mode: "disaggregated",
      queryList: undefined,
    });
  };

  return (
    <div className="pageContainer">
      <ModalConfirm data={confirm} />
      <ModalConfirm data={myConfirm} />
      <Dialog
        open={excelPopup}
        onClose={() => setExcelPopup(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 px-0 mb-3">
            <div className="col-12">
              <h4 className="my-0">Download Report</h4>
            </div>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setExcelPopup(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <Form style={{ padding: "0px 10px " }}>
            {["radio"].map((type) => (
              <>
                <span className="my-4 voda-bold">
                  Please select Export Type.
                </span>
                <div key={`inline-excel-${type}`} className="mt-2 mb-3">
                  <Form.Check
                    inline
                    label="Excel"
                    value="excel"
                    name="group1"
                    type="radio"
                    id="Excel"
                    checked={excelPopupData?.type === "excel" ? true : false}
                    onChange={() =>
                      setExcelPopupData({ ...excelPopupData, type: "excel" })
                    }
                  />
                  <Form.Check
                    inline
                    label="CSV"
                    value="csv"
                    name="group1"
                    type="radio"
                    id="CSV"
                    checked={excelPopupData?.type === "csv" ? true : false}
                    onChange={() =>
                      setExcelPopupData({ ...excelPopupData, type: "csv" })
                    }
                  />
                </div>
                <span className="my-5 voda-bold">
                  Please select Report Type.
                </span>
                <div key={`inline-mode${type}`} className="mt-2">
                  <Form.Check
                    inline
                    label="Aggregated"
                    value="aggregated"
                    name="group2"
                    type="radio"
                    id="Aggregated"
                    checked={
                      excelPopupData?.mode === "aggregated" ? true : false
                    }
                    onChange={() =>
                      setExcelPopupData({
                        ...excelPopupData,
                        mode: "aggregated",
                      })
                    }
                  />
                  <Form.Check
                    inline
                    label="Disaggregated"
                    value="disaggregated"
                    id="Disaggregated"
                    name="group2"
                    type="radio"
                    checked={
                      excelPopupData?.mode === "disaggregated" ? true : false
                    }
                    onChange={() =>
                      setExcelPopupData({
                        ...excelPopupData,
                        mode: "disaggregated",
                      })
                    }
                  />
                </div>
              </>
            ))}
          </Form>
        </DialogContent>
        <div className="justify-content-end mt-4 d-flex footerModal">
          <button
            className="download-to-excel"
            onClick={() => setExcelPopup(false)}
          >
            Cancel
          </button>
          <button
            className="btn btn-danger mrl-10"
            onClick={() => DownloadReport()}
          >
            Download
          </button>
        </div>
      </Dialog>
      <Dialog
        open={isVisibleModalPreview}
        onClose={() => onCloseModal()}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">View Report</div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => onCloseModal()}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          {viewReportId !== undefined && (
            <ViewReport
              reportName={viewReportName}
              viewReportId={viewReportId}
              action={{
                closeModal: onCloseModal,
                onDownload: (id, tabMode, queryList) => {
                  setExcelPopupData({
                    ...excelPopupData,
                    id: id,
                    mode: tabMode,
                    queryList: queryList,
                  });
                  setExcelPopup(true);
                },
              }}
              isVisibleModal={isVisibleModalPreview}
            />
          )}
        </DialogContent>
        <div className="col-12 justify-content-end mt-4 d-flex footerModal">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() => onCloseModal()}
            type="button"
          >
            Cancel
          </button>
        </div>
      </Dialog>
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
      <Dialog
        open={cloneVisibleModal}
        onClose={() => setCloneVisibleModal(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0 mt-2">Clone Report</h4>
            </div>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setCloneVisibleModal(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <div className="headerPage row mx-0 mt-4 pt-2">
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold   w-100">
                  Current Report Name
                  <input
                    readOnly={true}
                    className="inputForm w-100 voda-regular"
                    type="text"
                    value={reportDetails?.reportName ?? ""}
                  />
                </label>
              </div>
            </div>
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm voda-bold   w-100">
                  Creation User
                  <input
                    readOnly={true}
                    className="inputForm w-100 voda-regular"
                    type="text"
                    value={reportDetails?.creationUser ?? ""}
                  />
                </label>
              </div>
            </div>
            <div className="col-12">
              <div className="form-group">
                <label className="labelForm w-100">
                  <label className="labelForm voda-bold mb-0">
                    Clone Report Name<span className="red">*</span>
                  </label>
                  <input
                    className="inputForm w-100 voda-regular"
                    type="text"
                    value={cloneReportName}
                    onChange={(e) => setCloneReportName(e.target.value)}
                    placeholder="Provide clone report name here..."
                  />
                  {cloneReportName.toLowerCase() ===
                    reportDetails?.reportName.toLowerCase() && (
                    <label className="validation">
                      Clone report name should not be same as current report
                      name.
                    </label>
                  )}
                </label>
              </div>
            </div>
          </div>
          <div className="col-12 justify-content-end mt-3 d-flex ">
            <button
              className="  voda-bold btn btn-link px-4 btnHeader cancel"
              onClick={() => setCloneVisibleModal(false)}
              type="button"
            >
              Cancel
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              disabled={
                cloneReportName.toLowerCase() !==
                  reportDetails?.reportName.toLowerCase() &&
                cloneReportName.length >= 1
                  ? false
                  : true
              }
              onClick={() => callCloneReport()}
            >
              Clone Report
            </button>
          </div>
        </DialogContent>
      </Dialog>
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">
            {" "}
            {admin ? "User Defined Reports" : "Published User Defined Reports"}
          </h3>
        </div>
        <div className="row mx-0 justify-content-between">
          <div className="d-flex">
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
          </div>
        </div>
      </div>
      {data && (
        <>
          <ReportListGrid
            data={data}
            pagination={query}
            orphanColor={orphanColor}
            renderGrid={renderGridState?.render ?? []}
            action={{
              onDelete,
              onPublish,
              onCloneReport,
              onDownload: (id, mode) => {
                setExcelPopupData({ ...excelPopupData, id: id, mode: mode });
                setExcelPopup(true);
              },
              onOverride,
              previewReport,
              EditNotDetail,
              EditAndDetail,
              Filter: setQuery,
              Restore,
              closeModal,
              setIsFiltriAttivati,
            }}
          ></ReportListGrid>
          <Paginate
            pagination={{ page: query.page, pageSize: query.pageSize }}
            totalItems={GridDto?.totalItems}
            actions={{ next, back }}
          />
        </>
      )}
    </div>
  );
};

export default ReportListContainer;
