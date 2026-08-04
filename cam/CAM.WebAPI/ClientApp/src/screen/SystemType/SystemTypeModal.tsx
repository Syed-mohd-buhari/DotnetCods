import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { Tabs, Tab, Modal } from "react-bootstrap";
import ModalMajorHardware from "../SystemType/SystemTypeModalMajorHardware";
import {
  SystemTypeDtoCreate,
  MajorHardwareBuildMainSystemTypeDto,
  SystemTypeDtoUpdate,
  SystemTypeReleatedMajorEntity,
} from "../../Model/SystemTypeModel";
import { GET_GRID_MAJOR_SOFTWARE_BUILD } from "../../Model/MajorSoftwareBuild";
import { GET_GRID_MAJOR_HARDWARE_BUILD } from "../../Model/MajorHardwareBuild";
import { GetMajorHardwareBuildGrid } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildGridAction";
import {
  formatTime,
  formatDateWithTime,
  formatTimeLocal,
} from "../../Hook/Common";
import { CreatSystemType } from "../../Redux/Action/SystemType/SystemTypeCreateAction";
import { useSelector } from "react-redux";
import { GetMajorSoftwareBuildGrid } from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildGridAction";
import { EditSystemType } from "../../Redux/Action/SystemType/SystemTypeEditAction";
import Select from "react-select";
import AsyncSelect from "react-select/async";
import {
  GetCostraintInfo,
  GetSystemSolutionName,
  GetAssetCategoryReleated,
  GetMinorDateFromMajorEntity,
  GetVodafoneNameResource,
  GetVodafoneNameResourceWizardMode,
  GetAllSubdomainAndVerticalResponsibles,
} from "../../Redux/Action/SystemType/SystemTypeCommonAction";
import {
  MajorHardwareBuildDtoCreate,
  MajorHardwareBuildQueryObjectGrid,
} from "../../Model/MajorHardwareBuild";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import ProductImportanceContainer from "../../Containers/Lookup/ProductImportanceContainer";
import VerticalResponsibleContainer from "../../Containers/Lookup/VerticalResponsibleContainer";
import SubDomainResponsibleContainer from "../../Containers/Lookup/SubDomainResponsibleContainer";
import AssetCategoryContainer from "../../Containers/Lookup/AssetCategoryContainer";
import AssetTypeContainer from "../../Containers/Lookup/AssetTypeContainer";
import AssetClassContainer from "../../Containers/Lookup/AssetClassContainer";
import SubdomainSpocContainer from "../../Containers/Lookup/SubdomainSpocContainer";
import { useAuth } from "../../Hook/useAuth";
import { RelatedResource } from "../../Model/CommonModels";
import { colourStyles } from "../../Hook/Common";
import ModalConfirm from "../../Components/ModalConfirm";
import Container from "../../Components/Container";
import {
  dictionaryToArray,
  dictionaryToArrayRelatedResource,
  dictionaryToArrayAssetCategoryDto,
} from "../../Hook/Dictionary";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import DatePicker from "react-datepicker";
import { MajorSoftwareBuildDtoCreate } from "../../Model/MajorSoftwareBuild";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { AssetCategoryDto } from "../../Model/LookUp/AssetCategory";
import VodafoneName from "../../Containers/Lookup/VodafoneNameContainer";
import { TextInputComponent } from "../../Components/FormField";
import { GetOpCosAndVerticalAndSubDomainResponsibles } from "../../Redux/Action/OrganizationInfo/OrganizationInfoGridAction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import { useDispatch } from "react-redux";
import TourGuide from "../../Components/TourGuide";
import { endGuideTour, startGuideTour } from "../../Redux/Action/tourActions";
import { getSystemTypeModalTourSteps } from "../../Constant/TourSteps";

let QueryMajorHardware = {
  majorHardwareBuildId: [],
  name: "",
  originalEquipmentManufacturerId: [],
  hardwareSolution: [],
  platform: [],
  hardwareType: [],
  otherHardwareInfo: [],
  lastTimeBuyNewStartDate: undefined,
  lastTimeBuyNewEndDate: undefined,
  lastTimeBuyUpgradesStartDate: undefined,
  lastTimeBuyUpgradesEndDate: undefined,
  lastTimeBuyExpansionsStartDate: undefined,
  lastTimeBuyExpansionsEndDate: undefined,
  lastModifiedStartDate: undefined,
  lastModifiedEndDate: undefined,
  endOfMaintenanceStartDate: undefined,
  endOfMaintenanceEndDate: undefined,
  endOfsupportStartDate: undefined,
  endOfsupportEndDate: undefined,
  vulnerabilityStatus: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: undefined,
  pageSize: undefined,
  principalId: undefined,
};

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: SystemTypeDtoCreate,
      property: string
    );
    wizardBackFunction?(formData: SystemTypeDtoCreate, property: string): any;
    setConfirmExitWizard?(): any;
  };
  majorHardwareDto?: MajorHardwareBuildDtoCreate;
  majorSoftwareDto?: MajorSoftwareBuildDtoCreate;
  edit: boolean;
  keyTab?: string;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: SystemTypeDtoCreate;
  systemTypeRedirect?: boolean;
  prevPage?: string;
}

