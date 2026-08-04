import { useState } from "react";
import {
  DataModalConfirm,
  IOverrideBehavior,
  stateConfirm,
} from "../Model/Common";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { rootStore } from "../Redux/Store/rootStore";
import { lowerFirstLetter } from "./Common";
import { useNavigate, useLocation } from "react-router-dom";

export function useFormTableCrud<FormObject>(
  CreateSaveFunction: Function,
  EditSaveFunction: Function
) {
  let behavior: IOverrideBehavior[] = [];
  const [changed, setChanged] = useState<boolean>(false);
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [confirmForm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [formData, setFormData] = useState<FormObject | null>();
  const [inputValue, setInputValue] = useState<string>("");
  const [showEditBtn, setShowEditBtn] = useState<boolean>(true);
  const [checkIsExist, setChecIsExist] = useState<boolean>(false);

  const navigate = useNavigate();
  const location: any = useLocation();

  //Save EDIT & NEW
  const Save = (
    formData,
    edit: boolean,
    validationFunction: Function,
    callbackFunctionOnSave: Function,
    callbackFunctionRestore?: Function,
    orphanDeleted?: boolean
  ) => {
    let copy = { ...formData };

    if (validationFunction(copy).response === true) {
      if (edit) {
        // Return the Promise so the caller can await it
        return EditSaveFunction(formData, orphanDeleted).then((x) => {
          if (location.pathname === "/designAspect") {
            sessionStorage.setItem("SPID", x?.ResultDtoEdit?.data);
          }
          if (
            x.ResultDtoEdit?.warning === false &&
            callbackFunctionOnSave !== undefined
          ) {
            callbackFunctionOnSave();
          } else if (x.ResultDtoEdit?.data?.orphanDeleted === false) {
            setConfirm({
              title: "Warning",
              message:
                "The entry specified already exists, use edit to modify data.",
              button: "View",
              item: x.ResultDtoEdit.data.id,
              isOpen: true,
              actions: {
                cancel: () => CancelConfirm(),
                confirm: () => {
                  callbackFunctionRestore &&
                    callbackFunctionRestore(
                      x.ResultDtoEdit.data.id,
                      x.ResultDtoEdit?.data.orphanDeleted
                    );
                  setConfirm(stateConfirm);
                },
              },
            });
          } else if (x.ResultDtoEdit?.data?.orphanDeleted === true) {
            if (
              location.pathname.trim().toLocaleLowerCase() ===
                "/lcmengineering" ||
              location.pathname.trim().toLocaleLowerCase() ===
                "/designAspect" ||
              location.pathname.trim().toLocaleLowerCase() === "/asplanned"
            ) {
              // setConfirm({
              //   title: "Warning",
              //   message:
              //     "One Planned Activity is duplicate to another & updates are ignored. Please double-check.",
              //   item: x.ResultDtoEdit.data.id,
              //   isOpen: true,
              //   actions: {
              //     cancel: () => CancelConfirm(),
              //     confirm: () => {
              //       EditSaveFunction(formData, true).then((x) => {
              //         if (
              //           x.ResultDtoEdit?.warning === false &&
              //           callbackFunctionOnSave !== undefined
              //         ) {
              //           callbackFunctionOnSave();
              //         }
              //       });
              //     },
              //   },
              // });
            } else {
              setConfirm({
                title: "Warning",
                message:
                  "The entry specified already exists and the updated data is currently deleted",
                button: "Edit Anyway",
                item: x.ResultDtoEdit.data.id,
                isOpen: true,
                actions: {
                  cancel: () => CancelConfirm(),
                  confirm: () => {
                    EditSaveFunction(formData, true).then((x) => {
                      if (
                        x.ResultDtoEdit?.warning === false &&
                        callbackFunctionOnSave !== undefined
                      ) {
                        callbackFunctionOnSave();
                      }
                    });
                  },
                },
              });
            }
          }
          return x?.ResultDtoEdit;
        });
      } else {
        CreateSaveFunction(formData, orphanDeleted).then((x) => {
          if (location.pathname === "/designAspect") {
            sessionStorage.setItem("SPID", x?.ResultDtoCreate?.data);
          }
          if (
            x.ResultDtoCreate?.warning === false &&
            callbackFunctionOnSave !== undefined
          ) {
            callbackFunctionOnSave();
          } else {
            setConfirm({
              title: "Warning",
              message:
                "The entry specified already exists, use edit to modify data.",
              button: "View",
              item: x.ResultDtoCreate?.data?.id ?? x.ResultDtoCreate?.data,
              isOpen: true,
              actions: {
                cancel: () => CancelConfirm(),
                confirm: () => {
                  callbackFunctionRestore &&
                    callbackFunctionRestore(
                      x.ResultDtoCreate?.data?.id ?? x?.ResultDtoCreate?.data,
                      x.ResultDtoCreate?.data.orphanDeleted
                    );
                  setConfirm(stateConfirm);
                },
              },
            });
          }
        });
      }
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Check the fields entered",
          notifyType: NotifyType.warning,
        })
      );
      return;
    }
  };

  const CancelConfirm = () => {
    setChecIsExist(true);
    setConfirm(stateConfirm);
  };

  const onChange = async (
    property: string,
    event: any,
    overrideBehavior:
      | IOverrideBehavior
      | IOverrideBehavior[]
      | undefined = undefined
  ) => {
    setChanged(true);

    var val: any = "";
    if (event.currentTarget.type === "checkbox")
      val = event.currentTarget.checked;
    else val = event.currentTarget.value;

    let copy = { ...formData } as FormObject;
    copy[lowerFirstLetter(property)] = val;
    if (overrideBehavior) {
      behavior = behavior.concat(overrideBehavior);
      behavior.forEach(async (item) => {
        if (
          item.overrideProperty !== undefined &&
          item.overrideProperty !== null
        ) {
          if (property === item.overrideProperty) {
            copy = await item.operation(copy);
          }
        } else {
          copy = await item.operation(copy);
        }
      });
    }

    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  //CHANGE DROPSELECT
  const onChangeSelect = (
    property: string,
    obj: any,
    overrideBehavior:
      | IOverrideBehavior
      | IOverrideBehavior[]
      | undefined = undefined
  ) => {
    setChanged(true);
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idx = copy.property.indexOf(property);
      copy.property.splice(idx, 1);
      setValidation(copy);
    }

    let copy = { ...formData } as FormObject;
    if (
      property === "onChangeSelect" ||
      property === "internetFacing" ||
      property === "pcisox" ||
      property === "securityElement" ||
      property === "c3C4" ||
      property === "countrySpecificCriticality" ||
      property === "missionCritical"
    ) {
      copy[lowerFirstLetter(property)] =
        obj && obj["key"] === 1 ? true : obj && obj["key"] === 0 ? false : null;
    } else if (property === "gdprClassificationValue") {
      copy[lowerFirstLetter(property)] = obj && obj["value"];
    } else if (property === "ruleforSuccessorPlannedActivityCreation") {
      copy[property] = obj && obj["value"] === "Yes" ? true : false;
    } else if (property === "successorPlannedActivityId") {
      copy[property] = obj && obj["value"];
    } else {
      copy[lowerFirstLetter(property)] = obj && obj["key"];
    }

    if (overrideBehavior) {
      behavior = behavior.concat(overrideBehavior);
      behavior.forEach(async (item) => {
        if (
          item.overrideProperty !== undefined &&
          item.overrideProperty !== null
        ) {
          if (property === item.overrideProperty) {
            copy = await item.operation(copy);
            setFormData(copy);
          }
        } else {
          copy = await item.operation(copy);
          setFormData(copy);
        }
      });
    }

    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangeMultipleSelect = (
    property: string,
    obj: any,
    overrideBehavior:
      | IOverrideBehavior
      | IOverrideBehavior[]
      | undefined = undefined,
    reset: boolean = false
  ) => {
    setChanged(true);
    setValidation(null);
    let copy = { ...formData } as FormObject;
    if (reset) {
      copy[lowerFirstLetter(property)] = [];
    }

    let array = [] as Array<number>;
    if (obj && obj.length > 0) {
      for (let i = 0; i < obj.length; i++) {
        array.push(obj[i].key);
      }
      copy[property] = array;
    } else {
      copy[property] = undefined;
    }
    // if (obj !== null && obj !== undefined) {
    //   console.log("key => ", obj["key"]);
    //   copy[lowerFirstLetter(property)].push(obj["key"]);
    // }

    if (overrideBehavior) {
      behavior = behavior.concat(overrideBehavior);
      behavior.forEach(async (item) => {
        if (
          item.overrideProperty !== undefined &&
          item.overrideProperty !== null
        ) {
          if (property === item.overrideProperty) {
            copy = await item.operation(copy);
          }
        } else {
          copy = await item.operation(copy);
        }
      });
    }

    setFormData(copy);
  };

  const promiseSelect = (x, functionToSearch: (x: string) => any) =>
    new Promise((resolve) => {
      setTimeout(() => {
        resolve(functionToSearch(inputValue));
      }, 1);
    });

  const onChangeDate = (
    property: string,
    newDate: Date | null,
    overrideBehavior:
      | IOverrideBehavior
      | IOverrideBehavior[]
      | undefined = undefined
  ) => {
    setChanged(true);
    let copy = { ...formData } as FormObject;
    if (property === "endOfMaintenance") {
      if (newDate !== null) {
        copy["eomStatus"] = 2;
      } else {
        copy["eomStatus"] = 1;
      }
    }

    if (newDate === null) {
      copy[lowerFirstLetter(property)] = null;
    } else {
      const DateString = `${newDate.getFullYear()}/${
        newDate.getMonth() + 1
      }/${newDate.getDate()}`;
      copy[lowerFirstLetter(property)] = DateString;
    }

    if (overrideBehavior) {
      behavior = behavior.concat(overrideBehavior);
      behavior.forEach(async (item) => {
        if (
          item.overrideProperty !== undefined &&
          item.overrideProperty !== null
        ) {
          if (property === item.overrideProperty) {
            copy = await item.operation(copy);
          }
        } else {
          copy = await item.operation(copy);
        }
      });
    }

    setFormData(copy);

    //Rimuovi Validazione
    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  return {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeSelect,
    setChanged,
    onChangeMultipleSelect,
    onChangeDate,
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
    checkIsExist,
  };
}
