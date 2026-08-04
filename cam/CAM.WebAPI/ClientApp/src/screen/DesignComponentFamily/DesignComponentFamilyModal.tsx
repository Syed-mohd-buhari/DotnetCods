import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";

import {
  DesignComponentFamilyDtoUpdate,
  DesignComponentFamilyDtoCreate,
} from "../../Model/DesignComponentFamily";
import ImplementationStatusTable from "../../Containers/Lookup/ImplementationStatusTable";

import Select from "react-select";
import { formatDateWithTime, numberIsNullOrZero } from "../../Hook/Common";
import { CreatDesignComponentFamily } from "../../Redux/Action/DesignComponentFamily/DesignComponentFamilyCreateAction";
import { useSelector } from "react-redux";
import { EditDesignComponentFamily } from "../../Redux/Action/DesignComponentFamily/DesignComponentFamilyEditAction";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import ModalConfirm from "../../Components/ModalConfirm";
import Container from "../../Components/Container";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";
import { Form, Modal } from "react-bootstrap";
import SubNetworkBoundary from "../../Containers/Lookup/SubNetworkBoundaryContainer";
import SystemFunction from "../../Containers/Lookup/SystemFunctionContainer";
import { TipologicaGridDto } from "../../Model/LookUp/LookUpGenericModel";
import SharingType from "../../Containers/Lookup/SharingTypeContainer";
import { SubNetworkBoundaryGridDto } from "../../Model/LookUp/SubnetworkBoundry";
import NetworkFunction from "../../Containers/Lookup/NetworkFunctionContainer";

import CriticalAssetType from "../../Containers/Lookup/CriticalAssetTypeContainer";
import CustomerWheel from "../../Containers/Lookup/CustomerWheelContainer";
import { GetOpenImplementationStatus } from "../../Redux/Action/DesignComponentFamily/DesignComponentFamilyGridAction";
import ImplementationTable from "../../Containers/Lookup/Implementation";
import { GetServicesOfSubNetworkBoundaries } from "../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryCreateAction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";

export const GDR_ENUM = {
  2: "High",
  1: "Medium",
  0: "Low",
};
// interface Props {
//   action: {
//     closeModal?(changed?: boolean): any;
//     refresh?(): any;
//     Edit?(id: number | undefined): any;
//     validateFormWizard?(
//       response: boolean,
//       formData: DesignComponentFamilyDtoCreate,
//       property: string
//     );
//     wizardBackFunction?(
//       formData: DesignComponentFamilyDtoCreate,
//       property: string
//     ): any;
//     setConfirmExitWizard?(): any;
//     setIsVisibleServiceBoundaryModalLookup?(isVisible: boolean);
//   };
//   // data: DesignComponentFamilyDtoUpdate | DesignComponentFamilyDtoCreate | undefined | null,
//   systemSolution?: string | undefined;
//   edit: boolean;
//   keyTab?: string;
//   wizardMode: boolean;
//   wizardStep?: number;
//   dataWizard?: DesignComponentFamilyDtoCreate;
// }

