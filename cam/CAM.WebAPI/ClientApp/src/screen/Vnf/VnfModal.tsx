import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import { VNFTransitionDtoUpdate } from "../../Model/VNFTransition";
import {
  dictionaryToArray,
  dictionaryToArrayRelatedResource,
} from "../../Hook/Dictionary";
import { formatDateWithTime } from "../../Hook/Common";
import { CreatVNFTransition } from "../../Redux/Action/VNFTransition/VNFTransitionCreateAction";
import { useSelector } from "react-redux";
import { EditVNFTransition } from "../../Redux/Action/VNFTransition/VNFTransitionEditAction";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import { GetVNFTransitionGrid } from "../../Redux/Action/VNFTransition/VNFTransitionGridAction";
import Select from "react-select";

import { useAuth } from "../../Hook/useAuth";
import { Modal } from "react-bootstrap";
import OpCoContainer from "../../Containers/Lookup/OpCoContainer";
import EquipmentStatusContainer from "../../Containers/Lookup/EquipmentStatusContainer";
import VNFDesignComponentContainer from "../../Containers/Lookup/VNFDesignComponentContainer";
import NFVIBundleIDContainer from "../../Containers/Lookup/NFVIBundleIDContainer";
import ModalConfirm from "../../Components/ModalConfirm";
import { RelatedResource } from "../../Model/CommonModels";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
    Edit(id: number | undefined): any;
  };
  // data: VNFTransitionDtoUpdate | VNFTransitionDtoCreate | undefined | null,
  edit: boolean;
  keyTab?: string;
}