const ModalSystemType: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("systemtype");

  const {
    formData,
    setFormData,
    checkIsExist,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeDate,
    onChangeSelect,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<SystemTypeDtoUpdate>(CreatSystemType, EditSystemType);
  const dtoEditResourceState = (state: RootState) =>
    state.systemTypeEditReducer.SystemTypeDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.systemTypeCreateReducer.SystemTypeDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const [disableSelect, setDisableSelect] = useState<boolean>(false);
  const [apiSWObject, setApiSWObject] = useState<any>();
  const [apiHWObject, setApiHWObject] = useState<any>();
  const [majorEntityRelated, setMajorEntityRelated] = useState<
    SystemTypeReleatedMajorEntity | undefined
  >();
  const [resourceMajorSoftwareBuild, setResourceMajorSoftwareBuild] = useState<
    | {
        key: number;
        value: string;
        color?: string | "#000000";
        order: number | 0;
      }[]
    | undefined
  >([]);

  const GridSubSpocDto = (state: RootState) =>
    state.subDomainSpocGridReducer.LookUpGridResultAll;
  let GridSubDomainSpocDto = useSelector(GridSubSpocDto);

  const GridSubDomainResponsibleAll = (state: RootState) =>
    state.subDomainResponsibleGridReducer.LookUpGridResultAll;
  const GridSubdomainResponsibleDtoAll = useSelector(
    GridSubDomainResponsibleAll
  );

  const [assetClassName, setAssetClassName] = useState<string>("");

  const [showFurtherSection, setShowFurtherSection] = useState(false);
  const dispatch = useDispatch();

  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );

  const handleEndTour = () => dispatch(endGuideTour());

  const majorSoftware = useSelector(
    (state: RootState) =>
      state.majorSoftwareBuildGridReducer.MajorSoftwareBuildGridResult
  );

  const chnageShowFurtherSection = () => {
    setShowFurtherSection((prevState) => !prevState);
  };

  let resourceMajorSoftware = majorSoftware?.items?.map((s) => {
    return {
      key: s.majorSoftwareBuildId,
      value:
        `${s.originalEquipmentManufacturer} - ${s.productName} - ${s.softwareVersion}` ??
        " ",
    } as {
      key: number;
      value: string;
      order: number;
    };
  });

  const [resourceMajorHardwareBuild, setResourceMajorHardwareBuild] = useState<
    | {
        key: number;
        value: string;
        color?: string | "#000000";
        order: number | 0;
      }[]
    | undefined
  >([]);

  const majorHardware = useSelector(
    (state: RootState) =>
      state.majorHardwareBuildGridReducer.MajorHardwareBuildGridResult
  );

  let resourceMajorHardware = majorHardware?.items?.map((s) => {
    return {
      key: s.majorHardwareBuildId,
      value:
        `${s.originalEquipmentManufacturer} - ${s.hardwareSolution} - ${s.platform}  - ${s.hardwareType}` ??
        " ",
    } as { key: number; value: string; order: number };
  });

  useEffect(() => {
    if (
      formData &&
      formData?.assetCategoryId &&
      formData.assetCategoryId != null
    ) {
      if (majorEntityRelated && majorEntityRelated != null) {
        associaColorHardwareSoftware(majorEntityRelated);
      } else {
        relatesAssetCategory(formData?.assetCategoryId);
      }
    } else {
      setResourceMajorSoftwareBuild(resourceMajorSoftware);
      setResourceMajorHardwareBuild(resourceMajorHardware);
    }
  }, [majorSoftware, majorHardware, keyTabs]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      let principalIdList =
        formData?.majorHardwareBuildId &&
        formData?.majorHardwareBuildId.map((x) => x.majorHardwareBuildId ?? 0);
      let filtro: MajorHardwareBuildQueryObjectGrid | undefined =
        principalIdList === undefined
          ? undefined
          : { principalIdList: principalIdList };
      let copy = { ...editResource } as SystemTypeDtoUpdate;
      GetMajorSoftwareBuildGrid({
        principalId: editResource?.majorSoftwareBuildsId,
      });
      GetMajorHardwareBuildGrid(filtro);
      if (copy) {
        changeOemAndSystemSolution(
          editResource?.systemTypeNameOem ?? "",
          editResource?.majorSoftwareBuildsId,
          editResource?.majorHardwareBuildId &&
            editResource?.majorHardwareBuildId[0].majorHardwareBuildId,
          copy
        );
      } else {
        setFormData(editResource);
      }
    } else if (!props.wizardMode) {
      setFormData(createResource);
      GetMajorSoftwareBuildGrid();
      GetMajorHardwareBuildGrid();
    } else {
      if (
        props.dataWizard?.majorHardwareBuildId !== undefined &&
        props.dataWizard?.majorHardwareBuildId.length === 1 &&
        props.dataWizard?.majorHardwareBuildId[0].majorHardwareBuildId !== 0
      ) {
        GetSystemSolutionNameForWizard();
      } else {
        CreateSystemSolutionName();
      }
    }
    return () => {
      rootStore.dispatch({ type: GET_GRID_MAJOR_SOFTWARE_BUILD, payload: {} });
      rootStore.dispatch({ type: GET_GRID_MAJOR_HARDWARE_BUILD, payload: {} });
    };
  }, [createResource, editResource, props.edit, props.dataWizard]);

  useEffect(() => {
    if (checkIsExist) {
      setFormData(props.edit ? editResource : createResource);
    }
  }, [checkIsExist]);

  const GetSystemSolutionNameForWizard = async () => {
    if (props.dataWizard !== undefined) {
      const name = CreateSoftwareName();
      let copy = { ...props.dataWizard } as SystemTypeDtoCreate;
      copy.systemSolution = await GetSystemSolutionName(
        name,
        copy?.majorHardwareBuildId[0]?.majorHardwareBuildId ?? undefined,
        copy?.majorSoftwareBuildsId ?? undefined,
        rtnIdsMajorHardware()
      );
      setFormData(copy);

      if (props?.majorSoftwareDto?.productNameId) {
        GetVodafoneNameResourceWizardMode(
          props?.majorSoftwareDto?.productNameId
        ).then((result) => {
          copy.vodafoneNameResource = result;
          copy.vodafoneNameId = dictionaryToArray(result!)[0]?.key;
          copy.vodafoneName = dictionaryToArray(result!)[0]?.value;
          copy.systemTypeNameVodafone = dictionaryToArray(result!)[0]?.value;
          setFormData(copy);
        });
      }
    }
  };

  useEffect(() => {
    if (props.keyTab === "" || props.keyTab == null) {
      setKey("systemtype");
    } else {
      setKey(props.keyTab);
    }

    createAssetClassDescription();
  }, []);

  const removeValidation = (property: string) => {
    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  useEffect(() => {
    createAssetClassDescription();
  }, [
    formData?.majorHardwareBuildId,
    formData?.assetCategoryId,
    formData?.assetCategoryResource,
  ]);

  useEffect(() => {
    if (formData?.majorSoftwareBuildsId && !props.wizardMode) {
      GetVodafoneNameResource(formData?.majorSoftwareBuildsId).then(
        (result) => {
          const copy = { ...formData } as SystemTypeDtoUpdate;
          copy.vodafoneNameResource = result;
          copy.vodafoneNameId = dictionaryToArray(
            copy.vodafoneNameResource!
          )[0]?.key;
          setFormData(copy);
        }
      );
    }
  }, [formData?.majorSoftwareBuildsId]);

  const createAssetClassDescription = () => {
    let newAssetClass = "";
    if (formData != undefined && formData != null) {
      const copy = { ...formData } as SystemTypeDtoUpdate;

      if (
        formData?.assetCategoryId != undefined &&
        formData?.assetCategoryId != null &&
        formData?.assetCategoryResource != undefined
      ) {
        const relatedAssetClass =
          formData?.assetCategoryResource &&
          dictionaryToArrayAssetCategoryDto(
            formData?.assetCategoryResource
          ).find((x) => x.key === formData.assetCategoryId)?.value.idAssetClass;

        // Setto AssetType in base a takeAssetTypeFromTable
        const takeAssetTypeFromTable =
          formData?.assetCategoryResource &&
          dictionaryToArrayAssetCategoryDto(
            formData?.assetCategoryResource
          ).find((x) => x.key === formData.assetCategoryId)?.value
            .takeFromAssetTypeTable;

        if (!takeAssetTypeFromTable) {
          copy.assetType = copy.systemTypeNameVodafone ?? "";
        }

        if (
          relatedAssetClass != undefined &&
          relatedAssetClass != null &&
          formData.assetClassResource != undefined
        ) {
          let assetClassSelected = dictionaryToArrayRelatedResource(
            formData.assetClassResource
          ).find((x) => x.key === relatedAssetClass);
          if (assetClassSelected == undefined) {
            rootStore.dispatch(
              setNotification({
                message:
                  "The Asset Class related to this Asset Category has been deleted",
                notifyType: NotifyType.warning,
              })
            );
          }
          newAssetClass = assetClassSelected?.value.value ?? "";
          if (
            formData?.majorHardwareBuildId !== undefined &&
            formData?.majorHardwareBuildId !== null &&
            formData?.majorHardwareBuildId.length > 0
          ) {
            const RelatedBuildConstruction =
              props.majorHardwareDto?.buildConstructionResource &&
              dictionaryToArray(
                props.majorHardwareDto?.buildConstructionResource
              ).filter(
                (z) => z.key == props.majorHardwareDto?.buildConstructionId
              );
            let buildConstruction = "";
            if (props.wizardMode) {
              buildConstruction =
                RelatedBuildConstruction && RelatedBuildConstruction.length > 0
                  ? RelatedBuildConstruction[0].value
                  : "";
            } else {
              buildConstruction =
                majorHardware?.items?.filter(
                  (x) =>
                    x.majorHardwareBuildId ===
                    formData?.majorHardwareBuildId[0]?.majorHardwareBuildId
                )[0].buildConstruction ?? "";
            }
            newAssetClass +=
              buildConstruction != ""
                ? ` <b> on </b> ${buildConstruction}`
                : "";
          }
          copy.assetClassId = relatedAssetClass;
          copy.assetClassDescription = newAssetClass;
        } else {
          rootStore.dispatch(
            setNotification({
              message: "No one Asset Class is related to this Asset Category ",
              notifyType: NotifyType.warning,
            })
          );
        }
      } else {
        copy.assetClassId = 0;
        copy.assetClassDescription = newAssetClass;
      }

      setFormData(copy);
      removeValidation("assetClassId");
      setAssetClassName(newAssetClass);
    }
    return newAssetClass;
  };

  const { tipologicaPermesso } = useAuth();

  //CHANGE MAJORHARDWAREBUILD
  const onChangeMajorMain = (obj: any) => {
    setChanged(true);
    let copy = { ...formData } as SystemTypeDtoUpdate;
    copy.majorHardwareBuildId = [] as MajorHardwareBuildMainSystemTypeDto[];
    if (obj != null) {
      copy.majorHardwareBuildId.push({
        majorHardwareBuildId: obj && obj["key"],
        isMain: true,
      } as MajorHardwareBuildMainSystemTypeDto);
    }
    if (copy.majorHardwareBuildId != undefined) {
      let ids: number[] | undefined = copy.majorHardwareBuildId
        ? copy.majorHardwareBuildId.map((x) => {
            return x.majorHardwareBuildId ?? 0;
          }) ?? undefined
        : undefined;
      systemSolutionEConstraintInfo(
        copy.systemTypeNameOem ?? "",
        copy.majorSoftwareBuildsId,
        copy?.majorHardwareBuildId[0]?.majorHardwareBuildId ?? null,
        copy,
        ids
      );
    }
    setFormData(copy);

    removeValidation("majorHardwareBuildId");
  };

  //CHANGE MAJORSOFTWAREBUILD
  const onChangeMajorSoftware = async (obj: any) => {
    setChanged(true);
    let copy = { ...formData } as SystemTypeDtoUpdate;
    copy.majorSoftwareBuildsId = 0; //Default setting

    let nameOem = "";

    if (obj != null) {
      copy.majorSoftwareBuildsId = obj && obj["key"];
      nameOem =
        majorSoftware?.items?.find(
          (x) => x.majorSoftwareBuildId == copy.majorSoftwareBuildsId
        )?.productName ?? "";
    }

    changeOemAndSystemSolution(
      nameOem,
      copy.majorSoftwareBuildsId,
      copy?.majorHardwareBuildId &&
        copy?.majorHardwareBuildId[0]?.majorHardwareBuildId,
      copy
    );

    removeValidation("majorSoftwareBuildsId");
  };

  const systemSolutionEConstraintInfo = async (
    nameOem: string,
    majorSoftware: number | undefined,
    majorHardware: number | null,
    copy: SystemTypeDtoUpdate,
    hardware?: number[] | undefined
  ) => {
    let result = await GetCostraintInfo(copy.majorSoftwareBuildsId, hardware);
    let resultMinorDate = await GetMinorDateFromMajorEntity(
      copy.majorSoftwareBuildsId,
      hardware
    );
    if (resultMinorDate?.data != null) {
      copy.endOfMaintenance = resultMinorDate?.data;
    }
    copy.systemSolution = await GetSystemSolutionName(
      nameOem,
      majorHardware ?? undefined,
      majorSoftware ?? undefined,
      rtnIdsMajorHardware()
    );
    copy.lcmStatus = result?.lcmStatus;
    copy.constraintScaling = result?.constraintScaling;
    copy.constraintLcm = result?.constraintLcm;
    setFormData(copy);
  };

  const changeOemAndSystemSolution = async (
    nameOem: string,
    majorSoftware: number | undefined,
    majorHardware: number | undefined,
    copy: SystemTypeDtoUpdate
  ) => {
    copy.systemTypeNameOem = nameOem;
    copy.majorSoftwareBuildsId = majorSoftware ?? 0;
    copy.systemSolution = await GetSystemSolutionName(
      nameOem,
      majorHardware ?? undefined,
      majorSoftware ?? undefined,
      rtnIdsMajorHardware()
    );
    let ids: number[] | undefined = copy.majorHardwareBuildId
      ? copy.majorHardwareBuildId.map((x) => {
          return x.majorHardwareBuildId ?? 0;
        }) ?? undefined
      : undefined;
    let resultMinorDate = await GetMinorDateFromMajorEntity(
      copy.majorSoftwareBuildsId,
      ids
    );
    if (resultMinorDate?.data != null) {
      copy.endOfMaintenance = resultMinorDate?.data;
    }
    setFormData(copy);
  };

  //RICALCOLO SYSTEM SOLUTION
  const systemSolution = async (copy: SystemTypeDtoUpdate) => {
    copy.systemSolution = await GetSystemSolutionName(
      copy.systemTypeNameOem,
      copy?.majorHardwareBuildId
        ? copy?.majorHardwareBuildId[0]?.majorHardwareBuildId ?? 0
        : 0,
      copy?.majorSoftwareBuildsId != undefined
        ? copy?.majorSoftwareBuildsId ?? 0
        : 0,
      rtnIdsMajorHardware()
    );
    setFormData(copy);
    return copy;
  };

  const rtnIdsMajorHardware = () => {
    let ids = [] as Array<number>;
    if (formData?.majorHardwareBuildId != undefined) {
      formData?.majorHardwareBuildId.map((x, i) => {
        if (!x.isMain && x.majorHardwareBuildId != undefined) {
          ids.push(x.majorHardwareBuildId);
        }
      });
    }
    return ids;
  };

  const CreateSoftwareName = () => {
    let name: string = "";
    if (
      props?.majorSoftwareDto != null &&
      props?.majorSoftwareDto != undefined
    ) {
      let majorSoftware = props?.majorSoftwareDto;
      const equipmentManufacturerResource =
        majorSoftware?.originalEquipmentManufacturerResource &&
        dictionaryToArray(majorSoftware?.originalEquipmentManufacturerResource);
      const equipmentManufacturer = equipmentManufacturerResource?.find(
        (el) => el.key === majorSoftware.originalEquipmentManufacturerId
      );
      name += `${equipmentManufacturer?.value} - `;
    }

    if (
      props?.majorSoftwareDto != null &&
      props?.majorSoftwareDto != undefined
    ) {
      let majorSoftware = props?.majorSoftwareDto;

      name += `${majorSoftware?.productName} - ${majorSoftware?.softwareVersion}`;
    }
    return name;
  };

  const CreateSystemSolutionName = async () => {
    let copy = { ...props?.dataWizard } as SystemTypeDtoCreate;
    if (copy !== undefined) {
      let name = CreateSoftwareName();
      if (
        props?.majorHardwareDto !== null &&
        props?.majorHardwareDto !== undefined
      ) {
        if (
          props?.majorSoftwareDto !== null &&
          props?.majorSoftwareDto !== undefined
        ) {
          name += '<b class="text-lowercase"> on </b>';
        }

        const majorHardware = props?.majorHardwareDto;
        const PlatformResourceArray =
          majorHardware.platformResource &&
          dictionaryToArray(majorHardware?.platformResource);
        const Platform = PlatformResourceArray?.find(
          (e) => e.key === majorHardware.platformId
        );

        if (Platform?.value && majorHardware?.hardwareType) {
          name += `${majorHardware?.hardwareSolution} - ${Platform?.value} - ${majorHardware?.hardwareType}`;
        }
      }

      copy.systemSolution = name ? name : "";

      if (props?.majorSoftwareDto?.productNameId) {
        await GetVodafoneNameResourceWizardMode(
          props?.majorSoftwareDto?.productNameId
        ).then((result) => {
          copy.vodafoneNameResource = result;
          copy.vodafoneNameId = dictionaryToArray(result!)[0]?.key;
          copy.vodafoneName = dictionaryToArray(result!)[0]?.value;
          copy.systemTypeNameVodafone = dictionaryToArray(result!)[0]?.value;
        });
      }
      setFormData(copy);
    }
  };

  //RICALCOLO CONSTRAINT INFO
  const ConstraintInfo = async (data: SystemTypeDtoUpdate) => {
    let hardware: number[] | undefined = data.majorHardwareBuildId
      ? data.majorHardwareBuildId.map((x) => {
          return x.majorHardwareBuildId ?? 0;
        }) ?? undefined
      : undefined;
    let result = await GetCostraintInfo(data.majorSoftwareBuildsId, hardware);
    let resultMinorDate = await GetMinorDateFromMajorEntity(
      data.majorSoftwareBuildsId,
      hardware
    );
    let copy = { ...data };
    if (resultMinorDate?.data != null) {
      copy.endOfMaintenance = resultMinorDate?.data;
    }
    copy.lcmStatus = result?.lcmStatus;
    copy.constraintScaling = result?.constraintScaling;
    copy.constraintLcm = result?.constraintLcm;
    setFormData(copy);
    return copy;
  };

  //SELECT ASYNC SOFTWARE BUILD
  const searchSowtwareBuild = (input: string) => {
    return resourceMajorSoftwareBuild?.filter((x) =>
      x.value.toLowerCase().includes(input.toLowerCase())
    );
  };

  //SELECT ASYNC HARDWARE BUILD
  const searchHardwareBuild = (input: string) => {
    return resourceMajorHardwareBuild?.filter((x) =>
      x.value.toLowerCase().includes(input.toLowerCase())
    );
  };

  //VALIDAZIONE PRE Save
  const validazioneClient = (copy: SystemTypeDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.vodafoneNameId == null ||
      copy?.vodafoneNameId === undefined ||
      copy?.vodafoneNameId === 0
    ) {
      addInvalidProperty("vodafoneNameId");
    }
    if (
      copy?.majorHardwareBuildId == null ||
      copy?.majorHardwareBuildId === undefined ||
      copy?.majorHardwareBuildId[0] === undefined ||
      (copy?.majorHardwareBuildId[0].majorHardwareBuildId == 0 &&
        !props.wizardMode)
    ) {
      addInvalidProperty("majorHardwareBuildId");
    }
    if (
      (copy?.majorSoftwareBuildsId == null ||
        copy?.majorSoftwareBuildsId === undefined ||
        copy?.majorSoftwareBuildsId === 0) &&
      !props.wizardMode
    ) {
      addInvalidProperty("majorSoftwareBuildsId");
    }
    // if (
    //   copy?.systemTypeNameVodafone == null ||
    //   copy?.systemTypeNameVodafone === undefined ||
    //   copy?.systemTypeNameVodafone === ""
    // ) {
    //   addInvalidProperty("systemTypeNameVodafone");
    // }
    if (
      copy?.productImportanceId == null ||
      copy?.productImportanceId === undefined ||
      copy?.productImportanceId === 0
    ) {
      addInvalidProperty("productImportanceId");
    }
    if (
      copy?.assetCategoryId === null ||
      copy?.assetCategoryId === undefined ||
      copy?.assetCategoryId === 0
    ) {
      addInvalidProperty("assetCategoryId");
    }

    // if (
    //   copy?.verticalResponsibleId == null ||
    //   copy?.verticalResponsibleId === undefined ||
    //   copy?.verticalResponsibleId === 0
    // ) {
    //   addInvalidProperty("verticalResponsibleId");
    // }
    // if (
    //   copy?.subDomainResponsibleId == null ||
    //   copy?.subDomainResponsibleId === undefined ||
    //   copy?.subDomainResponsibleId === 0
    // ) {
    //   addInvalidProperty("subDomainResponsibleId");
    // }
    // if (
    //   copy?.subDomainSpocIds == null ||
    //   copy?.subDomainSpocIds === undefined ||
    //   copy?.subDomainSpocIds.length === 0
    // ) {
    //   addInvalidProperty("subDomainSpocIds");
    // }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const onChangeAssetCategory = (e: any) => {
    setDisableSelect(true);
    let copy = { ...formData } as SystemTypeDtoUpdate;
    copy.assetCategoryId = e && e["key"];
    copy.assetClassId = e && e["assetClassId"];
    copy.assetTypeId = undefined;
    copy.assetType = "";
    setFormData(copy);
    relatesAssetCategory(e && e["key"]);

    removeValidation("assetCategoryId");
  };

  const onChangeAssetClass = (e: any, property: string) => {
    let copy = { ...formData } as SystemTypeDtoUpdate;
    copy.assetClassId = e && e["key"];
    copy.assetTypeId = 0;
    setFormData(copy);

    removeValidation(property);
  };

  const associaColorHardwareSoftware = (
    s: SystemTypeReleatedMajorEntity | undefined
  ) => {
    if (s && s != null) {
      let rtnMajorHw = resourceMajorHardware?.map((x) => {
        return {
          key: x.key,
          value: x.value,
          color: s.idMajorHardwareSameAssetCategory?.includes(x.key)
            ? "#a1d100"
            : s.idMajorHardwareOtherAssetCategory?.includes(x.key)
            ? "#ff8900"
            : "#0000FF",
          order: s.idMajorHardwareSameAssetCategory?.includes(x.key)
            ? 1
            : s.idMajorHardwareOtherAssetCategory?.includes(x.key)
            ? 2
            : 3,
        } as {
          key: number;
          value: string;
          color: string | undefined;
          order: number | 0;
        };
      });
      setResourceMajorHardwareBuild(rtnMajorHw);

      let rtnMajorSw = resourceMajorSoftware?.map((x) => {
        return {
          key: x.key,
          value: x.value,
          color: s.idMajorSoftwareSameAssetCategory?.includes(x.key)
            ? "#a1d100"
            : s.idMajorSoftwareOtherAssetCategory?.includes(x.key)
            ? "#ff8900"
            : "#0000FF",
          order: s.idMajorSoftwareSameAssetCategory?.includes(x.key)
            ? 1
            : s.idMajorSoftwareOtherAssetCategory?.includes(x.key)
            ? 2
            : 3,
        } as {
          key: number;
          value: string;
          color: string | undefined;
          order: number | 0;
        };
      });
      setResourceMajorSoftwareBuild(rtnMajorSw);
    }
  };

  const relatesAssetCategory = (id?: number | null | undefined) => {
    if (id != null) {
      GetAssetCategoryReleated(id).then((s) => {
        setDisableSelect(false);
        setMajorEntityRelated(s);
        associaColorHardwareSoftware(s);
      });
    } else {
      let rtnMajorHw = resourceMajorHardware?.map((x) => {
        return { key: x.key, value: x.value, color: "black", order: 1 } as {
          key: number;
          value: string;
          color: string | undefined;
          order: number | 0;
        };
      });
      setResourceMajorHardwareBuild(rtnMajorHw);

      let rtnMajorSw = resourceMajorSoftware?.map((x) => {
        return { key: x.key, value: x.value, color: "black", order: 1 } as {
          key: number;
          value: string;
          color: string | undefined;
          order: number | 0;
        };
      });
      setResourceMajorSoftwareBuild(rtnMajorSw);
      setDisableSelect(false);
    }
  };

  //GET MAJOR HARDWARE LIST FROM MODAL TAB
  const GetMajorHardwareFromList = async (
    items: MajorHardwareBuildMainSystemTypeDto[]
  ) => {
    let copy = { ...formData } as SystemTypeDtoUpdate | SystemTypeDtoCreate;
    copy.majorHardwareBuildId = items;
    if (copy.majorHardwareBuildId !== undefined) {
      copy = await ConstraintInfo(copy);
    }
    setFormData(copy);
  };

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const ProductImportanceRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.productImportanceResource)
      formData.productImportanceResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const VerticalResponsibleRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.verticalResponsibleResource)
      formData.verticalResponsibleResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const SubDomainResponsibleRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.subDomainResponsibleResource)
      formData.subDomainResponsibleResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const AssetCategoryRefillData = (value: Array<any>) => {
    var obj = value.reduce((acc, item) => ({ ...acc, [item.id]: item }), {});
    if (formData && formData?.assetCategoryResource)
      formData.assetCategoryResource = obj as {
        [key: string]: AssetCategoryDto;
      };
    setFormData(formData);
  };

  const AssetTypeRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({
        ...acc,
        [item.id]: {
          id: item.idAssetCategory,
          value: item.description,
        } as RelatedResource,
      }),
      {}
    );
    if (formData && formData?.assetTypeResource)
      formData.assetTypeResource = obj as { [key: string]: RelatedResource };
    setFormData(formData);
  };

  const AssetClassRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({
        ...acc,
        [item.id]: {
          id: item.idAssetCategory?.toString(),
          value: item.description,
        } as RelatedResource,
      }),
      {}
    );
    if (formData && formData?.assetClassResource)
      formData.assetClassResource = obj as { [key: string]: RelatedResource };
    setFormData(formData);
  };
  const SubDomainSpocRefillData = (value: Array<any>) => {
    value = value.filter((item) => item.isSubDomain === true);
    var obj = value.reduce(
      (acc, item) => ({ ...acc, [item.id]: item.description }),
      {}
    );
    if (formData && formData?.subDomainSpocResource)
      formData.subDomainSpocResource = obj as { [key: string]: string };
    setFormData(formData);
  };

  const vodafoneNameRefillData = (value: Array<any>) => {
    if (props.wizardMode) {
      GetVodafoneNameResourceWizardMode(
        props?.majorSoftwareDto?.productNameId
      ).then((result) => {
        const copy = { ...formData } as SystemTypeDtoUpdate;
        copy.vodafoneNameResource = result;
        copy.vodafoneNameId = dictionaryToArray(result!)?.[0]?.key;
        setFormData(copy);
      });
    } else {
      GetVodafoneNameResource(formData?.majorSoftwareBuildsId).then(
        (result) => {
          const copy = { ...formData } as SystemTypeDtoUpdate;
          copy.vodafoneNameResource = result;
          setFormData(copy);
        }
      );
    }

    // var obj = value.reduce(
    //   (acc, item) => ({ ...acc, [item.id]: item.description }),
    //   {}
    // );
    // if (formData && formData?.vodafoneNameResource)
    //   formData.vodafoneNameResource = obj as { [key: string]: string };
    // setFormData(formData);
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as SystemTypeDtoUpdate;
    if (e != null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = undefined;
    }
    setFormData(copy);

    removeValidation(property);
  };

  const ReturnLookupContainer = (value: number) => {
    switch (value) {
      case 1:
        return (
          <ProductImportanceContainer
            returnObject={ProductImportanceRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></ProductImportanceContainer>
        );
      case 2:
        return (
          <VerticalResponsibleContainer
            returnObject={VerticalResponsibleRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></VerticalResponsibleContainer>
        );
      case 3:
        return (
          <SubDomainResponsibleContainer
            returnObject={SubDomainResponsibleRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></SubDomainResponsibleContainer>
        );
      case 4:
        return (
          <AssetCategoryContainer
            returnObject={AssetCategoryRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></AssetCategoryContainer>
        );
      case 5:
        return (
          <AssetClassContainer
            returnObject={AssetClassRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></AssetClassContainer>
        );
      case 6:
        return (
          <AssetTypeContainer
            returnObject={AssetTypeRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></AssetTypeContainer>
        );
      case 7:
        return (
          <SubdomainSpocContainer
            subDomain={false}
            returnObject={SubDomainSpocRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
          ></SubdomainSpocContainer>
        );

      case 8:
        return (
          <VodafoneName
            returnObject={vodafoneNameRefillData}
            modal={{ isModal: true, setIsVisibleModalLookup }}
            showButtons={false}
            isPopup={true}
          />
        );

      default:
        return;
    }
  };

  const onHideModel = () => {
    if (isVisibleModalLookup === 3) {
      let dataCopy = [...(GridSubdomainResponsibleDtoAll?.items ?? [])];
      SubDomainResponsibleRefillData(dataCopy);
    }

    if (isVisibleModalLookup === 7) {
      let dataCopy = [...(GridSubDomainSpocDto?.items ?? [])];
      SubDomainSpocRefillData(dataCopy);
    }
  };

  function CustomHTMLInputReadOnly(props: any) {
    return (
      <div className="form-group col-6 pr-4">
        <label className="labelForm voda-bold  w-100">
          {props.label}
          <div
            className="voda-regular customFakeInput disabled"
            dangerouslySetInnerHTML={{
              __html: props.child
                .replace(/ on /g, "\u00a0on\u00a0")
                .replace(/ with /g, "\u00a0with\u00a0"),
            }}
          ></div>
        </label>
      </div>
    );
  }
  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const RestoreOrphanDeleted = (id: number | undefined) => {
    setOrphanDeleted(true);
    props.action.Edit && props.action.Edit(id);
  };

  const validateWizard = () => {
    let copy = { ...formData } as SystemTypeDtoCreate;
    if (copy) {
      copy.systemTypeNameOem = props.majorSoftwareDto?.productName;
      props.action.validateFormWizard &&
        props.action.validateFormWizard(
          validazioneClient(copy).response,
          copy,
          "systemTypeDto"
        );
    }
  };

  const manageAssetType = (vodafoneName) => {
    const copy = { ...formData } as SystemTypeDtoUpdate;
    copy.assetType =
      vodafoneName ?? dictionaryToArray(copy?.vodafoneNameResource!)[0]?.value;
    copy.systemTypeNameVodafone = dictionaryToArray(
      copy?.vodafoneNameResource!
    )[0]?.value;
    setFormData(copy);
  };

  useEffect(() => {
    if (formData) {
      const takeFromAssetTypeTable =
        formData?.assetCategoryResource &&
        dictionaryToArrayAssetCategoryDto(formData?.assetCategoryResource).find(
          (x) => x.key === formData.assetCategoryId
        )?.value.takeFromAssetTypeTable;

      if (
        !takeFromAssetTypeTable &&
        formData?.assetCategoryId !== undefined &&
        formData?.assetCategoryId !== null
      ) {
        manageAssetType(formData?.systemTypeNameVodafone);
      }
    }
  }, [
    formData?.systemTypeNameVodafone,
    formData?.vodafoneNameId,
    props?.majorSoftwareDto?.vodafoneNameId,
  ]);

  const getAllSubdomainAndVerticalResponsibles = async (type) => {
    let copy = { ...formData } as SystemTypeDtoCreate;

    if (props.wizardMode === true && props.wizardStep === 3) {
      const swKeys = props?.majorSoftwareDto?.designContactIds ?? [];
      const swValues =
        swKeys.length > 0
          ? swKeys
              .filter((key) =>
                props?.majorSoftwareDto?.designContacts?.hasOwnProperty(key)
              )
              .map((key) => props?.majorSoftwareDto?.designContacts?.[key])
          : null;

      const hwKeys = props?.majorHardwareDto?.designContactIds ?? [];
      const hwValues =
        hwKeys.length > 0
          ? hwKeys
              .filter((key) =>
                props?.majorHardwareDto?.designContacts?.hasOwnProperty(key)
              )
              .map((key) => props?.majorHardwareDto?.designContacts?.[key])
          : null;

      const res = await GetOpCosAndVerticalAndSubDomainResponsibles(
        type == "SW"
          ? props?.majorSoftwareDto?.designContactIds ?? [0]
          : props?.majorHardwareDto?.designContactIds ?? [0]
      );

      type == "SW"
        ? setApiSWObject({ ...res, DesignContact: swValues?.join(",") ?? "" })
        : setApiHWObject({ ...res, DesignContact: hwValues?.join(",") ?? "" });
    } else {
      const res = await GetAllSubdomainAndVerticalResponsibles(
        type == "HW"
          ? copy?.majorHardwareBuildId?.[0]?.majorHardwareBuildId
          : 0,
        type == "SW" ? copy?.majorSoftwareBuildsId : 0
      );

      type == "SW" ? setApiSWObject(res) : setApiHWObject(res);
    }
  };

  useEffect(() => {
    if (formData && formData.majorSoftwareBuildsId !== null) {
      getAllSubdomainAndVerticalResponsibles("SW");
    }
  }, [formData?.majorSoftwareBuildsId]);

  useEffect(() => {
    if (formData && formData.majorHardwareBuildId !== null) {
      getAllSubdomainAndVerticalResponsibles("HW");
    }
  }, [formData?.majorHardwareBuildId]);

  return (
    <div className="col-12">
      <ModalConfirm data={confirmForm} />
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => {
          setIsVisibleModalLookup(0);
          onHideModel();
        }}
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
                onHideModel();
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
        <Tab eventKey="systemtype" title="System Type" className="">
          <form id="formSystemType" onChange={() => setChanged(true)}>
            <div className="row col-12 px-0 mx-0">
              <fieldset className="col-12 mt-3 row mx-0">
                <h4 className="text-bb">System Description</h4>
                <div className="row mx-0">
                  {formData && formData.systemSolution ? (
                    <div className="col-12 pl-0 pr-0">
                      <div className="col-12 form-group p-0">
                        <label className="labelForm voda-bold w-100 mb-0">
                          System Solution
                        </label>
                      </div>
                      <div className="col-12 pl-0">
                        <label
                          className="labelForm   w-100"
                          dangerouslySetInnerHTML={{
                            __html:
                              formData && formData.systemSolution != undefined
                                ? formData?.systemSolution
                                    .replace(/ on /g, "\u00a0on\u00a0")
                                    .replace(/ with /g, "\u00a0with\u00a0")
                                : "",
                          }}
                        ></label>
                      </div>
                    </div>
                  ) : null}

                  <div className="col-6 pl-0 pr-0">
                    <div
                      className="form-group w-100"
                      id="systemTypeAddModal_assetCategory_tour"
                    >
                      <label className="labelForm voda-bold w-100">
                        Please select the Asset Category first
                        <span className="red ">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.assetCategoryResource &&
                                dictionaryToArrayAssetCategoryDto(
                                  formData?.assetCategoryResource
                                ).map((x) => {
                                  return {
                                    key: x.key,
                                    value: x.value.description,
                                    takeFromAssetTypeTable:
                                      x.value.takeFromAssetTypeTable,
                                    assetClassId: x.value.assetClassId,
                                  };
                                })
                              }
                              value={
                                formData &&
                                formData?.assetCategoryResource &&
                                dictionaryToArrayAssetCategoryDto(
                                  formData?.assetCategoryResource
                                )
                                  .map((x) => {
                                    return {
                                      key: x.key,
                                      value: x.value.description,
                                      takeFromAssetTypeTable:
                                        x.value.takeFromAssetTypeTable,
                                      assetClassId: x.value.assetClassId,
                                    };
                                  })
                                  .filter(
                                    (x) => x.key == formData?.assetCategoryId
                                  )
                              }
                              onChange={(e) => onChangeAssetCategory(e)}
                              onBlur={() => setInputValue("")}
                              //placeholder={"Asset Category*"}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value ?? ""}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
                            />
                            {validation &&
                            validation.response === false &&
                            validation.property?.includes("assetCategoryId") ? (
                              <label className="validation">
                                *Asset Category must have a value
                              </label>
                            ) : null}
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
                                alt="plus"
                              />
                            </button>
                          )}
                        </div>
                      </label>
                    </div>
                  </div>
                  {!props.wizardMode && !props.edit && (
                    <>
                      <div className="col-12 px-2 row mx-0 pl-0 pr-0 mttb-20">
                        <div className="col-6 pr-4 pl-0">
                          <div
                            className="form-group w-100"
                            id="systemTypeAddModal_majorHardware_tour"
                          >
                            <label className="labelForm voda-bold w-100">
                              Major Hardware <span className="red ">*</span>
                              <Select
                                menuPosition={"fixed"}
                                options={
                                  resourceMajorHardwareBuild
                                    ? resourceMajorHardwareBuild
                                    : undefined
                                }
                                // cacheOptions
                                // defaultOptions={resourceMajorHardwareBuild}
                                value={
                                  resourceMajorHardwareBuild &&
                                  resourceMajorHardwareBuild
                                    ?.sort((a, b) => a.order - b.order)
                                    .filter(
                                      (x) =>
                                        x.key ===
                                        (formData?.majorHardwareBuildId !==
                                          undefined &&
                                        formData?.majorHardwareBuildId !== null
                                          ? formData?.majorHardwareBuildId[0]
                                              ?.majorHardwareBuildId
                                          : null)
                                    )[0]
                                }
                                // loadOptions={(x) => e &&
                                //   promiseSelect(x, searchHardwareBuild)
                                // }
                                onChange={(e) => onChangeMajorMain(e)}
                                onInputChange={setInputValue}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                isDisabled={props.edit ? true : false}
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                styles={colourStyles}
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
                              validation.response === false &&
                              validation.property?.includes(
                                "majorHardwareBuildId"
                              ) ? (
                                <label className="validation">
                                  *Major Hardware must have a value
                                </label>
                              ) : null}
                            </label>
                          </div>
                        </div>
                        <div className="col-6 pr-0 pl-4">
                          <div className="form-group w-100">
                            <label
                              className="labelForm voda-bold w-100"
                              id="systemTypeAddModal_majorSoftware_tour"
                            >
                              Major Software <span className="red ">*</span>
                              <Select
                                menuPosition={"fixed"}
                                name="majorSoftwareBuildsId"
                                // cacheOptions
                                // defaultOptions={resourceMajorSoftwareBuild}
                                options={
                                  resourceMajorSoftwareBuild
                                    ? resourceMajorSoftwareBuild
                                    : undefined
                                }
                                value={
                                  resourceMajorSoftwareBuild
                                    ?.sort((a, b) => a.order - b.order)
                                    .filter(
                                      (x) =>
                                        x.key ===
                                        formData?.majorSoftwareBuildsId
                                    )[0]
                                }
                                // loadOptions={(x) => x &&
                                //   promiseSelect(x, searchSowtwareBuild)
                                // }
                                onChange={(e) => onChangeMajorSoftware(e)}
                                onInputChange={setInputValue}
                                onBlur={() => setInputValue("")}
                                isSearchable
                                isClearable
                                isDisabled={
                                  disableSelect || (props.edit ? true : false)
                                }
                                getOptionLabel={(option) => option.value}
                                getOptionValue={(option) =>
                                  option["key"].toString()
                                }
                                styles={colourStyles}
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
                              validation.response === false &&
                              validation.property?.includes(
                                "majorSoftwareBuildsId"
                              ) ? (
                                <label className="validation">
                                  *Major Software must have a value
                                </label>
                              ) : null}
                            </label>
                          </div>
                        </div>
                      </div>
                    </>
                  )}

                  {!props.wizardMode && (
                    <>
                      <div className="col-6 pl-0">
                        <div className="w-100">
                          <label
                            className="labelForm voda-bold col-12 pl-0"
                            id="systemTypeAddModal_vodafoneName_tour"
                          >
                            {/* Vodafone Name */}
                            Vodafone Name (Which SW/HW collection is this
                            product part of?)
                            <span className="red ">*</span>
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.vodafoneNameResource &&
                                    dictionaryToArray(
                                      formData?.vodafoneNameResource
                                    )
                                  }
                                  value={
                                    formData &&
                                    formData?.vodafoneNameResource &&
                                    dictionaryToArray(
                                      formData?.vodafoneNameResource
                                    ).filter(
                                      (x) => x.key === formData?.vodafoneNameId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("vodafoneNameId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  placeholder={"Select..."}
                                  isDisabled={!formData?.majorSoftwareBuildsId}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                />
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(8)}
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
                            validation.property?.includes("vodafoneNameId") ? (
                              <label className="validation">
                                *Asset Category must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      </div>
                    </>
                  )}

                  <div
                    className={`${
                      !props.wizardMode ? "col-6 pr-0 pl-0" : "col-6 pl-0 pr-32"
                    }`}
                  >
                    <div className="form-group w-100">
                      <label
                        className="labelForm voda-bold w-100"
                        id="systemTypeAddModal_productImportance_tour"
                      >
                        Product Importance <span className="red">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              menuPosition={"fixed"}
                              options={
                                formData?.productImportanceResource &&
                                dictionaryToArray(
                                  formData?.productImportanceResource
                                )
                              }
                              value={
                                formData &&
                                formData?.productImportanceResource &&
                                dictionaryToArray(
                                  formData?.productImportanceResource
                                ).filter(
                                  (x) => x.key === formData?.productImportanceId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("productImportanceId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
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
                        validation.response === false &&
                        validation.property?.includes("productImportanceId") ? (
                          <label className="validation">
                            *Product Importance must have a value
                          </label>
                        ) : null}
                      </label>
                    </div>
                  </div>
                </div>
              </fieldset>

              {props.wizardMode && (
                <>
                  <div className="col-12 pl-0 pr-2 d-flex">
                    <fieldset className="col-12 mt-3 mx-0 align-content-start">
                      <legend className="text-bb mb-40">Name Aliases</legend>
                      <div className="row">
                        <div className="col-6 pl-0">
                          <label className="labelForm voda-bold col-12">
                            {/* Vodafone Name */}
                            Which SW/HW collection is this product part of?
                            <span className="red ">*</span>
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.vodafoneNameResource &&
                                    dictionaryToArray(
                                      formData?.vodafoneNameResource
                                    )
                                  }
                                  value={
                                    formData &&
                                    formData?.vodafoneNameResource &&
                                    dictionaryToArray(
                                      formData?.vodafoneNameResource
                                    ).filter(
                                      (x) => x.key === formData?.vodafoneNameId
                                    )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("vodafoneNameId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  placeholder={"Select..."}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                  isDisabled={
                                    !props?.majorSoftwareDto?.vodafoneNameId
                                  }
                                />
                              </div>
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVisibleModalLookup(8)}
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
                            validation.property?.includes("vodafoneNameId") ? (
                              <label className="validation ml-3">
                                *Vodafone Name must have a value
                              </label>
                            ) : null}
                          </label>
                        </div>
                      </div>
                    </fieldset>
                  </div>
                </>
              )}

              <div
                className={`col-12 mt-3 ${
                  props.wizardMode ? "pl-0 pr-2" : "pr-0 pl-2"
                } d-flex`}
              >
                <fieldset className="col-12 pl-0 pr-0">
                  <legend className="text-bb">Engineering Ownership</legend>
                  <div className="row">
                    <div className="col-6 w-100 pl-0 mt-2">
                      <div className="col-12 w-100 pr-0 pl-3">
                        {/* <label className="labelForm voda-bold w-100">
                        Vertical Responsible<span className="red ">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              options={
                                formData?.verticalResponsibleResource &&
                                dictionaryToArray(
                                  formData?.verticalResponsibleResource
                                )
                              }
                              value={
                                formData &&
                                formData?.verticalResponsibleResource &&
                                dictionaryToArray(
                                  formData?.verticalResponsibleResource
                                ).filter(
                                  (x) =>
                                    x.key == formData?.verticalResponsibleId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("verticalResponsibleId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
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
                                alt="plus"
                              />
                            </button>
                          )}
                        </div>
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "verticalResponsibleId"
                        ) ? (
                          <label className="validation">
                            *Vertical Responsible must have a value
                          </label>
                        ) : null}
                      </label> */}
                        <TextInputComponent
                          label={"HW Vertical Responsible"}
                          isList={true}
                          disabled={true}
                          labelCSS="mb-0"
                          inputCSS="labelForm mb-2"
                          value={apiHWObject?.["VerticalResponsibles"] ?? ""}
                          onChange={() => {}}
                        />
                      </div>
                      <div className="col-12 w-100 pr-0 pl-3">
                        {/* <label className="labelForm voda-bold  w-100">
                        Sub-Domain Responsible<span className="red ">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              options={
                                formData?.subDomainResponsibleResource &&
                                dictionaryToArray(
                                  formData?.subDomainResponsibleResource
                                )
                              }
                              value={
                                formData &&
                                formData?.subDomainResponsibleResource &&
                                dictionaryToArray(
                                  formData?.subDomainResponsibleResource
                                ).filter(
                                  (x) =>
                                    x.key == formData?.subDomainResponsibleId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("subDomainResponsibleId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
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
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "subDomainResponsibleId"
                        ) ? (
                          <label className="validation">
                            *Sub-Domain Responsible must have a value
                          </label>
                        ) : null}
                      </label> */}
                        <TextInputComponent
                          label={"HW Sub-Domain Responsible"}
                          isList={true}
                          disabled={true}
                          labelCSS="mb-0"
                          inputCSS="labelForm mb-2"
                          value={apiHWObject?.["SubDomainResponsibles"] ?? ""}
                          onChange={() => {}}
                        />
                      </div>
                      <div className="col-12 w-100 pr-0 pl-3">
                        {/* <label className="labelForm voda-bold  w-100">
                        Sub-Domain Spoc<span className="red ">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
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
                              isMulti
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
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
                                alt="plus"
                              />
                            </button>
                          )}
                        </div>
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes("subDomainSpocIds") ? (
                          <label className="validation">
                            *Sub-Domain Spoc must have a value
                          </label>
                        ) : null}
                      </label> */}

                        <TextInputComponent
                          label={"HW Design Contact"}
                          isList={true}
                          disabled={true}
                          labelCSS="mb-0"
                          inputCSS="labelForm mb-2"
                          value={apiHWObject?.["DesignContact"] ?? ""}
                          onChange={() => {}}
                        />
                      </div>
                    </div>
                    <div className="col-6 w-100 mt-2">
                      <div className="col-12 w-100 pr-0 pl-0">
                        {/* <label className="labelForm voda-bold w-100">
                        Vertical Responsible<span className="red ">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              options={
                                formData?.verticalResponsibleResource &&
                                dictionaryToArray(
                                  formData?.verticalResponsibleResource
                                )
                              }
                              value={
                                formData &&
                                formData?.verticalResponsibleResource &&
                                dictionaryToArray(
                                  formData?.verticalResponsibleResource
                                ).filter(
                                  (x) =>
                                    x.key == formData?.verticalResponsibleId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("verticalResponsibleId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
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
                                alt="plus"
                              />
                            </button>
                          )}
                        </div>
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "verticalResponsibleId"
                        ) ? (
                          <label className="validation">
                            *Vertical Responsible must have a value
                          </label>
                        ) : null}
                      </label> */}
                        <TextInputComponent
                          label={"SW Vertical Responsible"}
                          isList={true}
                          disabled={true}
                          labelCSS="mb-0"
                          inputCSS="labelForm mb-2"
                          value={apiSWObject?.["VerticalResponsibles"] ?? ""}
                          onChange={() => {}}
                        />
                      </div>
                      <div className="col-12 w-100 pr-0 pl-0">
                        {/* <label className="labelForm voda-bold  w-100">
                        Sub-Domain Responsible<span className="red ">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
                              options={
                                formData?.subDomainResponsibleResource &&
                                dictionaryToArray(
                                  formData?.subDomainResponsibleResource
                                )
                              }
                              value={
                                formData &&
                                formData?.subDomainResponsibleResource &&
                                dictionaryToArray(
                                  formData?.subDomainResponsibleResource
                                ).filter(
                                  (x) =>
                                    x.key == formData?.subDomainResponsibleId
                                )
                              }
                              onChange={(e) =>
                                onChangeSelect("subDomainResponsibleId", e)
                              }
                              onBlur={() => setInputValue("")}
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
                              }
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
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes(
                          "subDomainResponsibleId"
                        ) ? (
                          <label className="validation">
                            *Sub-Domain Responsible must have a value
                          </label>
                        ) : null}
                      </label> */}
                        <TextInputComponent
                          label={"SW Sub-Domain Responsible"}
                          isList={true}
                          disabled={true}
                          labelCSS="mb-0"
                          inputCSS="labelForm mb-2"
                          value={apiSWObject?.["SubDomainResponsibles"] ?? ""}
                          onChange={() => {}}
                        />
                      </div>
                      <div className="col-12 w-100 pr-0 pl-0">
                        {/* <label className="labelForm voda-bold  w-100">
                        Sub-Domain Spoc<span className="red ">*</span>
                        <div className="d-flex">
                          <div className="w-100">
                            <Select
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
                              isMulti
                              isSearchable
                              isClearable
                              getOptionLabel={(option) => option.value}
                              getOptionValue={(option) =>
                                option["key"].toString()
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
                                alt="plus"
                              />
                            </button>
                          )}
                        </div>
                        {validation &&
                        validation.response === false &&
                        validation.property?.includes("subDomainSpocIds") ? (
                          <label className="validation">
                            *Sub-Domain Spoc must have a value
                          </label>
                        ) : null}
                      </label> */}

                        <TextInputComponent
                          label={"SW Design Contact"}
                          isList={true}
                          disabled={true}
                          labelCSS="mb-0"
                          inputCSS="labelForm mb-2"
                          value={apiSWObject?.["DesignContact"] ?? ""}
                          onChange={() => {}}
                        />
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div>

              <div className="col-12 mt-20 mb-20">
                <button
                  type="button"
                  className="further-btn"
                  onClick={chnageShowFurtherSection}
                >
                  Click for further details
                </button>
              </div>

              {showFurtherSection && (
                <>
                  <div className="row col-12 pr-0">
                    {props?.wizardMode && (
                      <div className="col-6">
                        <label className="labelForm voda-bold w-100">
                          Standards Name
                          <input
                            type="text"
                            onChange={(e) => onChange("SystemTypeName3Gpp", e)}
                            onKeyUp={(e) => onChange("SystemTypeName3Gpp", e)}
                            className="inputForm w-100"
                            value={formData?.systemTypeName3Gpp ?? undefined}
                          />
                        </label>
                      </div>
                    )}
                    {!props.wizardMode && (
                      <>
                        <div className="col-6">
                          <label className="labelForm voda-bold w-100">
                            Standards Name
                            <input
                              type="text"
                              onChange={(e) =>
                                onChange("SystemTypeName3Gpp", e)
                              }
                              onKeyUp={(e) => onChange("SystemTypeName3Gpp", e)}
                              className="inputForm w-100"
                              value={formData?.systemTypeName3Gpp ?? undefined}
                            />
                          </label>
                        </div>
                        {!props.edit && (
                          <div className="col-6 pl-4">
                            <label className="labelForm voda-bold col-12">
                              Name From Equipment Manufacturer
                              <div
                                className="customFakeInput disabled"
                                style={{ marginTop: "4px" }}
                              >
                                <label
                                  className="w-100 mb-0"
                                  dangerouslySetInnerHTML={{
                                    __html:
                                      formData &&
                                      formData.systemTypeNameOem != undefined
                                        ? formData.systemTypeNameOem
                                        : "",
                                  }}
                                ></label>
                              </div>
                            </label>
                          </div>
                        )}
                      </>
                    )}

                    <div className="col-12 row pr-0">
                      {/* <div
                        style={{ paddingRight: "10px" }}
                        className="col-6"
                        onClick={(e) => e.preventDefault()}
                      >
                        <label className="labelForm voda-bold w-100">
                          Asset Class<span className="red ">*</span>
                          <div className="d-flex ">
                            <div className="w-100 voda-regular customFakeInputWithSpace disabled">
                              <label
                                dangerouslySetInnerHTML={{
                                  __html: assetClassName,
                                }}
                                style={{ margin: 0, color: "rgb(109,109,109)" }}
                              />
                            </div>
                          </div>
                          {validation &&
                          validation.response === false &&
                          validation.property?.includes("assetClassId") ? (
                            <label className="validation">
                              *Asset Class must have a value
                            </label>
                          ) : null}
                        </label>
                      </div> */}
                      <div className="col-6  pr-0">
                        <label className="labelForm voda-bold w-100">
                          Asset Type
                          <div className="d-flex">
                            <div className="w-100">
                              {formData?.assetCategoryId == undefined ||
                              (formData?.assetCategoryResource &&
                                dictionaryToArrayAssetCategoryDto(
                                  formData?.assetCategoryResource
                                ).find(
                                  (x) => x.key === formData.assetCategoryId
                                )?.value.takeFromAssetTypeTable === false) ? (
                                <input
                                  type="text"
                                  className="inputForm w-100"
                                  value={formData?.assetType ?? undefined}
                                  disabled
                                />
                              ) : (
                                <Select
                                  menuPosition={"fixed"}
                                  options={
                                    formData?.assetTypeResource &&
                                    dictionaryToArrayRelatedResource(
                                      formData?.assetTypeResource
                                    )
                                      .filter(
                                        (x) =>
                                          parseInt(x.value.id ?? "") ==
                                          formData.assetCategoryId
                                      )
                                      .map((x) => {
                                        return {
                                          key: x.key,
                                          value: x.value.value ?? "",
                                          assetCategory: x.value.id,
                                        };
                                      })
                                  }
                                  value={
                                    formData?.assetTypeResource &&
                                    dictionaryToArrayRelatedResource(
                                      formData?.assetTypeResource
                                    )
                                      .map((x) => {
                                        return {
                                          key: x.key,
                                          value: x.value.value ?? "",
                                          assetCategory: x.value.id,
                                        };
                                      })
                                      .find(
                                        (x) => x.key == formData?.assetTypeId
                                      )
                                  }
                                  onChange={(e) =>
                                    onChangeSelect("assetTypeId", e)
                                  }
                                  onBlur={() => setInputValue("")}
                                  isSearchable
                                  isClearable
                                  getOptionLabel={(option) => option.value}
                                  getOptionValue={(option) =>
                                    option["key"].toString()
                                  }
                                ></Select>
                              )}
                            </div>
                            {tipologicaPermesso && (
                              <button
                                className="btn btn-link"
                                onClick={() => setIsVisibleModalLookup(6)}
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
                    {!props.wizardMode && (
                      <>
                        <div className="col-6">
                          <label className="disabledDate labelForm voda-bold  w-100">
                            End of Maintenance
                            <input
                              type="text"
                              className=" disabledBackground inputForm w-100 text-uppercase voda-regular"
                              value={
                                formData?.endOfMaintenance != "Not Announced" &&
                                formData?.endOfMaintenance != "Not Specified"
                                  ? formatTimeLocal(formData?.endOfMaintenance)
                                  : formData?.endOfMaintenance
                              }
                            />
                          </label>
                        </div>
                        <div
                          style={{ paddingLeft: "40px" }}
                          className={`${
                            props.edit ? "col-6" : "col-6 d-none pl-0"
                          }`}
                        >
                          <label className="labelForm voda-bold w-100">
                            Constraint (Scaling)
                            <input
                              readOnly
                              onChange={(e) => onChange("constraintScaling", e)}
                              type={props.edit ? "text" : "hidden"}
                              className={`inputForm w-100 ${formData?.lcmStatus}`}
                              value={formData?.constraintScaling}
                            />
                          </label>
                        </div>
                        <div
                          className={`${
                            props.edit ? "col-6" : "d-none col-6 pl-0"
                          }`}
                        >
                          <label className="labelForm voda-bold w-100">
                            Constraint (LCM)
                            <input
                              onChange={(e) => onChange("constraintLcm", e)}
                              type={"text"}
                              readOnly
                              className="inputForm w-100"
                              value={formData?.constraintLcm ?? undefined}
                            />
                          </label>
                        </div>
                      </>
                    )}
                  </div>
                </>
              )}

              {props.edit === true && showFurtherSection ? (
                <div className="col-12 pl-2 pr-0">
                  <fieldset className="col-12 mt-3 row">
                    <div className="col-6" style={{ paddingLeft: "5px" }}>
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
                    <div className="pr-2 col-6" style={{ paddingLeft: "40px" }}>
                      <label className="labelForm voda-bold w-100">
                        Last Modified By
                        <input
                          readOnly={true}
                          className="inputForm w-100 voda-regular"
                          type="text"
                          value={formData?.lastModifiedBy ?? undefined}
                        />
                      </label>
                    </div>
                  </fieldset>
                </div>
              ) : null}
            </div>
          </form>
        </Tab>
        {formData?.majorHardwareBuildId &&
        formData?.majorHardwareBuildId?.length > 0 ? (
          <Tab eventKey="majorHardware" title="Link Hardware Solutions">
            <ModalMajorHardware
              tab={keyTabs}
              changed={changed}
              action={{ GetMajorHardwareFromList, setChanged }}
              data={
                formData?.majorHardwareBuildId ??
                ({} as MajorHardwareBuildMainSystemTypeDto[])
              }
            ></ModalMajorHardware>
          </Tab>
        ) : null}
      </Tabs>
      {!props.wizardMode && (
        <>
          <div className="col-12 justify-content-end mt-2 d-flex footerModal">
            <button
              className=" voda-bold btn btn-link px-4 btnHeader cancel"
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
                props.systemTypeRedirect === true
                  ? "disabledCursor"
                  : ""
              }`}
              onClick={() =>
                Save(
                  {
                    ...formData,
                    assetCategoryResource: {},
                    assetClassResource: {},
                    assetTypeResource: {},
                    productImportanceResource: {},
                    subDomainResponsibleResource: {},
                    subDomainSpocResource: {},
                    verticalResponsibleResource: {},
                  },
                  props.edit,
                  validazioneClient,
                  refresh,
                  RestoreOrphanDeleted,
                  orphanDeleted
                )
              }
              type="button"
              data-toggle="tooltip"
              data-placement="top"
              title={
                props.prevPage === "generatelcmdb" &&
                props.systemTypeRedirect === true
                  ? `Saving is disabled due to redirection from LCM Export screen`
                  : ""
              }
              disabled={
                props.prevPage === "generatelcmdb" &&
                props.systemTypeRedirect === true
                  ? true
                  : false
              }
              id="systemTypeAddModal_save_tour"
            >
              Save
            </button>
          </div>
        </>
      )}

      {props.wizardMode && (
        <>
          <div className="col-12 d-flex justify-content-between py-4 mt-4">
            <button
              className={` voda-bold btn btn-link px-4 btnHeader exit ${
                props.prevPage === "generatelcmdb" &&
                props.systemTypeRedirect === true
                  ? "disabledCursor"
                  : ""
              }`}
              type="button"
              onClick={() =>
                props.action.setConfirmExitWizard &&
                props.action.setConfirmExitWizard()
              }
              data-toggle="tooltip"
              data-placement="top"
              title={
                props.prevPage === "generatelcmdb" &&
                props.systemTypeRedirect === true
                  ? `Saving is disabled due to redirection from LCM Export screen`
                  : ""
              }
              disabled={
                props.prevPage === "generatelcmdb" &&
                props.systemTypeRedirect === true
                  ? true
                  : false
              }
            >
              Exit
            </button>
            <div className="">
              <button
                disabled={!(props.wizardStep && props.wizardStep > 1)}
                className=" voda-bold btn btn-link px-4 btnHeader cancel"
                type="button"
                onClick={() =>
                  props.action.wizardBackFunction &&
                  formData &&
                  props.action.wizardBackFunction(formData, "systemTypeDto")
                }
              >
                Back
              </button>
              <button
                className=" voda-bold btn btn-danger px-4 btnHeader"
                type="button"
                onClick={() => validateWizard()}
              >
                Continue
              </button>
            </div>
          </div>
        </>
      )}
      {tourStarted && (
        <TourGuide
          start={tourStarted}
          tourSteps={getSystemTypeModalTourSteps(props)}
          page={"systemTypeAddModal"}
          setStartTour={(val: boolean) =>
            dispatch(val ? startGuideTour() : endGuideTour())
          }
          onTourEnd={handleEndTour}
        />
      )}
    </div>
  );
};
export default ModalSystemType;
