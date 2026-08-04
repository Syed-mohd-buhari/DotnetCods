import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { Modal, Form } from "react-bootstrap";
import {
  formatDateWithTime,
  AddMonth,
  numberIsNullOrZero,
  stringIsNullOrEmpty,
  subtractMonths,
  lowerFirstLetter,
  safeNumber,
} from "../../Hook/Common";
import { useSelector } from "react-redux";
import Select from "react-select";
import { BuildBagDtoCreate, BuildBagDtoUpdate } from "../../Model/BuildBag";
import {
  CreatBuildBag,
  GetBuildBagCreateResource,
} from "../../Redux/Action/BuildBag/BuildBagCreateAction";
import { EditBuildBag } from "../../Redux/Action/BuildBag/BuildBagEditAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import OriginalEquipmentManufacturer from "../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import OperatingSystemContainer from "../../Containers/Lookup/OperatingSystemContainer";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import Container from "../../Components/Container";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  dictionaryToArray,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import DatePicker from "react-datepicker";
import { TipologicaGridDto } from "../../Model/LookUp/LookUpGenericModel";
import { SubNetworkBoundaryGridDto } from "../../Model/LookUp/SubnetworkBoundry";
import CriticalAssetType from "../../Containers/Lookup/CriticalAssetTypeContainer";
import NetworkFunction from "../../Containers/Lookup/NetworkFunctionContainer";
import {
  DataModalConfirm,
  rtnConfirmMessage,
  stateConfirm,
} from "../../Model/Common";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions, TextField } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import {
  DropdownInputComponent,
  MultiSelectComponent,
  TextInputComponent,
} from "../../Components/FormField";
import Paper from "@mui/material/Paper";
import InputBase from "@mui/material/InputBase";
import Divider from "@mui/material/Divider";
import { useNavigate } from "react-router";
import ComponentSWBuild from "../../Containers/ComponentSWBuildContainer";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: BuildBagDtoCreate,
      property: string
    );
    wizardBackFunction?(formData: BuildBagDtoCreate, property: string): any;
    setConfirmExitWizard?(): any;
    setDataCheck?(prop: string, val: number | string): any;
  };
  // data: BuildBagDtoUpdate | BuildBagDtoCreate | undefined | null,
  edit: boolean;
  keyTab?: string;
  softwareRedirect?: boolean;
  lcmId?: number | null;
  dcfId?: number | null;
  opcoId?: number | null;
  prevPage?: string;
  fromLcm?: boolean;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: BuildBagDtoCreate;
  onGetSwType?(type: string): void;
}

