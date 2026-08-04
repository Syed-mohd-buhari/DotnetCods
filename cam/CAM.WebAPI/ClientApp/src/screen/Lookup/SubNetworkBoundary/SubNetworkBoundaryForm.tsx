import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import {
  CreatSubNetworkBoundary,
  GetSNewSubnetworkBoundaryDescription,
} from "../../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryCreateAction";
import { EditSubNetworkBoundary } from "../../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundaryEditAction";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { SubNetworkBoundaryGridDto } from "../../../Model/LookUp/SubnetworkBoundry";
import Select from "react-select";
import { useAuth } from "../../../Hook/useAuth";
import { Form, Modal } from "react-bootstrap";
import SupportedService from "../../../Containers/Lookup/SupportedServiceContainer";
import { dictionaryToArray } from "./../../../Hook/Dictionary";

import SystemFunction from "../../../Containers/Lookup/SystemFunctionContainer";
import { TipologicaGridDto } from "../../../Model/LookUp/LookUpGenericModel";
import CriticalAssetType from "../../../Containers/Lookup/CriticalAssetTypeContainer";
import { GDR_ENUM } from "../../DesignComponentFamily/DesignComponentFamilyModal";
import CustomerWheel from "../../../Containers/Lookup/CustomerWheelContainer";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";

interface Props {
  vodafoneNameId?: number;
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
  productName?: string;
}

