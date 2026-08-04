import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import { NFVITransitionDtoUpdate } from "../../Model/NFVITransition";
import { formatDateWithTime } from "../../Hook/Common";
import { CreatNFVITransition } from "../../Redux/Action/NFVITransition/NFVITransitionCreateAction";
import { useSelector } from "react-redux";
import { EditNFVITransition } from "../../Redux/Action/NFVITransition/NFVITransitionEditAction";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import { GetNFVITransitionGrid } from "../../Redux/Action/NFVITransition/NFVITransitionGridAction";
import Select from "react-select";
import { useAuth } from "../../Hook/useAuth";
import { Modal } from "react-bootstrap";
import OpCoContainer from "../../Containers/Lookup/OpCoContainer";
import NFVIStatusContainer from "../../Containers/Lookup/NFVIStatusContainer";
import { colourStyles } from "../../Hook/Common";
import { RelatedResource } from "../../Model/CommonModels";
import ModalConfirm from "../../Components/ModalConfirm";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  dictionaryToArray,
  dictionaryToArrayRelatedResource,
} from "../../Hook/Dictionary";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
    Edit(id: number | undefined): any;
  };
  // data: NFVITransitionDtoUpdate | NFVITransitionDtoCreate | undefined | null,
  edit: boolean;
  keyTab?: string;
}

