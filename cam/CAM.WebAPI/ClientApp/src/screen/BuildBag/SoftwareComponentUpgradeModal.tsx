import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  formatTime,
  changeDate,
  changeText,
  lowerFirstLetter,
  numberIsNullOrZero,
} from "../../Hook/Common";
import {
  ComponentSwBuildToCloneDto,
  CloneComponentSwBuildDto,
} from "../../Model/ComponentSwBuild";
import ModalConfirm from "../../Components/ModalConfirm";
import {
  DataModalConfirm,
  rtnConfirmMessage,
  stateConfirm,
} from "../../Model/Common";
import {
  CloneComponentSw,
  GetComponentSwClonePreSubmit,
} from "../../Redux/Action/ComponentSwBuild/ComponentSwBuildCommonAction";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  dictionaryToArray,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import DatePicker from "react-datepicker";
import { Form } from "react-bootstrap";
import Select from "react-select";
import { BuildBagDtoUpdate } from "../../Model/BuildBag";
import {
  DropdownInputComponent,
  MultiSelectComponent,
} from "../../Components/FormField";
import { SoftwareComponentUpgrade } from "../../Redux/Action/BuildBag/BuildBagCommonAction";
import { useNavigate } from "react-router";
import { MdDelete, MdOutlineClear } from "react-icons/md";
import { IoIosRefresh } from "react-icons/io";

interface Props {
  action: {
    setIsVisibleModalUpgrade(value: boolean): any;
    refresh(): any;
  };
  data: ComponentSwBuildToCloneDto | undefined;
}

