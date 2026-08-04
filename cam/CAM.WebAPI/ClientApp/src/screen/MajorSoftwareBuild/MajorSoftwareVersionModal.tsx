import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  formatTime,
  changeDate,
  changeText,
  lowerFirstLetter,
} from "../../Hook/Common";
import {
  MajorSoftwareBuildToCloneDto,
  CloneMajorSoftwareBuildDto,
} from "../../Model/MajorSoftwareBuild";
import ModalConfirm from "../../Components/ModalConfirm";
import {
  DataModalConfirm,
  rtnConfirmMessage,
  stateConfirm,
} from "../../Model/Common";
import {
  CloneMajorSoftware,
  GetMajorSoftwareClonePreSubmit,
} from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCommonAction";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { dictionaryToArray } from "../../Hook/Dictionary";
import DatePicker from "react-datepicker";
import { Form } from "react-bootstrap";
import Select from "react-select";
import { useDispatch, useSelector } from "react-redux";
import TourGuide from "../../Components/TourGuide";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { startGuideTour, endGuideTour } from "../../Redux/Action/tourActions";
import { getMajorSwVersionModalTourSteps } from "../../Constant/TourSteps";

interface Props {
  action: {
    setIsVisibleModalUpgrade(value: boolean): any;
    refresh(): any;
  };
  data: MajorSoftwareBuildToCloneDto | undefined;
}