const ModalDesignComponentFamily = (props) => {
  const [keyTabs, setKey] = useState("DesignComponentFamily");
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeSelect,
    setChanged,
    setInputValue,
    confirmForm,
  } = useFormTableCrud<DesignComponentFamilyDtoUpdate>(
    CreatDesignComponentFamily,
    EditDesignComponentFamily
  );

  const { tipologicaPermesso } = useAuth();

  const dtoEditResourceState = (state: RootState) =>
    state.designComponentFamilyEditReducer.DesignComponentFamilyDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.designComponentFamilyCreateReducer.DesignComponentFamilyDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  // const [isVisibleServiceBoundaryModalLookup, setIsVisibleServiceBoundaryModalLookup] = useState<boolean>(false);
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [criticalCount, setCriticalCount] = useState<number>(0);

  const Grid = (state: RootState) =>
    state.sharingTypeGridReducer.LookUpGridResult;
  const GridDto = useSelector(Grid);
  const GridAll = (state: RootState) =>
    state.sharingTypeGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  const SubnetworkBoundaryGrid = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.LookUpGridResult;
  const SubnetworkBoundaryGridDto = useSelector(SubnetworkBoundaryGrid);
  const SubnetworkBoundaryGridAll = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.LookUpGridResultAll;
  const SubnetworkBoundaryGridDtoAll = useSelector(SubnetworkBoundaryGridAll);

  const NetworkFunctionGrid = (state: RootState) =>
    state.networkFunctionGridReducer.LookUpGridResult;
  const NetworkFunctionGridDto = useSelector(NetworkFunctionGrid);
  const NetworkFunctionGridAll = (state: RootState) =>
    state.networkFunctionGridReducer.LookUpGridResultAll;
  const NetworkFunctionGridDtoAll = useSelector(NetworkFunctionGridAll);

  const SystemFunctionGrid = (state: RootState) =>
    state.systemFunctionGridReducer.LookUpGridResult;
  const SystemFunctionGridDto = useSelector(SystemFunctionGrid);
  const SystemFunctionGridAll = (state: RootState) =>
    state.systemFunctionGridReducer.LookUpGridResultAll;
  const SystemFunctionGridDtoAll = useSelector(SystemFunctionGridAll);

  const [implementationTableData, setImplementationTableData] = useState<any>();

  const [supportedServiceList, setSupportedServiceList] = useState<any>([]);
  const [showFurtherDetails, setShowFurtherDetails] = useState<boolean>(false);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
      onChangeCount(
        "countrySpecificCriticality",
        editResource?.countrySpecificCriticality!
      );
    } else if (!props.wizardMode) {
      setFormData(createResource);
    } else {
      setFormData(props.dataWizard);
    }
  }, [createResource, editResource, props.edit, props.dataWizard]);

  useEffect(() => {
    if (formData && formData?.subNetworkBoundaryId) {
      GetServicesOfSubNetworkBoundaries([formData?.subNetworkBoundaryId]).then(
        (res) => {
          const arr = dictionaryToArray(res);
          setSupportedServiceList(arr);
        }
      );
    } else {
      setSupportedServiceList([]);
    }
  }, [formData?.subNetworkBoundaryId]);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("DesignComponentFamily");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  const validazioneClient = (copy: DesignComponentFamilyDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy.subNetworkBoundaryId === null ||
      copy?.subNetworkBoundaryId === undefined ||
      copy?.subNetworkBoundaryId === 0
    ) {
      addInvalidProperty("subNetworkBoundaryId");
    }
    if (copy.systemIsShared === true) {
      if (numberIsNullOrZero(copy.sharingTypeId)) {
        addInvalidProperty("sharingTypeId");
      }
    }

    if (
      copy.systemTypeIdentityName === null ||
      copy.systemTypeIdentityName === undefined ||
      copy.systemTypeIdentityName === ""
    ) {
      addInvalidProperty("systemTypeIdentityName");
    }
    if (
      copy.description === null ||
      copy.description === undefined ||
      copy.description.trim() === ""
    ) {
      addInvalidProperty("description");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const RestoreOrphanDeleted = (id: number | undefined) => {
    setOrphanDeleted(true);
    props.action.Edit && props.action.Edit(id);
  };

  const validateWizard = () => {
    let copy = { ...formData } as DesignComponentFamilyDtoCreate;
    props.action.validateFormWizard &&
      props.action.validateFormWizard(
        validazioneClient(copy).response,
        copy,
        "designComponentFamilyDto"
      );
  };

  const onChangeCount = (type: string, e: boolean) => {
    let copy = { ...formData } as DesignComponentFamilyDtoCreate;

    if (
      type === "countrySpecificCriticality" &&
      e !== copy.countrySpecificCriticality &&
      e
    ) {
      setCriticalCount((prevState) => prevState + 1);
    } else if (
      type === "countrySpecificCriticality" &&
      e !== copy.countrySpecificCriticality &&
      !e &&
      criticalCount !== 0
    ) {
      setCriticalCount((prevState) => prevState - 1);
    } else {
      return;
    }
  };

  const SystemFunctionRefillData = (value: TipologicaGridDto[] | undefined) => {
    let copy = { ...formData } as DesignComponentFamilyDtoUpdate;

    var obj = value?.reduce((acc, item) => {
      if (item.id !== null && item.id !== undefined) {
        return { ...acc, [item.id]: item.description } as TipologicaGridDto;
      } else {
        return {} as TipologicaGridDto;
      }
    }, {});
    if (copy && copy?.systemFunctionsResource)
      copy.systemFunctionsResource = obj as { [key: string]: string };

    setFormData(copy);
  };

  const NetworkFunctionRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.networkFunctionsResource)
      formData.networkFunctionsResource = obj as {
        [key: string]: string;
      };
    setFormData(formData);
  };

  const SharingTypeRefillData = (value: TipologicaGridDto[] | undefined) => {
    let copy = { ...formData } as DesignComponentFamilyDtoUpdate;

    var obj = value?.reduce((acc, item) => {
      if (item.id !== null && item.id !== undefined) {
        return { ...acc, [item.id]: item.description } as TipologicaGridDto;
      } else {
        return {} as TipologicaGridDto;
      }
    }, {});
    if (copy && copy?.sharingTypeResource)
      copy.sharingTypeResource = obj as { [key: string]: string };

    setFormData(copy);
  };

  const serviceBoundaryRefillData = (
    value: SubNetworkBoundaryGridDto[] | undefined
  ) => {
    // let copy = { ...formData } as DesignComponentFamilyDtoUpdate;
    // var obj = value?.reduce((acc, item) => {
    //   if (item.subNetworkBoundaryId) {
    //     return {
    //       ...acc,
    //       [item.subNetworkBoundaryId]: item.alias
    //         ? item.alias
    //         : item.subNetworkBoundaryDescription,
    //     } as SubNetworkBoundaryGridDto;
    //   } else {
    //     return {} as SubNetworkBoundaryGridDto;
    //   }
    // }, {});
    // if (copy && copy?.subNetworkBoundariesResource)
    //   copy.subNetworkBoundariesResource = obj as { [key: string]: string };
    // setFormData(copy);
  };

  const onHideModel = () => {
    let dataCopy;
    if (isVisibleModalLookup === 10) {
      dataCopy = [...(SubnetworkBoundaryGridDtoAll?.items ?? [])];
      serviceBoundaryRefillData(dataCopy);
    } else if (isVisibleModalLookup === 12) {
      dataCopy = [...(SystemFunctionGridDtoAll?.items ?? [])];
      SystemFunctionRefillData(dataCopy);
    } else if (isVisibleModalLookup === 13) {
      dataCopy = [...(GridDtoAll?.items ?? [])];
      SharingTypeRefillData(dataCopy);
    } else if (isVisibleModalLookup === 4) {
      dataCopy = [...(NetworkFunctionGridDtoAll?.items ?? [])];
      NetworkFunctionRefillData(dataCopy);
    }
  };

  const returnLookup = () => {
    switch (isVisibleModalLookup) {
      case 10:
        return (
          <SubNetworkBoundary
            returnObject={serviceBoundaryRefillData}
            modal={{
              isModal: true,
              setIsVisibleModalLookup: (e: number) =>
                setIsVisibleModalLookup(e),
            }}
            isPopup={true}
          />
        );
      case 12:
        return (
          <SystemFunction
            returnObject={SystemFunctionRefillData}
            modal={{
              isModal: true,
              setIsVisibleModalLookup: (e: number) =>
                setIsVisibleModalLookup(e),
            }}
          />
        );
      case 13:
        return (
          <SharingType
            returnObject={SharingTypeRefillData}
            modal={{
              isModal: true,
              setIsVisibleModalLookup: (e: number) =>
                setIsVisibleModalLookup(e),
            }}
          />
        );

      case 4:
        return (
          <NetworkFunction
            returnObject={NetworkFunctionRefillData}
            modal={{
              isModal: true,
              setIsVisibleModalLookup: (e: number) =>
                setIsVisibleModalLookup(e),
            }}
          />
        );

      default:
        return null;
    }
  };

  const onChangeNetworkFunctionIds = (obj: any) => {
    const copy = { ...formData } as DesignComponentFamilyDtoUpdate;
    if (obj !== null && obj !== undefined)
      copy.networkFunctionsIds = obj.map((el) => el.key);
    else copy.networkFunctionsIds = [];
    setFormData(copy);
  };

  const onChangeSharingType = (id: number) => {
    let copy = { ...formData } as DesignComponentFamilyDtoUpdate;
    copy.sharingTypeId = id;
    setFormData(copy);
  };
  const onChangeSystemIsShared = (val: boolean) => {
    let copy = { ...formData } as DesignComponentFamilyDtoUpdate;
    if (val === false) {
      copy.sharingTypeId = undefined;
    }
    copy.systemIsShared = val;
    setFormData(copy);
  };

  return (
    <div className="">
      <ModalConfirm data={confirmForm} />

      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => {
          onHideModel();
          setIsVisibleModalLookup(0);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            {isVisibleModalLookup === 2
              ? "Preview Design Components for"
              : isVisibleModalLookup === 1
              ? "This Design Component Family is implemented in"
              : isVisibleModalLookup === 3
              ? "Further Implementation Details"
              : ""}
            <span
              dangerouslySetInnerHTML={{
                __html:
                  isVisibleModalLookup === 2
                    ? implementationTableData?.dcfName
                    : "",
              }}
            ></span>
          </div>
        </DialogTitle>
        <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
          <IconButton
            aria-label="close"
            onClick={() => {
              onHideModel();
              setIsVisibleModalLookup(0);
            }}
          >
            <IoClose size={25} />
          </IconButton>
        </Box>
        <DialogContent>{returnLookup()}</DialogContent>
      </Dialog>

      <form
        id="formDesignComponentFamily"
        className="plr-10"
        onChange={() => setChanged(true)}
      >
        <div className="row">
          <div className="col-6 mb-2">
            <div className="form-group">
              <label className="labelForm voda-bold   w-100 mb-0">
                System Type Identity Name <span className="red">*</span>
                <div
                  className="col-12 pl-4 customFakeInput disabled"
                  style={{ marginTop: "4px" }}
                >
                  <label
                    className="labelForm  w-100 mb-0"
                    dangerouslySetInnerHTML={{
                      __html: formData?.systemTypeIdentityName ?? "",
                    }}
                  ></label>
                </div>
                {/* <input type="text" disabled className="inputForm w-100" defaultValue={formData?.hardwareSolution} /> */}
                {validation &&
                validation.response === false &&
                validation.property?.includes("systemTypeIdentityName") ? (
                  <label className="validation">
                    *System Type Identity Name must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          <div className="col-6 mb-2">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                SubNetwork Boundary <span className="red">*</span>
                <div className=" d-flex ">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={
                      formData?.subNetworkBoundariesResource &&
                      dictionaryToArray(formData?.subNetworkBoundariesResource)
                    }
                    value={
                      formData?.subNetworkBoundariesResource &&
                      dictionaryToArray(
                        formData?.subNetworkBoundariesResource
                      ).filter(
                        (el) => el.key === formData?.subNetworkBoundaryId
                      )
                    }
                    onChange={(e) => {
                      onChangeSelect("subNetworkBoundaryId", e);
                    }}
                    isDisabled={props.edit ? true : false}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                  {tipologicaPermesso && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(10)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
                {validation &&
                validation.response === false &&
                validation.property?.includes("subNetworkBoundaryId") ? (
                  <label className="validation">
                    *Service Boundary must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          {supportedServiceList.length ? (
            <div className="col-6 mb-2">
              <div className="form-group">
                <label className="labelForm voda-bold mb-0 w-100">
                  Supported Service
                  <div className="d-flex">
                    <Select
                      menuPosition={"fixed"}
                      className="w-100"
                      options={supportedServiceList}
                      value={supportedServiceList}
                      isMulti
                      isSearchable
                      isClearable
                      isDisabled
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option.key.toString()}
                    />
                  </div>
                </label>
              </div>
            </div>
          ) : null}

          {/* <div className="col-6 mb-2">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Network Function
                <div className="d-flex">
                  <Select
                    className="w-100"
                    options={
                      formData?.networkFunctionsResource &&
                      dictionaryToArray(formData?.networkFunctionsResource)
                    }
                    value={
                      formData?.networkFunctionsResource &&
                      dictionaryToArray(
                        formData?.networkFunctionsResource
                      ).filter((el) =>
                        formData?.networkFunctionsIds?.includes(el.key)
                      )
                    }
                    onChange={(e) => onChangeNetworkFunctionIds(e)}
                    onBlur={() => setInputValue("")}
                    isMulti
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                  {tipologicaPermesso && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(4)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
                {validation &&
                validation.response === false &&
                validation.property?.includes("networkFunctionsIds") ? (
                  <label className="validation">
                    *System Function must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div> */}

          <div className="col-6 mb-2 d-flex row mx-0">
            <div className="form-group col-12 px-0">
              <label className="labelForm voda-bold w-100 d-flex align-items-center mb-1">
                {/* System Is Shared */}
                Does this Design Component Family share it's hardware solution
                with Other Design Component Families?
              </label>
              <label className="w-100">
                <div className="d-flex align-items-center mr-3">
                  <label className="switch">
                    <input
                      type="checkbox"
                      onChange={(e) => onChangeSystemIsShared(e.target.checked)}
                      className="mr-1"
                      checked={formData?.systemIsShared}
                    />
                    <span className="slider round"></span>
                  </label>
                </div>
              </label>
            </div>
          </div>
          <div className="col-6 mb-2">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Implementation
                <div className="d-flex">
                  <input
                    type="text"
                    disabled
                    className="inputForm w-100"
                    value={formData?.implementation ? "Yes" : "No"}
                  />
                  <button
                    className="btn btn-link"
                    onClick={() => {
                      props.onOpenImplementationPopUp(
                        formData?.designComponentFamilyId
                      );
                    }}
                    type="button"
                    disabled={!formData?.implementation ? true : false}
                  >
                    <img
                      style={{ height: 15 }}
                      src={require("../../img/export_icon.png")}
                      alt="plus"
                    />
                  </button>
                </div>
              </label>
            </div>
          </div>
          <div className="col-12">
            {formData?.systemIsShared && (
              <div className="form-group col-12 px-0 mx-0">
                <label className="labelForm voda-bold mb-0 w-100">
                  Sharing Type <span className="red">*</span>
                </label>
                <div className="d-flex">
                  <div
                    className="d-flex flex-column"
                    style={{ marginTop: "14px", gap: "10px" }}
                  >
                    {formData &&
                      formData.sharingTypeResource &&
                      dictionaryToArray(formData.sharingTypeResource).map(
                        (x) => (
                          <label
                            className="mb-1 w-100"
                            onClick={(e) => onChangeSharingType(x.key)}
                            key={x.key}
                          >
                            <div className="d-flex align-items-center ">
                              <input
                                type="radio"
                                className="mr-1"
                                onChange={(e) => onChangeSharingType(x.key)}
                                checked={formData?.sharingTypeId === x.key}
                              />
                              <label className="mb-0 labelForm">
                                {x.value}
                              </label>
                            </div>
                          </label>
                        )
                      )}
                  </div>
                  {tipologicaPermesso && (
                    <button
                      className="btn btn-link ml-15"
                      onClick={() => setIsVisibleModalLookup(13)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../img/Plus_white.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>

                {validation &&
                validation.response === false &&
                validation.property?.includes("sharingTypeId") ? (
                  <label className="validation w-100">
                    *sharing Type must have a value
                  </label>
                ) : null}
              </div>
            )}
          </div>

          <div className="col-12">
            <button
              type="button"
              className="mb-20 mt-20 further-btn"
              onClick={() => setShowFurtherDetails(!showFurtherDetails)}
            >
              Click for further details
            </button>
          </div>

          {showFurtherDetails && (
            <>
              {/* <div className="col-12">
                <label className="labelForm voda-bold mb-0 w-100">
                  Criticality Rating
                </label>
                <p className="mb-3">{formData?.criticalityRating}</p>
              </div> */}

              {/* <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold mb-0 w-100">
                    Country Specific Criticality
                    <div className=" d-flex ">
                      <Select
                        className="w-100"
                        options={[
                          { key: 1, value: "Yes" },
                          { key: 0, value: "No" },
                        ]}
                        value={[
                          { key: 1, value: "Yes" },
                          { key: 0, value: "No" },
                        ].filter(
                          (el) =>
                            (el.key === 1 ? true : false) ===
                            formData?.countrySpecificCriticality
                        )}
                        onChange={(e) => {
                          onChangeCount(
                            "countrySpecificCriticality",
                            e && e["key"] === 1 ? true : false
                          );
                          onChangeSelect("countrySpecificCriticality", e);
                        }}
                        onBlur={() => setInputValue("")}
                        isSearchable
                        isDisabled={true}
                        isClearable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option.key.toString()}
                      />
                    </div>
                  </label>
                </div>
              </div> */}

              <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold w-100 mb-0">
                    Description <span className="red">*</span>
                  </label>
                  <div className="form-group">
                    <textarea
                      maxLength={1000}
                      className="inputForm w-100 voda-regular"
                      onChange={(e) => onChange("description", e)}
                      value={formData?.description ?? undefined}
                    />
                  </div>
                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("description") ? (
                    <label className="validation">
                      *Please add Description.
                    </label>
                  ) : null}
                </div>
              </div>

              {props.edit === true ? (
                <div className="col-md-12 row mx-0 px-0 mt-4">
                  <div className="col-md-6">
                    <div className="form-group">
                      <label className="labelForm voda-bold w-100">
                        Last Modified
                        <input
                          readOnly={true}
                          className="inputForm w-100 voda-regular"
                          type="text"
                          value={
                            formatDateWithTime(
                              formData?.lastModified
                            )?.toUpperCase() ?? ""
                          }
                        />
                      </label>
                    </div>
                  </div>
                  <div className="col-md-6">
                    <div className="form-group">
                      <label className="labelForm voda-bold   w-100">
                        Last Modified By
                        <input
                          readOnly={true}
                          className="inputForm w-100 voda-regular"
                          type="text"
                          value={formData?.lastModifiedBy ?? ""}
                        />
                      </label>
                    </div>
                  </div>
                </div>
              ) : null}
            </>
          )}
        </div>
      </form>

      <Container show={!props.wizardMode}>
        <div className="col-12 justify-content-end mt-4 d-flex footerModal">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() =>
              props.action.closeModal && props.action.closeModal(changed)
            }
            type="button"
          >
            Cancel
          </button>
          <button
            className={` voda-bold btn btn-danger px-4 btnHeader ${
              props.prevPage === "generatelcmdb" && props.dcfRedirect === true
                ? "disabledCursor"
                : ""
            }`}
            onClick={() =>
              Save(
                { ...formData, criticalityRating: criticalCount },
                props.edit,
                validazioneClient,
                refresh,
                RestoreOrphanDeleted,
                orphanDeleted
              )
            }
            type="button"
            data-toggle="tooltip"
            data-placement="top"
            title={
              props.prevPage === "generatelcmdb" && props.dcfRedirect === true
                ? `Saving is disabled due to redirection from LCM Export screen`
                : ""
            }
            disabled={
              props.prevPage === "generatelcmdb" && props.dcfRedirect === true
                ? true
                : props.formDisabed
            }
          >
            Save
          </button>
        </div>
      </Container>

      <Container show={props.wizardMode}>
        <div className="col-12 d-flex justify-content-between py-4 border-top mt-4">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            type="button"
            onClick={() =>
              props.action.setConfirmExitWizard &&
              props.action.setConfirmExitWizard()
            }
          >
            Exit
          </button>
          <div className="">
            <button
              disabled={!(props.wizardStep && props.wizardStep > 1)}
              className="  voda-bold btn btn-link px-4 btnHeader cancel"
              type="button"
              onClick={() =>
                props.action.wizardBackFunction &&
                formData &&
                props.action.wizardBackFunction(
                  formData,
                  "designComponentFamilyDto"
                )
              }
            >
              Back
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={() => validateWizard()}
            >
              Continue
            </button>
          </div>
        </div>
      </Container>
    </div>
  );
};

export default ModalDesignComponentFamily;
