import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import { BundleUpgradeInitiativeDtoUpdate } from "../../Model/BundleUpgradeIniziative";
import { formatDateWithTime } from "../../Hook/Common";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { CreatBundleUpgradeInitiative } from "../../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeCreateAction";
import { useSelector } from "react-redux";
import { EditBundleUpgradeInitiative } from "../../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeEditAction";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import { GetBundleUpgradeInitiativeGrid } from "../../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeGridAction";
import Select from "react-select";
import { useAuth } from "../../Hook/useAuth";
import { Modal } from "react-bootstrap";
import OriginalEquipmentManufacturer from "../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import ModalConfirm from "../../Components/ModalConfirm";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
    Edit(id: number | undefined): any;
  };
  edit: boolean;
  keyTab?: string;
}

const ModalBundleUpgradeInitiative: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("BundleUpgradeInitiative");
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
  } = useFormTableCrud<BundleUpgradeInitiativeDtoUpdate>(
    CreatBundleUpgradeInitiative,
    EditBundleUpgradeInitiative
  );
  const dtoEditResourceState = (state: RootState) =>
    state.bundleUpgradeInitiativeEditReducer.BundleUpgradeInitiativeDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.bundleUpgradeInitiativeCreateReducer.BundleUpgradeInitiativeDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const { tipologicaPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [visibleFurther, setVisibleFurther] = useState<boolean>(false);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
      GetBundleUpgradeInitiativeGrid({
        principalId: editResource?.bundleUpgradeInitiativeId,
      });
    } else {
      setFormData(createResource);
      GetBundleUpgradeInitiativeGrid();
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("BundleUpgradeInitiative");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  const validazioneClient = (copy: BundleUpgradeInitiativeDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.verticalOwner === null ||
      copy?.verticalOwner === undefined ||
      copy?.verticalOwner.trim() === ""
    ) {
      addInvalidProperty("verticalOwner");
    }
    if (
      copy?.originalEquipmentManufacturerId === null ||
      copy?.originalEquipmentManufacturerId === undefined ||
      copy?.originalEquipmentManufacturerId === 0
    ) {
      addInvalidProperty("originalEquipmentManufacturerId");
    }
    if (
      copy.vnfType === null ||
      copy?.vnfType === undefined ||
      copy?.vnfType === ""
    ) {
      addInvalidProperty("vnfType");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const OriginalEquipmentManufacturerRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.originalEquipmentManufacturerResource)
      formData.originalEquipmentManufacturerResource = obj as {
        [key: string]: string;
      };
    setFormData(formData);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OriginalEquipmentManufacturer
            returnObject={OriginalEquipmentManufacturerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></OriginalEquipmentManufacturer>
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
        <Modal.Header closeButton>
          <div className="col-12 px-0">
            <div className="col-12"></div>
          </div>
        </Modal.Header>
        <Modal.Body>{ReturnLookupContainer(isVisibleModalLookup)}</Modal.Body>
      </Modal>

      <form id="formBundleUpgradeInitiative" onChange={() => setChanged(true)}>
        <div className="col-12">
          <fieldset className="fieldset p-0">
            <legend className="text-bb mb-40">VNF DETAILS</legend>
            <div className="row">
              <div className="col-6">
                <div className="">
                  <label className="labelForm voda-bold w-100">
                    Equipment Manufacturer <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.originalEquipmentManufacturerResource
                            ).sort((a, b) =>
                              a.value.toLowerCase() < b.value.toLowerCase()
                                ? -1
                                : 1
                            )
                          }
                          value={
                            formData?.originalEquipmentManufacturerResource &&
                            dictionaryToArray(
                              formData?.originalEquipmentManufacturerResource
                            ).filter(
                              (x) =>
                                x.key ==
                                formData?.originalEquipmentManufacturerId
                            )
                          }
                          onChange={(e) =>
                            onChangeSelect("originalEquipmentManufacturerId", e)
                          }
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
                        //     alt="plus"
                        //   />
                        // </button>
                        null
                      )}
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes(
                      "originalEquipmentManufacturerId"
                    ) ? (
                      <label className="validation">
                        *Equipment manufacturer must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="form-group">
                  <label className="labelForm voda-bold   w-100">
                    Vertical Owner<span className="red">*</span>
                    <input
                      onChange={(e) => onChange("verticalOwner", e)}
                      onKeyUp={(e) => onChange("verticalOwner", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.verticalOwner}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("verticalOwner") ? (
                      <label className="validation">
                        *vertical Owner must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="">
                  <label className="labelForm voda-bold   w-100">
                    VNF Type<span className="red">*</span>
                    <input
                      onChange={(e) => onChange("vnfType", e)}
                      onKeyUp={(e) => onChange("vnfType", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.vnfType}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("vnfType") ? (
                      <label className="validation">
                        *Vnf Type must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
                <div className="form-group">
                  <label className="labelForm voda-bold  w-100">
                    Equipment Manufacturer Certified Release
                    <input
                      onChange={(e) => onChange("oemCertifiedRelease", e)}
                      onKeyUp={(e) => onChange("oemCertifiedRelease", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.oemCertifiedRelease}
                    />
                  </label>
                </div>
              </div>
            </div>
          </fieldset>

          <div className="col-12 pl-0">
            <button
              type="button"
              className="further-btn"
              onClick={() => setVisibleFurther(!visibleFurther)}
            >
              Click for further details
            </button>
          </div>
          {visibleFurther && (
            <fieldset className="fieldset p-0">
              <label className="text-bb mb-40 mt-50">Further Details</label>
              <div className="row">
                <div className="col-6">
                  <div className="">
                    <label className="labelForm voda-bold   w-100">
                      Remarks
                      <input
                        onChange={(e) => onChange("remarks", e)}
                        onKeyUp={(e) => onChange("remarks", e)}
                        type="text"
                        className="inputForm w-100"
                        value={formData?.remarks}
                      />
                    </label>
                  </div>
                </div>
                <div className="col-6">
                  <div className="">
                    <label className="labelForm voda-bold w-100">
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
                  <div className="col-6 mt-50">
                    <label className="labelForm voda-bold w-100">
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
                {props.edit === true ? (
                  <div className="col-6 mt-50">
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
                ) : null}
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

export default ModalBundleUpgradeInitiative;
