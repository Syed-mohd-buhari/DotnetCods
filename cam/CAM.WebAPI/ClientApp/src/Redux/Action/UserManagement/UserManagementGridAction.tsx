import { UserManagementApi } from "../../../Business/UserManagementBusiness";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  GET_GRID_USER_MANAGEMENT,
  UserManagementQueryObjectGrid,
  QueryResultDtoOfUserManagementDtoGrid,
  GET_CREATE_USER_MANAGEMENT,
  GET_EDIT_USER_MANAGEMENT,
  DELETE_USER_MANAGEMENT,
  GET_FILTER_USER_MANAGEMENT,
  UserManagementCreate,
  UserManagementEdit,
  UserManagementGrid,
  RESTORE_USER_MANAGEMENT,
  ACTIVATION_USER_MANAGEMENT,
  GET_GRID_USER_MANAGEMENT_ROLE,
  QueryResultDtoOfUserManagementRoleDtoGrid,
  UserManagementRoleGrid,
  RESTORE_USER_MANAGEMENT_ROLE,
  DELETE_USER_MANAGEMENT_ROLE,
  UserManagementEditRole,
  UserManagementCreateRole,
  GET_CREATE_USER_MANAGEMENT_ROLE,
  GET_EDIT_USER_MANAGEMENT_ROLE,
  UserManagementRoleDtoEdit,
  QueryResultDtoOfGetUserResourceListDtoGrid,
  GetUserResourceListGrid,
  GET_GRID_USER_RESOURCE,
  UserManagementDtoCreate,
  GET_FILTER_USER_MANAGEMENT_ROLE,
  ADD_NEW_USER_MANAGEMENT,
} from "../../../Model/UserManagement";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { FileResult, ReturnFile } from "../../../Model/Common";

export async function GetUserManagementGrid(
  queryFilter?: UserManagementQueryObjectGrid
) {
  setLoader("ADD", "GetUserManagementGrid");
  let api = new UserManagementApi();

  let result: QueryResultDtoOfUserManagementDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfUserManagementDtoGrid>
    >(() => api.userManagementGetResult(queryFilter ?? {}));

    let rtn = {
      UserManagementGridResult: result,
      filter: null,
    } as UserManagementGrid;

    rootStore.dispatch({ type: GET_GRID_USER_MANAGEMENT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_USER_MANAGEMENT,
      payload: {
        UserManagementGridResult: null,
        filter: null,
      } as UserManagementGrid,
    });
  }
  setLoader("REMOVE", "GetUserManagementGrid");
}

export async function GetUserManagementRoleGrid(
  queryFilter?: UserManagementQueryObjectGrid
) {
  setLoader("ADD", "GetUserManagementRoleGrid");
  let api = new UserManagementApi();

  let result: QueryResultDtoOfUserManagementRoleDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfUserManagementRoleDtoGrid>
    >(() => api.userManagementRoleGetResult(queryFilter ?? {}));
    let rtn = {
      UserManagementRoleGridResult: result,
      filter: null,
    } as UserManagementRoleGrid;

    rootStore.dispatch({ type: GET_GRID_USER_MANAGEMENT_ROLE, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_USER_MANAGEMENT_ROLE,
      payload: {
        UserManagementRoleGridResult: null,
        filter: null,
      } as UserManagementRoleGrid,
    });
  }
  setLoader("REMOVE", "GetUserManagementRoleGrid");
}

export async function CreateUserManagement(
  data: UserManagementQueryObjectGrid
) {
  setLoader("ADD", "CreateUserManagement");
  let api = new UserManagementApi();
  let create = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementCreate(data)
  );
  let rtn = {
    ResultDtoCreate: null,
    UserManagementDtoCreate: create,
  } as UserManagementCreate;
  rootStore.dispatch({ type: GET_CREATE_USER_MANAGEMENT, payload: rtn });
  setLoader("REMOVE", "CreateUserManagement");
}

export async function EditUserManagement(data: UserManagementQueryObjectGrid) {
  setLoader("ADD", "EditUserManagement");
  let api = new UserManagementApi();
  let create = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementEdit(data)
  );
  let rtn = {
    ResultDtoEdit: null,
    UserManagementDtoEdit: create,
  } as UserManagementEdit;
  rootStore.dispatch({ type: GET_EDIT_USER_MANAGEMENT, payload: rtn });
  setLoader("REMOVE", "EditUserManagement");
}

export async function GetFilterColumUserManagement(
  columName: string,
  columValue: string,
  queryFilter?: UserManagementQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new UserManagementApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.userManagementGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    UserManagementGridResult: null,
  } as UserManagementGrid;
  rootStore.dispatch({ type: GET_FILTER_USER_MANAGEMENT, payload: rtn });
}

export async function GetFilterColumUserManagementRole(
  columName: string,
  columValue: string,
  queryFilter?: UserManagementQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new UserManagementApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.userManagementRoleGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    UserManagementGridResult: null,
  } as UserManagementGrid;
  rootStore.dispatch({ type: GET_FILTER_USER_MANAGEMENT_ROLE, payload: rtn });
}

export async function GetListOfEmailIds(
  columName: string,
  columValue: string,
  queryFilter?: UserManagementQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new UserManagementApi();
  return (result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(
    () =>
      api.userManagementGetFilterResult(
        queryFilter ?? {},
        columName,
        columValue
      )
  ));
}

