import React, { useEffect, useState } from "react";
import { Button, InputGroup, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import Select from "react-select";
import Container from "../../Components/Container";
import ModalConfirm from "../../Components/ModalConfirm";
import Location from "../../Containers/Lookup/LocationContainer";
import OpcoContainer from "../../Containers/Lookup/OpCoContainer";
import OriginalEquipmentManufacturer from "../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  changeDate,
  changeText,
  formatDateWithTime,
  formatTime,
} from "../../Hook/Common";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import {
  NetworkElementAsIsDtoCreate,
  NetworkElementAsIsDtoUpdate,
  NetworkElementAsIsSystemTypeInfo,
} from "../../Model/NetworkElementAsIs";
import {
  GetSystemTypeInfo,
  GetSystemTypeList,
  GetAsPlannedLocation,
  GetNetworkElementAsPlannedResource,
  GetSystemTypeListFromAsPlanned,
} from "../../Redux/Action/NetworkElementAsIs/NetworkElementAsIsCommonAction";
import { EditNetworkElementAsIs } from "../../Redux/Action/NetworkElementAsIs/NetworkElementAsIsEditAction";
import { RootState } from "../../Redux/Store/rootStore";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import DatePicker from "react-datepicker";
import {
  UserManagementDtoCreate,
  UserManagementQueryObjectGrid,
} from "../../Model/UserManagement";
import {
  AddNewUser,
  CreateUserManagement,
  EditUserManagement,
  GetListOfEmailIds,
} from "../../Redux/Action/UserManagement/UserManagementGridAction";
import { FaSearch, FaRegThumbsUp, FaRegThumbsDown } from "react-icons/fa";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import { ResultDto } from "../../Model/CommonModels";

let paginationQuery: UserManagementQueryObjectGrid = {
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

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
    Edit(id: number | undefined): any;
  };
  // data: NetworkElementAsIsDtoUpdate | LcmEngineeringDtoCreate | undefined | null,
  edit: boolean;
}

