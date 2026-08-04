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
import InterVMType from "../../Containers/Lookup/InterVMTypeContainer";
import IntraVMType from "../../Containers/Lookup/IntraVMTypeContainer";
import VMWorkloadType from "../../Containers/Lookup/VMWorkloadTypeContainer";
import VMTypeName from "../../Containers/Lookup/VMTypeNameContainer";
import VNFClusterName from "../../Containers/Lookup/VNFClusterNameContainer";
import VNFName from "../../Containers/Lookup/VNFNameContainer";
import VNFHardwareType from "../../Containers/Lookup/VNFHardwareTypeContainer";

import CustomizedTables from "../../Components/CustomStaticMUITable";
import { MdAdd, MdGridView } from "react-icons/md";

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

const VBOMClusterInfoModal = forwardRef((props: Props, ref) => {
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
  } = useFormTableCrud<VBOMClusterInfoDtoUpdate>(
    CreateVBOMClusterInfo,
    EditVBOMInfo
  );
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [validationInterfaceForm, setValidationInterfaceForm] = useState<{
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
  const [vnfTypeNameOptions, setVnfTypeNameOptions] = useState<any>();
  const [opcoOptions, setOpcoOptions] = useState<any>();
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [instanceForm, setInstanceForm] = useState<VNFVMClusterInfo | null>(
    null
  );
  const [capacityForm, setCapacityForm] = useState<VNFVMClusterCapacity | null>(
    null
  );
  const [addInterfaceFlag, setAddInterfaceFlag] = useState<boolean>(false);
  const [addCapacityFlag, setAddCapacityFlag] = useState<boolean>(false);
  const [editInterfaceId, setEditInterfaceId] = useState<number | null>(null);
  const [isLocationCrudModalOpen, setIsLocationCrudModalOpen] = useState(false);
  const [isInterVMTypeCrudModalOpen, setIsInterVMTypeCrudModalOpen] =
    useState(false);
  const [isIntraVMTypeCrudModalOpen, setIsIntraVMTypeCrudModalOpen] =
    useState(false);
  const [isVMWorkloadTypeCrudModalOpen, setIsVMWorkloadTypeCrudModalOpen] =
    useState(false);
  const [isVMTypeNameCrudModalOpen, setIsVMTypeNameCrudModalOpen] =
    useState(false);
  const [isCLusterNameCrudModalOpen, setIsCLusterNamCrudModalOpen] =
    useState(false);
  const [isVNFNameCrudModalOpen, setIsVNFNamCrudModalOpen] = useState(false);
  const [isVNFHardwareTypeCrudModalOpen, setIsVNFHardwareTypeCrudModalOpen] =
    useState(false);

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
    state.VBOMClusterInfoEditReducer.VBOMClusterInfoDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.VBOMClusterInfoCreateReducer.VBOMClusterInfoDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  useEffect(() => {
    const mapVmTypeOptions = (data: any) =>
      data?.map((item) => ({
        key: item?.vmtypenameid,
        value: item?.vmtypedescription,
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
      locationId: number | null,
      resources: any[]
    ) =>
      resources
        ?.find((opco) => opco.opcoId == opcoId)
        ?.locationDetails?.find((loc) => loc.value == locationId)?.text ?? "";

    if (props.edit) {
      if (!editResource || !("data" in editResource)) return;

      const {
        data: {
          _VnfClusterInfoDto,
          vnVmTypeNameResource,
          opcoBasedLocationResource,
          ...rest
        },
      } = editResource as { data: VBOMClusterInfoDtoUpdate };
      const { opcoid, locationid } = _VnfClusterInfoDto as VNFClusterInfoDetail;
      const opcoOptions = mapOpcoOptions(opcoBasedLocationResource);

      setVnfTypeNameOptions(mapVmTypeOptions(vnVmTypeNameResource));
      setOpcoOptions(opcoOptions);
      setFormData({
        vnVmTypeNameResource,
        opcoBasedLocationResource,
        ...rest,
        _VnfClusterInfoDto: {
          ..._VnfClusterInfoDto,
          opco: getOpcoText(opcoid, opcoOptions),
          location: getLocationText(
            opcoid,
            locationid,
            opcoBasedLocationResource
          ),
        } as VNFClusterInfoDetail,
      });
      if (props?.capacityKey >= 0 && props?.capacityKey !== null) {
        _VnfClusterInfoDto?.vnfinfo?.map((res, index) => {
          if (res.vnfinfoid === props.instanceKey) {
            setInstanceIndex(index);
          }
        });
        _VnfClusterInfoDto?.vnfinfo
          ?.filter((res) => res.vnfinfoid === props.instanceKey)[0]
          ?.vnfvmcapacity?.map((res, index) => {
            if (res.vnfvmcapacityid === props.capacityKey) {
              setCapacityIndex(index);
              setCapacityForm(res);
              setEditCapacityId({
                instanceId: props.instanceKey,
                capacityId: res.vnfvmcapacityid ?? index,
              });
              setAddCapacityFlag(true);
              setIsAddCapacity(true);
            }
          });
      } else {
        _VnfClusterInfoDto?.vnfinfo?.map((res, index) => {
          if (res.vnfinfoid === props.instanceKey) {
            setInstanceIndex(index);
            setInstanceForm(res);
            setEditInterfaceId(res.vnfinfoid ?? index);
            setAddInterfaceFlag(true);
            setIsAddInstance(true);
          }
        });
      }
    } else if (createResource) {
      const {
        _VnfClusterInfoDto,
        vnVmTypeNameResource,
        opcoBasedLocationResource,
        ...rest
      } = createResource;

      setVnfTypeNameOptions(mapVmTypeOptions(vnVmTypeNameResource));
      setOpcoOptions(mapOpcoOptions(opcoBasedLocationResource));
      setFormData({
        ...createResource,
        _VnfClusterInfoDto: {
          ..._VnfClusterInfoDto,
          vnfinfo: [],
        } as VNFClusterInfoDetail,
      });
      setInstanceIndex(0);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: VBOMClusterInfoDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?._VnfClusterInfoDto?.opcoid === null ||
      copy?._VnfClusterInfoDto?.opcoid === undefined ||
      copy?._VnfClusterInfoDto?.opcoid === 0
    ) {
      addInvalidProperty("opcoid");
    }

    if (
      copy?._VnfClusterInfoDto?.locationid === null ||
      copy?._VnfClusterInfoDto?.locationid === undefined ||
      copy?._VnfClusterInfoDto?.locationid === 0
    ) {
      addInvalidProperty("locationid");
    }
    if (
      copy?._VnfClusterInfoDto?.noofblades === null ||
      copy?._VnfClusterInfoDto?.noofblades === undefined ||
      copy?._VnfClusterInfoDto?.noofblades === ""
    ) {
      addInvalidProperty("noofblades");
    }

    if (
      copy?._VnfClusterInfoDto?.clusternameid === null ||
      copy?._VnfClusterInfoDto?.clusternameid === undefined ||
      copy?._VnfClusterInfoDto?.clusternameid === 0
    ) {
      addInvalidProperty("clusternameid");
    }
    if (
      copy?._VnfClusterInfoDto?.revision === null ||
      copy?._VnfClusterInfoDto?.revision === undefined ||
      copy?._VnfClusterInfoDto?.revision === ""
    ) {
      addInvalidProperty("revision");
    }

    if (
      copy?._VnfClusterInfoDto?.hardwaretypeid === null ||
      copy?._VnfClusterInfoDto?.hardwaretypeid === undefined ||
      copy?._VnfClusterInfoDto?.hardwaretypeid === 0
    ) {
      addInvalidProperty("hardwaretypeid");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const validateInterface = (copy: VNFVMClusterInfo) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.vnfnameid === null ||
      copy?.vnfnameid === undefined ||
      copy?.vnfnameid === 0
    ) {
      addInvalidProperty("vnfnameid");
    }

    if (
      copy?.vnfvmtypenameid === null ||
      copy?.vnfvmtypenameid === undefined ||
      copy?.vnfvmtypenameid === 0 ||
      nameTypeError
    ) {
      addInvalidProperty("vnfvmtypenameid");
    }
    if (copy?.nsxt === null || copy?.nsxt === undefined) {
      addInvalidProperty("nsxt");
    }
    if (
      copy?.vmworkloadtypeid === null ||
      copy?.vmworkloadtypeid === undefined
    ) {
      addInvalidProperty("vmworkloadtypeid");
    }
    if (copy?.intravmtypeid === null || copy?.intravmtypeid === undefined) {
      addInvalidProperty("intravmtypeid");
    }
    if (copy?.intervmtypeid === null || copy?.intervmtypeid === undefined) {
      addInvalidProperty("intervmtypeid");
    }

    if (
      copy?.numa === true &&
      (copy?.socket === undefined ||
        copy?.socket === null ||
        copy?.socket === "")
    ) {
      addInvalidProperty("socket");
    }
    setValidationInterfaceForm(copyValidation);
    return copyValidation;
  };
  const validateCapacity = (copy: VNFVMClusterCapacity) => {
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
      copy?.vnfcpupervm === null ||
      copy?.vnfcpupervm === undefined ||
      !isMultipleOfTwo(copy?.vnfcpupervm)
    ) {
      addInvalidProperty("vnfcpupervm");
    }

    if (
      copy?.rampervm === undefined ||
      copy?.rampervm === null ||
      !isMultipleOfFour(copy?.rampervm)
    ) {
      addInvalidProperty("rampervm");
    }

    if (
      copy?.noofvnfinstances === undefined ||
      copy?.noofvnfinstances === null ||
      copy?.noofvnfinstances === ""
    ) {
      addInvalidProperty("noofvnfinstances");
    }

    if (
      copy?.noofvmspertype === undefined ||
      copy?.noofvmspertype === null ||
      copy?.noofvmspertype === ""
    ) {
      addInvalidProperty("noofvmspertype");
    }

    if (copy?.datadisk === undefined || copy?.datadisk === null) {
      addInvalidProperty("datadisk");
    }

    setValidationCapacityForm(copyValidation);
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
    const { _VnfClusterInfoDto, ...res } = formData as VBOMClusterInfoDtoUpdate;
    let copy = { ...formData } as VBOMClusterInfoDtoUpdate;
    if (_VnfClusterInfoDto) {
      if (property === "hardwaretype") {
        _VnfClusterInfoDto.hardwaretypeid = e && e["key"];

        _VnfClusterInfoDto.hardwaretype = e && e["value"];
      } else if (property === "locationid") {
        _VnfClusterInfoDto.location = e && e["value"];
        _VnfClusterInfoDto.locationid = e && e["key"];
      } else if (property === "opcoid") {
        _VnfClusterInfoDto.opco = e && e["value"];
        _VnfClusterInfoDto.locationid = null;
        _VnfClusterInfoDto.opcoid = e && e["key"];
      } else {
        _VnfClusterInfoDto[property] = e && e["key"];
      }
    }
    setFormData({ ...copy, _VnfClusterInfoDto });
  };

  const onChangeDropdownInterfaceSection = (property: string, e: any) => {
    const { _VnfClusterInfoDto, ...res } = formData as VBOMClusterInfoDtoUpdate;
    const { vnfinfo, ...res1 } = _VnfClusterInfoDto as VNFClusterInfoDetail;
    let copy = { ..._VnfClusterInfoDto } as VNFClusterInfoDetail;
    if (vnfinfo) {
      if (property === "intravmtype" || property === "intervmtype") {
        vnfinfo[0][property] = e && e["value"];
      } else {
        vnfinfo[0][property] = e && e["key"];
      }
    }
    setFormData({
      ...formData,
      _VnfClusterInfoDto: { ...copy, vnfinfo },
    });
  };

  const onChangeInstance = (property: string, e: any) => {
    let copy = { ...instanceForm } as VNFVMClusterInfo;

    if (property === "vnfnameid") {
      copy.vnfvmtypenameid = null;
    }
    if (
      property === "intravmtype" ||
      property === "intervmtype" ||
      property === "vmworkloadtype"
    ) {
      copy[property] = e && e["value"];
      copy[property + "id"] = e && e["key"];
    } else {
      copy[property] = e && e["key"];
    }
    setInstanceForm(copy);
  };

  const onChangeCapacity = (property: string, e: any) => {
    let copy = { ...capacityForm } as VNFVMClusterCapacity;
    copy[property] = e && e["key"];
    setCapacityForm(copy);
  };

  const onChangeTextInfoSection = (property: string, e: any) => {
    const { _VnfClusterInfoDto, ...res } = formData as VBOMClusterInfoDtoUpdate;
    let copy = { ...formData } as VBOMClusterInfoDtoUpdate;
    if (_VnfClusterInfoDto) {
      _VnfClusterInfoDto[property] = e && e.target.value;
    }
    setFormData({ ...copy, _VnfClusterInfoDto });
  };

  const onChangeTextInstanceSection = (property: string, e: any) => {
    let copy = { ...instanceForm } as VNFVMClusterInfo;
    copy[property] = e && e.target.value;
    setInstanceForm(copy);
  };

  const onChangeTextCapacitySection = (property: string, e: any) => {
    let copy = { ...capacityForm } as VNFVMClusterCapacity;
    if (property === "financialyear") {
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
      _VnfClusterInfoDto: {
        vnfinfo = [],
        ...restInfo
      } = {} as VNFClusterInfoDetail,
      ...rest
    } = formData as VBOMClusterInfoDtoUpdate;

    const updatedInstances = (vnfinfo ?? []).map((instance, idx) => {
      const isMatch =
        instance.vnfinfoid === instanceDelecteId || idx === instanceIndexId;
      if (!isMatch) return instance;

      const currentCapacities = instance.vnfvmcapacity ?? [];

      const updatedCapacities = currentCapacities.filter(
        (cap, i) => i !== capacityIndex
      );

      return { ...instance, vnfvmcapacity: updatedCapacities };
    });
    const updatedData: VBOMClusterInfoDtoUpdate = {
      ...rest,
      _VnfClusterInfoDto: {
        ...restInfo,
        vnfinfo: updatedInstances,
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
        _VnfClusterInfoDto: {
          vnfinfo = [],
          ...restInfo
        } = {} as VNFClusterInfoDetail,
        ...rest
      } = formData as VBOMClusterInfoDtoUpdate;
      const updatedInstances = (vnfinfo ?? []).map((instance, idx) => {
        const isMatch = instance.vnfinfoid === instanceId || idx === instanceId;
        if (!isMatch) return instance;
        const currentCapacities = instance.vnfvmcapacity ?? [];
        if (edit) {
          const updatedCapacities = currentCapacities.map((cap, i) =>
            cap.vnfvmcapacityid === capacityId || i === capacityId
              ? capacityForm
              : cap
          );
          return { ...instance, vnfvmcapacity: updatedCapacities };
        } else {
          const newCapacity = {
            ...capacityForm,
            vnfinfoid: 0,
          };
          return {
            ...instance,
            vnfvmcapacity: [...currentCapacities, newCapacity],
          };
        }
      });
      const updatedData: VBOMClusterInfoDtoUpdate = {
        ...rest,
        _VnfClusterInfoDto: {
          ...restInfo,
          vnfinfo: updatedInstances,
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
      validateInterface(instanceForm).response === true
    ) {
      const { _VnfClusterInfoDto = { vnfinfo: [] }, ...rest } =
        formData as VBOMClusterInfoDtoUpdate;
      let instances = [...(_VnfClusterInfoDto.vnfinfo || [])];

      instances = instances?.map((inst, index) => {
        const isMatch =
          inst.vnfinfoid === editInterfaceId || index === editInterfaceId;

        if (edit && isMatch) return instanceForm;

        if (!edit && index === instanceId) return instanceForm;

        return inst;
      });

      // Add new instance if not matched
      if (!edit && (!instanceId || instanceId >= instances.length)) {
        instances.push({ ...instanceForm, vnfvmcapacity: [] });
      }

      const updatedData: VBOMClusterInfoDtoUpdate = {
        ...rest,
        _VnfClusterInfoDto: {
          ..._VnfClusterInfoDto,
          filename: "Manual",
          vnfinfo: instances.map((res) => ({
            ...res,
            numa: res.numa === "Yes" || res.numa === true,
          })),
        } as VNFClusterInfoDetail,
      };
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
    const { _VnfClusterInfoDto = { vnfinfo: [] }, ...rest } =
      formData as VBOMClusterInfoDtoUpdate;
    let instances = [...(_VnfClusterInfoDto.vnfinfo || [])];

    instances = instances?.filter((inst, index) => index !== instanceIndex);

    const updatedData: VBOMClusterInfoDtoUpdate = {
      ...rest,
      _VnfClusterInfoDto: {
        ..._VnfClusterInfoDto,
        vnfinfo: instances,
      } as VNFClusterInfoDetail,
    };

    setFormData(updatedData);
  };

  useImperativeHandle(ref, () => ({
    onSaveFormData,
  }));
  const onSaveFormData = () => {
    if (!formData) return;

    const { _VnfClusterInfoDto, ...rest } =
      formData as VBOMClusterInfoDtoUpdate;

    const updatedInstances =
      _VnfClusterInfoDto?.vnfinfo?.map((res) => ({
        ...res,
        vnfinfoid: res.vnfinfoid ?? 0,
        numa: res.numa === "Yes" || res.numa === true,
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

      const instances = (obj as VNFClusterInfoDetail).vnfinfo;
      if (!Array.isArray(instances) || instances.length === 0) {
        return {
          pass: false,
          message: "Please add at least one Instance.",
        };
      }

      const invalidInstanceIndex = instances.findIndex(
        (inst) =>
          !Array.isArray(inst.vnfvmcapacity) || inst.vnfvmcapacity.length === 0
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
      _VnfClusterInfoDto ? _VnfClusterInfoDto : {}
    );
    if (validation?.pass) {
      Save(
        {
          ...rest,
          _VnfClusterInfoDto: {
            ..._VnfClusterInfoDto,
            filename: "Manual",
            vnfinfo: updatedInstances,
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

  const getInterfaceColumns: Column<VNFVMClusterInfo>[] = [
    { key: "vnfname", label: "Vnf Name" },
    { key: "vnfvmtypename", label: "VNF VMType Name" },
    { key: "nsxt", label: "NSXT" },
    { key: "intravmtype", label: "Intra Vm Type" },
    { key: "intervmtype", label: "Inter Vm Type" },
    { key: "vmworkloadtype", label: "VM Workload Type" },
    { key: "vmstorageblocksize", label: "VM Storage Block Size" },
    { key: "numa", label: "Numa" },
    { key: "socket", label: "Socket" },
  ];

  const getCapacityColumns: Column<VNFVMClusterCapacity>[] = [
    {
      key: "financialyear",
      label: LabelsDictionary["financialYear"]?.Full ?? "Financial Year",
    },
    {
      key: "financialversion",
      label: LabelsDictionary["financialversion"]?.Full ?? "Version",
    },
    {
      key: "vnfcpupervm",
      label: LabelsDictionary["vcpuPerVm"]?.Full ?? "No Of VCPU Per VM",
    },
    {
      key: "rxtxcpucount",
      label: LabelsDictionary["rxTxCpuCount"]?.Full ?? "RX/TX CPU INCLUDED",
    },
    {
      key: "rampervm",
      label:
        LabelsDictionary["ramPerVm"]?.Full ?? "RAM (GB) Per VM - Data Disk",
    },
    { key: "noofvnfinstances", label: "No of VNF Instances" },
    { key: "noofvmspertype", label: "No of VMS Per Type" },
    {
      key: "datadisk",
      label:
        LabelsDictionary["dataDisk"]?.Full ?? "Storage (GB) per VM - Data Disk",
    },
    {
      key: "osdisk",
      label:
        LabelsDictionary["osDisk"]?.Full ?? "Storage (GB) per VM - OS disk",
    },
    {
      key: "iopsrunning",
      label: LabelsDictionary["iopsRunning"]?.Full ?? "IOPS per VM Running",
    },
    {
      key: "iopsloading",
      label: LabelsDictionary["iopsLoading"]?.Full ?? "IOPS per VM Loading",
    },
    {
      key: "vmworkloaddistribution",
      label:
        LabelsDictionary["vmWorkLoadDistribution"]?.Full ??
        "Read and Write VM workload distribution",
    },
    {
      key: "northsouthboundbandwidth",
      label:
        LabelsDictionary["northDouthBoundBandWidth"]?.Full ??
        "North and South bandwidth per VM (Mbits)",
    },
    {
      key: "eastwestboundbandwidth",
      label:
        LabelsDictionary["eastWestBoundBandWidth"]?.Full ??
        "East and West bandwidth per VM (Mbits)",
    },
    {
      key: "otherrequirements",
      label:
        LabelsDictionary["otherRequirements"]?.Full ??
        "Onboarding Date or Other requirements",
    },
    {
      key: "backuprequired",
      label: LabelsDictionary["backupRequired"]?.Full ?? "Backup Required",
    },
    {
      key: "probingrequired",
      label: LabelsDictionary["probIngRequired"]?.Full ?? "Probing Required",
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
    //       message: "Please check the fields entered in VNF Info Section.",
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
      formData?._VnfClusterInfoDto?.vnfinfo
        ?.filter((res) => res.vnfinfoid === props.instanceKey)[0]
        ?.vnfvmcapacity?.map((val, index) => ({
          year: safeNumber(val.financialyear),
          version: safeNumber(val.financialversion),
          index: val.vnfvmcapacityid,
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

  const vnfNameTypeCheck = ({ name, type }: { name: any; type: any }) => {
    setNameTypeError(false);
    const obj1 =
      formData?._VnfClusterInfoDto?.vnfinfo?.map((val, index) => ({
        name: safeNumber(val.vnfnameid),
        type: safeNumber(val.vnfvmtypenameid),
        index: val?.vnfinfoid,
      })) || [];

    const obj2 = {
      name: name ? safeNumber(name?.key) : safeNumber(instanceForm?.vnfnameid),
      type: type
        ? safeNumber(type?.key)
        : safeNumber(instanceForm?.vnfvmtypenameid),
      index: isAddInstance && editInterfaceId === null ? -1 : editInterfaceId,
    };
    function hasDuplicateConflict(arr, obj) {
      return arr.some((item) => {
        if (item.index === obj.index) return false;
        return item.name === obj.name && item.type === obj.type;
      });
    }
    const conflict = hasDuplicateConflict(obj1, obj2);
    if (conflict) {
      setNameTypeError(true);
    } else {
      setNameTypeError(false);
    }
  };

  const updateInterVMTypeResourceValue = (value: any) => {
    setFormData({
      ...formData,
      interTypeResource: value?.map((res) => ({
        text: res?.interDescription,
        value: String(res?.interVmTypeId),
      })),
    });
  };
  const updateIntraVMTypeResourceValue = (value: any) => {
    setFormData({
      ...formData,

      intraVmTypeResource: value?.map((res) => ({
        text: res?.intraDescription,
        value: String(res?.intraVmTypeId),
      })),
    });
  };
  const updateVMWorkloadTypeResourceValue = (value: any) => {
    setFormData({
      ...formData,

      vmWorkLoadTypeDetail: value?.map((res) => ({
        text: res?.description,
        value: String(res?.vmWorkLoadTypeId),
      })),
    });
  };
  const updateVNFClusterNameResourceValue = (value: any) => {
    setFormData({
      ...formData,

      vnClusterNameResource: value?.map((res) => ({
        text: res?.clusterDescription,
        value: String(res?.clusterNameId),
      })),
    });
  };
  const updateVNFHardwareTypeResourceValue = (value: any) => {
    setFormData({
      ...formData,
      hardwareTypeResource: value?.map((res) => ({
        text: res?.description,
        value: String(res?.vnfHardwareId),
      })),
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
                  VNF Cluster Info Details
                </h5>
              </div>
              <Divider />
              <div className="col-12" style={{ padding: "15px" }}>
                <fieldset className="fieldset p-0">
                  <div className="row">
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
                                x.key == formData?._VnfClusterInfoDto?.opcoid
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
                      <div className="col-12 px-0 d-flex align-items-end">
                        <div className="flex-grow-1 pr-2">
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
                                  formData?._VnfClusterInfoDto?.opcoid ===
                                  val.opcoId
                              )[0]
                              ?.locationDetails?.map((res) => ({
                                key: res.value,
                                value: res.text,
                              }))
                              .filter(
                                (x) =>
                                  x.key ==
                                  formData?._VnfClusterInfoDto?.locationid
                              )}
                            options={
                              formData?._VnfClusterInfoDto?.opcoid !== 0
                                ? formData?.opcoBasedLocationResource
                                    ?.filter(
                                      (val) =>
                                        formData?._VnfClusterInfoDto?.opcoid ===
                                        val.opcoId
                                    )[0]
                                    ?.locationDetails?.map((res) => ({
                                      key: res.value,
                                      value: res.text,
                                    }))
                                : null
                            }
                            isError={
                              validation &&
                              validation.response === false &&
                              validation.property?.includes("locationid")
                                ? true
                                : false
                            }
                            error="*Site must have a value."
                            onChange={(e: any) =>
                              onChangeDropdownInfoSection("locationid", e)
                            }
                          />
                        </div>
                        <div className="pb-4">
                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link"
                              onClick={() => setIsLocationCrudModalOpen(true)}
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
                      <div className="col-12 px-0 d-flex align-items-end">
                        <div className="flex-grow-1 pr-2">
                          {" "}
                          <DropdownInputComponent
                            label={"Cluster Name"}
                            labelCSS="mb-0 text-left"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={true}
                            value={
                              formData?.vnClusterNameResource &&
                              resourceArrayRefactor(
                                formData?.vnClusterNameResource
                              ).filter(
                                (x) =>
                                  x.key ==
                                  formData?._VnfClusterInfoDto?.clusternameid
                              )
                            }
                            options={
                              formData?.vnClusterNameResource &&
                              resourceArrayRefactor(
                                formData?.vnClusterNameResource
                              )
                            }
                            isError={
                              validation &&
                              validation.response === false &&
                              validation.property?.includes("clusternameid")
                                ? true
                                : false
                            }
                            error="Cluster Name must have a value."
                            onChange={(e: any) =>
                              onChangeDropdownInfoSection("clusternameid", e)
                            }
                          />
                        </div>
                        <div className="pb-4">
                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link"
                              onClick={() => setIsCLusterNamCrudModalOpen(true)}
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
                          label="No of Blades"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={
                            formData?._VnfClusterInfoDto?.noofblades ?? null
                          }
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("noofblades")
                              ? true
                              : false
                          }
                          validationError={
                            inputError?.field === "noofblades" ? true : false
                          }
                          error={
                            inputError?.field === "noofblades"
                              ? "*Only number are allowed."
                              : "*No of Blades must have a value."
                          }
                          onChange={(e: any) => {
                            const value = e.target.value;
                            if (/^\d*$/.test(value)) {
                              onChangeTextInfoSection("noofblades", e);
                              setInputError(null);
                            } else {
                              setInputError({
                                field: "noofblades",
                                type: "number",
                              });
                            }
                          }}
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
                              formData?.hardwareTypeResource &&
                              resourceArrayRefactor(
                                formData?.hardwareTypeResource
                              ).filter(
                                (x) =>
                                  x.key ===
                                  formData?._VnfClusterInfoDto?.hardwaretypeid
                              )
                            }
                            options={
                              formData?.hardwareTypeResource &&
                              resourceArrayRefactor(
                                formData?.hardwareTypeResource
                              )
                            }
                            isError={
                              validation &&
                              validation.response === false &&
                              validation.property?.includes("hardwaretypeid")
                                ? true
                                : false
                            }
                            error="Hardware Type must have a value."
                            onChange={(e: any) =>
                              onChangeDropdownInfoSection("hardwaretypeid", e)
                            }
                          />
                        </div>
                        <div className="pb-4">
                          {tipologicaPermesso && (
                            <button
                              className="btn btn-link"
                              onClick={() =>
                                setIsVNFHardwareTypeCrudModalOpen(true)
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
                          label="Revision"
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          value={formData?._VnfClusterInfoDto?.revision}
                          required={true}
                          isError={
                            validation &&
                            validation.response === false &&
                            validation.property?.includes("revision")
                              ? true
                              : false
                          }
                          validationError={
                            inputError?.field === "revision" ? true : false
                          }
                          error={
                            inputError?.field === "revision"
                              ? "*Only number are allowed."
                              : "*Revision must have a value."
                          }
                          onChange={(e: any) => {
                            const value = e.target.value;
                            if (/^\d*$/.test(value)) {
                              onChangeTextInfoSection("revision", e);
                              setInputError(null);
                            } else {
                              setInputError({
                                field: "revision",
                                type: "number",
                              });
                            }
                          }}
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
                  VNF Instance Details
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
                                label={"VNF Name"}
                                labelCSS="mb-0 text-left"
                                inputCSS="labelForm voda-bold mb-2"
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.vnfNameResources &&
                                  resourceArrayRefactor(
                                    formData?.vnfNameResources
                                  ).filter(
                                    (x) => x.key == instanceForm?.vnfnameid
                                  )
                                }
                                options={
                                  formData?.vnfNameResources &&
                                  resourceArrayRefactor(
                                    formData?.vnfNameResources
                                  )
                                }
                                isError={
                                  validationInterfaceForm &&
                                  validationInterfaceForm.response === false &&
                                  validationInterfaceForm.property?.includes(
                                    "vnfnameid"
                                  )
                                    ? true
                                    : false
                                }
                                error="VNF Name must have a value."
                                onChange={(e: any) => {
                                  onChangeInstance("vnfnameid", e);
                                  vnfNameTypeCheck({
                                    name: e,
                                    type: null,
                                  });
                                }}
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() => setIsVNFNamCrudModalOpen(true)}
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
                                label={"VNF VMType Name"}
                                labelCSS="mb-0 text-left"
                                labelFormCSS={`labelForm voda-bold ${
                                  nameTypeError ? "pb-4" : ""
                                }`}
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.vnVmTypeNameResource
                                    ?.filter(
                                      (x: any) =>
                                        x.vnfnameid ===
                                          instanceForm?.vnfnameid &&
                                        x.vmtypenameid ===
                                          instanceForm?.vnfvmtypenameid
                                    )
                                    ?.map((res: any) => {
                                      return {
                                        key: res.vmtypenameid,
                                        value: res.vmtypedescription,
                                      };
                                    }) ?? null
                                }
                                options={
                                  formData?.vnVmTypeNameResource
                                    ?.filter(
                                      (x: any) =>
                                        x.vnfnameid === instanceForm?.vnfnameid
                                    )
                                    ?.map((res: any) => {
                                      return {
                                        key: res.vmtypenameid,
                                        value: res.vmtypedescription,
                                      };
                                    }) ?? []
                                }
                                isError={
                                  nameTypeError ||
                                  (validationInterfaceForm &&
                                    validationInterfaceForm.response ===
                                      false &&
                                    validationInterfaceForm.property?.includes(
                                      "vnfvmtypenameid"
                                    ))
                                    ? true
                                    : false
                                }
                                error={
                                  nameTypeError
                                    ? `VMType is already existing for the selected VNF Name.`
                                    : "VNF VMType Name must have a value."
                                }
                                onChange={(e: any) => {
                                  onChangeInstance("vnfvmtypenameid", e);
                                  vnfNameTypeCheck({
                                    name: null,
                                    type: e,
                                  });
                                }}
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() =>
                                    setIsVMTypeNameCrudModalOpen(true)
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
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"NSX-T"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={true}
                              value={
                                evumOption &&
                                evumOption.filter(
                                  (x) => x.key == instanceForm?.nsxt
                                )
                              }
                              options={evumOption}
                              isError={
                                validationInterfaceForm &&
                                validationInterfaceForm.response === false &&
                                validationInterfaceForm.property?.includes(
                                  "nsxt"
                                )
                                  ? true
                                  : false
                              }
                              error="NSX-T must have a value."
                              onChange={(e: any) => onChangeInstance("nsxt", e)}
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0 d-flex align-items-end">
                            <div className="flex-grow-1 pr-2">
                              <DropdownInputComponent
                                label={"INTRA VM Type"}
                                labelCSS="mb-0 text-left"
                                inputCSS="labelForm voda-bold mb-2"
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.intraVmTypeResource &&
                                  formData?.intraVmTypeResource
                                    ?.filter(
                                      (x) =>
                                        x.value ==
                                        instanceForm?.intravmtypeid?.toString()
                                    )
                                    ?.map((res) => ({
                                      key: res.value,
                                      value: res.text,
                                    }))
                                }
                                options={
                                  formData?.intraVmTypeResource?.map((res) => ({
                                    key: res.value,
                                    value: res.text,
                                  })) ?? []
                                }
                                isError={
                                  validationInterfaceForm &&
                                  validationInterfaceForm.response === false &&
                                  validationInterfaceForm.property?.includes(
                                    "intravmtypeid"
                                  )
                                    ? true
                                    : false
                                }
                                error="Intra VMType must have a value."
                                onChange={(e: any) =>
                                  onChangeInstance("intravmtype", e)
                                }
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() =>
                                    setIsIntraVMTypeCrudModalOpen(true)
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
                                label={"INTER VM Type"}
                                labelCSS="mb-0 text-left"
                                inputCSS="labelForm voda-bold mb-2"
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.interTypeResource &&
                                  formData?.interTypeResource
                                    ?.filter(
                                      (x) =>
                                        x.value ===
                                        instanceForm?.intervmtypeid?.toString()
                                    )
                                    ?.map((res) => ({
                                      key: res.value,
                                      value: res.text,
                                    }))
                                }
                                options={
                                  formData?.interTypeResource?.map((res) => ({
                                    key: res.value,
                                    value: res.text,
                                  })) ?? []
                                }
                                isError={
                                  validationInterfaceForm &&
                                  validationInterfaceForm.response === false &&
                                  validationInterfaceForm.property?.includes(
                                    "intervmtypeid"
                                  )
                                    ? true
                                    : false
                                }
                                error="Inter VMType must have a value."
                                onChange={(e: any) =>
                                  onChangeInstance("intervmtype", e)
                                }
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() =>
                                    setIsInterVMTypeCrudModalOpen(true)
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
                                label={"VM Workload Type"}
                                labelCSS="mb-0 text-left"
                                inputCSS="labelForm voda-bold mb-2"
                                isSearchable={true}
                                isClearable={true}
                                required={true}
                                value={
                                  formData?.vmWorkLoadTypeDetail &&
                                  formData?.vmWorkLoadTypeDetail
                                    ?.filter(
                                      (x) =>
                                        x.value ===
                                        instanceForm?.vmworkloadtypeid?.toString()
                                    )
                                    ?.map((res) => ({
                                      key: res.value,
                                      value: res.text,
                                    }))
                                }
                                options={
                                  formData?.vmWorkLoadTypeDetail?.map(
                                    (res) => ({
                                      key: res.value,
                                      value: res.text,
                                    })
                                  ) ?? []
                                }
                                isError={
                                  validationInterfaceForm &&
                                  validationInterfaceForm.response === false &&
                                  validationInterfaceForm.property?.includes(
                                    "vmworkloadtypeid"
                                  )
                                    ? true
                                    : false
                                }
                                error="Inter VMType must have a value."
                                onChange={(e: any) =>
                                  onChangeInstance("vmworkloadtype", e)
                                }
                              />
                            </div>
                            <div className="pb-4">
                              {tipologicaPermesso && (
                                <button
                                  className="btn btn-link"
                                  onClick={() =>
                                    setIsVMWorkloadTypeCrudModalOpen(true)
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
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Storage Block Size"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={instanceForm?.vmstorageblocksize}
                              required={false}
                              isError={
                                validationInterfaceForm &&
                                validationInterfaceForm.response === false &&
                                validationInterfaceForm.property?.includes(
                                  "vmstorageblocksize"
                                )
                                  ? true
                                  : false
                              }
                              error={"*Storage Block Size must have a value."}
                              onChange={(e: any) =>
                                onChangeTextInstanceSection(
                                  "vmstorageblocksize",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Numa"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={false}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) =>
                                        x.key == instanceForm?.numa ||
                                        x.value == instanceForm?.numa
                                    )
                                  : null
                              }
                              options={evumOption}
                              isError={
                                validationInterfaceForm &&
                                validationInterfaceForm.response === false &&
                                validationInterfaceForm.property?.includes(
                                  "numa"
                                )
                                  ? true
                                  : false
                              }
                              error="*Numa must have a value."
                              onChange={(e: any) => onChangeInstance("numa", e)}
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Enter Socket"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={instanceForm?.socket ?? ""}
                              required={
                                instanceForm?.numa === true ? true : false
                              }
                              isError={
                                validationInterfaceForm &&
                                validationInterfaceForm.response === false &&
                                validationInterfaceForm.property?.includes(
                                  "socket"
                                )
                                  ? true
                                  : false
                              }
                              error={"*socket must have a value."}
                              onChange={(e: any) =>
                                onChangeTextInstanceSection("socket", e)
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
                      formData?._VnfClusterInfoDto?.vnfinfo.map((res) => ({
                        ...res,
                        numa: res?.numa === true ? "Yes" : "No",
                        nsxt: res?.nsxt === true ? "Yes" : "No",
                        vnfvmtypename: formData?.vnVmTypeNameResource?.filter(
                          (val: any) => val.vmtypenameid == res.vnfvmtypenameid
                        )[0]?.["vmtypedescription"],
                        vnfname: formData?.vnfNameResources?.filter(
                          (val: any) => val.key == res.vnfnameid
                        )[0]?.text,
                        vmworkloadtype: formData?.vmWorkLoadTypeDetail?.filter(
                          (val: any) => val.value == res.vmworkloadtypeid
                        )[0]?.text,
                        socket: res?.socket,
                        intervmtype: formData?.interTypeResource?.filter(
                          (val: any) => val.value == res.intervmtypeid
                        )[0]?.text,
                        intravmtype: formData?.intraVmTypeResource?.filter(
                          (val: any) => val.value == res.intravmtypeid
                        )[0]?.text,
                      })) ?? []
                    }
                    activeIndex={instanceIndex}
                    onEdit={(row: any, index: any) => {
                      setInstanceForm({
                        ...row,
                        numa: row.numa
                          ? row.numa === "Yes"
                            ? true
                            : false
                          : null,
                        nsxt: row.nsxt
                          ? row.nsxt === "Yes"
                            ? true
                            : false
                          : null,
                      });
                      setEditInterfaceId(row?.vnfinfoid ?? index);
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
                    tableName="VnfInstance"
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
                  VNF Capacity Details
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
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="No of vCPU per VM"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.vnfcpupervm}
                              required={true}
                              isError={
                                (validationCapacityForm &&
                                  validationCapacityForm.response === false &&
                                  validationCapacityForm.property?.includes(
                                    "vnfcpupervm"
                                  )) ||
                                (capacityForm?.vnfcpupervm &&
                                  !isMultipleOfTwo(capacityForm?.vnfcpupervm))
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "vnfcpupervm"
                                  ? true
                                  : false
                              }
                              error={
                                capacityForm?.vnfcpupervm &&
                                !isMultipleOfTwo(capacityForm?.vnfcpupervm)
                                  ? "*Value must be a multiple of 2 (e.g. 2, 4, 8, 16, ...)."
                                  : inputError?.field === "vnfcpupervm"
                                  ? "*Only number are allowed."
                                  : "*No of vCPU per VM must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection("vnfcpupervm", e);
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "vnfcpupervm",
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
                              label="No of VNF Instances"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.noofvnfinstances}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "noofvnfinstances"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "noofvnfinstances"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "noofvnfinstances"
                                  ? "*Only number are allowed."
                                  : "*No of VNF Instances must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "noofvnfinstances",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "noofvnfinstances",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="No of VMS Per Type"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.noofvmspertype}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "noofvmspertype"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "noofvmspertype"
                                  ? true
                                  : false
                              }
                              error={
                                inputError?.field === "noofvmspertype"
                                  ? "*Only number are allowed."
                                  : "*No of VMS Per Type must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection(
                                    "noofvmspertype",
                                    e
                                  );
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "noofvmspertype",
                                    type: "number",
                                  });
                                }
                              }}
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"RX/TX' CPU INCLUDED IN vCPU Count"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={false}
                              position={"auto"}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) => x.key == capacityForm?.rxtxcpucount
                                    )
                                  : null
                              }
                              options={evumOption}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "rxtxcpucount"
                                )
                                  ? true
                                  : false
                              }
                              error="RX/TX' CPU INCLUDED IN vCPU Count must have a value."
                              onChange={(e: any) =>
                                onChangeCapacity("rxtxcpucount", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="RAM (GB) per VM"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.rampervm}
                              required={true}
                              isError={
                                (validationCapacityForm &&
                                  validationCapacityForm.response === false &&
                                  validationCapacityForm.property?.includes(
                                    "rampervm"
                                  )) ||
                                (capacityForm?.rampervm &&
                                  !isMultipleOfFour(capacityForm?.rampervm))
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "rampervm" ? true : false
                              }
                              error={
                                capacityForm?.rampervm &&
                                !isMultipleOfFour(capacityForm?.rampervm)
                                  ? "*Value must be a multiple of 4 (e.g. 4, 8, 12, ...)."
                                  : inputError?.field === "rampervm"
                                  ? "*Only number are allowed."
                                  : "*RAM (GB) per VM must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection("rampervm", e);
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "rampervm",
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
                              label="Storage (GB) per VM- Data Disk"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.datadisk}
                              required={true}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "datadisk"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "datadisk" ? true : false
                              }
                              error={
                                inputError?.field === "datadisk"
                                  ? "*Only number and decimal are allowed."
                                  : "*Storage (GB) per VM- Data Disk must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                // Allow only numbers and an optional decimal point
                                if (/^\d*\.?\d*$/.test(value)) {
                                  onChangeTextCapacitySection("datadisk", e);
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "datadisk",
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
                              label="Storage (GB) per VM- OS disk"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.osdisk}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "osdisk"
                                )
                                  ? true
                                  : false
                              }
                              validationError={
                                inputError?.field === "osdisk" ? true : false
                              }
                              error={
                                inputError?.field === "osdisk"
                                  ? "*Only number are allowed."
                                  : "*Storage (GB) per VM- OS disk must have a value."
                              }
                              onChange={(e: any) => {
                                const value = e.target.value;
                                if (/^\d*$/.test(value)) {
                                  onChangeTextCapacitySection("osdisk", e);
                                  setInputError(null);
                                } else {
                                  setInputError({
                                    field: "osdisk",
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
                              label="Storage IOPS per VM - Running"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.iopsrunning}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "iopsrunning"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*Storage IOPS per VM - Running must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection("iopsrunning", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Storage IOPS per VM - Loading"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.iopsloading}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "iopsloading"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*Storage IOPS per VM - Loading must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection("iopsloading", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Storage Read/Write VM workload distribution (%/%)"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.vmworkloaddistribution}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "vmworkloaddistribution"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*Storage Read/Write VM workload distribution (%/%) must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "vmworkloaddistribution",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="North/South bandwidth requirement per VM (Mbit/s)"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.northsouthboundbandwidth}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "northsouthboundbandwidth"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*North/South bandwidth requirement per VM (Mbit/s) must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "northsouthboundbandwidth",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="East/West bandwidth requirement per VM (Mbit/s)"
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.eastwestboundbandwidth}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "eastwestboundbandwidth"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*East/West bandwidth requirement per VM (Mbit/s) must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "eastwestboundbandwidth",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6">
                          <div className="col-12 px-0">
                            <TextInputComponent
                              label="Onboarding Date / Other requirements?      "
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              value={capacityForm?.otherrequirements}
                              required={false}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "otherrequirements"
                                )
                                  ? true
                                  : false
                              }
                              error={
                                "*Onboarding Date / Other requirements must have a value."
                              }
                              onChange={(e: any) =>
                                onChangeTextCapacitySection(
                                  "otherrequirements",
                                  e
                                )
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Backup Required"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={false}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) =>
                                        x.key == capacityForm?.backuprequired
                                    )
                                  : null
                              }
                              position={"auto"}
                              options={evumOption}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "backuprequired"
                                )
                                  ? true
                                  : false
                              }
                              error="*Backup Required must have a value."
                              onChange={(e: any) =>
                                onChangeCapacity("backuprequired", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="col-6 ">
                          <div className="col-12 px-0">
                            <DropdownInputComponent
                              label={"Probing Required?"}
                              labelCSS="mb-0 text-left"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={false}
                              value={
                                evumOption
                                  ? evumOption.filter(
                                      (x) =>
                                        x.key == capacityForm?.probingrequired
                                    )
                                  : null
                              }
                              options={evumOption}
                              position={"auto"}
                              isError={
                                validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "probingrequired"
                                )
                                  ? true
                                  : false
                              }
                              error="*Probing Required must have a value."
                              onChange={(e: any) =>
                                onChangeCapacity("probingrequired", e)
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
                            formData?._VnfClusterInfoDto?.vnfinfo?.[
                              instanceIndex
                            ]?.vnfinfoid ?? instanceIndex,
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
                        formData?._VnfClusterInfoDto?.vnfinfo[
                          instanceIndex
                        ]?.vnfvmcapacity?.map((res) => ({
                          ...res,
                          financialversion:
                            res?.financialversion == "1" ? "H1" : "H2",
                          rxtxcpucount:
                            res?.rxtxcpucount === true ? "Yes" : "No",
                          noofvmspertype: res?.noofvmspertype,
                          noofvnfinstances: res?.noofvnfinstances,
                          backuprequired:
                            res?.backuprequired === true ? "Yes" : "No",
                          probingrequired:
                            res?.probingrequired === true ? "Yes" : "No",
                        })) ?? []
                      }
                      onEdit={(row: any, idx: any) => {
                        setEditCapacityId({
                          instanceId: row.vnfinfoid
                            ? row.vnfinfoid
                            : editCapacityId?.instanceId,
                          capacityId: row.vnfvmcapacityid ?? idx,
                        });
                        setCapacityForm({
                          ...row,
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
                      tableName="VnfCapacity"
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
                setFormData((prev) => ({
                  ...prev,
                  opcoBasedLocationResource: formatted,
                }));
              }}
            />
          </DialogContent>
        </Dialog>
      )}

      {isInterVMTypeCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsInterVMTypeCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <InterVMType
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsInterVMTypeCrudModalOpen(false),
              }}
              returnObject={updateInterVMTypeResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}
      {isIntraVMTypeCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsIntraVMTypeCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <IntraVMType
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsIntraVMTypeCrudModalOpen(false),
              }}
              returnObject={updateIntraVMTypeResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}
      {isVMWorkloadTypeCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsVMWorkloadTypeCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <VMWorkloadType
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsVMWorkloadTypeCrudModalOpen(false),
              }}
              returnObject={updateVMWorkloadTypeResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}

      {isVNFNameCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsVNFNamCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <VNFName
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () => setIsVNFNamCrudModalOpen(false),
              }}
              // returnObject={updateVNFClusterNameResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}

      {isCLusterNameCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsCLusterNamCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <VNFClusterName
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsCLusterNamCrudModalOpen(false),
              }}
              returnObject={updateVNFClusterNameResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}
      {isVMTypeNameCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsVMTypeNameCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <VMTypeName
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsVMTypeNameCrudModalOpen(false),
              }}
              // returnObject={updateVMWorkloadTypeResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}
      {isVNFHardwareTypeCrudModalOpen && (
        <Dialog
          open={true}
          onClose={() => setIsVNFHardwareTypeCrudModalOpen(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogContent>
            <VNFHardwareType
              modal={{
                isModal: true,
                setIsVisibleModalLookup: () =>
                  setIsVNFHardwareTypeCrudModalOpen(false),
              }}
              returnObject={updateVNFHardwareTypeResourceValue}
            />
          </DialogContent>
        </Dialog>
      )}
    </div>
  );
});
export default VBOMClusterInfoModal;
