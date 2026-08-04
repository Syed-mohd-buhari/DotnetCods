import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/InputRange.css";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import Slider from "react-rangeslider";

import {
  dictionaryToArray,
  dictionaryToArrayPlannedActivityToConnect,
  dictionaryToArrayPlannedActivityToConnectData,
} from "../../Hook/Dictionary";
import ModalConfirm from "../../Components/ModalConfirm";
import Select from "react-select";
import {
  GetDesignComponentListForPlannedToConnect,
  GetOpcoListForPlannedToConnect,
  GetPlannedActivityForLink,
  GetPlannedActivityListForPlannedToConnect,
  SubmitPlannedActivityMigration,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCommonAction";
import {
  PlannedActivityForLinkDto,
  PlannedActivityMigrationsDto,
} from "../../Model/PlannedActivity";
import { rootStore } from "../../Redux/Store/rootStore";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { NetworkElementAssociated } from "../../Model/LcmEngineering";

interface Props {
  action: {
    setIsVisibleModalManage(val: boolean): any;
    changeNodesFromModal?(newNodesValue: number): any;
    callBackForRefreshGetUpdate?(id: number): any;
  };
  plannedActivityId: number | undefined;
  isFromPlannedActivityModal: boolean;
  lcmId?: number;
}

const ManageMigration: React.FC<Props> = (props) => {
  const [changed, setChanged] = useState(false);
  const [nOfNodeOfLinkedPlanned, setNOfNodeOfLinkedPlanned] =
    useState<number>(0);
  const [nOfLabNodeOfLinkedPlanned, setNOfLabNodeOfLinkedPlanned] =
    useState<number>(0);
  const [nOFNodeOfDesignComponent, setNOFNodeOfDesignComponent] =
    useState<number>(0);
  const [nOFMaxNode, setNOFMaxNode] = useState<number>(0);
  const [nOFMaxLabNode, setNOFMaxLabNode] = useState<number>(0);
  const [plannedActivityId, setPlannedActivityId] = useState<
    number | undefined
  >();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [formData, setFormData] = useState<PlannedActivityForLinkDto>();
  const [dataSubmit, setDataSubmit] = useState<PlannedActivityMigrationsDto>();
  const [inputValue, setInputValue] = useState<string>("");

  const [startSelected, setStartSelected] = useState<number[]>([]);
  const [endSelected, setEndSelected] = useState<number[]>([]);
  const [allowLabNodes, setAllowLabNodes] = useState<boolean>(true);

  useEffect(() => {
    let copy = { ...formData } as PlannedActivityForLinkDto;
    setPlannedActivityId(plannedActivityId);
    if (props.plannedActivityId == undefined) {
      GetOpcoList(copy);
    } else {
      let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
      copySubmit.plannedActivityId = props.plannedActivityId;
      setDataSubmit(copySubmit);
      GetPlannedActivityToConnect(props.plannedActivityId, copy);
    }
  }, []);

  const GetOpcoList = async (copy: PlannedActivityForLinkDto) => {
    await GetOpcoListForPlannedToConnect().then((x) => {
      if (x && x != undefined) {
        copy.opCoResource = x.data;
        copy.designComponentId = undefined;
      }
    });
    setFormData(copy);
  };

  const GetPlannedActivityToConnect = async (
    id: number,
    copy: PlannedActivityForLinkDto
  ) => {
    await GetPlannedActivityForLink(id).then((x) => {
      if (x && x !== undefined) {
        copy.designComponentResource = x.designComponentResource;
        copy.designComponentId = x.designComponentId;
        copy.numberOfNodes = x.numberOfNodes;
        copy.numberOfLabNodes = x.numberOfLabNodes;
        copy.opCoId = x.opCoId;
        copy.elementCount = x.elementCount;
        copy.startNetworkElementAssociateds = x.startNetworkElementAssociateds;
        copy.endNetworkElementAssociateds = x.endNetworkElementAssociateds;
        copy.opCoResource = x.opCoResource;
        copy.plannedActivityId = x.plannedActivityId;
        copy.plannedActivityResource = x.plannedActivityResource;

        if (copy.plannedActivityResource) {
          let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
          if (
            dictionaryToArrayPlannedActivityToConnect(
              copy.plannedActivityResource
            ).length == 1
          ) {
            copySubmit.plannedActivityId = x.plannedActivityId;

            copySubmit.numberOfNodes = copy.plannedActivityResource
              ? dictionaryToArrayPlannedActivityToConnect(
                  copy.plannedActivityResource
                )[0].value.numberOfNodes
              : 0;

            copySubmit.numberOfLabNodes = copy.plannedActivityResource
              ? dictionaryToArrayPlannedActivityToConnect(
                  copy.plannedActivityResource
                )[0].value.numberOfLabNodes
              : 0;

            setNOfNodeOfLinkedPlanned(copySubmit.numberOfNodes ?? 0);
            setNOfLabNodeOfLinkedPlanned(copySubmit.numberOfLabNodes ?? 0);
            setDataSubmit(copySubmit);
          }

          const NofNodeLCM = copy.numberOfNodes;
          const NoofLabNodeLcm = copy.numberOfLabNodes;

          if (
            copySubmit.numberOfNodes !== undefined &&
            NofNodeLCM !== undefined
          ) {
            const max = NofNodeLCM + copySubmit.numberOfNodes;
            setNOFMaxNode(max);
          }

          if (
            copySubmit.numberOfLabNodes !== undefined &&
            NoofLabNodeLcm !== undefined
          ) {
            const max = NoofLabNodeLcm + copySubmit.numberOfLabNodes;
            setNOFMaxLabNode(max);
          }
        }
      }

      setNOFNodeOfDesignComponent(copy.numberOfNodes ?? 0);

      setFormData(copy);
    });
  };

  const onChangeSelect = (property: string, e: any) => {
    let copy = { ...formData } as PlannedActivityForLinkDto;
    copy[property] = e && e["key"];
    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangeOpcoId = async (e: any) => {
    setNOFMaxNode(0);
    setNOFMaxLabNode(0);

    let copy = { ...formData } as PlannedActivityForLinkDto;
    let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
    copySubmit.plannedActivityId = undefined;
    copySubmit.linkedPlannedActivityId = undefined;
    copySubmit.numberOfNodes = undefined;
    copySubmit.numberOfLabNodes = undefined;
    setDataSubmit(copySubmit);
    if (e && e["key"]) {
      copy.opCoId = e["key"];
      await GetDesignComponentListForPlannedToConnect(e["key"]).then((x) => {
        if (x && x.data) {
          copy.designComponentResource = x?.data;
          copy.designComponentId = undefined;
          copy.plannedActivityId = undefined;
        }
      });
      setFormData(copy);
    } else {
      copy.numberOfNodes = undefined;
      copy.numberOfLabNodes = undefined;
      copy.plannedActivityId = undefined;
      copy.designComponentId = undefined;
      copy.designComponentResource = undefined;
      copy.opCoId = undefined;
      copy.plannedActivityResource = undefined;

      await GetOpcoListForPlannedToConnect().then((x) => {
        if (x && x != undefined) {
          copy.opCoResource = x.data;
        }
      });
      setFormData(copy);
    }

    //Rimuovi Validazione
    if (validation?.property?.includes("opCoId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("opCoId");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangeDesignComponent = async (e: any) => {
    setNOFMaxNode(0);
    setNOFMaxLabNode(0);

    let copy = { ...formData } as PlannedActivityForLinkDto;
    let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
    copySubmit.plannedActivityId = undefined;
    copySubmit.linkedPlannedActivityId = undefined;
    copySubmit.numberOfNodes = undefined;
    copySubmit.numberOfLabNodes = undefined;

    if (e && e["key"] !== undefined) {
      copy.designComponentId = e["key"];
      copySubmit.designComponentIdStart = e["key"];
      await GetPlannedActivityListForPlannedToConnect(
        e["key"],
        copy.opCoId
      ).then((x) => {
        if (x && x.data) {
          copy.plannedActivityResource = x.data;
          if (dictionaryToArrayPlannedActivityToConnect(x.data).length === 1) {
            copy.plannedActivityId = dictionaryToArrayPlannedActivityToConnect(
              x.data
            )[0].key;
            GetPlannedActivityToConnect(copy.plannedActivityId, copy);
            checkShowProdNodes(x.data, "response");
            return;
          } else {
            copy.plannedActivityId = undefined;
          }
        } else {
          copy.plannedActivityId = undefined;
          copy.plannedActivityResource = undefined;
          copy.numberOfNodes = 0;
          copy.numberOfLabNodes = 0;
        }
        setFormData(copy);
        return;
      });
    } else {
      copy.designComponentId = undefined;
      copy.plannedActivityId = undefined;
      copy.numberOfNodes = undefined;
      copy.numberOfLabNodes = undefined;
      copy.plannedActivityResource = undefined;
      copySubmit.designComponentIdStart = undefined;
      await GetOpcoListForPlannedToConnect().then((x) => {
        if (x && x != undefined) {
          copy.opCoResource = x.data;
        }
      });
      if (copy.opCoId) {
        await GetDesignComponentListForPlannedToConnect(copy.opCoId).then(
          (x) => {
            if (x && x.data) {
              copy.designComponentResource = x?.data;
            }
          }
        );
      }
      setFormData(copy);
    }
    setDataSubmit(copySubmit);

    //Rimuovi Validazione
    if (
      validation?.property?.includes("designComponentId") ||
      validation?.property?.includes("designComponentId")
    ) {
      let copy = { ...validation, property: [...validation.property] };
      if (validation?.property?.includes("designComponentId")) {
        let idxOfProperty = copy.property.indexOf("designComponentId");
        copy.property.splice(idxOfProperty, 1);
      }
      //Rimuovi validazione per planned Activity //
      if (validation?.property?.includes("plannedActivityId")) {
        let idxOfProperty = copy.property.indexOf("plannedActivityId");
        copy.property.splice(idxOfProperty, 1);
      }
      setValidation(copy);
    }
  };

  const onChangePlannedActivityId = async (e: any) => {
    let copy = { ...formData } as PlannedActivityForLinkDto;
    let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
    if (e && e["key"]) {
      copySubmit.plannedActivityId = e["key"];
      copy.plannedActivityId = e["key"];
      setDataSubmit(copySubmit);
      await GetPlannedActivityToConnect(e["key"], copy);
      checkShowProdNodes(e["value"], "normal");
    } else {
      if (copy.opCoId) {
        await GetDesignComponentListForPlannedToConnect(copy.opCoId).then(
          (x) => {
            if (x && x.data) {
              copy.designComponentResource = x.data;
            }
          }
        );
        if (copy.designComponentId) {
          await GetPlannedActivityListForPlannedToConnect(
            copy.designComponentId,
            copy.opCoId
          ).then((x) => {
            if (x && x.data) {
              copy.plannedActivityResource = x.data;
            } else {
              copy.plannedActivityResource = undefined;
            }
          });
        }
      }
      copy.plannedActivityId = undefined;
      copy.numberOfNodes = undefined;
      copySubmit.plannedActivityId = undefined;
      copySubmit.numberOfLabNodes = undefined;
      copySubmit.linkedPlannedActivityId = undefined;
      copy.plannedActivityResource = undefined;

      setNOFMaxNode(0);
      setNOFMaxLabNode(0);
      setFormData(copy);
      setDataSubmit(copySubmit);
    }

    //Rimuovi Validazione
    if (validation?.property?.includes("plannedActivityId")) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf("plannedActivityId");
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangeLinkedPlannedActivityId = async (e: any) => {
    let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
    if (e && e["key"]) {
      copySubmit.linkedPlannedActivityId = e["key"];
      copySubmit.numberOfNodes =
        linkedPlannedActivityToArray?.find((x) => x.key == e["key"])
          ?.numberOfNodes ?? 0;
      // const NofNodeLinkedPlanned = linkedPlannedActivityToArray?.find((x) => x.key == dataSubmit?.linkedPlannedActivityId)?.numberOfNodes ?? 0;
      const NewMaxNofNode = nOFNodeOfDesignComponent + copySubmit.numberOfNodes;
      // setNOFMaxNode((prev) => prev + NewNofNodeLinkedPlanned);

      setNOFMaxNode(NewMaxNofNode);
      setDataSubmit(copySubmit);
    } else {
      const plannedActivityResource = formData?.plannedActivityResource
        ? dictionaryToArrayPlannedActivityToConnect(
            formData?.plannedActivityResource
          )
        : undefined;
      copySubmit.numberOfNodes = plannedActivityResource
        ? plannedActivityResource[0]?.value.numberOfNodes ?? 0
        : 0;
      const NewMaxNofNode = nOFNodeOfDesignComponent + copySubmit.numberOfNodes;

      // setNOFMaxNode((prev) => prev + NewNofNodeLinkedPlanned);
      setNOFMaxNode(NewMaxNofNode);

      copySubmit.linkedPlannedActivityId = undefined;
      if (formData?.plannedActivityResource)
        copySubmit.numberOfNodes =
          plannedActivityResource &&
          plannedActivityResource[0]?.value.numberOfNodes;

      setDataSubmit(copySubmit);
    }
  };

  const checkShowProdNodes = (data: any, type: string) => {
    if (type === "normal") {
      const plannedActivityNameSplittingArr = data?.split(" | ");
      setAllowLabNodes(
        !plannedActivityNameSplittingArr?.includes("RFS Achieved")!
      );
    } else {
      const plannedActivityNameSplittingArr =
        dictionaryToArrayPlannedActivityToConnect(
          data
        )[0].value?.plannedActivityName?.split(" | ");
      setAllowLabNodes(
        !plannedActivityNameSplittingArr?.includes("RFS Achieved")!
      );
    }
  };

  const [validazioneCustom, setValidazioneCustom] = useState<{
    response: boolean;
    property?: string;
    message?: string;
  }>();

  const onChangeNodes = (e: any, property: string) => {
    let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
    let copy = { ...formData } as PlannedActivityForLinkDto;
    let value = +e;

    // REMOVE VALIDATION
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }

    if (value < 0) {
      setValidazioneCustom({
        property: property,
        response: false,
        message: "*Number of Nodes can not be negative",
      });
    } else {
      if (property === "numberOfNodes") {
        if (value > (copySubmit.numberOfNodes ?? 0)) {
          let toDim = value - (copySubmit.numberOfNodes ?? 0);
          if (copy.numberOfNodes != undefined && copy.numberOfNodes >= toDim) {
            copy.numberOfNodes = copy.numberOfNodes - toDim;
            copySubmit.numberOfNodes = value;
            setDataSubmit(copySubmit);
            setFormData(copy);
          }
        } else {
          let toAdd = (copySubmit.numberOfNodes ?? 0) - value;
          if (copy.numberOfNodes !== undefined) {
            copy.numberOfNodes = copy.numberOfNodes + toAdd;
            copySubmit.numberOfNodes = value;
            setDataSubmit(copySubmit);
            setFormData(copy);
          }
        }
      } else {
        if (value > (copySubmit.numberOfLabNodes ?? 0)) {
          let toDim = value - (copySubmit.numberOfLabNodes ?? 0);
          if (
            copy.numberOfLabNodes != undefined &&
            copy.numberOfLabNodes >= toDim
          ) {
            copy.numberOfLabNodes = copy.numberOfLabNodes - toDim;
            copySubmit.numberOfLabNodes = value;
            setDataSubmit(copySubmit);
            setFormData(copy);
          }
        } else {
          let toAdd = (copySubmit.numberOfLabNodes ?? 0) - value;
          if (copy.numberOfLabNodes != undefined) {
            copy.numberOfLabNodes = copy.numberOfLabNodes + toAdd;
            copySubmit.numberOfLabNodes = value;
            setDataSubmit(copySubmit);
            setFormData(copy);
          }
        }
      }

      setValidazioneCustom({ response: true });
    }
  };

  const ValidazioneClient = (copySubmit: PlannedActivityMigrationsDto) => {
    let copy = { ...formData } as PlannedActivityForLinkDto;

    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (copy.opCoId === undefined || copy.opCoId === 0) {
      addInvalidProperty("opCoId");
    }
    if (copy.designComponentId === undefined || copy.designComponentId === 0) {
      addInvalidProperty("designComponentId");
    }
    if (copy.plannedActivityId === undefined || copy.plannedActivityId === 0) {
      addInvalidProperty("plannedActivityId");
    }
    if (copy.numberOfNodes && !copy.elementCount && allowLabNodes) {
      if (
        copySubmit.numberOfNodes === undefined ||
        (copySubmit.numberOfNodes ?? 0) <= 0 ||
        (copySubmit.numberOfNodes ?? 0) < nOfNodeOfLinkedPlanned
      ) {
        addInvalidProperty("numberOfNodes");
      }
    }

    if (copy.numberOfLabNodes && !copy.elementCount) {
      if (
        copySubmit.numberOfLabNodes === undefined ||
        (copySubmit.numberOfLabNodes ?? 0) <= 0 ||
        (copySubmit.numberOfLabNodes ?? 0) < nOfLabNodeOfLinkedPlanned
      ) {
        addInvalidProperty("numberOfNodes");
      }
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const SubmitManage = async () => {
    let copySubmit = { ...dataSubmit } as PlannedActivityMigrationsDto;
    if (ValidazioneClient(copySubmit).response == true) {
      copySubmit.designComponentIdStart = formData?.designComponentId;

      if (!formData?.elementCount) {
        if (copySubmit.numberOfNodes) {
          copySubmit.numberOfNodes -= nOfNodeOfLinkedPlanned;
        }
        if (copySubmit.numberOfLabNodes) {
          copySubmit.numberOfLabNodes -= nOfLabNodeOfLinkedPlanned;
        }
      } else {
        let numberOfNodes = 0;
        let numberOfLabNodes = 0;
        let arrEnd = [] as number[];
        formData?.endNetworkElementAssociateds?.map((x) => {
          if (x.id) arrEnd.push(x.id);
          if (x.enviroment === "PRODUCTION") {
            numberOfNodes++;
          } else {
            numberOfLabNodes++;
          }
        });
        copySubmit.networkElementEnd = arrEnd;

        let arrStart = [] as number[];
        formData?.startNetworkElementAssociateds?.map((x) => {
          if (x.id) arrStart.push(x.id);
        });
        copySubmit.networkElementStart = arrStart;
        copySubmit.numberOfNodes = numberOfNodes;
        copySubmit.numberOfLabNodes = numberOfLabNodes;
      }
      setDataSubmit(copySubmit);
      await SubmitPlannedActivityMigration(copySubmit).then((x) => {
        if (x && !x.warning) {
          if (
            props.action.changeNodesFromModal &&
            formData?.numberOfNodes !== undefined &&
            formData?.numberOfNodes !== null
          ) {
            if (!formData?.elementCount) {
              props.action.changeNodesFromModal(formData?.numberOfNodes);
            } else {
              props.action.changeNodesFromModal(
                copySubmit.networkElementStart?.length ?? 0
              );
            }
          }
          props.action.setIsVisibleModalManage(false);
          props.action.callBackForRefreshGetUpdate &&
            props.lcmId &&
            props.action.callBackForRefreshGetUpdate(props.lcmId);

          if (props.isFromPlannedActivityModal === false) {
            rootStore.dispatch({ type: "REFRESH", payload: true });
          }
        }
      });
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered",
          notifyType: NotifyType.warning,
        })
      );
    }
  };

  const Exit = () => {
    if (changed) {
      setConfirm({
        title: "Confirm",
        message: "Are you sure you want to quit? Unsaved changes will be lost.",
        button: "Exit",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => setConfirm(stateConfirm),
          confirm: () => props.action.setIsVisibleModalManage(false),
        },
      });
    } else {
      props.action.setIsVisibleModalManage(false);
    }
  };

  const plannedResource =
    formData?.plannedActivityResource &&
    dictionaryToArrayPlannedActivityToConnect(formData?.plannedActivityResource)
      .map((x) => {
        return {
          key: x.key,
          value: x.value.plannedActivityName,
          linkedDesignComponent: x.value.linkedDesignComponent,
          linkedToPlannedActivity: x.value.linkedToPlannedActivity,
        };
      })
      .sort((a, b) =>
        (a.value?.toLowerCase() ?? "") < (b.value?.toLowerCase() ?? "") ? -1 : 1
      );

  const linkedDesignComponent = plannedResource?.find(
    (x) => x.key == formData?.plannedActivityId
  )?.linkedDesignComponent;
  const linkedPlannedActivity = plannedResource?.find(
    (x) => x.key == formData?.plannedActivityId
  )?.linkedToPlannedActivity;
  const linkedPlannedActivityToArray =
    linkedPlannedActivity &&
    dictionaryToArrayPlannedActivityToConnectData(linkedPlannedActivity).map(
      (x) => {
        return {
          key: x.key,
          value: x.value.name,
          numberOfNodes: x.value.numberOfNode,
        };
      }
    );

  const selectRow = (
    list: number[],
    set: Function,
    checked: boolean,
    id: number
  ) => {
    let copy = [...list];
    if (checked) {
      copy.push(id);
    } else {
      let index = copy.findIndex((x) => x == id);
      if (index != -1) {
        copy.splice(index, 1);
      }
    }
    set(copy);
    return;
  };

  const transfertRow = (
    listToAdd: string,
    listToRemove: string,
    list: number[],
    set: Function
  ) => {
    let copy = { ...formData } as PlannedActivityForLinkDto;
    let toAdd = [] as NetworkElementAssociated[];
    copy[listToRemove]?.map((x) => {
      if (x.id && list.includes(x.id)) {
        toAdd.push(x);
      }
    });
    if (copy[listToAdd] != undefined) {
      copy[listToAdd]?.push(...toAdd);
    } else {
      copy[listToAdd] = toAdd;
    }

    copy[listToRemove] = copy[listToRemove]?.filter(
      (x) => x.id && !list.includes(x.id)
    );
    set([]);
    setFormData(copy);
  };

  return (
    <div className="d-flex justify-content-center row">
      <ModalConfirm data={confirm} />
      <form onChange={() => setChanged(true)} className="w-100">
        <div className="row col-12 px-0 mx-0">
          <div className="row mx-0 d-flex">
            <div className="col-6 py-3 d-flex row mx-0 align-content-start">
              <div className="col-12">
                <label className="voda-bold w-100">
                  Please Select the OpCo <span className="red">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={
                          formData?.opCoResource &&
                          dictionaryToArray(formData?.opCoResource).sort(
                            (a, b) =>
                              (a.value?.toLowerCase() ?? "") <
                              (b.value?.toLowerCase() ?? "")
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
                        onChange={(e) => onChangeOpcoId(e)}
                        onBlur={() => setInputValue("")}
                        isDisabled={props.plannedActivityId != undefined}
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                      ></Select>
                    </div>
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
              <div className="col-12 mt-3 mb-3">
                <label className="voda-bold w-100">
                  Please Select The Design Component
                  <span className="red">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={
                          formData?.designComponentResource &&
                          dictionaryToArray(
                            formData?.designComponentResource
                          ).sort((a, b) =>
                            (a.value?.toLowerCase()?.trim() ?? "") <
                            (b.value?.toLowerCase()?.trim() ?? "")
                              ? -1
                              : 1
                          )
                        }
                        value={
                          formData?.designComponentId != undefined
                            ? formData?.designComponentResource &&
                              dictionaryToArray(
                                formData?.designComponentResource
                              ).filter(
                                (x) => x.key == formData?.designComponentId
                              )
                            : null
                        }
                        onChange={(e) => onChangeDesignComponent(e)}
                        onBlur={() => setInputValue("")}
                        isDisabled={
                          props.plannedActivityId != undefined ||
                          formData?.opCoId == undefined
                        }
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
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
                  </div>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("designComponentId") ? (
                    <label className="validation">
                      *design Component must have a value
                    </label>
                  ) : null}
                </label>
              </div>
              <div className="col-12">
                <label className="labelForm voda-bold w-100">
                  Please select the Planned Activity
                  <span className="red fz-20">*</span>
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        menuPosition={"fixed"}
                        options={plannedResource}
                        value={
                          formData?.plannedActivityId != undefined
                            ? plannedResource &&
                              plannedResource.find(
                                (x) => x.key === formData?.plannedActivityId
                              )
                            : null
                        }
                        onChange={(e) => onChangePlannedActivityId(e)}
                        isDisabled={
                          props.plannedActivityId != undefined ||
                          formData?.designComponentId == undefined
                        }
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value ?? ""}
                        getOptionValue={(option) => option["key"].toString()}
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
                  </div>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("plannedActivityId") ? (
                    <label className="validation">
                      *planned Activity must have a value
                    </label>
                  ) : null}
                </label>
              </div>
            </div>
            <div className="col-6 py-3 d-flex row mx-0 align-content-start">
              {/* <div className="form-group col-12 ">
                <label className="voda-bold w-100">
                  The OpCo
                  <div className="d-flex">
                    <div className="w-100">
                      <Select
                        options={
                          formData?.opCoResource &&
                          dictionaryToArray(formData?.opCoResource)
                        }
                        value={
                          formData?.opCoResource &&
                          dictionaryToArray(formData?.opCoResource).filter(
                            (x) => x.key === formData?.opCoId
                          )
                        }
                        onChange={(e) => onChangeSelect("opCoId", e)}
                        // onBlur={() => setInputValue("")}
                        isDisabled
                        isSearchable
                        isClearable
                        getOptionLabel={(option) => option.value}
                        getOptionValue={(option) => option["key"].toString()}
                      ></Select>
                    </div>
                  </div>
                </label>
              </div> */}
              <div className="form-group col-12 ">
                <label className="labelForm voda-bold w-100">
                  The Planned Design Component
                  <div
                    className="col-12 pl-4 manage-style"
                    style={{ marginTop: "4px" }}
                  >
                    <label
                      className="labelForm w-100 mb-0"
                      dangerouslySetInnerHTML={{
                        __html:
                          linkedDesignComponent != undefined
                            ? dictionaryToArray(linkedDesignComponent)[0].value
                            : "",
                      }}
                    ></label>
                  </div>
                  {validation &&
                  validation.response == false &&
                  validation.property?.includes("linkedDesignComponent") ? (
                    <label className="validation">
                      *Planned Design Component must have a value
                    </label>
                  ) : null}
                </label>
              </div>
              {/* <div className="form-group col-12 ">
								<label className="labelForm voda-bold w-100">
									The Linked Planned Activity
									<div className="d-flex">
										<div className="w-100">
											<Select
												options={linkedPlannedActivityToArray}
												value={linkedPlannedActivityToArray && linkedPlannedActivityToArray.find((x) => x.key == dataSubmit?.linkedPlannedActivityId)}
												onChange={(e) => onChangeLinkedPlannedActivityId(e)}
												// onBlur={() => setInputValue("")}
												// isDisabled={!(linkedPlannedActivityToArray && linkedPlannedActivityToArray.length <= 1)}
												// isDisabled={formData?.numberOfNodes == undefined || formData.numberOfNodes == 0}
												isDisabled={nOFNodeOfDesignComponent == 0 || linkedPlannedActivityToArray?.length === 0}
												isSearchable
												isClearable
												getOptionLabel={(option) => option.value ?? ""}
												getOptionValue={(option) => option["key"].toString()}
											></Select>
										</div>
									</div>
								</label>
								{validation && validation.response == false && validation.property?.includes("linkedPlannedActivityId") ? (
									<label className="validation">*linked Planned Activity must have a value</label>
								) : null}
							</div> */}
            </div>
            {formData?.elementCount ? (
              <div className="col-12 plr-30">
                <label className="text-bb mb-4">
                  Please set the migrated nodes <span className="red">*</span>
                </label>
                <div className="d-flex row mx-0 col-12 mt-4">
                  <div className="col pl-0 d-flex flex-column align-items-center justify-content-center voda-bold">
                    <h6 className="fz-18">Network Elements</h6>
                  </div>
                  <div className="col-2 d-flex flex-column justify-content-center align-items-center"></div>
                  <div className="col pl-0 d-flex flex-column align-items-center justify-content-center voda-bold">
                    <h6 className="fz-18">Migrated Network Elements</h6>
                  </div>
                </div>
                <div className="form-group col-12 mt-1 d-flex flex-row">
                  <div className="col pl-0">
                    <table className="w-100  ">
                      <thead>
                        <tr className="intestazione">
                          <th className="pl-2 py-0">Element Name</th>
                          <th className="pl-2 py-0">Environment</th>
                          <th className="pl-2 py-0">Location</th>
                          <th className="pl-2 py-0"></th>
                        </tr>
                      </thead>
                      <tbody>
                        {!allowLabNodes
                          ? formData?.startNetworkElementAssociateds
                              ?.filter((ele) => ele.enviroment !== "PRODUCTION")
                              .map((item, i) => (
                                <tr className={`dati`} key={i}>
                                  <td>{item.elementName}</td>
                                  <td>{item.enviroment}</td>
                                  <td>{item.location}</td>
                                  <td>
                                    <input
                                      type="checkbox"
                                      checked={
                                        item.id != undefined
                                          ? startSelected.includes(item.id)
                                          : false
                                      }
                                      onChange={(e) =>
                                        item.id &&
                                        selectRow(
                                          startSelected,
                                          setStartSelected,
                                          e.target.checked,
                                          item.id
                                        )
                                      }
                                    ></input>
                                  </td>
                                </tr>
                              ))
                          : formData?.startNetworkElementAssociateds?.map(
                              (item, i) => (
                                <tr className={`dati`} key={i}>
                                  <td>{item.elementName}</td>
                                  <td>{item.enviroment}</td>
                                  <td>{item.location}</td>
                                  <td>
                                    <input
                                      type="checkbox"
                                      checked={
                                        item.id != undefined
                                          ? startSelected.includes(item.id)
                                          : false
                                      }
                                      onChange={(e) =>
                                        item.id &&
                                        selectRow(
                                          startSelected,
                                          setStartSelected,
                                          e.target.checked,
                                          item.id
                                        )
                                      }
                                    ></input>
                                  </td>
                                </tr>
                              )
                            )}
                      </tbody>
                    </table>
                  </div>
                  <div className="col-2 d-flex flex-column justify-content-center align-items-center">
                    <button
                      disabled={startSelected.length == 0}
                      className="btn px-2 w-100 mb-2 mig-bt"
                      type="button"
                      onClick={() =>
                        transfertRow(
                          "endNetworkElementAssociateds",
                          "startNetworkElementAssociateds",
                          startSelected,
                          setStartSelected
                        )
                      }
                    >
                      Add
                      <img
                        src={require("../../img/right-page-arrow.png")}
                        className="mrl-5"
                      />
                    </button>
                    <button
                      disabled={endSelected.length == 0}
                      className="btn px-2 w-100 mig-bt"
                      type="button"
                      onClick={() =>
                        transfertRow(
                          "startNetworkElementAssociateds",
                          "endNetworkElementAssociateds",
                          endSelected,
                          setEndSelected
                        )
                      }
                    >
                      <img
                        src={require("../../img/left-page-arrow.png")}
                        className="mrl-5"
                      />
                      Remove
                    </button>
                  </div>
                  <div className="col pl-0">
                    <table className="w-100  ">
                      <thead>
                        <tr className="intestazione">
                          <th className="pl-2 py-0">Element Name</th>
                          <th className="pl-2 py-0">Environment</th>
                          <th className="pl-2 py-0">Location</th>
                          <th className="pl-2 py-0"></th>
                        </tr>
                      </thead>
                      <tbody>
                        {!allowLabNodes
                          ? formData?.endNetworkElementAssociateds
                              ?.filter((ele) => ele.enviroment !== "PRODUCTION")
                              ?.map((item, i) => (
                                <tr className={`dati`} key={i}>
                                  <td>{item.elementName}</td>
                                  <td>{item.enviroment}</td>
                                  <td>{item.location}</td>
                                  <td>
                                    <input
                                      type="checkbox"
                                      checked={
                                        item.id != undefined
                                          ? endSelected.includes(item.id)
                                          : false
                                      }
                                      onChange={(e) =>
                                        item.id &&
                                        selectRow(
                                          endSelected,
                                          setEndSelected,
                                          e.target.checked,
                                          item.id
                                        )
                                      }
                                    ></input>
                                  </td>
                                </tr>
                              ))
                          : formData?.endNetworkElementAssociateds?.map(
                              (item, i) => (
                                <tr className={`dati`} key={i}>
                                  <td>{item.elementName}</td>
                                  <td>{item.enviroment}</td>
                                  <td>{item.location}</td>
                                  <td>
                                    <input
                                      type="checkbox"
                                      checked={
                                        item.id != undefined
                                          ? endSelected.includes(item.id)
                                          : false
                                      }
                                      onChange={(e) =>
                                        item.id &&
                                        selectRow(
                                          endSelected,
                                          setEndSelected,
                                          e.target.checked,
                                          item.id
                                        )
                                      }
                                    ></input>
                                  </td>
                                </tr>
                              )
                            )}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            ) : (
              <>
                {allowLabNodes && (
                  <div className="form-group col-12 plr-30">
                    <label className="text-bb mb-4">
                      Please set the migrated PROD nodes
                      <span className="red fz-20">*</span>
                    </label>

                    <label className="labelForm voda-bold w-100">
                      <div
                        className={`d-flex align-items-center ${
                          nOFNodeOfDesignComponent ? "" : "disabledRange"
                        }`}
                      >
                        <span className="">{formData?.numberOfNodes}</span>
                        <Slider
                          onChange={(e) => onChangeNodes(e, "numberOfNodes")}
                          value={dataSubmit?.numberOfNodes ?? 0}
                          max={nOFMaxNode}
                          className="w-100"
                        />

                        <span className="">
                          {dataSubmit?.numberOfNodes ?? null}
                        </span>
                      </div>
                      {validation &&
                      validation.response == false &&
                      validation.property?.includes("numberOfNodes") ? (
                        <label className="validation">
                          *Number of Nodes value is not valid
                        </label>
                      ) : null}
                      {validazioneCustom &&
                      validazioneCustom.response == false &&
                      validazioneCustom.property == "numberOfNodes" ? (
                        <label className="validation">
                          {validazioneCustom.message}
                        </label>
                      ) : null}
                    </label>
                  </div>
                )}

                <div className="form-group col-12 plr-30">
                  <label className="text-bb mb-4">
                    Please set the migrated lab node
                    <span className="red fz-20">*</span>
                  </label>

                  <label className="labelForm voda-bold w-100">
                    <div
                      className={`d-flex align-items-center ${
                        nOFNodeOfDesignComponent ? "" : "disabledRange"
                      }`}
                    >
                      <span className="">{formData?.numberOfLabNodes}</span>
                      <Slider
                        onChange={(e) => onChangeNodes(e, "numberOfLabNodes")}
                        value={dataSubmit?.numberOfLabNodes ?? 0}
                        max={nOFMaxLabNode}
                        className="w-100"
                      />

                      <span className="">
                        {dataSubmit?.numberOfLabNodes ?? null}
                      </span>
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("numberOfLabNodes") ? (
                      <label className="validation">
                        *Number of Nodes value is not valid
                      </label>
                    ) : null}
                    {validazioneCustom &&
                    validazioneCustom.response == false &&
                    validazioneCustom.property == "numberOfLabNodes" ? (
                      <label className="validation">
                        {validazioneCustom.message}
                      </label>
                    ) : null}
                  </label>
                </div>
              </>
            )}
          </div>
          <div className="col-12 justify-content-end d-flex mb-3 plr-30 prl-30">
            <button
              className="  voda-bold btn btn-link px-4 btnHeader cancel"
              onClick={() => Exit()}
              type="button"
            >
              Cancel
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={() => SubmitManage()}
            >
              Submit
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default ManageMigration;
