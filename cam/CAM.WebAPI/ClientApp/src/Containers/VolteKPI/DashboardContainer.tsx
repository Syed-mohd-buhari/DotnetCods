import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import AdditionalFiltersMenu from "../../Components/AdditionalFiltersMenu";
import ModalConfirm from "../../Components/ModalConfirm";
import Pagination from "../../Components/PaginationComponent";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useResourceTableCrudForVolteKPI } from "../../Hook/useResourceTableCrudForVolteKPI";
import {
  CustomGridRenderOfVolteKPIDtoGrid,
  VolteKPIDtoCreate,
  VolteKPIDtoGrid,
  VolteKPIDtoUpdate,
  VolteKPIQueryObjectGrid,
} from "../../Model/VolteKpi/VolteKPI";
import setLoader from "../../Redux/Action/LoaderAction";
import {
  CreatVolteKPI,
  GetVolteKPICreateResource,
} from "../../Redux/Action/VolteKPI/VolteKPICreateAction";
import {
  deleteVolteKPI,
  RestoreVolteKPI,
} from "../../Redux/Action/VolteKPI/VolteKPIDeleteAction";
import { GetVolteKPIEditResource } from "../../Redux/Action/VolteKPI/VolteKPIEditAction";
import { GetVolteKPIGrid } from "../../Redux/Action/VolteKPI/VolteKPIGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { CommonValidation } from "../../screen/SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import Dashboard from "../../screen/VolteKPI/Dashboard/Dashboard";
import DashboardGrid from "../../screen/VolteKPI/Dashboard/DashboardGrid";
import ModalKPI from "../../screen/VolteKPI/Dashboard/ModalKPI";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { useAuth } from "../../Hook/useAuth";

export let paginationQuery = {
  volteKPIId: [],
  opCo: [],
  volteKPITypes: [1, 2, 3],
  month: new Date().getMonth() + 1, //Mese Corrente
  year: new Date().getFullYear(), //Anno Corrente
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
} as VolteKPIQueryObjectGrid;