const ComponentSwModal: React.FC<Props> = (props) => {
  const navigate = useNavigate();
  const [keyTabs, setKey] = useState("BuildBag");
  const [checkDeliveryMethod, setCheckDeliveryMethod] =
    useState<boolean>(false);
  const [existSoftwareVerionResource, setExistSoftwareVerionResource] =
    useState<any>();
  const [checkSWVersion, setCheckSWVersion] = useState<boolean>(true);
  const [datesGenerated, setDatesGenerated] = useState<boolean>(false);
  const [showFurtherDetails, setShowFurtherDetails] = useState<boolean>(false);
  const [vulnerabilityError, setVulnerabilityError] = useState<boolean>(false);
  const [descriptionError, setDescriptionError] = useState<boolean>(false);
  const [componentIds, setComponentIds] = useState<any>([]);
  const [dataConfirm, setDataConfirm] =
    useState<DataModalConfirm>(stateConfirm);
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeDate,
    onChangeSelect,
    onChangeMultipleSelect,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<BuildBagDtoUpdate>(CreatBuildBag, EditBuildBag);
  const dtoEditResourceState = (state: RootState) =>
    state.buildBagEditReducer.BuildBagDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.buildBagCreateReducer.BuildBagDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const [forceEdit, setforceEdit] = useState(false);
  const [disabledDate, setDisabledDate] = useState<boolean>(false);
  const [disabledEoSDate, setDisabledEoSDate] = useState<boolean>(false);
  const [warringFlag, setWarringFlag] = useState<boolean>(false);
  const [isComponentModalFlag, setIsComponentModalFlag] =
    useState<boolean>(false);
  const [lookupFlag, setLookupFlag] = useState<string>("");
  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const [editDescription, setEditDescription] = useState<any>([]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit || forceEdit) {
      setforceEdit(false);
      setEditDescription(editResource?.bagVersion);
      setComponentIds(
        editResource?.["componentBuildBagEditPageDto"]?.[
          "upgradeComponentSoftwareDetails"
        ]?.map((res: any) => {
          return res?.componentSoftwareBuildId;
        }) ?? []
      );
      setFormData({
        ...formData,
        buildBagId: editResource?.buildBagId,
        opcoResource: editResource?.opcoResource,
        dcfResource: editResource?.dcfResource,
        opCoId: props?.opcoId ?? editResource?.opCoId,
        designComponentFamilyId:
          props?.dcfId ?? editResource?.designComponentFamilyId,
        componentSofwareBuild:
          editResource?.["componentBuildBagEditPageDto"]?.[
            "existingComponentSwBuild"
          ],
        buildBagDescription: editResource?.buildBagDescription,
      });
    } else if (!props.wizardMode) {
      setFormData({
        ...createResource,
        opCoId: props?.opcoId ?? 0,
        designComponentFamilyId: props?.dcfId ?? 0,
      });
    }
  }, [createResource, editResource, props.edit, props.dataWizard]);

  useEffect(() => {
    if (formData?.opCoId && formData?.designComponentFamilyId) {
      const opcoName =
        formData?.opcoResource &&
        resourceArrayRefactor(formData?.opcoResource).find(
          (x) => x.key === formData?.opCoId
        )?.value;

      const dcfFullText =
        formData?.dcfResource &&
        resourceArrayRefactor(formData?.dcfResource).find(
          (x) => x.key === formData?.designComponentFamilyId
        )?.value;

      if (opcoName && dcfFullText) {
        const dcfExtracted = extractDcfName(dcfFullText);
        const combinedName = `${opcoName} ${dcfExtracted}`;

        setFormData({
          ...formData,
          buildBagDescription: combinedName,
        });
      }
    }
  }, [formData?.opCoId, formData?.designComponentFamilyId]);
  // useEffect(() => {
  //   if (
  //     formData &&
  //     formData?.productName &&
  //     props.wizardMode &&
  //     !stringIsNullOrEmpty(formData?.productName)
  //   ) {
  //     props.action.setDataCheck &&
  //       props.action.setDataCheck("productName", formData?.productName);

  //     if (props.onGetSwType) {
  //       props.onGetSwType(formData?.productName);
  //     }
  //   }
  // }, [formData?.productName]);
  const { tipologicaPermesso } = useAuth();

  const validazioneClient = (copy: BuildBagDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      !props?.edit &&
      (copy?.opCoId == null || copy?.opCoId === undefined || copy?.opCoId === 0)
    ) {
      addInvalidProperty("opCoId");
    }
    if (
      !props?.edit &&
      (copy?.designComponentFamilyId == null ||
        copy?.designComponentFamilyId === undefined ||
        copy.designComponentFamilyId === 0)
    ) {
      addInvalidProperty("designComponentFamilyId");
    }
    if (
      copy?.buildBagDescription == null ||
      copy?.buildBagDescription === undefined ||
      copy?.buildBagDescription.trim() === ""
    ) {
      addInvalidProperty("buildBagDescription");
    }

    setValidation(copyValidation);
    return copyValidation;
  };
  const changeSwApplicationName = (property: string, e: any) => {
    let copy = { ...formData } as BuildBagDtoUpdate;

    if (e && e["value"]) {
      copy.designComponentFamilyId = e["key"];
    } else {
      copy.designComponentFamilyId = undefined;
    }
    setFormData(copy);
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [isVisibleFurtherDetails, setIsVisibleFurtherDetails] =
    useState<boolean>(false);

  const onChangeCheckDeliveryMethod = (e: any) => {
    let checked = e.target.checked;
    setCheckDeliveryMethod(checked);
    if (!checked) {
      let copy = { ...formData } as BuildBagDtoUpdate;
      setDatesGenerated(false);
      setDisabledEoSDate(false);
      setFormData(copy);
    }
  };
  const RestoreOrphanDeleted = async (
    id: number | undefined,
    orphanDeletedValue?: boolean
  ) => {
    setOrphanDeleted(orphanDeletedValue ?? true);
    setforceEdit(true);
    if (props.action.Edit) await props.action.Edit(id);
    if (orphanDeletedValue === false) {
      // setDisableForm(true);
      setChanged(false);
    }
  };
  const validateWizard = () => {
    let copy = { ...formData } as BuildBagDtoCreate;
    props.action.validateFormWizard &&
      props.action.validateFormWizard(
        validazioneClient(copy).response,
        copy,
        "buildBagDto"
      );
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as BuildBagDtoUpdate;
    if (e !== null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = null;
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const handleMultiSelect = (selected) => {
    setComponentIds(selected.map((val) => val.value));
  };

  const stripHtmlTags = (str) =>
    str
      ?.replace(/<[^>]*>/g, "") // Remove HTML tags
      ?.replace(/\s+/g, " ") // Collapse multiple spaces into one
      ?.trim() || // Remove leading/trailing whitespace
    "";

  const extractDcfName = (dcfText) => {
    if (!dcfText) return "";

    const cleanText = dcfText.replace(/<[^>]*>/g, "").trim();

    const match = cleanText.match(/^(.*?)\s+on\s+/i);
    if (match && match[1]) {
      const textBeforeOn = match[1].trim();
      const words = textBeforeOn.split(/\s+/);
      return words[words.length - 1];
    }

    const words = cleanText.split(/\s+/);
    return words[words.length - 1];
  };

  const getBagDescription = () => {
    const oemValue = editDescription?.length
      ? editDescription[0]
      : formData?.originalEquipmentManufacturerResource &&
        resourceArrayRefactor(
          formData?.originalEquipmentManufacturerResource
        )?.filter((x) => x.key == formData?.originalEquipmentManufacturerId)[0]
          ?.value;

    const productValue = editDescription?.length
      ? editDescription[1]
      : formData?.productNamesResource &&
        resourceArrayRefactor(formData?.productNamesResource).filter(
          (x) => x.key == formData?.productNameId
        )[0]?.value;

    const versionValue = editDescription?.length ? editDescription[3] : "1.0";

    return `${formData?.buildBagDescription}`;
  };

  const SoftwareComponentRefillData = async (value: Array<any>) => {
    try {
      const res: any = await GetBuildBagCreateResource({
        isRefillData: true,
      });
      if (formData) {
        var obj: any = res?.componentSofwareBuild
          ? res?.componentSofwareBuild
          : [];

        setFormData({
          ...formData,
          componentSofwareBuild: obj,
        });
      }
    } catch (error) {
      console.error("Error in SoftwareComponentRefillData:", error);
    }
  };
  return (
    <div className="col-12">
      <ModalConfirm data={dataConfirm} />
      <ModalConfirm data={confirmForm} />{" "}
      <Dialog
        open={isComponentModalFlag}
        onClose={(event, reason) => {
          if (reason === "backdropClick" || reason === "escapeKeyDown") {
            return;
          } else {
            setIsComponentModalFlag(false);
          }
        }}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="xl"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        {/* <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>Upgrade Software Component</h4>
          </div>
        </DialogTitle> */}
        <IconButton
          aria-label="close"
          onClick={() => setIsComponentModalFlag(false)}
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
          {" "}
          <ComponentSWBuild
            redirect={"bagScreen"}
            returnObject={SoftwareComponentRefillData}
            modal={{ isModal: true, setIsComponentModalFlag }}
          />
        </DialogContent>
      </Dialog>
      <form id="formSoftwareBuild" onChange={() => setChanged(true)}>
        <div className="row">
          <fieldset className="fieldset">
            <label className="text-bb">Bag Details</label>
            <div className="row">
              <>
                <div className="form-group col-6 mb-0">
                  <div className="col-12 pl-0">
                    <DropdownInputComponent
                      label={"OpCo"}
                      labelCSS="mb-0"
                      inputCSS="labelForm voda-bold mb-0"
                      isSearchable={true}
                      isClearable={true}
                      isAdd={false}
                      required={true}
                      disabled={props.edit || props.fromLcm}
                      isError={
                        validation &&
                        validation.response == false &&
                        validation.property?.includes("opCoId")
                          ? true
                          : false
                      }
                      error={"*OpCo must have a value"}
                      value={
                        formData?.opcoResource &&
                        resourceArrayRefactor(formData?.opcoResource).filter(
                          (x) => x.key == formData?.opCoId
                        )
                      }
                      options={
                        formData?.opcoResource &&
                        resourceArrayRefactor(formData?.opcoResource)
                      }
                      onChange={(e: any) => onChangeSelect("opCoId", e)}
                    />
                  </div>
                </div>
                <div className="form-group col-6 pr-0 mb-0">
                  <div className="col-12">
                    <DropdownInputComponent
                      label={"DCF Name"}
                      labelCSS="mb-0"
                      inputCSS="labelForm voda-bold mb-0"
                      isSearchable={true}
                      isClearable={true}
                      isAdd={false}
                      disabled={props.edit || props.fromLcm}
                      required={true}
                      isError={
                        validation &&
                        validation.response == false &&
                        validation.property?.includes("designComponentFamilyId")
                          ? true
                          : false
                      }
                      error={"*Dcf must have a value"}
                      value={
                        formData?.dcfResource &&
                        resourceArrayRefactor(formData?.dcfResource).filter(
                          (x) => x.key == formData?.designComponentFamilyId
                        )
                      }
                      options={
                        formData?.dcfResource &&
                        resourceArrayRefactor(formData?.dcfResource)
                      }
                      onChange={(e: any) =>
                        changeSwApplicationName("designComponentFamilyId", e)
                      }
                    />
                  </div>
                </div>
              </>

              <div className="form-group col-12">
                <div className="col-6 pl-0">
                  <label className={"labelForm  voda-bold w-100 mb-1"}>
                    {"Bag Name"}
                    <span className="red">*</span>
                  </label>
                  <label className={`labelForm voda-bold w-100 `}>
                    <div
                      style={{
                        display: "flex",
                        alignItems: "center",
                        border: "1px solid #ccc",
                        borderRadius: "4px",
                      }}
                    >
                      <input
                        type="text"
                        className={`inputForm w-100 mt-0`}
                        disabled={true}
                        onChange={(e) => {
                          const value = e.target.value;
                          setWarringFlag(false);
                          onChange("buildBagDescription", e);
                        }}
                        value={formData?.buildBagDescription ?? ""}
                        style={{
                          border: "none",
                          outline: "none",
                          width: "6rem",
                          flex: 1,
                          height: "2.8rem",
                          borderRadius: 0,
                          background: `${
                            (props.lcmId === null && props.fromLcm) ||
                            (formData?.opCoId &&
                              formData?.designComponentFamilyId &&
                              !props.edit)
                              ? "#eee"
                              : "white"
                          }`,
                        }}
                      />
                      <span
                        style={{
                          padding: "10.5px 8px",
                          minWidth: "2rem",
                          textAlign: "center",
                          height: "2.8rem",
                          background: "#eee",
                          color: "#6c757d",
                        }}
                      >
                        {editDescription !== "" && props.edit
                          ? editDescription
                          : "1.0"}
                      </span>
                    </div>
                    <div className="w-100">
                      {warringFlag === true ? (
                        <label className="validation">
                          *Special characters are not allowed.
                        </label>
                      ) : validation &&
                        validation.response === false &&
                        validation.property?.includes("buildBagDescription") ? (
                        <label className="validation">
                          *Bag Name must have value
                        </label>
                      ) : null}
                    </div>
                  </label>
                </div>
              </div>
            </div>
          </fieldset>
        </div>
        <div className="row">
          <fieldset className="fieldset">
            <label className="text-bb">Manage Bag Contents</label>
            <div className="row">
              <div className="form-group col-6">
                <div className="col-12 pl-0">
                  <MultiSelectComponent
                    label={"Select Software Components"}
                    labelCSS="mb-0"
                    inputCSS="labelForm voda-bold mb-2"
                    required={false}
                    isAdd={tipologicaPermesso}
                    onAddClicked={() => setIsComponentModalFlag(true)}
                    value={
                      (componentIds &&
                        formData?.componentSofwareBuild &&
                        formData?.componentSofwareBuild
                          ?.map((comp) => {
                            return {
                              key: comp?.componentSoftwareBuildId,
                              value: comp?.displayDescription,
                            };
                          })
                          .filter((item) => componentIds?.includes(item.key))
                          .map((item) => ({
                            label: item.value,
                            value: item.key,
                          }))) ??
                      []
                    }
                    options={
                      formData?.componentSofwareBuild
                        ? formData?.componentSofwareBuild
                            ?.map((comp) => {
                              return {
                                key: comp?.componentSoftwareBuildId,
                                value: comp?.displayDescription,
                              };
                            })
                            .map((item) => ({
                              label: item.value,
                              value: item.key,
                            }))
                        : []
                    }
                    onChange={(e: any) => handleMultiSelect(e)}
                  />
                  {/* <DropdownInputComponent
                    label={"Select Component Bags"}
                    labelCSS="mb-0"
                    inputCSS="labelForm voda-bold mb-0"
                    isSearchable={true}
                    isClearable={true}
                    value={
                      formData?.componentSofwareBuild &&
                      formData?.componentSofwareBuild
                        ?.map((comp) => {
                          return {
                            key: comp?.componentSoftwareBuildId,
                            value: comp?.displayDescription,
                          };
                        })
                        .filter(
                          (x) => x.key == formData?.componentSoftwareBuildId
                        )
                    }
                    options={
                      formData?.componentSofwareBuild &&
                      formData?.componentSofwareBuild?.map((comp) => {
                        return {
                          key: comp?.componentSoftwareBuildId,
                          value: comp?.displayDescription,
                        };
                      })
                    }
                    onChange={(e: any) =>
                      onChangeSelect("componentSoftwareBuildId", e)
                    }
                  /> */}
                </div>
              </div>
            </div>
            <div className="row col-12">
              <legend className="voda-bold mb-4 fz-18">
                Software Component Details
              </legend>
              <div className="row col-12 mb-4 pb-3">
                <table className="w-80 ml-5">
                  <thead>
                    <tr className="mt-4 head">
                      {/* <th className="pl-2 ptb-12">Bag Name</th> */}
                      <th className="pl-2 ptb-12">Software Component</th>
                      <th className="pl-2 ptb-12"></th>
                    </tr>
                  </thead>
                  <tbody>
                    {formData?.componentSofwareBuild
                      ?.filter((comp) =>
                        componentIds.includes(comp?.componentSoftwareBuildId)
                      )
                      ?.map((item, i) => (
                        <tr
                          className={`dati ${
                            numberIsNullOrZero(item.componentSoftwareBuildId)
                              ? ""
                              : ""
                          }`}
                          key={i}
                        >
                          {/* <td>{getBagDescription()}</td> */}
                          <td>{item.displayDescription}</td>
                          <td>
                            <img
                              onClick={() => {
                                // console.log(item);
                                setComponentIds(
                                  componentIds?.filter(
                                    (i) => i !== item.componentSoftwareBuildId
                                  )
                                );
                              }}
                              className="btnEdit op-55"
                              src={require("../../img/delete.png")}
                            />
                          </td>
                        </tr>
                      ))}
                  </tbody>
                </table>
              </div>
            </div>
          </fieldset>
        </div>
      </form>
      <Container show={!props.wizardMode}>
        <div className="col-12 justify-content-end d-flex footerModal mt-5">
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
              props.prevPage === "generatelcmdb" &&
              props.softwareRedirect === true
                ? "disabledCursor"
                : ""
            }`}
            onClick={() => {
              Save(
                {
                  ...formData,
                  ...(props.lcmId !== null && {
                    lcmEngineeringId: props.lcmId,
                  }),
                  ...(props.dcfId !== null && {
                    designComponentFamilyId: props.dcfId,
                  }),
                  ...(props.opcoId !== null && {
                    opCoId: props.opcoId,
                  }),
                  buildBagDescription: formData?.buildBagDescription,
                  componentSoftwareId: componentIds,
                  bagVersion: props.edit ? editDescription : "1",
                },
                props.edit,
                validazioneClient,
                refresh,
                RestoreOrphanDeleted,
                orphanDeleted
              );
            }}
            type="button"
            data-toggle="tooltip"
            data-placement="top"
            title={
              props.prevPage === "generatelcmdb" &&
              props.softwareRedirect === true
                ? `Saving is disabled due to redirection from LCM Export screen`
                : ""
            }
            disabled={
              props.prevPage === "generatelcmdb" &&
              props.softwareRedirect === true
                ? true
                : false
            }
          >
            Submit
          </button>
        </div>
      </Container>
      <Container show={props.wizardMode}>
        <div className="col-12 d-flex justify-content-between py-4 mt-4">
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
                props.action.wizardBackFunction(formData, "buildBagDto")
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

export default ComponentSwModal;