const SubNetworkBoundaryForm: React.FC<Props> = (props) => {
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    setInputValue,
    onChangeSelect,
    setChanged,
  } = useFormTableCrud<SubNetworkBoundaryGridDto>(
    CreatSubNetworkBoundary,
    EditSubNetworkBoundary
  );

  const { tipologicaPermesso } = useAuth();

  const dtoEditResourceState = (state: RootState) =>
    state.subNetworkBoundaryEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.subNetworkBoundaryCreateReducer.LookUpDtoCreate;

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [criticalCount, setCriticalCount] = useState<number>(0);

  const GridAll = (state: RootState) =>
    state.supportedServiceGridReducer.LookUpGridResultAll;
  const GridDtoAll = useSelector(GridAll);

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      onChangeCount("c3C4", editResource?.c3C4!);
      onChangeCount("internetFacing", editResource?.internetFacing!);
      onChangeCount("securityElement", editResource?.securityElement!);
      onChangeCount("pcisox", editResource?.pcisox!);
      onChangeCount("missionCritical", editResource?.missionCritical!);
      onChangeCount(
        "gdprClassification",
        editResource?.gdprClassification === 2 ? true : false
      );

      console.log("props.productName =>>", props?.productName);
      if (props?.productName) {
        editResource = {
          ...editResource,
          swApplicationTypes: {
            ...editResource?.swApplicationTypes,
            [props.productName]: props.productName,
          },
        };
      }
      setFormData(editResource);
    } else {
      if (props?.vodafoneNameId) {
        createResource = {
          ...createResource,
          vodafoneNameId: props.vodafoneNameId,
        };

        console.log("creae => ", createResource);
        GetSNewSubnetworkBoundaryDescription(props.vodafoneNameId).then(
          (res) => {
            setFormData({
              ...createResource,
              subNetworkBoundaryDescription: res?.data!,
            });
          }
        );
      } else {
        setFormData(createResource);
      }
    }
  }, [createResource, editResource, props.edit]);

  const onChangeSubNetworkBoundaryIds = (obj: any) => {
    const copy = { ...formData } as SubNetworkBoundaryGridDto;
    if (obj !== null && obj !== undefined)
      copy.supportedServicesIDs = obj.map((el) => el.id);
    else copy.supportedServicesIDs = [];
    setFormData(copy);
  };

  const validazioneClient = (copy: SubNetworkBoundaryGridDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.subNetworkBoundaryDescription === null ||
      copy?.subNetworkBoundaryDescription === undefined ||
      copy?.subNetworkBoundaryDescription.trim() === ""
    ) {
      addInvalidProperty("description");
    }

    if (
      copy.supportedServicesIDs === null ||
      copy.supportedServicesIDs === undefined ||
      copy.supportedServicesIDs.length === 0
    ) {
      addInvalidProperty("supportedServicesIDs");
    }

    if (
      copy.alias === null ||
      copy.alias === undefined ||
      copy?.alias.trim() === ""
    ) {
      addInvalidProperty("alias");
    }

    if (
      copy.vodafoneNameId === null ||
      copy.vodafoneNameId === undefined ||
      copy.vodafoneNameId === -1
    ) {
      addInvalidProperty("vodafoneNameId");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  const onHideFunc = () => {
    let dataCopy = [...(GridDtoAll?.items ?? [])];
    supportedServiceRefillData(dataCopy, "normal");
  };

  const onChangeSW = async (e: any) => {
    const copy = { ...formData } as SubNetworkBoundaryGridDto;
    if (e && e["key"]) {
      //copy.swApplicationName = value;
      copy.vodafoneNameId = e["key"];
      await GetSNewSubnetworkBoundaryDescription(e["key"]).then((res) => {
        copy.subNetworkBoundaryDescription = res?.data!;
      });
    } else {
      copy.vodafoneNameId = e["key"];
    }
    setFormData(copy);
  };

  const onChangeGDPR = (value: boolean | undefined) => {
    const copy = { ...formData } as SubNetworkBoundaryGridDto;
    copy.gdprRelevant = value;
    setFormData(copy);
  };

  const SystemFunctionRefillData = (value: TipologicaGridDto[] | undefined) => {
    let copy = { ...formData } as SubNetworkBoundaryGridDto;

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

  const CustomerWheelRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.customerWheelResource)
      formData.customerWheelResource = obj as {
        [key: string]: string;
      };
    setFormData(formData);
  };

  const returnLookup = () => {
    switch (isVisibleModalLookup) {
      case 1:
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

      case 2:
        return (
          <SupportedService
            returnObject={supportedServiceRefillData}
            modal={{
              isModal: true,
              setIsVisibleModalLookup: (e: number) =>
                setIsVisibleModalLookup(e),
            }}
          />
        );

      case 4:
        return (
          <CustomerWheel
            returnObject={CustomerWheelRefillData}
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

  const onChangeSystemFunctionsIds = (obj: any) => {
    const copy = { ...formData } as SubNetworkBoundaryGridDto;
    if (obj !== null && obj !== undefined)
      copy.systemFunctionsIds = obj.map((el) => el.key);
    else copy.systemFunctionsIds = [];
    setFormData(copy);
  };

  const onChangeLCMPolicy = (property: string, e: number) => {
    const copy = { ...formData } as SubNetworkBoundaryGridDto;
    copy[property] = e;
    setFormData(copy);
  };

  const onChangeCustomerWheel = (obj: any) => {
    const copy = { ...formData } as SubNetworkBoundaryGridDto;
    if (obj !== null && obj !== undefined)
      copy.customerWheelsIds = obj.map((el) => el.key);
    else copy.customerWheelsIds = [];
    setFormData(copy);
  };

  const onChangeCount = (type: string, e: boolean) => {
    let copy = { ...formData } as SubNetworkBoundaryGridDto;

    if (type === "internetFacing" && e !== copy.internetFacing && e) {
      setCriticalCount((prevState) => prevState + 1);
    } else if (
      type === "internetFacing" &&
      e !== copy.internetFacing &&
      !e &&
      criticalCount !== 0
    ) {
      setCriticalCount((prevState) => prevState - 1);
    } else if (type === "pcisox" && e !== copy.pcisox && e) {
      setCriticalCount((prevState) => prevState + 1);
    } else if (
      type === "pcisox" &&
      e !== copy.pcisox &&
      !e &&
      criticalCount !== 0
    ) {
      setCriticalCount((prevState) => prevState - 1);
    } else if (type === "securityElement" && e !== copy.securityElement && e) {
      setCriticalCount((prevState) => prevState + 1);
    } else if (
      type === "securityElement" &&
      e !== copy.securityElement &&
      !e &&
      criticalCount !== 0
    ) {
      setCriticalCount((prevState) => prevState - 1);
    } else if (type === "c3C4" && e !== copy.c3C4 && e) {
      setCriticalCount((prevState) => prevState + 1);
    } else if (
      type === "c3C4" &&
      e !== copy.c3C4 &&
      !e &&
      criticalCount !== 0
    ) {
      setCriticalCount((prevState) => prevState - 1);
    } else if (type === "missionCritical" && e !== copy.missionCritical && e) {
      setCriticalCount((prevState) => prevState + 1);
    } else if (
      type === "missionCritical" &&
      e !== copy.missionCritical &&
      !e &&
      criticalCount !== 0
    ) {
      setCriticalCount((prevState) => prevState - 1);
    } else if (
      type === "gdprClassification" &&
      e !== (copy.gdprClassification === 2 ? true : false) &&
      e
    ) {
      setCriticalCount((prevState) => prevState + 1);
    } else if (
      type === "gdprClassification" &&
      e !== (copy.gdprClassification === 2 ? true : false) &&
      !e &&
      criticalCount !== 0
    ) {
      setCriticalCount((prevState) => prevState - 1);
    } else {
      return;
    }
  };

  const supportedServiceRefillData = (value: Array<any>, type?: string) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );

    if (formData && formData?.supportedServices) {
      formData.supportedServices = dictionaryToArray(obj).map((item) => ({
        id: item.key,
        description: item.value,
      })) as any;
    }
    setFormData(formData);
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  return (
    <div className="col-12">
      <Dialog
        open={isVisibleModalLookup === 0 ? false : true}
        onClose={() => {
          onHideFunc();
          setIsVisibleModalLookup(0);
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogContent>
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <IconButton
              aria-label="close"
              onClick={() => {
                onHideFunc();
                setIsVisibleModalLookup(0);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {returnLookup()}
        </DialogContent>
      </Dialog>
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Vodafone Name<span className="red">*</span>
                <Select
                  menuPosition={"fixed"}
                  options={
                    formData?.vodafoneNAmesResource &&
                    dictionaryToArray(formData?.vodafoneNAmesResource)
                  }
                  value={
                    formData?.vodafoneNAmesResource &&
                    dictionaryToArray(formData?.vodafoneNAmesResource).filter(
                      (x) =>
                        x.key ===
                        (formData?.vodafoneNameId || props.vodafoneNameId)
                    )
                  }
                  onChange={(e) => {
                    onChangeSW(e);
                  }}
                  isDisabled={
                    props.edit || props?.vodafoneNameId ? true : false
                  }
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value.toUpperCase()}
                  onBlur={() => setInputValue("")}
                  getOptionValue={(option) => option.value}
                ></Select>
                {validation &&
                validation.response === false &&
                validation.property?.includes("vodafoneNameId") ? (
                  <label className="validation">
                    *Vodafone Name must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>
          <div className="col-12">
            <div className="row">
              <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold mb-0 w-100">
                    Subnetwork Boundary<span className="red">*</span>
                    <input
                      type="text"
                      onChange={(e) =>
                        onChange("subNetworkBoundaryDescription", e)
                      }
                      onKeyUp={(e) =>
                        onChange("subNetworkBoundaryDescription", e)
                      }
                      className="inputForm w-100"
                      readOnly={true}
                      defaultValue={formData?.subNetworkBoundaryDescription}
                    />
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("description") ? (
                      <label className="validation">
                        *Description is required
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="form-group">
                  <label className="labelForm voda-bold mb-0 w-100">
                    Alias <span className="red">*</span>
                    <input
                      type="text"
                      onChange={(e) => onChange("alias", e)}
                      onKeyUp={(e) => onChange("alias", e)}
                      className="inputForm w-100"
                      required
                      defaultValue={formData?.alias}
                    />
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("alias") ? (
                      <label className="validation">*Alias is required</label>
                    ) : null}
                  </label>
                </div>
              </div>
            </div>
          </div>

          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Supported Services<span className="red">*</span>
                <div className="d-flex">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={
                      formData && formData?.supportedServices
                        ? formData?.supportedServices
                        : undefined
                    }
                    value={
                      formData && formData?.supportedServices
                        ? formData?.supportedServices.filter((el) =>
                            formData?.supportedServicesIDs?.includes(
                              el.id ? el.id : 0
                            )
                          )
                        : undefined
                    }
                    onChange={onChangeSubNetworkBoundaryIds}
                    // onKeyUp={onChangeSubNetworkBoundaryIds}
                    //onBlur={() => setInputValue("")}
                    isMulti
                    isSearchable
                    isClearable
                    getOptionLabel={(option) => option.description!}
                    getOptionValue={(option) => option?.id?.toString() ?? "0"}
                  />
                  {tipologicaPermesso && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(2)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
                {validation &&
                validation.response === false &&
                validation.property?.includes("supportedServicesIDs") ? (
                  <label className="validation">
                    *Supported Service must have a value
                  </label>
                ) : null}
              </label>
            </div>
          </div>

          <div className="col-12 col-md-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                System Function
                <div className="d-flex">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={
                      formData?.systemFunctionsResource &&
                      dictionaryToArray(formData?.systemFunctionsResource)
                    }
                    value={
                      formData?.systemFunctionsResource &&
                      dictionaryToArray(
                        formData?.systemFunctionsResource
                      ).filter((el) =>
                        formData?.systemFunctionsIds?.includes(el.key)
                      )
                    }
                    onChange={(e) => onChangeSystemFunctionsIds(e)}
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
                      onClick={() => setIsVisibleModalLookup(1)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
              </label>
            </div>
          </div>

          <div className="col-12 col-md-6">
            <label className="labelForm voda-bold w-100 mb-0">
              Criticality
            </label>
            <div className="form-group">
              <input
                type="text"
                maxLength={1000}
                disabled={!tipologicaPermesso}
                className="inputForm w-100 voda-regular"
                onChange={(e) => onChange("criticality", e)}
                value={formData?.criticality}
              />
            </div>
          </div>

          <div className="col-12 col-md-6">
            <label className="labelForm voda-bold w-100 mb-0">
              Customer Wheel
            </label>
            <div className="form-group">
              <div className=" d-flex ">
                <Select
                  menuPosition={"fixed"}
                  className="w-100"
                  options={
                    formData?.customerWheelResource &&
                    dictionaryToArray(formData?.customerWheelResource)
                  }
                  value={
                    formData?.customerWheelResource &&
                    dictionaryToArray(formData?.customerWheelResource).filter(
                      (el) => formData?.customerWheelsIds?.includes(el.key)
                    )
                  }
                  onChange={(e) => onChangeCustomerWheel(e)}
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
                      src={require("../../../img/plus_icon.png")}
                      alt="plus"
                    />
                  </button>
                )}
              </div>
            </div>
          </div>

          <div className="col-12">
            <div className="row">
              <div className="col-12 col-md-6">
                <label className="labelForm voda-bold mb-1 w-100">
                  GDPR Relevant
                </label>
                <div className="form-group">
                  <div className="d-flex align-items-center flex-gab">
                    <Form.Check
                      type="radio"
                      onChange={(e) => onChangeGDPR(undefined)}
                      checked={
                        formData?.gdprRelevant === null ||
                        formData?.gdprRelevant === undefined
                          ? true
                          : false
                      }
                      label="Unspecified"
                    />
                    <Form.Check
                      type="radio"
                      onChange={(e) => onChangeGDPR(true)}
                      checked={formData?.gdprRelevant === true ? true : false}
                      label="Yes"
                    />
                    <Form.Check
                      type="radio"
                      onChange={(e) => onChangeGDPR(false)}
                      checked={formData?.gdprRelevant === false ? true : false}
                      label="No"
                    />
                  </div>
                </div>
              </div>

              <div className="col-12 col-md-6">
                <label className="labelForm voda-bold mb-1 w-100">
                  LCM Policy
                </label>
                <div className="form-group">
                  <div className="d-flex align-items-center flex-gab">
                    <Form.Check
                      type="radio"
                      className="mr-1"
                      name="LCMPolicy"
                      value="1"
                      onChange={(e) =>
                        onChangeLCMPolicy("lcmPolicy", +e.target.value)
                      }
                      checked={+formData?.lcmPolicy! === 1 ? true : false}
                      label="Telco"
                    />
                    <Form.Check
                      type="radio"
                      name="LCMPolicy"
                      value="0"
                      className="mr-1"
                      onChange={(e) =>
                        onChangeLCMPolicy("lcmPolicy", +e.target.value)
                      }
                      checked={+formData?.lcmPolicy! === 0 ? true : false}
                      label="IT"
                    />
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div className="col-12">
            <h1 className="text-bb mt-3">Criticality Classification</h1>
          </div>

          {/* <div className="col-12">
            <label className="labelForm voda-bold mb-0 w-100">
              Criticality Rating
            </label>
            <p className="mb-3">{criticalCount}</p>
          </div> */}

          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                GDPR Classification
                <div className="d-flex">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={GDR_ENUM && dictionaryToArray(GDR_ENUM)}
                    value={dictionaryToArray(GDR_ENUM).filter(
                      (el) => el.key === formData?.gdprClassification
                    )}
                    onChange={(e) => {
                      onChangeCount(
                        "gdprClassification",
                        e && e["key"] === 2 ? true : false
                      );
                      onChangeSelect("gdprClassification", e);
                    }}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                </div>
              </label>
            </div>
          </div>
          <div className="col-6 mb-2 d-flex flex-column">
            <div className="form-group">
              <label className="labelForm voda-bold   w-100 d-flex align-items-center mb-1">
                Internet Facing
              </label>
              <div className="d-flex align-items-center flex-gab">
                <Select
                  menuPosition={"fixed"}
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
                      (el.key === 1 ? true : false) === formData?.internetFacing
                  )}
                  onChange={(e) => {
                    onChangeSelect("internetFacing", e);
                    onChangeCount(
                      "internetFacing",
                      e && e["key"] === 1 ? true : false
                    );
                  }}
                  onBlur={() => setInputValue("")}
                  isSearchable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option.key.toString()}
                />
              </div>
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                PCI/SOX
                <div className="d-flex">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={[
                      { key: 1, value: "Yes" },
                      { key: 0, value: "No" },
                    ]}
                    value={[
                      { key: 1, value: "Yes" },
                      { key: 0, value: "No" },
                    ].filter(
                      (el) => (el.key === 1 ? true : false) === formData?.pcisox
                    )}
                    onChange={(e) => {
                      onChangeCount(
                        "pcisox",
                        e && e["key"] === 1 ? true : false
                      );
                      onChangeSelect("pcisox", e);
                    }}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                </div>
              </label>
            </div>
          </div>

          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Security Element
                <div className=" d-flex ">
                  <Select
                    menuPosition={"fixed"}
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
                        formData?.securityElement
                    )}
                    onChange={(e) => {
                      onChangeCount(
                        "securityElement",
                        e && e["key"] === 1 ? true : false
                      );
                      onChangeSelect("securityElement", e);
                    }}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                </div>
              </label>
            </div>
          </div>

          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                C3/C4
                <div className=" d-flex ">
                  <Select
                    menuPosition={"fixed"}
                    className="w-100"
                    options={[
                      { key: 1, value: "Yes" },
                      { key: 0, value: "No" },
                    ]}
                    value={[
                      { key: 1, value: "Yes" },
                      { key: 0, value: "No" },
                    ].filter(
                      (el) => (el.key === 1 ? true : false) === formData?.c3C4
                    )}
                    onChange={(e) => {
                      onChangeCount("c3C4", e && e["key"] === 1 ? true : false);
                      onChangeSelect("c3C4", e);
                    }}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                </div>
              </label>
            </div>
          </div>

          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Mission Critical
                <div className=" d-flex ">
                  <Select
                    menuPosition={"fixed"}
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
                        formData?.missionCritical
                    )}
                    onChange={(e) => {
                      onChangeCount(
                        "missionCritical",
                        e && e["key"] === 1 ? true : false
                      );
                      onChangeSelect("missionCritical", e);
                    }}
                    onBlur={() => setInputValue("")}
                    isSearchable
                    getOptionLabel={(option) => option.value}
                    getOptionValue={(option) => option.key.toString()}
                  />
                </div>
              </label>
            </div>
          </div>

          <div className="col-12">
            <div className="row">
              {props.edit === true ? (
                <div className="col-6">
                  <div className="form-group">
                    <label className="labelForm voda-bold   w-100">
                      Last Modified
                      <input
                        readOnly={true}
                        className="inputForm w-100 voda-regular"
                        type="text"
                        value={formatDateWithTime(
                          formData?.lastModified
                        )?.toUpperCase()}
                      />
                    </label>
                  </div>
                </div>
              ) : null}
              {props.edit === true ? (
                <div className="col-6">
                  <div className="form-group">
                    <label className="labelForm voda-bold   w-100">
                      Last Modified By
                      <input
                        readOnly={true}
                        className="inputForm w-100 voda-regular"
                        type="text"
                        value={formData?.lastModifiedBy}
                      />
                    </label>
                  </div>
                </div>
              ) : null}
            </div>
          </div>
        </div>
      </form>
      <div className="col-12 justify-content-end d-flex ">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => {
            Save(
              {
                ...formData,
                customerWheelResource: {},
                productNamesResource: {},
                systemFunctionsResource: {},
                vodafoneNAmesResource: {},
              },
              props.edit,
              validazioneClient,
              refresh
            );
          }}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default SubNetworkBoundaryForm;
