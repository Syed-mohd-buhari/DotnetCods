import React, { useState, useEffect } from "react";
import DatePicker from "react-datepicker";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  formatDateWithTime,
  getMaxDate,
  listIsNullOrEmpty,
  numberIsNullOrZero,
} from "../../Hook/Common";
import { useSelector } from "react-redux";
import Select from "react-select";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { Tabs, Tab, Modal, Form } from "react-bootstrap";
import NetworkElementAsPlannedActivity from "./NetworkElementAsPlannedActivity";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import {
  GetLocationTypeById,
  GetPlannedActivityCreateResource,
} from "../../Redux/Action/PlannedActivity/PlannedActivityCreateAction";
import { deletePlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityDeleteAction";
import { GetPlannedActivityEditResource } from "../../Redux/Action/PlannedActivity/PlannedActivityEditAction";
import { GetPlannedActivityGrid } from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
  PlannedActivityQueryObjectGrid,
} from "../../Model/PlannedActivity";
import OpcoContainer from "../../Containers/Lookup/OpCoContainer";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import {
  DataModalConfirm,
  PlannedActivityTypeForEnum,
  getLcmIdObject,
  stateConfirm,
} from "../../Model/Common";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  dictionaryToArray,
  dictionaryToArrayDeploymentStatusDto,
  dictionaryToArrayLocationDto,
  dictionaryToArrayPlannedActivityResourceDto,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import EnvironmentContainer from "../../Containers/Lookup/EnvironmentContainer";
import {
  NetworkElementAsPlannedDtoUpdate,
  NetworkElementAsPlannedDtoCreate,
} from "../../Model/NetworkElementAsPlanned";
import {
  CreatNetworkElementAsPlanned,
  GetAssetResourceKey,
  GetLCMID,
} from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCreateAction";
import {
  EditNetworkElementAsPlanned,
  GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource,
} from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedEditAction";
import OriginalEquipmentManufacturer from "../../Containers/Lookup/OriginalEquipmentManufacturerContainer";
import DeploymentStatus from "../../Containers/Lookup/DeploymentStatusContainer";
import DeploymentType from "../../Containers/Lookup/DeploymentTypeContainer";
import Location from "../../Containers/Lookup/LocationContainer";
import NFVIBundleIDContainer from "../../Containers/Lookup/NFVIBundleIDContainer";
import { GetDesignComponentFamilyFromOem } from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedCommonAction";
import { DeploymentStatusDto } from "../../Model/LookUp/DeploymentStatus";
import { LocationDto } from "../../Model/LookUp/Location";
import SubDomainSpoc from "../../Containers/Lookup/SubdomainSpocContainer";
import { ResultDto } from "../../Model/CommonModels";
import setLoader from "../../Redux/Action/LoaderAction";
import { GetNetworkElementAsPlannedGrid } from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedGridAction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import SystemName from "../../Containers/Lookup/DomainContainer";

let paginationQueryPlanning: PlannedActivityQueryObjectGrid = {
  plannedImplementationYear: [],
  activityStatusId: [],
  planningActivityStatusId: [],
  designComponentId: [],
  buildBagId: [],
  driver: [],
  benefits: [],
  plannedActivityDescription: [],
  activityDetailsText: [],
  budgetAvailability: [],
  deliveryProjectName: [],
  localApproval: [],
  deliveryStatusId: [],
  responsibilityPhaseId: [],
  plannedCompletion: undefined,
  assetLiveStatusDateValue: undefined,
  dateAssetDecommissionedAssetValue: undefined,
  notes: [],
  lcmEngineeringId: [],
  riskEngineeringEvaluation: [],
  riskEngineeringNotes: [],
  riskOperationalEvaluation: [],
  riskOperationalNotes: [],
  sortBy: "",
  isSortAscending: false,
  budgetTrackingId: [],
  budgetValueGrid: [],
  deliveryProjectId: [],
  plannedActivityResourceId: [],
  planningRisk: [],
  relatesToId: [],
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
  forLcm: true,
  forNetwork: true,
};

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
    Edit(id: number | undefined): any;
  };
  edit: boolean;
  keyTab?: string;
  idDetail?: number | string | undefined | null;
}