const UserManagementModal: React.FC<Props> = (props) => {
  const { tipologicaPermesso, isPermesso, pageSize } = useAuth();
  const [isMailEmpty, setIsMailEmpty] = useState<boolean>(false);
  const [emailList, setEmailList] = useState<FilterValueDto[] | undefined>();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    undefined
  );

  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeSelect,
    onChangeDate,
    setChanged,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<UserManagementDtoCreate>(AddNewUser, EditUserManagement);

  useEffect(() => {
    async function getEmailList() {
      let result = await GetListOfEmailIds("email", "", paginationQuery);
      setEmailList(result);
    }
    getEmailList();
  }, []);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);

  //VALIDAZIONE PRE Save
  const validazionClient = (copy: UserManagementDtoCreate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (copy?.userName == null || copy?.userName === undefined) {
      addInvalidProperty("userName");
    }
    if (copy?.email == null || copy?.email === undefined) {
      addInvalidProperty("email");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const OpCoRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.opCoResource)
      formData.opCoResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const [disableForm, setDisableForm] = useState<boolean>(false);

  const [editState, setEditState] = useState<number>(0);
  const [typeActive, setTypeActive] = useState<number>(0);
  const [mailCheck, setMailCheck] = useState("YetToCheck");

  const RestoreOrphanDeleted = (
    id: number | undefined,
    orphanDeletedValue?: boolean
  ) => {
    setOrphanDeleted(orphanDeletedValue ?? true);
    props.action.Edit(id);
    if (orphanDeletedValue === false) {
      setDisableForm(true);
      setChanged(false);
    }
  };

  const validateEmail = () => {
    if (formData?.email == null || formData.email == undefined) {
      setIsMailEmpty(true);
    } else if (emailList) {
      setIsMailEmpty(false);
      let isAvailable = emailList.filter(
        (item) => formData.email === item.value
      );

      if (isAvailable.length > 0) {
        setMailCheck("No");
      } else {
        setMailCheck("Yes");
      }
    }
  };
  return (
    <div className="px-0 col-12">
      <ModalConfirm data={confirmForm} />
      <ModalConfirm data={confirm} />
      <Modal
        show={isVisibleModalLookup > 0}
        backdrop="static"
        backdropClassName="backdropGrid"
        dialogClassName="dialogGrid"
        className="modalGrid"
        keyboard={false}
        size="lg"
        centered
        onHide={() => setIsVisibleModalLookup(0)}
      >
        <Modal.Header>
          <div className="col-12 px-0">
            <div className="col-12">
              {/* <h4 className="mb-0">Lookup Tables</h4> */}
            </div>
            {/* <ErrorNotification OnModal={true} /> */}
          </div>
        </Modal.Header>
        {/* <Modal.Body>{ReturnLookupContainer(isVisibleModalLookup)}</Modal.Body> */}
      </Modal>
      <form id="formSoftwareBuild" onChange={() => setChanged(true)}>
        <div className="row col-12">
          <div className="col-12 p-0">
            <fieldset className="fieldset">
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12">
                    <label className="voda-bold w-100 mt-2">
                      User Name
                      <span className="red">*</span>
                    </label>
                    <InputGroup className="mb-3 mt-0">
                      <InputGroup.Text className="border-radius-none noStyle w-100 p-0">
                        <input
                          type="text"
                          // readOnly={props.edit}
                          onChange={(e) => onChange("userName", e)}
                          onKeyUp={(e) => onChange("userName", e)}
                          className="form-control border-radius-none textBorderBg"
                          value={formData?.userName}
                        />
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes("userName") ? (
                          <label className="validation">
                            *User Name must have a value
                          </label>
                        ) : null}
                      </InputGroup.Text>
                    </InputGroup>
                  </div>
                </div>
                <div className="col-6 pl-4 pr-0">
                  <div className="col-12 px-0">
                    <label className="voda-bold w-85 mt-2">
                      Email ID <span className="red">*</span>
                    </label>
                    <InputGroup className="mb-3 mt-0">
                      <InputGroup.Text className="border-radius-none noStyle w-100 p-0">
                        <input
                          type="text"
                          // readOnly={props.edit}
                          onChange={(e) => onChange("email", e)}
                          onKeyUp={(e) => onChange("email", e)}
                          className="form-control border-radius-none textBorderBg h-100 py-0 pr-0"
                          value={formData?.email}
                          disabled={mailCheck === "Yes" ? true : false}
                        />
                      </InputGroup.Text>
                      <InputGroup.Text className="p-0">
                        {(mailCheck == "YetToCheck" || mailCheck == "No") && (
                          <Button
                            type="button"
                            className="textBorderBg border-radius-none userSearch"
                            data-toggle="tooltip"
                            data-placement="top"
                            title="Check Availability"
                            onClick={() => validateEmail()}
                          >
                            <FaSearch color="" />
                          </Button>
                        )}
                        {mailCheck == "No" && (
                          <Button
                            type="button"
                            className="textBorderBg border-radius-none userSearch"
                            data-toggle="tooltip"
                            data-placement="top"
                            title="Already Taken"
                            //onClick={() => validateEmail()}
                          >
                            <FaRegThumbsDown color="Red" />
                          </Button>
                        )}
                        {mailCheck == "Yes" && (
                          <Button
                            type="button"
                            className="textBorderBg border-radius-none userSearch"
                            data-toggle="tooltip"
                            data-placement="top"
                            title="Available"
                            //onClick={() => validateEmail()}
                          >
                            <FaRegThumbsUp color="Green" />
                          </Button>
                        )}
                      </InputGroup.Text>
                    </InputGroup>
                    {isMailEmpty ||
                    (validation &&
                      validation.response == false &&
                      validation.property?.includes("email")) ? (
                      <label className="validation">
                        *Email ID must have a value
                      </label>
                    ) : mailCheck == "Yes" ? (
                      <label className="validation green-color">
                        Email ID is Available
                      </label>
                    ) : mailCheck == "No" ? (
                      <label className="validation">
                        Mail ID already Taken!
                      </label>
                    ) : null}
                    {/* <div className="">
                      <label className="  labelForm voda-bold mb-0 w-100 fontFamily">
                        <div className="input-group">
                          <input
                            type="text"
                            // readOnly={props.edit}
                            
                            onChange={(e) => onChange("email", e)}
                            onKeyUp={(e) => onChange("email", e)}
                            className="inputForm w-90"
                            value={formData?.email}
                            disabled={mailCheck === "Yes" ? true : false}
                          />
                          <div className="input-group-append ">
                          <div
                            className="btn-group mr-2"
                            role="group"
                            aria-label="First group"
                          >
                            {(mailCheck == "YetToCheck" || mailCheck == "No") && (
                              <button
                                type="button"
                                className="btn pt-2 pb-2"
                                data-toggle="tooltip"
                                data-placement="top"
                                title="Check Availability"
                                onClick={() => validateEmail()}
                              >
                                <FaSearch color="" />
                              </button>
                            )}
                            {mailCheck == "No" && (
                              <button
                                type="button"
                                className="btn pt-2 pb-2"
                                data-toggle="tooltip"
                                data-placement="top"
                                title="Already Taken"
                                //onClick={() => validateEmail()}
                              >
                                <FaRegThumbsDown color="Red" />
                              </button>
                            )}
                            {mailCheck == "Yes" && (
                              <button
                                type="button"
                                className="btn pt-2 pb-2"
                                data-toggle="tooltip"
                                data-placement="top"
                                title="Available"
                                //onClick={() => validateEmail()}
                              >
                                <FaRegThumbsUp color="Green" />
                              </button>
                            )}
                          </div>
                          </div>
                        </div>
                      </label>
                    </div> */}
                  </div>
                </div>
              </div>
            </fieldset>
          </div>
        </div>
      </form>

      <div className="col-12 justify-content-end mt-4 d-flex footerModal">
        <button
          className=" voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={() =>
            Save(
              {
                ...formData,
                active: true,
              },
              props.edit,
              validazionClient,
              refresh,
              RestoreOrphanDeleted,
              orphanDeleted
            )
          }
          disabled={mailCheck == "Yes" ? false : true}
          type="button"
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default UserManagementModal;
