import React, { useEffect, useState } from "react";
import { Form, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import Select from "react-select";
import AsyncSelect from "react-select/async";

import Container from "../../Components/Container";
import ModalConfirm from "../../Components/ModalConfirm";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import {
  formatDateWithTime,
  numberIsNullOrZero,
  stringIsNullOrEmpty,
} from "../../Hook/Common";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import {
  DesignComponentDtoCreate,
  DesignComponentDtoUpdate,
  ImpactServiceBoundaryChanged,
  SystemAndServiceBoundary,
  UsedSubnetworkBoundary,
} from "../../Model/DesignComponent";
import {
  GetSubnetworkBoundaries,
  GetSubNetworkBoundariesDestructured,
  GetSystemTypeAndServiceBoundary,
  GetSystemTypeAndServiceBoundaryDestructured,
} from "../../Redux/Action/DesignComponent/DesignComponentCommonAction";
import { CreatDesignComponent } from "../../Redux/Action/DesignComponent/DesignComponentCreateAction";
import { EditDesignComponent } from "../../Redux/Action/DesignComponent/DesignComponentEditAction";
import { GetSystemTypeGrid } from "../../Redux/Action/SystemType/SystemTypeGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

import { MultiSelect } from "react-multi-select-component";
import { returnUniqueArray } from "../../Business/Common/CommonBusiness";
import SubNetworkBoundary from "../../Containers/Lookup/SubNetworkBoundaryContainer";
import { RelatedRecordsResultDto } from "../../Model/CommonModels";
import { GetServicesOfSubNetworkBoundaries } from "../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryCreateAction";
import { dictionaryToArray } from "./../../Hook/Dictionary";
import { UnUsedSubnetworkBoundary } from "./../../Model/DesignComponent";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";

interface Props {
  action: {
    closeModal?(changed?: boolean): any;
    refresh?(): any;
    Edit?(id: number | undefined): any;
    validateFormWizard?(
      response: boolean,
      formData: DesignComponentDtoCreate,
      property: string
    );
    wizardBackFunction?(
      formData: DesignComponentDtoCreate,
      property: string
    ): any;
    setConfirmExitWizard?(): any;
    setIsVisibleServiceBoundaryModalLookup?(isVisible: boolean);
    setDataCheck?(prop: string, val: number | string): any;
  };
  systemSolution?: string | undefined;
  vodafoneName?: string;
  destructuredData?: { msOem?: number; msst?: number; mhOem?: number };
  edit: boolean;
  DC?: string;
  keyTab?: string;
  wizardMode: boolean;
  wizardStep?: number;
  dataWizard?: DesignComponentDtoCreate;
  dataCheckDcfExist?: {
    mhOemId: number;
    swAppType: string;
    swOemId: number;
    sbId: number;
    platform: number;
  };
  isBack?: boolean;
  productName?: string;
}

const ModalDesignComponent: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("lcm");
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChangeSelect,
    setChanged,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<DesignComponentDtoUpdate>(
    CreatDesignComponent,
    EditDesignComponent
  );

  const dtoEditResourceState = (state: RootState) =>
    state.designComponentEditReducer.DesignComponentDtoEdit;

  const dtoNewResourceState = (state: RootState) =>
    state.designComponentCreateReducer.DesignComponentDtoCreate;
  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const [resourceSystemType, setResourceSystemType] = useState<
    { key: number; value: string }[] | undefined
  >([]);
  const resourceSystemTypeState = useSelector(
    (state: RootState) => state.systemTypeGridReducer.SystemTypeGridResult
  );

  const [unUsedSubNetworkBoundaries, setUnUsedSubNetworkBoundaries] = useState<
    Array<UnUsedSubnetworkBoundary>
  >([]);
  const [supportedAllServiceFlag, setSupportedAllServiceFlag] = useState(false);

  const [usedSubnetworkBoundary, setUsedSubnetworkBoundary] = useState<
    Array<UsedSubnetworkBoundary>
  >([]);

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);
  const [selected, setSelected] = useState<
    { label: string; value: number }[] | undefined
  >(undefined);

  let resourceSystemTypeData = resourceSystemTypeState?.items?.map((s) => {
    return { key: s.systemTypeId, value: s.systemSolution ?? "" } as {
      key: number;
      value: string;
    };
  });

  const [systemAndServiceBoundary, setSystemAndServiceBoundary] =
    useState<Array<SystemAndServiceBoundary>>();
  const [showSupportedAllService, setAllSupportedAllService] =
    useState<boolean>(false);
  const [systemSolutionIsChanged, setSystemSolutionIsChanged] =
    useState<boolean>(false);
  const [serviceBoundaryIsChanged, setServiceBoundaryIsChanged] =
    useState<boolean>(false);
  const [serviceBoundarySelected, setServiceBoundarySelected] = useState<
    Array<number>
  >([]);

  const [supportedServiceList, setSupportedServiceList] = useState<Array<any>>(
    []
  );

  const [swApplicationType, setSwApplicationType] = useState<number>(0);

  const [systemSolutionSelected, setSystemSolutionSelected] =
    useState<number>();

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();

  const [otherButtonForRelatedModal, setOtherButtonForRelatedModal] = useState<{
    deleteAllButton: string | undefined;
    deleteButton: string | undefined;
  }>({ deleteAllButton: undefined, deleteButton: undefined });

  const [systemSolutionBackup, setSystemSolutionBackup] = useState<number>();
  const [vodafoneNameId, setVodafoneNameId] = useState<number | undefined>(
    undefined
  );
  const [serviceBoundaryToCreate, setServiceBoundaryToCreate] =
    useState<string>();
  const [impactServiceBoundaryChanged, setImpactServiceBoundaryChanged] =
    useState<ImpactServiceBoundaryChanged>();

  const [
    serviceBoundarySelectedFromCheckBox,
    setServiceBoundarySelectedFromCheckBox,
  ] = useState<Array<number>>([]);

  useEffect(() => {
    setResourceSystemType(resourceSystemTypeData);
  }, [resourceSystemTypeState]);

  useEffect(() => {
    if (props.edit) {
    } else if (!props.wizardMode) {
      setFormData(createResource);
      setSystemSolutionBackup(undefined);
      // GetSystemTypeGrid();
    }
  }, [createResource, editResource, props.edit, props.dataWizard]);

  useEffect(() => {
    if (props.keyTab == "" || props.keyTab == null) {
      setKey("lcm");
    } else {
      setKey(props.keyTab);
    }
  }, []);

  useEffect(() => {
    if (props?.wizardStep) {
      let copy = { ...createResource } as DesignComponentDtoCreate;
      let destr = props.destructuredData;
      if (props?.isBack) {
        GetSystemTypeAndServiceBoundaryDestructured(
          destr?.mhOem,
          destr?.msst,
          destr?.msOem
        ).then((response) => {
          setVodafoneNameId(destr?.msst);
          if (response) {
            setUsedSubnetworkBoundary(response?.usedSubNetworkBoundaries!);
            setUnUsedSubNetworkBoundaries([
              ...response?.unUsedSubNetworkBoundaries!,
              {
                systemSolutionName: "Supports All Services",
                subNetworkBoundaryId: -1,
                subNetworkBoundaryName: "Supports All Services",
                systemTypeId: 1,
                subNetworkBoundaryAlias: "1",
              },
            ]);

            copy.subNetworkBoundaryResource =
              response?.unUsedSubNetworkBoundaries!;
            let selectedSubnetworkBoundary: any[] = [];
            props?.dataWizard?.subNetworkBoundaryIds?.forEach((id) => {
              let els = [
                ...response?.unUsedSubNetworkBoundaries!,
                {
                  systemSolutionName: "Supports All Services",
                  subNetworkBoundaryId: -1,
                  subNetworkBoundaryName: "Supports All Services",
                  systemTypeId: 1,
                  subNetworkBoundaryAlias: "1",
                },
              ]?.filter((el) => el?.subNetworkBoundaryId === id);

              // copy.supportedAllServices = response?.supportedAllServices!;
              if (els?.length) {
                selectedSubnetworkBoundary = [
                  ...selectedSubnetworkBoundary,
                  ...els,
                ];
              }
            });
            setSelected(
              selectedSubnetworkBoundary.map((item) => ({
                label: item?.subNetworkBoundaryName,
                value: item?.subNetworkBoundaryId,
              }))
            );
          }
        });
        if (props?.dataWizard) {
          copy.supportedAllServices = props?.dataWizard?.supportedAllServices;
          copy.subNetworkBoundaryIds = props?.dataWizard?.subNetworkBoundaryIds;
          copy.supportedAllServices =
            props?.dataWizard?.subNetworkBoundaryIds?.includes(1)!;

          setSystemSolutionIsChanged(true);
          setServiceBoundaryIsChanged(true);
        }
      } else {
        setVodafoneNameId(destr?.msst);
        //GET SUBNETWORK BOUNDARIES AND DATA SCREEN FORM WIZARD MOOD
        GetSystemTypeAndServiceBoundaryDestructured(
          destr?.mhOem,
          destr?.msst,
          destr?.msOem
        ).then((response) => {
          setSystemSolutionIsChanged(true);
          setServiceBoundaryIsChanged(false);
          setServiceBoundarySelected([]);
          setSelected([]);
          setCheckInsertBtn(false);
          copy.subNetworkBoundaryIds = undefined;
          if (response) {
            setUsedSubnetworkBoundary(response?.usedSubNetworkBoundaries!);
            setUnUsedSubNetworkBoundaries([
              ...response?.unUsedSubNetworkBoundaries!,
              {
                systemSolutionName: "Supports All Services",
                subNetworkBoundaryId: -1,
                subNetworkBoundaryName: "Supports All Services",
                systemTypeId: 1,
                subNetworkBoundaryAlias: "1",
              },
            ]);
            // copy.supportedAllServices = response?.supportedAllServices!;
            setAllSupportedAllService(response?.supportedAllServices!);
          }

          copy.subNetworkBoundaryResource = unUsedSubNetworkBoundaries;
        });
      }

      setFormData(copy);
    }
  }, [props?.wizardStep]);

  useEffect(() => {
    if (selected && selected?.length) {
      changeSelectedFromSelect(
        selected.map((item) => ({ key: item?.value, value: item?.label }))
      );
    } else {
      changeSelectedFromSelect([]);
      setSupportedServiceList([]);
    }

    if (props?.wizardMode) {
      let copy = { ...formData } as DesignComponentDtoUpdate;
      if (selected && !selected?.length) {
        copy.subNetworkBoundaryIds = undefined;
      } else {
        const checkAllSupportedService = selected?.filter(
          (item) => item.value === -1
        );
        checkAllSupportedService?.length
          ? (copy.supportedAllServices = true)
          : (copy.supportedAllServices = false);
      }
      setFormData(copy);
    }
  }, [selected]);

  useEffect(() => {
    if (
      formData &&
      props?.wizardMode &&
      formData?.subNetworkBoundaryIds?.length
    ) {
      formData?.subNetworkBoundaryIds.forEach((id) => {
        formData?.subNetworkBoundaryIds &&
          props.action.setDataCheck &&
          props.action.setDataCheck("sbId", id);
      });
    }
  }, [formData?.subNetworkBoundaryIds]);

  useEffect(() => {
    if (formData?.subNetworkSupportedServices) {
      const arr = dictionaryToArray(formData.subNetworkSupportedServices);
      setSupportedServiceList(arr);
    }
  }, [formData?.subNetworkSupportedServices]);

  useEffect(() => {
    let arr;
    let ids: number[] = [];
    if (
      selected &&
      selected?.length &&
      selected.filter((item) => item.value == -1).length
    ) {
      setSupportedServiceList([{ key: -1, value: "Supports All Services" }]);
      setSupportedAllServiceFlag(true);
      let copy = { ...formData } as DesignComponentDtoUpdate;
      copy.supportedAllServices = true;
      setFormData(copy);
    }
    if (selected && selected?.length) {
      ids = selected.map((item1) => item1.value);
      ids = returnUniqueArray([...ids, ...serviceBoundarySelectedFromCheckBox]);

      GetServicesOfSubNetworkBoundaries(ids).then((response) => {
        arr = dictionaryToArray(response);
        setSupportedServiceList(arr);
        let uniqueArray = [
          ...serviceBoundarySelected!,
          ...serviceBoundarySelectedFromCheckBox,
        ]?.filter(function (item, pos) {
          return (
            [
              ...serviceBoundarySelected!,
              ...serviceBoundarySelectedFromCheckBox,
            ] &&
            [
              ...serviceBoundarySelected!,
              ...serviceBoundarySelectedFromCheckBox,
            ].indexOf(item) == pos
          );
        });
        insertServiceBoundary(uniqueArray ?? [], arr ?? []);
      });
    }
  }, [selected]);

  useEffect(() => {
    if (formData?.supportedAllServices) {
      if (formData.systemTypeId && !props?.wizardMode) {
      } else {
        var obj = props?.dataCheckDcfExist;
        if (
          obj &&
          !numberIsNullOrZero(obj?.mhOemId) &&
          !numberIsNullOrZero(obj?.swOemId) &&
          !stringIsNullOrEmpty(obj?.swAppType) &&
          formData?.supportedAllServices
        ) {
          setSystemSolutionIsChanged(true);
        }
      }
    }
  }, [formData?.supportedAllServices]);

  //SELECT ASYNC SOFTWARE BUILD
  const searchSystemType = (input: string) => {
    return formData?.systemTypeResource?.filter((x) =>
      x.value.toLowerCase().includes(input.toLowerCase())
    );
  };

  const validazioneClient = (copy: DesignComponentDtoUpdate) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (numberIsNullOrZero(copy.systemTypeId) && !props?.wizardMode) {
      addInvalidProperty("systemTypeId");
    }
    if (
      copy?.subNetworkBoundaryIds === undefined &&
      selected?.length === 0 &&
      !copy.supportedAllServices
    ) {
      addInvalidProperty("subNetworkBoundaryIds");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  const customValidateServiceBoundaryCreate = () => {
    return !stringIsNullOrEmpty(serviceBoundaryToCreate);
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal && props.action.closeModal(changed);
    props.action.refresh && props.action.refresh();
  };

  const [checkInsertBtn, setCheckInsertBtn] = useState<boolean>(false);

  const [vodafoneName, setVodafoneName] = useState<string>();

  const [orphanDeleted, setOrphanDeleted] = useState<boolean>(false);
  const RestoreOrphanDeleted = (id: number | undefined) => {
    setOrphanDeleted(true);
    props.action.Edit && props.action.Edit(id);
  };

  const SubnetworkBoundaryGrid = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.LookUpGridResult;
  const SubnetworkBoundaryGridDto = useSelector(SubnetworkBoundaryGrid);
  const SubnetworkBoundaryGridAll = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.LookUpGridResultAll;
  const SubnetworkBoundaryGridDtoAll = useSelector(SubnetworkBoundaryGridAll);

  const validateWizard = () => {
    let copy = { ...formData } as DesignComponentDtoCreate;
    props.action.validateFormWizard &&
      props.action.validateFormWizard(
        validazioneClient(copy).response,
        copy,
        "designComponentDto"
      );
  };

  const onChangeGDPR = (value: boolean | undefined) => {
    let copy = { ...formData } as DesignComponentDtoUpdate;
    copy.gdprRelevant = value;
    setFormData(copy);
  };

  // const onChangeSupportedAllService = async (e: any) => {
  //   let copy = { ...formData } as DesignComponentDtoUpdate;
  //   copy.supportedAllServices = e.target.checked;

  //   if (e.target.checked) {
  //     if (!props.wizardMode) {
  //       setSelected([]);
  //       setSupportedServiceList([]);
  //     } else {
  //       var obj = props.dataCheckDcfExist;
  //       setSelected([]);
  //     }
  //   } else {
  //   }
  //   setFormData(copy);
  // };

  const onChangeSystemSolution = async (e: any) => {
    let copy = { ...formData } as DesignComponentDtoUpdate;

    if (e && e["key"]) {
      if (e["key"]) {
        setSwApplicationType(e["key"]);
        await GetSystemTypeAndServiceBoundary(e["key"]).then((response) => {
          if (response) {
            setUsedSubnetworkBoundary(response?.usedSubNetworkBoundaries!);
            if (!response.supportedAllServices) {
              setUnUsedSubNetworkBoundaries([
                ...response?.unUsedSubNetworkBoundaries!,
                {
                  systemSolutionName: "Supports All Services",
                  subNetworkBoundaryId: -1,
                  subNetworkBoundaryName: "Supports All Services",
                  systemTypeId: 1,
                  subNetworkBoundaryAlias: "1",
                },
              ]);
            } else {
              setUnUsedSubNetworkBoundaries(
                response?.unUsedSubNetworkBoundaries!
              );
            }
            setVodafoneNameId(response?.vodafoneNameId);
            setSystemSolutionIsChanged(true);
            setServiceBoundaryIsChanged(false);
            setServiceBoundarySelected([]);
            setSelected([]);
            setCheckInsertBtn(false);
            setVodafoneName(response?.vodafoneName);
            copy.subNetworkBoundaryIds = undefined;
            copy.supportedAllServices = response?.supportedAllServices!;
            setAllSupportedAllService(response.supportedAllServices!);
          }
        });

        if (props.edit) {
        }
      } else {
        setSystemSolutionIsChanged(false);
      }
      copy.systemTypeId = e["key"];
      copy.supportedAllServices = false;
      setFormData(copy);
    } else {
      setSystemSolutionIsChanged(false);
      copy.systemTypeId = undefined;
      copy.subNetworkBoundaryIds = undefined;
      setFormData(copy);
      setSelected([]);
    }
  };

  const insertServiceBoundary = async (
    ids: number[],
    supportAllServiceList: any[]
  ) => {
    let copy = { ...formData } as DesignComponentDtoUpdate;
    // setCheckInsertBtn(true);
    copy.subNetworkBoundaryIds = selected?.map((item) => item?.value);
    if (props.wizardMode && props.dataWizard) {
      copy.subNetworkBoundaryIds = selected?.map((item) => item?.value);
      copy.subNetworkBoundaryResource = unUsedSubNetworkBoundaries;
    }
    if (!copy.supportedAllServices) {
      setAllSupportedAllService(true);
    }

    ids.forEach((id) => {
      setSystemSolutionBackup(id);
    });
    // CHECK DCF EXIST

    if (supportedServiceList?.length || supportAllServiceList.length) {
      copy.supportedServiceIds = supportedServiceList.length
        ? supportedServiceList.map((item) => item?.key)
        : supportAllServiceList.map((item) => item?.key);
    }

    setFormData(copy);
  };

  const serviceBoundaryRefillData = async () => {
    let copy = { ...formData } as DesignComponentDtoUpdate;
    if (props?.wizardMode) {
      await GetSubNetworkBoundariesDestructured(
        props?.destructuredData?.msst!
      ).then((res) => {
        setUnUsedSubNetworkBoundaries(
          [
            ...res!,
            {
              systemSolutionName: "Supports All Services",
              subNetworkBoundaryId: -1,
              subNetworkBoundaryName: "Supports All Services",
              systemTypeId: 1,
              subNetworkBoundaryAlias: "1",
            },
          ] ?? []
        );
      });
    } else {
      await GetSubnetworkBoundaries(swApplicationType).then((res) => {
        if (showSupportedAllService) {
          setUnUsedSubNetworkBoundaries([...res!]!);
        } else {
          setUnUsedSubNetworkBoundaries(
            [
              ...res!,
              {
                systemSolutionName: "Supports All Services",
                subNetworkBoundaryId: -1,
                subNetworkBoundaryName: "Supports All Services",
                systemTypeId: 1,
                subNetworkBoundaryAlias: "1",
              },
            ]!
          );
        }
      });
    }

    setFormData(copy);
  };

  const changeSelectedFromSelect = (e: any) => {
    let array = [...serviceBoundarySelected];

    if (e != null && e?.length > 0 && e !== undefined) {
      for (let i = 0; i < e?.length; i++) {
        array.push(e[i].key);
      }
      setServiceBoundarySelected([...array]);
      // let uniqueArray = [
      //   ...array!,
      //   ...serviceBoundarySelectedFromCheckBox,
      // ]?.filter(function (item, pos) {
      //   return (
      //     [
      //       ...serviceBoundarySelected!,
      //       ...serviceBoundarySelectedFromCheckBox,
      //     ] &&
      //     [
      //       ...serviceBoundarySelected!,
      //       ...serviceBoundarySelectedFromCheckBox,
      //     ].indexOf(item) == pos
      //   );
      // });
      // insertServiceBoundary(uniqueArray ?? []);
    } else {
      setServiceBoundarySelected([]);
      setSystemSolutionSelected(undefined);
    }
  };

  const onHideModal = async () => {
    if (isVisibleModalLookup === 1) {
      await serviceBoundaryRefillData();
    }
  };

  const returnLookup = () => {
    switch (isVisibleModalLookup) {
      case 1:
        return (
          <SubNetworkBoundary
            returnObject={serviceBoundaryRefillData}
            vodafoneNameId={vodafoneNameId}
            modal={{
              isModal: true,
              setIsVisibleModalLookup: (e: number) =>
                setIsVisibleModalLookup(e),
            }}
            isPopup={true}
          />
        );

      default:
        return null;
    }
  };
  return (
    <div className="col-12">
      <ModalConfirm data={confirmForm} />

      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => {
          onHideModal();
          setIsVisibleModalLookup(0);
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
                onHideModal();
                setIsVisibleModalLookup(0);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {returnLookup()}
        </DialogContent>
      </Dialog>

      <form id="formDesignComponent" onChange={() => setChanged(true)}>
        <div className="row mx-0 col-12 px-0">
          {!props.wizardMode && (
            <>
              <div className="col-6">
                <div className="form-group">
                  <label className="vosa-bold mb-0 w-100">
                    <label className="labelForm mb-0 voda-bold">
                      System Solution<span className="red fz-20">*</span>
                    </label>
                    <Select
                      name="systemTypeId"
                      //cacheOptions
                      //defaultOptions={formData?.systemTypeResource}
                      options={
                        formData && formData.systemTypeResource
                          ? formData.systemTypeResource
                          : undefined
                      }
                      value={formData?.systemTypeResource!?.find(
                        (x) => x.key === formData?.systemTypeId
                      )}
                      // loadOptions={(x) =>
                      //   x && promiseSelect(x, searchSystemType)
                      // }
                      onChange={(e) => onChangeSystemSolution(e)}
                      onInputChange={setInputValue}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      menuPosition={"fixed"}
                      getOptionLabel={(option) => option.value!}
                      getOptionValue={(option) => option["key"]?.toString()!}
                      formatOptionLabel={function (data) {
                        return (
                          <span
                            dangerouslySetInnerHTML={{ __html: data.value! }}
                          />
                        );
                      }}
                    ></Select>
                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("systemTypeId") ? (
                      <label
                        className="validation"
                        style={{ bottom: "inherit", left: "15px" }}
                      >
                        *System Type must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
            </>
          )}

          {props?.wizardMode && (
            <div className="col-6">
              <div className="form-group">
                <label className="labelForm mb-0 w-100 d-flex flex-column">
                  <label className="labelForm voda-bold">System Solution</label>
                  <label
                    className="labelForm w-100"
                    dangerouslySetInnerHTML={{
                      __html:
                        props.systemSolution != undefined
                          ? props?.systemSolution
                              .replace(/ on /g, "\u00a0on\u00a0")
                              .replace(/ with /g, "\u00a0with\u00a0")
                          : "",
                    }}
                  ></label>
                </label>
              </div>
            </div>
          )}

          <div className="col-6 mb-2">
            <div className="row pl-50">
              {!showSupportedAllService && systemSolutionIsChanged
                ? // <div className="col-12 form-group mt-44 text-left">
                  //   <Form.Check
                  //     type="checkbox"
                  //     label="Supports All Services"
                  //     id="supportedServiceBoundary"
                  //     value="supportedServiceBoundary"
                  //     onChange={onChangeSupportedAllService}
                  //     checked={formData?.supportedAllServices ? true : false}
                  //   />
                  // </div>
                  null
                : null}

              {selected?.length && checkInsertBtn ? (
                <div className="col-12 form-group text-left">
                  <label className="labelForm voda-bold mb-0 w-100">
                    SubNetwork Boundary
                    {selected.map((item, idx) => (
                      <p key={idx} className="text-left regularFont">
                        {idx + 1} - {item.label}
                      </p>
                    ))}
                    {/* <div className="d-flex">
                    <Select
                      className="w-100"
                      options={
                        formData?.subNetworkBoundaryResource &&
                        dictionaryToArray(formData?.subNetworkBoundaryResource)
                      }
                      value={
                        formData?.subNetworkBoundaryResource &&
                        dictionaryToArray(
                          formData?.subNetworkBoundaryResource
                        ).filter((x) => {
                          return (
                            formData &&
                            formData?.subNetworkBoundaryIds?.indexOf(x.key) !=
                              -1 &&
                            formData?.subNetworkBoundaryIds?.indexOf(x.key) !=
                              undefined
                          );
                        })
                      }
                      onChange={(e) =>
                        onChangeSelect("subNetworkBoundaryIds", e)
                      }
                      onBlur={() => setInputValue("")}
                      isSearchable
                      isClearable
                      isDisabled
                      isMulti={props.edit ? false : true}
                      getOptionLabel={(option) => option.value}
                      getOptionValue={(option) => option.key.toString()}
                    />
                  </div> */}
                  </label>
                </div>
              ) : null}
            </div>
          </div>

          {(vodafoneName || props?.vodafoneName) && (
            <div className="col-6 pr-0">
              <div className="form-group col-12 pl-0">
                <label className="labelForm voda-bold   w-100">
                  Vodafone Name
                  <input
                    readOnly={true}
                    className="inputForm w-100 voda-regular"
                    type="text"
                    value={(vodafoneName || props.vodafoneName)?.toUpperCase()}
                  />
                </label>
              </div>
            </div>
          )}

          {systemSolutionIsChanged && !checkInsertBtn && (
            <>
              <fieldset className="col-12">
                <div className="form-group">
                  <h6 className="labelForm voda-bold designC-title mt-2">
                    The same system solution can exist in multiple subnetworks
                    within a single Opco. This may be driven by wanting to force
                    service separation or because of different licensing or
                    resilience requirements for example. We call this the
                    boundary of the subnetwork. This particular system solution
                    is already linked to the following subnetwork designations.
                  </h6>
                  <div className="w-100">
                    <table className="w-100" style={{ minHeight: "inherit" }}>
                      <thead>
                        <tr className="intestazione">
                          <th className="pl-2" style={{ height: "35px" }}>
                            <label className="mb-0">SubNetwork Boundary</label>
                          </th>

                          <th
                            className="smallColumn"
                            style={{ height: "35px" }}
                          ></th>
                        </tr>
                      </thead>
                      <tbody>
                        {usedSubnetworkBoundary != undefined &&
                        usedSubnetworkBoundary?.length > 0 ? (
                          usedSubnetworkBoundary.map((x, i) => (
                            <tr
                              className="dati"
                              key={`${x.subNetworkBoundaryId}-${x.systemTypeId}-${i}`}
                            >
                              <td>
                                <label className="mb-0">
                                  {x.subNetworkBoundaryName}
                                </label>
                              </td>

                              <td className="smallColumn text-center">
                                {/* {props.edit && (
                                <input
                                  className="checkStyle"
                                  type="checkbox"
                                  name={`systemAndServiceBoundary${i}`}
                                  value={x.subNetworkBoundaryId}
                                  checked={
                                    serviceBoundarySelectedFromCheckBox[0] ===
                                      x.subNetworkBoundaryId &&
                                    systemSolutionSelected === x.systemTypeId
                                  }
                                  onChange={(e) => {
                                    chnageSingleSelectedCheckBox(
                                      e.target.checked
                                        ? x.subNetworkBoundaryId
                                        : undefined
                                    );
                                    setSystemSolutionSelected(
                                      e.target.checked
                                        ? x.systemTypeId
                                        : undefined
                                    );
                                  }}
                                ></input>
                              )}
                              {!props.edit && (
                                <input
                                  className="checkStyle"
                                  type="checkbox"
                                  name={`systemAndServiceBoundary${i}`}
                                  value={x.subNetworkBoundaryId}
                                  onChange={(e) => {
                                    onChangeCheckBoxServiceBoundary(
                                      e.target.checked,
                                      x.subNetworkBoundaryId ?? 0
                                    );

                                    setSystemSolutionSelected(
                                      e.target.checked
                                        ? x.systemTypeId
                                        : undefined
                                    );
                                  }}
                                ></input>
                              )} */}
                              </td>
                            </tr>
                          ))
                        ) : (
                          <tr className="dati">
                            <td>There is no Similar System Solutions</td>
                            <td>There is no Alias</td>
                          </tr>
                        )}
                      </tbody>
                    </table>
                  </div>
                  <div className="w-100 mt-3">
                    <h6 className="labelForm w-100 voda-bold mt-50 fz-16">
                      If it is required to link this system solution to a new
                      subnetwork designation, please select from the list below
                      and use 'Insert'. (If a suitable designation is not
                      already defined you will need to open the Subnetwork table
                      and create a New Subnetwork Boundary).{" "}
                      <span className="red fz-20">*</span>
                    </h6>
                    <div className="row">
                      <div className="col-6">
                        <MultiSelect
                          className="w-100 multiSelect"
                          options={
                            unUsedSubNetworkBoundaries &&
                            unUsedSubNetworkBoundaries.map((item) => ({
                              value: item?.subNetworkBoundaryId ?? "",
                              label:
                                item?.subNetworkBoundaryName! ??
                                item?.subNetworkBoundaryAlias!,
                            }))
                          }
                          value={selected ?? []}
                          onChange={setSelected}
                          labelledBy="Select"
                        />
                      </div>
                      <div className="col-6">
                        {/* <button
                            disabled={
                              !serviceBoundarySelected?.length
                              //!serviceBoundarySelectedFromCheckBox.length
                            }
                            className="btn-danger defBtn mt-5 br-0"
                            type="button"
                            onClick={() => {
                              let uniqueArray = [
                                ...serviceBoundarySelected!,
                                ...serviceBoundarySelectedFromCheckBox,
                              ]?.filter(function (item, pos) {
                                return (
                                  [
                                    ...serviceBoundarySelected!,
                                    ...serviceBoundarySelectedFromCheckBox,
                                  ] &&
                                  [
                                    ...serviceBoundarySelected!,
                                    ...serviceBoundarySelectedFromCheckBox,
                                  ].indexOf(item) == pos
                                );
                              });
                              insertServiceBoundary(uniqueArray ?? [],[]);
                            }}
                          >
                            Insert
                          </button> */}
                        <span
                          className="pointer openSubnetworkBoundary-btn fz-15"
                          onClick={() => setIsVisibleModalLookup(1)}
                        >
                          Open Subnetwork Table
                        </span>
                      </div>
                    </div>

                    {validation &&
                    validation.response === false &&
                    validation.property?.includes("subNetworkBoundaryIds") &&
                    (!selected || selected.length === 0) ? (
                      <label
                        className="validation"
                        style={{ bottom: "-12px", left: "15px" }}
                      >
                        *SubNetwork Boundary must have a value
                      </label>
                    ) : null}
                    {/* <div className="d-flex align-items-center">
                    <div className="col-12 px-0 maxW-96">
                      <div className="d-flex">
                        <Select
                          className="w-100"
                          options={serviceBoundaryFiltered()}
                          value={serviceBoundaryFiltered().filter(
                            (el) => el.key === serviceBoundarySelected
                          )}
                          onChange={(e) => changeSelectedFromSelect(e)}
                          onBlur={() => setInputValue("")}
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option.key.toString()}
                        />
                      </div>
                    </div>
                    <span
                      onClick={() =>
                        setIsVisibleServiceBoundaryModalLookup(true)
                      }
                    >
                      <img
                        src={require("../../img/plus_btn_icon.png")}
                        alt=""
                        title=""
                        className="mrl-5"
                      />
                    </span>
                   
                  </div> */}
                  </div>
                </div>
              </fieldset>
            </>
          )}
          {supportedServiceList?.length > 0 && (
            <div className="w-100 mt-44">
              <p className="mb-3">Supported Service</p>
              <Select
                // menuPosition={"fixed"}
                className="w-100"
                options={supportedServiceList}
                value={supportedServiceList}
                isSearchable
                isClearable
                isDisabled
                isMulti={true}
                getOptionLabel={(option) => option.value}
                getOptionValue={(option) => option.key.toString()}
              />
            </div>
          )}
          {/* <Container show={!props.wizardMode && serviceBoundaryIsChanged}>
            <fieldset className="col-12">
              <div className="form-group">
                <label className="labelForm  mb-0 w-100 d-flex flex-column ">
                  <h6 className="labelForm voda-bold  ">
                    Changing SubNetwork Boundary will impact the following:
                  </h6>
                  <div className="w-100">
                    <Tabs
                      defaultActiveKey="hardware"
                      id="report"
                      activeKey={keyTabs}
                      onSelect={(x) => setKey(x || "")}
                    >
                      <Tab eventKey="lcm" title="LCM Engineering">
                        <div className="w-100">
                          <h6 className="voda-bold mt-3">
                            The Previous SubNetwork Boundary was used in:
                          </h6>
                          <table
                            className="w-100"
                            style={{ minHeight: "inherit" }}
                          >
                            <thead>
                              <tr className="intestazione  ">
                                <th className="pl-2">
                                  <label className="mb-0">Opco</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Design Component
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Operational Contact
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">N° of Nodes</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    N° of Nodes in Lab
                                  </label>
                                </th>
                              </tr>
                            </thead>
                            <tbody>
                              {impactServiceBoundaryChanged?.engineeringDtoGrids !=
                                undefined &&
                              impactServiceBoundaryChanged?.engineeringDtoGrids
                                .length > 0 ? (
                                impactServiceBoundaryChanged?.engineeringDtoGrids.map(
                                  (x) => (
                                    <tr
                                      className="dati"
                                      key={x.lcmEngineeringId}
                                    >
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.opCo ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.designComponent ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.operationalContact ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.numberOfNodes?.toString() ??
                                              "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.numberOfNodesInLab?.toString() ??
                                              "---",
                                          }}
                                        ></label>
                                      </td>
                                    </tr>
                                  )
                                )
                              ) : (
                                <tr className="dati">
                                  <td colSpan={5}>
                                    There is no Impact in Lcm Engineering
                                  </td>
                                </tr>
                              )}
                            </tbody>
                          </table>
                        </div>
                      </Tab>
                      <Tab eventKey="capacity" title="Assets">
                        <div className="w-100">
                          <h6 className="voda-bold mt-3">
                            The Previous SubNetwork Boundary was used in:
                          </h6>
                          <table className="w-100">
                            <thead>
                              <tr className="intestazione  ">
                                <th className="pl-2">
                                  <label className="mb-0">Node Index</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">Opco</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Design Component
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">Element Name</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Network Construct
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">Environment</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Deployment Status
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">Location Type</label>
                                </th>
                              </tr>
                            </thead>
                            <tbody>
                              {impactServiceBoundaryChanged?.networkElementAsPlannedDtoGrids !=
                                undefined &&
                              impactServiceBoundaryChanged
                                ?.networkElementAsPlannedDtoGrids.length > 0 ? (
                                impactServiceBoundaryChanged?.networkElementAsPlannedDtoGrids.map(
                                  (x) => (
                                    <tr
                                      className="dati"
                                      key={x.networkElementAsPlannedId}
                                    >
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.networkElementAsPlannedId?.toString() ??
                                              "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.opCo ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.designComponent ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.elementName ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.networkConstruct ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.environment ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.deploymentStatus ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.deploymentType ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                    </tr>
                                  )
                                )
                              ) : (
                                <tr className="dati">
                                  <td colSpan={8}>
                                    There is no Impact in Capacity Engineering
                                  </td>
                                </tr>
                              )}
                            </tbody>
                          </table>
                        </div>
                      </Tab>
                      <Tab eventKey="planned" title="Planned Activities">
                        <div className="w-100">
                          <h6 className="voda-bold mt-3">
                            The Previous SubNetwork Boundary was used in:
                          </h6>
                          <table className="w-100">
                            <thead>
                              <tr className="intestazione  ">
                                <th className="pl-2">
                                  <label className="mb-0">Opco</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">Orig. DC</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">Planned DC</label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Implementation Year
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Planned Activity
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Activity Details
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Activity Status
                                  </label>
                                </th>
                                <th className="pl-2">
                                  <label className="mb-0">
                                    Delivery Status
                                  </label>
                                </th>
                              </tr>
                            </thead>
                            <tbody>
                              {impactServiceBoundaryChanged?.plannedActivityDtoGrids !=
                                undefined &&
                              impactServiceBoundaryChanged
                                ?.plannedActivityDtoGrids.length > 0 ? (
                                impactServiceBoundaryChanged?.plannedActivityDtoGrids.map(
                                  (x) => (
                                    <tr
                                      className="dati"
                                      key={x.plannedActivityId}
                                    >
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.opCo ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.originalDesignComponent ??
                                              "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.designComponentId ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.plannedImplementationYear?.toString() ??
                                              "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.plannedActivityResourceId ??
                                              "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              x.activityDetailsText ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.activityStatusId ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                      <td>
                                        <label
                                          className="mb-0"
                                          dangerouslySetInnerHTML={{
                                            __html: x.deliveryStatusId ?? "---",
                                          }}
                                        ></label>
                                      </td>
                                    </tr>
                                  )
                                )
                              ) : (
                                <tr className="dati">
                                  <td colSpan={8}>
                                    There is no Impact in Planned Activity
                                  </td>
                                </tr>
                              )}
                            </tbody>
                          </table>
                        </div>
                      </Tab>
                    </Tabs>
                  </div>
                </label>
              </div>
            </fieldset>
          </Container> */}

          {props?.edit === true ? (
            <div className="col-md-12 row mx-0 px-0 mt-50">
              <div className="col-md-6">
                <div className="form-group">
                  <label className="labelForm voda-bold   w-100">
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
              </div>
              <div className="col-md-6">
                <div className="form-group">
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
              </div>
            </div>
          ) : null}
        </div>
      </form>

      {!props?.wizardMode && (
        <div className="col-12 justify-content-end mt-4 d-flex footerModal">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() =>
              props.action.closeModal && props.action.closeModal(changed)
            }
            type="button"
          >
            Cancel
          </button>
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={() => {
              let copy = { ...formData } as DesignComponentDtoCreate;
              if (supportedAllServiceFlag) {
                copy.supportedAllServices = true;
                copy.subNetworkBoundaryIds =
                  copy.subNetworkBoundaryIds?.includes(-1)
                    ? copy.subNetworkBoundaryIds.filter((item) => item !== -1)
                    : copy.subNetworkBoundaryIds;
                setFormData(copy);
              }
              if (copy.subNetworkBoundaryIds !== undefined) {
                let uniqueArray = copy.subNetworkBoundaryIds?.filter(function (
                  item,
                  pos
                ) {
                  return (
                    copy.subNetworkBoundaryIds &&
                    copy.subNetworkBoundaryIds.indexOf(item) == pos
                  );
                });

                copy.subNetworkBoundaryIds = uniqueArray;
                setFormData(copy);
              }

              Save(
                copy,
                props?.edit,
                validazioneClient,
                refresh,
                RestoreOrphanDeleted,
                orphanDeleted
              );
            }}
            type="button"
          >
            Save
          </button>
        </div>
      )}

      {props.wizardMode && (
        <div className="col-12 d-flex justify-content-between py-4 mt-4 w-100">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader exit"
            type="button"
            onClick={() =>
              props.action.setConfirmExitWizard &&
              props.action.setConfirmExitWizard()
            }
          >
            Exit
          </button>
          <div className="">
            <button
              disabled={!(props.wizardStep && props.wizardStep > 1)}
              className="  voda-bold btn btn-link px-4 btnHeader cancel"
              type="button"
              onClick={() =>
                props.action.wizardBackFunction &&
                formData &&
                props.action.wizardBackFunction(formData, "designComponentDto")
              }
            >
              Back
            </button>
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              type="button"
              onClick={() => validateWizard()}
            >
              Continue
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default ModalDesignComponent;
