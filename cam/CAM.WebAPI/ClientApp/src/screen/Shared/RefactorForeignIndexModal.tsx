import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/wizard.css";
import {
  DataModalConfirm,
  stateConfirm,
  QueryObjectGrid,
} from "../../Model/Common";
import ModalConfirm from "../../Components/ModalConfirm";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";

import Select from "react-select";
import Container from "../../Components/Container";

import { ForeignIndexDto, FIGroupRecord } from "../../Model/ForeignIndexModel";
import {
  ApplyDataRefactoring,
  CancelForeignIndex,
  GetDesignComponentOrphans,
  GetGroupsForeignIndex,
  GetImpactCheckForeignIndex,
  GetSystemTypeOrphans,
} from "../../Redux/Action/ForeignIndex/ForeignIndexCommonAction";
import { useSelector } from "react-redux";
import {
  dictionaryToArray,
  dictionaryToArraySimilarityGroup,
} from "../../Hook/Dictionary";
import { listIsNullOrEmpty, stringIsNullOrEmpty } from "../../Hook/Common";
import { Modal, Tab, Tabs } from "react-bootstrap";
interface Props {
  action: {
    setIsVisibleModal(val: boolean): any;
    refreshGrid(): any;
  };
  entityType: number;
}

interface GroupSelected {
  similarity: FIGroupRecord[];
  charDiff: FIGroupRecord[];
  resource: { key: number; value: string }[];
}

