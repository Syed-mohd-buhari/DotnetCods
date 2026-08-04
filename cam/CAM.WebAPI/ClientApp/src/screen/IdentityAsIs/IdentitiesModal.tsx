import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { Modal } from "react-bootstrap";
import { numberIsNullOrZero } from "../../Hook/Common";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { useSelector } from "react-redux";
import Select from "react-select";
import { IdentityAsIsDtoUpdate } from "../../Model/LookUp/Identities";
import {
  CreatIdentityAsIs,
  GetIdentityAsIsResourceKey,
} from "../../Redux/Action/IdentityAsIs/IdentityAsIsCreateAction";
import { EditIdentityAsIs } from "../../Redux/Action/IdentityAsIs/IdentityAsIsEditAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import HardwareSolutionResourceContainer from "../../Containers/Lookup/HardwareSolutionResourceContainer";
import { useAuth } from "../../Hook/useAuth";
import BuildConstructionContainer from "../../Containers/Lookup/BuildConstructionContainer";
import ModalConfirm from "../../Components/ModalConfirm";
import { GetRuleFromBuildCostruction } from "../../Redux/Action/LookUp/BuildConstruction/BuildConstructionCommonAction";
import Container from "../../Components/Container";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import SharedLookUp from "../../Containers/Lookup/SharedLookUpContainer";
import ClassContainer from "../../Containers/Lookup/ClassContainer";
import TypeContainer from "../../Containers/Lookup/TypeContainer";
import { GetAssetsByOpcoIdAndDcfId } from "../../Redux/Action/IdentityAsIs/IdentityAsIsCommonAction";
import { GetClasssesByCategoryId } from "../../Redux/Action/LookUp/Class/ClassCreateAction";
import { GetTypesByClassId } from "../../Redux/Action/LookUp/Type/TypeCreateAction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: IdentityAsIsDtoUpdate,
      property: string
    );
    wizardBackFunction?(formData: IdentityAsIsDtoUpdate, property: string): any;
    setConfirmExitWizard?(): any;
    setDataCheck?(prop: string, val: number | string): any;
  };
  edit: boolean;
  keyTab?: string;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: IdentityAsIsDtoUpdate;
}

