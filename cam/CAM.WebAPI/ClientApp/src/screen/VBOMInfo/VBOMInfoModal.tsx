import React, { useCallback, useEffect, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { formatDateWithTime } from "../../Hook/Common";
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
import { Card, DialogActions, Paper, Typography } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";
import {
  VBOMInfoDtoCreate,
  VBOMInfoDtoGrid,
  VBOMInfoDtoUpdate,
  VNFInfoDetail,
  VNFVMCapacity,
  VNFVMInstances,
} from "../../Model/VBOMInfo";
import { CreateVBOMInfo } from "../../Redux/Action/VBOMInfo/VBOMInfoCreateAction";
import { EditVBOMInfo } from "../../Redux/Action/VBOMInfo/VBOMInfoEditAction";
import CustomAccordion from "../../Components/CustomAccordion";
import CustomCollapsibleTable, {
  Column,
} from "../../Components/CustomCollapsibleTable";
import CustomMUITable from "../../Components/CustomMUITable";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import Location from "../../Containers/Lookup/LocationContainer";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const defaultInstances: VNFVMInstances = {
  opcoid: null,
  locationid: null,
  opco: "",
  location: "",
  noofvnfinstances: null,
  noofvmspertype: null,
  numa: null,
  socket: "",
  hardwaretypeid: null,
  vnfvmcapacity: [],
};

const VBOMInfoModal: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<VBOMInfoDtoUpdate>(CreateVBOMInfo, EditVBOMInfo);
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
  const [instanceForm, setInstanceForm] = useState<VNFVMInstances | null>(null);
  const [capacityForm, setCapacityForm] = useState<VNFVMCapacity | null>(null);
  const [addInterfaceFlag, setAddInterfaceFlag] = useState<boolean>(false);
  const [addCapacityFlag, setAddCapacityFlag] = useState<boolean>(false);
  const [editInterfaceId, setEditInterfaceId] = useState<number | null>(null);
  const [isLocationCrudModalOpen, setIsLocationCrudModalOpen] = useState(false);

  const [inputError, setInputError] = useState<{
    field: string;
    type: string;
  } | null>(null);
  const [editCapacityId, setEditCapacityId] = useState<{
    instanceId: number | null;
    capacityId: number | null;
  } | null>({
    instanceId: null,
    capacityId: null,
  });
  const dtoEditResourceState = (state: RootState) =>
    state.VBOMInfoEditReducer.VBOMInfoDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.VBOMInfoCreateReducer.VBOMInfoDtoCreate;
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
          vnfinfoDetail,
          vnVmTypeNameResource,
          opcoBasedLocationResource,
          ...rest
        },
      } = editResource as { data: VBOMInfoDtoUpdate };

      const opcoOptions = mapOpcoOptions(opcoBasedLocationResource);
      const vnfInfoData = vnfinfoDetail?.vnfvminstances?.map((res) => ({
        ...res,
        opco: getOpcoText(res?.opcoid, opcoOptions),
        location: getLocationText(
          res?.opcoid,
          res?.locationid,
          opcoBasedLocationResource
        ),
      }));

      setVnfTypeNameOptions(mapVmTypeOptions(vnVmTypeNameResource));
      setOpcoOptions(opcoOptions);
      setFormData({
        vnVmTypeNameResource,
        opcoBasedLocationResource,
        ...rest,
        vnfinfoDetail: { ...vnfinfoDetail, vnfvminstances: vnfInfoData ?? [] },
      });
    } else if (createResource) {
      const {
        vnfinfoDetail,
        vnVmTypeNameResource,
        opcoBasedLocationResource,
        ...rest
      } = createResource;

      setVnfTypeNameOptions(mapVmTypeOptions(vnVmTypeNameResource));
      setOpcoOptions(mapOpcoOptions(opcoBasedLocationResource));
      setFormData({
        ...createResource,
        vnfinfoDetail: {
          ...vnfinfoDetail,
          vnfvminstances: [],
        },
      });
    }
  }, [createResource, editResource, props.edit, isLocationCrudModalOpen]);

  const validazioneClient = (copy: VBOMInfoDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.vnfinfoDetail?.vnfnameid === null ||
      copy?.vnfinfoDetail?.vnfnameid === undefined ||
      copy?.vnfinfoDetail?.vnfnameid === 0
    ) {
      addInvalidProperty("vnfnameid");
    }

    if (
      copy?.vnfinfoDetail?.clusterid === null ||
      copy?.vnfinfoDetail?.clusterid === undefined ||
      copy?.vnfinfoDetail?.clusterid === 0
    ) {
      addInvalidProperty("clusterid");
    }

    if (
      copy?.vnfinfoDetail?.vmtypenameid === null ||
      copy?.vnfinfoDetail?.vmtypenameid === undefined ||
      copy?.vnfinfoDetail?.vmtypenameid === 0
    ) {
      addInvalidProperty("vmtypenameid");
    }
    if (
      copy?.vnfinfoDetail?.nsxt === null ||
      copy?.vnfinfoDetail?.nsxt === undefined
    ) {
      addInvalidProperty("nsxt");
    }
    if (
      copy?.vnfinfoDetail?.vmworkloadtype === null ||
      copy?.vnfinfoDetail?.vmworkloadtype === undefined
    ) {
      addInvalidProperty("vmworkloadtype");
    }
    if (
      copy?.vnfinfoDetail?.intravmtype === null ||
      copy?.vnfinfoDetail?.intravmtype === undefined
    ) {
      addInvalidProperty("intravmtype");
    }
    if (
      copy?.vnfinfoDetail?.intervmtype === null ||
      copy?.vnfinfoDetail?.intervmtype === undefined
    ) {
      addInvalidProperty("intervmtype");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  const validateInterface = (copy: VNFVMInstances) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.opcoid === null ||
      copy?.opcoid === undefined ||
      copy?.opcoid === 0
    ) {
      addInvalidProperty("opcoid");
    }

    if (
      copy?.locationid === null ||
      copy?.locationid === undefined ||
      copy?.locationid === 0
    ) {
      addInvalidProperty("locationid");
    }

    if (
      copy?.noofvnfinstances === undefined ||
      copy?.noofvnfinstances === null
    ) {
      addInvalidProperty("noofvnfinstances");
    }

    if (copy?.noofvmspertype === undefined || copy?.noofvmspertype === null) {
      addInvalidProperty("noofvmspertype");
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
  const validateCapacity = (copy: VNFVMCapacity) => {
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
      copy?.vcpupervm === null ||
      copy?.vcpupervm === undefined ||
      !isMultipleOfTwo(copy?.vcpupervm)
    ) {
      addInvalidProperty("vcpupervm");
    }

    if (
      copy?.rampervm === undefined ||
      copy?.rampervm === null ||
      !isMultipleOfFour(copy?.rampervm)
    ) {
      addInvalidProperty("rampervm");
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
    const { vnfinfoDetail, ...res } = formData as VBOMInfoDtoUpdate;
    let copy = { ...formData } as VBOMInfoDtoUpdate;
    if (vnfinfoDetail) {
      if (property === "intravmtype" || property === "intervmtype") {
        vnfinfoDetail[property] = e && e["value"];
      } else {
        vnfinfoDetail[property] = e && e["key"];
      }
    }
    setFormData({ ...copy, vnfinfoDetail });
  };

  const onChangeDropdownInterfaceSection = (property: string, e: any) => {
    const { vnfinfoDetail, ...res } = formData as VBOMInfoDtoUpdate;
    const { vnfvminstances, ...res1 } = vnfinfoDetail as VNFInfoDetail;
    let copy = { ...vnfinfoDetail } as VNFInfoDetail;
    if (vnfvminstances) {
      if (property === "intravmtype" || property === "intervmtype") {
        vnfvminstances[0][property] = e && e["value"];
      } else {
        vnfvminstances[0][property] = e && e["key"];
      }
    }
    setFormData({ ...formData, vnfinfoDetail: { ...copy, vnfvminstances } });
  };

  const onChangeInstance = (property: string, e: any) => {
    let copy = { ...instanceForm } as VNFVMInstances;
    if (property === "opcoid") {
      copy.opco = e && e["value"];
      copy.locationid = null;
    }
    if (property === "locationid") {
      copy.location = e && e["value"];
    }
    copy[property] = e && e["key"];
    setInstanceForm(copy);
  };

  const onChangeCapacity = (property: string, e: any) => {
    let copy = { ...capacityForm } as VNFVMCapacity;
    copy[property] = e && e["key"];
    setCapacityForm(copy);
  };

  const onChangeTextInfoSection = (property: string, e: any) => {
    const { vnfinfoDetail, ...res } = formData as VBOMInfoDtoUpdate;
    let copy = { ...formData } as VBOMInfoDtoUpdate;
    if (vnfinfoDetail) {
      vnfinfoDetail[property] = e && e.target.value;
    }
    setFormData({ ...copy, vnfinfoDetail });
  };

  const onChangeTextInstanceSection = (property: string, e: any) => {
    let copy = { ...instanceForm } as VNFVMInstances;
    copy[property] = e && e.target.value;
    setInstanceForm(copy);
  };

  const onChangeTextCapacitySection = (property: string, e: any) => {
    let copy = { ...capacityForm } as VNFVMCapacity;
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
      vnfinfoDetail: { vnfvminstances = [], ...restInfo } = {} as VNFInfoDetail,
      ...rest
    } = formData as VBOMInfoDtoUpdate;

    const updatedInstances = (vnfvminstances ?? []).map((instance, idx) => {
      const isMatch =
        instance.vnfvminstanceid === instanceDelecteId ||
        idx === instanceIndexId;
      if (!isMatch) return instance;

      const currentCapacities = instance.vnfvmcapacity ?? [];

      const updatedCapacities = currentCapacities.filter(
        (cap, i) => i !== capacityIndex
      );

      return { ...instance, vnfvmcapacity: updatedCapacities };
    });
    const updatedData: VBOMInfoDtoUpdate = {
      ...rest,
      vnfinfoDetail: {
        ...restInfo,
        vnfvminstances: updatedInstances,
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
        vnfinfoDetail: {
          vnfvminstances = [],
          ...restInfo
        } = {} as VNFInfoDetail,
        ...rest
      } = formData as VBOMInfoDtoUpdate;
      const updatedInstances = (vnfvminstances ?? []).map((instance, idx) => {
        const isMatch =
          instance.vnfvminstanceid === instanceId || idx === instanceId;
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
            vnfvminstanceid: addInstenceId,
          };
          return {
            ...instance,
            vnfvmcapacity: [...currentCapacities, newCapacity],
          };
        }
      });

      const updatedData: VBOMInfoDtoUpdate = {
        ...rest,
        vnfinfoDetail: {
          ...restInfo,
          vnfvminstances: updatedInstances,
        },
      };

      setFormData(updatedData);
      setCapacityForm(null);
      setAddCapacityFlag(false);
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
      const { vnfinfoDetail = { vnfvminstances: [] }, ...rest } =
        formData as VBOMInfoDtoUpdate;
      let instances = [...(vnfinfoDetail.vnfvminstances || [])];

      instances = instances?.map((inst, index) => {
        const isMatch =
          inst.vnfvminstanceid === editInterfaceId || index === editInterfaceId;

        if (edit && isMatch) return instanceForm;

        if (!edit && index === instanceId) return instanceForm;

        return inst;
      });

      // Add new instance if not matched
      if (!edit && (!instanceId || instanceId >= instances.length)) {
        instances.push({ ...instanceForm, vnfvmcapacity: [] });
      }

      const updatedData: VBOMInfoDtoUpdate = {
        ...rest,
        vnfinfoDetail: {
          ...vnfinfoDetail,
          vnfvminstances: instances.map((res) => ({
            ...res,
            numa: res.numa === "Yes" || res.numa === true,
          })),
        },
      };

      setFormData(updatedData);
      setInstanceForm(null);
      setAddInterfaceFlag(false);
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
    const { vnfinfoDetail = { vnfvminstances: [] }, ...rest } =
      formData as VBOMInfoDtoUpdate;
    let instances = [...(vnfinfoDetail.vnfvminstances || [])];

    instances = instances?.filter((inst, index) => index !== instanceIndex);

    const updatedData: VBOMInfoDtoUpdate = {
      ...rest,
      vnfinfoDetail: {
        ...vnfinfoDetail,
        vnfvminstances: instances,
      },
    };

    setFormData(updatedData);
  };
  const onSaveFormData = () => {
    if (!formData) return;

    const { vnfinfoDetail, ...rest } = formData as VBOMInfoDtoUpdate;

    const updatedInstances =
      vnfinfoDetail?.vnfvminstances?.map((res) => ({
        ...res,
        vnfvminstanceid: res.vnfvminstanceid ?? 0,
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

      const instances = (obj as VNFInfoDetail).vnfvminstances;
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

    const validation = validateObject(vnfinfoDetail ? vnfinfoDetail : {});
    if (validation?.pass) {
      Save(
        {
          ...rest,
          vnfinfoDetail: {
            ...vnfinfoDetail,
            vnfvminstances: updatedInstances,
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

  const getInterfaceColumns: Column<VNFVMInstances>[] = [
    { key: "opco", label: "Opco" },
    { key: "location", label: "Location" },
    { key: "noofvnfinstances", label: "No of VNF Instances" },
    { key: "noofvmspertype", label: "No of VMS Per Type" },
    { key: "numa", label: "Numa" },
    { key: "socket", label: "Socket" },
  ];

  const getCapacityColumns: Column<VNFVMCapacity>[] = [
    { key: "financialyear", label: "Financial Year" },
    { key: "vcpupervm", label: "LocNo of vCPU per VMation" },
    { key: "rxtxcpucount", label: "RX/TX' CPU INCLUDED IN vCPU Count" },
    { key: "rampervm", label: "RAM (GB) per VM" },
    { key: "datadisk", label: "Storage (GB) per VM - Data Disk" },
    { key: "osdisk", label: "Storage (GB) per VM - OS disk" },
    { key: "iopsrunning", label: "Storage IOPS per VM - Running" },
    { key: "iopsloading", label: "Storage IOPS per VM - Loading" },
    {
      key: "vmworkloaddistribution",
      label: "Storage Read/Write VM workload distribution (%/%)",
    },
    {
      key: "northsouthboundbandwidth",
      label: "North/South bandwidth requirement per VM (Mbit/s)",
    },
    {
      key: "eastwestboundbandwidth",
      label: "East/West bandwidth requirement per VM (Mbit/s)",
    },
    { key: "otherrequirements", label: "Onboarding Date / Other requirements" },
    { key: "backuprequired", label: "Backup Required" },
    { key: "probingrequired", label: "Probing Required" },
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
    if (formData && validazioneClient(formData).response === false) {
      rootStore.dispatch(
        setNotification({
          message: "Please check the fields entered in VNF Info Section.",
          notifyType: NotifyType.warning,
        })
      );
    } else {
      setAddInterfaceFlag(true);
    }
  };

  return (
    <div className="col-12">
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

      {/* <CustomAccordion
        title="VNF Info Details"
        disableToggle={true}
        key="VNF Info Details"
      > */}
      <div className="row col-12 px-0 mx-0">
        <fieldset className="fieldset p-0">
          <label className="text-bb mb-4">VNF Info Details</label>
          <div
            className="row pb-2 mb-4"
            style={{ borderBottom: "1px solid #e0e0e0" }}
          >
            <div className="col-6">
              <div className="col-12 pl-0">
                <DropdownInputComponent
                  label={"VNF Name"}
                  labelCSS="mb-0 text-left"
                  inputCSS="labelForm voda-bold mb-2"
                  isSearchable={true}
                  isClearable={true}
                  required={true}
                  disabled={props.edit}
                  value={
                    formData?.vnfNameResources &&
                    resourceArrayRefactor(formData?.vnfNameResources).filter(
                      (x) => x.key == formData?.vnfinfoDetail?.vnfnameid
                    )
                  }
                  options={
                    formData?.vnfNameResources &&
                    resourceArrayRefactor(formData?.vnfNameResources)
                  }
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("vnfnameid")
                      ? true
                      : false
                  }
                  error="VNF Name must have a value."
                  onChange={(e: any) =>
                    onChangeDropdownInfoSection("vnfnameid", e)
                  }
                />
              </div>
            </div>
            <div className="col-6">
              <div className="col-12 pr-0">
                <DropdownInputComponent
                  label={"Cluster Name"}
                  labelCSS="mb-0 text-left"
                  inputCSS="labelForm voda-bold mb-2"
                  isSearchable={true}
                  isClearable={true}
                  required={true}
                  disabled={props.edit}
                  value={
                    formData?.vnClusterNameResource &&
                    resourceArrayRefactor(
                      formData?.vnClusterNameResource
                    ).filter((x) => x.key == formData?.vnfinfoDetail?.clusterid)
                  }
                  options={
                    formData?.vnClusterNameResource &&
                    resourceArrayRefactor(formData?.vnClusterNameResource)
                  }
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("clusterid")
                      ? true
                      : false
                  }
                  error="Cluster Name must have a value."
                  onChange={(e: any) =>
                    onChangeDropdownInfoSection("clusterid", e)
                  }
                />
              </div>
            </div>
            <div className="col-6">
              <div className="col-12 pl-0">
                <DropdownInputComponent
                  label={"VNF VMType Name"}
                  labelCSS="mb-0 text-left"
                  inputCSS="labelForm voda-bold mb-2"
                  isSearchable={true}
                  isClearable={true}
                  required={true}
                  disabled={props.edit}
                  value={
                    vnfTypeNameOptions &&
                    vnfTypeNameOptions.filter(
                      (x) => x.key == formData?.vnfinfoDetail?.vmtypenameid
                    )
                  }
                  options={vnfTypeNameOptions}
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("vmtypenameid")
                      ? true
                      : false
                  }
                  error="VNF VMType Name must have a value."
                  onChange={(e: any) =>
                    onChangeDropdownInfoSection("vmtypenameid", e)
                  }
                />
              </div>
            </div>

            <div className="col-6 ">
              <div className="col-12 pr-0">
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
                      (x) => x.key == formData?.vnfinfoDetail?.nsxt
                    )
                  }
                  options={evumOption}
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("nsxt")
                      ? true
                      : false
                  }
                  error="NSX-T must have a value."
                  onChange={(e: any) => onChangeDropdownInfoSection("nsxt", e)}
                />
              </div>
            </div>
            <div className="col-6">
              <div className="col-12 pl-0">
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
                        (x) => x.value === formData?.vnfinfoDetail?.intravmtype
                      )
                      ?.map((res) => ({ key: res.value, value: res.text }))
                  }
                  options={
                    formData?.intraVmTypeResource?.map((res) => ({
                      key: res.value,
                      value: res.text,
                    })) ?? []
                  }
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("intravmtype")
                      ? true
                      : false
                  }
                  error="Intra VMType must have a value."
                  onChange={(e: any) =>
                    onChangeDropdownInfoSection("intravmtype", e)
                  }
                />
              </div>
            </div>

            <div className="col-6">
              <div className="col-12 pr-0">
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
                        (x) => x.value === formData?.vnfinfoDetail?.intervmtype
                      )
                      ?.map((res) => ({ key: res.value, value: res.text }))
                  }
                  options={
                    formData?.interTypeResource?.map((res) => ({
                      key: res.value,
                      value: res.text,
                    })) ?? []
                  }
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("intervmtype")
                      ? true
                      : false
                  }
                  error="Inter VMType must have a value."
                  onChange={(e: any) =>
                    onChangeDropdownInfoSection("intervmtype", e)
                  }
                />
              </div>
            </div>
            <div className="col-6">
              <div className="col-12 pl-0">
                <DropdownInputComponent
                  label={"VM Workload type"}
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
                          x.value === formData?.vnfinfoDetail?.vmworkloadtype
                      )
                      ?.map((res) => ({ key: res.value, value: res.text }))
                  }
                  options={
                    formData?.vmWorkLoadTypeDetail?.map((res) => ({
                      key: res.value,
                      value: res.text,
                    })) ?? []
                  }
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("vmworkloadtype")
                      ? true
                      : false
                  }
                  error="Inter VMType must have a value."
                  onChange={(e: any) =>
                    onChangeDropdownInfoSection("vmworkloadtype", e)
                  }
                />
              </div>
            </div>

            <div className="col-6 ">
              <div className="col-12 pr-0">
                <TextInputComponent
                  label="Storage Block Size"
                  labelCSS="mb-0"
                  inputCSS="labelForm voda-bold mb-2"
                  value={formData?.vnfinfoDetail?.vmstorageblocksize}
                  required={false}
                  isError={
                    validation &&
                    validation.response === false &&
                    validation.property?.includes("vmstorageblocksize")
                      ? true
                      : false
                  }
                  error={"*Storage Block Size must have a value."}
                  onChange={(e: any) =>
                    onChangeTextInfoSection("vmstorageblocksize", e)
                  }
                />
              </div>
            </div>
          </div>
        </fieldset>
      </div>
      {/* </CustomAccordion> */}
      <CustomAccordion title="Instance">
        {addInterfaceFlag ? (
          <div className="d-flex mb-4 mx-1 justify-content-start">
            <h4>{editInterfaceId !== null ? "Edit" : "Add"} Instance</h4>
          </div>
        ) : (
          <div className="d-flex mb-4 mx-1 justify-content-between">
            <Typography variant="h6" gutterBottom>
              Instance
            </Typography>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              onClick={() => onAddInstanceHandle()}
              type="button"
            >
              Add Instance
            </button>
          </div>
        )}
        {addInterfaceFlag && (
          <Card
            variant="outlined"
            sx={{ marginBottom: "1rem", padding: "1rem" }}
          >
            <>
              <div className="row">
                <div className="col-6">
                  <div className="col-12 pl-0">
                    <DropdownInputComponent
                      label={"Opco"}
                      labelCSS="mb-0 text-left"
                      inputCSS="labelForm voda-bold mb-2"
                      isSearchable={true}
                      isClearable={true}
                      required={true}
                      value={
                        opcoOptions &&
                        opcoOptions.filter((x) => x.key == instanceForm?.opcoid)
                      }
                      options={opcoOptions}
                      isError={
                        validationInterfaceForm &&
                        validationInterfaceForm.response === false &&
                        validationInterfaceForm.property?.includes("opcoid")
                          ? true
                          : false
                      }
                      error="VNF VMType Name must have a value."
                      onChange={(e: any) => onChangeInstance("opcoid", e)}
                    />
                  </div>
                </div>
                <div className="col-6">
                  <div className="col-12 d-flex align-items-end">
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
                            (val) => instanceForm?.opcoid === val.opcoId
                          )[0]
                          ?.locationDetails?.map((res) => ({
                            key: res.value,
                            value: res.text,
                          }))
                          .filter((x) => x.key == instanceForm?.locationid)}
                        options={
                          instanceForm?.opcoid !== 0
                            ? formData?.opcoBasedLocationResource
                                ?.filter(
                                  (val) => instanceForm?.opcoid === val.opcoId
                                )[0]
                                ?.locationDetails?.map((res) => ({
                                  key: res.value,
                                  value: res.text,
                                }))
                            : null
                        }
                        isError={
                          validationInterfaceForm &&
                          validationInterfaceForm.response === false &&
                          validationInterfaceForm.property?.includes(
                            "locationid"
                          )
                            ? true
                            : false
                        }
                        error="*Site must have a value."
                        onChange={(e: any) => onChangeInstance("locationid", e)}
                      />
                    </div>
                    <div className="pb-4">
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
                    </div>
                  </div>
                </div>
                <div className="col-6">
                  <div className="col-12 pl-0">
                    <TextInputComponent
                      label="No of VNF Instances"
                      labelCSS="mb-0"
                      inputCSS="labelForm voda-bold mb-2"
                      value={instanceForm?.noofvnfinstances}
                      required={true}
                      isError={
                        validationInterfaceForm &&
                        validationInterfaceForm.response === false &&
                        validationInterfaceForm.property?.includes(
                          "noofvnfinstances"
                        )
                          ? true
                          : false
                      }
                      validationError={
                        inputError?.field === "noofvnfinstances" ? true : false
                      }
                      error={
                        inputError?.field === "noofvnfinstances"
                          ? "*Only number are allowed."
                          : "*No of VNF Instances must have a value."
                      }
                      onChange={(e: any) => {
                        const value = e.target.value;
                        if (/^\d*$/.test(value)) {
                          onChangeTextInstanceSection("noofvnfinstances", e);
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
                  <div className="col-12 pr-0">
                    <TextInputComponent
                      label="No of VMS Per Type"
                      labelCSS="mb-0"
                      inputCSS="labelForm voda-bold mb-2"
                      value={instanceForm?.noofvmspertype}
                      required={true}
                      isError={
                        validationInterfaceForm &&
                        validationInterfaceForm.response === false &&
                        validationInterfaceForm.property?.includes(
                          "noofvmspertype"
                        )
                          ? true
                          : false
                      }
                      validationError={
                        inputError?.field === "noofvmspertype" ? true : false
                      }
                      error={
                        inputError?.field === "noofvmspertype"
                          ? "*Only number are allowed."
                          : "*No of VMS Per Type must have a value."
                      }
                      onChange={(e: any) => {
                        const value = e.target.value;
                        if (/^\d*$/.test(value)) {
                          onChangeTextInstanceSection("noofvmspertype", e);
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
                  <div className="col-12 pl-0">
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
                        validationInterfaceForm.property?.includes("numa")
                          ? true
                          : false
                      }
                      error="*Numa must have a value."
                      onChange={(e: any) => onChangeInstance("numa", e)}
                    />
                  </div>
                </div>
                <div className="col-6 ">
                  <div className="col-12 pr-0">
                    <TextInputComponent
                      label="Enter Socket"
                      labelCSS="mb-0"
                      inputCSS="labelForm voda-bold mb-2"
                      value={instanceForm?.socket ?? ""}
                      required={instanceForm?.numa === true ? true : false}
                      isError={
                        validationInterfaceForm &&
                        validationInterfaceForm.response === false &&
                        validationInterfaceForm.property?.includes("socket")
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
              <div className="d-flex my-2" style={{ justifySelf: "self-end" }}>
                <button
                  className="  voda-bold btn btn-link px-4 btnHeader cancel"
                  onClick={() => {
                    setInstanceForm(null);
                    setAddInterfaceFlag(false);
                    setEditInterfaceId(null);
                  }}
                  type="button"
                >
                  Cancel
                </button>
                <button
                  className="  voda-bold btn btn-danger px-4 btnHeader"
                  onClick={() =>
                    onSubmitInterface({
                      edit: editInterfaceId !== null ? true : false,
                    })
                  }
                  type="button"
                >
                  {editInterfaceId !== null ? "Update" : "Save"} Instance
                </button>
              </div>
            </>
          </Card>
        )}
        <CustomCollapsibleTable
          columns={getInterfaceColumns}
          data={
            formData?.vnfinfoDetail?.vnfvminstances.map((res) => ({
              ...res,
              numa: res?.numa === true ? "Yes" : "No",
            })) ?? []
          }
          size="small"
          onEdit={(row: any, index: any) => {
            setInstanceForm({
              ...row,
              numa: row.numa ? (row.numa === "Yes" ? true : false) : null,
            });
            setEditInterfaceId(row.vnfvminstanceid ?? index);
            setAddInterfaceFlag(true);
          }}
          onDelete={(row, idx) =>
            onDelectInstance({ instanceId: null, instanceIndex: idx })
          }
          setCapacityFlag={(val) => setAddCapacityFlag(val)}
          renderExpand={(row: any, index) => (
            <>
              {addCapacityFlag ? (
                <div className="d-flex my-4 mx-1 justify-content-start">
                  <h4>
                    {editCapacityId !== null &&
                    editCapacityId?.capacityId !== null
                      ? "Edit"
                      : "Add"}{" "}
                    Capacity
                  </h4>
                </div>
              ) : (
                <div className="d-flex my-4 mx-1 justify-content-between">
                  <Typography variant="h6" gutterBottom>
                    Capacity
                  </Typography>
                  <button
                    className="  voda-bold btn btn-danger px-4 btnHeader"
                    onClick={() => {
                      setEditCapacityId({
                        instanceId: row.vnfvminstanceid ?? index,
                        capacityId: null,
                      });
                      setAddCapacityFlag(true);
                    }}
                    type="button"
                  >
                    Add Capacity
                  </button>
                </div>
              )}
              {addCapacityFlag && (
                <Card
                  variant="outlined"
                  sx={{ marginBottom: "1rem", padding: "1rem" }}
                >
                  <>
                    <div className="row">
                      <div className="col-6 ">
                        <div className="col-12 pl-0">
                          <ShowYearInputComponent
                            label={`${
                              LabelsDictionary["financialyear"]?.Full ??
                              "Financial Year"
                            }`}
                            labelCSS="mb-0"
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
                            }}
                          />
                        </div>
                      </div>
                      <div className="col-6 ">
                        <div className="col-12 pr-0">
                          <TextInputComponent
                            label="No of vCPU per VM"
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={capacityForm?.vcpupervm}
                            required={true}
                            isError={
                              (validationCapacityForm &&
                                validationCapacityForm.response === false &&
                                validationCapacityForm.property?.includes(
                                  "vcpupervm"
                                )) ||
                              (capacityForm?.vcpupervm &&
                                !isMultipleOfTwo(capacityForm?.vcpupervm))
                                ? true
                                : false
                            }
                            validationError={
                              inputError?.field === "vcpupervm" ? true : false
                            }
                            error={
                              capacityForm?.vcpupervm &&
                              !isMultipleOfTwo(capacityForm?.vcpupervm)
                                ? "*Value must be a multiple of 2 (e.g. 2, 4, 8, 16, ...)."
                                : inputError?.field === "vcpupervm"
                                ? "*Only number are allowed."
                                : "*No of vCPU per VM must have a value."
                            }
                            onChange={(e: any) => {
                              const value = e.target.value;
                              if (/^\d*$/.test(value)) {
                                onChangeTextCapacitySection("vcpupervm", e);
                                setInputError(null);
                              } else {
                                setInputError({
                                  field: "vcpupervm",
                                  type: "number",
                                });
                              }
                            }}
                          />
                        </div>
                      </div>
                      <div className="col-6 ">
                        <div className="col-12 pl-0">
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
                        <div className="col-12 pr-0">
                          <TextInputComponent
                            label="RAM (GB) per VM"
                            labelCSS="mb-0"
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
                        <div className="col-12 pl-0">
                          <TextInputComponent
                            label="Storage (GB) per VM- Data Disk"
                            labelCSS="mb-0"
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
                        <div className="col-12 pr-0">
                          <TextInputComponent
                            label="Storage (GB) per VM- OS disk"
                            labelCSS="mb-0"
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
                        <div className="col-12 pl-0">
                          <TextInputComponent
                            label="Storage IOPS per VM - Running"
                            labelCSS="mb-0"
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
                        <div className="col-12 pr-0">
                          <TextInputComponent
                            label="Storage IOPS per VM - Loading"
                            labelCSS="mb-0"
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
                        <div className="col-12 pl-0">
                          <TextInputComponent
                            label="Storage Read/Write VM workload distribution (%/%)"
                            labelCSS="mb-0"
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
                        <div className="col-12 pr-0">
                          <TextInputComponent
                            label="North/South bandwidth requirement per VM (Mbit/s)"
                            labelCSS="mb-0"
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
                        <div className="col-12 pl-0">
                          <TextInputComponent
                            label="East/West bandwidth requirement per VM (Mbit/s)"
                            labelCSS="mb-0"
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
                        <div className="col-12 pr-0">
                          <TextInputComponent
                            label="Onboarding Date / Other requirements?"
                            labelCSS="mb-0"
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
                        <div className="col-12 pl-0">
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
                                    (x) => x.key == capacityForm?.backuprequired
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
                        <div className="col-12 pr-0">
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
                    <div
                      className="d-flex my-2"
                      style={{ justifySelf: "self-end" }}
                    >
                      <button
                        className="  voda-bold btn btn-link px-4 btnHeader cancel"
                        onClick={() => {
                          setCapacityForm(null);
                          setAddCapacityFlag(false);
                          setEditCapacityId(null);
                        }}
                        type="button"
                      >
                        Cancel
                      </button>
                      <button
                        className="  voda-bold btn btn-danger px-4 btnHeader"
                        onClick={() =>
                          onSubmitCapacity({
                            edit:
                              editCapacityId !== null &&
                              editCapacityId?.capacityId !== null
                                ? true
                                : false,
                            addInstenceId: row?.vnfvminstanceid,
                          })
                        }
                        type="button"
                      >
                        {editCapacityId !== null &&
                        editCapacityId?.capacityId !== null
                          ? "Update"
                          : "Save"}{" "}
                        Capacity
                      </button>
                    </div>
                  </>
                </Card>
              )}
              <CustomMUITable
                columns={getCapacityColumns}
                data={
                  formData?.vnfinfoDetail?.vnfvminstances[
                    index
                  ]?.vnfvmcapacity?.map((res) => ({
                    ...res,
                    rxtxcpucount: res?.rxtxcpucount === true ? "Yes" : "No",
                    backuprequired: res?.backuprequired === true ? "Yes" : "No",
                    probingrequired:
                      res?.probingrequired === true ? "Yes" : "No",
                  })) ?? []
                }
                onEdit={(row: any, idx: any) => {
                  setEditCapacityId({
                    instanceId: row.vnfvminstanceid
                      ? row.vnfvminstanceid
                      : editCapacityId?.instanceId,
                    capacityId: row.vnfvmcapacityid ?? idx,
                  });
                  setCapacityForm({
                    ...row,
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
                }}
                onDelete={(row, idex) => {
                  onDeleteCapacity({
                    instanceDelecteId: null,
                    instanceIndexId: index,
                    capacityDeleteId: null,
                    capacityIndex: idex,
                  });
                }}
                size="small"
              />
            </>
          )}
        />
      </CustomAccordion>
      <div className="col-12 justify-content-end d-flex mt-4">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => onSaveFormData()}
          type="button"
        >
          Submit
        </button>
      </div>
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
    </div>
  );
};
export default VBOMInfoModal;