const ModalVNFTransition: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("VNFTransition");
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
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<VNFTransitionDtoUpdate>(
    CreatVNFTransition,
    EditVNFTransition
  );
  const dtoEditResourceState = (state: RootState) =>
    state.vNFTransitionEditReducer.VNFTransitionDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.vNFTransitionCreateReducer.VNFTransitionDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const { tipologicaPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
      GetVNFTransitionGrid({ principalId: editResource?.vnfTransitionId });
    } else {
      setFormData(createResource);
      GetVNFTransitionGrid();
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("VNFTransition");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  const validazioneClient = (copy: VNFTransitionDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.opCoId === null ||
      copy?.opCoId === undefined ||
      copy?.opCoId === 0
    ) {
      addInvalidProperty("opCoId");
    }
    if (
      copy.vnfType === null ||
      copy?.vnfType === undefined ||
      copy?.vnfType === ""
    ) {
      addInvalidProperty("vnfType");
    }
    if (
      copy.elementName === null ||
      copy?.elementName === undefined ||
      copy?.elementName.trim() === ""
    ) {
      addInvalidProperty("elementName");
    }
    if (
      copy.equipmentStatusId === null ||
      copy?.equipmentStatusId === undefined ||
      copy?.equipmentStatusId === 0
    ) {
      addInvalidProperty("equipmentStatusId");
    }
    if (
      copy.vnfDesignComponentId === null ||
      copy?.vnfDesignComponentId === undefined ||
      copy?.vnfDesignComponentId === 0
    ) {
      addInvalidProperty("vnfDesignComponentId");
    }
    if (
      copy.currentRelease === null ||
      copy?.currentRelease === undefined ||
      copy?.currentRelease === ""
    ) {
      addInvalidProperty("currentRelease");
    }
    if (
      copy.plannedRelease === null ||
      copy?.plannedRelease === undefined ||
      copy?.plannedRelease === ""
    ) {
      addInvalidProperty("plannedRelease");
    }
    if (
      copy.location === null ||
      copy?.location === undefined ||
      copy?.location === ""
    ) {
      addInvalidProperty("location");
    }
    if (
      copy.nfviSiteDesignation === null ||
      copy?.nfviSiteDesignation === undefined ||
      copy?.nfviSiteDesignation === ""
    ) {
      addInvalidProperty("nfviSiteDesignation");
    }
    if (
      copy.nfviBundleIDId === null ||
      copy?.nfviBundleIDId === undefined ||
      copy?.nfviBundleIDId === 0
    ) {
      addInvalidProperty("nfviBundleIDId");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const OpCoRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.opCoResource)
      formData.opCoResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const VnfDesignComponentRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.vnfDesignComponentResource)
      formData.vnfDesignComponentResource = obj as { [key: string]: string };
    setFormData(formData);
  };
  const NfviBundleIdRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({
        ...acc,
        [item.id]: {
          id: item.order,
          value: item.description,
        } as RelatedResource,
      }),
      {}
    );
    if (formData && formData?.nfviBundleIDResource)
      formData.nfviBundleIDResource = obj as { [key: string]: RelatedResource };
    setFormData(formData);
  };
  const EquipmentStatusRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.equipmentStatusResource)
      formData.equipmentStatusResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OpCoContainer
            returnObject={OpCoRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></OpCoContainer>
        );
      case 2:
        return (
          <VNFDesignComponentContainer
            returnObject={VnfDesignComponentRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></VNFDesignComponentContainer>
        );
      case 3:
        return (
          <NFVIBundleIDContainer
            returnObject={NfviBundleIdRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></NFVIBundleIDContainer>
        );
      case 4:
        return (
          <EquipmentStatusContainer
            returnObject={EquipmentStatusRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></EquipmentStatusContainer>
        );
      default:
        return;
    }
  };

  const OnChangeVnfType = (e: any, property: string) => {
    let copy = { ...formData } as VNFTransitionDtoUpdate;
    if (e && e != null) {
      copy.vnfType = e && e["key"];
    } else {
      copy.vnfType = "";
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(property);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }
  };

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const [disableForm, setDisableForm] = useState<boolean>(false);
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

  const OnChangeNfviSiteDesignation = (
    e: any,
    property: string
  ) => {
    let copy = { ...formData } as VNFTransitionDtoUpdate;
    if (e && e != null) {
      copy.nfviSiteDesignation = e && e["key"];
    } else {
      copy.nfviSiteDesignation = "";
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(property);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }
  };

  return (
    <div className="mt-4">
      <ModalConfirm data={confirmForm} />
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
        <Modal.Header >
          <div className="col-12 px-0">
            <div className="col-12">
              {/* <h4 className="mb-0">Lookup Tables</h4> */}
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>{ReturnLookupContainer(isVisibleModalLookup)}</Modal.Body>
      </Modal>

      <form id="formVNFTransition" onChange={() => setChanged(true)}>
        <div className="col-12 row mx-0">
          <div className="col-12 pl-0">
            <fieldset className="p-0">
              <legend className="text-bb mb-40">Description</legend>
              <div className="row">
                <div className="col-6">
                  <label className="labelForm voda-bold w-100">
                    OpCo <span className="red">*</span>
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
                          value={
                            formData && formData.opCoResource &&
                            dictionaryToArray(formData.opCoResource).filter(
                              (x) => x.key === formData.opCoId
                            )
                          }
                          onChange={(e) => onChangeSelect("opCoId", e)}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                      {tipologicaPermesso && (
                        // <button
                        //   className="btn btn-link"
                        //   onClick={() => setIsVisibleModalLookup(1)}
                        //   type="button"
                        // >
                        //   <img
                        //     style={{ height: 15 }}
                        //     src={require("../../img/plus_icon.png")}
                        //     alt="+"
                        //   />
                        // </button>
                        null
                      )}
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("opCoId") ? (
                      <label className="validation">
                        *OpCo must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="col-6">
                  <label className="labelForm voda-bold   w-100">
                    VNF Type<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.vnfTypeResource &&
                            formData?.vnfTypeResource
                              ?.map((x) => {
                                return { key: x ? x : "", value: x ? x : "" };
                              })
                              .sort((a, b) =>
                                a.value.toLowerCase() < b.value.toLowerCase()
                                  ? -1
                                  : 1
                              )
                          }
                          value={
                            formData && formData?.vnfTypeResource &&
                            formData?.vnfTypeResource
                              ?.map((x) => {
                                return { key: x, value: x };
                              })
                              .filter((x) => x.key === formData?.vnfType)
                          }
                          onChange={(e) => OnChangeVnfType(e, "vnfType")}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"]}
                        ></Select>
                      </div>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("vnfType") ? (
                      <label className="validation">
                        *VNF Type must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="col-6">
                  <label className="labelForm voda-bold  w-100">
                    Equipment Status<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.equipmentStatusResource &&
                            dictionaryToArray(
                              formData?.equipmentStatusResource
                            ).sort((a, b) =>
                              a.value.toLowerCase() < b.value.toLowerCase()
                                ? -1
                                : 1
                            )
                          }
                          value={
                            formData && formData?.equipmentStatusResource &&
                            dictionaryToArray(
                              formData?.equipmentStatusResource
                            ).filter(
                              (x) => x.key === formData?.equipmentStatusId
                            )
                          }
                          onChange={(e) =>
                            onChangeSelect("equipmentStatusId", e)
                          }
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                      {tipologicaPermesso && (
                        <button
                          className="btn btn-link"
                          onClick={() => setIsVisibleModalLookup(4)}
                          type="button"
                        >
                          <img
                            style={{ height: 15 }}
                            src={require("../../img/plus_icon.png")}
                            alt="+"
                          />
                        </button>
                      )}
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("equipmentStatusId") ? (
                      <label className="validation">
                        *equipment Status must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="col-6">
                  <label className="labelForm voda-bold   w-100">
                    Design Component<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.vnfDesignComponentResource &&
                            dictionaryToArray(
                              formData?.vnfDesignComponentResource
                            ).sort((a, b) =>
                              a.value.toLowerCase() < b.value.toLowerCase()
                                ? -1
                                : 1
                            )
                          }
                          value={
                            formData && formData?.vnfDesignComponentResource &&
                            dictionaryToArray(
                              formData?.vnfDesignComponentResource
                            ).filter(
                              (x) => x.key === formData?.vnfDesignComponentId
                            )
                          }
                          onChange={(e) =>
                            onChangeSelect("vnfDesignComponentId", e)
                          }
                          onBlur={() => setInputValue("")}
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
                            alt="+"
                          />
                        </button>
                      )}
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("vnfDesignComponentId") ? (
                      <label className="validation">
                        *Vnf Design Component must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="col-6">
                  <label className="labelForm voda-bold   w-100">
                    Element Name<span className="red">*</span>
                    <input
                      onChange={(e) => onChange("elementName", e)}
                      onKeyUp={(e) => onChange("elementName", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.elementName}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("elementName") ? (
                      <label className="validation">
                        *Element Name must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
            </fieldset>
          </div>
          <div className="col-12 pl-0">
            <fieldset className="p-0  mt-25">
              <label className="text-bb mb-40 mt-50">VNF Release Details</label>
              <div className="row">
                <div className="col-6">
                  <label className="labelForm voda-bold   w-100">
                    Current Release<span className="red">*</span>
                    <input
                      onChange={(e) => onChange("currentRelease", e)}
                      onKeyUp={(e) => onChange("currentRelease", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.currentRelease}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("currentRelease") ? (
                      <label className="validation">
                        *current Release must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="col-6">
                  <label className="labelForm voda-bold   w-100">
                    Planned Release (Pre Upg)<span className="red">*</span>
                    <input
                      onChange={(e) => onChange("plannedRelease", e)}
                      onKeyUp={(e) => onChange("plannedRelease", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.plannedRelease}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("plannedRelease") ? (
                      <label className="validation">
                        *planned Release must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
            </fieldset>
            <fieldset className="mt-4 p-0 pl-0">
              <label className="text-bb mb-40 mt-50">
                Active bundle release
              </label>
              <div className="row">
                <div className="col-6">
                  <label className="labelForm voda-bold w-100">
                    NFVI Bundle ID<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.nfviBundleIDResource &&
                            dictionaryToArrayRelatedResource(
                              formData?.nfviBundleIDResource
                            )
                              .map((x) => {
                                return {
                                  key: x.key,
                                  value: x.value.value ?? "",
                                  order: x.value.id ?? "",
                                };
                              })
                              .sort((a, b) => (a.order < b.order ? -1 : 1))
                          }
                          value={
                            formData && formData?.nfviBundleIDResource &&
                            dictionaryToArrayRelatedResource(
                              formData?.nfviBundleIDResource
                            )
                              .map((x) => {
                                return {
                                  key: x.key,
                                  value: x.value.value ?? "",
                                  order: x.value.id ?? "",
                                };
                              })
                              .filter((x) => x.key === formData?.nfviBundleIDId)
                          }
                          onChange={(e) => onChangeSelect("nfviBundleIDId", e)}
                          onBlur={() => setInputValue("")}
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
                            alt="+"
                          />
                        </button>
                      )}
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("nfviBundleIDId") ? (
                      <label className="validation">
                        *nfvi planned release must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
            </fieldset>
          </div>
          <div className="col-12 px-0 mt-25">
            <fieldset className="fieldset pl-0">
              <label className="text-bb mb-40 mt-50">Equipment Location</label>
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="form-group col-12">
                    <label className="labelForm voda-bold w-100">
                      Location<span className="red">*</span>
                      <input
                        onChange={(e) => onChange("location", e)}
                        onKeyUp={(e) => onChange("location", e)}
                        type="text"
                        className="inputForm w-100"
                        value={formData?.location}
                      />
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("location") ? (
                        <label className="validation">
                          *location must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
                <div className="col-6 pr-0 pl-0">
                  <div className="col-12">
                    <label className="labelForm voda-bold w-100">
                      NFVI-Site Designation<span className="red">*</span>
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                          menuPosition={"fixed"}
                            options={
                              formData && formData.nfviSiteDesignationResource ?
                              formData.nfviSiteDesignationResource.map(
                                (x) => {
                                  return { key: x, value: x };
                                }
                              ) : []
                            }
                            value={
                              formData && formData.nfviSiteDesignationResource ?
                              formData.nfviSiteDesignationResource
                                ?.map((x) => {
                                  return { key: x, value: x };
                                })
                                .filter(
                                  (x) => x.key === formData.nfviSiteDesignation
                                ) : undefined
                            }
                            onChange={(e) =>
                              OnChangeNfviSiteDesignation(
                                e,
                                "nfviSiteDesignation"
                              )
                            }
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isClearable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) => option["key"]}
                          ></Select>
                        </div>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("nfviSiteDesignation") ? (
                        <label className="validation">
                          *NFVI-Site Designation must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
              </div>
            </fieldset>
          </div>

          {props.edit === true ? (
            <div className="col-12 px-0">
              <fieldset className="fieldset pl-0">
                <label className="text-bb mb-40 mt-50">Further Details</label>
                <div className="row pr-0">
                  <div className="col-6 form-group pr-0">
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
                  <div className="col-6 pr-0">
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
              </fieldset>
            </div>
          ) : null}
          {/* <div className="col-12 px-0">
            <fieldset className="fieldset">
              <legend className="red  ">Further Details:</legend>
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12 pl-0">
                    <label className="labelForm voda-bold   w-100">
                      Spare 1
                      <input
                        onChange={(e) => onChange("spare1Json", e)}
                        onKeyUp={(e) => onChange("spare1Json", e)}
                        type="text"
                        className="inputForm w-100"
                        value={formData?.spare1Json}
                      />
                    </label>
                  </div>
                </div>
                {props.edit === true ? (
                  <div className="col-6 pr-0">
                    <div className="col-12 form-group pr-0">
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
                    <div className="col-12 pr-0">
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
            </fieldset>
          </div> */}
        </div>
      </form>
      <div className="col-12 justify-content-end mt-4 d-flex footerModal">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={() =>
            Save(
              formData,
              props.edit,
              validazioneClient,
              refresh,
              RestoreOrphanDeleted,
              orphanDeleted
            )
          }
          type="button"
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default ModalVNFTransition;