export async function DownloadUserManagementReport(
  queryFilter?: UserManagementQueryObjectGrid
) {
  setLoader("ADD", "DownloadUserManagementReport");

  let api = new UserManagementApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.userManagementExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "DownloadUserManagementReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadUserManagementReport");
}

export async function DeleteUserManagement(id: number) {
  setLoader("ADD", "DeleteUserManagement");
  let api = new UserManagementApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: DELETE_USER_MANAGEMENT, payload: rtn });

  setLoader("REMOVE", "DeleteUserManagement");
  return rtn;
}

export async function UserManagementActivation(data: UserManagementDtoCreate) {
  setLoader("ADD", "UserManagementActivation");
  let api = new UserManagementApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementActivation(data)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: ACTIVATION_USER_MANAGEMENT, payload: rtn });

  setLoader("REMOVE", "UserManagementActivation");
  return rtn;
}

export async function RestoreUserManagement(id: number) {
  setLoader("ADD", "RestoreUserManagement");
  let api = new UserManagementApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementRestore(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: RESTORE_USER_MANAGEMENT, payload: rtn });

  setLoader("REMOVE", "RestoreUserManagement");
  return rtn;
}

export async function CreateUserManagementRole(
  data: UserManagementRoleDtoEdit
) {
  setLoader("ADD", "CreateUserManagementRole");
  let api = new UserManagementApi();
  let create = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementCreateRole(data)
  );
  let rtn = {
    ResultDtoCreate: null,
    UserManagementRoleDtoCreate: create,
  } as UserManagementCreateRole;
  rootStore.dispatch(
    setNotification({
      message: create?.info ?? "",
      notifyType: create?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: GET_CREATE_USER_MANAGEMENT_ROLE, payload: rtn });
  setLoader("REMOVE", "CreateUserManagementRole");
  return rtn;
}

export async function AddNewUser(data: UserManagementDtoCreate) {
  setLoader("ADD", "AddNewUser");
  let api = new UserManagementApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementAddNewUser(data)
  );
  let rtn = {
    ResultDtoCreate: { ...result, data: { ...data, userId: result?.data } },
    UserManagementRoleDtoCreate: null,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  console.log(rtn);
  rootStore.dispatch({ type: ADD_NEW_USER_MANAGEMENT, payload: rtn });

  setLoader("REMOVE", "AddNewUser");
  return rtn;
}

export async function AddNewRoleScreen(data: any) {
  try {
    let api = new UserManagementApi();
    const response = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.userManagementCreateRoleModule(data)
    );
    rootStore.dispatch(
      setNotification({
        message: response?.info ?? "",
        notifyType: response?.warning ? NotifyType.error : NotifyType.success,
      })
    );

    return response;
  } catch (error) {
    console.error("AddNewRoleScreen API Error:", error);
    throw error;
  }
}

export async function EditUserManagementRole(data: UserManagementRoleDtoEdit) {
  setLoader("ADD", "EditUserManagementRole");
  let api = new UserManagementApi();
  let create = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementEditRole(data)
  );
  let rtn = {
    ResultDtoEdit: null,
    UserManagementRoleDtoEdit: create,
  } as UserManagementEditRole;
  rootStore.dispatch({ type: GET_EDIT_USER_MANAGEMENT_ROLE, payload: rtn });
  setLoader("REMOVE", "EditUserManagementRole");
}

export async function DeleteUserManagementRole(id: number) {
  setLoader("ADD", "DeleteUserManagementRole");
  let api = new UserManagementApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementDeleteRole(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: DELETE_USER_MANAGEMENT_ROLE, payload: rtn });

  setLoader("REMOVE", "DeleteUserManagementRole");
  return rtn;
}

export async function RestoreUserManagementRole(id: number) {
  setLoader("ADD", "RestoreUserManagementRole");
  let api = new UserManagementApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.userManagementRestoreRole(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: RESTORE_USER_MANAGEMENT_ROLE, payload: rtn });

  setLoader("REMOVE", "RestoreUserManagementRole");
  return rtn;
}

export async function GetUserResourceList() {
  setLoader("ADD", "GetUserResourceList");
  let api = new UserManagementApi();

  let result: QueryResultDtoOfGetUserResourceListDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfGetUserResourceListDtoGrid>
    >(() => api.userManagementResourceListGetResult());
    let rtn = {
      GetUserResourceListGridResult: result,
      filter: null,
    } as GetUserResourceListGrid;

    rootStore.dispatch({ type: GET_GRID_USER_RESOURCE, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_USER_RESOURCE,
      payload: {
        GetUserResourceListGridResult: null,
        filter: null,
      } as GetUserResourceListGrid,
    });
  }
  setLoader("REMOVE", "GetUserResourceList");
  return result;
}

export async function GetUserManagementRoles() {
  let api = new UserManagementApi();

  try {
    const result = await ApiCallWithErrorHandling<Promise<any>>(() =>
      api.userManagementGetRolesForUpdatePage()
    );

    // console.log("API Response:", result);
    return result;
  } catch (error) {
    console.error("Error fetching roles:", error);
    return null;
  }
}