const ComponentSwModalUpgrade: React.FC<Props> = (props) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<BuildBagDtoUpdate>();
  const [disabledDate, setDisabledDate] = useState<boolean>(false);
  const [descriptionError, setDescriptionError] = useState<boolean>(false);
  const [upgradeResource, setUpgradeResource] = useState<any>([]);
  const [isUpgrade, setIsUpgrade] = useState<boolean>(false);
  const [releventConponentIds, setReleventConponentIds] = useState<
    { componentIds: number; releventIds: number }[]
  >([]);
  const [upgradeId, setUpgradeId] = useState<number | null>(null);
  const [componentIds, setComponentIds] = useState<any>([]);
  const [editDescription, setEditDescription] = useState<any>([]);
  const [dataConfirm, setDataConfirm] =
    useState<DataModalConfirm>(stateConfirm);
  const [changed, setChanged] = useState<boolean>(false);
  const [data, setData] = useState<ComponentSwBuildToCloneDto>();
  const [disabledEoSDate, setDisabledEoSDate] = useState<boolean>(false);
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);

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

  useEffect(() => {
    if (props.data != null) {
      setData(props.data);
      setEditDescription(props.data?.["buildBagDescription"]?.split("-"));
      setComponentIds(
        props.data?.["componentBuildBagEditPageDto"]?.[
          "upgradeComponentSoftwareDetails"
        ]?.map((res: any) => {
          return res?.componentSoftwareBuildId;
        }) ?? []
      );
      setUpgradeResource(
        props.data?.["componentBuildBagEditPageDto"]?.[
          "upgradeComponentSoftwareDetails"
        ]
      );
      setFormData({
        ...formData,
        buildBagId: props.data?.["buildBagId"],
        opCoId:props.data?.opCoId,
        designComponentFamilyId:props.data?.designComponentFamilyId,
        componentSofwareBuild:
          props.data?.["componentBuildBagEditPageDto"]?.[
            "existingComponentSwBuild"
          ],
        buildBagDescription:
          props.data?.["componentBuildBagEditPageDto"]?.[
            "componentBagDescription"
          ],
      });
    }
  }, [props.data]);

  const validazioneClient = (copy: CloneComponentSwBuildDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    // if (
    //   copy?.softwareVersion == null ||
    //   copy?.softwareVersion === undefined ||
    //   copy?.softwareVersion === ""
    // ) {
    //   addInvalidProperty("softwareVersion");
    // }
    // if (copy?.endOfMaintenance === undefined) {
    //   addInvalidProperty("endOfMaintenance");
    // }
    // if (
    //   (copy?.endOfsupport == null || copy?.endOfsupport === undefined) &&
    //   !disabledEoSDate
    // ) {
    //   addInvalidProperty("endOfsupport");
    // }
    // if (
    //   copy?.designContactIds === undefined ||
    //   copy?.designContactIds === null ||
    //   copy?.designContactIds.length === 0
    // ) {
    //   addInvalidProperty("designContactIds");
    // }
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

  const confirmSubmit = async () => {
    const newFormData = {
      ...formData,
      buildBagDescription: editDescription?.join("-"),
      componentSoftwareId: [
        ...releventConponentIds?.map((res) => res.releventIds),
        ...upgradeResource
          ?.map((res) => res?.componentSoftwareBuildId)
          ?.filter(
            (id) => !releventConponentIds.some((obj) => obj.componentIds === id)
          ),
      ],
    };
    await SoftwareComponentUpgrade(newFormData).then((x) => {
      if (x && !x.warning) {
        setDataConfirm(stateConfirm);
        refresh();
      }
    });
  };

  const handleMultiSelect = (selected) => {
    setComponentIds(selected.map((val) => val.value));
  };

  const handleRedirectSwComponent = (e, parentKey) => {
    setReleventConponentIds((prev) => {
      if (!e) {
        // console.log("Cleared selection");
        return prev.filter((id) => id.componentIds !== parentKey);
      }

      // console.log("Selected:", e);
      return prev.some((val) => val.componentIds === parentKey)
        ? prev.map((val) =>
            val.componentIds === parentKey
              ? { ...val, releventIds: e.key }
              : val
          )
        : [...prev, { componentIds: parentKey, releventIds: e.key }];
    });
  };

  const getUgradedValue = (item) => {
    const relevantResources = resourceArrayRefactor(
      upgradeResource?.find(
        (res) => res.componentSoftwareBuildId === item.componentSoftwareBuildId
      )?.relavantComponetSwBuild
    );

    const selectedIds = releventConponentIds
      ?.filter((val) => val.componentIds === item?.componentSoftwareBuildId)
      ?.map((val) => val.releventIds);

    return (
      relevantResources?.find((res) => selectedIds?.includes(res.key)) ?? null
    );
  };
  return (
    <div className="col-12">
      <ModalConfirm data={dataConfirm} />
      <form id="formSoftwareBuild" onChange={() => setChanged((prev) => !prev)}>
        <div className="row">
          <fieldset className="fieldset">
            <label className="text-bb">Bag Details</label>
            <div className="row">
              <div className="form-group col-12">
                <div className="col-6 pl-0">
                  <label className={"labelForm  voda-bold w-100 mb-1"}>
                    {"Bag Name"}
                    <span className="red">*</span>
                  </label>
                  <label className={`labelForm voda-bold `}>
                    <div
                      style={{
                        display: "flex",
                        alignItems: "center",
                        border: "1px solid #ccc",
                        borderRadius: "4px",
                        width: "100% !important",
                      }}
                    >
                      {editDescription?.length ? (
                        <span
                          style={{
                            padding: "10.5px 10px",
                            height: "2.8rem",
                            width: "100%",
                            background: "#eee", // Same as disabled input background
                            color: "#6c757d", // Disabled text color
                          }}
                        >
                          {editDescription?.join("-")}
                        </span>
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
            {/* <div className="row">
              <div className="form-group col-6">
                <div className="col-12 pl-0">
                  <MultiSelectComponent
                    label={"Select Software Components"}
                    labelCSS="mb-0"
                    inputCSS="labelForm voda-bold mb-2"
                    required={false}
                    isAdd={true}
                    onAddClicked={() => navigate("/componentswbuild")}
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
                </div>
              </div>
            </div> */}
          </fieldset>
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
                  <th
                    className="px-2 ptb-12"
                    style={{
                      textAlign: "right",
                    }}
                  >
                    Upgrade
                  </th>
                </tr>
              </thead>
              <tbody>
                {upgradeResource?.map((item, i) => (
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
                    <td
                      className="d-flex"
                      style={{
                        maxWidth: "unset",
                        placeItems: "center",
                        justifySelf: "end",
                      }}
                    >
                      {item?.componentSoftwareBuildId !== upgradeId &&
                        upgradeResource
                          ?.filter(
                            (res) =>
                              res.componentSoftwareBuildId ===
                              item.componentSoftwareBuildId
                          )[0]
                          ?.["relavantComponetSwBuild"]?.filter((res) => {
                            const checkKey = releventConponentIds
                              ?.filter(
                                (res) =>
                                  res.componentIds ===
                                  item.componentSoftwareBuildId
                              )
                              ?.map((res) => res.releventIds)[0];
                            if (res.key === checkKey) {
                              return res;
                            }
                          })[0]?.text}
                      {isUpgrade &&
                      item?.componentSoftwareBuildId === upgradeId ? (
                        <Select
                          key={item.componentSoftwareBuildId}
                          menuPortalTarget={document.body}
                          menuPosition={"fixed"}
                          className="w-100 text-left"
                          styles={{
                            control: (baseStyles) => ({
                              ...baseStyles,
                              width: "20rem !important",
                            }),
                            menuPortal: (base) => ({
                              ...base,
                              zIndex: 9999, // ensure it's on top
                            }),
                          }}
                          options={
                            resourceArrayRefactor(
                              upgradeResource?.find(
                                (res) =>
                                  res.componentSoftwareBuildId ===
                                  item.componentSoftwareBuildId
                              )?.relavantComponetSwBuild
                            )?.map((res) => ({
                              ...res,
                              isDisabled: releventConponentIds?.some(
                                (id) => id.releventIds === res.key
                              ),
                            })) ?? []
                          }
                          value={getUgradedValue(item) ?? null}
                          onChange={(e) =>
                            handleRedirectSwComponent(
                              e,
                              item?.componentSoftwareBuildId
                            )
                          }
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option.key?.toString()}
                          formatOptionLabel={({ value }) => (
                            <span dangerouslySetInnerHTML={{ __html: value }} />
                          )}
                        />
                      ) : null}
                      <button
                        type="button"
                        title={`${
                          isUpgrade &&
                          item.componentSoftwareBuildId === upgradeId
                            ? "Clear"
                            : "Upgrade"
                        }`}
                        className="btn btn-link"
                        onClick={() => {
                          setIsUpgrade((prev) => !prev);
                          setUpgradeId(
                            isUpgrade ? null : item?.componentSoftwareBuildId
                          );
                        }}
                      >
                        {isUpgrade &&
                        item.componentSoftwareBuildId === upgradeId ? (
                          <MdOutlineClear color={"black"} />
                        ) : (
                          <IoIosRefresh color={"black"} />
                        )}
                      </button>
                      {/* <img
                        onClick={() => {
                          // console.log(
                          //   releventConponentIds
                          //     ?.map((res) => res.componentIds)
                          //     .includes(item?.componentSoftwareBuildId)
                          // );
                          setIsUpgrade((prev) => !prev);
                          setUpgradeId(
                            isUpgrade ? null : item?.componentSoftwareBuildId
                          );
                        }}
                        className="btnEdit op-55 ml-2"
                        src={
                          isUpgrade &&
                          item.componentSoftwareBuildId === upgradeId
                            ? require("../../img/deleteIcon.png")
                            : require("../../img/update.png")
                        }
                      /> */}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
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
          onClick={() => confirmSubmit()}
          type="button"
          disabled={releventConponentIds?.length > 0 ? false : true}
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default ComponentSwModalUpgrade;