const NetworkElementAsPlannedModal: React.FC<Props> = (props) => {
  setLoader("REMOVE", "GetNetworkElementAsPlannedGrid");
  const [keyTabs, setKey] = useState("networkelement");
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
    setChanged,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<NetworkElementAsPlannedDtoUpdate>(
    CreatNetworkElementAsPlanned,
    EditNetworkElementAsPlanned
  );

  const [softwareManufacturer, setSoftwareManufacturer] = useState<{
    key: number;
    value: string;
  }>();

  const [selectedDeploymentStatus, setSelectedDeploymentStatus] =
    useState<DeploymentStatusDto>({});

  const dtoEditResourceState = (state: RootState) =>
    state.networkElementAsPlannedEditReducer.NetworkElementAsPlannedDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.networkElementAsPlannedCreateReducer.NetworkElementAsPlannedDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  const { tipologicaPermesso, isPermesso } = useAuth();
  const [showFurtherDetails, setShowFurtherDetails] = useState<boolean>(false);
  const [toggleResource, setToggleResource] = useState<boolean>(false);
  const [editSwKeyName, setEditSwKeyName] = useState<boolean>(false);
  const [editHwKeyName, setEditHwKeyName] = useState<boolean>(false);
  const [editPrevSwKeyName, setEditPrevSwKeyName] = useState<boolean>(false);
  const [editPrevHwKeyName, setEditPrevHwKeyName] = useState<boolean>(false);
  const [swKeyError, setSwKeyError] = useState<boolean>(false);
  const [prevSwKeyError, setPrevSwKeyError] = useState<boolean>(false);
  const [hwKeyError, setHwKeyError] = useState<boolean>(false);
  const [prevHwKeyError, setPrevHwKeyError] = useState<boolean>(false);
  const [checkPlannedActivityRequired, setCheckPlannedActivityRequired] =
    useState<boolean>(false);
  const [showLiveDate, setShowLiveDate] = useState<boolean>(false);
  const [lcmIdResponse, setLcmIdResponse] = useState<ResultDto | undefined>();

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      const selectedDS =
        editResource?.deploymentStatusReosurce &&
        dictionaryToArrayDeploymentStatusDto(
          editResource?.deploymentStatusReosurce
        ).find((el) => el.key === editResource?.deploymentStatusId)?.value;
      selectedDS && setSelectedDeploymentStatus(selectedDS);
      let copy = { ...editResource } as NetworkElementAsPlannedDtoUpdate;
      if (selectedDS?.checkPlannedActivity || selectedDS?.rule === 1) {
        copy.plannedAction = true;
      }
      setFormData(copy);
    } else {
      let copy = { ...createResource } as NetworkElementAsPlannedDtoCreate;
      if (
        copy.environmentReosurce != undefined &&
        (copy.environmentId == undefined || copy.environmentId == 0)
      ) {
        copy.environmentId = dictionaryToArray(copy?.environmentReosurce).find(
          (x) => x.value.toLowerCase() == "unspecified"
        )?.key;
      }
      if (
        copy.deploymentStatusReosurce != undefined &&
        (copy.deploymentStatusId == undefined || copy.deploymentStatusId == 0)
      ) {
        copy.deploymentStatusId = dictionaryToArrayDeploymentStatusDto(
          copy?.deploymentStatusReosurce
        ).find(
          (x) =>
            x.value.deploymentStatusDescription?.toLowerCase() == "unspecified"
        )?.key;
      }
      const selectedDS =
        copy.deploymentStatusReosurce &&
        dictionaryToArrayDeploymentStatusDto(
          copy.deploymentStatusReosurce
        ).find((el) => el.key === copy.deploymentStatusId)?.value;
      selectedDS && setSelectedDeploymentStatus(selectedDS);
      if (selectedDS?.checkPlannedActivity || selectedDS?.rule == 1) {
        copy.plannedAction = true;
      }

      setFormData(copy);
    }
  }, [createResource, editResource, props.edit]);

  useEffect(() => {
    if (
      props.keyTab === "" ||
      props.keyTab === null ||
      props.keyTab === undefined ||
      !props.edit
    ) {
      setKey("networkelement");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  // useEffect(() => {
  //   let flag = false;
  //   if (formData) {
  //     let copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
  //     const selectedDS =
  //       copy.deploymentStatusReosurce &&
  //       dictionaryToArrayDeploymentStatusDto(
  //         copy.deploymentStatusReosurce
  //       ).find((el) => el.key === copy.deploymentStatusId)?.value
  //         .deploymentStatusDescription;
  //     flag = selectedDS == "PLANNED" ? true : false;
  //   }
  //   setShowLiveDate(flag);
  // }, [formData?.deploymentStatusId]);

  // useEffect(() => {
  //   if (
  //     formData &&
  //     formData.designComponentId &&
  //     formData.opCoId &&
  //     !props?.edit
  //   ) {
  //     const selectResource = {
  //       opCoId: formData?.opCoId.toString(),
  //       designComponentId: formData?.designComponentId.toString(),
  //     };
  //     GetAssetResourceKey(selectResource).then((x) => {
  //       if (x && x !== undefined) {
  //         setFormData({
  //           ...formData,
  //           swResourceKey: x?.["item1"]?.data,
  //           hwResourceKey: x?.["item2"]?.data,
  //         });
  //       }
  //     });
  //   }
  // }, [formData?.opCoId, formData?.designComponentId]);

  // Logic code get the resource key using api call

  // useEffect(() => {
  //   if (formData && formData.opCoId && formData.elementName && !props.edit) {
  //     const debounceTimer = setTimeout(() => {
  //       const selectResource = {
  //         opCoId: formData?.opCoId?.toString(),
  //         elementName: formData?.elementName,
  //       };
  //       GetAssetResourceKey(selectResource).then((x) => {
  //         if (x && x !== undefined) {
  //           setFormData({
  //             ...formData,
  //             swResourceKey: x?.["item1"]?.data,
  //             hwResourceKey: x?.["item2"]?.data,
  //           });
  //         }
  //       });
  //     }, 2000);
  //     return () => clearTimeout(debounceTimer);
  //   }
  // }, [formData?.opCoId, formData?.elementName]);

  const goToPlannedActivity = () => {
    setKey("plannedActivities");
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    if (property == "subDomainSpocIds") {
      e = [e];
    }
    let array = [] as Array<number>;
    let copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
    if (e != null && e.length > 0 && e !== undefined) {
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

  //VALIDAZIONE PRE Save
  const validazioneClient = (copy: NetworkElementAsPlannedDtoUpdate) => {
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
      (copy?.originalEquipmentManufacturerId == null ||
        copy?.originalEquipmentManufacturerId === undefined ||
        copy?.originalEquipmentManufacturerId === 0) &&
      !props.edit
    ) {
      addInvalidProperty("originalEquipmentManufacturerId");
    }
    if (
      copy?.designComponentId == null ||
      copy?.designComponentId === undefined ||
      copy?.designComponentId === 0
    ) {
      addInvalidProperty("designComponentId");
    }
    if (
      copy?.elementName == null ||
      copy?.elementName === undefined ||
      copy?.elementName.trim() === ""
    ) {
      addInvalidProperty("elementName");
    }
    if (
      copy?.environmentId == null ||
      copy?.environmentId === undefined ||
      copy?.environmentId === 0
    ) {
      addInvalidProperty("environmentId");
    }
    if (
      copy?.deploymentStatusId === null ||
      copy?.deploymentStatusId === undefined ||
      copy?.deploymentStatusId === 0
    ) {
      addInvalidProperty("deploymentStatusId");
    }

    if (
      props?.edit &&
      (swKeyError === true ||
        copy?.swResourceKey === null ||
        copy?.swResourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("swResourceKey");
    }

    if (
      props?.edit &&
      (hwKeyError === true ||
        copy?.hwResourceKey === null ||
        copy?.hwResourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("hwResourceKey");
    }
    if (
      props?.edit &&
      formData?.previousSWResourceKey !== null &&
      (prevSwKeyError === true ||
        copy?.previousSWResourceKey === null ||
        copy?.previousSWResourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("previousSWResourceKey");
    }

    if (
      props?.edit &&
      formData?.previousHWResourceKey !== null &&
      (prevHwKeyError === true ||
        copy?.previousHWResourceKey === null ||
        copy?.previousHWResourceKey === undefined)
    ) {
      setToggleResource(true);
      addInvalidProperty("previousHWResourceKey");
    }
    // if (
    //   showLiveDate &&
    //   (copy.assetLiveStatusDate == undefined ||
    //     copy.assetLiveStatusDate == null)
    // ) {
    //   addInvalidProperty("assetLiveStatusDate");
    // }

    if (numberIsNullOrZero(copy.locationId)) {
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

  //Modifiche Planning
  const { query, setQuery } = useResourceTableCrud(
    paginationQueryPlanning,
    isPermesso ? GetPlannedActivityGrid : undefined
  );

  const { New, Edit, closeModal, Delete } = useOperationTableCrud<
    PlannedActivityDtoUpdate,
    PlannedActivityDtoCreate
  >(
    GetPlannedActivityCreateResource,
    GetPlannedActivityEditResource,
    deletePlannedActivity,
    refresh
  );

  const AddOnList = async (item: PlannedActivityDtoUpdate[]) => {
    let copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
    const dates = item.map((ele) => ele.plannedCompletion);
    const maxDateObj = getMaxDate(dates);
    if (
      copy.plannedActivityDto === null ||
      copy.plannedActivityDto === undefined
    ) {
      copy.plannedActivityDto = [];
    }
    copy.plannedActivityDto = item;

    let result =
      await GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource(
        item[maxDateObj.index].plannedActivityResourceId,
        item[maxDateObj.index].deliveryStatusId,
        !props?.edit
      );

    copy.deploymentStatusReosurce = result?.data;
    if (dictionaryToArray(result?.data).length === 1) {
      copy.deploymentStatusId = dictionaryToArray(result?.data)[0].key;
    }

    setFormData(copy);
  };

  const GetElementFromList = (index: number) => {
    return formData?.plannedActivityDto && formData?.plannedActivityDto[index];
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const OpCoRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.opCoReosurce)
      formData.opCoReosurce = obj as { [key: string]: string };
    setFormData(formData);
  };

  const SubDomainSpocRefillData = (value: Array<any>) => {
    if (formData && formData.eduSpocResource) {
      formData.eduSpocResource = value
        .filter((itm) => itm.isEdu === true)
        .reduce(
          (acc, item) => ({ ...acc, [item.id]: item.description }),
          {}
        ) as {
        [key: string]: string;
      };
    }

    if (formData && formData.subDomainSpocResource) {
      formData.subDomainSpocResource = value
        .filter((itm) => itm.isSubDomain === true)
        .reduce(
          (acc, item) => ({ ...acc, [item.id]: item.description }),
          {}
        ) as {
        [key: string]: string;
      };
    }
    setFormData(formData);
  };

  const EnvironmentContainerRefillData = (value: Array<any>) => {
    //   var data = value.map(x => x.id, x.description )
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.environmentReosurce)
      formData.environmentReosurce = obj as { [key: string]: string };
    setFormData(formData);
  };

  const LocationRefillData = (value: Array<any>) => {
    var obj = value.reduce((acc, item) => ({ ...acc, [item.id]: item }), {});
    if (formData && formData?.locationReosurce)
      formData.locationReosurce = obj as { [key: string]: LocationDto };

    if (formData && formData?.locationId) {
      onChangeLocation({ key: formData?.locationId });
    }

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

  const DeploymentStatusRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.deploymentStatusId]: item }),
      {}
    );

    if (formData && formData?.deploymentStatusReosurce)
      formData.deploymentStatusReosurce = obj as {
        [key: string]: DeploymentStatusDto;
      };
    setFormData(formData);
  };

  const DeploymentTypeRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.deploymentTypeReosurce)
      formData.deploymentTypeReosurce = obj as { [key: string]: string };
    setFormData(formData);
  };

  const NfviBundleIdRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.nfviBundleIDReosurce)
      formData.nfviBundleIDReosurce = obj as { [key: string]: string };
    setFormData(formData);
  };

  const systemNamesRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.systemNameId]: item }),
      {}
    );

    setFormData(formData);
  };

  useEffect(() => {
    ManagePlannedActivityAllowed();
  }, [formData?.deploymentStatusId, formData?.deploymentStatusReosurce]);

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
      case 4:
        return (
          <EnvironmentContainer
            returnObject={EnvironmentContainerRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 5:
        return (
          <DeploymentStatus
            returnObject={DeploymentStatusRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 6:
        return (
          <DeploymentType
            returnObject={DeploymentTypeRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 7:
        return (
          <Location
            returnObject={LocationRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 9:
        return (
          <SubDomainSpoc
            subDomain={false}
            returnObject={SubDomainSpocRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 11:
        return (
          <SubDomainSpoc
            subDomain={true}
            returnObject={SubDomainSpocRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 10:
        return (
          <NFVIBundleIDContainer
            returnObject={NfviBundleIdRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
        );
      case 15:
        return (
          <SystemName
            returnObject={systemNamesRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          />
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

  const [opco, setOpco] = useState<string>();
  const [designComponent, setDesignComponent] = useState<
    { key: number; value: string } | undefined
  >();
  const [eduSpoc, setEduSpoc] = useState<
    { key: number; value: string }[] | undefined
  >();
  const [subdomainSpoc, setSubdomainSpoc] = useState<
    { key: number; value: string }[] | undefined
  >();

  // useEffect(() => {
  //   const copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
  //   if (formData?.opCoId === undefined || formData.opCoId === null) {
  //     copy.locationId = undefined;
  //   }

  //   setFormData(copy);
  // }, [formData?.opCoId]);

  const onChangeOpCO = (e: any) => {
    const copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
    copy.locationId = undefined;
    if (e && e["key"]) {
      copy.opCoId = e["key"];
    } else {
      copy.opCoId = undefined;
    }
    setFormData(copy);
  };

  //PROPS PLANNED ACTIVITY
  useEffect(() => {
    if (formData?.opCoId != undefined && formData.opCoReosurce != undefined) {
      let opcoString = dictionaryToArray(formData.opCoReosurce).find(
        (x) => x.key == formData.opCoId
      )?.value;
      setOpco(opcoString);
    } else {
      setOpco("");
    }

    if (
      formData?.designComponentId != undefined &&
      formData.designComponentReosurce != undefined
    ) {
      let designComponent = dictionaryToArray(
        formData.designComponentReosurce
      ).find((x) => x.key == formData.designComponentId);
      if (designComponent?.key && designComponent.value)
        setDesignComponent({
          key: designComponent?.key,
          value: designComponent?.value,
        });
    } else {
      setDesignComponent(undefined);
    }

    let eduspocSelected =
      formData?.subDomainSpocResource &&
      dictionaryToArray(formData?.subDomainSpocResource).filter((x) => {
        return (
          formData &&
          formData?.eduSpocIds?.indexOf(x.key) != -1 &&
          formData?.eduSpocIds?.indexOf(x.key) != undefined
        );
      });
    setEduSpoc(eduspocSelected);

    let subDomainSelected =
      formData?.subDomainSpocResource &&
      dictionaryToArray(formData?.subDomainSpocResource).filter((x) => {
        return (
          formData &&
          formData?.subDomainSpocIds?.indexOf(x.key) != -1 &&
          formData?.subDomainSpocIds?.indexOf(x.key) != undefined
        );
      });
    setSubdomainSpoc(subDomainSelected);
  }, [
    formData?.opCoId,
    formData?.designComponentId,
    formData?.eduSpocIds,
    formData?.subDomainSpocIds,
  ]);

  const [showForm, setShowForm] = useState<boolean>(false);
  const [editName, setEditName] = useState<boolean>(false);
  const [editDomain, setEditDomain] = useState<boolean>(false);
  const [showTypingHint, setShowTypingHint] = useState<boolean>(false);

  const [confirmPlanned, setConfirmPlanned] =
    useState<DataModalConfirm>(stateConfirm);

  const SaveOrConfirmPlanned = () => {
    const copy = {
      ...formData,
      opCoReosurce: {},
      designComponentReosurce: {},
      locationReosurce: {},
      originalEquipmentManufacturerResource: {},
      subDomainSpocResource: {},
      environmentReosurce: {},
      eduSpocResource: {},
      deploymentTypeReosurce: {},
      deploymentStatusReosurce: {},
    } as NetworkElementAsPlannedDtoUpdate;

    if (
      !copy.plannedActivityDto?.length &&
      !props.edit &&
      checkPlannedActivityRequired
    ) {
      rootStore.dispatch(
        setNotification({
          message: "Planned Activity is required",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }

    if (
      !formData?.["lcmEngineeringId"] &&
      formData?.deploymentStatusId !== 6 &&
      lcmIdResponse !== undefined
    ) {
      rootStore.dispatch(
        setNotification({
          message: lcmIdResponse?.info,
          notifyType: NotifyType.warning,
        })
      );
      return;
    }

    if (formData?.deploymentStatusId === 6) {
      rootStore.dispatch(
        setNotification({
          message: "Please check the deployment status of asset.",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }

    const confirmPlannedState: DataModalConfirm = {
      title: "Continue without saving planned activities?",
      button: "Continue",
      message:
        "Are you sure you want to continue? All changes in Planned activities will be lost",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setConfirmPlanned(stateConfirm),
        confirm: () =>
          Save(
            copy,
            props.edit,
            validazioneClient,
            refresh,
            RestoreOrphanDeleted,
            orphanDeleted
          ),
      },
    };

    if (copy && validazioneClient(copy).response) {
      let targetDs =
        copy.deploymentStatusReosurce &&
        dictionaryToArrayDeploymentStatusDto(
          copy.deploymentStatusReosurce
        ).find((x) => x.key == copy.deploymentStatusId)?.value;
      if (
        copy.plannedAction &&
        targetDs?.checkPlannedActivity === true &&
        listIsNullOrEmpty(copy.plannedActivityDto)
      ) {
        //Se non è stata collegata almeno una planned Activity ed è required
        rootStore.dispatch(
          setNotification({
            message: "You must connect a Planned Activity",
            notifyType: NotifyType.warning,
          })
        );
      } else if (showForm) {
        setConfirmPlanned(confirmPlannedState);
      } else {
        Save(
          copy,
          props.edit,
          validazioneClient,
          refresh,
          RestoreOrphanDeleted,
          orphanDeleted
        );
      }
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered in Network Element",
          notifyType: NotifyType.warning,
        })
      );
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

    let copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
    if (e && e["key"]) {
      copy.originalEquipmentManufacturerId = e["key"];
      copy.designComponentId = undefined;
      await GetDesignComponentFamilyFromOem(e["key"]).then((x) => {
        if (x != undefined) {
          copy.designComponentReosurce = x;
        }
      });
    } else {
      copy.originalEquipmentManufacturerId = undefined;
      copy.designComponentId = undefined;
      copy.designComponentReosurce = createResource?.designComponentReosurce;
    }
    setFormData(copy);
  };

  const onChangeDeploymentStatus = (e: any) => {
    let copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
    if (e && e["key"]) {
      copy.deploymentStatusId = e["key"];
      if (e["rule"] === 1) {
        copy.plannedAction = true;
      } else {
        copy.plannedAction = false;
      }
    } else {
      copy.deploymentStatusId = undefined;
      copy.plannedAction = false;
    }
    const selectedDS =
      copy.deploymentStatusReosurce &&
      dictionaryToArrayDeploymentStatusDto(copy.deploymentStatusReosurce).find(
        (el) => el.key === copy.deploymentStatusId
      )?.value;
    selectedDS && setSelectedDeploymentStatus(selectedDS);
    // if (showForm && !copy.plannedAction) setShowForm(false);
    if (
      selectedDS?.checkPlannedActivity &&
      copy?.plannedActivityDto &&
      copy?.plannedActivityDto?.length > 0
    ) {
      copy.plannedAction = true;
    }
    if (showForm) setShowForm(false);
    if (!props.edit) {
      if (copy.plannedActivityDto) {
        const copyOfPlannedActivityDto = [
          ...copy.plannedActivityDto,
        ] as PlannedActivityDtoUpdate[];
        copy.plannedActivityDto?.map((el, idx) => {
          if (
            el.plannedActivityResourceId !== undefined &&
            el.plannedActivityResourceId !== null &&
            !selectedDS?.plannedActivityResourceAllowedId?.includes(
              el.plannedActivityResourceId
            )
          ) {
            copyOfPlannedActivityDto.splice(idx, 1);
          }
        });

        copy.plannedActivityDto = copyOfPlannedActivityDto;
      }
    }
    setFormData(copy);
  };

  useEffect(() => {
    const selectedSoftwManuf =
      formData?.originalEquipmentManufacturerResource &&
      dictionaryToArray(formData?.originalEquipmentManufacturerResource).find(
        (x) => x.key === formData?.originalEquipmentManufacturerId
      );
    setSoftwareManufacturer(selectedSoftwManuf);
  }, [formData?.originalEquipmentManufacturerId]);

  useEffect(() => {
    if (formData && formData?.deploymentTypeReosurce) {
      const arr = dictionaryToArray(formData.deploymentTypeReosurce);
      if (arr.length === 1) {
        const copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
        copy.deploymentTypeId = arr[0].key;
        setFormData(copy);
      }
    }
  }, [formData?.deploymentTypeReosurce]);

  useEffect(() => {
    if (
      formData &&
      formData?.opCoId &&
      formData?.designComponentId &&
      formData?.deploymentStatusId !== 6
    ) {
      let data = {
        OpCo: [formData?.opCoId],
        DesignComponent: [formData?.designComponentId],
        DeploymentStatus: [formData?.deploymentStatusId],
      } as getLcmIdObject;
      let isCheckPAforDecommession = false;
      if (
        formData?.plannedActivityDto &&
        formData?.plannedActivityDto?.length > 0
      ) {
        formData?.plannedActivityDto?.map((res: any) => {
          const isRuleCheck = dictionaryToArrayPlannedActivityResourceDto(
            res.plannedActivityResource
          ).find((y) => y.key == res.plannedActivityResourceId)?.value
            ?.ruleLinkedDc;
          if (res.plannedActivityResourceId && isRuleCheck === 20) {
            isCheckPAforDecommession = true;
          } else {
            isCheckPAforDecommession = false;
          }
        });
      }
      !isCheckPAforDecommession && getLcmId(data);
    }
  }, [
    formData?.opCoId,
    formData?.designComponentId,
    formData?.deploymentStatusId,
  ]);

  const getLcmId = async (data: getLcmIdObject) => {
    let response = await GetLCMID(data);
    setLcmIdResponse(response);
    let copy = { ...formData } as PlannedActivityDtoUpdate;
    copy.lcmEngineeringId = response?.data;
    setFormData(copy);
  };

  const [allowedPlannedActivity, setAllowedPlannedActivity] = useState<
    number[]
  >([]);

  const ManagePlannedActivityAllowed = () => {
    const selectedDeploymentStatus =
      formData?.deploymentStatusReosurce &&
      dictionaryToArrayDeploymentStatusDto(
        formData?.deploymentStatusReosurce
      ).find((el) => el.key === formData?.deploymentStatusId);
    const AllowedPlanned = selectedDeploymentStatus?.value
      .plannedActivityResourceAllowedId as number[];
    const checkPlannedActivityRequired =
      formData &&
      formData?.deploymentStatusReosurce &&
      selectedDeploymentStatus &&
      formData?.deploymentStatusReosurce[selectedDeploymentStatus?.key]
        .checkPlannedActivity;

    setCheckPlannedActivityRequired(checkPlannedActivityRequired ?? false);
    setAllowedPlannedActivity(AllowedPlanned ?? []);
  };

  const onChangeLocation = async (obj: any) => {
    let copy = { ...formData } as NetworkElementAsPlannedDtoUpdate;
    copy.deploymentTypeId = undefined;
    if (copy.deploymentTypeReosurce != undefined) {
      if (obj && obj["value"] && obj["value"]["locationTypeId"]) {
        let target = dictionaryToArray(copy.deploymentTypeReosurce).find(
          (x) => x.key == obj["value"]["locationTypeId"]
        )?.key;
      }
    }
    if (obj && obj["key"]) {
      copy.locationId = obj["key"];
      const result = await GetLocationTypeById(obj["key"]);
      copy.deploymentTypeReosurce = result?.data?.deploymentTypeRelatedDict;
    } else {
      copy.locationId =
        copy.locationReosurce &&
        dictionaryToArrayLocationDto(copy.locationReosurce).find(
          (x) => x.value.defaultValue == true
        )?.key;
    }
    setFormData(copy);
  };

  const rtnDisableTab = () => {
    if (formData !== undefined && formData !== null) {
      if (
        formData.plannedActivityDto &&
        formData.plannedActivityDto?.length > 0
      ) {
        return false;
      } else if (props.edit) {
        return false;
      } else if (
        numberIsNullOrZero(formData.opCoId) ||
        numberIsNullOrZero(formData.designComponentId) ||
        numberIsNullOrZero(formData.deploymentStatusId)
      ) {
        return true;
      } else if (formData.plannedAction === false && !props.edit) {
        return true;
      } else {
        return false;
      }
    } else {
      return true;
    }
  };

  const validateResourceKeys = (key, event) => {
    if (key === "swResourceKey") {
      onChange("swResourceKey", event);
      if (/^1[0-9A-Za-z]{7}$/.test(event.target.value)) setSwKeyError(false);
      else setSwKeyError(true);
    }
    if (key === "previousSWResourceKey") {
      onChange("previousSWResourceKey", event);
      if (event.target.value.length <= 255) setPrevSwKeyError(false);
      else setPrevSwKeyError(true);
    }
    if (key === "hwResourceKey") {
      onChange("hwResourceKey", event);
      if (/^2[0-9A-Za-z]{7}$/.test(event.target.value)) setHwKeyError(false);
      else setHwKeyError(true);
    }
    if (key === "previousHWResourceKey") {
      onChange("previousHWResourceKey", event);
      if (event.target.value.length <= 255) setPrevHwKeyError(false);
      else setPrevHwKeyError(true);
    }
  };

  return (
    <div className="col-12">
      <ModalConfirm data={confirmForm} />
      <ModalConfirm data={confirmPlanned} />
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
      <Tabs
        defaultActiveKey={keyTabs}
        id="uncontrolled-tab-example"
        activeKey={keyTabs}
        onSelect={(x) => setKey(x || "")}
      >
        <Tab eventKey="networkelement" title="Network Element" className="">
          <form onChange={() => setChanged(true)}>
            <div className="row col-12 px-0 mx-0 mt-4">
              <div className="col-12">
                <fieldset className="fieldset">
                  {/* <legend className="  red">Implementation:</legend> */}
                  <div className="row">
                    <label className="text-bb">Implementation Details</label>
                    <div className="col-6 pl-0">
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold w-100">
                          OpCo<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.opCoReosurce &&
                                  dictionaryToArray(formData?.opCoReosurce)
                                }
                                value={
                                  formData?.opCoReosurce &&
                                  dictionaryToArray(
                                    formData?.opCoReosurce
                                  ).filter((x) => x.key === formData?.opCoId)
                                }
                                onChange={(e) => {
                                  formData?.linkedToNetworkAsIs
                                    ? setConfirmPlanned({
                                        title: "Save changes?",
                                        message:
                                          "The linked network element as-is will be change , Are you sure you want to change?",
                                        button: "Continue",
                                        item: 0,
                                        isOpen: true,
                                        actions: {
                                          cancel: () => {
                                            setConfirmPlanned(stateConfirm);
                                          },
                                          confirm: () => {
                                            onChangeOpCO(e);
                                            setConfirmPlanned(stateConfirm);
                                          },
                                        },
                                      })
                                    : onChangeOpCO(e);
                                }}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isDisabled={props.edit ? true : false}
                                isClearable
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                              ></Select>
                            </div>
                            {tipologicaPermesso &&
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
                              null}
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
                      <div className="form-group col-12 pl-0">
                        <label className="labelForm voda-bold mb-0 w-100">
                          Element Deployment Name<span className="red">*</span>
                        </label>
                        <div className="d-flex">
                          <div className="w-100">
                            <input
                              type="text"
                              onChange={(e) => onChange("elementName", e)}
                              onKeyUp={(e) => onChange("elementName", e)}
                              className="inputForm w-100"
                              disabled={!editName && props.edit}
                              value={formData?.elementName}
                            />
                            {validation &&
                            validation.response == false &&
                            validation.property?.includes("elementName") ? (
                              <label className="validation">
                                *Element Name must have a value
                              </label>
                            ) : null}
                          </div>
                          {props.edit && (
                            <button
                              type="button"
                              title={"Edit Element Deployment Name"}
                              className="btn btn-link"
                              onClick={() => setEditName(!editName)}
                            >
                              <img
                                className="btnEdit op-55"
                                src={require("../../img/edit.png")}
                              />
                            </button>
                          )}
                        </div>
                      </div>
                      <div className="form-group col-12 pl-0">
                        <label className="labelForm voda-bold mb-0 w-100">
                          Element Domain Name
                        </label>
                        <div className="d-flex">
                          <div className="w-100">
                            <input
                              type="text"
                              maxLength={1000}
                              onChange={(e) => {
                                onChange("elementDomianName", e);
                                if (e.target.value.length > 0) {
                                  setShowTypingHint(true);
                                } else {
                                  setShowTypingHint(false);
                                }
                              }}
                              onKeyUp={(e) => onChange("elementDomianName", e)}
                              className="inputForm w-100"
                              disabled={!editDomain && props.edit}
                              value={formData?.elementDomianName}
                            />
                            {showTypingHint && (
                              <label
                                style={{ color: "red", fontWeight: "bold" }}
                              >
                                Kindly use ';' to add more than one value
                              </label>
                            )}
                          </div>
                          {props.edit && (
                            <button
                              type="button"
                              title={"Edit Element Domain Name"}
                              className="btn btn-link"
                              onClick={() => setEditDomain(!editDomain)}
                            >
                              <img
                                className="btnEdit op-55"
                                src={require("../../img/edit.png")}
                              />
                            </button>
                          )}
                        </div>
                      </div>

                      {props.edit && (
                        <div className="mt-4 col-12 pl-0 form-group">
                          <label className="labelForm voda-bold w-100 mb-0">
                            Node Index<span className="red">*</span>
                            <input
                              type="text"
                              disabled
                              className="inputForm w-100"
                              defaultValue={formData?.nodeIndex}
                            />
                          </label>
                        </div>
                      )}
                    </div>

                    <div className="col-6 pr-0">
                      {!props.edit ? (
                        <div className="col-12 pr-0">
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
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso &&
                                // <button
                                //   className="btn btn-link"
                                //   onClick={() => setIsVisibleModalLookup(2)}
                                //   type="button"
                                // >
                                //   <img
                                //     style={{ height: 15 }}
                                //     src={require("../../img/plus_icon.png")}
                                //     alt="+"
                                //   />
                                // </button>
                                null}
                            </div>
                            {validation &&
                            validation.response == false &&
                            validation.property?.includes(
                              "originalEquipmentManufacturerId"
                            ) ? (
                              <label className="validation">
                                *This Field must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      ) : null}
                      <div className="col-12 pr-0">
                        <label className="labelForm w-100">
                          <label
                            className="labelForm voda-bold mb-0"
                            title={
                              formData?.designComponentReosurce &&
                              dictionaryToArray(
                                formData?.designComponentReosurce
                              )
                                .find(
                                  (x) => x.key === formData?.designComponentId
                                )
                                ?.value.replace(
                                  '<b class="text-lowercase">',
                                  ""
                                )
                                .replace("</b>", "")
                            }
                          >
                            Current Design Component
                            <span className="red">*</span>
                          </label>
                          <Select
                            menuPosition={"fixed"}
                            options={
                              formData?.designComponentReosurce &&
                              dictionaryToArray(
                                formData?.designComponentReosurce
                              ).sort((a, b) =>
                                a.value.toLowerCase() < b.value.toLowerCase()
                                  ? -1
                                  : 1
                              )
                            }
                            value={
                              formData?.designComponentReosurce &&
                              dictionaryToArray(
                                formData?.designComponentReosurce
                              ).filter(
                                (x) => x.key === formData?.designComponentId
                              )
                            }
                            onChange={(e) => {
                              formData?.linkedToNetworkAsIs
                                ? setConfirmPlanned({
                                    title: "Save changes?",
                                    message:
                                      "This item is already linked to a Network Element As-Is record. Are you sure you want to change?",
                                    button: "Continue",
                                    item: 0,
                                    isOpen: true,
                                    actions: {
                                      cancel: () => {
                                        setConfirmPlanned(stateConfirm);
                                      },
                                      confirm: () => {
                                        onChangeSelect("designComponentId", e);
                                        setConfirmPlanned(stateConfirm);
                                      },
                                    },
                                  })
                                : onChangeSelect("designComponentId", e);
                            }}
                            onBlur={() => setInputValue("")}
                            isSearchable
                            isClearable
                            isDisabled={props?.edit ? true : false}
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
                          validation.property?.includes("designComponentId") ? (
                            <label className="validation">
                              *Design Component must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>

                      {props.edit ? (
                        <div className="col-12 pr-0">
                          <label className="labelForm voda-bold w-100">
                            Current Bag <span className="red">*</span>
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.buildBagResources &&
                                    resourceArrayRefactor(
                                      formData?.buildBagResources
                                    )
                                  }
                                  value={
                                    formData?.buildBagResources &&
                                    resourceArrayRefactor(
                                      formData?.buildBagResources
                                    ).filter(
                                      (x) => x.key === formData?.buildBagId
                                    )
                                  }
                                  onChange={(e) => {
                                    onChangeSelect("buildBagId", e);
                                  }}
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                  isDisabled={props.edit ? true : false}
                                ></Select>
                              </div>
                              {/* {tipologicaPermesso && !props.formDisabed && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsBuildBagItemFlag(true)}
                                type="button"
                              >
                                <img
                                  style={{ height: 15 }}
                                  src={require("../../img/plus_icon.png")}
                                  alt="plus"
                                />
                              </button>
                            )} */}
                            </div>
                            {validation &&
                            validation.response == false &&
                            validation.property?.includes("buildBagId") ? (
                              <label className="validation h-16">
                                *Current Bag must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      ) : null}
                    </div>
                  </div>
                </fieldset>
              </div>
              {props?.edit && (
                <div className="col-12">
                  <fieldset className="fieldset px-0">
                    <div className="row">
                      <div className="col-12">
                        {" "}
                        <label className="labelForm voda-bold">
                          View Resource Keys
                        </label>
                        <label className="labelForm voda-bold">
                          <div className="switchSmall ml-2">
                            <input
                              type="checkbox"
                              onChange={(e) =>
                                setToggleResource(e.target.checked)
                              }
                              className="mr-1"
                              checked={toggleResource}
                            />
                            <span className="sliderSmall round"></span>
                          </div>
                        </label>
                      </div>
                    </div>
                    {toggleResource && (
                      <div className="row">
                        <div className="col-6">
                          <div className="col-12 pl-0">
                            <label className="labelForm voda-bold mb-0">
                              SW ResourceKey
                            </label>
                            <div className="d-flex mb-0">
                              <label className="labelForm voda-bold mb-0 w-90">
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    validateResourceKeys("swResourceKey", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.swResourceKey}
                                  disabled={!editSwKeyName && props.edit}
                                />
                              </label>
                              {props.edit && (
                                <button
                                  type="button"
                                  title={"Edit Sw Resource Key"}
                                  className="btn btn-link"
                                  onClick={() =>
                                    setEditSwKeyName(!editSwKeyName)
                                  }
                                >
                                  <img
                                    className="btnEdit op-55"
                                    src={require("../../img/edit.png")}
                                  />
                                </button>
                              )}
                            </div>
                            {swKeyError === true ||
                            (validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "swResourceKey"
                              )) ? (
                              <label className="validationFeild h-16 mb-1 w-100">
                                Sw Resource Key must start with '1', followed by
                                7 alphanumeric characters.
                              </label>
                            ) : null}
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 pr-0">
                            <label className="labelForm voda-bold mb-0">
                              Previous SW ResourceKey
                            </label>
                            <div className="d-flex mb-0">
                              <label className="labelForm voda-bold mb-0 w-90">
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    validateResourceKeys(
                                      "previousSWResourceKey",
                                      e
                                    )
                                  }
                                  className="inputForm w-100"
                                  value={formData?.previousSWResourceKey}
                                  disabled={!editPrevSwKeyName && props.edit}
                                />
                              </label>
                              {props.edit && (
                                <button
                                  type="button"
                                  title={"Edit Previous Sw Resource Key"}
                                  className="btn btn-link"
                                  onClick={() =>
                                    setEditPrevSwKeyName(!editPrevSwKeyName)
                                  }
                                >
                                  <img
                                    className="btnEdit op-55"
                                    src={require("../../img/edit.png")}
                                  />
                                </button>
                              )}
                            </div>
                            {prevSwKeyError === true ||
                            (validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "previousSWResourceKey"
                              )) ? (
                              <label className="validationFeild h-16 mb-1 w-100">
                                Previous Sw Resource Key must be within 255
                                characters.
                              </label>
                            ) : null}
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 pl-0">
                            <label className="labelForm voda-bold mb-0">
                              HW ResourceKey
                            </label>
                            <div className="d-flex mb-0">
                              <label className="labelForm voda-bold mb-0 w-90">
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    validateResourceKeys("hwResourceKey", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.hwResourceKey}
                                  disabled={!editHwKeyName && props.edit}
                                />
                              </label>
                              {props.edit && (
                                <button
                                  type="button"
                                  title={"Edit Hw Resource Key"}
                                  className="btn btn-link"
                                  onClick={() =>
                                    setEditHwKeyName(!editHwKeyName)
                                  }
                                >
                                  <img
                                    className="btnEdit op-55"
                                    src={require("../../img/edit.png")}
                                  />
                                </button>
                              )}
                            </div>
                            {hwKeyError === true ||
                            (validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "hwResourceKey"
                              )) ? (
                              <label className="validationFeild h-16 mb-1 w-100">
                                Hw Resource Key must start with '2', followed by
                                7 alphanumeric characters.
                              </label>
                            ) : null}
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 pr-0">
                            <label className="labelForm voda-bold mb-0">
                              Previous HW ResourceKey
                            </label>
                            <div className="d-flex mb-0">
                              <label className="labelForm voda-bold mb-0 w-90">
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    validateResourceKeys(
                                      "previousHWResourceKey",
                                      e
                                    )
                                  }
                                  className="inputForm w-100"
                                  value={formData?.previousHWResourceKey}
                                  disabled={!editPrevHwKeyName && props.edit}
                                />
                              </label>
                              {props.edit && (
                                <button
                                  type="button"
                                  title={"Edit Previous Hw Resource Key"}
                                  className="btn btn-link"
                                  onClick={() =>
                                    setEditPrevHwKeyName(!editPrevHwKeyName)
                                  }
                                >
                                  <img
                                    className="btnEdit op-55"
                                    src={require("../../img/edit.png")}
                                  />
                                </button>
                              )}
                            </div>
                            {prevHwKeyError === true ||
                            (validation &&
                              validation.response == false &&
                              validation.property?.includes(
                                "previousHWResourceKey"
                              )) ? (
                              <label className="validationFeild h-16 mb-1 w-100">
                                Previous Hw Resource Key must be within 255
                                characters.
                              </label>
                            ) : null}
                          </div>
                        </div>
                      </div>
                    )}
                  </fieldset>
                </div>
              )}
              <div className="col-12 mb-2">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mt-2">Deployment Details</label>
                  <div className="row">
                    <div className="col-6">
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold   w-100">
                          Deployment Status<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.deploymentStatusReosurce &&
                                  dictionaryToArrayDeploymentStatusDto(
                                    formData?.deploymentStatusReosurce
                                  ).map((x) => {
                                    return {
                                      key: x.key,
                                      value:
                                        x.value.deploymentStatusDescription,
                                      rule: x.value.rule,
                                    };
                                  })
                                }
                                value={
                                  formData?.deploymentStatusReosurce &&
                                  dictionaryToArrayDeploymentStatusDto(
                                    formData?.deploymentStatusReosurce
                                  )
                                    .filter(
                                      (x) =>
                                        x.key === formData?.deploymentStatusId
                                    )
                                    .map((x) => {
                                      return {
                                        key: x.key,
                                        value:
                                          x.value.deploymentStatusDescription,
                                        rule: x.value.rule,
                                      };
                                    })
                                }
                                onChange={(e) => onChangeDeploymentStatus(e)}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                getOptionLabel={(option) => option.value ?? ""}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                              ></Select>
                            </div>
                            {tipologicaPermesso && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsVisibleModalLookup(5)}
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
                          validation.response === false &&
                          validation.property?.includes(
                            "deploymentStatusId"
                          ) ? (
                            <label className="validation">
                              *deployment Status must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="form-group col-12 pr-0">
                        <label className="labelForm voda-bold   w-100">
                          Do we have Planned Activity?
                          <div className="d-flex flex-gab-15">
                            <Form.Check
                              inline
                              label="Yes"
                              value="yes"
                              name="group1"
                              disabled={true}
                              type="radio"
                              checked={formData?.plannedAction ? true : false}
                            />
                            <Form.Check
                              inline
                              label="No"
                              value="no"
                              name="group1"
                              disabled={true}
                              type="radio"
                              checked={!formData?.plannedAction ? true : false}
                            />
                          </div>
                        </label>
                      </div>
                    </div>

                    <div className="col-6 pl-0">
                      <div className="col-md-12">
                        <label className="labelForm voda-bold w-100">
                          Asset Live Date
                          {/* <span className="red">*</span> */}
                          <DatePicker
                            selected={
                              formData?.assetLiveStatusDate &&
                              new Date(formData?.assetLiveStatusDate)
                            }
                            onChange={(newDate, e) => {
                              e.preventDefault();
                              onChangeDate("assetLiveStatusDate", newDate);
                            }}
                            className={`inputForm w-100 mt-0`}
                            minDate={new Date(1980, 0, 1)}
                            maxDate={new Date(2999, 0, 1)}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={"NOT SPECIFIED"}
                          />
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes(
                            "assetLiveStatusDate"
                          ) ? (
                            <label className="validation">
                              *Asset Live Date must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6 pr-0">
                      <div className="col-md-12">
                        <label className="labelForm voda-bold w-100">
                          Asset Decommissioning Date
                          <DatePicker
                            selected={
                              formData?.dateAssetDecommissionedAsset &&
                              new Date(formData?.dateAssetDecommissionedAsset)
                            }
                            onChange={(newDate, e) => {
                              e.preventDefault();
                              onChangeDate(
                                "dateAssetDecommissionedAsset",
                                newDate
                              );
                            }}
                            className={`inputForm w-100 mt-0`}
                            minDate={new Date(1980, 0, 1)}
                            maxDate={new Date(2999, 0, 1)}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={"NOT SPECIFIED"}
                          />
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes(
                            "dateAssetDecommissionedAsset"
                          ) ? (
                            <label className="validation">
                              *Asset Decommissioning Date must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div>

              <div className="col-12">
                <fieldset className="fieldset p-0">
                  <legend className="text-bb mt-2">Location Details</legend>
                  <div className="row">
                    <div className="col-6">
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold w-100">
                          Location<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.locationReosurce &&
                                  dictionaryToArrayLocationDto(
                                    formData?.locationReosurce
                                  )
                                    .map((x) => {
                                      return { key: x.key, value: x.value };
                                    })
                                    .filter(
                                      (x) => x.value.opcoId == formData?.opCoId
                                    )
                                }
                                value={
                                  formData?.locationReosurce &&
                                  dictionaryToArrayLocationDto(
                                    formData?.locationReosurce
                                  ).filter((x) => x.key == formData?.locationId)
                                }
                                onChange={(e) => onChangeLocation(e)}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                isDisabled={numberIsNullOrZero(
                                  formData?.opCoId
                                )}
                                getOptionLabel={(option) =>
                                  option.value.description ?? ""
                                }
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
                                  alt="+"
                                />
                              </button>
                            )}
                          </div>
                          {validation &&
                          validation.response == false &&
                          validation.property?.includes("locationId") ? (
                            <label className="validation">
                              *location must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 pr-0">
                        <label className="labelForm voda-bold w-100">
                          Location Type
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.deploymentTypeReosurce &&
                                  dictionaryToArray(
                                    formData?.deploymentTypeReosurce
                                  )
                                }
                                value={
                                  formData?.deploymentTypeReosurce &&
                                  dictionaryToArray(
                                    formData?.deploymentTypeReosurce
                                  ).filter(
                                    (el) =>
                                      el.key === formData?.deploymentTypeId
                                  )
                                }
                                onChange={(e) =>
                                  onChangeSelect("deploymentTypeId", e)
                                }
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                isDisabled={
                                  numberIsNullOrZero(formData?.locationId) ||
                                  numberIsNullOrZero(formData?.opCoId)
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
                          </div>
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes("deploymentTypeId") ? (
                            <label className="validation">
                              *deployment Type must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>

                    <div className="form-group col-6 pr-1-7">
                      <label className="labelForm voda-bold w-100">
                        Environment<span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.environmentReosurce &&
                                dictionaryToArray(
                                  formData?.environmentReosurce
                                ).sort((a, b) =>
                                  a.value.toLowerCase() < b.value.toLowerCase()
                                    ? -1
                                    : 1
                                )
                              }
                              value={
                                formData?.environmentReosurce &&
                                dictionaryToArray(
                                  formData?.environmentReosurce
                                ).filter(
                                  (x) => x.key === formData?.environmentId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("environmentId", e)
                              }
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
                        validation.response === false &&
                        validation.property?.includes("environmentId") ? (
                          <label className="validation">
                            *Environment must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                </fieldset>
              </div>

              <div className="col-12 row mx-0">
                <div className="col-12 pl-0">
                  <fieldset className="fieldset p-0">
                    <legend className="text-bb mt-2">Contacts Details</legend>
                    <div className="row">
                      <div className="col-6">
                        <div className="form-group pr-1">
                          <label className="labelForm voda-bold w-100">
                            Eduspoc
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.eduSpocResource &&
                                    dictionaryToArray(formData?.eduSpocResource)
                                  }
                                  value={
                                    formData?.eduSpocResource &&
                                    dictionaryToArray(
                                      formData?.eduSpocResource
                                    ).filter((x) => {
                                      return (
                                        formData &&
                                        formData?.eduSpocIds?.indexOf(x.key) !=
                                          -1 &&
                                        formData?.eduSpocIds?.indexOf(x.key) !=
                                          undefined
                                      );
                                    })
                                  }
                                  onChange={(e) =>
                                    OnChangeMultiSelect("eduSpocIds", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isMulti
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso &&
                                // <button
                                //   className="btn btn-link"
                                //   onClick={() => setIsVisibleModalLookup(9)}
                                //   type="button"
                                // >
                                //   <img
                                //     style={{ height: 15 }}
                                //     src={require("../../img/plus_icon.png")}
                                //     alt="+"
                                //   />
                                // </button>
                                null}
                            </div>
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes("eduSpocIds") ? (
                              <label className="validation">
                                *Edu-spoc must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      </div>
                      <div className="col-6 pr-0">
                        <div className="form-group pl-4">
                          <label className="labelForm voda-bold w-100">
                            Sub-Domain Spoc
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.subDomainSpocResource &&
                                    dictionaryToArray(
                                      formData?.subDomainSpocResource
                                    )
                                  }
                                  value={
                                    formData?.subDomainSpocResource &&
                                    dictionaryToArray(
                                      formData?.subDomainSpocResource
                                    ).filter((x) => {
                                      return (
                                        formData &&
                                        formData?.subDomainSpocIds?.indexOf(
                                          x.key
                                        ) != -1 &&
                                        formData?.subDomainSpocIds?.indexOf(
                                          x.key
                                        ) != undefined
                                      );
                                    })
                                  }
                                  onChange={(e) =>
                                    OnChangeMultiSelect("subDomainSpocIds", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isMulti={false}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              </div>
                              {tipologicaPermesso &&
                                // <button
                                //   className="btn btn-link"
                                //   onClick={() => setIsVisibleModalLookup(11)}
                                //   type="button"
                                // >
                                //   <img
                                //     style={{ height: 15 }}
                                //     src={require("../../img/plus_icon.png")}
                                //     alt="+"
                                //   />
                                // </button>
                                null}
                            </div>
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "subDomainSpocIds"
                            ) ? (
                              <label className="validation">
                                *Sub-Domain Spoc must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      </div>
                    </div>
                  </fieldset>
                </div>

                <button
                  type="button"
                  onClick={() => setShowFurtherDetails(!showFurtherDetails)}
                  className="further-btn mb-20"
                >
                  Click for further details
                </button>
                {showFurtherDetails && (
                  <div className="col-12 px-0">
                    <fieldset className="fieldset p-0">
                      <div className="row">
                        <div className="col-12">
                          <div className="row">
                            <div className="col-6 pr-5">
                              <label className="labelForm voda-bold w-100">
                                Capacity Plan Reference
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    onChange("capacityPlanReference", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.capacityPlanReference}
                                />
                                {validation &&
                                validation.response == false &&
                                validation.property?.includes(
                                  "capacityPlanReference"
                                ) ? (
                                  <label className="validation">
                                    *Capacity Plan Reference must have a value
                                  </label>
                                ) : null}
                              </label>
                            </div>
                            <div className="col-6">
                              <div className="form-group col-12">
                                <label className="labelForm voda-bold w-100">
                                  Network Construct
                                  <div className="d-flex">
                                    <div className="w-100">
                                      <input
                                        type="text"
                                        onChange={(e) =>
                                          onChange("networkConstruct", e)
                                        }
                                        onKeyUp={(e) =>
                                          onChange("networkConstruct", e)
                                        }
                                        className="inputForm w-100"
                                        value={formData?.networkConstruct}
                                      />
                                    </div>
                                  </div>
                                </label>
                              </div>
                            </div>
                            {props.edit === true ? (
                              <div className="col-6 form-group">
                                <label className="labelForm voda-bold w-100">
                                  NFVI Bundle ID
                                  <div className="d-flex">
                                    <div className="w-100">
                                      <Select
                                        menuPosition={"fixed"}
                                        options={
                                          formData?.nfviBundleIDReosurce &&
                                          dictionaryToArray(
                                            formData?.nfviBundleIDReosurce
                                          ).sort((a, b) =>
                                            a.value.toLowerCase() <
                                            b.value.toLowerCase()
                                              ? -1
                                              : 1
                                          )
                                        }
                                        value={
                                          formData?.nfviBundleIDReosurce &&
                                          dictionaryToArray(
                                            formData?.nfviBundleIDReosurce
                                          ).filter(
                                            (x) =>
                                              x.key === formData?.nfviBundleIDId
                                          )
                                        }
                                        onChange={(e) =>
                                          onChangeSelect("nfviBundleIDId", e)
                                        }
                                        onBlur={() => setInputValue("")}
                                        isSearchable
                                        isClearable
                                        getOptionLabel={(option) =>
                                          option.value
                                        }
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
                                    </div>
                                    {tipologicaPermesso && (
                                      <button
                                        className="btn btn-link"
                                        onClick={() =>
                                          setIsVisibleModalLookup(10)
                                        }
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
                                  validation.response === false &&
                                  validation.property?.includes(
                                    "nfviBundleIDId"
                                  ) ? (
                                    <label className="validation">
                                      *nfvi Bundle ID must have a value
                                    </label>
                                  ) : null}
                                </label>
                              </div>
                            ) : null}
                          </div>
                        </div>

                        <div className="col-12">
                          <div className="row">
                            <div className="col-6 pr-5">
                              <label className="labelForm voda-bold w-100">
                                Additional Information 1
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    onChange("additionalInformation1", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.additionalInformation1}
                                />
                              </label>
                            </div>
                            <div className="col-6 plr-2">
                              <label className="labelForm voda-bold w-100">
                                Additional Information 2
                                <input
                                  type="text"
                                  onChange={(e) =>
                                    onChange("additionalInformation2", e)
                                  }
                                  className="inputForm w-100"
                                  value={formData?.additionalInformation2}
                                />
                              </label>
                            </div>
                          </div>
                        </div>

                        <div className="col-12">
                          <div className="row mt-2">
                            {props.edit === true ? (
                              <>
                                <div className="col-6 pr-5">
                                  <label className="labelForm voda-bold w-100">
                                    Last Modified
                                    <input
                                      readOnly={true}
                                      className=" form-group inputForm w-100 voda-regular"
                                      type="text"
                                      defaultValue={formatDateWithTime(
                                        formData?.lastModified
                                      )?.toUpperCase()}
                                    />
                                  </label>
                                </div>
                                <div className="col-6 plr-2">
                                  <label className="labelForm voda-bold w-100">
                                    Last Modified By
                                    <input
                                      readOnly={true}
                                      className="inputForm w-100 voda-regular"
                                      type="text"
                                      defaultValue={formData?.lastModifiedBy}
                                    />
                                  </label>
                                </div>
                              </>
                            ) : null}
                          </div>
                        </div>
                      </div>
                    </fieldset>
                  </div>
                )}
              </div>

              {/* <div className="col-12 mb-2">
                <fieldset className="fieldset p-0">
                  <label className="text-bb mt-2">Domain Details</label>
                  <div className="row">
                    <div className="col-6">
                      <div className="col-12 pl-0">
                        <label className="labelForm voda-bold   w-100">
                          Domain<span className="red">*</span>
                          <div className="d-flex">
                            <div className="w-100">
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  formData?.systemNames &&
                                  dictionaryToArray(formData?.systemNames).sort(
                                    (a, b) =>
                                      a.value.toLowerCase() <
                                      b.value.toLowerCase()
                                        ? -1
                                        : 1
                                  )
                                }
                                value={
                                  formData?.systemNames &&
                                  dictionaryToArray(
                                    formData?.systemNames
                                  ).filter(
                                    (x) => x.key === formData?.domainNameId
                                  )
                                }
                                onChange={(e) =>
                                  onChangeSelect("domainNameId", e)
                                }
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
                            </div>
                            {tipologicaPermesso && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsVisibleModalLookup(15)}
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
                          validation.response === false &&
                          validation.property?.includes(
                            "deploymentStatusId"
                          ) ? (
                            <label className="validation">
                              *deployment Status must have a value
                            </label>
                          ) : null}
                        </label>
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div> */}
            </div>
          </form>
        </Tab>
        <Tab
          eventKey="plannedActivities"
          title="Planned Activities"
          className=""
          disabled={rtnDisableTab()}
        >
          <NetworkElementAsPlannedActivity
            idDetail={props.idDetail}
            selectedDeploymentStatus={selectedDeploymentStatus}
            plannedActivityAllowed={allowedPlannedActivity}
            pagination={query}
            action={{
              Delete,
              Edit,
              New,
              setChanged,
              Filter: setQuery,
              AddOnList,
              GetElementFromList,
              setShowForm,
              setConfirm: setConfirmPlanned,
              closeModal: props.action.closeModal,
            }}
            showForm={showForm}
            principalId={editResource?.networkElementAsPlannedId ?? 0}
            formArray={formData?.plannedActivityDto}
            opco={opco}
            opcoId={formData?.opCoId}
            buildBagIds={formData?.buildBagId}
            designComponent={designComponent}
            softwareManufacturer={softwareManufacturer}
            productImportance={undefined}
            numberOfNodesInProd={0}
            numberOfNodesInLab={0}
            eduSpoc={eduSpoc}
            edit={props.edit}
            subdomainSpoc={subdomainSpoc}
            isForAddAsset={!props.edit}
            isForEditAsset={props.edit}
            isDesignAspect={false}
            plannedActivityTypeForEnum={
              props.edit
                ? PlannedActivityTypeForEnum["EditAsset"]
                : PlannedActivityTypeForEnum["AddAsset"]
            }
          />
        </Tab>
      </Tabs>
      <div className="col-12 justify-content-end mt-4 d-flex footerModal">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        {(!checkPlannedActivityRequired ||
          formData?.plannedActivityDto?.length ||
          props.edit ||
          keyTabs === "plannedActivities") && (
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={() => SaveOrConfirmPlanned()}
            type="button"
          >
            Save
          </button>
        )}

        {checkPlannedActivityRequired &&
          !props.edit &&
          !formData?.plannedActivityDto?.length &&
          keyTabs !== "plannedActivities" && (
            <button
              className="voda-bold btn btn-danger px-4 btnHeader"
              onClick={() => goToPlannedActivity()}
              type="button"
              disabled={rtnDisableTab()}
            >
              Next
            </button>
          )}
      </div>
    </div>
  );
};

export default NetworkElementAsPlannedModal;