const VolteContainer: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  //DTO
  // const [selectedOpcos, setSelectedOpcos] = useState<string[] | undefined>([]);
  const [dataToSend, setDataToSend] = useState<VolteKPIDtoUpdate>({});
  const [targetValueChangeProposal, setTargetValueChangeProposal] =
    useState<number>();

  // const Grid = (state: RootState) => state.volteKPIGridReducer.VolteKPIGridResult;
  // const GridDto = useSelector(Grid);
  const [orphanColor, setOrphanColor] = useState(false);

  const dtoNewResourceState = (state: RootState) =>
    state.volteKPICreateReducer.VolteKPIDtoCreate;
  const dtoEditResourceState = (state: RootState) =>
    state.volteKPIEditReducer.VolteKPIDtoEdit;

  let createResource = useSelector(dtoNewResourceState);

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRenderOfVolteKPIDtoGrid | undefined
  >();
  const [KPITypeForEdit, setKPITypeForEdit] = useState<number>();

  const [comment, setComment] = useState("");

  const { pageSize } = useAuth();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const refresh = () => {
    closeModal();
    GetVolteKPIGrid(query);
  };

  const { darkMode } = useTheme();

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery } = useResourceTableCrudForVolteKPI(
    paginationQuery,
    GetVolteKPIGrid
  );
  const { validation, setValidation, confirmForm, setChanged, setInputValue } =
    useFormTableCrud<VolteKPIDtoCreate>(
      GetVolteKPICreateResource,
      GetVolteKPIEditResource
    );

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const {
    New,
    Edit,
    confirm,
    closeModal,
    Delete,
    localStateHistory,
    setLocalState,
    Restore,
  } = useOperationTableCrud<VolteKPIDtoUpdate, VolteKPIDtoCreate>(
    GetVolteKPICreateResource,
    GetVolteKPIEditResource,
    deleteVolteKPI,
    refresh,
    RestoreVolteKPI
  );

  //UPDATE ON CHANGE DTO
  // useEffect(() => {
  // 	if (GridDto !== null) {
  // 		setDataGrid(GridDto?.items);
  // 	}
  // }, [GridDto]);

  const [isVisibleModalAdd, setIsVisibleModalAdd] = useState<boolean>(false);
  const [isVisibleChangingModal, setIsVisibleChangingModal] =
    useState<boolean>(false);
  const [isVisibleModalEdit, setIsVisibleModalEdit] = useState<boolean>(false);

  const onSave = async (changeProposal: boolean, data: VolteKPIDtoCreate) => {
    // if (changeProposal) {
    // 	data && setDataToSend(data);
    // 	setIsVisibleChangingModal(true);
    // } else {
    // 	setTargetValueChangeProposal(undefined);
    // 	setComment("");
    // 	setIsVisibleModalAdd(false);
    // 	await CreatVolteKPI(data, false);
    // 	refresh();
    // }
    setTargetValueChangeProposal(undefined);
    setIsVisibleModalAdd(false);
    setIsVisibleModalEdit(false);
    await CreatVolteKPI(data, false);
    refresh();
  };

  useEffect(() => {
    if (isVisibleModalAdd) {
      New();
    }
  }, [isVisibleModalAdd]);

  const changeTargetValueChangeProposal = (value: string | undefined) => {
    //Rimuovi Validazione
    if (validation?.property?.includes("targetMonthlyApproval")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("targetMonthlyApproval");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    const newTargetValue =
      value != undefined && value != "" && !isNaN(+value)
        ? +parseFloat(value).toFixed(3)
        : undefined;
    setTargetValueChangeProposal(newTargetValue);
  };

  const OnSaveChangingModal = () => {
    const isValid = validazioneClient().response;
    if (isValid) {
      let copy = { ...dataToSend } as VolteKPIDtoUpdate;

      switch (copy.volteKPIType) {
        case 1:
          copy.kpiOneTargetValueChangeProposal = targetValueChangeProposal;
          copy.kpiOneComment = comment;
          break;
        case 2:
          // copy.kpiTwoTargetValueChangeProposal = targetValueChangeProposal;
          // copy.kpiTwoComment = comment;
          break;
        case 3:
          copy.kpiThreeTargetValueChangeProposal = targetValueChangeProposal;
          copy.kpiThreeComment = comment;
          break;
        case 4:
          copy.kpiFourTargetValueChangeProposal = targetValueChangeProposal;
          copy.kpiFourComment = comment;
          break;

        default:
          break;
      }
      setDataToSend(copy);
      onSave(false, copy);

      setIsVisibleChangingModal(false);
    }
  };

  const validazioneClient = () => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      targetValueChangeProposal == undefined ||
      targetValueChangeProposal == null
    ) {
      addInvalidProperty("targetMonthlyApproval");
    }
    if (
      targetValueChangeProposal != undefined &&
      targetValueChangeProposal >= 1000000
    ) {
      addInvalidProperty("targetMonthlyApproval");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  return (
    <div className="pageContainer">
      <ModalConfirm data={confirm} />
      {/* <Modal show={isVisibleModalAdd} backdrop="static" backdropClassName="backdropGrid" dialogClassName="dialogGrid" className="modalGrid" keyboard={false} size="xl" centered> */}
      <Dialog
        open={isVisibleModalAdd}
        onClose={() => setIsVisibleModalAdd(false)}
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
              <h4 className="mb-0">Add KPI</h4>
            </div>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalAdd(false)}
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
          <ModalKPI
            action={{
              closeModal: () => setIsVisibleModalAdd(false),
              onSave: (changeProposal, data) => onSave(changeProposal, data),
            }}
            edit={false}
          />
        </DialogContent>
      </Dialog>

      <Modal
        show={isVisibleChangingModal}
        backdropClassName="backdropLookup"
        dialogClassName="modalLookup"
        className="modalLookup"
        keyboard={false}
        size="xl"
        centered
      >
        <Modal.Header>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Changing the Monthly Target Form</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <div className="col-12 row pb-2">
            <div className="col-6">
              <div className="form-group">
                <label className="themeText voda-bold mb-0 w-100">
                  New Target Monthly Value
                  <input
                    type="number"
                    onChange={(e) =>
                      changeTargetValueChangeProposal(e.target.value)
                    }
                    onKeyUp={(e) =>
                      changeTargetValueChangeProposal(e.currentTarget.value)
                    }
                    className="inputForm w-100"
                    value={targetValueChangeProposal}
                  />
                </label>
                {validation &&
                validation.response === false &&
                validation.property?.includes("targetMonthlyApproval") ? (
                  <label className="validation">
                    {targetValueChangeProposal &&
                    targetValueChangeProposal >= 1000000
                      ? "This number must be smaller than 1.000.000 "
                      : "*This Field cannot be empty"}
                  </label>
                ) : null}
              </div>
            </div>
            <div className="col-6">
              <div className="form-group">
                <label className="themeText voda-bold   mb-0 w-100">
                  Comments:
                  <input
                    type="text"
                    onChange={(e) => setComment(e.target.value)}
                    onKeyUp={(e) => setComment(e.currentTarget.value)}
                    className="inputForm w-100"
                    value={comment}
                  />
                </label>
              </div>
            </div>
          </div>
          <div className="col-12 justify-content-end mt-4 d-flex footerModal">
            <button
              className="  voda-bold btn btn-link px-4 btnHeader cancel"
              onClick={() => {
                setComment("");
                setTargetValueChangeProposal(0);
                setIsVisibleChangingModal(false);
              }}
              type="button"
            >
              Cancel
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={OnSaveChangingModal}
            >
              Submit for Approval
            </button>
          </div>
        </Modal.Body>
      </Modal>

      <Modal
        show={isVisibleModalEdit}
        backdrop="static"
        backdropClassName="backdropGrid"
        dialogClassName="dialogGrid"
        className="modalGrid"
        keyboard={false}
        size="lg"
        centered
      >
        <Modal.Header>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Edit KPI</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <ModalKPI
            kpiTypeForEdit={KPITypeForEdit}
            action={{
              closeModal: () => setIsVisibleModalEdit(false),
              onSave: (changeProposal, data) => onSave(changeProposal, data),
            }}
            edit={true}
          />
        </Modal.Body>
      </Modal>

      <div className="headerPage col-12 row mx-0 justify-content-between mb-4">
        <div className="d-flex flex-row align-items-center">
          {redirect === true ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2"
              to={{ pathname: localStateHistory?.prevPage }}
            >
              <GoArrowLeft
                onClick={() => navigate(-1)}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          <h3 className="voda-bold">VoLTE KPI Dashboard</h3>
        </div>
      </div>

      <Dashboard
        query={query}
        action={{
          setQuery,
          openModal: () => {
            setIsVisibleModalAdd(true);
          },
        }}
        edit={false}
      />

      <div className="mt-4 col-12 px-3">
        <DashboardGrid
          renderGrid={renderGridState?.render ?? []}
          action={{
            Delete,
            Edit: (parameters) => GetVolteKPICreateResource(parameters),
            Filter: setQuery,
            Restore,
            openModalEdit: (idKPIType) => {
              setKPITypeForEdit(idKPIType);
              setIsVisibleModalEdit(true);
            },
          }}
          pagination={query}
        />
      </div>
    </div>
  );
};

export default VolteContainer;