const MajorSoftwareModalUpgrade: React.FC<Props> = (props) => {
  const [formData, setFormData] = useState<CloneMajorSoftwareBuildDto>();
  const [disabledDate, setDisabledDate] = useState<boolean>(false);

  const [dataConfirm, setDataConfirm] =
    useState<DataModalConfirm>(stateConfirm);
  const [changed, setChanged] = useState<boolean>(false);
  const [data, setData] = useState<MajorSoftwareBuildToCloneDto>();
  const [disabledEoSDate, setDisabledEoSDate] = useState<boolean>(false);
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);

  const dispatch = useDispatch();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const handleEndTour = () => dispatch(endGuideTour());
  const cancelConfirm = {
    title: "Exit",
    button: "Exit",
    message: "Are you sure you want to exit?, Unsaved changes will be lost.",
    item: "",
    isOpen: true,
    actions: {
      cancel: () => setDataConfirm(stateConfirm),
      confirm: () => closeModal(false),
    },
  } as DataModalConfirm;

  const onHandelChangeAnnounced = (e: boolean) => {
    let copy = { ...formData } as CloneMajorSoftwareBuildDto;
    if (e) {
      setDisabledDate(true);
      copy.eomStatus = 0;
      copy.endOfMaintenance = null;
      if (disabledEoSDate) {
        copy.endOfsupport = null;
      }
      setFormData(copy);
    } else {
      setDisabledDate(false);
      copy.eomStatus = 1;
      setFormData(copy);
    }
  };

  const onHandleCopyEoM = (e: boolean) => {
    let copy = { ...formData } as CloneMajorSoftwareBuildDto;
    if (e && copy) {
      setDisabledEoSDate(true);
      copy.endOfsupport = (copy?.endOfMaintenance as Date) ?? null;
      //copy.endOfMaintenance = null;
      setFormData(copy);
    } else {
      setDisabledEoSDate(false);
      //copy.endOfsupport = null;
      setFormData(copy);
    }
  };

  useEffect(() => {
    if (props.data != null) {
      setData(props.data);
      let copy = { ...formData } as CloneMajorSoftwareBuildDto;
      copy.majorSoftwareBuildsId = props.data.majorSoftwareBuildsId;
      copy.tcpBundleVersion = props.data.tcpBundleVersion;
      copy.tciBundleVersion = props.data.tciBundleVersion;
      copy.designContactIds = props.data.designContactIds;
      copy.designContacts = props.data.designContacts;
      copy.tcpSoftwareCompatibilityIdList =
        props.data.tcpSoftwareCompatibilityIdList;
      copy.tciSoftwareCompatibilityIdList =
        props.data.tciSoftwareCompatibilityIdList;
      copy.eomStatus = 1;
      setFormData(copy);
    }
  }, [props.data]);

  const validazioneClient = (copy: CloneMajorSoftwareBuildDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.softwareVersion == null ||
      copy?.softwareVersion === undefined ||
      copy?.softwareVersion === ""
    ) {
      addInvalidProperty("softwareVersion");
    }
    if (copy?.endOfMaintenance === undefined) {
      addInvalidProperty("endOfMaintenance");
    }
    if (
      (copy?.endOfsupport == null || copy?.endOfsupport === undefined) &&
      !disabledEoSDate
    ) {
      addInvalidProperty("endOfsupport");
    }
    if (
      copy?.designContactIds === undefined ||
      copy?.designContactIds === null ||
      copy?.designContactIds.length === 0
    ) {
      addInvalidProperty("designContactIds");
    }
    // if (formData && formData.originalEquipmentManufacturer !== "VMWare") {
    //   if (
    //     copy?.tcpSoftwareCompatibilityIdList == null ||
    //     copy?.tcpSoftwareCompatibilityIdList === undefined ||
    //     copy?.tcpSoftwareCompatibilityIdList.length === 0
    //   ) {
    //     addInvalidProperty("tcpSoftwareCompatibilityIdList");
    //   }
    //   if (
    //     copy?.tciSoftwareCompatibilityIdList == null ||
    //     copy?.tciSoftwareCompatibilityIdList === undefined ||
    //     copy?.tciSoftwareCompatibilityIdList.length === 0
    //   ) {
    //     addInvalidProperty("tciSoftwareCompatibilityIdList");
    //   }
    // }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = () => {
    props.action.refresh();
    props.action.setIsVisibleModalUpgrade(false);
  };

  //REFRESH DATI PAGINA
  const closeModal = (changed: boolean) => {
    if (changed) {
      setDataConfirm(cancelConfirm);
    } else {
      refresh();
    }
  };

  useEffect(() => {
    if (data && data.isvmware) {
      let copy = { ...formData } as CloneMajorSoftwareBuildDto;
      copy.tcpSoftwareCompatibilityIdList = [];
      copy.tciSoftwareCompatibilityIdList = [];
      setFormData(copy);
    }
  }, [data?.isvmware]);

  const setConfirmSubmit = async () => {
    let copy = { ...formData } as CloneMajorSoftwareBuildDto;

    if (validazioneClient(copy).response == true) {
      if (data && data.majorSoftwareBuildsId) {
        await GetMajorSoftwareClonePreSubmit(
          data?.majorSoftwareBuildsId,
          false
        ).then((x) => {
          if (x && x != undefined) {
            const SubmitConfirm = {
              title: "Confirm",
              button: "Confirm",
              message: rtnConfirmMessage(x?.info ?? ""),
              item: "",
              isOpen: true,
              actions: {
                cancel: () => setDataConfirm(stateConfirm),
                confirm: () => confirmSubmit(),
              },
            } as DataModalConfirm;
            setDataConfirm(SubmitConfirm);
          }
        });
      }
    }
  };

  const confirmSubmit = async () => {
    if (formData && formData.majorSoftwareBuildsId) {
      await CloneMajorSoftware(formData).then((x) => {
        if (x && !x.warning) {
          setDataConfirm(stateConfirm);
          refresh();
        }
      });
    }
  };

  const onChange = (property: string, e: any) => {
    let copy = { ...formData } as CloneMajorSoftwareBuildDto;
    copy[property] = e.target.value;
    setFormData(copy);
  };

  const onChangeDate = (property: string, newDate: Date | null) => {
    let copy = { ...formData } as CloneMajorSoftwareBuildDto;

    if (property === "endOfMaintenance") {
      if (newDate !== null) {
        copy["eomStatus"] = 2;
      } else {
        copy["eomStatus"] = 0;
      }
    }

    if (property === "endOfsupport") {
      if (newDate !== null) {
        copy["endOfsupport"] = newDate;
      }
    }

    if (newDate == null) {
      copy[lowerFirstLetter(property)] = null;
    } else {
      const DateString = ` ${newDate.getFullYear()}/${
        newDate.getMonth() + 1
      }/${newDate.getDate()}`;
      copy[lowerFirstLetter(property)] = DateString;
    }
    if (disabledEoSDate) {
      copy["endOfsupport"] = (copy?.endOfMaintenance as Date) ?? null;
    }

    setChanged(true);

    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };
  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as CloneMajorSoftwareBuildDto;
    if (e !== null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = undefined;
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };
  return (
    <div className="col-12">
      <ModalConfirm data={dataConfirm} />
      <form id="formSoftwareBuild" onChange={() => setChanged(true)}>
        <div className="row">
          <fieldset className="col-12 row mx-0">
            <legend className="text-bb">Based on</legend>
            <div className="col-12 px-0 row mx-0">
              <div className="form-group col-6 pl-0">
                <label className="labelForm voda-bold w-100">
                  Equipment Manufacturer
                </label>
                <label>{data && data.originalEquipmentManufacturer}</label>
              </div>
              <div className="form-group col-6 pr-0">
                <label className="labelForm voda-bold w-100">
                  Sw Application Type
                </label>
                <label>{data && data.productName}</label>
              </div>
              <div className="form-group col-12 pl-0">
                <label className="labelForm voda-bold w-100 pl-0">
                  Release No.
                </label>
                <label>{data && data.softwareVersion}</label>
              </div>
            </div>
          </fieldset>
          <fieldset className="col-12 row mx-0">
            <legend className="text-bb">Used in</legend>
            <div className="col-12 ptb-0">
              <table
                className="table table-borderless upgrade-table"
                style={{ filter: "none", width: "100%", margin: "0 auto" }}
              >
                <tbody>
                  {data &&
                  data.designComponentResource &&
                  dictionaryToArray(data.designComponentResource).length > 0 ? (
                    dictionaryToArray(data.designComponentResource).map((x) => (
                      <tr>
                        <td>
                          <label
                            className="mb-0"
                            dangerouslySetInnerHTML={{
                              __html:
                                x.value === "" || x.value == null
                                  ? "---"
                                  : x.value,
                            }}
                          ></label>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr className=" ">
                      <td>
                        <label className="mb-0">
                          The record selected it's not used in any Design
                          Component
                        </label>
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </fieldset>
          <fieldset className="col-12 row mx-0 mt-4">
            <legend className="text-bb">What is the Upgraded Software?</legend>
            <div className="row">
              <div className="col-6 pl-0">
                <div className="col-12">
                  <label
                    className="labelForm voda-bold w-100"
                    id="majorSwModal_swVersion_tour"
                  >
                    What is the Software Version Number?
                    <span className="red fz-20">*</span>
                    <input
                      onChange={(e) => onChange("softwareVersion", e)}
                      type="text"
                      className="inputForm w-100"
                      value={formData?.softwareVersion}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("softwareVersion") ? (
                      <label className="validation">
                        *Software Application must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="col-12" id="majorSwModal_endOfMain_tour">
                  <span
                    className="fz-16 voda-bold pr-0"
                    style={{ display: "block" }}
                  >
                    End of Maintenance
                    <span className="red fz-20">*</span>
                  </span>
                  <div className="flex">
                    <label className="labelForm voda-bold sp w-100 flex-basis-60">
                      <DatePicker
                        selected={
                          formData?.endOfMaintenance &&
                          new Date(formData?.endOfMaintenance)
                        }
                        onChange={(newDate, e) => {
                          e.preventDefault();
                          onChangeDate("endOfMaintenance", newDate);
                        }}
                        className="inputForm w-100"
                        minDate={new Date(1980, 0, 1)}
                        maxDate={new Date(2999, 0, 1)}
                        dateFormat="dd/MM/yyyy"
                        placeholderText={
                          formData?.eomStatus === 0
                            ? "NOT ANNOUNCED"
                            : "NOT SPECIFIED"
                        }
                        disabled={disabledDate}
                      />
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("endOfMaintenance") ? (
                        <label className="validation">
                          *Cannot be Empty.Please select Not Announced
                        </label>
                      ) : null}
                    </label>
                    <Form.Check
                      type="checkbox"
                      className="radio labelForm voda w-100 flex-basis-40"
                      name="userLogin"
                      value="1"
                      label="Not Announced"
                      checked={formData?.eomStatus === 0 ? true : false}
                      onChange={(e: any) =>
                        onHandelChangeAnnounced(e.target.checked)
                      }
                    />
                  </div>
                  {/* <label className="labelForm voda-bold w-100">
                    <div className="abso">
                      <Form.Check
                        type="checkbox"
                        className="radio labelForm voda-bold w-100 cpt"
                        name="userLogin"
                        value="1"
                        label="Not Announced"
                        onChange={(e: any) =>
                          onHandelChangeAnnounced(e.target.checked)
                        }
                      />
                    </div>
                    What is the end of maintenance date?
                    <DatePicker
                      selected={
                        formData?.endOfMaintenance &&
                        new Date(formData?.endOfMaintenance)
                      }
                      onChange={(newDate, e) => {
                        e.preventDefault();
                        onChangeDate("endOfMaintenance", newDate);
                      }}
                      className="inputForm w-100"
                      minDate={new Date(1980, 0, 1)}
                      maxDate={new Date(2999, 0, 1)}
                      dateFormat="dd/MM/yyyy"
                      placeholderText={
                        formData?.eomStatus === 0
                          ? "NOT ANNOUNCED"
                          : "NOT SPECIFIED"
                      }
                      disabled={disabledDate}
                    />
                  </label> */}
                </div>
              </div>
              <div className="col-6 pl-0">
                <div className="col-12" id="majorSwModal_endOfSupport_tour">
                  <label className="voda-bold w-100">
                    End of Support
                    {<span className="red">*</span>}
                    <DatePicker
                      selected={
                        formData?.endOfsupport &&
                        new Date(formData?.endOfsupport)
                      }
                      onChange={(newDate, e) => {
                        e.preventDefault();
                        onChangeDate("endOfsupport", newDate);
                      }}
                      className="inputForm w-100 "
                      minDate={new Date(1980, 0, 1)}
                      maxDate={new Date(2999, 0, 1)}
                      dateFormat="dd/MM/yyyy"
                      placeholderText={
                        formData?.eomStatus === 0 && disabledEoSDate
                          ? "NOT ANNOUNCED"
                          : "NOT SPECIFIED"
                      }
                      disabled={disabledEoSDate}
                    />
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("endOfsupport") ? (
                      <label className="validation">
                        *End Of Support cannot be empty.
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>

              <div className="col-6">
                <div className="col-12" id="majorSwModal_endOfMain_tour">
                  <div className="col-6">
                    <label className="voda-bold w-100">
                      <Form.Check
                        type="checkbox"
                        className="radio labelForm voda w-100 mt-35 flex-basis-40"
                        name="userLogin"
                        value="1"
                        label="Same as End Of Maintenance"
                        checked={disabledEoSDate}
                        onChange={(e: any) => onHandleCopyEoM(e.target.checked)}
                      />
                    </label>
                  </div>
                </div>
              </div>

              <div className="col-6 pl-0">
                <div className="col-12">
                  <label className="voda-bold w-100 mt-2">
                    Design Contact
                    <span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                          menuPosition={"fixed"}
                          options={
                            formData?.designContacts &&
                            dictionaryToArray(formData.designContacts)
                          }
                          value={
                            formData?.designContacts &&
                            dictionaryToArray(formData?.designContacts).filter(
                              (x) => formData?.designContactIds?.includes(x.key)
                            )
                          }
                          onChange={(e) => {
                            OnChangeMultiSelect("designContactIds", e);
                          }}
                          isMulti
                          // onBlur={() => setInputValue("")}
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
                        {validation &&
                        validation.response == false &&
                        validation.property?.includes("designContactIds") ? (
                          <label className="validation">
                            *Design Contact must have a value
                          </label>
                        ) : null}
                      </div>
                    </div>
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div
                  className="switchContainer d-flex flex-row align-items-center"
                  style={{ marginTop: "30px" }}
                >
                  <label className="switch mr-2">
                    <input
                      type="checkbox"
                      checked={data?.isvmware ?? false}
                      disabled
                    />
                    <span
                      className="slider round"
                      style={{
                        opacity: 0.5,
                        cursor: "not-allowed",
                      }}
                    ></span>
                  </label>
                  Is this a Platform Software (Eg: Broadcom)?
                </div>
              </div>
            </div>
          </fieldset>
          {data && !data.isvmware && (
            <fieldset className="fieldset mb-4 pb-4">
              <legend className="text-bb">
                VmWare Compatibility Attribute
              </legend>
              <div className="row">
                <div className="col-6 pl-0">
                  <div className="col-12">
                    <label className="voda-bold w-100 mt-2">
                      TCI
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={
                              formData?.tciBundleVersion &&
                              dictionaryToArray(formData?.tciBundleVersion)
                            }
                            value={
                              formData?.tciBundleVersion &&
                              dictionaryToArray(
                                formData?.tciBundleVersion
                              ).filter((el) =>
                                formData.tciSoftwareCompatibilityIdList?.includes(
                                  el.key
                                )
                              )
                            }
                            onChange={(e) =>
                              OnChangeMultiSelect(
                                "tciSoftwareCompatibilityIdList",
                                e
                              )
                            }
                            // onKeyUp={(e) => OnChangeMultiSelect("tciSoftwareCompatibilityIdList", e)}
                            // onBlur={() => setInputValue("")}
                            isSearchable
                            isMulti
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          ></Select>
                        </div>
                      </div>
                      {/* {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "tciSoftwareCompatibilityIdList"
                        ) ? (
                          <label className="validation h-16">
                            *TCP must have a value
                          </label>
                        ) : null} */}
                    </label>
                  </div>
                </div>
                <div className="col-6 pr-0">
                  <div className="col-12">
                    <label className="voda-bold w-100 mt-2">
                      TCP
                      <div className="d-flex">
                        <div className="w-100">
                          <Select
                            menuPosition={"fixed"}
                            options={
                              formData?.tcpBundleVersion &&
                              dictionaryToArray(formData?.tcpBundleVersion)
                            }
                            value={
                              formData?.tcpBundleVersion &&
                              dictionaryToArray(
                                formData?.tcpBundleVersion
                              ).filter((el) =>
                                formData.tcpSoftwareCompatibilityIdList?.includes(
                                  el.key
                                )
                              )
                            }
                            onChange={(e) =>
                              OnChangeMultiSelect(
                                "tcpSoftwareCompatibilityIdList",
                                e
                              )
                            }
                            // onKeyUp={(e) => OnChangeMultiSelect("tcpSoftwareCompatibilityIdList", e)}
                            // onBlur={() => setInputValue("")}
                            isSearchable
                            isMulti
                            getOptionLabel={(option) => option.value.toString()}
                            getOptionValue={(option) =>
                              option["key"].toString()
                            }
                          ></Select>
                        </div>
                      </div>
                      {/* {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "tcpSoftwareCompatibilityIdList"
                        ) ? (
                          <label className="validation h-16">
                            *TCP must have a value
                          </label>
                        ) : null} */}
                    </label>
                  </div>
                </div>
              </div>
            </fieldset>
          )}
        </div>
      </form>

      <div className="col-12 justify-content-end d-flex footerModal">
        <button
          className="voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => setConfirmSubmit()}
          type="button"
          id="majorSwModal_submit_tour"
        >
          Submit
        </button>
      </div>
      {tourStarted && (
        <TourGuide
          start={tourStarted}
          tourSteps={getMajorSwVersionModalTourSteps}
          page={"majorSwModal"}
          setStartTour={(val: boolean) =>
            dispatch(val ? startGuideTour() : endGuideTour())
          }
          onTourEnd={handleEndTour}
        />
      )}
    </div>
  );
};

export default MajorSoftwareModalUpgrade;

// Date.prototype.formatMMDDYYYY = function(){
//     return (this.getMonth() + 1) +
//     "/" +  this.getDate() +
//     "/" +  this.getFullYear();
// }
