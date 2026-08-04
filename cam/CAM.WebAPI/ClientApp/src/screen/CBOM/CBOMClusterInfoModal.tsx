import React, {
  useCallback,
  useEffect,
  useState,
  useImperativeHandle,
  forwardRef,
} from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { formatDateWithTime, safeNumber } from "../../Hook/Common";
import DatePicker from "react-datepicker";
import {
  DateInputComponent,
  DropdownInputComponent,
  ShowYearInputComponent,
  TextAreaInputComponent,
  TextInputComponent,
  ToggleInputComponent,
} from "../../Components/FormField";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import {
  dictionaryToArray,
  resourceArrayRefactor,
} from "../../Hook/Dictionary";
import { useAuth } from "../../Hook/useAuth";
import PracticeContainer from "../../Containers/Lookup/PracticeContainer";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import ModalRelated from "../../Components/ModalRelated";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import {
  Button,
  Card,
  Chip,
  DialogActions,
  Divider,
  Grid,
  Paper,
  Stack,
  Typography,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import {
  VBOMInfoDtoCreate,
  VBOMInfoDtoGrid,
  VBOMClusterInfoDtoUpdate,
  VNFClusterInfoDetail,
  VNFVMClusterCapacity,
  VNFVMClusterInfo,
} from "../../Model/VBOMInfo";
import {
  CreateVBOMClusterInfo,
  CreateVBOMInfo,
} from "../../Redux/Action/VBOMInfo/VBOMInfoCreateAction";
import { EditVBOMInfo } from "../../Redux/Action/VBOMInfo/VBOMInfoEditAction";
import CustomAccordion from "../../Components/CustomAccordion";
import CustomCollapsibleTable, {
  Column,
} from "../../Components/CustomCollapsibleTable";
import CustomMUITable from "../../Components/CustomMUITable";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import Location from "../../Containers/Lookup/LocationContainer";
import CNFPriority from "../../Containers/Lookup/CNFPriorityContainer";
import CNFHardwareType from "../../Containers/Lookup/CNFHardwareTypeContainer";
import CNFFunctionStandardName from "../../Containers/Lookup/CNFFunctionStandardNameContainer";
import PodTypeInfo from "../../Containers/Lookup/PodTypeInfoContainer";
import CNFName from "../../Containers/Lookup/CNFNameContainer";
import CNFCluster from "../../Containers/Lookup/CNFClusterContainer";

import CustomizedTables from "../../Components/CustomStaticMUITable";
import { MdAdd, MdGridView } from "react-icons/md";
import {
  CBOMDto,
  CBOMDtoUpdate,
  CnfCapacityDto,
  CnfClusterInfoDto,
} from "../../Model/CBOM";
import { CreateCBOM } from "../../Redux/Action/CBOM/CBOMCreateAction";
import { EditCBOM } from "../../Redux/Action/CBOM/CBOMEditAction";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
  instanceKey?: any;
  capacityKey?: any;
}

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: "#fff",
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: "center",
  color: theme.palette.text.secondary,
  ...theme.applyStyles("dark", {
    backgroundColor: "#1A2027",
  }),
}));

