import React, { useEffect, useState } from "react";
import { Alert, Dropdown } from "react-bootstrap";
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
import { RelatedRecordsResultDto, ResultDto } from "../Model/CommonModels";
import setLoader from "../Redux/Action/LoaderAction";
import { RootState, rootStore } from "../Redux/Store/rootStore";
import UserManagementGrid from "../screen/UserManagement/UserManagementGrid";
import NetworkElementAsIsModal from "../screen/NetworkElementAsIs/NetworkElementAsIsModal";
import SetupColumns from "../screen/Shared/SetupColumns";
import {
  ADD_NEW_USER_MANAGEMENT,
  UserManagementDtoCreate,
  UserManagementDtoEdit,
  UserManagementDtoGrid,
  UserManagementQueryObjectGrid,
} from "../Model/UserManagement";
import {
  DeleteUserManagement,
  DownloadUserManagementReport,
  CreateUserManagement,
  EditUserManagement,
  GetUserManagementGrid,
  RestoreUserManagement,
  UserManagementActivation,
  GetUserManagementRoles,
  AddNewRoleScreen,
} from "../Redux/Action/UserManagement/UserManagementGridAction";
import UserManagementModal from "../screen/UserManagement/UserManagementModal";
import UserManagementForm from "../screen/UserManagement/UserManagementForm";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import DialogActions from "@mui/material/DialogActions";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import { Modal, Button, Tab, Tabs } from "react-bootstrap";

import { IoClose } from "react-icons/io5";
import Menu from "../Components/Menu";
import MenuManagementModal from "../screen/UserManagement/MenuManagementModal";
import RoleMenuManagement from "../screen/UserManagement/RoleMenuManagement";

import TeamsGrid from "../screen/TeamManagement/TeamManagementGrid";
import TeamsForm from "../screen/TeamManagement/TeamManagementform";
import { QueryDtoforTeam, TeamsGridDto } from "../Model/TeamManagement";
import { GetTeamsGrid } from "../Redux/Action/TeamManagement/TeamManagementGridAction";
import { GetTeamsCreateResource } from "../Redux/Action/TeamManagement/TeamManagementCreateAction";
import { GetTeamsEditResource } from "../Redux/Action/TeamManagement/TeamManagementEditAction";

import { DownloadTeamManagementReport } from "../Redux/Action/TeamManagement/TeamManagementDownloadAction";

import { DeleteDeepTeamManagement } from "../Redux/Action/TeamManagement/TeamManagementDeleteAction";
import TeamManagementDeleteModal from "../screen/TeamManagement/TeamManagementDeleteModal";

