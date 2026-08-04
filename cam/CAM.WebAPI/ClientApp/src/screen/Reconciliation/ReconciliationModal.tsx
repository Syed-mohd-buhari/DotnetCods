import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
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
import { CreatNetworkElementAsIs } from "../../Redux/Action/NetworkElementAsIs/NetworkElementAsIsCreateAction";
import { EditNetworkElementAsIs } from "../../Redux/Action/NetworkElementAsIs/NetworkElementAsIsEditAction";
import { RootState } from "../../Redux/Store/rootStore";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import DatePicker from "react-datepicker";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
    Edit(id: number | undefined): any;
  };
  // data: NetworkElementAsIsDtoUpdate | LcmEngineeringDtoCreate | undefined | null,
  edit: boolean;
}

const NetworkElementAsIsModal: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("networkelement");
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
  } = useFormTableCrud<NetworkElementAsIsDtoUpdate>(
    CreatNetworkElementAsIs,
    EditNetworkElementAsIs
  );

  const dtoEditResourceState = (state: RootState) =>
    state.networkElementAsIsEditReducer.NetworkElementAsIsDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.networkElementAsIsCreateReducer.NetworkElementAsIsDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const { tipologicaPermesso } = useAuth();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      let copy = { ...editResource } as NetworkElementAsIsDtoUpdate;
      InsertSystemTypeInfo(copy);
      // setFormData(editResource);
    } else {
      let copy = { ...createResource } as NetworkElementAsIsDtoCreate;
      copy.dataAcquisitionMethod = "Manual";
      copy.hardwareAcquisition = "Full";
      copy.manualOverride = false;
      copy.nodeType = undefined;
      copy.elementManagerExportFileFormat = "N/A";

      //DA CHIARIRE
      copy.networkFunction = " ";
      setFormData(copy);
    }
  }, [createResource, editResource, props.edit]);

  //VALIDAZIONE PRE Save
  const validazioneClient = (copy: NetworkElementAsIsDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.opCoId == null ||
      copy?.opCoId === undefined ||
      copy?.opCoId === 0
    ) {
      addInvalidProperty("opCoId");
    }
    if (
      copy?.originalEquipmentManufacturerId == null ||
      copy?.originalEquipmentManufacturerId === undefined ||
      copy?.originalEquipmentManufacturerId === 0
    ) {
      addInvalidProperty("originalEquipmentManufacturerId");
    }
    if (
      copy?.systemTypeId == null ||
      copy?.systemTypeId === undefined ||
      copy?.systemTypeId === 0
    ) {
      addInvalidProperty("systemTypeId");
    }
    if (
      copy?.elementDeploymentName == null ||
      copy?.elementDeploymentName === undefined ||
      copy?.elementDeploymentName.trim() === ""
    ) {
      addInvalidProperty("elementDeploymentName");
    }
    if (
      copy?.locationId == null ||
      copy?.locationId === undefined ||
      copy?.locationId === 0
    ) {
      addInvalidProperty("locationId");
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

  const OriginalEquipmentManufacturerRefillData = (value: Array<any>) => {
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

  const LocationRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.locationResource)
      formData.locationResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <OpcoContainer
            returnObject={OpCoRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 2:
        return (
          <OriginalEquipmentManufacturer
            returnObject={OriginalEquipmentManufacturerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></OriginalEquipmentManufacturer>
        );
      case 7:
        return (
          <Location
            returnObject={LocationRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      default:
        return;
    }
  };

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const [disableForm, setDisableForm] = useState<boolean>(false);

  const [editState, setEditState] = useState<number>(0);
  const [typeActive, setTypeActive] = useState<number>(0);

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

  const onChangeOem = async (e: any) => {
    //Rimuovi Validazione
    if (validation?.property?.includes("originalEquipmentManufacturerId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(
        "originalEquipmentManufacturerId"
      );
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    let copy = { ...formData } as NetworkElementAsIsDtoUpdate;
    if (e && e["key"]) {
      copy.originalEquipmentManufacturerId = e["key"];
      copy.systemTypeId = undefined;
      await GetSystemTypeList(e["key"]).then((x) => {
        if (x != undefined) {
          copy.systemTypeResource = x;
        }
      });
    } else {
      copy.originalEquipmentManufacturerId = undefined;
      copy.systemTypeId = undefined;
      copy.systemTypeResource = createResource?.systemTypeResource;
    }
    setFormData(copy);
  };

  const onChangeSystemType = async (e: any) => {
    //Rimuovi Validazione
    if (validation?.property?.includes("systemTypeId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("systemTypeId");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    let copy = { ...formData } as NetworkElementAsIsDtoUpdate;
    if (e && e["key"]) {
      copy.systemTypeId = e["key"];
      // await GetNetworkElementAsPlannedResource(e["key"]).then((x) => {
      //   if (x != undefined) {
      //     copy.networkElementAsPlannedResource = x;
      //   } else {
      //     copy.networkElementAsPlannedResource = undefined;
      //   }
      // });
      setFormData(copy);
      //InsertSystemTypeInfo(copy);
    } else {
      copy.systemTypeId = undefined;
      copy.platform = undefined;
      copy.hardwareType = undefined;
      copy.hardwareSolution = undefined;
      copy.otherHardwareInfo = undefined;
      copy.softwareReleaseInformation = undefined;
      setFormData(copy);
    }
  };

  const getNewtworkEleemntAsPlanned = async () => {
    let copy = { ...formData } as NetworkElementAsIsDtoUpdate;
    await GetNetworkElementAsPlannedResource({
      sysId: copy?.systemTypeId!,
      opcoId: copy?.opCoId!,
    }).then((x) => {
      if (x != undefined) {
        copy.networkElementAsPlannedResource = x;
      } else {
        copy.networkElementAsPlannedResource = undefined;
      }
    });
    InsertSystemTypeInfo(copy);
  };

  //commnt

  useEffect(() => {
    if (formData?.systemTypeId && formData?.opCoId) {
      getNewtworkEleemntAsPlanned();
    }
  }, [formData?.systemTypeId, formData?.opCoId]);

  const InsertSystemTypeInfo = async (copy: NetworkElementAsIsDtoUpdate) => {
    if (props.edit && copy.networkElementAsPlannedId != undefined) {
      await GetSystemTypeListFromAsPlanned(copy.networkElementAsPlannedId).then(
        (x) => {
          if (x != undefined) {
            copy.systemTypeResource = x;
          }
        }
      );
    }
    if (copy.systemTypeId != undefined) {
      await GetSystemTypeInfo(copy.systemTypeId).then((x) => {
        if (x != undefined) {
          let result = x as NetworkElementAsIsSystemTypeInfo;
          copy.platform = result.platform;
          copy.hardwareType = result.hardwareType;
          copy.hardwareSolution = result.hardwareSolution;
          copy.otherHardwareInfo = result.otherHardwareInfo;
          copy.softwareReleaseInformation = result.softwareReleaseInformation;
          copy.networkFunction = result.networkFunction;
          setFormData(copy);
        }
      });
    }
  };

  const onChangeElementName = async (e: any) => {
    //Rimuovi Validazione
    if (validation?.property?.includes("elementDeploymentName")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("elementDeploymentName");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    let copy = { ...formData } as NetworkElementAsIsDtoUpdate;
    if (e && e["key"]) {
      copy.elementDeploymentName = e.value;
      copy.networkElementAsPlannedId = e["key"];

      await GetAsPlannedLocation(e["key"]).then((x) => {
        if (x != undefined) {
          copy.locationId = x;
        }
      });
    } else {
      copy.elementDeploymentName = undefined;
      copy.networkElementAsPlannedId = undefined;
    }
    setFormData(copy);
  };

  const [backupFormData, setBackupFormData] =
    useState<NetworkElementAsIsDtoUpdate | null>();

  const onChangeEditState = (val: number) => {
    setEditState(val);
    setTypeActive(val);
    setBackupFormData(formData);
    let copy = { ...formData } as NetworkElementAsIsDtoUpdate;
    InsertSystemTypeInfo(copy);
  };

  const cancelEditState = () => {
    setFormData(backupFormData);
    setEditState(0);
  };

  const setConfirmEditState = () => {
    let copy = { ...formData } as NetworkElementAsIsDtoUpdate;
    const isValid = validazioneClient(copy).response;
    if (isValid) {
      setConfirm({
        title: "Save changes?",
        message: "Are you sure you want to continue?",
        button: "Continue",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => setConfirm(stateConfirm),
          confirm: () => {
            setEditState(0);
            setConfirm(stateConfirm);
          },
        },
      });
    }
  };

  return (
    <div className="mt-4 col-12">
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
        <Modal.Body>{ReturnLookupContainer(isVisibleModalLookup)}</Modal.Body>
      </Modal>

      <form onChange={() => setChanged(true)}>
        <div className="row">
          <div className="col-12">
            <fieldset className="fieldset p-0">
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
                              dictionaryToArray(formData?.opCoResource)
                            }
                            value={
                              formData &&
                              formData?.opCoResource &&
                              dictionaryToArray(formData?.opCoResource).filter(
                                (x) => x.key === formData?.opCoId
                              )
                            }
                            onChange={(e) => {
                              onChangeSelect("opCoId", e);
                            }}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isDisabled={props.edit}
                            isClearable
                            getOptionLabel={(option) => option.value}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                            // isDisabled={true}
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
                              alt="+"
                            />
                          </button>
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
                  {props.edit && (
                    <div className="mt-4 col-12 pl-0 form-group">
                      <label className="labelForm voda-bold  w-100 mb-0">
                        Network Function
                        <input
                          type="text"
                          disabled
                          className="inputForm w-100"
                          defaultValue={formData?.networkFunction}
                        />
                      </label>
                    </div>
                  )}
                </div>

                <div className="col-6">
                  {!props.edit ? (
                    <div className="form-group col-12 pr-0">
                      <label className="labelForm voda-bold   w-100">
                        Software Manufacturer<span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.originalEquipmentManufacturerResource &&
                                dictionaryToArray(
                                  formData?.originalEquipmentManufacturerResource
                                )
                              }
                              value={
                                formData &&
                                formData?.originalEquipmentManufacturerResource &&
                                dictionaryToArray(
                                  formData?.originalEquipmentManufacturerResource
                                ).filter(
                                  (x) =>
                                    x.key ===
                                    formData?.originalEquipmentManufacturerId
                                )
                              }
                              onChange={(e) => onChangeOem(e)}
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              // isDisabled={props.edit}
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
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
                        validation.property?.includes(
                          "originalEquipmentManufacturerId"
                        ) ? (
                          <label className="validation">
                            *Sw Manufacturer must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  ) : null}
                  <div className="col-12 pr-0">
                    <label className="labelForm   w-100">
                      <label
                        className="labelForm voda-bold mb-0"
                        title={
                          formData?.systemTypeResource &&
                          dictionaryToArray(formData?.systemTypeResource)
                            .find((x) => x.key === formData?.systemTypeId)
                            ?.value.replace('<b class="text-lowercase">', "")
                            .replace("</b>", "")
                        }
                      >
                        System Type<span className="red">*</span>
                      </label>
                      <Select
                        menuPosition={"fixed"}
                        options={
                          formData?.systemTypeResource &&
                          dictionaryToArray(formData?.systemTypeResource).sort(
                            (a, b) =>
                              a.value.toLowerCase() < b.value.toLowerCase()
                                ? -1
                                : 1
                          )
                        }
                        value={
                          formData &&
                          formData?.systemTypeResource &&
                          dictionaryToArray(
                            formData?.systemTypeResource
                          ).filter((x) => x.key === formData?.systemTypeId)
                        }
                        onChange={(e) => onChangeSystemType(e)}
                        onBlur={() => setInputValue("")}
                        isSearchable
                        isClearable
                        isDisabled={
                          formData?.originalEquipmentManufacturerId ==
                            undefined ||
                          formData?.originalEquipmentManufacturerId == 0 ||
                          (props.edit ? true : false)
                        }
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                        formatOptionLabel={function (data) {
                          return (
                            <span
                              dangerouslySetInnerHTML={{ __html: data.value }}
                            />
                          );
                        }}
                      ></Select>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("systemTypeId") ? (
                        <label className="validation">
                          *Design Component must have a value
                        </label>
                      ) : null}
                    </label>
                  </div>
                </div>
                <Container show={props.edit}>
                  <div className="col-12 d-flex justify-content-around align-items-center py-3">
                    <button
                      disabled={editState > 0}
                      className={`btn px-4 btnHeader asIsBtn ${
                        typeActive === 1 ? "activeAsIs" : ""
                      }`}
                      type="button"
                      onClick={() => onChangeEditState(1)}
                    >
                      Rename Network Element
                    </button>
                    <button
                      disabled={editState > 0}
                      className={`btn px-4 btnHeader asIsBtn ${
                        typeActive === 2 ? "activeAsIs" : ""
                      }`}
                      type="button"
                      onClick={() => onChangeEditState(2)}
                    >
                      Upgrade System
                    </button>
                    <button
                      disabled={editState > 0}
                      className={`btn px-4 btnHeader asIsBtn ${
                        typeActive === 3 ? "activeAsIs" : ""
                      }`}
                      type="button"
                      onClick={() => onChangeEditState(3)}
                    >
                      Patch System
                    </button>
                  </div>
                </Container>
              </div>
            </fieldset>
          </div>
          <div className="col-12">
            <Container show={!props.edit || editState == 1}>
              <fieldset className="fieldset p-0">
                <label className="text-bb mb-40 mt-50">Node Description</label>
                <div className="row">
                  <Container show={!props.edit}>
                    <div className="col-6">
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold mb-0 w-100">
                          Element Deployment Name<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.networkElementAsPlannedResource &&
                                  dictionaryToArray(
                                    formData?.networkElementAsPlannedResource
                                  )
                                }
                                value={
                                  formData &&
                                  formData?.networkElementAsPlannedResource &&
                                  dictionaryToArray(
                                    formData?.networkElementAsPlannedResource
                                  ).filter(
                                    (x) =>
                                      x.key ===
                                      formData?.networkElementAsPlannedId
                                  )
                                }
                                onChange={(e) => onChangeElementName(e)}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isDisabled={
                                  formData?.systemTypeId == undefined ||
                                  formData?.systemTypeId == 0 ||
                                  formData?.networkElementAsPlannedResource ==
                                    undefined
                                }
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
                          validation.property?.includes(
                            "elementDeploymentName"
                          ) ? (
                            <label className="validation">
                              *Element Deployment Name must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                  </Container>
                  <Container show={editState == 1}>
                    <div className="col-6">
                      <div className="col-12 pl-0 form-group">
                        <label className="labelForm voda-bold w-100 mb-0">
                          Element Deployment Name<span className="red">*</span>
                          <input
                            type="text"
                            onChange={(e) =>
                              onChange("elementDeploymentName", e)
                            }
                            className="inputForm w-100"
                            value={formData?.elementDeploymentName}
                          />
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes(
                            "elementDeploymentName"
                          ) ? (
                            <label className="validation">
                              *Element Deployment Name must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                  </Container>
                  <div className="col-6">
                    <div className="col-12 pr-0">
                      <label className="labelForm voda-bold   w-100">
                        Location <span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.locationResource &&
                                dictionaryToArray(formData?.locationResource)
                              }
                              value={
                                formData &&
                                formData?.locationResource &&
                                dictionaryToArray(
                                  formData?.locationResource
                                ).filter((x) => x.key === formData?.locationId)
                              }
                              onChange={(e) => onChangeSelect("locationId", e)}
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              isDisabled={
                                !props.edit || !tipologicaPermesso || props.edit
                              }
                              getOptionLabel={(option) => option.value ?? ""}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                              formatOptionLabel={function (data) {
                                return (
                                  <span
                                    dangerouslySetInnerHTML={{
                                      __html: data.value ?? "",
                                    }}
                                  />
                                );
                              }}
                            ></Select>
                          </div>
                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link"
                              onClick={() => setIsVisibleModalLookup(7)}
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
                        validation.property?.includes("locationId") ? (
                          <label className="validation">
                            *location Type must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                  <Container show={props.edit && editState > 0}>
                    <div className="col-12 d-flex justify-content-center align-items-center py-3">
                      <button
                        className=" br-5 voda-bold btn px-4 mx-3 btnHeader cancel"
                        type="button"
                        onClick={() => cancelEditState()}
                      >
                        Cancel
                      </button>
                      <button
                        className=" br-5 voda-bold btn btn-danger px-4 mx-3 btnHeader"
                        type="button"
                        onClick={() => setConfirmEditState()}
                      >
                        Save
                      </button>
                    </div>
                  </Container>
                </div>
              </fieldset>
            </Container>
            <Container show={!props.edit || editState == 2}>
              <div className="col-12 row px-0 mx-0">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mb-40 mt-50">
                    {props.edit ? "Upgrade System" : "Build Details"}
                  </label>
                  <div className="row">
                    <Container show={!props.edit}>
                      <div className="col-12 pl-0">
                        <div className="col-6 form-group pr-4">
                          <label className="labelForm voda-bold   w-100 mb-0">
                            Network Function
                            <input
                              type="text"
                              disabled
                              className="inputForm w-100"
                              defaultValue={formData?.networkFunction}
                            />
                          </label>
                        </div>
                      </div>
                    </Container>
                    <Container show={props.edit}>
                      <div className="col-12 pl-0">
                        <div className="col-6 form-group pr-4">
                          <label className="labelForm   w-100">
                            <label
                              className="labelForm voda-bold mb-0"
                              title={
                                formData?.systemTypeResource &&
                                dictionaryToArray(formData?.systemTypeResource)
                                  .find((x) => x.key === formData?.systemTypeId)
                                  ?.value.replace(
                                    '<b class="text-lowercase">',
                                    ""
                                  )
                                  .replace("</b>", "")
                              }
                            >
                              System Type<span className="red">*</span>
                            </label>
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.systemTypeResource &&
                                dictionaryToArray(
                                  formData?.systemTypeResource
                                ).sort((a, b) =>
                                  a.value.toLowerCase() < b.value.toLowerCase()
                                    ? -1
                                    : 1
                                )
                              }
                              value={
                                formData &&
                                formData?.systemTypeResource &&
                                dictionaryToArray(
                                  formData?.systemTypeResource
                                ).filter(
                                  (x) => x.key === formData?.systemTypeId
                                )
                              }
                              onChange={(e) => onChangeSystemType(e)}
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
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
                            {validation &&
                            validation.response == false &&
                            validation.property?.includes("systemTypeId") ? (
                              <label className="validation">
                                *Design Component must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      </div>
                    </Container>
                    <div className="col-12 mt-3">
                      <fieldset className="fieldset p-0">
                        <legend className="text-bb mb-40">
                          Hardware Details
                        </legend>
                        <div className="row">
                          <div className="col-6 labelForm">
                            <label className="labelForm voda-bold w-100 mb-0">
                              Hardware Solution
                              <div
                                className="col-12 pl-4 customFakeInput disabled"
                                style={{ marginTop: "4px" }}
                              >
                                <label
                                  className="labelForm   w-100 mb-0"
                                  dangerouslySetInnerHTML={{
                                    __html: formData?.hardwareSolution ?? "",
                                  }}
                                ></label>
                              </div>
                              {/* <input type="text" disabled className="inputForm w-100" defaultValue={formData?.hardwareSolution} /> */}
                            </label>
                          </div>
                          <div className="col-6 labelForm">
                            <label className="labelForm voda-bold w-100 mb-0">
                              Platform
                              <input
                                type="text"
                                disabled
                                className="inputForm w-100"
                                defaultValue={formData?.platform}
                              />
                            </label>
                          </div>
                          <div className="col-6 labelForm">
                            <label className="labelForm voda-bold w-100 mb-0">
                              Hardware Type
                              <div
                                className="col-12 pl-4 customFakeInput disabled"
                                style={{ marginTop: "4px" }}
                              >
                                <label
                                  className="labelForm   w-100 mb-0"
                                  dangerouslySetInnerHTML={{
                                    __html: formData?.hardwareType ?? "",
                                  }}
                                ></label>
                              </div>
                              {/* <input type="text" disabled className="inputForm w-100" defaultValue={formData?.hardwareType} /> */}
                            </label>
                          </div>
                          <div className="col-6 labelForm">
                            <label className="labelForm voda-bold w-100 mb-0">
                              Other Hardware Info
                              <input
                                type="text"
                                disabled
                                className="inputForm w-100"
                                defaultValue={formData?.otherHardwareInfo}
                              />
                            </label>
                          </div>
                          <div className="col-6 labelForm">
                            <label className="labelForm voda-bold w-100 mb-0">
                              Hardware Install Date
                              <DatePicker
                                selected={
                                  formData?.hardwareInstallDate &&
                                  new Date(formData?.hardwareInstallDate)
                                }
                                onChange={(newDate, e) => {
                                  e.preventDefault();
                                  onChangeDate("hardwareInstallDate", newDate);
                                }}
                                className="inputForm w-100"
                                minDate={new Date(1980, 0, 1)}
                                maxDate={new Date(2999, 0, 1)}
                                dateFormat="dd/MM/yyyy"
                                placeholderText={"NOT SPECIFIED"}
                              />
                            </label>
                          </div>
                        </div>
                      </fieldset>
                    </div>
                    <div className="col-12 pr-0 mt-25 mt-3">
                      <fieldset className="fieldset p-0">
                        <label className="text-bb mb-40 mt-50">
                          Software Details
                        </label>
                        <div className="row">
                          <div className="col-6 labelForm">
                            <label className="labelForm voda-bold w-100 mb-0">
                              Software Version Info
                              <span className="red">*</span>
                              <input
                                type="text"
                                disabled
                                className="inputForm w-100"
                                onChange={(e) =>
                                  onChange("softwareReleaseInformation", e)
                                }
                                defaultValue={
                                  formData?.softwareReleaseInformation
                                }
                              />
                            </label>
                          </div>
                          <div className="col-6 labelForm">
                            <label className="labelForm voda-bold w-100 mb-0">
                              Software Product Number
                              <input
                                type="text"
                                className="inputForm w-100"
                                onChange={(e) =>
                                  onChange("softwareProductNumber", e)
                                }
                                value={formData?.softwareProductNumber}
                              />
                            </label>
                          </div>
                          <div className="col-6 labelForm">
                            <label className="voda-bold w-100 mb-0">
                              Patch Details
                              <input
                                type="text"
                                onChange={(e) => onChange("patchDetails", e)}
                                className="inputForm w-100"
                                value={formData?.patchDetails}
                              />
                            </label>
                          </div>

                          <div className="col-12">
                            <div className="row">
                              <label className="labelForm form-group voda-bold col-6">
                                SW Production Date
                                <DatePicker
                                  selected={
                                    formData?.softwareProductionDate &&
                                    new Date(formData?.softwareProductionDate)
                                  }
                                  onChange={(newDate, e) => {
                                    e.preventDefault();
                                    onChangeDate(
                                      "softwareProductionDate",
                                      newDate
                                    );
                                  }}
                                  className="inputForm w-100"
                                  minDate={new Date(1980, 0, 1)}
                                  maxDate={new Date(2999, 0, 1)}
                                  dateFormat="dd/MM/yyyy"
                                  placeholderText={"NOT SPECIFIED"}
                                />
                              </label>
                              <div className="col-6">
                                <label className="labelForm voda-bold form-group w-100 mb-0">
                                  Software Install Date
                                  <DatePicker
                                    selected={
                                      formData?.softwareInstallDate &&
                                      new Date(formData?.softwareInstallDate)
                                    }
                                    onChange={(newDate, e) => {
                                      e.preventDefault();
                                      onChangeDate(
                                        "softwareInstallDate",
                                        newDate
                                      );
                                    }}
                                    className="inputForm w-100"
                                    minDate={new Date(1980, 0, 1)}
                                    maxDate={new Date(2999, 0, 1)}
                                    dateFormat="dd/MM/yyyy"
                                    placeholderText={"NOT SPECIFIED"}
                                  />
                                </label>
                              </div>
                            </div>
                          </div>
                        </div>
                      </fieldset>
                    </div>
                    <Container show={props.edit && editState > 0}>
                      <div className="col-12 d-flex justify-content-center align-items-center py-3">
                        <button
                          className="voda-bold btn px-4 mx-3 btnHeader cancel br-5"
                          type="button"
                          onClick={() => cancelEditState()}
                        >
                          Cancel
                        </button>
                        <button
                          className="voda-bold br-5 btn btn-danger px-4 mx-3 btnHeader"
                          type="button"
                          onClick={() => setConfirmEditState()}
                        >
                          Save
                        </button>
                      </div>
                    </Container>
                  </div>
                </fieldset>
              </div>
            </Container>
            <Container show={editState == 3}>
              <div className="col-12 pl-0">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mb-40 mt-50">
                    Patch System Details
                  </label>
                  <div className="row">
                    <div className="col-6 pr-0">
                      <label className="labelForm voda-bold  w-100 mb-0">
                        Software Version Information (System Level)
                        <span className="red">*</span>
                        <input
                          type="text"
                          disabled
                          className="inputForm w-100"
                          onChange={(e) =>
                            onChange("softwareReleaseInformation", e)
                          }
                          defaultValue={formData?.softwareReleaseInformation}
                        />
                      </label>
                    </div>
                    <div className="col-6">
                      <label className="labelForm voda-bold w-100 mb-0">
                        Patch Details
                        <input
                          type="text"
                          onChange={(e) => onChange("patchDetails", e)}
                          className="inputForm w-100"
                          value={formData?.patchDetails}
                        />
                      </label>
                    </div>

                    <Container show={props.edit && editState > 0}>
                      <div className="col-12 d-flex justify-content-center align-items-center py-3">
                        <button
                          className="voda-bold btn px-4 mx-3 btnHeader cancel br-5"
                          type="button"
                          onClick={() => cancelEditState()}
                        >
                          Cancel
                        </button>
                        <button
                          className=" br-5 voda-bold btn btn-danger px-4 mx-3 btnHeader"
                          type="button"
                          onClick={() => setConfirmEditState()}
                        >
                          Save
                        </button>
                      </div>
                    </Container>
                  </div>
                </fieldset>
              </div>
            </Container>
            <div className=" mt-3  mt-25">
              <fieldset className="fieldset p-0">
                <label className="text-bb mb-40 mt-50">
                  Network Element Data Acquisition Details
                </label>
                <div className="row">
                  <div className="col-6">
                    <div className="col-12 pl-0 labelForm">
                      <label className="labelForm voda-bold w-100 mb-0">
                        Data Acquisition Date
                        <DatePicker
                          selected={
                            formData?.dataAcquisitionDate &&
                            new Date(formData?.dataAcquisitionDate)
                          }
                          onChange={(newDate, e) => {
                            e.preventDefault();
                            onChangeDate("dataAcquisitionDate", newDate);
                          }}
                          // onKeyUp={(e) =>
                          //   onChangeDate("dataAcquisitionDate", e)
                          // }
                          className="inputForm w-100"
                          minDate={new Date(1980, 0, 1)}
                          maxDate={new Date(2999, 0, 1)}
                          dateFormat="dd/MM/yyyy"
                          placeholderText={"NOT SPECIFIED"}
                        />
                      </label>
                    </div>
                    <div className="col-12 pl-0">
                      {props.edit === true ? (
                        <div className="w-100 mt-50">
                          <label className="labelForm voda-bold   w-100">
                            Last Modified By
                            <input
                              readOnly={true}
                              className="inputForm w-100 voda-regular"
                              type="text"
                              defaultValue={formData?.lastModifiedBy}
                            />
                          </label>
                        </div>
                      ) : null}
                    </div>
                  </div>
                  <div className="col-6">
                    <div className="col-12 pr-0">
                      <label className="labelForm voda-bold w-100 mb-0">
                        Element Manager
                        <input
                          type="text"
                          className="inputForm w-100"
                          onChange={(e) => onChange("elementManager", e)}
                          value={formData?.elementManager}
                        />
                      </label>
                    </div>
                    <div className="col-12 pr-0">
                      {props.edit === true ? (
                        <div className="w-100">
                          <label className="labelForm voda-bold mt-50  w-100">
                            Last Modified
                            <input
                              readOnly={true}
                              className="inputForm w-100 voda-regular"
                              type="text"
                              defaultValue={formatDateWithTime(
                                formData?.lastModified
                              )?.toUpperCase()}
                            />
                          </label>
                        </div>
                      ) : null}
                    </div>
                  </div>
                </div>
              </fieldset>
            </div>
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
                opCoResource: {},
                originalEquipmentManufacturerResource: {},
                networkElementAsPlannedResource: {},
                systemTypeResource: {},
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
          Save
        </button>
      </div>
    </div>
  );
};

export default NetworkElementAsIsModal;