const ModalNFVITransition: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("NFVITransition");
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
  } = useFormTableCrud<NFVITransitionDtoUpdate>(
    CreatNFVITransition,
    EditNFVITransition
  );
  const dtoEditResourceState = (state: RootState) =>
    state.nFVITransitionEditReducer.NFVITransitionDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.nFVITransitionCreateReducer.NFVITransitionDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const { tipologicaPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [visibleFurther, setVisibleFurher] = useState<boolean>(false);

  const [resourceStatusLookup, setResourceStatusLookup] = useState<
    { key: number; value: string; color?: string | "#000000" }[] | undefined
  >([]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
      GetNFVITransitionGrid({ principalId: editResource?.nfviTransitionId });
    } else {
      setFormData(createResource);
      GetNFVITransitionGrid();
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("NFVITransition");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  const validazioneClient = (copy: NFVITransitionDtoUpdate) => {
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
      copy.nfviSiteDesignation === null ||
      copy?.nfviSiteDesignation === undefined ||
      copy?.nfviSiteDesignation === ""
    ) {
      addInvalidProperty("nfviSiteDesignation");
    }
    if (
      copy.statusLabMCId === null ||
      copy?.statusLabMCId === undefined ||
      copy?.statusLabMCId === 0
    ) {
      addInvalidProperty("statusLabMCId");
    }
    if (
      copy.statusLabSCId === null ||
      copy?.statusLabSCId === undefined ||
      copy?.statusLabSCId === 0
    ) {
      addInvalidProperty("statusLabSCId");
    }
    if (
      copy.statusLiveMCId === null ||
      copy?.statusLiveMCId === undefined ||
      copy?.statusLiveMCId === 0
    ) {
      addInvalidProperty("statusLiveMCId");
    }
    if (
      copy.statusLiveSCId === null ||
      copy?.statusLiveSCId === undefined ||
      copy?.statusLiveSCId === 0
    ) {
      addInvalidProperty("statusLiveSCId");
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
  const NFVIStatusRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({
        ...acc,
        [item.id]: {
          id: item.color,
          value: item.description,
        } as RelatedResource,
      }),
      {}
    );
    if (
      formData &&
      formData?.statusLiveMCResource &&
      formData?.statusLiveSCResource &&
      formData?.statusLabSCResource &&
      formData?.statusLabMCResource &&
      formData.status12KSwitchResource
    ) {
      formData.statusLiveMCResource = obj as { [key: string]: RelatedResource };
      formData.statusLiveSCResource = obj as { [key: string]: RelatedResource };
      formData.statusLabSCResource = obj as { [key: string]: RelatedResource };
      formData.statusLabMCResource = obj as { [key: string]: RelatedResource };
      formData.status12KSwitchResource = obj as {
        [key: string]: RelatedResource;
      };
    }
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
          <NFVIStatusContainer
            returnObject={NFVIStatusRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></NFVIStatusContainer>
        );
      default:
        return;
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

      <form id="formNFVITransition" onChange={() => setChanged(true)}>
        <div className="col-12">
          <fieldset className="fieldset p-0">
            <legend className="text-bb mb-40">NFVI Details</legend>
            <div className="row">
              <div className="col-6">
                <div className="form-group col-12 pl-0">
                  <label className="labelForm voda-bold w-100">
                    OpCo<span className="red">*</span>
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
                            formData?.opCoResource &&
                            dictionaryToArray(formData?.opCoResource).filter(
                              (x) => x.key === formData?.opCoId
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
              </div>
              <div className="col-6">
                <div className="form-group col-12 pr-0 pl-1">
                  <label className="labelForm voda-bold w-100">
                    NFVI-Site Designation<span className="red">*</span>
                    <input
                      onChange={(e) => onChange("nfviSiteDesignation", e)}
                      onKeyUp={(e) => onChange("nfviSiteDesignation", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.nfviSiteDesignation}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("nfviSiteDesignation") ? (
                      <label className="validation">
                        *NFVI Site Designation must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-12">
                <fieldset className="p-0">
                  <label className="text-bb mb-40 mt-50">
                    NFVI Upgrade Lab Status
                  </label>
                  <div className="row">
                    <div className="col-6">
                      <label className="labelForm voda-bold w-100">
                        Lab MC Status<span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                            menuPosition={"fixed"}
                              options={
                                formData?.statusLabMCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLabMCResource
                                ).map((x) => {
                                  return {
                                    key: x.key,
                                    value: x.value.value ?? "",
                                    color: x.value.id ?? "",
                                  };
                                })
                              }
                              value={
                                formData?.statusLabMCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLabMCResource
                                )
                                  .filter(
                                    (x) => x.key === formData?.statusLabMCId
                                  )
                                  .map((x) => {
                                    return {
                                      key: x.key,
                                      value: x.value.value ?? "",
                                      color: x.value.id ?? "",
                                    };
                                  })
                              }
                              onChange={(e) =>
                                onChangeSelect("statusLabMCId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              styles={colourStyles}
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
                        validation.property?.includes("statusLabMCId") ? (
                          <label className="validation">
                            *Lab-MC must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                    <div className="col-6">
                      <label className="labelForm voda-bold w-100">
                        Lab SC Status<span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                            menuPosition={"fixed"}
                              options={
                                formData?.statusLabSCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLabSCResource
                                ).map((x) => {
                                  return {
                                    key: x.key,
                                    value: x.value.value ?? "",
                                    color: x.value.id ?? "",
                                  };
                                })
                              }
                              value={
                                formData?.statusLabSCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLabSCResource
                                )
                                  .filter(
                                    (x) => x.key === formData?.statusLabSCId
                                  )
                                  .map((x) => {
                                    return {
                                      key: x.key,
                                      value: x.value.value ?? "",
                                      color: x.value.id ?? "",
                                    };
                                  })
                              }
                              onChange={(e) =>
                                onChangeSelect("statusLabSCId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              styles={colourStyles}
                            ></Select>
                          </div>
                        </div>
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes("statusLabSCId") ? (
                          <label className="validation">
                            *Lab-SC must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                </fieldset>
              </div>
              <div className="col-12">
                <fieldset className="p-0 mt-25">
                  <label className="text-bb mb-40 mt-50">
                    NFVI Upgrade Live Status
                  </label>
                  <div className="row">
                    <div className="col-6">
                      <label className="labelForm voda-bold w-100">
                        Live MC Status <span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                            menuPosition={"fixed"}
                              options={
                                formData?.statusLiveMCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLiveMCResource
                                ).map((x) => {
                                  return {
                                    key: x.key,
                                    value: x.value.value ?? "",
                                    color: x.value.id ?? "",
                                  };
                                })
                              }
                              value={
                                formData?.statusLiveMCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLiveMCResource
                                )
                                  .filter(
                                    (x) => x.key === formData?.statusLiveMCId
                                  )
                                  .map((x) => {
                                    return {
                                      key: x.key,
                                      value: x.value.value ?? "",
                                      color: x.value.id ?? "",
                                    };
                                  })
                              }
                              onChange={(e) =>
                                onChangeSelect("statusLiveMCId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              styles={colourStyles}
                            ></Select>
                          </div>
                        </div>
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes("statusLiveMCId") ? (
                          <label className="validation">
                            *Live-MC must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                    <div className="col-6">
                      <label className="labelForm voda-bold w-100">
                        Live SC Status<span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                            menuPosition={"fixed"}
                              options={
                                formData?.statusLiveSCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLiveSCResource
                                ).map((x) => {
                                  return {
                                    key: x.key,
                                    value: x.value.value ?? "",
                                    color: x.value.id ?? "",
                                  };
                                })
                              }
                              value={
                                formData?.statusLiveSCResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.statusLiveSCResource
                                )
                                  .filter(
                                    (x) => x.key === formData?.statusLiveSCId
                                  )
                                  .map((x) => {
                                    return {
                                      key: x.key,
                                      value: x.value.value ?? "",
                                      color: x.value.id ?? "",
                                    };
                                  })
                              }
                              onChange={(e) =>
                                onChangeSelect("statusLiveSCId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              styles={colourStyles}
                            ></Select>
                          </div>
                        </div>
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes("statusLiveSCId") ? (
                          <label className="validation">
                            *Live-SC must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                </fieldset>
              </div>
            </div>
          </fieldset>

          <button
            type="button"
            className="further-btn"
            onClick={() => setVisibleFurher(!visibleFurther)}
          >
            Click for further details
          </button>
          {visibleFurther && (
            <fieldset className="fieldset p-0 mt-25">
              <label className="text-bb mb-40 mt-50">Further Details</label>
              <div className="row">
                <div className="col-12">
                  <div className="row">
                    <div className="col-6">
                      <label className="labelForm voda-bold w-100">
                        Status: 12K Switch
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                            menuPosition={"fixed"}
                              options={
                                formData?.status12KSwitchResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.status12KSwitchResource
                                ).map((x) => {
                                  return {
                                    key: x.key,
                                    value: x.value.value ?? "",
                                    color: x.value.id ?? "",
                                  };
                                })
                              }
                              value={
                                formData?.status12KSwitchResource &&
                                dictionaryToArrayRelatedResource(
                                  formData?.status12KSwitchResource
                                )
                                  .filter(
                                    (x) => x.key === formData?.status12KSwitchId
                                  )
                                  .map((x) => {
                                    return {
                                      key: x.key,
                                      value: x.value.value ?? "",
                                      color: x.value.id ?? "",
                                    };
                                  })
                              }
                              onChange={(e) =>
                                onChangeSelect("status12KSwitchId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              styles={colourStyles}
                            ></Select>
                          </div>
                        </div>
                      </label>
                      {/* {validation && validation.response == false && validation.property == "statusLiveSCId" ? <label className="validation">*Live-SC must have a value</label> : null} */}
                    </div>
                    <div className="col-6">
                      <label className="labelForm voda-bold  w-100">
                        Next Step
                        <input
                          onChange={(e) => onChange("nextStep", e)}
                          onKeyUp={(e) => onChange("nextStep", e)}
                          type="text"
                          className="inputForm w-100"
                          value={formData?.nextStep}
                        />
                      </label>
                    </div>
                  </div>
                </div>

                <div className="col-12">
                  <div className="row mt-50">
                    {props.edit === true ? (
                      <div className=" col-6 form-group">
                        <label className="labelForm voda-bold  w-100">
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
                    ) : null}
                    {props.edit ? (
                      <div className="col-6">
                        <label className="labelForm voda-bold w-100">
                          Last Modified By
                          <input
                            readOnly={true}
                            className="inputForm w-100 voda-regular"
                            type="text"
                            value={formData?.lastModifiedBy}
                          />
                        </label>
                      </div>
                    ) : null}
                  </div>
                </div>
              </div>
            </fieldset>
          )}
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

export default ModalNFVITransition;