const CBOMClusterInfoModal = forwardRef((props: Props, ref) => {
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
    confirmForm,
    checkIsExist,
  } = useFormTableCrud<CBOMDto>(CreateCBOM, EditCBOM);
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [validationInstanceForm, setValidationInstanceForm] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [validationCapacityForm, setValidationCapacityForm] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const { tipologicaPermesso, readonly, isPermesso } = useAuth();
  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [podTypeInfoOptions, setPodTypeInfoOptions] = useState<any>();
  const [opcoOptions, setOpcoOptions] = useState<any>();
  const [clusterNameResources, setClusterNameResources] = useState<any>();
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [instanceForm, setInstanceForm] = useState<CnfClusterInfoDto | null>(
    null
  );
  const [capacityForm, setCapacityForm] = useState<CnfCapacityDto | null>(null);
  const [addInterfaceFlag, setAddInterfaceFlag] = useState<boolean>(false);
  const [addCapacityFlag, setAddCapacityFlag] = useState<boolean>(false);
  const [editInterfaceId, setEditInterfaceId] = useState<number | null>(null);
  const [isCNFPriorityCrudModalOpen, setIsCNFPriorityCrudModalOpen] =
    useState(false);
  const [isCNFHardwareTypeCrudModalOpen, setIsCNFHardwareTypeCrudModalOpen] =
    useState(false);
  const [
    isCnfFunctionStandardNameCrudModalOpen,
    setIsCnfFunctionStandardNameCrudModalOpen,
  ] = useState(false);
  const [isPodTypeInfoCrudModalOpen, setIsPodTypeInfoCrudModalOpen] =
    useState(false);
  const [isCNFNameCrudModalOpen, setIsCNFNameCrudModalOpen] = useState(false);
  const [isCNFClusterCrudModalOpen, setIsCNFClusterCrudModalOpen] =
    useState(false);

  const [isLocationCrudModalOpen, setIsLocationCrudModalOpen] = useState(false);
  const [isAddInstance, setIsAddInstance] = useState(false);
  const [isAddCapacity, setIsAddCapacity] = useState(false);
  const [nameTypeError, setNameTypeError] = useState(false);
  const [versionError, setVersionError] = useState(false);
  const [instanceIndex, setInstanceIndex] = useState<any>(
    props.instanceKey ?? 0
  );
  const [capacityIndex, setCapacityIndex] = useState<any>(
    props.capacityKey ?? 0
  );
  const [inputError, setInputError] = useState<{
    field: string;
    type: string;
  } | null>(null);
  const [editCapacityId, setEditCapacityId] = useState<{
    instanceId: number | null;
    capacityId: number | null;
  } | null>({
    instanceId: props.instanceKey ?? 0,
    capacityId: props.capacityKey ?? 0,
  });
  const dtoEditResourceState = (state: RootState) =>
    state.CBOMEditReducer.CBOMDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.CBOMCreateReducer.CBOMDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);
  useEffect(() => {
    const mapPodTypeInfoOptions = (data: any) =>
      data?.map((item) => ({
        key: item?.podtypeinfoid,
        value: item?.podtypeinfoname,
      })) || [];

    const mapOpcoOptions = (data: any[]) =>
      data?.map((item) => ({
        key: item?.opcoId,
        value: item?.opcoDescription,
      })) || [];

    const getOpcoText = (opcoId: number | null, options: any[]) =>
      options?.find((o) => o.key === opcoId)?.value ?? "";

    const getLocationText = (
      opcoId: number | null,
      siteId: number | null,
      resources: any[]
    ) =>
      resources
        ?.find((opco) => opco.opcoId == opcoId)
        ?.locationDetails?.find((loc) => loc.value == siteId)
        ?.shortDescription ?? "";

    if (props.edit) {
      if (!editResource || !("data" in editResource)) return;
      const {
        data: {
          _cnfClusterInfoEntity,
          cnfNameResources,
          cnfClusterNameResource,
          cnfClusterNodePoolResource,
          functionStandardedNameResource,
          podTypeInfoResource,
          podTypeDescriptionInfoResource,
          opcoBasedLocationResource,
          priorityResource,
          hardwareResource,
          verticalResource,
          financialVersion,
          ...rest
        },
      } = editResource as { data: CBOMDto };
      const { opcoid, siteid } = _cnfClusterInfoEntity as CBOMDtoUpdate;
      const opcoOptions = mapOpcoOptions(opcoBasedLocationResource);
      setPodTypeInfoOptions(mapPodTypeInfoOptions(podTypeInfoResource));
      setOpcoOptions(opcoOptions);
      setFormData({
        cnfNameResources,
        cnfClusterNameResource,
        cnfClusterNodePoolResource,
        functionStandardedNameResource,
        podTypeInfoResource,
        podTypeDescriptionInfoResource,
        opcoBasedLocationResource,
        priorityResource,
        hardwareResource,
        verticalResource,
        financialVersion,
        _cnfClusterInfoEntity: {
          ..._cnfClusterInfoEntity,
          opco: getOpcoText(opcoid, opcoOptions),
          site: getLocationText(opcoid, siteid, opcoBasedLocationResource),
        } as CBOMDtoUpdate,
      } as CBOMDto);
      if (props?.capacityKey >= 0 && props?.capacityKey !== null) {
        _cnfClusterInfoEntity?.cnfpodinfo?.map((res, index) => {
          if (res.cnfpodinfoid === props.instanceKey) {
            setInstanceIndex(index);
          }
        });
        _cnfClusterInfoEntity?.cnfpodinfo
          ?.filter((res) => res.cnfpodinfoid === props.instanceKey)[0]
          ?.cnfcapacity?.map((res, index) => {
            if (res.cnfcapacityid === props.capacityKey) {
              setCapacityIndex(index);
              setCapacityForm(res);
              setEditCapacityId({
                instanceId: props.instanceKey,
                capacityId: res.cnfcapacityid ?? index,
              });
              setAddCapacityFlag(true);
              setIsAddCapacity(true);
            }
          });
      } else {
        _cnfClusterInfoEntity?.cnfpodinfo?.map((res, index) => {
          if (res.cnfpodinfoid === props.instanceKey) {
            setInstanceIndex(index);
            setInstanceForm(res);
            setEditInterfaceId(res.cnfpodinfoid ?? index);
            setAddInterfaceFlag(true);
            setIsAddInstance(true);
          }
        });
      }
    } else if (createResource) {
      const {
        _cnfClusterInfoEntity,
        cnfNameResources,
        cnfClusterNameResource,
        cnfClusterNodePoolResource,
        functionStandardedNameResource,
        podTypeInfoResource,
        podTypeDescriptionInfoResource,
        opcoBasedLocationResource,
        priorityResource,
        hardwareResource,
        verticalResource,
        financialVersion,
        ...rest
      } = createResource as CBOMDto;

      setPodTypeInfoOptions(mapPodTypeInfoOptions(cnfNameResources));
      setOpcoOptions(mapOpcoOptions(opcoBasedLocationResource));
      setFormData({
        ...createResource,
        cnfNameResources,
        cnfClusterNameResource,
        cnfClusterNodePoolResource,
        functionStandardedNameResource,
        podTypeInfoResource,
        podTypeDescriptionInfoResource,
        opcoBasedLocationResource,
        priorityResource,
        hardwareResource,
        verticalResource,
        financialVersion,
        _cnfClusterInfoEntity: {
          ..._cnfClusterInfoEntity,
          cpukubelet: 0.5,
          cpusystem: 0.5,
          memkubelet: 2,
          memsystem: 2,
          cnfpodinfo: [],
        } as CBOMDtoUpdate,
      });
    }
  }, [createResource, editResource, props.edit, isLocationCrudModalOpen]);

  const validazioneClient = (copy: CBOMDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?._cnfClusterInfoEntity?.cnfnameid === null ||
      copy?._cnfClusterInfoEntity?.cnfnameid === undefined
    ) {
      addInvalidProperty("cnfnameid");
    }

    if (
      copy?._cnfClusterInfoEntity?.cnfclusterid === null ||
      copy?._cnfClusterInfoEntity?.cnfclusterid === undefined
    ) {
      addInvalidProperty("cnfclusterid");
    }

    if (
      copy?._cnfClusterInfoEntity?.opcoid === null ||
      copy?._cnfClusterInfoEntity?.opcoid === undefined ||
      copy?._cnfClusterInfoEntity?.opcoid === 0
    ) {
      addInvalidProperty("opcoid");
    }

    if (
      copy?._cnfClusterInfoEntity?.siteid === null ||
      copy?._cnfClusterInfoEntity?.siteid === undefined ||
      copy?._cnfClusterInfoEntity?.siteid === 0
    ) {
      addInvalidProperty("siteid");
    }

    if (
      copy?._cnfClusterInfoEntity?.nodepoolbreakup === null ||
      copy?._cnfClusterInfoEntity?.nodepoolbreakup === undefined
    ) {
      addInvalidProperty("nodepoolbreakup");
    }

    if (
      copy?._cnfClusterInfoEntity?.cnfclusternodepoolid === null ||
      copy?._cnfClusterInfoEntity?.cnfclusternodepoolid === undefined
    ) {
      addInvalidProperty("cnfclusternodepoolid");
    }

    if (
      copy?._cnfClusterInfoEntity?.cnfhardwareid === null ||
      copy?._cnfClusterInfoEntity?.cnfhardwareid === undefined
    ) {
      addInvalidProperty("cnfhardwareid");
    }

    if (
      copy?._cnfClusterInfoEntity?.specialrequirements === null ||
      copy?._cnfClusterInfoEntity?.specialrequirements === undefined ||
      copy?._cnfClusterInfoEntity?.specialrequirements === ""
    ) {
      addInvalidProperty("specialrequirements");
    }

    if (
      copy?._cnfClusterInfoEntity?.hyperthreading === null ||
      copy?._cnfClusterInfoEntity?.hyperthreading === undefined ||
      copy?._cnfClusterInfoEntity?.hyperthreading === ""
    ) {
      addInvalidProperty("hyperthreading");
    }

    if (
      copy?._cnfClusterInfoEntity?.overprovisioning === null ||
      copy?._cnfClusterInfoEntity?.overprovisioning === undefined ||
      copy?._cnfClusterInfoEntity?.overprovisioning === ""
    ) {
      addInvalidProperty("overprovisioning");
    }

    // if (
    //   copy?._cnfClusterInfoEntity?.workernodeconfiguration === null ||
    //   copy?._cnfClusterInfoEntity?.workernodeconfiguration === undefined ||
    //   copy?._cnfClusterInfoEntity?.workernodeconfiguration === ""
    // ) {
    //   addInvalidProperty("workernodeconfiguration");
    // }

    // if (
    //   copy?._cnfClusterInfoEntity?.hardware === null ||
    //   copy?._cnfClusterInfoEntity?.hardware === undefined ||
    //   copy?._cnfClusterInfoEntity?.hardware === ""
    // ) {
    //   addInvalidProperty("hardware");
    // }

    if (
      copy?._cnfClusterInfoEntity?.cpukubelet === null ||
      copy?._cnfClusterInfoEntity?.cpukubelet === undefined
    ) {
      addInvalidProperty("cpukubelet");
    }

    if (
      copy?._cnfClusterInfoEntity?.memkubelet === null ||
      copy?._cnfClusterInfoEntity?.memkubelet === undefined
    ) {
      addInvalidProperty("memkubelet");
    }
    if (
      copy?._cnfClusterInfoEntity?.cpusystem === null ||
      copy?._cnfClusterInfoEntity?.cpusystem === undefined
    ) {
      addInvalidProperty("cpusystem");
    }
    if (
      copy?._cnfClusterInfoEntity?.memsystem === null ||
      copy?._cnfClusterInfoEntity?.memsystem === undefined
    ) {
      addInvalidProperty("memsystem");
    }
    if (
      copy?._cnfClusterInfoEntity?.verticalresponsibleid === null ||
      copy?._cnfClusterInfoEntity?.verticalresponsibleid === undefined ||
      copy?._cnfClusterInfoEntity?.verticalresponsibleid === 0
    ) {
      addInvalidProperty("verticalresponsibleid");
    }
    if (
      copy?._cnfClusterInfoEntity?.filename === null ||
      copy?._cnfClusterInfoEntity?.filename === undefined ||
      copy?._cnfClusterInfoEntity?.filename === ""
    ) {
      addInvalidProperty("filename");
    }
    if (
      copy?._cnfClusterInfoEntity?.revision === null ||
      copy?._cnfClusterInfoEntity?.revision === undefined ||
      copy?._cnfClusterInfoEntity?.revision === ""
    ) {
      addInvalidProperty("revision");
    }
    if (
      copy?._cnfClusterInfoEntity?.aggregateimageclustersize === null ||
      copy?._cnfClusterInfoEntity?.aggregateimageclustersize === undefined ||
      copy?._cnfClusterInfoEntity?.aggregateimageclustersize === ""
    ) {
      addInvalidProperty("aggregateimageclustersize");
    }
    setValidation(copyValidation);
    // console.log("Form Vali", copyValidation);
    return copyValidation;
  };

  const validateInstance = (copy: CnfClusterInfoDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.podtypeinfoid === null ||
      copy?.podtypeinfoid === undefined ||
      copy?.podtypeinfoid === 0
    ) {
      addInvalidProperty("podtypeinfoid");
    }

    if (
      copy?.functionstandardid === null ||
      copy?.functionstandardid === undefined ||
      copy?.functionstandardid === 0
    ) {
      addInvalidProperty("functionstandardid");
    }

    if (
      copy?.priorityid === undefined ||
      copy?.priorityid === null ||
      copy?.priorityid === 0
    ) {
      addInvalidProperty("priorityid");
    }

    if (copy?.daemonsetpod === null || copy?.daemonsetpod === undefined) {
      addInvalidProperty("daemonsetpod");
    }

    if (
      copy?.podroledescriptionid === null ||
      copy?.podroledescriptionid === undefined ||
      copy?.podroledescriptionid === 0
    ) {
      addInvalidProperty("podroledescriptionid");
    }

    // if (
    //   copy?.intrapodrules === undefined ||
    //   copy?.intrapodrules === null ||
    //   copy?.intrapodrules === ""
    // ) {
    //   addInvalidProperty("intrapodrules");
    // }
    // if (
    //   copy?.interpodrules === undefined ||
    //   copy?.interpodrules === null ||
    //   copy?.interpodrules === ""
    // ) {
    //   addInvalidProperty("interpodrules");
    // }
    if (copy?.isenhancedha === null || copy?.isenhancedha === undefined) {
      addInvalidProperty("isenhancedha");
    }
    if (
      copy?.podtypeqos === undefined ||
      copy?.podtypeqos === null ||
      copy?.podtypeqos === ""
    ) {
      addInvalidProperty("podtypeqos");
    }
    // if (
    //   copy?.ispersistancestorageflag === undefined ||
    //   copy?.ispersistancestorageflag === null ||
    //   copy?.ispersistancestorageflag === ""
    // ) {
    //   addInvalidProperty("ispersistancestorageflag");
    // }
    if (copy?.isprodhpaenable === null || copy?.isprodhpaenable === undefined) {
      addInvalidProperty("isprodhpaenable");
    }
    setValidationInstanceForm(copyValidation);
    // console.log("Instance Validation", copyValidation);
    return copyValidation;
  };

  const validateCapacity = (copy: CnfCapacityDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.financialyear === null ||
      copy?.financialyear === undefined ||
      copy?.financialyear === 0
    ) {
      addInvalidProperty("financialyear");
    }
    if (
      copy?.financialversion === null ||
      copy?.financialversion === undefined ||
      copy?.financialversion === "" ||
      versionError
    ) {
      addInvalidProperty("financialversion");
    }

    if (
      copy?.numberofpodsperpodtype === null ||
      copy?.numberofpodsperpodtype === undefined
    ) {
      addInvalidProperty("numberofpodsperpodtype");
    }

    if (
      copy?.noofcnfinstancespersite === null ||
      copy?.noofcnfinstancespersite === undefined
    ) {
      addInvalidProperty("noofcnfinstancespersite");
    }

    if (
      copy?.vcpurequestforpodtype === null ||
      copy?.vcpurequestforpodtype === undefined
    ) {
      addInvalidProperty("vcpurequestforpodtype");
    }
    if (
      copy?.pcpurequestforpodtype === null ||
      copy?.pcpurequestforpodtype === undefined
    ) {
      addInvalidProperty("pcpurequestforpodtype");
    }
    if (
      copy?.vcpulimitforpodtype === null ||
      copy?.vcpulimitforpodtype === undefined
    ) {
      addInvalidProperty("vcpulimitforpodtype");
    }
    if (
      copy?.memrequestforpodtype === undefined ||
      copy?.memrequestforpodtype === null
    ) {
      addInvalidProperty("memrequestforpodtype");
    }
    if (
      copy?.nonpresistentstorageforprodtype === undefined ||
      copy?.nonpresistentstorageforprodtype === null
    ) {
      addInvalidProperty("nonpresistentstorageforprodtype");
    }
    if (
      copy?.ispresistentvolumesrequired === undefined ||
      copy?.ispresistentvolumesrequired === null
    ) {
      addInvalidProperty("ispresistentvolumesrequired");
    }
    if (
      copy?.persistentvolumneaccessmode === undefined ||
      copy?.persistentvolumneaccessmode === null
    ) {
      addInvalidProperty("persistentvolumneaccessmode");
    }
    if (
      copy?.persistentstorageforpodtype === undefined ||
      copy?.persistentstorageforpodtype === null
    ) {
      addInvalidProperty("persistentstorageforpodtype");
    }
    if (
      copy?.memrequestforpodtype === undefined ||
      copy?.memrequestforpodtype === null
    ) {
      addInvalidProperty("memrequestforpodtype");
    }
    if (
      copy?.storageiopsforpodtype === undefined ||
      copy?.storageiopsforpodtype === null
    ) {
      addInvalidProperty("storageiopsforpodtype");
    }
    // if (
    //   copy?.storagerworkloaddistribution === undefined ||
    //   copy?.storagerworkloaddistribution === null ||
    //   copy?.storagerworkloaddistribution === ""
    // ) {
    //   addInvalidProperty("storagerworkloaddistribution");
    // }
    // if (
    //   copy?.northsouthbandwidthforpodtype === undefined ||
    //   copy?.northsouthbandwidthforpodtype === null ||
    //   copy?.northsouthbandwidthforpodtype === ""
    // ) {
    //   addInvalidProperty("northsouthbandwidthforpodtype");
    // }
    // if (
    //   copy?.eastwestbandwidthforpodtype === undefined ||
    //   copy?.eastwestbandwidthforpodtype === null ||
    //   copy?.eastwestbandwidthforpodtype === ""
    // ) {
    //   addInvalidProperty("eastwestbandwidthforpodtype");
    // }
    // if (
    //   copy?.specialrequirementperpodtype === undefined ||
    //   copy?.specialrequirementperpodtype === null ||
    //   copy?.specialrequirementperpodtype === ""
    // ) {
    //   addInvalidProperty("specialrequirementperpodtype");
    // }
    // if (
    //   copy?.specialrequirement === undefined ||
    //   copy?.specialrequirement === null ||
    //   copy?.specialrequirement === ""
    // ) {
    //   addInvalidProperty("specialrequirement");
    // }

    setValidationCapacityForm(copyValidation);
    // console.log("Capacity Validation", copyValidation);
    return copyValidation;
  };
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const evumOption = [
    { key: true, value: "Yes" },
    { key: false, value: "No" },
  ];

  const evumOption1 = [
    { key: "Yes", value: "Yes" },
    { key: "No", value: "No" },
  ];

  const onChangeDropdownInfoSection = (property: string, e: any) => {
    const { _cnfClusterInfoEntity, ...res } = formData as CBOMDto;
    if (_cnfClusterInfoEntity) {
      if (property === "opcoid") {
        _cnfClusterInfoEntity.opco = e && e["value"];
        _cnfClusterInfoEntity.siteid = null;
      }
      if (property === "locationid") {
        _cnfClusterInfoEntity.siteid = e && e["value"];
      }
      if (property === "podtypeinfoid" || property === "intervmtype") {
        _cnfClusterInfoEntity[property] = e && e["value"];
      } else {
        _cnfClusterInfoEntity[property] = e && e["key"];
      }
    }
    setFormData({ ...res, _cnfClusterInfoEntity });
  };
  const onChangeDropdownInstanceSection = (property: string, e: any) => {
    const { _cnfClusterInfoEntity, ...res } = formData as CBOMDto;
    const { cnfpodinfo, ...resInfo } = _cnfClusterInfoEntity as CBOMDtoUpdate;

    if (cnfpodinfo) {
      // console.log("cnfpodinfo", cnfpodinfo);
      if (property === "podtypeinfoid" || property === "intervmtype") {
        cnfpodinfo[0][property] = e && e["value"];
      } else {
        cnfpodinfo[0][property] = e && e["key"];
      }
    }
    setFormData({
      ...res,
      _cnfClusterInfoEntity: {
        ..._cnfClusterInfoEntity,
        cnfpodinfo: cnfpodinfo,
      },
    } as CBOMDto);
  };

  const removeIdSuffix = (str: string) => {
    return str.replace(/id$/i, "");
  };

  const onChangeInstance = (property: string, e: any) => {
    let copy = { ...instanceForm } as CnfClusterInfoDto;
    // const props = [
    //   "podroledescriptionid",
    //   "functionstandardid",
    //   "podtypeinfoid",
    //   "priorityid",
    // ];

    // props?.map((id) => {
    //   if (property === id) {
    //     copy[removeIdSuffix(id)] = e && e["value"];
    //   }
    // });
    copy[property] = e && e["key"];
    setInstanceForm(copy);
  };

  const onChangeCapacity = (property: string, e: any) => {
    let copy = { ...capacityForm } as CnfCapacityDto;
    copy[property] = e && e["key"];
    setCapacityForm(copy);
  };

  const onChangeTextInfoSection = (property: string, e: any) => {
    const { _cnfClusterInfoEntity, ...res } = formData as CBOMDto;
    if (_cnfClusterInfoEntity) {
      _cnfClusterInfoEntity[property] = e && e.target.value;
    }
    setFormData({ ...res, _cnfClusterInfoEntity });
  };

  const onChangeTextInstanceSection = (property: string, e: any) => {
    let copy = { ...instanceForm } as CnfClusterInfoDto;
    copy[property] = e && e.target.value;
    setInstanceForm(copy);
  };

  const onChangeTextCapacitySection = (property: string, e: any) => {
    let copy = { ...capacityForm } as CnfCapacityDto;
    if (
      property === "noofcnfinstancespersite" ||
      property === "numberofpodsperpodtype"
    ) {
      copy[property] = safeNumber(e.target.value);
    } else if (property === "financialyear") {
      copy[property] = e;
    } else {
      copy[property] = e && e.target.value;
    }
    setCapacityForm(copy);
  };

  const onDeleteCapacity = ({
    instanceDelecteId,
    instanceIndexId,
    capacityDeleteId,
    capacityIndex,
  }) => {
    if (!formData) return;
    const {
      _cnfClusterInfoEntity: {
        cnfpodinfo = [],
        ...restInfo
      } = {} as CBOMDtoUpdate,
      ...rest
    } = formData as CBOMDto;

    const updatedInstances = (cnfpodinfo ?? []).map((instance, idx) => {
      const isMatch =
        instance.cnfpodinfoid === instanceDelecteId || idx === instanceIndexId;
      if (!isMatch) return instance;

      const currentCapacities = instance.cnfcapacity ?? [];

      const updatedCapacities = currentCapacities.filter(
        (cap, i) => i !== capacityIndex
      );

      return { ...instance, cnfcapacity: updatedCapacities };
    });
    const updatedData: CBOMDto = {
      ...rest,
      _cnfClusterInfoEntity: {
        ...restInfo,
        cnfpodinfo: updatedInstances,
      },
    };

    setFormData(updatedData);
  };

  const onSubmitCapacity = ({
    edit,
    addInstenceId,
  }: {
    edit: boolean;
    addInstenceId?: number | null;
  }) => {
    if (!formData || !capacityForm) return;
    if (
      capacityForm !== null &&
      validateCapacity(capacityForm).response === true
    ) {
      const { instanceId, capacityId } = editCapacityId ?? {};
      const {
        _cnfClusterInfoEntity: {
          cnfpodinfo = [],
          ...restInfo
        } = {} as CBOMDtoUpdate,
        ...rest
      } = formData as CBOMDto;

      const updatedInstances = (cnfpodinfo ?? []).map((instance, idx) => {
        const isMatch =
          instance.cnfpodinfoid == instanceId || idx == instanceId;
        if (!isMatch) return instance;

        const currentCapacities = instance.cnfcapacity ?? [];
        if (edit) {
          const updatedCapacities = currentCapacities.map((cap, i) =>
            cap.cnfcapacityid === capacityId || i === capacityId
              ? capacityForm
              : cap
          );

          return { ...instance, cnfcapacity: updatedCapacities };
        } else {
          const newCapacity = {
            ...capacityForm,
            cnfclusterinfoid: 0,
          };
          return {
            ...instance,
            cnfcapacity: [...currentCapacities, newCapacity],
          };
        }
      });

      const updatedData: CBOMDto = {
        ...rest,
        _cnfClusterInfoEntity: {
          ...restInfo,
          cnfpodinfo: updatedInstances,
        },
      };
      setFormData(updatedData);
      setCapacityForm(null);
      setAddCapacityFlag(false);
      setIsAddCapacity(false);
      setEditCapacityId(null);
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered in Capacity",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }
  };

  const onSubmitInterface = ({
    instanceId,
    edit,
  }: {
    instanceId?: number;
    edit: boolean;
  }) => {
    if (!formData || !instanceForm) return;
    if (
      instanceForm !== null &&
      validateInstance(instanceForm).response === true
    ) {
      const { _cnfClusterInfoEntity = { cnfpodinfo: [] }, ...rest } =
        formData as CBOMDto;
      let instances = [...(_cnfClusterInfoEntity.cnfpodinfo || [])];

      instances = instances?.map((inst, index) => {
        const isMatch =
          inst.cnfpodinfoid === editInterfaceId || index === editInterfaceId;

        if (edit && isMatch) return instanceForm;

        if (!edit && index === instanceId) return instanceForm;

        return inst;
      });

      // Add new instance if not matched
      if (!edit && (!instanceId || instanceId >= instances.length)) {
        instances.push({ ...instanceForm, cnfcapacity: [] });
      }

      const updatedData = {
        ...rest,
        _cnfClusterInfoEntity: {
          ..._cnfClusterInfoEntity,
          cnfpodinfo: instances.map((res) => ({
            ...res,
          })),
        },
      } as CBOMDto;

      setFormData(updatedData);
      setInstanceForm(null);
      setAddInterfaceFlag(false);
      setIsAddInstance(false);
      setEditInterfaceId(null);
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered in Instance",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }
  };

  const onDelectInstance = ({ instanceId, instanceIndex }) => {
    if (!formData) return;
    const { _cnfClusterInfoEntity = { cnfpodinfo: [] }, ...rest } =
      formData as CBOMDto;
    let instances = [...(_cnfClusterInfoEntity.cnfpodinfo || [])];

    instances = instances?.filter((inst, index) => index !== instanceIndex);

    const updatedData = {
      ...rest,
      _cnfClusterInfoEntity: {
        ..._cnfClusterInfoEntity,
        cnfpodinfo: instances,
      },
    } as CBOMDto;

    setFormData(updatedData);
  };

  useImperativeHandle(ref, () => ({
    onSaveFormData,
  }));

  const onSaveFormData = () => {
    if (!formData) return;

    const { _cnfClusterInfoEntity, ...rest } = formData as CBOMDto;

    const updatedInstances =
      _cnfClusterInfoEntity?.cnfpodinfo?.map((res) => ({
        ...res,
        cnfpodinfoid: res.cnfpodinfoid ?? 0,
        // numa: res.numa === "Yes" || res.numa === true,
      })) ?? [];

    const validateObject = (obj: any): { pass: boolean; message: string } => {
      if (!obj || typeof obj !== "object" || Object.keys(obj).length === 0) {
        return {
          pass: false,
          message: "Invalid input: empty or not an object.",
        };
      }

      if (formData && validazioneClient(formData).response === false) {
        return {
          pass: false,
          message: "Please check the fields.",
        };
      }

      const instances = (obj as CBOMDtoUpdate).cnfpodinfo;
      if (!Array.isArray(instances) || instances.length === 0) {
        return {
          pass: false,
          message: "Please add at least one Instance.",
        };
      }

      const invalidInstanceIndex = instances.findIndex(
        (inst) =>
          !Array.isArray(inst.cnfcapacity) || inst.cnfcapacity.length === 0
      );

      if (invalidInstanceIndex !== -1) {
        return {
          pass: false,
          message: `Instance row ${
            invalidInstanceIndex + 1
          } has missing capacity. Please add at least one "Capacity" for each Instance.`,
          // message: `Instance ${invalidInstanceIndex + 1} is missing capacity. Please add at least one "Capacity" for each Instance.`,
        };
      }

      return { pass: true, message: "Validation passed." };
    };

    const validation = validateObject(
      _cnfClusterInfoEntity ? _cnfClusterInfoEntity : {}
    );
    if (validation?.pass) {
      Save(
        {
          ...rest,
          _cnfClusterInfoEntity: {
            ..._cnfClusterInfoEntity,
            // cpukubelet: _cnfClusterInfoEntity?.cpukubelet ?? 0.5,
            // cpusystem: _cnfClusterInfoEntity?.cpusystem ?? 0.5,
            // memkubelet: _cnfClusterInfoEntity?.memkubelet ?? 2,
            // memsystem: _cnfClusterInfoEntity?.memsystem ?? 2,
            cnfpodinfo: updatedInstances,
          },
        },
        props.edit,
        validazioneClient,
        refresh
      );
    } else {
      rootStore.dispatch(
        setNotification({
          message: validation?.message ?? "",
          notifyType: NotifyType.warning,
        })
      );
    }
  };

  const getInterfaceColumns: Column<CnfClusterInfoDto>[] = [
    { key: "podtypeinfo", label: "Pod Type" },
    { key: "functionstandard", label: "Function Standard" },
    { key: "priority", label: "Priority" },
    { key: "podroledescription", label: "Description Of Each Pod Role" },
    { key: "daemonsetpod", label: "Daemon Set Pod" },
    { key: "intrapodrules", label: "Intra Pod Rules" },
    { key: "interpodrules", label: "Inter Pod Rules" },
    { key: "isenhancedha", label: "Enhanced HA" },
    { key: "podtypeqos", label: "Pod Type QoS" },
    { key: "ispersistancestorageflag", label: "Persistance Storage Flag" },
    { key: "isprodhpaenable", label: "Pod HPA" },
  ];

  const getCapacityColumns: Column<CnfCapacityDto>[] = [
    { key: "financialyear", label: "Financial Year" },
    {
      key: "financialversion",
      label: "Version",
    },
    { key: "noofcnfinstancespersite", label: "No of CNF Instances Per Site" },
    { key: "numberofpodsperpodtype", label: "Number of Pods per Pod Type" },
    { key: "vcpurequestforpodtype", label: "VCPU Request" },
    { key: "memrequestforpodtype", label: "Mem Request Pod Type" },
    {
      key: "nonpresistentstorageforprodtype",
      label: "Non-Persistent Storage Pod Type",
    },
    {
      key: "ispresistentvolumesrequired",
      label: "Persistent Volumes Required",
    },
    {
      key: "persistentvolumneaccessmode",
      label: "Persistent Volume Access Mode",
    },
    {
      key: "persistentstorageforpodtype",
      label: "Persistent Storage Pod Type",
    },
    {
      key: "storageiopsforpodtype",
      label: "Storage IOPS Pod Type",
    },
    {
      key: "storagerworkloaddistribution",
      label: "Storage RW Workload Distribution",
    },
    {
      key: "northsouthbandwidthforpodtype",
      label: "North/South BandWidth PodType",
    },
    {
      key: "eastwestbandwidthforpodtype",
      label: "East/West BandWidth PodType",
    },
    {
      key: "specialrequirementperpodtype",
      label: "Special Requirement Per PodType",
    },
    {
      key: "capacityspecialrequirement",
      label: "Capacity Special Requirement",
    },
  ];

  const isMultipleOfTwo = (value: string | number): boolean => {
    const num = Number(value);
    return num >= 2 && num % 2 === 0;
  };
  const isMultipleOfFour = (value: string | number): boolean => {
    const num = Number(value);
    return num >= 4 && num % 4 === 0;
  };

  const onAddInstanceHandle = () => {
    // if (formData && validazioneClient(formData).response === false) {
    //   rootStore.dispatch(
    //     setNotification({
    //       message: "Please check the fields entered in CNF Info Section.",
    //       notifyType: NotifyType.warning,
    //     })
    //   );
    // } else {
    //   setAddInterfaceFlag(true);
    // }
    setAddInterfaceFlag(true);
  };

  const handleClusterInstance = () => {
    setIsAddInstance(!isAddInstance);
    setInstanceForm(null);
    setAddInterfaceFlag(false);
    setEditInterfaceId(null);
  };
  const handleClusterCapacity = () => {
    setEditCapacityId({
      instanceId: instanceIndex,
      capacityId: null,
    });
    setIsAddCapacity(!isAddCapacity);
    setVersionError(false);
    setCapacityForm(null);
    setAddCapacityFlag(false);
  };

  const yearVersionCheck = ({ year, version }: { year: any; version: any }) => {
    setVersionError(false);
    const obj1 =
      formData?._cnfClusterInfoEntity?.cnfpodinfo
        ?.filter((res) => res.cnfpodinfoid === props.instanceKey)[0]
        ?.cnfcapacity?.map((val, index) => ({
          year: safeNumber(val.financialyear),
          version: safeNumber(val.financialversion),
          index: val.cnfcapacityid,
        })) || [];

    const obj2 = {
      year: year ? safeNumber(year) : safeNumber(capacityForm?.financialyear),
      version: version
        ? safeNumber(version?.key)
        : safeNumber(capacityForm?.financialversion),
      index:
        isAddCapacity && editCapacityId?.capacityId === null
          ? -1
          : editCapacityId?.capacityId,
    };
    function hasDuplicateConflict(arr, obj) {
      return arr.some((item) => {
        if (item.index === obj.index) return false;
        return item.year === obj.year && item.version === obj.version;
      });
    }
    const conflict = hasDuplicateConflict(obj1, obj2);
    if (conflict) {
      setVersionError(true);
    } else {
      setVersionError(false);
    }
  };

  const updateCNFPodTypeValue = (value: any) => {
    setFormData((prevData) => {
      if (!prevData) return prevData;

      return {
        ...prevData,
        podTypeInfoResourceResource: value?.map((res) => ({
          text: res?.podTypeInfoName,
          value: String(res?.podTypeInfoId),
        })),
      };
    });
  };

  const updateCNFPriorityResourceValue = (value: any) => {
    setFormData((prevData) => {
      if (!prevData) return prevData;

      return {
        ...prevData,
        priorityResource: value?.map((res) => ({
          text: res?.description,
          value: String(res?.cnfPriorityId),
        })),
      };
    });
  };

  const updateCNFHardwareTypeResourceValue = (value: any) => {
    setFormData((prevData) => {
      if (!prevData) return prevData;

      return {
        ...prevData,
        hardwareResource: value?.map((res) => ({
          text: res?.description,
          value: String(res?.cnfHardwareId),
        })),
      };
    });
  };

  return (
    <div className="col-12 px-0">
      <ModalRelated
        show={isVisibleModalRelated}
        data={relatedRecord}
        headerTitle="Edit VBOM Entry"
        action={{
          closeModal: () => {
            setIsVisibleModalRelated(false);
          },
        }}
      />
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
          {}
        </DialogContent>
      </Dialog>

      <Box sx={{ flexGrow: 1 }}>
        <Grid container spacing={2} columns={16}>
          <Grid size={{ xs: 24 }}>
            <Item
              sx={{
                border: "thin solid lightgrey",
                borderRadius: "7px",
                padding: 0,
              }}
            >
              <div
                className="col-12 mx-0"
                style={{
                  padding: "1rem",
                  paddingLeft: "1rem !important",
                  display: "flex",
                  justifyContent: "space-between",
                }}
              >
                <h5
                  className="mb-0"
                  style={{
                    fontSize: "18px",
                    fontWeight: "bold",
                    alignContent: "center",
                  }}
                >
                  CNF Cluster Info Details
                </h5>
              </div>
              <Divider />
              <div className="col-12" style={{ padding: "15px" }}>
                <fieldset className="fieldset p-0">
                  <div className="row">
                    <div className="col-3">
                      <div className="col-12 px-0 d-flex align-items-end">
                        <div className="flex-grow-1 pr-2">
                          <DropdownInputComponent
                            label={"CNF Name"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={true}
                            disabled={false}
                            value={
                              formData?.cnfNameResources &&
                              resourceArrayRefactor(
                                formData?.cnfNameResources
                              ).filter(
                                (x) =>
                                  x.key ==
                                  formData?._cnfClusterInfoEntity?.cnfnameid
                              )
                            }
                            options={
                              formData?.cnfNameResources &&
                              resourceArrayRefactor(formData?.cnfNameResources)
                            }
                            isError={
                              validation &&
                              validation.response === false &&
                              validation.property?.includes("cnfnameid")
                                ? true
                                : false
                            }
                            error="CNF Name must have a value."
                            onChange={(e: any) =>
                              onChangeDropdownInfoSection("cnfnameid", e)
                            }
                          />
                        </div>
                        <div className="pb-4">
                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link"
                              onClick={() => setIsCNFNameCrudModalOpen(true)}
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
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <DropdownInputComponent
                          label={"Node Pool Breakup"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          value={
                            evumOption &&
                            evumOption.filter(
                              (x) =>
                                x.key ==
                                formData?._cnfClusterInfoEntity?.nodepoolbreakup
                            )
                          }
                          options={evumOption}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("nodepoolbreakup")
                              ? true
                              : false
                          }
                          error="Node Pool Breakup must have a value."
                          onChange={(e: any) =>
                            onChangeDropdownInfoSection("nodepoolbreakup", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0 d-flex align-items-end">
                        <div className="flex-grow-1 pr-2">
                          <DropdownInputComponent
                            label={"CNF Cluster"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={true}
                            value={
                              formData?.cnfClusterNameResource &&
                              formData?.cnfClusterNameResource
                                ?.filter(
                                  (val) =>
                                    val.cnfnameid ===
                                      formData?._cnfClusterInfoEntity
                                        ?.cnfnameid &&
                                    val.cnfclusterid ===
                                      formData?._cnfClusterInfoEntity
                                        ?.cnfclusterid
                                )
                                ?.map((res) => ({
                                  key: res.cnfclusterid,
                                  value: res.cnfclustername,
                                }))
                            }
                            options={
                              formData?._cnfClusterInfoEntity?.cnfnameid !== 0
                                ? formData?.cnfClusterNameResource &&
                                  formData?.cnfClusterNameResource
                                    ?.filter(
                                      (val) =>
                                        formData?._cnfClusterInfoEntity
                                          ?.cnfnameid === val.cnfnameid
                                    )
                                    ?.map((res) => ({
                                      key: res.cnfclusterid,
                                      value: res.cnfclustername,
                                    }))
                                : null
                            }
                            isError={
                              validation &&
                              validation.response === false &&
                              validation.property?.includes("cnfclusterid")
                                ? true
                                : false
                            }
                            error="CNF Cluster must have a value."
                            onChange={(e: any) =>
                              onChangeDropdownInfoSection("cnfclusterid", e)
                            }
                          />
                        </div>
                        <div className="pb-4">
                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link"
                              onClick={() => setIsCNFClusterCrudModalOpen(true)}
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
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <DropdownInputComponent
                          label={"Node Pool"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          value={
                            formData?.cnfClusterNodePoolResource &&
                            formData?.cnfClusterNodePoolResource
                              ?.filter(
                                (val) =>
                                  val.cnfnameid ===
                                    formData?._cnfClusterInfoEntity
                                      ?.cnfnameid &&
                                  val.cnfclusterid ===
                                    formData?._cnfClusterInfoEntity
                                      ?.cnfclusternodepoolid
                              )
                              ?.map((res) => ({
                                key: res.cnfclusterid,
                                value: res.nodepool,
                              }))
                          }
                          options={
                            formData?._cnfClusterInfoEntity?.cnfnameid !== 0
                              ? formData?.cnfClusterNodePoolResource &&
                                formData?.cnfClusterNodePoolResource
                                  ?.filter(
                                    (val) =>
                                      formData?._cnfClusterInfoEntity
                                        ?.cnfnameid === val.cnfnameid
                                  )
                                  ?.map((res) => ({
                                    key: res.cnfclusterid,
                                    value: res.nodepool,
                                  }))
                              : null
                          }
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "cnfclusternodepoolid"
                            )
                              ? true
                              : false
                          }
                          error="*Node Pool must have a value."
                          onChange={(e: any) =>
                            onChangeDropdownInfoSection(
                              "cnfclusternodepoolid",
                              e
                            )
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <DropdownInputComponent
                          label={"Opco"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          value={
                            opcoOptions &&
                            opcoOptions.filter(
                              (x) =>
                                x.key == formData?._cnfClusterInfoEntity?.opcoid
                            )
                          }
                          options={opcoOptions}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("opcoid")
                              ? true
                              : false
                          }
                          error="Opco must have a value."
                          onChange={(e: any) =>
                            onChangeDropdownInfoSection("opcoid", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <DropdownInputComponent
                          label={"Site"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          value={formData?.opcoBasedLocationResource
                            ?.filter(
                              (val) =>
                                formData?._cnfClusterInfoEntity?.opcoid ===
                                val.opcoId
                            )?.[0]
                            ?.locationDetails?.map((res) => ({
                              key: res.value,
                              value: res.shortDescription,
                            }))
                            .filter(
                              (x) =>
                                x.key == formData?._cnfClusterInfoEntity?.siteid
                            )}
                          options={
                            formData?._cnfClusterInfoEntity?.opcoid !== 0
                              ? formData?.opcoBasedLocationResource
                                  ?.filter(
                                    (val) =>
                                      formData?._cnfClusterInfoEntity
                                        ?.opcoid === val.opcoId
                                  )?.[0]
                                  ?.locationDetails?.map((res) => ({
                                    key: res.value,
                                    value: res.shortDescription,
                                  }))
                              : null
                          }
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("siteid")
                              ? true
                              : false
                          }
                          error="*Site must have a value."
                          onChange={(e: any) =>
                            onChangeDropdownInfoSection("siteid", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0 d-flex align-items-end">
                        <div className="flex-grow-1 pr-2">
                          <DropdownInputComponent
                            label={"Hardware Type"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={true}
                            value={
                              formData?.hardwareResource &&
                              resourceArrayRefactor(
                                formData?.hardwareResource
                              )?.filter(
                                (x) =>
                                  x.key ==
                                  formData?._cnfClusterInfoEntity?.cnfhardwareid
                              )
                            }
                            options={
                              formData?.hardwareResource &&
                              resourceArrayRefactor(formData?.hardwareResource)
                            }
                            isError={
                              validation &&
                              validation.response === false &&
                              validation.property?.includes("cnfhardwareid")
                                ? true
                                : false
                            }
                            error="Hardware Type must have a value."
                            onChange={(e: any) =>
                              onChangeDropdownInfoSection("cnfhardwareid", e)
                            }
                          />
                        </div>
                        <div className="pb-4">
                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link"
                              onClick={() =>
                                setIsCNFHardwareTypeCrudModalOpen(true)
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
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Special Requirements"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity?.specialrequirements
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("specialrequirements")
                              ? true
                              : false
                          }
                          error={"*Special Requirements must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("specialrequirements", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Hyper Threading"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity?.hyperthreading
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("hyperthreading")
                              ? true
                              : false
                          }
                          error={"*Hyper Threading must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("hyperthreading", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Over Provisioning"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity?.overprovisioning
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("overprovisioning")
                              ? true
                              : false
                          }
                          error={"*Over Provisioning must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("overprovisioning", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Worker Node Configuration"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity
                              ?.workernodeconfiguration
                          }
                          required={false}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "workernodeconfiguration"
                            )
                              ? true
                              : false
                          }
                          error={
                            "*Worker Node Configuration must have a value."
                          }
                          onChange={(e: any) =>
                            onChangeTextInfoSection(
                              "workernodeconfiguration",
                              e
                            )
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Hardware"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={formData?._cnfClusterInfoEntity?.hardware}
                          required={false}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("hardware")
                              ? true
                              : false
                          }
                          error={"*Hardware must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("hardware", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="CPU Kubelet"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity?.cpukubelet ?? null
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("cpukubelet")
                              ? true
                              : false
                          }
                          validationError={
                            inputError?.field === "cpukubelet" ? true : false
                          }
                          error={
                            inputError?.field === "cpukubelet"
                              ? "*Only numbers between 0 and 1 (inclusive) are allowed."
                              : "*CPU Kubelet must have a value."
                          }
                          onChange={(
                            e: React.ChangeEvent<HTMLInputElement>
                          ) => {
                            const value = e.target.value;

                            // Allow empty string temporarily for typing
                            if (
                              value === "" ||
                              /^(\d+)?(\.\d*)?$/.test(value)
                            ) {
                              const numeric = parseFloat(value);

                              // Allow values from 0 to 1
                              if (
                                value === "" ||
                                (!isNaN(numeric) &&
                                  numeric >= 0 &&
                                  numeric <= 1)
                              ) {
                                onChangeTextInfoSection("cpukubelet", e);
                                setInputError(null);
                              } else {
                                setInputError({
                                  field: "cpukubelet",
                                  type: "number",
                                });
                              }
                            } else {
                              setInputError({
                                field: "cpukubelet",
                                type: "number",
                              });
                            }
                          }}
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Mem Kubelet"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity?.memkubelet ?? null
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("memkubelet")
                              ? true
                              : false
                          }
                          validationError={
                            inputError?.field === "memkubelet" ? true : false
                          }
                          error={
                            inputError?.field === "memkubelet"
                              ? "*Only integers between 1 and 4 are allowed."
                              : "*Mem Kubelet must have a value."
                          }
                          onChange={(
                            e: React.ChangeEvent<HTMLInputElement>
                          ) => {
                            const value = e.target.value;

                            // Allow only integers
                            if (/^\d*$/.test(value)) {
                              const numeric = parseInt(value, 10);

                              // Allow between 1 and 4
                              if (
                                value === "" ||
                                (!isNaN(numeric) &&
                                  numeric >= 1 &&
                                  numeric <= 4)
                              ) {
                                onChangeTextInfoSection("memkubelet", e);
                                setInputError(null);
                              } else {
                                setInputError({
                                  field: "memkubelet",
                                  type: "number",
                                });
                              }
                            } else {
                              setInputError({
                                field: "memkubelet",
                                type: "number",
                              });
                            }
                          }}
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="CPU System"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity?.cpusystem ?? null
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("cpusystem")
                              ? true
                              : false
                          }
                          validationError={
                            inputError?.field === "cpusystem" ? true : false
                          }
                          error={
                            inputError?.field === "cpusystem"
                              ? "*Only numbers between 0 and 1 (inclusive) are allowed."
                              : "*CPU System must have a value."
                          }
                          onChange={(
                            e: React.ChangeEvent<HTMLInputElement>
                          ) => {
                            const value = e.target.value;

                            // Allow empty string temporarily for typing
                            if (
                              value === "" ||
                              /^(\d+)?(\.\d*)?$/.test(value)
                            ) {
                              const numeric = parseFloat(value);

                              // Allow values from 0 to 1
                              if (
                                value === "" ||
                                (!isNaN(numeric) &&
                                  numeric >= 0 &&
                                  numeric <= 1)
                              ) {
                                onChangeTextInfoSection("cpusystem", e);
                                setInputError(null);
                              } else {
                                setInputError({
                                  field: "cpusystem",
                                  type: "number",
                                });
                              }
                            } else {
                              setInputError({
                                field: "cpusystem",
                                type: "number",
                              });
                            }
                          }}
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Mem System"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity?.memsystem ?? null
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("memsystem")
                              ? true
                              : false
                          }
                          validationError={
                            inputError?.field === "memsystem" ? true : false
                          }
                          error={
                            inputError?.field === "memsystem"
                              ? "*Only integers between 1 and 4 are allowed."
                              : "*Mem System must have a value."
                          }
                          onChange={(
                            e: React.ChangeEvent<HTMLInputElement>
                          ) => {
                            const value = e.target.value;

                            // Allow only integers
                            if (/^\d*$/.test(value)) {
                              const numeric = parseInt(value, 10);

                              // Allow between 1 and 4
                              if (
                                value === "" ||
                                (!isNaN(numeric) &&
                                  numeric >= 1 &&
                                  numeric <= 4)
                              ) {
                                onChangeTextInfoSection("memsystem", e);
                                setInputError(null);
                              } else {
                                setInputError({
                                  field: "memsystem",
                                  type: "number",
                                });
                              }
                            } else {
                              setInputError({
                                field: "memsystem",
                                type: "number",
                              });
                            }
                          }}
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <DropdownInputComponent
                          label={"Vertical Domain"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={true}
                          required={true}
                          value={
                            formData?.verticalResource &&
                            resourceArrayRefactor(
                              formData?.verticalResource
                            ).filter(
                              (x) =>
                                x.key ==
                                formData?._cnfClusterInfoEntity
                                  ?.verticalresponsibleid
                            )
                          }
                          options={
                            formData?.verticalResource &&
                            resourceArrayRefactor(formData?.verticalResource)
                          }
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "verticalresponsibleid"
                            )
                              ? true
                              : false
                          }
                          error="Vertical Domain must have a value."
                          onChange={(e: any) =>
                            onChangeDropdownInfoSection(
                              "verticalresponsibleid",
                              e
                            )
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="File Name"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={formData?._cnfClusterInfoEntity?.filename}
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("filename")
                              ? true
                              : false
                          }
                          error={"*File Name must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("filename", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Revision"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={formData?._cnfClusterInfoEntity?.revision}
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("revision")
                              ? true
                              : false
                          }
                          error={"*Revision must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("revision", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Aggregate Image Cluster Size"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._cnfClusterInfoEntity
                              ?.aggregateimageclustersize
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes(
                              "aggregateimageclustersize"
                            )
                              ? true
                              : false
                          }
                          error={
                            "*Aggregate Image Cluster Size must have a value."
                          }
                          onChange={(e: any) =>
                            onChangeTextInfoSection(
                              "aggregateimageclustersize",
                              e
                            )
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Comments"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={formData?._cnfClusterInfoEntity?.comments}
                          required={false}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("comments")
                              ? true
                              : false
                          }
                          error={"*Comments must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("comments", e)
                          }
                        />
                      </div>
                    </div>
                    <div className="col-3">
                      <div className="col-12 px-0">
                        <TextInputComponent
                          label="Note"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={formData?._cnfClusterInfoEntity?.notes}
                          required={false}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("notes")
                              ? true
                              : false
                          }
                          error={"*Note must have a value."}
                          onChange={(e: any) =>
                            onChangeTextInfoSection("notes", e)
                          }
                        />
                      </div>
                    </div>
                  </div>
                </fieldset>
              </div>
            </Item>
          </Grid>
          <Grid size={{ xs: 8 }}>
            <Item
              sx={{
                border: "thin solid lightgrey",
                borderRadius: "7px",
                padding: 0,
              }}
            >
              <div
                className="col-12 mx-0"
                style={{
                  padding: "1rem",
                  paddingLeft: "1rem !important",
                  display: "flex",
                  justifyContent: "space-between",
                }}
              >
                <h5
                  className="mb-0"
                  style={{
                    fontSize: "18px",
                    fontWeight: "bold",
                    alignContent: "center",
                  }}
                >
                  CNF Instance Details
                </h5>
                <Button
                  variant="contained"
                  color={!isAddInstance ? "error" : "primary"}
                  size="small"
                  startIcon={
                    isAddInstance ? (
                      <MdGridView size={15} />
                    ) : (
                      <MdAdd size={15} />
                    )
                  }
                  onClick={() => handleClusterInstance()}
                  disabled={isAddCapacity}
                  style={{ borderRadius: "20px" }}
                >
                  {isAddInstance ? "View Grid" : "Add Instance"}
                </Button>
              </div>
              <Divider />
              {isAddInstance ? (
                <>
                  <div
                    className="col-12"
                    style={{
                      padding: "15px",
                      height: "38rem",
                      overflowY: "auto",
                    }}
                  >
                    <fieldset className="fieldset p-0">
                      <div className="row">
                        <div className="col-6">
                          <div className="col-12 px-0 d-flex align-items-end">
                            <div className="flex-grow-1 pr-2">
                              <DropdownInputComponent
                                label={"Pod Type"}
                                labelCSS="mb-0 text-left"
                                inputCSS="labelForm voda-bold mb-2"
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.podTypeInfoResource &&
                                  formData?.podTypeInfoResource
                                    ?.map((res) => ({
                                      key: res.podtypeinfoid,
                                      value: res.podtypeinfoname,
                                    }))
                                    ?.filter(
                                      (x) =>
                                        x.key == instanceForm?.podtypeinfoid
                                    )
                                }
                                options={
                                  formData?.podTypeInfoResource?.map((res) => ({
                                    key: res.podtypeinfoid,
                                    value: res.podtypeinfoname,
                                  })) ?? []
                                }
                                isError={
                                  validationInstanceForm &&
                                  validationInstanceForm.response === false &&
                                  validationInstanceForm.property?.includes(
                                    "podtypeinfoid"
                                  )
                                    ? true
                                    : false
                                }
                                error="Pod Type must have a value."
                                onChange={(e: any) =>
                                  onChangeInstance("podtypeinfoid", e)
                                }
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() =>
                                    setIsPodTypeInfoCrudModalOpen(true)
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
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0 d-flex align-items-end">
                            <div className="flex-grow-1 pr-2">
                              <DropdownInputComponent
                                label={"Function Standard"}
                                labelCSS="mb-0 text-left"
                                inputCSS="labelForm voda-bold mb-2"
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.functionStandardedNameResource &&
                                  formData?.functionStandardedNameResource
                                    ?.filter(
                                      (val) =>
                                        instanceForm?.functionstandardid ===
                                        val.functionstandardnameid
                                    )
                                    ?.map((res) => ({
                                      key: res.functionstandardnameid,
                                      value: res.functionname,
                                    }))
                                }
                                options={
                                  instanceForm?.functionstandardid !== 0
                                    ? formData?.functionStandardedNameResource &&
                                      formData?.functionStandardedNameResource?.map(
                                        (res) => ({
                                          key: res.functionstandardnameid,
                                          value: res.functionname,
                                        })
                                      )
                                    : null
                                }
                                isError={
                                  validationInstanceForm &&
                                  validationInstanceForm.response === false &&
                                  validationInstanceForm.property?.includes(
                                    "functionstandardid"
                                  )
                                    ? true
                                    : false
                                }
                                error="*Function Standard must have a value."
                                onChange={(e: any) =>
                                  onChangeInstance("functionstandardid", e)
                                }
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() =>
                                    setIsCnfFunctionStandardNameCrudModalOpen(
                                      true
                                    )
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
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0 d-flex align-items-end">
                            <div className="flex-grow-1 pr-2">
                              <DropdownInputComponent
                                label={"Priority"}
                                labelCSS="mb-0 text-left"
                                inputCSS="labelForm voda-bold mb-2"
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.priorityResource &&
                                  resourceArrayRefactor(
                                    formData?.priorityResource
                                  )?.filter(
                                    (val) =>
                                      instanceForm?.priorityid === val.key
                                  )
                                }
                                options={
                                  formData?.priorityResource &&
                                  resourceArrayRefactor(
                                    formData?.priorityResource
                                  )
                                }
                                isError={
                                  validationInstanceForm &&
                                  validationInstanceForm.response === false &&
                                  validationInstanceForm.property?.includes(
                                    "priorityid"
                                  )
                                    ? true
                                    : false
                                }
                                error="*Priority must have a value."
                                onChange={(e: any) =>
                                  onChangeInstance("priorityid", e)
                                }
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() =>
                                    setIsCNFPriorityCrudModalOpen(true)
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
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Description Of Each Pod Role"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                formData?.podTypeDescriptionInfoResource &&
                                formData?.podTypeDescriptionInfoResource
                                  ?.filter(
                                    (val) =>
                                      instanceForm?.podtypeinfoid ===
                                      val.podtypeinfoid
                                  )
                                  ?.map((res) => ({
                                    key: res.podtypeinfoid,
                                    value: res.podroledescription,
                                  }))
                                  ?.filter(
                                    (res) =>
                                      res.key ===
                                      instanceForm?.podroledescriptionid
                                  )
                              }
                              options={
                                instanceForm?.podtypeinfoid !== 0
                                  ? formData?.podTypeDescriptionInfoResource &&
                                    formData?.podTypeDescriptionInfoResource
                                      ?.filter(
                                        (val) =>
                                          instanceForm?.podtypeinfoid ===
                                          val.podtypeinfoid
                                      )
                                      ?.map((res) => ({
                                        key: res.podtypeinfoid,
                                        value: res.podroledescription,
                                      }))
                                  : []
                              }
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "podroledescriptionid"
                                )
                                  ? true
                                  : false
                              }
                              error="*Description Of Each Pod Role must have a value."
                              onChange={(e: any) =>
                                onChangeInstance("podroledescriptionid", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Daemon Set Pod"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) => x.key == instanceForm?.daemonsetpod
                                    )
                                  : null
                              }
                              options={evumOption}
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "daemonsetpod"
                                )
                                  ? true
                                  : false
                              }
                              error="*Daemon Set Pod must have a value."
                              onChange={(e: any) =>
                                onChangeInstance("daemonsetpod", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Intra Pod Rules"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={instanceForm?.intrapodrules}
                              required={false}
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "intrapodrules"
                                )
                                  ? true
                                  : false
                              }
                              error={"*IntraPodRules must have a value."}
                              onChange={(e: any) =>
                                onChangeTextInstanceSection("intrapodrules", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Inter Pod Rules"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={instanceForm?.interpodrules}
                              required={false}
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "interpodrules"
                                )
                                  ? true
                                  : false
                              }
                              error={"*InterPodRules must have a value."}
                              onChange={(e: any) =>
                                onChangeTextInstanceSection("interpodrules", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Enhanced HA"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) => x.key == instanceForm?.isenhancedha
                                    )
                                  : null
                              }
                              options={evumOption}
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "isenhancedha"
                                )
                                  ? true
                                  : false
                              }
                              error="*IsEnhancedHa must have a value."
                              onChange={(e: any) =>
                                onChangeInstance("isenhancedha", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Pod Type Qos"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={instanceForm?.podtypeqos}
                              required={true}
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "podtypeqos"
                                )
                                  ? true
                                  : false
                              }
                              error={"*PodTypeQoS must have a value."}
                              onChange={(e: any) =>
                                onChangeTextInstanceSection("podtypeqos", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Persistance Storage Flag"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={instanceForm?.ispersistancestorageflag}
                              required={false}
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "ispersistancestorageflag"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*IsPersistanceStorageFlag must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextInstanceSection(
                                  "ispersistancestorageflag",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Pod HPA"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) =>
                                        x.key == instanceForm?.isprodhpaenable
                                    )
                                  : null
                              }
                              options={evumOption}
                              isError={
                                validationInstanceForm &&
                                validationInstanceForm.response === false &&
                                validationInstanceForm.property?.includes(
                                  "isprodhpaenable"
                                )
                                  ? true
                                  : false
                              }
                              error="*Is Prod Hpa Enable must have a value."
                              onChange={(e: any) =>
                                onChangeInstance("isprodhpaenable", e)
                              }
                            />
                          </div>
                        </div>
                      </div>
                    </fieldset>
                  </div>
                  <Divider />
                  <div
                    className="col-12 mx-0"
                    style={{
                      padding: "1rem",
                      paddingLeft: "1rem !important",
                      display: "flex",
                      justifyContent: "right",
                    }}
                  >
                    <Button
                      variant="contained"
                      color={"inherit"}
                      size="small"
                      style={{ borderRadius: "20px", marginRight: "1rem" }}
                      onClick={() => {
                        setInstanceForm(null);
                        setAddInterfaceFlag(false);
                        setEditInterfaceId(null);
                        setIsAddInstance(false);
                      }}
                    >
                      Cancel
                    </Button>
                    <Button
                      variant="contained"
                      color={"error"}
                      size="small"
                      style={{ borderRadius: "20px" }}
                      onClick={() =>
                        onSubmitInterface({
                          edit: editInterfaceId !== null ? true : false,
                        })
                      }
                      disabled={false}
                    >
                      {editInterfaceId !== null ? "Update" : "Save"} Instance
                    </Button>
                  </div>
                </>
              ) : (
                <div
                  className="col-12"
                  style={{
                    padding: "15px",
                    minHeight: `${
                      isAddCapacity || isAddInstance ? "42rem" : "29rem"
                    }`,
                    maxHeight: "42rem",
                    overflowY: "auto",
                  }}
                >
                  <CustomMUITable
                    columns={getInterfaceColumns}
                    data={
                      formData?._cnfClusterInfoEntity?.cnfpodinfo?.map(
                        (res) => ({
                          ...res,
                          isenhancedha:
                            res?.isenhancedha === true ? "Yes" : "No",
                          daemonsetpod:
                            res?.daemonsetpod === true ? "Yes" : "No",
                          isprodhpaenable:
                            res?.isprodhpaenable === true ? "Yes" : "No",
                          podtypeinfo: formData?.podTypeInfoResource?.filter(
                            (val: any) => val.podtypeinfoid == res.podtypeinfoid
                          )[0]?.["podtypeinfoname"],
                          functionstandard:
                            formData?.functionStandardedNameResource?.filter(
                              (val: any) =>
                                val.functionstandardnameid ==
                                res.functionstandardid
                            )[0]?.["functionname"],
                          priority: formData?.priorityResource?.filter(
                            (val: any) => val.key == res.priorityid
                          )[0]?.["text"],
                          podroledescription:
                            formData?.podTypeDescriptionInfoResource &&
                            formData?.podTypeDescriptionInfoResource?.filter(
                              (val) =>
                                res?.podtypeinfoid === val?.podtypeinfoid &&
                                res.podroledescriptionid === val?.podtypeinfoid
                            )?.[0]?.["podroledescription"],
                        })
                      ) ?? []
                    }
                    activeIndex={instanceIndex}
                    onEdit={(row: any, index: any) => {
                      setInstanceForm({
                        ...row,
                        isenhancedha: row.isenhancedha === "Yes" ? true : false,
                        daemonsetpod: row.daemonsetpod === "Yes" ? true : false,
                        isprodhpaenable:
                          row.isprodhpaenable === "Yes" ? true : false,
                      });
                      setEditInterfaceId(row?.cnfpodinfoid ?? index);
                      setAddInterfaceFlag(true);
                      setIsAddInstance(true);
                    }}
                    onDelete={(row, idx) =>
                      onDelectInstance({ instanceId: null, instanceIndex: idx })
                    }
                    onRowClick={(row, idx) => {
                      setInstanceIndex(idx);
                      setIsAddCapacity(false);
                    }}
                    tableName="CnfInstance"
                    size="small"
                  />
                </div>
              )}
            </Item>
          </Grid>
          <Grid size={{ xs: 8 }}>
            <Item
              sx={{
                border: "thin solid lightgrey",
                borderRadius: "7px",
                padding: 0,
              }}
            >
              <div
                className="col-12 mx-0"
                style={{
                  padding: "1rem",
                  paddingLeft: "1rem !important",
                  display: "flex",
                  justifyContent: "space-between",
                }}
              >
                <h5
                  className="mb-0"
                  style={{
                    fontSize: "18px",
                    fontWeight: "bold",
                    alignContent: "center",
                  }}
                >
                  CNF Capacity Details
                </h5>
                <Button
                  variant="contained"
                  color={isAddCapacity ? "primary" : "error"}
                  size="small"
                  startIcon={
                    isAddCapacity ? (
                      <MdGridView size={15} />
                    ) : (
                      <MdAdd size={15} />
                    )
                  }
                  style={{ borderRadius: "20px" }}
                  onClick={() => handleClusterCapacity()}
                  disabled={isAddInstance}
                >
                  {isAddCapacity ? "View Grid" : "Add Capacity"}
                </Button>
              </div>
              <Divider />
              {isAddCapacity ? (
                <>
                  <div
                    className="col-12"
                    style={{
                      padding: "15px",
                      height: "38rem",
                      overflowY: "auto",
                    }}
                  >
                    <fieldset className="fieldset p-0">
                      <div className="row">
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <ShowYearInputComponent
                              label={`${
                                LabelsDictionary["financialyear"]?.Full ??
                                "Financial Year"
                              }`}
                              labelCSS="mb-0 text-left"
                              value={
                                capacityForm?.financialyear &&
                                new Date(`${capacityForm?.financialyear}/1/1`)
                              }
                              required={true}
                              // disableYears={[2020, 2021, 2022]}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "financialyear"
                                )
                                  ? true
                                  : false
                              }
                              error={"*Financial Year must have a value."}
                              onChange={(e, newDate) => {
                                e.preventDefault();
                                onChangeTextCapacitySection(
                                  "financialyear",
                                  `${newDate.getFullYear()}`
                                );
                                yearVersionCheck({
                                  year: `${newDate.getFullYear()}`,
                                  version: null,
                                });
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Version"}
                              labelCSS="mb-0 text-left"
                              labelFormCSS={`labelForm voda-bold`}
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                formData?.financialVersion &&
                                formData?.financialVersion
                                  ?.filter(
                                    (x) =>
                                      x.value ===
                                      capacityForm?.financialversion?.toString()
                                  )
                                  ?.map((res) => ({
                                    key: res.value,
                                    value: res.text,
                                  }))
                              }
                              options={
                                formData?.financialVersion?.map((res) => ({
                                  key: res.value,
                                  value: res.text,
                                })) ?? []
                              }
                              isError={
                                versionError ||
                                (validation &&
                                  validation.response === false &&
                                  validation.property?.includes(
                                    "financialversion"
                                  ))
                                  ? true
                                  : false
                              }
                              error={
                                versionError
                                  ? `${
                                      formData?.financialVersion?.filter(
                                        (x) =>
                                          x.value ===
                                          capacityForm?.financialversion?.toString()
                                      )[0]?.text
                                    } alreday exist for selected year.`
                                  : "Version must have a value."
                              }
                              onChange={(e: any) => {
                                onChangeCapacity("financialversion", e);
                                yearVersionCheck({
                                  version: e,
                                  year: null,
                                });
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="No of CNF Instances Per Site"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.noofcnfinstancespersite}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "noofcnfinstancespersite"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "noofcnfinstancespersite"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "noofcnfinstancespersite"
                                  ? "*Only number are allowed."
                                  : "*NoOfCnfInstancesPerSite must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "noofcnfinstancespersite",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "noofcnfinstancespersite",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Number of Pods per Pod Type"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.numberofpodsperpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "numberofpodsperpodtype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "numberofpodsperpodtype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "numberofpodsperpodtype"
                                  ? "*Only number are allowed."
                                  : "*NumberOfPodsPerPodType must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "numberofpodsperpodtype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "numberofpodsperpodtype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="VCPU Request"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.vcpurequestforpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "vcpurequestforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "vcpurequestforpodtype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "vcpurequestforpodtype"
                                  ? "*Only number are allowed."
                                  : "*VcpuRequestForPodType must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "vcpurequestforpodtype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "vcpurequestforpodtype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="VCPU Limit"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.vcpulimitforpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "vcpulimitforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "vcpulimitforpodtype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "vcpulimitforpodtype"
                                  ? "*Only number are allowed."
                                  : "*VcpuRequestForPodType must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "vcpulimitforpodtype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "vcpurequestforpodtype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="PCPU Limit"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.pcpurequestforpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "pcpurequestforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "pcpurequestforpodtype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "pcpurequestforpodtype"
                                  ? "*Only number are allowed."
                                  : "*VcpuRequestForPodType must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "pcpurequestforpodtype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "pcpurequestforpodtype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Mem Request Pod Type"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.memrequestforpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "memrequestforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "memrequestforpodtype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "memrequestforpodtype"
                                  ? "*Only number and decimal are allowed."
                                  : "*MemRequestForPodType must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                // Allow only numbers and an optional decimal point
                                if (/^\d*\.?\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "memrequestforpodtype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "memrequestforpodtype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Mem Limit Pod Type"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.memlimitforpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "memlimitforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "memlimitforpodtype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "memlimitforpodtype"
                                  ? "*Only number and decimal are allowed."
                                  : "*MemRequestForPodType must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                // Allow only numbers and an optional decimal point
                                if (/^\d*\.?\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "memlimitforpodtype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "memlimitforpodtype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Non-Persistent Storage Pod Type"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={
                                capacityForm?.nonpresistentstorageforprodtype
                              }
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "nonpresistentstorageforprodtype"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*NonPresistentStorageForProdType must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "nonpresistentstorageforprodtype",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Persistent Volumes Required"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) =>
                                        x.key ==
                                        capacityForm?.ispresistentvolumesrequired
                                    )
                                  : null
                              }
                              position={"auto"}
                              options={evumOption}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "ispresistentvolumesrequired"
                                )
                                  ? true
                                  : false
                              }
                              error="*IsPresistentVolumesRequired must have a value."
                              onChange={(e: any) =>
                                onChangeCapacity(
                                  "ispresistentvolumesrequired",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Persistent Volume Access Mode"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.persistentvolumneaccessmode}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "persistentvolumneaccessmode"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*PersistentVolumneAccessMode must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "persistentvolumneaccessmode",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Persistent Storage Pod Type"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.persistentstorageforpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "persistentstorageforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*PersistentStorageForPodType must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "persistentstorageforpodtype",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Storage IOPS Pod Type"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.storageiopsforpodtype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "storageiopsforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "storageiopsforpodtype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "storageiopsforpodtype"
                                  ? "*Only number are allowed."
                                  : "*Storage (GB) per VM- OS disk must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "storageiopsforpodtype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "storageiopsforpodtype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Storage RW Workload Distribution"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.storagerworkloaddistribution}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "storagerworkloaddistribution"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*StorageRWWorkLoadDistribution must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "storagerworkloaddistribution",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="North/South BandWidth PodType"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={
                                capacityForm?.northsouthbandwidthforpodtype
                              }
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "northsouthbandwidthforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*NorthSouthBandwidthForPodType must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "northsouthbandwidthforpodtype",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="East/West BandWidth PodType"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.eastwestbandwidthforpodtype}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "eastwestbandwidthforpodtype"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*EastWestBandwidthForPodType must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "eastwestbandwidthforpodtype",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Special Requirement Per PodType"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.specialrequirementperpodtype}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "specialrequirementperpodtype"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*SpecialRequirementPerPodType must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "specialrequirementperpodtype",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Capacity Special Requirement"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.capacityspecialrequirement}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "capacityspecialrequirement"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*Capacity Special Requirement must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "capacityspecialrequirement",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                      </div>
                    </fieldset>
                  </div>

                  <Divider />
                  <div
                    className="col-12 mx-0"
                    style={{
                      padding: "1rem",
                      paddingLeft: "1rem !important",
                      display: "flex",
                      justifyContent: "right",
                    }}
                  >
                    <Button
                      variant="contained"
                      color={"inherit"}
                      size="small"
                      style={{ borderRadius: "20px", marginRight: "1rem" }}
                      onClick={() => {
                        setCapacityForm(null);
                        setAddCapacityFlag(false);
                        setEditCapacityId(null);
                        setIsAddCapacity(false);
                      }}
                    >
                      Cancel
                    </Button>

                    <Button
                      variant="contained"
                      color={"error"}
                      size="small"
                      style={{ borderRadius: "20px" }}
                      onClick={() => {
                        onSubmitCapacity({
                          edit:
                            editCapacityId !== null &&
                            editCapacityId?.capacityId !== null
                              ? true
                              : false,
                          addInstenceId:
                            formData?._cnfClusterInfoEntity?.cnfpodinfo?.[
                              instanceIndex
                            ]?.cnfpodinfoid ?? instanceIndex,
                        });
                      }}
                      disabled={false}
                    >
                      {editCapacityId !== null &&
                      editCapacityId?.capacityId !== null
                        ? "Update"
                        : "Save"}{" "}
                      Capacity
                    </Button>
                  </div>
                </>
              ) : (
                <div
                  className="col-12"
                  style={{
                    padding: "15px",
                    minHeight: `${
                      isAddCapacity || isAddInstance ? "42rem" : "29rem"
                    }`,
                    maxHeight: "42rem",
                    overflowY: "auto",
                  }}
                >
                  {!isAddInstance || editInterfaceId ? (
                    <CustomMUITable
                      columns={getCapacityColumns}
                      data={
                        formData?._cnfClusterInfoEntity?.cnfpodinfo?.[
                          instanceIndex
                        ]?.cnfcapacity?.map((res) => ({
                          ...res,
                          financialversion:
                            res?.financialversion == "1" ? "H1" : "H2",
                          ispresistentvolumesrequired:
                            res?.ispresistentvolumesrequired === true
                              ? "Yes"
                              : "No",
                          // rxtxcpucount: res?.rxtxcpucount === true ? "Yes" : "No",
                          // backuprequired: res?.backuprequired === true ? "Yes" : "No",
                          // probingrequired:
                          //   res?.probingrequired === true ? "Yes" : "No",
                        })) ?? []
                      }
                      onEdit={(row: any, idx: any) => {
                        setEditCapacityId({
                          instanceId: row.cnfpodinfoid
                            ? row.cnfpodinfoid
                            : editCapacityId?.instanceId,
                          capacityId: row.vnfvmcapacityid ?? idx,
                        });
                        setCapacityForm({
                          ...row,
                          ispresistentvolumesrequired:
                            row?.ispresistentvolumesrequired === "Yes"
                              ? true
                              : false,
                          financialversion:
                            row?.financialversion == "H1" ? 1 : 2,
                          rxtxcpucount: row?.rxtxcpucount
                            ? row?.rxtxcpucount === "Yes"
                              ? true
                              : false
                            : null,
                          backuprequired: row?.backuprequired
                            ? row?.backuprequired === "Yes"
                              ? true
                              : false
                            : null,
                          probingrequired: row?.probingrequired
                            ? row?.probingrequired === "Yes"
                              ? true
                              : false
                            : null,
                        });
                        setAddCapacityFlag(true);
                        setIsAddCapacity(true);
                      }}
                      onDelete={(row, idex) => {
                        onDeleteCapacity({
                          instanceDelecteId: null,
                          instanceIndexId: capacityIndex,
                          capacityDeleteId: null,
                          capacityIndex: idex,
                        });
                      }}
                      tableName="CnfCapacity"
                      size="small"
                    />
                  ) : null}
                </div>
              )}
            </Item>
          </Grid>
        </Grid>
      </Box>
      {isLocationCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsLocationCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <Location
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsLocationCrudModalOpen(false),
              }}
              returnObject={(updatedLocations) => {
                const opcoMap = new Map<number, any>();

                updatedLocations.forEach((loc) => {
                  if (!opcoMap.has(loc.opcoId)) {
                    opcoMap.set(loc.opcoId, {
                      opcoId: loc.opcoId,
                      opcoDescription: loc.opcoDescription,
                      locationDetails: [],
                    });
                  }

                  const entry = opcoMap.get(loc.opcoId);
                  if (
                    entry &&
                    !entry.locationDetails.some(
                      (d: any) => d.value === loc.locationId
                    )
                  ) {
                    entry.locationDetails.push({
                      value: loc.locationId,
                      text: loc.locationDescription,
                    });
                  }
                });

                const formatted = Array.from(opcoMap.values());
                setFormData(
                  (prev) =>
                    ({
                      ...prev,
                      opcoBasedLocationResource: formatted,
                    } as CBOMDto)
                );
              }}
            />
          </DialogContent>
        </Dialog>
      )}

      {isCNFClusterCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsCNFClusterCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <CNFCluster
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsCNFClusterCrudModalOpen(false),
              }}
              // returnObject={}
            />
          </DialogContent>
        </Dialog>
      )}

      {isCNFNameCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsCNFNameCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <CNFName
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () => setIsCNFNameCrudModalOpen(false),
              }}
              // returnObject={}
            />
          </DialogContent>
        </Dialog>
      )}
      {isPodTypeInfoCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsPodTypeInfoCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <PodTypeInfo
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsPodTypeInfoCrudModalOpen(false),
              }}
              returnObject={updateCNFPodTypeValue}
            />
          </DialogContent>
        </Dialog>
      )}
      {isCnfFunctionStandardNameCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsCnfFunctionStandardNameCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <CNFFunctionStandardName
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsCnfFunctionStandardNameCrudModalOpen(false),
              }}
              // returnObject={}
            />
          </DialogContent>
        </Dialog>
      )}
      {isCNFHardwareTypeCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsCNFHardwareTypeCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <CNFHardwareType
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsCNFHardwareTypeCrudModalOpen(false),
              }}
              returnObject={updateCNFHardwareTypeResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}
      {isCNFPriorityCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsCNFPriorityCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <CNFPriority
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsCNFPriorityCrudModalOpen(false),
              }}
              returnObject={updateCNFPriorityResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}
    </div>
  );
});
export default CBOMClusterInfoModal;