const RefactorForeignIndexModal: React.FC<Props> = (props) => {
  // const [formData, setFormData] = useState<InizializeNewProductCreateDto>();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [isVisibleModalLegend, setIsVisibleModalLegend] =
    useState<boolean>(false);
  const [validation, setValidation] = useState<{
    response: boolean | null;
    property?: string[];
  } | null>(null);
  const [wizardStep, setWizardStep] = useState<number>(1);
  const [data, setData] = useState<ForeignIndexDto>();

  const [indexGroupSelected, setIndexGroupSelected] = useState<{
    preGroupIndex: number;
    groupIndex: number;
  }>();
  const [groupSelected, setGroupSelected] = useState<GroupSelected>({
    similarity: [],
    charDiff: [],
    resource: [],
  });
  const [customRecord, setCustomRecord] = useState<number>();

  const Grid = (state: RootState) => state.foreignIndexReducer.data;
  const ForeignDto = useSelector(Grid);

  const [keyTabs, setKey] = useState("lcm");

  //INIZIALIZZAZIONE
  useEffect(() => {
    setWizardStep(1);
  }, []);

  //SETUP DATA
  useEffect(() => {
    if (ForeignDto != null && ForeignDto != undefined) {
      setData(ForeignDto);
    }
  }, [ForeignDto]);

  //SELEZIONE----------------------------------------------------------------------------
  //#region

  const selectOrphan = (entity: string, checked: boolean, id: number) => {
    let copy = { ...data } as ForeignIndexDto;
    switch (entity) {
      case "designComponents":
        if (copy.designComponents != undefined) {
          let index = copy.designComponents.findIndex(
            (x) => x.designComponentId == id
          );
          if (index != -1) {
            copy.designComponents[index]["toDelete"] = checked;
            setData(copy);
          }
        }
        return;
      case "systemTypes":
        if (copy.systemTypes != undefined) {
          let index = copy.systemTypes.findIndex((x) => x.systemTypesId == id);
          if (index != -1) {
            copy.systemTypes[index]["toDelete"] = checked;
            setData(copy);
          }
        }
        return;
      default:
        return;
    }
  };

  const selectAllOrphans = (entity: string) => {
    let copy = { ...data } as ForeignIndexDto;
    if (copy[entity] != undefined) {
      let allSelected = copy[entity].filter((x) => !x.toDelete).length == 0;
      if (allSelected) {
        copy[entity].map((z) => {
          z.toDelete = false;
        });
      } else {
        copy[entity].map((z) => {
          z.toDelete = true;
        });
      }
      setData(copy);
    }
  };

  const selectCorrect = (id: number) => {
    let copy = { ...groupSelected } as GroupSelected;
    copy.similarity.map((x) => {
      if (x.key === id) {
        x.correct = true;
        x.selected = false;
      } else {
        x.correct = false;
      }
    });
    setGroupSelected(copy);
  };

  const selectDuplicate = (id: number, checked: boolean) => {
    let copy = { ...groupSelected } as GroupSelected;
    let index = copy.similarity.findIndex((x) => x.key === id);
    if (index != -1) {
      copy.similarity[index].selected = checked;
      copy.similarity[index].correct = false;
      setGroupSelected(copy);
    }
  };

  //#endregion

  //NAVIGAZIONE-------------------------------------------------------------------------------------------------------------------------------------
  //#region

  //PROCEDI STEP
  const nextStep = async () => {
    // 	rootStore.dispatch(setNotification({ message: "Check the fields entered", notifyType: NotifyType.warning }));

    switch (wizardStep) {
      case 1:
        if (data != null && data != undefined) {
          await GetSystemTypeOrphans(data).then((x) => {
            if (!x.warning) {
              setData(x.data);
              setWizardStep(2);
            }
          });
        }
        return;

      case 2:
        if (data != null && data != undefined) {
          let copy = { ...data } as ForeignIndexDto;
          if (copy.session) copy.session.source = props.entityType;
          await GetGroupsForeignIndex(copy).then((x) => {
            if (!x.warning) {
              setData(x.data);
              setWizardStep(3);
            }
          });
        }
        return;

      case 3:
        if (indexGroupSelected == undefined || indexGroupSelected == null) {
          rootStore.dispatch(
            setNotification({
              message: "Please select a Group of Duplicates",
              notifyType: NotifyType.warning,
            })
          );
          return;
        }
        if (
          data &&
          data.groups &&
          indexGroupSelected?.preGroupIndex != undefined
        ) {
          let preGroup = data.groups.filter(
            (x) =>
              x.similarityGroups != undefined && x.similarityGroups.length > 0
          )[indexGroupSelected?.preGroupIndex];
          if (preGroup.similarityGroups) {
            let similarity =
              preGroup.similarityGroups[indexGroupSelected?.groupIndex];
            let similArr = [] as FIGroupRecord[];
            dictionaryToArraySimilarityGroup(similarity).map((x) => {
              if (!x.value.correct) {
                x.value.selected = true;
              }
              similArr.push(x.value);
            });

            let charDiff =
              preGroup.charDiffGroups?.[indexGroupSelected?.groupIndex];
            let charArr = [] as FIGroupRecord[];

            if (charDiff != undefined) {
              dictionaryToArraySimilarityGroup(charDiff).map((x) => {
                charArr.push(x.value);
              });
            }

            if (preGroup.dropDownResource != undefined) {
              dictionaryToArray(preGroup.dropDownResource).map((x) => {
                let toAdd: FIGroupRecord = {
                  correct: false,
                  description: x?.value,
                  key: x?.key,
                  selected: true,
                  fromCharDiff: false,
                  fromResource: true,
                  count: 0,
                };
                charArr.push(toAdd);
              });
            }

            let toSelect = {
              similarity: similArr,
              charDiff: charArr,
              resource:
                preGroup.dropDownResource != undefined
                  ? dictionaryToArray(preGroup.dropDownResource)
                  : [],
            } as GroupSelected;

            setGroupSelected(toSelect);
            setWizardStep(4);
          }
        }

        return;

      case 4:
        if (
          listIsNullOrEmpty(
            groupSelected.similarity.filter((x) => x.correct == true)
          )
        ) {
          rootStore.dispatch(
            setNotification({
              message: "Please select a Correct Record",
              notifyType: NotifyType.warning,
            })
          );
          return;
        } else if (
          listIsNullOrEmpty(
            groupSelected.similarity.filter((x) => x.selected == true)
          )
        ) {
          rootStore.dispatch(
            setNotification({
              message: "Please select a Duplicate Record",
              notifyType: NotifyType.warning,
            })
          );
          return;
        } else {
          let copy = { ...data } as ForeignIndexDto;
          copy.groupToUpdate = groupSelected.similarity;

          await GetImpactCheckForeignIndex(copy).then((x) => {
            setData(x.data);
            setWizardStep(5);
          });
        }

        return;
      case 5:
        if (data != undefined) {
          const applyConfirm = {
            title: "Confirm",
            button: "Apply",
            message: "Are you sure you want to apply refactoring?",
            item: "",
            isOpen: true,
            actions: {
              cancel: () => setConfirm(stateConfirm),
              confirm: () => ApplyRefactoring(),
            },
          } as DataModalConfirm;
          setConfirm(applyConfirm);
        }
        return;
      default:
        return;
    }
  };

  //INDIETREGGIA
  const backFunction = async () => {
    switch (wizardStep) {
      case 2:
        await GetDesignComponentOrphans(data?.session?.sessionId).then((x) => {
          setWizardStep(1);
        });
        break;

      default:
        setWizardStep(wizardStep - 1);
        break;
    }
  };

  //#endregion

  //USCITA-----------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const setConfirmExit = (override?: boolean) => {
    const cancelConfirm = {
      title: "Exit",
      button: "Exit",
      message: "Are you sure you want to exit?, Unsaved changes will be lost.",
      item: "",
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: () => refreshGridOnExit(),
      },
    } as DataModalConfirm;
    if (override == true) {
      refreshGridOnExit();
    } else {
      setConfirm(cancelConfirm);
    }
  };

  const refreshGridOnExit = async () => {
    if (data && data != undefined) {
      await CancelForeignIndex(data);
    }
    props.action.refreshGrid();
    props.action.setIsVisibleModal(false);
  };

  //#endregion

  //APPLY-----------------------------------------------------------------------------------------------------------------------------------------------
  //#region
  const ApplyRefactoring = async () => {
    if (data != undefined) {
      await ApplyDataRefactoring(data).then((x) => {
        if (!x.warning) {
          props.action.refreshGrid();
          setConfirm(stateConfirm);
          props.action.setIsVisibleModal(false);
        }
      });
    }
  };

  //#endregion

  //RTN----------------------------------------------------------------------------------------------------------------------------
  //#region
  const rtnWizardTitle = (val: number) => {
    switch (val) {
      case 1:
        return {
          title: "Design Components",
          subtitle: "Select Orphans Design Components to delete",
        };
      case 2:
        return {
          title: "System Type Orphans",
          subtitle: "Select Orphans System Types to delete",
        };
      case 3:
        return {
          title: "Duplicate groups",
          subtitle: `Select the ${rtnEntityName()} group you want to edit`,
        };
      default:
        return { title: "", subtitle: "" };
    }
  };

  const rtnEntityName = () => {
    let entity = props.entityType;
    switch (entity) {
      case 1:
        return "Major Hardware Build";
      case 2:
        return "Major Software Build";
      case 3:
        return "Design Component Family";
      default:
        return "";
    }
  };

  //#endregion

  const onChangeCustomRecord = (e: any) => {
    if (e["key"] && e["key"] != undefined) {
      setCustomRecord(e["key"]);
    } else {
      setCustomRecord(undefined);
    }
  };

  const addCustomRecordFromSelect = () => {
    let copy = { ...groupSelected } as GroupSelected;
    let target = copy.resource.find((x) => x.key == customRecord);
    let toAdd: FIGroupRecord = {
      correct: false,
      description: target?.value,
      key: target?.key,
      selected: true,
      fromCharDiff: false,
      fromResource: true,
      count: 0,
    };

    if (copy.similarity.filter((x) => x.key === target?.key).length == 0) {
      copy.similarity.push(toAdd);
    }

    setGroupSelected(copy);
    setCustomRecord(undefined);
  };

  const addCustomRecordFromCharDiff = (id: number) => {
    let copy = { ...groupSelected } as GroupSelected;
    let target = copy.charDiff.find((x) => x.key == id);
    if (target != undefined) {
      let toAdd: FIGroupRecord = {
        correct: false,
        description: target.description,
        key: target.key,
        selected: true,
        fromCharDiff: true,
        fromResource: false,
        count: 0,
      };

      let toRemoveIndex = copy.charDiff.findIndex((x) => x.key == id);
      copy.similarity.push(toAdd);
      if (toRemoveIndex != -1) {
        copy.charDiff.splice(toRemoveIndex, 1);
      }
    }
    setGroupSelected(copy);
    setCustomRecord(undefined);
  };

  const removeCustomRecord = (id: number) => {
    let copy = { ...groupSelected } as GroupSelected;
    let target = copy.similarity.find((x) => x.key == id);
    if (target != undefined) {
      copy.charDiff.push(target);
    }

    let targetIndex = copy.similarity.findIndex((x) => x.key == id);
    if (targetIndex != -1) {
      copy.similarity.splice(targetIndex, 1);
    }

    setGroupSelected(copy);
    setCustomRecord(undefined);
  };

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 d-flex justify-content-center">
      <ModalConfirm data={confirm} />
      <Modal
        show={isVisibleModalLegend}
        backdrop="static"
        backdropClassName="backdropLookup"
        dialogClassName="dialogLookup"
        className="modalLookup"
        keyboard={false}
        size="lg"
        centered
      >
        <Modal.Header>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">{"Refactor Legend"}</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <div className="col-12 row mx-0">
            <div className="col-12 d-flex align-items-center greenLine">
              <label className="my-3">
                <b>Green line:</b> correct entry, no changes to be applied
              </label>
            </div>
            <div className="col-12 d-flex align-items-center blueLine">
              <label className="my-3">
                <b>Blue line:</b> final entry values after the "refactor"
              </label>
            </div>
            <div className="col-12 d-flex align-items-center yellowLine">
              <label className="my-3">
                <b>Yellow line:</b> starting entry values with attributes to be
                changed marked in red
              </label>
            </div>
            <div className="col-12 d-flex align-items-center redLine">
              <label className="my-3">
                <b>Red line:</b> duplicated entry, to be removed
              </label>
            </div>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <div className="d-flex justify-content-center align-items-center col-12 pb-3">
            <button
              className="  voda-bold btn btn-danger px-4 btnHeader"
              onClick={() => setIsVisibleModalLegend(false)}
              type="button"
            >
              Close
            </button>
          </div>
        </Modal.Footer>
      </Modal>
      <div className="col-12 d-flex row mx-0">
        <div className="col-md-12   d-flex flex-column row mx-0 pt-2">
          {/* TITOLO STEP */}
          <h4 className="voda-bold">{rtnWizardTitle(wizardStep).title}</h4>
          <label>{rtnWizardTitle(wizardStep).subtitle}</label>
        </div>
        <div
          className="col-12"
          style={{ maxHeight: "60vh", overflowY: "auto" }}
        >
          <Container show={wizardStep == 1}>
            {/*Design Components*/}
            <table className="table table-borderless table-responsive-md  w-100  ">
              <thead>
                <tr className="intestazione">
                  <th className="  pl-2">
                    <label>
                      <input
                        type="checkbox"
                        checked={
                          data?.designComponents?.filter((x) => !x.toDelete)
                            .length == 0
                        }
                        onChange={(e) => selectAllOrphans("designComponents")}
                      ></input>
                    </label>
                  </th>
                  <th className="  pl-2">
                    <label>Description</label>
                  </th>
                </tr>
              </thead>
              <tbody>
                {data?.designComponents
                  ?.sort((a, b) =>
                    (a.description ?? "").toLowerCase() <
                    (b.description ?? "").toLowerCase()
                      ? -1
                      : 1
                  )
                  .map((item, i) => (
                    <tr
                      className={`dati`}
                      key={item.designComponentId}
                      onClick={() =>
                        selectOrphan(
                          "designComponents",
                          !item.toDelete,
                          item.designComponentId ?? 0
                        )
                      }
                    >
                      <td className=" ">
                        <input
                          type="checkbox"
                          checked={item.toDelete}
                          onChange={(e) =>
                            selectOrphan(
                              "designComponents",
                              e.target.checked,
                              item.designComponentId ?? 0
                            )
                          }
                        ></input>
                      </td>
                      <td className=" ">
                        <label
                          className="labelForm   w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html: item.description ?? "",
                          }}
                        ></label>
                      </td>
                    </tr>
                  ))}
              </tbody>
            </table>
          </Container>
          <Container show={wizardStep == 2}>
            {/*System Type*/}
            <table className="table table-borderless table-responsive-md  w-100  ">
              <thead>
                <tr className="intestazione">
                  <th className="  pl-2">
                    <label>
                      <input
                        type="checkbox"
                        checked={
                          data?.systemTypes?.filter((x) => !x.toDelete)
                            .length == 0
                        }
                        onChange={(e) => selectAllOrphans("systemTypes")}
                      ></input>
                    </label>
                  </th>
                  <th className="  pl-2">
                    <label>Description</label>
                  </th>
                </tr>
              </thead>
              <tbody>
                {data?.systemTypes
                  ?.sort((a, b) =>
                    (a.description ?? "").toLowerCase() <
                    (b.description ?? "").toLowerCase()
                      ? -1
                      : 1
                  )
                  .map((item, i) => (
                    <tr
                      className={`dati`}
                      key={item.systemTypesId}
                      onClick={() =>
                        selectOrphan(
                          "systemTypes",
                          !item.toDelete,
                          item.systemTypesId ?? 0
                        )
                      }
                    >
                      <td className=" ">
                        <input
                          type="checkbox"
                          checked={item.toDelete}
                          onChange={(e) =>
                            selectOrphan(
                              "systemTypes",
                              e.target.checked,
                              item.systemTypesId ?? 0
                            )
                          }
                        ></input>
                      </td>
                      <td className=" ">
                        <label
                          className="labelForm   w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html: item.description ?? "",
                          }}
                        ></label>
                      </td>
                    </tr>
                  ))}
              </tbody>
            </table>
          </Container>
          <Container show={wizardStep == 3}>
            {/*SELECT GROUPS*/}
            <div className="col-12 px-0 row mx-0">
              {/* Pre gruppo */}
              {data?.groups
                ?.filter(
                  (x) =>
                    x.similarityGroups != undefined &&
                    x.similarityGroups.length > 0
                )
                .map((group, preGroupIndex) => (
                  <div className="w-100" key={`${preGroupIndex}`}>
                    {/* Gruppo */}
                    {group?.similarityGroups?.map((item, groupIndex) => (
                      <table
                        className="table table-bordered border-bottom my-3 w-100"
                        key={`${preGroupIndex}-${groupIndex}`}
                      >
                        <tbody>
                          <tr
                            onClick={() =>
                              setIndexGroupSelected({
                                preGroupIndex: preGroupIndex,
                                groupIndex: groupIndex,
                              })
                            }
                          >
                            <td
                              className="smallColumn"
                              rowSpan={
                                dictionaryToArraySimilarityGroup(item).length +
                                1
                              }
                            >
                              <input
                                type="checkbox"
                                checked={
                                  indexGroupSelected?.preGroupIndex ===
                                    preGroupIndex &&
                                  indexGroupSelected?.groupIndex === groupIndex
                                }
                                onChange={() =>
                                  setIndexGroupSelected({
                                    preGroupIndex: preGroupIndex,
                                    groupIndex: groupIndex,
                                  })
                                }
                              ></input>
                            </td>
                          </tr>
                          {/* Record */}
                          {dictionaryToArraySimilarityGroup(item).map(
                            (record, recordIndex) => (
                              <tr
                                className={`dati notHover`}
                                key={`${preGroupIndex}-${groupIndex}-${recordIndex}`}
                              >
                                <td className=" ">
                                  <label
                                    className="labelForm   w-100 mb-0"
                                    dangerouslySetInnerHTML={{
                                      __html: record.value.description ?? "",
                                    }}
                                  ></label>
                                </td>
                              </tr>
                            )
                          )}
                        </tbody>
                      </table>
                    ))}
                  </div>
                ))}
            </div>
          </Container>
          <Container show={wizardStep == 4}>
            {/*SELECT CORRECT*/}
            <div className="col-12 px-0 row mx-0">
              {/* <div className="w-100 d-flex justify-content-end mb-3">
								<div className="d-flex col-7 px-0 align-items-center">
									<div className="w-100">
										<Select
											options={groupSelected.resource}
											value={groupSelected.resource.filter(x => x.key == customRecord)}
											onChange={(e) => onChangeCustomRecord(e)}
											isSearchable
											isClearable
											isOptionDisabled={op => groupSelected.similarity.filter(x => x.key === op.key).length > 0}
											getOptionLabel={(option) => option.value}
											getOptionValue={(option) => option["key"].toString()}>
										</Select>
									</div>
									<button disabled={customRecord == undefined} style={{ height: "40px !important" }} className="  voda-bold btn btn-danger ml-2 px-2 btnHeader" onClick={() => addCustomRecordFromSelect()} type="button">
										ADD
									</button>
								</div>
							</div> */}
              <h4>Levenshtein Analysis</h4>
              <table className="table table-bordered border-bottom mb-3 w-100">
                <tbody>
                  {/* SIMILARITY */}
                  {groupSelected.similarity.map((record, recordIndex) => (
                    <tr
                      className={`dati notHover ${
                        record.correct
                          ? "correct"
                          : record.selected
                          ? "duplicate"
                          : ""
                      }`}
                      key={`${record.key}-${recordIndex}`}
                    >
                      <td className="mediumColumn">
                        <div className="d-flex flex-column">
                          <label className="mb-0">
                            <input
                              className="mr-1"
                              type="checkbox"
                              onChange={(e) => selectCorrect(record.key ?? 0)}
                              checked={record.correct}
                            ></input>
                            Correct
                          </label>
                          <label className="mb-0">
                            <input
                              className="mr-1"
                              type="checkbox"
                              onChange={(e) =>
                                selectDuplicate(
                                  record.key ?? 0,
                                  e.target.checked
                                )
                              }
                              checked={record.selected}
                            ></input>
                            Duplicate
                          </label>
                        </div>
                      </td>
                      <td className=" ">
                        <label
                          className="labelForm   w-100 mb-0"
                          dangerouslySetInnerHTML={{
                            __html: record.description ?? "",
                          }}
                        ></label>
                      </td>
                      <td className="  mediumColumn text-center">
                        {record.fromResource == true ||
                        record.fromCharDiff == true ? (
                          <button
                            type="button"
                            className="btn btn-link"
                            onClick={() => removeCustomRecord(record.key ?? 0)}
                          >
                            <img
                              className="btnEdit"
                              src={require("../../img/delete.png")}
                            />
                          </button>
                        ) : null}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>

              <h4>Other Results</h4>
              {groupSelected.charDiff.length > 0 ? (
                <table className="table table-bordered border-bottom mb-3 w-100">
                  <tbody>
                    {/* CHARDIFF */}
                    {groupSelected.charDiff.map((record, recordIndex) => (
                      <tr
                        className={`dati`}
                        key={`${record.key}-${recordIndex}`}
                      >
                        <td className=" ">
                          <label
                            className="labelForm   w-100 mb-0"
                            dangerouslySetInnerHTML={{
                              __html: record.description ?? "",
                            }}
                          ></label>
                        </td>
                        <td className="  mediumColumn text-center">
                          <button
                            type="button"
                            className="btn btn-link"
                            onClick={() =>
                              addCustomRecordFromCharDiff(record.key ?? 0)
                            }
                          >
                            <img
                              style={{ height: 20 }}
                              src={require("../../img/plus_B.png")}
                            />
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              ) : (
                <label className="w-100 my-2">
                  The analysis produced no results
                </label>
              )}
            </div>
          </Container>
          <Container show={wizardStep == 5}>
            {/*Impact Check*/}
            <div className="col-12 px-0 row mx-0 d-flex flex-column">
              <div className="d-flex justify-content-start align-items-center">
                <h4 className="">Impact Checks</h4>
                <button
                  className="btn btn-icon p-2"
                  type="button"
                  onClick={() => setIsVisibleModalLegend(true)}
                >
                  <img
                    className=""
                    alt={"Legend"}
                    title={"Legend"}
                    style={{ width: 25, height: 25, cursor: "pointer" }}
                    src={require("../../../src/img/info.png")}
                  />
                </button>
              </div>

              <Tabs
                defaultActiveKey={keyTabs}
                id="uncontrolled-tab-example"
                activeKey={keyTabs}
                onSelect={(x) => setKey(x || "")}
              >
                <Tab
                  eventKey="lcm"
                  title={`Lcm Engineerings (${
                    data?.impactChecks?.filter(
                      (x) => x.type == "Lcm" && !x.notToShow && !x.correct
                    )?.length ?? 0
                  })`}
                  className=""
                >
                  <div className="row mx-0 w-100">
                    {(data?.impactChecks?.filter(
                      (x) => x.type == "Lcm" && !x.notToShow
                    )?.length ?? 0) > 0 ? (
                      <table className="table table-bordered border-bottom mb-3 mt-2 w-100">
                        <tbody>
                          {data?.impactChecks
                            ?.filter(
                              (x) =>
                                x.type == "Lcm" &&
                                !stringIsNullOrEmpty(x.key) &&
                                !x.notToShow &&
                                (x.toDelete || x.correct)
                            )
                            ?.sort((a, b) =>
                              (a.key ?? "") < (b.key ?? "") ? -1 : 1
                            )
                            .sort((a, b) =>
                              a.correct && a.key == b.key ? -1 : 1
                            )
                            .map((impact, impactIndex) => (
                              <>
                                {!impact.correct ? (
                                  <tr
                                    className={`dati notHover original`}
                                    key={`${1} - ${impact.key}`}
                                  >
                                    <td className=" ">{impact.opco}</td>
                                    {impact.lcmNameNew?.map((x, i) => (
                                      <td
                                        className=" "
                                        key={`${1} - ${impact.key} - ${i}`}
                                      >
                                        <label
                                          className={`labelForm   w-100 mb-0`}
                                          dangerouslySetInnerHTML={{
                                            __html: x,
                                          }}
                                        ></label>
                                      </td>
                                    ))}
                                  </tr>
                                ) : null}
                                <tr
                                  className={`dati notHover ${
                                    impact.correct
                                      ? "correct"
                                      : impact.toDelete
                                      ? "duplicate"
                                      : !impact.correct && !impact.toDelete
                                      ? "toEdit"
                                      : ""
                                  }`}
                                  key={`${2} - ${impact.key}`}
                                >
                                  <td className=" ">{impact.opco}</td>
                                  {impact.correct
                                    ? impact.lcmNameNew?.map((x, i) => (
                                        <td
                                          className=" "
                                          key={`${2} - ${impact.key} - ${i}`}
                                        >
                                          <label
                                            className={`labelForm   w-100 mb-0 ${
                                              x != impact.lcmNameNew?.[i]
                                                ? "toCorrect"
                                                : ""
                                            }`}
                                            dangerouslySetInnerHTML={{
                                              __html: x,
                                            }}
                                          ></label>
                                        </td>
                                      ))
                                    : impact.lcmName?.map((x, i) => (
                                        <td
                                          className=" "
                                          key={`${2} - ${impact.key} - ${i}`}
                                        >
                                          <label
                                            className={`labelForm   w-100 mb-0 ${
                                              x != impact.lcmNameNew?.[i]
                                                ? "toCorrect"
                                                : ""
                                            }`}
                                            dangerouslySetInnerHTML={{
                                              __html: x,
                                            }}
                                          ></label>
                                        </td>
                                      ))}
                                </tr>
                                {(impact?.plannedActivity?.length ?? 0) > 0 ? (
                                  <td
                                    colSpan={9}
                                    key={`${impactIndex} - ${"planned"} - ${"key"}`}
                                  >
                                    <tr className="voda-bold">
                                      <th>
                                        <label className="mb-0 labelForm">
                                          OpCo
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Design Component
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          N° Nodes Prod
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          N° Nodes Lab
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Product Importance
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Planned Action
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Activity Index
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Edu Spoc
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Sub-Domain Spoc
                                        </label>
                                      </th>
                                    </tr>
                                    {impact.plannedActivity?.map((p, i) => (
                                      <tr
                                        key={`${impactIndex} - ${i} - ${p.activityIndex}`}
                                      >
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.opco ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.designComponent ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html:
                                                p.numberOfNodes?.toString() ??
                                                "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html:
                                                p.numberOfNodesInLab?.toString() ??
                                                "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.productImportance ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.plannedAction ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.activityIndex ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.eduspoc ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.subDomainSpoc ?? "",
                                            }}
                                          ></label>
                                        </td>
                                      </tr>
                                    ))}
                                  </td>
                                ) : null}

                                <tr
                                  className={""}
                                  key={`${impactIndex} - ${2}`}
                                >
                                  <td colSpan={9} className=" "></td>
                                </tr>
                                {!impact.correct ? (
                                  <tr
                                    className={""}
                                    key={`${impactIndex} - ${3}`}
                                  >
                                    <td colSpan={9} className=" "></td>
                                  </tr>
                                ) : null}
                              </>
                            ))}
                          {data?.impactChecks
                            ?.filter(
                              (x) =>
                                x.type == "Lcm" &&
                                stringIsNullOrEmpty(x.key) &&
                                !x.notToShow &&
                                !x.toDelete &&
                                !x.correct
                            )
                            ?.sort((a, b) =>
                              (a.opco ?? "") < (b.opco ?? "") ? -1 : 1
                            )
                            .map((impact, impactIndex) => (
                              <>
                                <tr
                                  className={`dati notHover original`}
                                  key={`${impactIndex} - ${0}`}
                                >
                                  <td className=" ">{impact.opco}</td>
                                  {impact.lcmNameNew?.map((x, i) => (
                                    <td
                                      className=" "
                                      key={`${impactIndex} - ${0} - ${i}`}
                                    >
                                      <label
                                        className={`labelForm   w-100 mb-0`}
                                        dangerouslySetInnerHTML={{ __html: x }}
                                      ></label>
                                    </td>
                                  ))}
                                </tr>
                                <tr
                                  className={`dati notHover ${
                                    impact.correct
                                      ? "correct"
                                      : impact.toDelete
                                      ? "duplicate"
                                      : !impact.correct && !impact.toDelete
                                      ? "toEdit"
                                      : ""
                                  }`}
                                  key={`${impactIndex} - ${1}`}
                                >
                                  <td className=" ">{impact.opco}</td>
                                  {impact.lcmName?.map((x, i) => (
                                    <td
                                      className=" "
                                      key={`${impactIndex} - ${1} - ${i}`}
                                    >
                                      <label
                                        className={`labelForm   w-100 mb-0 ${
                                          x != impact.lcmNameNew?.[i]
                                            ? "toCorrect"
                                            : ""
                                        }`}
                                        dangerouslySetInnerHTML={{ __html: x }}
                                      ></label>
                                    </td>
                                  ))}
                                </tr>

                                <tr
                                  className={""}
                                  key={`${impactIndex} - ${4}`}
                                >
                                  <td colSpan={9} className=" "></td>
                                </tr>
                              </>
                            ))}
                        </tbody>
                      </table>
                    ) : (
                      <label className="w-100 my-2">
                        The selected analysis did not impact Lcm Engineering
                        Records
                      </label>
                    )}
                  </div>
                </Tab>
                <Tab
                  eventKey="networkElement"
                  title={`Network Elements (${
                    data?.impactChecks?.filter(
                      (x) => x.type != "Lcm" && !x.notToShow && !x.correct
                    )?.length ?? 0
                  })`}
                  className=""
                >
                  <div className="row mx-0 w-100">
                    {(data?.impactChecks?.filter(
                      (x) => x.type != "Lcm" && !x.notToShow
                    )?.length ?? 0) > 0 ? (
                      <table className="table table-bordered border-bottom mb-3 mt-2 w-100">
                        <tbody>
                          {data?.impactChecks
                            ?.filter(
                              (x) =>
                                x.type != "Lcm" &&
                                !stringIsNullOrEmpty(x.key) &&
                                !x.notToShow &&
                                (x.toDelete || x.correct)
                            )
                            ?.sort((a, b) =>
                              (a.key ?? "") < (b.key ?? "") ? -1 : 1
                            )
                            .sort((a, b) =>
                              a.correct && a.key == b.key ? -1 : 1
                            )
                            .map((impact, impactIndex) => (
                              <>
                                {!impact.correct ? (
                                  <tr
                                    className={`dati notHover original`}
                                    key={`${1} - ${impact.key}`}
                                  >
                                    <td className=" ">{impact.opco}</td>
                                    {impact.lcmNameNew?.map((x, i) => (
                                      <td
                                        className=" "
                                        key={`${1} - ${impact.key} - ${i}`}
                                      >
                                        <label
                                          className={`labelForm   w-100 mb-0`}
                                          dangerouslySetInnerHTML={{
                                            __html: x,
                                          }}
                                        ></label>
                                      </td>
                                    ))}
                                  </tr>
                                ) : null}
                                <tr
                                  className={`dati notHover ${
                                    impact.correct
                                      ? "correct"
                                      : impact.toDelete
                                      ? "duplicate"
                                      : !impact.correct && !impact.toDelete
                                      ? "toEdit"
                                      : ""
                                  }`}
                                  key={`${2} - ${impact.key}`}
                                >
                                  <td className=" ">{impact.opco}</td>
                                  {impact.correct
                                    ? impact.lcmNameNew?.map((x, i) => (
                                        <td
                                          className=" "
                                          key={`${2} - ${impact.key} - ${i}`}
                                        >
                                          <label
                                            className={`labelForm   w-100 mb-0 ${
                                              x != impact.lcmNameNew?.[i]
                                                ? "toCorrect"
                                                : ""
                                            }`}
                                            dangerouslySetInnerHTML={{
                                              __html: x,
                                            }}
                                          ></label>
                                        </td>
                                      ))
                                    : impact.lcmName?.map((x, i) => (
                                        <td
                                          className=" "
                                          key={`${2} - ${impact.key} - ${i}`}
                                        >
                                          <label
                                            className={`labelForm   w-100 mb-0 ${
                                              x != impact.lcmNameNew?.[i]
                                                ? "toCorrect"
                                                : ""
                                            }`}
                                            dangerouslySetInnerHTML={{
                                              __html: x,
                                            }}
                                          ></label>
                                        </td>
                                      ))}
                                </tr>
                                {(impact?.plannedActivity?.length ?? 0) > 0 ? (
                                  <td
                                    colSpan={9}
                                    key={`${impactIndex} - ${"planned"} - ${"key"}`}
                                  >
                                    <tr className="voda-bold">
                                      <th>
                                        <label className="mb-0 labelForm">
                                          OpCo
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Design Component
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          N° Nodes Prod
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          N° Nodes Lab
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Product Importance
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Planned Action
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Activity Index
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Edu Spoc
                                        </label>
                                      </th>
                                      <th>
                                        <label className="mb-0 labelForm">
                                          Sub-Domain Spoc
                                        </label>
                                      </th>
                                    </tr>
                                    {impact.plannedActivity?.map((p, i) => (
                                      <tr
                                        key={`${impactIndex} - ${i} - ${p.activityIndex}`}
                                      >
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.opco ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.designComponent ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html:
                                                p.numberOfNodes?.toString() ??
                                                "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html:
                                                p.numberOfNodesInLab?.toString() ??
                                                "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.productImportance ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.plannedAction ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.activityIndex ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.eduspoc ?? "",
                                            }}
                                          ></label>
                                        </td>
                                        <td>
                                          <label
                                            className={`labelForm   w-100 mb-0`}
                                            dangerouslySetInnerHTML={{
                                              __html: p.subDomainSpoc ?? "",
                                            }}
                                          ></label>
                                        </td>
                                      </tr>
                                    ))}
                                  </td>
                                ) : null}

                                <tr
                                  className={""}
                                  key={`${impactIndex} - ${2}`}
                                >
                                  <td colSpan={9} className=" "></td>
                                </tr>
                                {!impact.correct ? (
                                  <tr
                                    className={""}
                                    key={`${impactIndex} - ${3}`}
                                  >
                                    <td colSpan={9} className=" "></td>
                                  </tr>
                                ) : null}
                              </>
                            ))}
                          {data?.impactChecks
                            ?.filter(
                              (x) =>
                                x.type != "Lcm" &&
                                stringIsNullOrEmpty(x.key) &&
                                !x.notToShow &&
                                !x.toDelete &&
                                !x.correct
                            )
                            ?.sort((a, b) =>
                              (a.opco ?? "") < (b.opco ?? "") ? -1 : 1
                            )
                            .map((impact, impactIndex) => (
                              <>
                                <tr
                                  className={`dati notHover original`}
                                  key={`${impactIndex} - ${0}`}
                                >
                                  <td className=" ">{impact.opco}</td>
                                  {impact.lcmNameNew?.map((x, i) => (
                                    <td
                                      className=" "
                                      key={`${impactIndex} - ${0} - ${i}`}
                                    >
                                      <label
                                        className={`labelForm   w-100 mb-0`}
                                        dangerouslySetInnerHTML={{ __html: x }}
                                      ></label>
                                    </td>
                                  ))}
                                </tr>
                                <tr
                                  className={`dati notHover ${
                                    impact.correct
                                      ? "correct"
                                      : impact.toDelete
                                      ? "duplicate"
                                      : !impact.correct && !impact.toDelete
                                      ? "toEdit"
                                      : ""
                                  }`}
                                  key={`${impactIndex} - ${1}`}
                                >
                                  <td className=" ">{impact.opco}</td>
                                  {impact.lcmName?.map((x, i) => (
                                    <td
                                      className=" "
                                      key={`${impactIndex} - ${1} - ${i}`}
                                    >
                                      <label
                                        className={`labelForm   w-100 mb-0 ${
                                          x != impact.lcmNameNew?.[i]
                                            ? "toCorrect"
                                            : ""
                                        }`}
                                        dangerouslySetInnerHTML={{ __html: x }}
                                      ></label>
                                    </td>
                                  ))}
                                </tr>

                                <tr
                                  className={""}
                                  key={`${impactIndex} - ${4}`}
                                >
                                  <td colSpan={9} className=" "></td>
                                </tr>
                              </>
                            ))}
                        </tbody>
                      </table>
                    ) : (
                      <label className="w-100 my-2">
                        The selected analysis did not impact Network Elements
                        Records
                      </label>
                    )}
                  </div>
                </Tab>
              </Tabs>
            </div>
          </Container>
        </div>
        <Container show={true}>
          <div className="col-12 d-flex justify-content-between py-4 border-top mt-4">
            <button
              className="  voda-bold btn btn-link px-4 btnHeader cancel"
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
                onClick={() => backFunction()}
              >
                Back
              </button>
              <button
                className="  voda-bold btn btn-danger px-4 btnHeader"
                onClick={() => nextStep()}
                type="button"
              >
                Continue
              </button>
            </div>
          </div>
        </Container>
      </div>
    </div>
  );
};

export default RefactorForeignIndexModal;
