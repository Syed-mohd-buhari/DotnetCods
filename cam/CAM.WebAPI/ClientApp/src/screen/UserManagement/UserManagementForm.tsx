import React, { useEffect, useRef, useState } from "react";
import { Button, InputGroup } from "react-bootstrap";
import { FaEdit } from "react-icons/fa";
import { useSelector } from "react-redux";
import { useLocation } from "react-router-dom";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import { useAuth } from "../../Hook/useAuth";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import setLoader from "../../Redux/Action/LoaderAction";
import { ResultDto } from "../../Model/CommonModels";
import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  ADD_NEW_USER_MANAGEMENT,
  UserManagementQueryObjectGrid,
  UserManagementRoleDtoCreate,
  UserManagementRoleDtoEdit,
  UserManagementRoleDtoGrid,
} from "../../Model/UserManagement";
import {
  AddNewUser,
  CreateUserManagementRole,
  DeleteUserManagementRole,
  EditUserManagementRole,
  GetListOfEmailIds,
  GetUserManagementRoleGrid,
  GetUserManagementRoles,
  GetUserResourceList,
  RestoreUserManagementRole,
} from "../../Redux/Action/UserManagement/UserManagementGridAction";
import EmbeddedOrganizationInfo from "./EmbeddedOrganizationInfo";
import MenuManagementModal from "./MenuManagementModal";

let paginationQuery: UserManagementQueryObjectGrid = {
  userId: [],
  roleId: [],
  opCoId: [],
  verticalResponsibleId: [],
  active: [],
  opCo: [],
  creationUser: [],
  modificationUser: [],
  verticalResponsible: [],
  aspNetUserRoleId: [],
  userName: [],
  email: [],
  role: [],
  lastModifiedBy: [],
  tempCreationDate: undefined,
  creationDate: undefined,
  modificationDate: undefined,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  opCoResource: undefined,
  verticalResource: undefined,
  roleResource: undefined,
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
};

interface Props {
  action: { closeModal(): any; refresh(): any };
  isAdd: boolean;
  isView: boolean;
  userInfo: any;
}