const IdentitiesModal: React.FC<Props> = (props) => {
  const { tipologicaPermesso, VerifyIsInRole } = useAuth();
  const [keyTabs, setKey] = useState("IdentityAsIs");
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
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<IdentityAsIsDtoUpdate>(
    CreatIdentityAsIs,
    // GetIdentityAsIsCreateResource,
    EditIdentityAsIs
  );
  const dtoEditResourceState = (state: RootState) =>
    state.IdentityAsIsEditReducer.IdentityAsIsDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.IdentityAsIsCreateReducer.IdentityAsIsDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const [forceEdit, setforceEdit] = useState(false);
  const [disabledDate, setDisabledDate] = useState<boolean>(false);
  const [disableWhat, setDisableWhat] = useState<boolean>(true);
  const [disablePlatform, setDisablePlatform] = useState<boolean>(true);
  const [disableHwType, setDisableHwType] = useState<boolean>(true);
  const [required, setRequired] = useState<boolean>(true);
  const [showFurtherDetails, setShowFurtherDetails] = useState<boolean>(false);
  const [toggleResource, setToggleResource] = useState<boolean>(false);
  const [editResourceKey, setEditResourceKey] = useState<boolean>(false);
  const [editPrevResourceKey, setEditPrevResourceKey] =
    useState<boolean>(false);
  const [resourceKeyError, setResourceKeyError] = useState<boolean>(false);
  const [prevResourceKeyError, setPrevResourceKeyError] =
    useState<boolean>(false);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("IdentityAsIs");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  useEffect(() => {
    if (props.edit || forceEdit) {
      setforceEdit(false);
      let newEditResource: any = { ...editResource };
      newEditResource.interfaceType =
        newEditResource.interfaceTypes &&
        dictionaryToArray(newEditResource.interfaceTypes).filter(
          (x) => x.value == newEditResource.interfaceType
        )[0]?.key;
      setFormData(newEditResource);
    } else {
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: IdentityAsIsDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.opcoId == null ||
      copy?.opcoId === undefined ||
      copy?.opcoId === 0
    ) {
      addInvalidProperty("opcoId");
    }
    if (copy?.value == "" || copy?.value == undefined) {
      addInvalidProperty("value");
    }
    if (copy?.dcfId == null || copy?.dcfId === undefined || copy?.dcfId === 0) {
      addInvalidProperty("dcfId");
    }
    if (
      copy?.assetId == null ||
      copy?.assetId === undefined ||
      copy?.assetId === 0
    ) {
      addInvalidProperty("assetId");
    }

    if (
      copy?.categoryId == null ||
      copy?.categoryId === undefined ||
      copy?.categoryId === 0
    ) {
      addInvalidProperty("categoryId");
    }

    if (copy?.categoryId === 1) {
      if (
        copy?.interfaceName == null ||
        copy?.interfaceName === undefined ||
        copy?.interfaceName === ""
      ) {
        addInvalidProperty("interfaceName");
      }
      if (
        copy?.interfaceType == null ||
        copy?.interfaceType === undefined ||
        copy?.interfaceType === 0
      ) {
        addInvalidProperty("interfaceType");
      }
    }

    if (
      props?.edit &&
      (resourceKeyError === true ||
        copy?.resourceKey === null ||
        copy?.resourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("resourceKey");
    }
    if (
      props?.edit &&
      formData?.previousResourceKey !== null &&
      (prevResourceKeyError === true ||
        copy?.previousResourceKey === null ||
        copy?.previousResourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("previousResourceKey");
    }
    setValidation(copyValidation);
    return copyValidation;

    // return { response: true }
  };

  const [validazioneCustom, setValidazioneCustom] = useState<
    { response: boolean; property: string } | undefined
  >();

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const categoriesRefillData = (data: any) => {
    var obj = data.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.categoryResource)
      formData.categoryResource = obj as {
        [key: string]: string;
      };
    if (formData?.interfaceTypes) {
      formData.interfaceTypes = obj as {
        [key: string]: string;
      };
    }
    setFormData(formData);
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <SharedLookUp
            returnObject={categoriesRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            apiType="Category"
          />
        );
      case 2:
        return (
          <ClassContainer
            returnObject={ClassRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></ClassContainer>
        );
      case 3:
        return (
          <TypeContainer
            returnObject={TypeResourceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></TypeContainer>
        );

      default:
        return;
    }
  };
  useEffect(() => {
    if (formData?.opcoId && formData.dcfId && !props.edit) {
      let copy = { ...formData } as IdentityAsIsDtoUpdate;
      GetAssetsByOpcoIdAndDcfId(formData?.opcoId!, formData?.dcfId!).then(
        (res) => {
          copy.assetResource = res?.data;
          setFormData(copy);
        }
      );
    }
  }, [formData?.opcoId, formData?.dcfId]);

  // Logic code get the resource key using api call

  // useEffect(() => {
  //   if (formData && formData?.assetId && !props?.edit) {
  //     let copy = { ...formData } as IdentityAsIsDtoUpdate;
  //     const selectResource = {
  //       assetId: formData?.assetId.toString(),
  //     };
  //     GetIdentityAsIsResourceKey(selectResource).then((x) => {
  //       if (x) {
  //         setFormData({ ...copy, resourceKey: x?.data });
  //       }
  //     });
  //   }
  // }, [formData?.assetId]);

  const OriginalEquipmentManufacturerRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    // if (formData && formData?.originalEquipmentManufacturerResource)
    //   formData.originalEquipmentManufacturerResource = obj as {
    //     [key: string]: string;
    //   };
    setFormData(formData);
  };

  const ClassRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    let copy = { ...formData } as IdentityAsIsDtoUpdate;
    GetClasssesByCategoryId(formData?.categoryId!).then((res) => {
      copy.classResource = res?.data;
      setFormData(copy);
    });
    //   if (formData && formData?.classResource)
    //     // formData.classResource = obj as { [key: string]: string };
    //   let copy = { ...formData } as IdentityAsIsDtoUpdate;
    //     GetClasssesByCategoryId(formData?.categoryId!).then((res)=>{
    //       copy.classResource=res?.data
    //       setFormData(copy)
    //     })
    //   // setFormData(formData);
  };

  const TypeResourceRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    //   if (formData && formData?.typeResource)
    //     formData.typeResource = obj as { [key: string]: string };
    //   setFormData(formData);
    let copy = { ...formData } as IdentityAsIsDtoUpdate;

    GetTypesByClassId(formData?.classId!).then((res) => {
      copy.typeResource = res?.data;
      setFormData(copy);
    });
  };

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const [isVisibleFurtherDetails, setIsVisibleFurtherDetails] =
    useState<boolean>(false);

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
  useEffect(() => {
    if (formData?.categoryId) {
      let copy = { ...formData } as IdentityAsIsDtoUpdate;

      GetClasssesByCategoryId(formData?.categoryId!).then((res) => {
        copy.classResource = res?.data;
        setFormData(copy);
      });
    }
  }, [formData?.categoryId]);
  useEffect(() => {
    if (formData?.classId) {
      let copy = { ...formData } as IdentityAsIsDtoUpdate;

      GetTypesByClassId(formData?.classId!).then((res) => {
        copy.typeResource = res?.data;
        setFormData(copy);
      });
    }
  }, [formData?.classId]);

  const validateResourceKeys = (key, event) => {
    if (key === "resourceKey") {
      onChange("resourceKey", event);
      if (/^4[0-9A-Za-z]{7}$/.test(event.target.value))
        setResourceKeyError(false);
      else setResourceKeyError(true);
    }
    if (key === "previousResourceKey") {
      onChange("previousResourceKey", event);
      if (event.target.value.length <= 255) setPrevResourceKeyError(false);
      else setPrevResourceKeyError(true);
    }
  };

  //NUOVE REGOLE
  const [proprietaryHardware, setProprietaryHardware] =
    useState<boolean>(false);

  const [visualizeHardware, setVirsualizeHardware] = useState<boolean>(false);
  const [cotsOrOther, setCotsOrOther] = useState<boolean>(false);
  const [noRules, setNoRules] = useState<boolean>(false);

  const [autoFilled, setAutoFilled] = useState<boolean>(false);

  const onChangeBuildConstruction = () => {};

  return (
    <div className="mt-2 col-12">
      <ModalConfirm data={confirmForm} />
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => setIsVisibleModalLookup(0)}
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
                setIsVisibleModalLookup(0);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {ReturnLookupContainer(isVisibleModalLookup)}
        </DialogContent>
      </Dialog>
      <form id="formHardwareBuild" onChange={() => setChanged(true)}>
        <div>
          <fieldset className="fieldset">
            <div className="row">
              <div className="col-6">
                <div className="col-12 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    OPCO <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.opCoResource &&
                            dictionaryToArray(formData?.opCoResource).sort(
                              (a, b) =>
                                a.value.toLowerCase() < b.value.toLowerCase()
                                  ? -1
                                  : 1
                            )
                          }
                          placeholder="Select OPCO "
                          value={
                            formData?.opCoResource != undefined
                              ? formData?.opCoResource &&
                                dictionaryToArray(
                                  formData?.opCoResource
                                ).filter((x) => x.key == formData?.opcoId)
                              : null
                          }
                          onChange={(e) => onChangeSelect("opcoId", e)}
                          isDisabled={props.edit}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("opcoId") ? (
                      <label className="validation">
                        *OPCO must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="col-12 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    Design Component Family <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.dcfResource &&
                            dictionaryToArray(formData?.dcfResource).sort(
                              (a, b) =>
                                a?.value?.toLowerCase() <
                                b?.value?.toLowerCase()
                                  ? -1
                                  : 1
                            )
                          }
                          placeholder="Select  Design Component Family  "
                          value={
                            formData?.dcfResource != undefined
                              ? formData?.dcfResource &&
                                dictionaryToArray(formData?.dcfResource).filter(
                                  (x) => x.key == formData?.dcfId
                                )
                              : null
                          }
                          onChange={(e) => onChangeSelect("dcfId", e)}
                          isDisabled={props.edit}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                          formatOptionLabel={function (data) {
                            return (
                              <span
                                dangerouslySetInnerHTML={{
                                  __html: data.value,
                                }}
                              />
                            );
                          }}
                        ></Select>
                      </div>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("dcfId") ? (
                      <label className="validation">
                        *product Name must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="col-12 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    Assets<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.assetResource &&
                            dictionaryToArray(formData?.assetResource).sort(
                              (a, b) =>
                                a.value.toLowerCase() < b.value.toLowerCase()
                                  ? -1
                                  : 1
                            )
                          }
                          placeholder="Select Assets"
                          value={
                            formData?.assetResource &&
                            dictionaryToArray(formData?.assetResource).filter(
                              (x) => x.key == formData?.assetId
                            )
                          }
                          onChange={(e) => onChangeSelect("assetId", e)}
                          isDisabled={props.edit}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("assetId") ? (
                      <label className="validation">
                        *Assets must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="col-12 pr-0 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    Category<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.categoryResource &&
                            dictionaryToArray(formData?.categoryResource)
                          }
                          value={
                            formData?.categoryResource &&
                            dictionaryToArray(
                              formData?.categoryResource
                            ).filter((x) => x.key == formData?.categoryId)
                          }
                          onChange={(e) => {
                            onChangeSelect("categoryId", e);
                          }}
                          placeholder="Select Category"
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => setIsVisibleModalLookup(1)}
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
                    validation.response == false &&
                    validation.property?.includes("categoryId") ? (
                      <label className="validation">
                        *Category must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              {formData?.categoryId === 1 && (
                <div className="col-6">
                  <div className="col-12 pr-0 pl-0">
                    <label className="labelForm voda-bold w-100 pr-2">
                      Interface Name <span className="red">*</span>
                      <input
                        type="text"
                        onChange={(e) => onChange("interfaceName", e)}
                        onKeyUp={(e) => onChange("interfaceName", e)}
                        className="inputForm w-100"
                        value={formData?.interfaceName ?? ""}
                      />
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("interfaceName") ? (
                        <label className="validation">
                          *Interface Name must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
              )}
              {formData?.categoryId === 1 && (
                <div className="col-6">
                  <div className="col-12 pr-0 pl-0">
                    <label className="labelForm voda-bold w-100 mb-20">
                      Interface Type<span className="red">*</span>
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={
                              formData?.interfaceTypes &&
                              dictionaryToArray(formData?.interfaceTypes)
                            }
                            value={
                              formData?.interfaceTypes &&
                              dictionaryToArray(
                                formData?.interfaceTypes
                              ).filter((x) => x.key == formData?.interfaceType)
                            }
                            onChange={(e) => {
                              onChangeSelect("interfaceType", e);
                            }}
                            placeholder="Select Interface Type"
                            isSearchable
                            isClearable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          ></Select>
                        </div>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("interfaceType") ? (
                        <label className="validation">
                          *Interface type must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
              )}
              <div className="col-6">
                <div className="col-12 pr-0 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    Class
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.classResource &&
                            dictionaryToArray(formData?.classResource)
                          }
                          value={
                            formData?.classResource &&
                            dictionaryToArray(formData?.classResource).filter(
                              (x) => x.key == formData?.classId
                            )
                          }
                          onChange={(e) => onChangeSelect("classId", e)}
                          placeholder="Select Class"
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => setIsVisibleModalLookup(2)}
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
                    validation.response == false &&
                    validation.property?.includes("buildConstructionId") ? (
                      <label className="validation">
                        *build Construction must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="col-12 pr-0 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    Type
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.typeResource &&
                            dictionaryToArray(formData?.typeResource)
                          }
                          value={
                            formData?.typeResource &&
                            dictionaryToArray(formData?.typeResource).filter(
                              (x) => x.key == formData?.typeId
                            )
                          }
                          onChange={(e) => onChangeSelect("typeId", e)}
                          placeholder="Select Type"
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => setIsVisibleModalLookup(3)}
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
                  </label>
                </div>
              </div>
            </div>
          </fieldset>
          {/* CLASSIC */}
          <Container show={true}>
            <fieldset className="fieldset">
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12">
                    <label className="labelForm voda-bold w-100 pr-2">
                      Value <span className="red">*</span>
                      <input
                        type="text"
                        onChange={(e) => onChange("value", e)}
                        onKeyUp={(e) => onChange("value", e)}
                        className="inputForm w-100"
                        value={formData?.value ?? ""}
                      />
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("value") ? (
                        <label className="validation">
                          *Value must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
              </div>

              {props?.edit && (
                <>
                  <div className="row">
                    <div className="col-12 pl-0">
                      <div className="col-12">
                        <label className="labelForm voda-bold">
                          View Resource Keys
                        </label>
                        <label className="labelForm voda-bold">
                          <div className="switchSmall ml-2">
                            <input
                              type="checkbox"
                              onChange={(e) => {
                                setToggleResource(e.target.checked);
                              }}
                              className="mr-1"
                              checked={toggleResource}
                            />
                            <span className="sliderSmall round"></span>
                          </div>
                        </label>
                      </div>
                    </div>
                  </div>
                  {toggleResource && (
                    <div className="row">
                      <div className="col-6 px-0">
                        <div className="col-12">
                          <div className="form-group">
                            <label className="labelForm voda-bold mb-0">
                              Resource Key
                            </label>
                            <div className="d-flex">
                              <label className="labelForm voda-bold mb-0 w-90">
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    validateResourceKeys("resourceKey", e)
                                  }
                                  onKeyUp={(e) =>
                                    validateResourceKeys("resourceKey", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.resourceKey!}
                                  disabled={!editResourceKey && props.edit}
                                />
                              </label>
                              {props.edit && (
                                <button
                                  type="button"
                                  title={"Edit Resource Key"}
                                  className="btn btn-link"
                                  onClick={() =>
                                    setEditResourceKey(!editResourceKey)
                                  }
                                >
                                  <img
                                    className="btnEdit op-55"
                                    src={require("../../img/edit.png")}
                                  />
                                </button>
                              )}
                            </div>

                            {resourceKeyError === true ||
                            (validation &&
                              validation.response == false &&
                              validation.property?.includes("resourceKey")) ? (
                              <label className="validationFeild h-16 mb-1 w-100">
                                Resource Key must start with '4', followed by 7
                                alphanumeric characters.
                              </label>
                            ) : null}
                          </div>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="col-12 pl-0">
                          <div className="form-group">
                            <label className="labelForm voda-bold mb-0 w-90">
                              Previous ResourceKey
                              <div className="d-flex">
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    validateResourceKeys(
                                      "previousResourceKey",
                                      e
                                    )
                                  }
                                  onKeyUp={(e) =>
                                    validateResourceKeys(
                                      "previousResourceKey",
                                      e
                                    )
                                  }
                                  className="inputForm w-100"
                                  value={formData?.previousResourceKey!}
                                  disabled={!editPrevResourceKey && props.edit}
                                />
                              </div>
                            </label>
                            {props.edit && (
                              <button
                                type="button"
                                title={"Edit Previous Resource Key"}
                                className="btn btn-link"
                                onClick={() =>
                                  setEditPrevResourceKey(!editPrevResourceKey)
                                }
                              >
                                <img
                                  className="btnEdit op-55"
                                  src={require("../../img/edit.png")}
                                />
                              </button>
                            )}
                          </div>
                          {prevResourceKeyError === true ||
                          (validation &&
                            validation.response == false &&
                            validation.property?.includes(
                              "previousResourceKey"
                            )) ? (
                            <label className="validationFeild h-16 mb-1 w-100">
                              Previous Resource Key must be within 255
                              characters.
                            </label>
                          ) : null}
                        </div>
                      </div>
                    </div>
                  )}
                </>
              )}
            </fieldset>
          </Container>
          <Container show={!props.wizardMode}>
            <div className="col-12 justify-content-end mt-4 pr-4 d-flex footerModal">
              <button
                className="voda-bold btn btn-link px-4 btnHeader cancel"
                type="button"
                onClick={() => {
                  props.action.closeModal && props.action.closeModal(changed);
                }}
              >
                Cancel
              </button>
              <button
                className="voda-bold btn btn-danger px-4 btnHeader"
                onClick={() =>
                  Save(
                    {
                      ...formData,
                      interfaceName:
                        formData?.categoryId !== 1
                          ? null
                          : formData?.interfaceName,
                      interfaceType:
                        formData?.categoryId !== 1
                          ? null
                          : formData?.interfaceType
                          ? formData?.interfaceTypes &&
                            dictionaryToArray(formData?.interfaceTypes).filter(
                              (x) => x.key == formData?.interfaceType
                            )[0]?.value
                          : null,
                    },
                    props.edit,
                    validazioneClient,
                    refresh,
                    RestoreOrphanDeleted,
                    orphanDeleted
                  )
                }
                type="button"
              >
                Submit
              </button>
            </div>
          </Container>
          {/* //WIZARD */}
        </div>
      </form>
    </div>
  );
};

export default IdentitiesModal;
