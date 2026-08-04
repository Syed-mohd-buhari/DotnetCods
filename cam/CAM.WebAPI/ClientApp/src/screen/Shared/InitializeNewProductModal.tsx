import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/wizard.css";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import ModalConfirm from "../../Components/ModalConfirm";
import { MajorHardwareBuildMainSystemTypeDto } from "../../Model/SystemTypeModel";
import MajorSoftwareModal from "../MajorSoftwareBuild/MajorSoftwareModal";
import SystemTypeModal from "../SystemType/SystemTypeModal";
import { GetMajorSoftwareBuildCreateResource } from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCreateAction";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import MajorHardwareModal from "../MajorHardwareBuild/MajorHardwareModal";
import DesignComponentModal from "../DesignComponent/DesignComponentModal";
import ReviewNewProductModal from "./ReviewNewProductModal";
import { GetMajorHardwareBuildCreateResource } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildCreateAction";
import { GetMajorHardwareBuildGrid } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildGridAction";
import { useSelector } from "react-redux";
import Select from "react-select";
import { GetSystemTypeCreateResource } from "../../Redux/Action/SystemType/SystemTypeCreateAction";
import Container from "../../Components/Container";
import { GetMajorHardwareBuildEditResource } from "../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildEditAction";
import { GetDesignComponentCreateResource } from "../../Redux/Action/DesignComponent/DesignComponentCreateAction";
import { InizializeNewProductCreateDto } from "../../Model/InizializeNewProduct";
import { paginationQuery } from "../../Containers/MajorHardwareBuildContainer";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";

interface Props {
  action: {
    setIsVisibleModalInitializeNewProduct(val: boolean): any;
  };
}