const UserManagementForm: React.FC<Props> = (props) => {
  const [formFields, setFormFields] = useState({
    email: props.userInfo?.email || "",
    active: false,
  });
  const [errors, setErrors] = useState({ email: "" });
  const [emailList, setEmailList] = useState<FilterValueDto[]>();
  const [userId, setUserId] = useState<any>(
    props.userInfo?.userId || undefined
  );
  const [editMail, setEditMail] = useState(!!props.userInfo?.userId);
  const [isViewVisibleModal, setIsViewVisibleModal] = useState(props.isView);
  const [isVerifying, setIsVerifying] = useState(false);
  const [opcoValue, setOpcoValue] = useState<any>([]);
  const [roleValue, setRoleValue] = useState<any>([]);
  const [verticalValue, setVerticalValue] = useState<any>([]);
  const [subDomainValue, setSubDomainValue] = useState<any>([]);
  const [opcoResValue, setOpcoResValue] = useState<any>([]);
  const [roleResValue, setRoleResValue] = useState<any>([]);
  const [verticalResValue, setVerticalResValue] = useState<any>([]);
  const [verticalId, setVerticalId] = useState<any>([]);
  const [isDesignContact, setIsDesignContact] = useState(false);
  const [isEduSpoc, setIsEduSpoc] = useState(false);
  const [isSubDomainSpoc, setIsSubDomainSpoc] = useState(false);
  const [organisationData, setOrganisationData] = useState<any>();
  const [allROVList, setAllROVList] = useState<any>();
  const [data, setData] = useState<UserManagementRoleDtoGrid[]>([]);
  const [showMenuModal, setShowMenuModal] = useState(false);
  const [moduleResources, setModuleResources] = useState<any[]>([]);
  const [roleModuleResources, setRoleModuleResources] = useState<any[]>([]);
  const [userPrefrenceDetails, setUserPrefrenceDetails] = useState<any[]>([]);
  const [restrictedOpcoValue, setRestrictedOpcoValue] = useState<any>([]);
  const [verticalResponsibleValue, setVerticalResponsibleValue] = useState<any>(
    []
  );
  const [
    verticalResponcibleResourceValue,
    setVerticalResponcibleResourceValue,
  ] = useState<any>([]);
  const [restrictedOpcoResValue, setRestrictedOpcoResValue] = useState<any>([]);

  const [hasGridErrors, setHasGridErrors] = useState(false);
  const [roleDescriptions, setRoleDescriptions] = useState<
    Record<string, string>
  >({});

  const orgRef = useRef<any>(null);
  const { isPermesso, pageSize } = useAuth();
  const location: any = useLocation();
  const GridDto = useSelector(
    (s: RootState) =>
      s.userManagementRoleGridReducer.UserManagementRoleGridResult
  );
  const CreationGridDto = useSelector(
    (s: RootState) => s.userManagementRoleCreateReducer.ResultDtoCreate
  );

  useEffect(() => {
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const { query, setQuery } = useResourceTableCrud(
    {
      ...paginationQuery,
      userId: [props?.userInfo?.userId ?? userId],
      email: [props?.userInfo?.email ?? formFields?.email],
    },
    isPermesso && (props?.isView || userId)
      ? GetUserManagementRoleGrid
      : undefined
  );

  const refresh = () => {
    if (userId !== undefined || props?.userInfo?.userId !== undefined) {
      setLoader("ADD", "GetUserManagementRoleGrid");
      GetUserManagementRoleGrid(query).then(() =>
        setLoader("REMOVE", "GetUserManagementRoleGrid")
      );
    }
  };

  const { Edit, setLocalState } = useOperationTableCrud<
    UserManagementRoleDtoCreate,
    UserManagementRoleDtoEdit
  >(
    CreateUserManagementRole,
    EditUserManagementRole,
    DeleteUserManagementRole,
    refresh,
    RestoreUserManagementRole
  );

  useEffect(() => {
    if (props.isAdd)
      GetListOfEmailIds("email", "", paginationQuery).then(setEmailList);
  }, [props.isAdd]);

  useEffect(() => {
    if (props.isAdd && userId)
      GetUserResourceList().then((x) => {
        if (x !== undefined) setAllROVList(x);
      });
  }, [userId]);

  useEffect(() => {
    if (
      CreationGridDto !== null &&
      CreationGridDto !== undefined &&
      props.isAdd
    ) {
      if (CreationGridDto?.data !== null) {
        setFormFields(CreationGridDto?.data);
        const newQuery = {
          ...query,
          email: [CreationGridDto?.data?.email],
          active: [CreationGridDto?.data?.active],
          userId: [CreationGridDto?.data?.userId ?? userId],
        };
        setQuery(newQuery);
        setUserId(CreationGridDto?.data?.userId);
        setIsViewVisibleModal(true);
        setEditMail(true);
      }
    } else setIsViewVisibleModal(false);
  }, [CreationGridDto]);

  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null && props.isView === true) {
      const details = GridDto?.data?.aspNetUserRovDetails as any;
      setData(details ? [details] : []);
      if (details) {
        const csv = (s: string) =>
          s
            ?.split(",")
            .map((v: string) => v.trim())
            .filter(Boolean) || [];
        const csvNum = (s: string) =>
          s
            ?.split(",")
            .map((v: string) => Number(v.trim()))
            .filter(Boolean) || [];
        const restrictedOpcos = csv(details?.restrictedOpCoIds);
        setOpcoResValue(csv(details.opCo));
        setRoleResValue(csvNum(details.roleId));
        setVerticalResValue(csv(details.vertical));
        setVerticalId(csv(details.verticalId));
        setSubDomainValue(csvNum(details.subdomainResponsibleId));
        setVerticalResponcibleResourceValue(csv(details.verticalResponsible));
        setIsDesignContact(details?.isdesigncontact);
        setIsEduSpoc(details?.iseduspoc);
        setIsSubDomainSpoc(details?.issubdomainspoc);
        setRestrictedOpcoResValue(restrictedOpcos);
      }
      setLoader("REMOVE", "GetUserManagementRoleGrid");
    }
  }, [GridDto]);

  const filterOrganisationDataByVertical = (verticalIds: string[]) => {
    if (
      !allROVList?.organisatioDtoGrids ||
      !verticalIds ||
      verticalIds.length === 0
    ) {
      setOrganisationData([]);
      return;
    }
    setOrganisationData(
      allROVList.organisatioDtoGrids.filter(
        (org: any) =>
          org.verticalId && verticalIds.includes(org.verticalId.toString())
      )
    );
  };

  useEffect(() => {
    if (roleResValue.length) setRoleValue(roleResValue);
    if (opcoResValue) {
      const matchingIds =
        dictionaryToArray(allROVList?.opCoResource)
          .filter((res: any) => opcoResValue.includes(res?.value?.trim()))
          .map((res: any) => res?.key) ?? [];
      if (matchingIds.length) setOpcoValue(matchingIds);
    }
    if (restrictedOpcoResValue) {
      const matchingRestrictedIds =
        (allROVList?.opCoResource &&
          dictionaryToArray(allROVList?.opCoResource)
            .filter((res: any) =>
              restrictedOpcoResValue.includes(res?.value?.trim())
            )
            .map((res: any) => res?.key)) ??
        [];
      setRestrictedOpcoValue(matchingRestrictedIds);
    }
    if (verticalResponcibleResourceValue) {
      const matchingIds =
        dictionaryToArray(allROVList?.verticalResponcibleResource)
          .filter((res: any) =>
            verticalResponcibleResourceValue.includes(res?.value)
          )
          .map((res: any) => res?.key) ?? [];
      setVerticalResponsibleValue(matchingIds);
    }
    if (verticalResValue) {
      const matchingIds =
        dictionaryToArray(allROVList?.verticalResource)
          .filter((res: any) => verticalResValue.includes(res?.value))
          .map((res: any) => res?.key) ?? [];
      setVerticalValue(matchingIds);
      filterOrganisationDataByVertical(
        verticalId?.map((id: number) => id.toString()) || []
      );
    }
  }, [allROVList, opcoResValue]);

  useEffect(() => {
    if (userId !== undefined || userId !== null) {
      setOpcoValue([]);

      setOpcoResValue([]);

      setOrganisationData(undefined);
      setLoader("ADD", "GetUserManagementRoleGrid");
      if (location.state != null && location.state !== undefined) {
        const localState = location.state as any;
        if (localState?.id != null) {
          if (localState.idDetail && localState.idDetail != null) {
            Edit(localState?.id);
            return;
          }
          setLocalState(localState);
          Edit(localState?.id);
          const copy = {
            ...query,
            principalId: localState?.id,
          } as UserManagementQueryObjectGrid;
          setQuery(copy);
          GetUserManagementRoleGrid(copy).then(() =>
            setLoader("REMOVE", "GetUserManagementRoleGrid")
          );
        }
      } else setLoader("REMOVE", "GetUserManagementRoleGrid");
      if (props?.isAdd || props?.isView)
        GetUserResourceList().then((x: any) => {
          if (x !== undefined) {
            setAllROVList(x);
            const filteredDescriptions: Record<string, string> = {};
            Object.entries(x.roleDescriptionResource).forEach(
              ([key, value]) => {
                if (value && String(value).trim() !== "") {
                  filteredDescriptions[key] = String(value);
                }
              }
            );
            setRoleDescriptions(filteredDescriptions);
          }
        });
    }
  }, [userId]);

  const handleChange = (e: any) => {
    setFormFields({ ...formFields, [e.target.name]: e.target.value });
    setErrors({ email: "" });
  };

  const onChangeMainDropdown = (selected: any, type: string) => {
    switch (type) {
      case "role":
        setRoleValue(selected.map((v: any) => v.value));
        break;
      case "opco":
        setOpcoValue(selected.map((v: any) => v.value));

        break;
      case "subDomain":
        setSubDomainValue(selected?.key ? [selected.key] : []);
        break;
      case "vertical":
        setVerticalValue(selected.map((v: any) => v.value));
        setVerticalId(selected.map((v: any) => String(v.value)));
        setVerticalResValue(selected.map((v: any) => v.label));
        filterOrganisationDataByVertical(
          selected.map((v: any) => String(v.value))
        );
        break;
      case "restrictedOpco":
        setRestrictedOpcoValue(selected.map((v: any) => v.value));
        break;
      case "verticalResponsible":
        setVerticalResponsibleValue(selected.map((v: any) => v.value));
        break;
    }
  };

  const handleVerifyAndCreate = async (e: any) => {
    e.preventDefault();
    setIsVerifying(true);
    if (!formFields.email) {
      setErrors({ email: "Email is required" });
      setIsVerifying(false);
      return;
    }
    if (
      !/^[A-Za-z0-9._%+-]+@(vodafone|vodafone-itc)+\.com$/i.test(
        formFields.email
      )
    ) {
      setErrors({ email: "Invalid email address" });
      setIsVerifying(false);
      return;
    }
    if (
      emailList?.filter(
        (i) => formFields.email.toLowerCase() === i.value?.toLowerCase()
      ).length
    ) {
      setErrors({ email: "Email-Id already exist!" });
      setIsVerifying(false);
      return;
    }
    try {
      const result = await AddNewUser({
        email: formFields.email,
        active: true,
        userName: formFields.email,
      });
      if (result?.data?.userId) {
        setUserId(result.data.userId);
        setIsViewVisibleModal(true);
        setEditMail(true);
        setQuery({
          ...query,
          userId: [result.data.userId],
          email: [result.data.email],
          page: 1,
        });
        setTimeout(refresh, 500);
      }
    } catch (error) {
      setErrors({ email: "Error creating user." });
    } finally {
      setIsVerifying(false);
    }
  };

  const fetchRoles = async () => {
    try {
      const result = await GetUserManagementRoles();
      if (result) {
        if (Array.isArray(result.moduleResources))
          setModuleResources(result.moduleResources);
        if (Array.isArray(result.roleModuleResources))
          setRoleModuleResources(result.roleModuleResources);
      }
      return result;
    } catch (error) {
      console.error("Error fetching roles:", error);
    }
  };

  const handleManageMenuClick = async (rows: any[]) => {
    const response: any = await fetchRoles();
    const allRoleModules = (response?.roleModuleResources as any) || [];
    setRoleModuleResources(
      allRoleModules.filter((r: any) => roleValue.includes(Number(r.roleId)))
    );
    setShowMenuModal(true);
  };

  const handleSaveAllChanges = async () => {
    try {
      setLoader("ADD", "SaveAllChanges");

      const isOrgValid = orgRef.current?.validate?.();

      if (isOrgValid === false) return;
      const organisationPayload = orgRef.current?.getFormData?.() || {};
      const rolePayloadValue = {
        userId,
        userName: formFields.email,
        email: formFields.email,
        active: true,
        aspNetUserRoleId: data?.[0]?.aspNetUserRoleId || 0,
        opcoId: 0,
        verticalResponsibleId: 0,
        roleId: 0,
        opCoIds: opcoValue?.join(",") || "",
        restrictedOpCoIds: restrictedOpcoValue?.join(",") || "",
        verticalIds: verticalId?.join(",") || verticalValue?.join(",") || "",
        roleIds: roleValue?.join(",") || "",
        subDomainIds: subDomainValue?.join(",") || "",
        organisationIds:
          organisationData?.map((org: any) => org.organisationId).join(",") ||
          "",
        verticalResponsibleIds: verticalResponsibleValue?.join(",") || "",
      };
      const payload: any = {
        aspNetUserRoleCreateAndUpdateDto: rolePayloadValue,
        userPrefrenceDetails,
        userId,
        issubdomainspoc: organisationPayload.isSubDomainSpoc ?? isSubDomainSpoc,
        iseduspoc: organisationPayload.isEduSpoc ?? isEduSpoc,
        isdesigncontact: organisationPayload.isDesignContact ?? isDesignContact,
        subdomainresponsibleid: subDomainValue?.[0] || 0,
      };
      await CreateUserManagementRole(payload as any);
      refresh();
      onclose();
    } catch (error) {
      console.error("Save failed", error);
    } finally {
      setLoader("REMOVE", "SaveAllChanges");
    }
  };
  const onclose = () => {
    setEditMail(false);
    setUserId(null);
    setFormFields({ email: "", active: false });
    setIsViewVisibleModal(false);
    setOpcoValue([]);
    rootStore.dispatch({
      type: ADD_NEW_USER_MANAGEMENT,
      payload: { ResultDtoCreate: null, UserManagementRoleDtoCreate: null },
    });
    props.action.closeModal();
  };

  return (
    <>
      <MenuManagementModal
        show={showMenuModal}
        onClose={() => setShowMenuModal(false)}
        userId={userId}
        moduleResources={moduleResources}
        permissionResource={
          userPrefrenceDetails.length > 0
            ? userPrefrenceDetails
            : GridDto?.data?.userPrefrenceDetails?.length
            ? GridDto.data.userPrefrenceDetails
            : roleModuleResources
        }
        onSavePermissions={(m) => {
          setUserPrefrenceDetails(m);
          setShowMenuModal(false);
        }}
      />

      <div className="px-0 col-12">
        <div className="row mx-4">
          <div className="col-4 pl-0">
            <label htmlFor="email" className="voda-bold w-100 mt-2">
              Email
            </label>
            <InputGroup className="mb-3 mt-0">
              <input
                type="email"
                id="email"
                name="email"
                readOnly={props.isView || !!userId}
                disabled={editMail || !!userId}
                value={formFields.email}
                onChange={handleChange}
                className={`form-control ${
                  userId ? "border-green" : errors.email ? "border-red" : ""
                }`}
              />
              {editMail && !userId && (
                <Button
                  className="userSearch"
                  onClick={() => setEditMail(!editMail)}
                >
                  <FaEdit />
                </Button>
              )}
              {errors.email && (
                <label className="text-danger">{errors.email}</label>
              )}
              {userId && props.isAdd && (
                <label className="text-success d-block w-100">
                  User Created Successfully
                </label>
              )}
            </InputGroup>
          </div>
          <div className="col-8 pr-0" style={{ marginTop: 32 }}>
            {props.isAdd && !userId ? (
              <button
                className="btn btn-danger px-4"
                onClick={handleVerifyAndCreate}
                disabled={isVerifying}
              >
                {isVerifying ? "Creating User..." : "Verify & Create User"}
              </button>
            ) : // (
            //   <button
            //     className="btn btn-danger px-4"
            //     onClick={() =>
            //       handleManageMenuClick(
            //         data?.map((item: any) => ({
            //           ...item,
            //           isNew: false,
            //           role: item.role
            //             ? { label: item.role, value: item.role }
            //             : null,
            //           opCo: item.opCo
            //             ? item.opCo
            //                 .split(",")
            //                 .map((op: any) => {
            //                   const f = dictionaryToArray(
            //                     allROVList?.opCoResource
            //                   )?.find((x: any) => x.value === op.trim());
            //                   return f
            //                     ? { key: f.key, label: f.value, value: f.value }
            //                     : null;
            //                 })
            //                 .filter(Boolean)
            //             : [],
            //           verticalRes: item.verticalResponsible
            //             ? item.verticalResponsible
            //                 .split(",")
            //                 .map((v: any) => {
            //                   const f = dictionaryToArray(
            //                     allROVList?.verticalResource
            //                   )?.find((x: any) => x.value === v.trim());
            //                   return f
            //                     ? { key: f.key, label: f.value, value: f.value }
            //                     : null;
            //                 })
            //                 .filter(Boolean)
            //             : [],
            //         })) ?? []
            //       )
            //     }
            //   >
            //     Manage Access
            //   </button>
            // )
            null}
          </div>
        </div>

        {(userId || props?.userInfo?.userId) && (
          <div className="row mx-4">
            {organisationData && (
              <>
                <EmbeddedOrganizationInfo
                  opcoRes={allROVList?.opCoResource}
                  roleRes={allROVList?.roleResource}
                  subdomRes={allROVList?.subdomainResbonsibleResource}
                  verticalRes={allROVList?.verticalResource}
                  verticalResponsibleRes={
                    allROVList?.verticalResponcibleResource
                  }
                  verticalResponsibleValue={verticalResponsibleValue}
                  opcoValue={opcoValue}
                  roleValue={roleValue}
                  subDomainValue={subDomainValue}
                  verticalValue={verticalValue}
                  restrictedOpcoValue={restrictedOpcoValue}
                  isDesignContact={isDesignContact}
                  isEduSpoc={isEduSpoc}
                  isSubDomainSpoc={isSubDomainSpoc}
                  ref={orgRef}
                  initialData={organisationData}
                  userId={userId}
                  userEmail={formFields.email}
                  onChangeMainDropdown={onChangeMainDropdown}
                  readOnly={false}
                  roleDescriptions={roleDescriptions}
                />
              </>
            )}
          </div>
        )}

        {editMail && (
          <div className="row">
            <div className="col-12 justify-content-end mt-4 d-flex footerModal">
              <button className="btn btn-link px-4 cancel" onClick={onclose}>
                Cancel
              </button>
              <button
                className="btn btn-danger px-4"
                onClick={handleSaveAllChanges}
              >
                Save All Changes
              </button>
            </div>
          </div>
        )}
      </div>
    </>
  );
};

export default UserManagementForm;
