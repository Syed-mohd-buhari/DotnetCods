import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import { useLocation } from "react-router-dom";

export function useAuth() {
  const [role, setRole] = useState<string[]>([]);

  let roleList = useSelector(
    (state: RootState) => state.autenticazione.role as string[]
  );
  let userPrefrenceList = useSelector(
    (state: RootState) =>
      state.autenticazione.userPagePrefrenceDetails as string[]
  );
  let checkPermesso = useSelector(
    (state: RootState) => state.autenticazione.permesso as boolean
  );
  let operatingModeReducer = useSelector(
    (state: RootState) => state.autenticazione.mode as string
  );
  let updatedPageSize = useSelector(
    (state: RootState) => state.autenticazione.pagesize as number
  );
  const location = useLocation();

  const [tipologicaPermesso, setTipologicaPermesso] = useState(false);
  const [admin, setAdmin] = useState(false);
  const [simpleUser, setSimpleUser] = useState(false);
  const [KPIAdmin, setKPIAdmin] = useState(false);
  const [KPIEditor, setKPIEditor] = useState(false);
  const [RefactorUser, setRefactorUser] = useState(false);
  const [readonly, setReadOnly] = useState<boolean>(false);
  const [operatingMode, setOperatingMode] = useState<string>("normal");
  const [tipologicaPermessoSpecial, setTipologicaPermessoSpecial] =
    useState(false);
  const [isPermesso, setIsPermesso] = useState<boolean>(false);
  const [pageSize, setPageSize] = useState(updatedPageSize);
  const [userPrefrenceDetails, setUserPrefrenceDetails] = useState<any>(null);

  useEffect(() => {
    setRole(roleList);
    if (roleList !== null && roleList?.includes("User")) {
      setSimpleUser(true);
    }
    // if (
    //   roleList !== null &&
    //   (roleList?.includes("Admin") || roleList?.includes("LookUp"))
    // ) {
    //   setTipologicaPermesso(true);
    // }
    if (roleList !== null && roleList?.includes("Admin")) {
      setAdmin(true);
    }
    if (roleList !== null && roleList?.includes("KPI Administrator")) {
      setKPIAdmin(true);
    }
    if (roleList !== null && roleList?.includes("KPI Editor")) {
      setKPIEditor(true);
    }
    if (roleList !== null && roleList?.includes("Refactor User")) {
      setRefactorUser(true);
    }
    // if (roleList !== null && roleList.includes("READONLY")) {
    //   setReadOnly(true);
    // }
  }, [roleList]);

  useEffect(() => {
    if (operatingModeReducer) {
      setOperatingMode(operatingModeReducer);
    }
  }, [operatingModeReducer]);

  useEffect(() => {
    if (checkPermesso !== null) {
      setIsPermesso(checkPermesso);
    }
  }, [checkPermesso]);

  useEffect(() => {
    if (updatedPageSize) {
      setPageSize(updatedPageSize);
    }
  }, [updatedPageSize]);

  useEffect(() => {
    if (userPrefrenceList) {
      setUserPrefrenceDetails(userPrefrenceList);
    }
  }, [userPrefrenceList]);

  useEffect(() => {
    const preferenceData = userPrefrenceList as any;
    const currentPath = location.pathname;

    const PATH_ALIASES: Record<string, string> = {
      "/overview": "/majorsoftware",
    };

    const effectivePath = PATH_ALIASES[currentPath] ?? currentPath;

    const screen = preferenceData?.pagePrefrenceDetail?.find(
      (item: any) => item.path === effectivePath
    );

    const permission = screen?.screenPermission;

    setReadOnly(permission === 3);
    setTipologicaPermesso(permission === 7);
  }, [location.pathname, userPrefrenceList]);

  const VerifyIsInRole = (roleName: string): boolean => {
    return roleList?.includes(roleName);
  };

  return {
    KPIAdmin,
    KPIEditor,
    admin,
    readonly,
    role,
    simpleUser,
    tipologicaPermesso,
    RefactorUser,
    VerifyIsInRole,
    isPermesso,
    operatingMode,
    pageSize,
    userPrefrenceDetails,
  };
}