const InizializeNewProductCreateDtoModal: React.FC<Props> = (props) => {
  const [formData, setFormData] = useState<InizializeNewProductCreateDto>();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [stepsNavigated, setStepsNavigated] = useState<number[]>([]);
  const [wizardStep, setWizardStep] = useState<number>(0);

  const [isBack, setISBack] = useState<boolean>(false);
  const [productName, setProductName] = useState<string>();

  const [dataCheckDcfExist, setDataCheckDcfExist] = useState<{
    mhOemId: number;
    swAppType: string;
    swOemId: number;
    sbId: number;
    platform: number;
  }>({ mhOemId: 0, swAppType: "", swOemId: 0, sbId: 0, platform: 0 });

  const steps = [1, 2, 3, 4, 5];

  //INIZIALIZZAZIONE
  useEffect(() => {
    setWizardStep(1);
    GetResourceMajorHardwareBuildList();
  }, []);

  const GetResourceMajorHardwareBuildList = async () => {
    await GetMajorHardwareBuildGrid(undefined, true).then((x) => {
      if (x && x != undefined) {
        let resource = x.map((s) => {
          return {
            key: s.majorHardwareBuildId,
            value:
              `${s.originalEquipmentManufacturer} - ${s.platform} - ${s.hardwareSolution} - ${s.hardwareType}` ??
              " ",
          } as { key: number; value: string };
        });
        setResourceMajorHardwareBuild(resource);
      }
    });
  };

  const RtnWizardClass = (val: number) => {
    if (wizardStep === val) {
      return "active";
    } else {
      return "";
    }
  };

  const RtnWizardTitle = (val: number) => {
    switch (val) {
      case 1:
        return {
          title: "Sw Details",
          subtitle: "",
        };
      case 2:
        return {
          title: "Hw Details",
          subtitle: "",
        };
      case 3:
        return { title: "System Details", subtitle: "" };
      case 4:
        return {
          title: "Design Component",
          subtitle: "",
        };
      case 5:
        return { title: "Final Review", subtitle: "" };
      default:
        return { title: "", subtitle: "" };
    }
  };

  const setDataCheck = (prop: string, val: number | string) => {
    let copy = { ...dataCheckDcfExist } as {
      mhOemId: number;
      swAppType: string;
      swOemId: number;
      sbId: number;
      platform: number;
    };
    copy[prop] = val;
    setDataCheckDcfExist(copy);
  };

  const handelOnGetSwType = (typeName: string): void => {
    setProductName(typeName);
  };

  const RtnWizardComponent = (val: number) => {
    switch (val) {
      case 1:
        return (
          <MajorSoftwareModal
            edit={false}
            keyTab={""}
            action={{
              validateFormWizard: validateFormStep,
              wizardBackFunction: backFunction,
              setConfirmExitWizard: setConfirmExit,
              setDataCheck,
            }}
            wizardMode={true}
            wizardStep={wizardStep}
            dataWizard={formData?.majorSoftwareBuildDto}
            onGetSwType={handelOnGetSwType}
          ></MajorSoftwareModal>
        );
      case 2:
        return (
          <MajorHardwareModal
            edit={false}
            keyTab={""}
            action={{
              validateFormWizard: validateFormStep,
              wizardBackFunction: backFunction,
              setConfirmExitWizard: setConfirmExit,
              setDataCheck,
            }}
            wizardMode={true}
            wizardStep={wizardStep}
            dataWizard={formData?.majorHardwareBuildDto}
          ></MajorHardwareModal>
        );
      case 3:
        return (
          <SystemTypeModal
            edit={false}
            keyTab={""}
            action={{
              validateFormWizard: validateFormStep,
              wizardBackFunction: backFunction,
              setConfirmExitWizard: setConfirmExit,
            }}
            wizardMode={true}
            wizardStep={wizardStep}
            dataWizard={formData?.systemTypeDto}
            majorHardwareDto={formData?.majorHardwareBuildDto}
            majorSoftwareDto={formData?.majorSoftwareBuildDto}
          ></SystemTypeModal>
        );
      case 4:
        return (
          <DesignComponentModal
            edit={false}
            DC="IN"
            keyTab={""}
            action={{
              validateFormWizard: validateFormStep,
              wizardBackFunction: backFunction,
              setConfirmExitWizard: setConfirmExit,
              setDataCheck,
            }}
            wizardMode={true}
            wizardStep={wizardStep}
            dataWizard={formData?.designComponentDto}
            systemSolution={formData?.systemTypeDto.systemSolution}
            destructuredData={{
              msOem:
                formData?.majorHardwareBuildDto.originalEquipmentManufacturerId,
              msst: formData?.systemTypeDto.vodafoneNameId,
              mhOem:
                formData?.majorSoftwareBuildDto.originalEquipmentManufacturerId,
            }}
            vodafoneName={formData?.systemTypeDto?.vodafoneName}
            dataCheckDcfExist={dataCheckDcfExist}
            isBack={isBack}
            productName={productName}
          ></DesignComponentModal>
        );
      case 5:
        return (
          <ReviewNewProductModal
            action={{
              wizardBackFunction: backFunctionReview,
              setConfirmExitWizard: setConfirmExit,
            }}
            dataWizard={formData ?? null}
            isBack={isBack}
          ></ReviewNewProductModal>
        );

      default:
        break;
    }
  };

  //CARICAMENTO RISORSE AL PRIMO AVVIO DELLO STEP
  useEffect(() => {
    let copy = [...stepsNavigated] as number[];
    switch (wizardStep) {
      case 1:
        if (!stepsNavigated.includes(1)) {
          GetMajorSoftware();
        }
        break;
      case 2:
        if (!stepsNavigated.includes(2)) {
          GetMajorHardwareResource();
        }
        break;

      case 3:
        if (!stepsNavigated.includes(3)) {
          GetSystemTypeResource();
        }
        break;

      case 4:
        if (!stepsNavigated.includes(4)) {
          GetDesignComponentResource();
        }
        break;

      default:
        break;
    }
    copy.push(wizardStep);
    setStepsNavigated(copy);
  }, [wizardStep]);

  //RISORSE
  const GetMajorSoftware = async () => {
    await GetMajorSoftwareBuildCreateResource().then((x) => {
      let copy = { ...formData } as InizializeNewProductCreateDto;
      if (x && x != null) {
        copy.majorSoftwareBuildDto = x;
        setFormData(copy);
      }
    });
  };

  const GetMajorHardwareResource = async () => {
    await GetMajorHardwareBuildCreateResource().then((x) => {
      if (x && x != undefined) {
        let copy = { ...formData } as InizializeNewProductCreateDto;
        copy.majorHardwareBuildDto = x;
        copy.majorHardwareBuildDto.majorHardwareId = 0;
        setFormData(copy);
      }
    });
  };

  const GetSystemTypeResource = async () => {
    await GetSystemTypeCreateResource().then((x) => {
      if (x && x != undefined) {
        let copy = { ...formData } as InizializeNewProductCreateDto;
        copy.systemTypeDto = x;
        if (
          copy.majorHardwareBuildDto?.majorHardwareId != undefined &&
          copy.majorHardwareBuildDto?.majorHardwareId != 0
        ) {
          let arr = [] as MajorHardwareBuildMainSystemTypeDto[];
          arr.push({
            majorHardwareBuildId: copy.majorHardwareBuildDto?.majorHardwareId,
            isMain: true,
          });
          copy.systemTypeDto.majorHardwareBuildId = arr;
        } else {
          let arr = [] as MajorHardwareBuildMainSystemTypeDto[];
          arr.push({ majorHardwareBuildId: 0, isMain: true });
          copy.systemTypeDto.majorHardwareBuildId = arr;
        }
        setFormData(copy);
      }
    });
  };

  const GetDesignComponentResource = async () => {
    await GetDesignComponentCreateResource().then((x) => {
      console.log("response on review => ", x);
      if (x && x != undefined) {
        let copy = { ...formData } as InizializeNewProductCreateDto;
        copy.designComponentDto = x;
        copy.designComponentDto.systemTypeId = 0;
        setFormData(copy);
      }
    });
  };

  //PROCEDI STEP
  const validateFormStep = (valid: boolean, data: any, property: string) => {
    if (valid) {
      let copy = { ...formData } as InizializeNewProductCreateDto;
      copy[property] = data;
      setFormData(copy);
      setWizardStep(wizardStep + 1);
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered",
          notifyType: NotifyType.warning,
        })
      );
    }
  };

  //INDIETREGGIA
  const backFunction = (data: any, property: string) => {
    let copy = { ...formData } as InizializeNewProductCreateDto;
    copy[property] = data;
    setFormData(copy);
    setWizardStep(wizardStep - 1);
    if (property === "designComponentDto") {
      setISBack(true);
    }
  };

  //BACK DALLO STEP REVIEW
  const backFunctionReview = (data: InizializeNewProductCreateDto) => {
    setFormData(data);
    setISBack(true);
    setWizardStep(wizardStep - 1);
  };

  const [resourceMajorHardwareBuild, setResourceMajorHardwareBuild] = useState<
    { key: number; value: string }[] | undefined
  >([]);
  const [forceAddMajorHardware, setForceAddMajorHardware] =
    useState<boolean>(false);

  const majorHardwareEditResource = useSelector(
    (state: RootState) =>
      state.majorHardwareBuildEditReducer.MajorHardwareBuildDtoEdit
  );

  useEffect(() => {
    let copy = { ...formData } as InizializeNewProductCreateDto;
    if (majorHardwareEditResource && majorHardwareEditResource != undefined) {
      copy.majorHardwareBuildDto = majorHardwareEditResource;
      setFormData(copy);
    }
  }, [majorHardwareEditResource]);

  const onChangeMajorMain = async (e: any, property: string) => {
    let copy = { ...formData } as InizializeNewProductCreateDto;
    if (e && e["key"]) {
      await GetMajorHardwareBuildEditResource(e["key"]).then((x) => {
        if (x && x != undefined) {
          copy.majorHardwareBuildDto = x;

          let copyCheck = { ...dataCheckDcfExist };
          copyCheck.mhOemId = x.originalEquipmentManufacturerId;
          copyCheck.platform = x.platformId;
          setDataCheckDcfExist(copyCheck);

          if (
            copy.majorHardwareBuildDto?.majorHardwareId &&
            copy.systemTypeDto
          ) {
            let arr = [] as MajorHardwareBuildMainSystemTypeDto[];
            arr.push({
              majorHardwareBuildId: copy.majorHardwareBuildDto?.majorHardwareId,
              isMain: true,
            });
            copy.systemTypeDto.majorHardwareBuildId = arr;
          }
        }
      });
    } else {
      await GetMajorHardwareBuildCreateResource().then((x) => {
        if (x && x != undefined) {
          copy.majorHardwareBuildDto = x;
          copy.majorHardwareBuildDto.majorHardwareId = 0;
        }
      });
      if (
        copy.systemTypeDto &&
        copy.systemTypeDto.majorHardwareBuildId?.length > 0
      ) {
        let index = copy.systemTypeDto.majorHardwareBuildId.findIndex(
          (x) => x.isMain == true
        );
        if (index != undefined && index != -1) {
          copy.systemTypeDto.majorHardwareBuildId[
            index
          ].majorHardwareBuildId = 0;
        } else {
          copy.systemTypeDto.majorHardwareBuildId =
            [] as MajorHardwareBuildMainSystemTypeDto[];
        }
      }
    }
    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(property);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }
  };

  const validateMajorHardware = (copy: InizializeNewProductCreateDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy.majorHardwareBuildDto?.majorHardwareId == null ||
      copy.majorHardwareBuildDto?.majorHardwareId == undefined ||
      copy.majorHardwareBuildDto?.majorHardwareId == 0
    ) {
      addInvalidProperty("majorHardwareId");
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered",
          notifyType: NotifyType.warning,
        })
      );
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  const confirmMajorHardware = async () => {
    let copy = { ...formData } as InizializeNewProductCreateDto;
    if (validateMajorHardware(copy).response) {
      if (copy.majorHardwareBuildDto?.majorHardwareId) {
        await GetMajorHardwareBuildEditResource(
          copy.majorHardwareBuildDto?.majorHardwareId
        ).then((x) => {
          if (x && x != undefined) {
            validateFormStep(
              validateMajorHardware(copy).response,
              x,
              "majorHardwareBuildDto"
            );
          }
        });
      }
    }
  };

  const EnableSelectMajorHardware = async () => {
    let copy = { ...formData } as InizializeNewProductCreateDto;
    await GetMajorHardwareBuildCreateResource().then((x) => {
      if (x && x != undefined) {
        copy.majorHardwareBuildDto = x;
        copy.majorHardwareBuildDto.majorHardwareId = 0;
      }
    });
    if (copy.systemTypeDto) {
      copy.systemTypeDto.majorHardwareBuildId =
        [] as MajorHardwareBuildMainSystemTypeDto[];
    }
    await GetMajorHardwareBuildCreateResource().then((x) => {
      if (x && x != undefined) {
        copy.majorHardwareBuildDto = x;
        copy.majorHardwareBuildDto.majorHardwareId = 0;
      }
    });

    setFormData(copy);
    setForceAddMajorHardware(false);
  };

  const AddNewMajorHardware = async () => {
    let copy = { ...formData } as InizializeNewProductCreateDto;
    await GetMajorHardwareBuildCreateResource().then((x) => {
      if (x && x != undefined) {
        copy.majorHardwareBuildDto = x;
        copy.majorHardwareBuildDto.majorHardwareId = 0;
      }
    });
    if (copy.systemTypeDto) {
      if (copy.systemTypeDto.majorHardwareBuildId?.length > 0) {
        let index = copy.systemTypeDto.majorHardwareBuildId.findIndex(
          (x) => x.isMain == true
        );
        if (index != undefined && index != -1) {
          copy.systemTypeDto.majorHardwareBuildId[
            index
          ].majorHardwareBuildId = 0;
        } else {
          copy.systemTypeDto.majorHardwareBuildId =
            [] as MajorHardwareBuildMainSystemTypeDto[];
        }
      } else {
        let arr = [] as MajorHardwareBuildMainSystemTypeDto[];
        arr.push({ majorHardwareBuildId: 0, isMain: true });
        copy.systemTypeDto.majorHardwareBuildId = arr;
      }
    }
    await GetMajorHardwareBuildCreateResource().then((x) => {
      if (x && x != undefined) {
        copy.majorHardwareBuildDto = x;
        copy.majorHardwareBuildDto.majorHardwareId = 0;
      }
    });
    setFormData(copy);
    setForceAddMajorHardware(true);
    setValidation(null);
  };

  const setConfirmExit = (override?: boolean) => {
    const cancelConfirm = {
      title: "Warning!",
      button: "Do Nothing",
      message:
        "Are you sure you want to cancel? If you cancel, your changes will be permanently lost.",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => refreshGridOnExit(),
        confirm: () => setConfirm(stateConfirm),
      },
    } as DataModalConfirm;
    if (override && override == true) {
      refreshGridOnExit();
    } else {
      setConfirm(cancelConfirm);
    }
  };

  const refreshGridOnExit = async () => {
    await GetMajorHardwareBuildGrid(paginationQuery).then((x) => {
      props.action.setIsVisibleModalInitializeNewProduct(false);
    });
  };

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 d-flex justify-content-center">
      <ModalConfirm data={confirm} />
      <div className="d-flex row">
        <div className="col-md-12 d-flex align-content-start row mx-0">
          {steps.map((x) => {
            return (
              <div
                className="row mx-0 col-md px-0 d-flex align-items-center justify-content-center align-content-start mb-2"
                key={x}
              >
                <div
                  className={`wizardNumerator d-flex align-items-center justify-content-center ${RtnWizardClass(
                    x
                  )}`}
                >
                  {x}
                </div>
                <div className="wizardName d-flex row mx-0 w-100 text-center">
                  <label className="labelForm w-100 mb-0 voda-bold">
                    {RtnWizardTitle(x).title}
                  </label>
                  <label className="subtitle mb-0 w-100">
                    {RtnWizardTitle(x).subtitle}
                  </label>
                </div>
              </div>
            );
          })}
        </div>
        <div className="col-md-12">
          {wizardStep != 2 ? (
            <div className="col-12 px-0 row mx-0">
              {/* <div className="col-12 d-flex justify-content-between">
                <h3 className="voda-bold">
                  {RtnWizardTitle(wizardStep).title}
                </h3>
              </div> */}
              {RtnWizardComponent(wizardStep)}
            </div>
          ) : (
            <div className="col-12 px-0 row mx-0">
              {/* <div className="col-12 d-flex justify-content-between">
                <h3 className="voda-bold">
                  {RtnWizardTitle(wizardStep).title}
                </h3>
              </div> */}
              <div className="col-12">
                <div className="col-12 row mx-0 mb-4">
                  <div className="col-6 p-0">
                    <label className="labelForm voda-bold w-100 mb-0">
                      Select an existing Hardware<span className="red">*</span>
                      <Select
                      menuPosition={"fixed"}
                        options={resourceMajorHardwareBuild}
                        value={
                          formData != undefined &&
                          formData.majorHardwareBuildDto != undefined &&
                          formData.majorHardwareBuildDto?.majorHardwareId !=
                            undefined &&
                          formData.majorHardwareBuildDto?.majorHardwareId !=
                            0 &&
                          resourceMajorHardwareBuild != undefined
                            ? resourceMajorHardwareBuild.find(
                                (x) =>
                                  x.key ==
                                  (formData.majorHardwareBuildDto
                                    ?.majorHardwareId ?? 0)
                              )
                            : null
                        }
                        onChange={(e) =>
                          onChangeMajorMain(e, "majorHardwareId")
                        }
                        isSearchable
                        isClearable
                        isDisabled={forceAddMajorHardware}
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
                    </label>
                  </div>
                  <div className="col-6 pl-0 d-flex align-items-end">
                    {forceAddMajorHardware ? (
                      <button
                        type="button"
                        className="mrl-10 voda-bold btn btn-danger"
                        onClick={() => EnableSelectMajorHardware()}
                      >
                        Select the HW
                      </button>
                    ) : (
                      <button
                        type="button"
                        className="voda-bold btn btn-danger mrl-10"
                        onClick={() => AddNewMajorHardware()}
                      >
                        Add new HW
                      </button>
                    )}
                  </div>
                  {!forceAddMajorHardware &&
                  validation &&
                  validation.response == false &&
                  validation.property?.includes("majorHardwareId") ? (
                    <label className="pl-3 pt-2 validation">
                      *You must select an hw or add a new one
                    </label>
                  ) : null}
                </div>
              </div>
              <Container show={!forceAddMajorHardware}>
                <div className="col-12 d-flex justify-content-between py-4 w-100">
                  <button
                    className="  voda-bold btn btn-link px-4 btnHeader exit"
                    type="button"
                    onClick={() => setConfirmExit()}
                  >
                    Exit
                  </button>
                  <div className="">
                    <button
                      disabled={wizardStep <= 1}
                      className="  voda-bold btn btn-link px-4 btnHeader cancel mr-3"
                      type="button"
                      onClick={() =>
                        backFunction(
                          formData?.majorHardwareBuildDto,
                          "majorHardwareBuildDto"
                        )
                      }
                    >
                      Back
                    </button>
                    <button
                      className="  voda-bold btn btn-danger px-4 btnHeader"
                      onClick={() => confirmMajorHardware()}
                      type="button"
                    >
                      Continue
                    </button>
                  </div>
                </div>
              </Container>
              <Container show={forceAddMajorHardware}>
                {RtnWizardComponent(wizardStep)}
              </Container>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default InizializeNewProductCreateDtoModal;