export let paginationQuery: UserManagementQueryObjectGrid = {
  userId: [],
  roleId: [],
  tempCreationDate: undefined,
  opCoId: [],
  verticalResponsibleId: [],
  active: [],
  opCo: [],
  creationUser: [],
  creationDate: undefined,
  modificationUser: [],
  modificationDate: undefined,
  verticalResponsible: [],
  aspNetUserRoleId: [],
  userName: [],
  email: [],
  role: [],
  opCoResource: undefined,
  verticalResource: undefined,
  roleResource: undefined,
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

export let paginationQueryTeams: QueryDtoforTeam = {
  teamId: [],
  teamName: [],
  teamDescription: [],
  lastModified: undefined,
  lastModifiedBy: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
};

const UserManagementContainer: React.FC = (props) => {
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [detailId, setDetailId] = useState(null);
  const [filterRedirect, setFilterRedirect] = useState(false);
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
  const [isViewVisibleModal, setIsViewVisibleModal] = useState(false);
  const [isAddEnable, setIsAddEnable] = useState(false);
  const [isViewEnable, setIsViewEnable] = useState(false);
  const [viewUserInfo, setViewUserInfo] = useState<any>();
  const [orphanColor, setOrphanColor] = useState(false);
  const [isVisibleRolesModal, setIsVisibleRolesModal] = useState(false);
  const [selectedRole, setSelectedRole] = useState<any>(null);
  const [selectedRoleObj, setSelectedRoleObj] = useState<any>(null);
  const [rolesList, setRolesList] = useState<any[]>([]);
  const [isLoadingRoles, setIsLoadingRoles] = useState(false);
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [moduleResources, setModuleResources] = useState<any[]>([]);
  const [roleModuleResources, setRoleModuleResources] = useState<any[]>([]);
  const [roleError, setRoleError] = useState("");
  const [isEditMode, setIsEditMode] = useState(false);

  const [rolePermissionResource, setRolePermissionResource] = useState<any[]>(
    []
  );
  const [selectedModules, setSelectedModules] = useState<string>();
  //DTO
  const [data, setData] = useState<UserManagementDtoGrid[] | undefined>([]);
  let GridDto = useSelector(
    (state: RootState) =>
      state.userManagementGridReducer.UserManagementGridResult
  );

  //DTO
  let CreationGridDto = useSelector(
    (state: RootState) => state.userManagementRoleCreateReducer.ResultDtoCreate
  );

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [prevPage, setPrevPage] = useState<string>();
  const { readonly, isPermesso, tipologicaPermesso, pageSize, role } =
    useAuth();
  console.log(role, "roles");

  const [renderGridState, setRenderGridState] = useState<any>();
  const [roleModules, setRoleModules] = useState<any[]>([]);

  const [isVisibleAdditionalFilter, setIsVisibleAdditionalFilter] =
    useState(false);
  const [isAddingRole, setIsAddingRole] = useState(false);
  const [newRoleName, setNewRoleName] = useState("");
  const [newRoleDescription, setNewRoleDescription] = useState("");
  const [activeTab, setActiveTab] = useState("general");
  const { darkMode } = useTheme();

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetUserManagementGrid : undefined
  );

  const [isDeleteTeamModalOpen, setIsDeleteTeamModalOpen] = useState(false);
  const [deleteTeamRow, setDeleteTeamRow] = useState<TeamsGridDto | null>(null);
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

  const [teamsData, setTeamsData] = useState<TeamsGridDto[] | undefined>([]);
  const [teamsRenderGridState, setTeamsRenderGridState] = useState<any>();
  const [isTeamsFiltriAttivati, setIsTeamsFiltriAttivati] = useState(false);

  const {
    query: teamsQuery,
    setQuery: setTeamsQuery,
    next: teamsNext,
    back: teamsBack,
  } = useResourceTableCrud(
    paginationQueryTeams,
    isPermesso ? GetTeamsGrid : undefined
  );

  let TeamsGridDtoResult = useSelector(
    (state: RootState) => state.teamManagementGridReducer.TeamsGridResult
  );

  useEffect(() => {
    if (TeamsGridDtoResult !== undefined) {
      setTeamsData(TeamsGridDtoResult?.items);
      let copy = { ...TeamsGridDtoResult?.gridRender } as
        | CustomGridRender
        | undefined;
      setTeamsRenderGridState(copy);
    }
  }, [TeamsGridDtoResult]);

  const [isTeamFormVisible, setIsTeamFormVisible] = useState(false);
  const [isTeamFormAdd, setIsTeamFormAdd] = useState(false);
  const [editTeamId, setEditTeamId] = useState<number | null>(null);

  const refreshTeams = () => {
    GetTeamsGrid(teamsQuery);
  };

  const closeTeamsModal = () => {
    setIsTeamFormVisible(false);
    setEditTeamId(null);
  };

  const AddTeam = async () => {
    setIsTeamFormAdd(true);
    setEditTeamId(null);
    await GetTeamsCreateResource();
    setIsTeamFormVisible(true);
  };

  const EditTeam = async (details: any) => {
    setIsTeamFormAdd(false);
    setEditTeamId(details?.teamId ?? null);
    if (details?.teamId !== undefined && details?.teamId !== null) {
      await GetTeamsEditResource(details.teamId);
    }
    setIsTeamFormVisible(true);
  };

  const DeleteTeam = (row: TeamsGridDto) => {
    setDeleteTeamRow(row);
    setIsDeleteTeamModalOpen(true);
  };

  const closeDeleteTeamModal = () => {
    setIsDeleteTeamModalOpen(false);
    setDeleteTeamRow(null);
  };

  const confirmDeleteTeam = async (row: TeamsGridDto) => {
    if (row?.teamId === undefined) return;
    await DeleteDeepTeamManagement(row.teamId);
    refreshTeams();
  };

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
    paginationQueryTeams.pageSize = pageSize;
  }, [pageSize]);

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetUserManagementGrid");
    closeModal();
    GetUserManagementGrid(query).then(() =>
      setLoader("REMOVE", "GetUserManagementGrid")
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
  } = useOperationTableCrud<UserManagementDtoCreate, UserManagementDtoEdit>(
    CreateUserManagement,
    EditUserManagement,
    DeleteUserManagement,
    refresh,
    RestoreUserManagement
  );

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  const EditUser = (details) => {
    setLocalState({
      id: details?.userId,
      tab: "userManagement",
      prevPage: localStateHistory?.prevPage ?? "",
    });
    Edit(details.userId);
    setDetailId(details?.userId);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetUserManagementGrid");
    }
  }, [GridDto]);

  useEffect(() => {
    if (CreationGridDto !== null && CreationGridDto !== undefined) {
      if (CreationGridDto?.data?.userId !== null) {
        setViewUserInfo(CreationGridDto?.data);
        setIsViewVisibleModal(true);
      }
    } else setIsViewVisibleModal(false);
  }, [CreationGridDto !== null]);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    if (isPermesso) {
      setLoader("ADD", "GetUserManagementGrid");
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
            EditUser(localState?.id);
            return;
          }
          setLocalState(localState);
          Edit(localState?.id);
          setRedirect(true);
          setFilterRedirect(true);
          let copy = { ...query } as UserManagementQueryObjectGrid;
          copy.principalId = localState?.id;
          setQuery(copy);
          GetUserManagementGrid(copy).then((x) =>
            setLoader("REMOVE", "GetUserManagementGrid")
          );
        }
        if (localState.prevPage && localState.prevPage != "") {
          setPrevPage(localState.prevPage);
        }
      } else {
        // GetUserManagementGrid(paginationQuery).then((x) =>
        //   setLoader("REMOVE", "GetUserManagementGrid")
        // );
      }
      setLoader("REMOVE", "GetUserManagementGrid");
    }
  }, [isPermesso]);

  useEffect(() => {
    if (isVisibleRolesModal) {
      fetchRoles();
    }
  }, [isVisibleRolesModal]);

  const fetchRoles = async () => {
    setIsLoadingRoles(true);

    try {
      console.log("Calling GetUserManagementRoles...");
      const result = await GetUserManagementRoles();
      console.log("Got result:", result);

      let rolesData: any[] = [];

      if (result) {
        // SAVE MODULES
        if (Array.isArray(result.moduleResources)) {
          setModuleResources(result.moduleResources);
        }

        // SAVE ROLE-MODULE MAPPING
        if (Array.isArray(result.roleModuleResources)) {
          setRoleModuleResources(result.roleModuleResources);

          rolesData = result.roleModuleResources.map((role: any) => ({
            id: role.roleId,
            name: role.roleDescription,
            permissionLevel: role.permissionLevel,
            description: role.description,
          }));
        }
      }

      console.log("Final Mapped roles:", rolesData);
      setRolesList(rolesData);
    } catch (error) {
      console.error("Error fetching roles:", error);
      setRolesList([]);
    } finally {
      setIsLoadingRoles(false);
    }
  };

  const closeModalSetup = (changed: boolean) => {
    GetUserManagementGrid(query).then((x) => setIsVisibleModalSetup(false));
  };

  const openAddRolesModal = () => {
    setIsVisibleRolesModal(true);
    setSelectedRoleObj(null);
    setIsAddingRole(false);
    setIsEditMode(false);
    setRolePermissionResource([]);
  };

  const closeAddRolesModal = () => {
    setIsVisibleRolesModal(false);
    setSelectedRoleObj(null);
    setIsAddingRole(false);
    setIsEditMode(false);
    setRolePermissionResource([]);
    setNewRoleDescription("");
  };

  // const handleAddRole = async () => {
  //   if (selectedRole) {
  //     console.log("Selected Role:", selectedRole);
  //     setShow(true);
  //     setAlertStatus({
  //       message: "Role added successfully!",
  //       class: "success",
  //     });
  //     closeAddRolesModal();
  //   }
  // };
  const getModulesByRole = (roleId: number) => {
    const roleModules = roleModuleResources.filter(
      (r: any) => r.roleId === roleId
    );

    const moduleIdString = roleModules[0]?.moduleId || "";
    const matches = moduleIdString.matchAll(/\((\d+),(\d+)\)/g);

    const modulePermissions: Record<number, number> = {};
    for (const match of matches) {
      const moduleId = parseInt(match[1]);
      const permissionLevel = parseInt(match[2]);
      modulePermissions[moduleId] = permissionLevel;
    }

    return moduleResources
      .filter((m: any) => modulePermissions[m.key] !== undefined)
      .map((m: any) => ({
        ...m,
        permissionLevel: modulePermissions[m.key],
      }));
  };

  const InvocheDownload = async () => {
    if (activeTab === "teamManagement") {
      let result = await DownloadTeamManagementReport(teamsQuery);
      if (result !== undefined) {
        let url = window.URL.createObjectURL(result.file);
        let a = document.createElement("a");
        a.href = url;
        a.download = result.fileName;
        a.click();
      }
      return;
    }
    let result = await DownloadUserManagementReport(query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const DeleteUser = async (id: number) => {
    setMyConfirm({
      title: "Delete user record",
      message: "Are you sure do you want to delete it?",
      button: "Delete",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setMyConfirm(stateConfirm),
        confirm: async () => {
          setMyConfirm(stateConfirm);
          await DeleteUserManagement(id);
          refresh();
        },
      },
    });
  };

  const UserActivation = async (id: number, activeStatus: any) => {
    let payload: UserManagementDtoCreate = {
      userId: id,
      active: !activeStatus,
      roleId: 0,
      verticalResponsibleId: 0,
      creationUser: 0,
      modificationUser: 0,
      verticalResponsible: "",
      role: "",
      email: "",
    };
    setMyConfirm({
      title: "",
      message: `Do you want to ${
        !activeStatus ? "activate" : "deactivate"
      } the account?`,
      button: "Save",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setMyConfirm(stateConfirm),
        confirm: async () => {
          setMyConfirm(stateConfirm);
          await UserManagementActivation(payload);
          refresh();
        },
      },
    });
  };

  const onCloseModel = () => {
    setIsViewVisibleModal(false);
    setIsAddEnable(false);
    setIsViewEnable(false);
    setViewUserInfo(null);
    closeModal();
    refresh();
  };

  const closeButton = () => {
    let rtn = {
      ResultDtoCreate: null,
      UserManagementRoleDtoCreate: null,
    } as ResultDto;
    rootStore.dispatch({ type: ADD_NEW_USER_MANAGEMENT, payload: rtn });
    onCloseModel();
  };
  const ViewDetails = (item) => {
    setIsViewVisibleModal(true);
    setIsAddEnable(false);
    setIsViewEnable(true);
    setViewUserInfo(item);
  };
  const groupedMenus = roleModules.reduce((acc: any, module: any) => {
    const category = module.menu || "Other";

    if (!acc[category]) {
      acc[category] = [];
    }

    acc[category].push({
      title: module.value,
      path: module.modulePath,
      category: module.category,
    });

    return acc;
  }, {});
  const handleMenuChange = (menus: any) => {
    console.log("menus", menus);
    setSelectedModules(menus);
  };
  const handleAddRole = async () => {
    if (isAddingRole) {
      if (!newRoleName.trim()) {
        setRoleError("Role name is required");
        return;
      }
    }
    const payload = {
      roleId: isAddingRole ? 0 : selectedRoleObj?.id,
      moduleId: selectedModules,
      roleName: isAddingRole ? newRoleName : selectedRoleObj?.name,
      description: isAddingRole
        ? newRoleDescription
        : selectedRoleObj?.description,
    };

    try {
      await AddNewRoleScreen(payload);

      refresh();
    } catch (error) {
      console.error("Save failed", error);
    }
    closeAddRolesModal();
  };

  return (
    <div className="pageContainer">
      <TeamManagementDeleteModal
        open={isDeleteTeamModalOpen}
        row={deleteTeamRow}
        onClose={closeDeleteTeamModal}
        onConfirmed={confirmDeleteTeam}
      />
      <ModalConfirm data={confirm} />
      <ModalConfirm data={myConfirm} />
      <Dialog
        open={isViewVisibleModal}
        onClose={() => closeButton()}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mt-4 voda-bold">
              {isViewEnable
                ? "View User Details"
                : isAddEnable
                ? "Add New User"
                : "Edit User"}
            </h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => closeButton()}
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
          <UserManagementForm
            isAdd={isAddEnable}
            isView={isViewEnable}
            userInfo={{
              ...viewUserInfo,
              userId: isViewEnable ? viewUserInfo?.userId : null,
            }}
            action={{ closeModal: () => onCloseModel(), refresh }}
          />
        </DialogContent>
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
      {/* Add Roles Modal */}
      <Dialog
        open={isVisibleRolesModal}
        onClose={() => closeAddRolesModal()}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="xl"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mt-4">Manage Role - Screen Mapping</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => closeAddRolesModal()}
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
          <div className="row">
            <div className="col-6">
              <FormControl fullWidth sx={{ mt: 2 }}>
                {isAddingRole || isEditMode ? (
                  <div className="col-12">
                    <button
                      type="button"
                      className="btn btn-link p-0 mt-1"
                      onClick={() => {
                        setIsAddingRole(false);
                        setIsEditMode(false);
                        setNewRoleName("");
                        setNewRoleDescription("");
                        setRoleError("");
                      }}
                    >
                      ← Back to Select Role
                    </button>
                    <label className="voda-bold w-100 mt-2" id="addrole_id">
                      {isAddingRole ? "Add Role" : "Update Role"}
                      <span className="red">*</span>
                      <input
                        type="text"
                        value={
                          isAddingRole
                            ? newRoleName
                            : selectedRoleObj?.name || ""
                        }
                        onChange={(e) => {
                          if (isAddingRole) {
                            setNewRoleName(e.target.value);
                          } else {
                            setSelectedRoleObj((prev: any) => ({
                              ...prev,
                              name: e.target.value,
                            }));
                          }
                          setRoleError("");
                        }}
                        className="inputForm w-100"
                      />
                      {roleError && (
                        <span className="validation text-danger">
                          {roleError}
                        </span>
                      )}
                    </label>
                    <label
                      className="voda-bold w-100 mt-3"
                      id="role_description_id"
                    >
                      Role Description
                      <textarea
                        // type="text"
                        value={
                          isAddingRole
                            ? newRoleDescription
                            : selectedRoleObj?.description || ""
                        }
                        onChange={(e) => {
                          if (isAddingRole) {
                            setNewRoleDescription(e.target.value);
                          } else {
                            setSelectedRoleObj((prev: any) => ({
                              ...prev,
                              description: e.target.value,
                            }));
                          }
                        }}
                        className="inputForm w-100"
                        placeholder="Enter role description (optional)"
                      />
                    </label>
                  </div>
                ) : (
                  <>
                    <InputLabel>Select Role</InputLabel>
                    <Select
                      value={selectedRoleObj?.id || ""}
                      label="Select Role"
                      onChange={(e) => {
                        const valueId = e.target.value;
                        const roleId = Number(valueId);
                        // setSelectedRole(roleId);
                        const roleObj = rolesList.find((r) => r.id === valueId);
                        setSelectedRoleObj(roleObj);
                        setIsEditMode(false);
                        const modules = getModulesByRole(roleId);

                        console.log("Modules for role:", roleObj);

                        const permissions = modules.map((m: any) => ({
                          id: m.key,
                          text: m.value,
                          path: m.modulePath,
                          menu: m.menu,
                          order: 0,
                          default: true,
                          permissionLevel: m.permissionLevel,
                        }));

                        setRolePermissionResource(permissions);
                      }}
                      disabled={isLoadingRoles}
                      MenuProps={{
                        slotProps: {
                          paper: {
                            sx: {
                              maxHeight: 200,
                              maxWidth: 300,
                            },
                          },
                        },
                      }}
                    >
                      {isLoadingRoles ? (
                        <MenuItem disabled>Loading roles...</MenuItem>
                      ) : (
                        rolesList.map((role) => (
                          <MenuItem key={role.id} value={role.id}>
                            {role.name}
                          </MenuItem>
                        ))
                      )}
                    </Select>
                    {selectedRoleObj && (
                      <label
                        className="voda-bold w-100 mt-3"
                        id="role_description_view"
                      >
                        Role Description
                        <textarea
                          // type="text"
                          value={selectedRoleObj?.description || ""}
                          className="inputForm w-100"
                          placeholder="No description available"
                          disabled
                          style={{
                            backgroundColor: "#f5f5f5",
                            cursor: "not-allowed",
                          }}
                        />
                      </label>
                    )}
                  </>
                )}
              </FormControl>
            </div>
            {!selectedRoleObj && !isAddingRole && (
              <div className="col-3">
                <button className="btn btn-link" type="button">
                  <img
                    style={{ height: 15, marginTop: "28px" }}
                    onClick={() => {
                      setIsAddingRole(true);
                      setSelectedRoleObj(null);
                      setRolePermissionResource([]);
                    }}
                    src={require("../img/plus_icon.png")}
                    alt="plus"
                  />
                </button>
              </div>
            )}
            {selectedRoleObj && !isEditMode && !isAddingRole && (
              <button
                className="btn btn-link"
                type="button"
                onClick={() => {
                  setIsEditMode(true);
                  setIsAddingRole(false);
                }}
              >
                <img
                  className="btnEdit op-55"
                  src={require("../img/edit.png")}
                />
              </button>
            )}
          </div>
          {(selectedRoleObj?.id || isAddingRole) && (
            <RoleMenuManagement
              permissionResource={rolePermissionResource}
              moduleResources={moduleResources}
              onMenuChange={handleMenuChange}
            />
          )}

          <div className="d-flex justify-content-end gap-2 mt-4">
            <Button
              className="voda-bold btn btn-link px-4 btnHeader cancel"
              onClick={() => closeAddRolesModal()}
              size="sm"
            >
              Cancel
            </Button>

            <button className="btn btn-danger" onClick={() => handleAddRole()}>
              Submit
            </button>
          </div>
        </DialogContent>
      </Dialog>

      <Dialog
        open={isTeamFormVisible}
        onClose={() => closeTeamsModal()}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mt-4 voda-bold">
              {isTeamFormAdd ? "Add New Team" : "Edit Team"}
            </h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => closeTeamsModal()}
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
          <TeamsForm
            isAdd={isTeamFormAdd}
            teamId={editTeamId}
            action={{ closeModal: closeTeamsModal, refresh: refreshTeams }}
          />
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          {redirect === true ? (
            <Link
              className="d-flex justify-content-center align-items-center mr-3 mb-2"
              to={{ pathname: prevPage }}
            >
              <GoArrowLeft
                onClick={() => navigate(-1)}
                size={25}
                color={`${darkMode ? "white" : "black"}`}
              />
            </Link>
          ) : null}
          <h3 className="voda-bold">User Access Management</h3>
          {redirect === true && filterRedirect === true ? (
            <button className="btn btn-link ml-4" onClick={resetQuery}>
              Reset all filters
            </button>
          ) : null}
        </div>
        <div className="d-flex">
          {tipologicaPermesso && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader flex flex-gab grid-main-btn"
              onClick={() => {
                if (activeTab === "teamManagement") {
                  AddTeam();
                } else {
                  setIsViewVisibleModal(true);
                  setIsViewEnable(false);
                  setIsAddEnable(true);
                }
              }}
              type="button"
            >
              <img src={require("../img/Plus_white.png")} className="img-15" />
              <span className="fz-14">
                {activeTab === "teamManagement" ? "Add Team" : "Add User"}
              </span>
            </button>
          )}

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

            <Dropdown.Menu className="grid-main-btn">
              {activeTab !== "teamManagement" && (
                <Dropdown.Item onClick={() => setIsVisibleModalSetup(true)}>
                  Manage Table Content
                </Dropdown.Item>
              )}
              {!readonly && (
                <Dropdown.Item onClick={openAddRolesModal}>
                  Manage Roles
                </Dropdown.Item>
              )}
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      <div className="mt-1 mb-4">
        {role?.includes("Manager") ? (
          <Tabs
            defaultActiveKey="general"
            id="userManagement"
            activeKey={activeTab}
            onSelect={(x) => setActiveTab(x || "")}
          >
            <Tab eventKey="general" title="User Management">
              {activeTab === "general" && (
                <div className="mt-3">
                  {show && (
                    <div className="mt-2">
                      <Alert
                        variant={alerStatus?.class}
                        onClose={() => setShow(false)}
                        dismissible
                      >
                        <p>{alerStatus?.message}</p>
                      </Alert>
                    </div>
                  )}
                  <UserManagementGrid
                    data={data}
                    pagination={query}
                    orphanColor={orphanColor}
                    renderGrid={renderGridState?.render ?? []}
                    action={{
                      DeleteUser,
                      EditUser,
                      ViewDetails,
                      UserActivation,
                      Filter: setQuery,
                      Restore,
                      closeModal,
                      setIsFiltriAttivati,
                    }}
                  ></UserManagementGrid>
                  <Paginate
                    pagination={{ page: query.page, pageSize: query.pageSize }}
                    totalItems={GridDto?.totalItems}
                    actions={{ next, back }}
                  />
                </div>
              )}
            </Tab>
            <Tab eventKey="teamManagement" title="Team Management">
              {activeTab === "teamManagement" && (
                <div className="mt-3">
                  <TeamsGrid
                    data={teamsData}
                    pagination={teamsQuery}
                    renderGrid={teamsRenderGridState?.render ?? []}
                    action={{
                      DeleteTeam,
                      EditTeam,
                      Filter: setTeamsQuery,
                      closeModal: closeTeamsModal,
                      setIsFiltriAttivati: setIsTeamsFiltriAttivati,
                    }}
                  ></TeamsGrid>
                  <Paginate
                    pagination={{
                      page: teamsQuery.page,
                      pageSize: teamsQuery.pageSize,
                    }}
                    totalItems={TeamsGridDtoResult?.totalItems}
                    actions={{ next: teamsNext, back: teamsBack }}
                  />
                </div>
              )}
            </Tab>
          </Tabs>
        ) : (
          <>
            {show && (
              <div className="mt-2">
                <Alert
                  variant={alerStatus?.class}
                  onClose={() => setShow(false)}
                  dismissible
                >
                  <p>{alerStatus?.message}</p>
                </Alert>
              </div>
            )}
            <UserManagementGrid
              data={data}
              pagination={query}
              orphanColor={orphanColor}
              renderGrid={renderGridState?.render ?? []}
              action={{
                DeleteUser,
                EditUser,
                ViewDetails,
                UserActivation,
                Filter: setQuery,
                Restore,
                closeModal,
                setIsFiltriAttivati,
              }}
            ></UserManagementGrid>
            <Paginate
              pagination={{ page: query.page, pageSize: query.pageSize }}
              totalItems={GridDto?.totalItems}
              actions={{ next, back }}
            />
          </>
        )}
      </div>
    </div>
  );
};

export default UserManagementContainer;
