import React, { useEffect, useRef, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import { BiSolidPencil } from "react-icons/bi";
import { BsCheck2Circle } from "react-icons/bs";
import {
  MdKeyboardArrowDown,
  MdKeyboardArrowLeft,
  MdKeyboardArrowRight,
  MdKeyboardArrowUp,
  MdOutlineCancel,
  MdClose,
} from "react-icons/md";
// ---------------------------------------------------------------------
// All react-bootstrap components (Accordion, Card, Dropdown, InputGroup,
// Modal, Form, Table) have been swapped for MUI equivalents. Icons stay
// on react-icons (imported above) rather than @mui/icons-material, per
// request, so the existing icon set/behavior (up/down arrows, pencil,
// check, cancel) carries over unchanged.
// ---------------------------------------------------------------------
import MuiAccordion from "@mui/material/Accordion";
import MuiAccordionSummary from "@mui/material/AccordionSummary";
import MuiAccordionDetails from "@mui/material/AccordionDetails";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import IconButton from "@mui/material/IconButton";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
// ---------------------------------------------------------------------
// Building blocks for the MUI "Transfer List" pattern
// (https://mui.com/material-ui/react-transfer-list/) that now drives the
// left "Attribute Selection" list and the middle Add/Remove controls.
// The right "Report Definition" list keeps its drag-and-drop reordering
// and rename/color features, but its row/header checkboxes are now MUI
// Checkbox components too, for a consistent look with the transfer list.
// ---------------------------------------------------------------------
import List from "@mui/material/List";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import Checkbox from "@mui/material/Checkbox";
import Stack from "@mui/material/Stack";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import setLoader from "../Redux/Action/LoaderAction";
import { rootStore } from "../Redux/Store/rootStore";
import {
  CreateGenericReport,
  GetGenericReport,
  PreviewReport,
} from "../Redux/Action/GenericReport/GenericReportCommonAction";
import {
  CreateGenericReportBody,
  GetGenericReportResponse,
  GenericReportAssociated,
  TableAndPropertiesResponse,
  genericPaginationQuery,
  GenericViewReportQueryObjectGrid,
} from "../Model/GenericReport";
import LabelsDictionary from "../Constant/LabelsAndDescriptions.json";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { RenderDetail } from "../Model/Common";
import { useLocation, useNavigate } from "react-router-dom";
import { dictionaryToArray } from "../Hook/Dictionary";
import {
  DropdownInputComponent,
  MultiSelectComponent,
  ToggleInputComponent,
} from "../Components/FormField";

const weekdayOptions = [
  { label: "Monday", value: "Monday" },
  { label: "Tuesday", value: "Tuesday" },
  { label: "Wednesday", value: "Wednesday" },
  { label: "Thursday", value: "Thursday" },
  { label: "Friday", value: "Friday" },
  { label: "Saturday", value: "Saturday" },
  { label: "Sunday", value: "Sunday" },
];

const columnColorOptions: Array<{
  key: string;
  label: string;
  swatchClass: string;
}> = [
  { key: "red", label: "Mandatory Field (Engineering)", swatchClass: "red" },
  { key: "green", label: "Mandatory Field (Operations)", swatchClass: "green" },
  { key: "gray", label: "Nice to have field", swatchClass: "gray" },
  { key: "blue", label: "Automatic Calculation", swatchClass: "blu" },
  { key: "white", label: "Default", swatchClass: "white" },
];

// Single source of truth for the "Report Definition" column widths so the
// header row and every data row always stay in sync (they previously
// diverged: header totalled 100% across 3 cells while each row rendered
// 4 cells totalling 125%).
const REPORT_COL_WIDTHS = {
  checkbox: "6%",
  columnName: "27%",
  customName: "52%",
  color: "15%",
};

const GenericReportContainer = () => {
  const location: any = useLocation();
  const navigate = useNavigate();
  const [isEdit, setIsEdit] = useState<boolean>(
    location?.state?.isEdit ?? false
  );
  const [tableList, setTableList] = useState<TableAndPropertiesResponse>();
  const [activeKey, setActiveKey] = useState(null);
  const [value, setValue] = useState("");
  const [valueId, setValueId] = useState("");
  const [isChecked, setIsChecked] = useState(false);
  const [selectedColumn, setSelectedColumn] = useState<
    GenericReportAssociated[]
  >([]);
  const [unselectedColumn, setUnselectedColumn] = useState<
    GenericReportAssociated[]
  >([]);
  const [startGenericReport, setStartGenericReport] = useState<any>([]);
  const [endGenericReport, setEndGenericReport] = useState<any>([]);
  const [matchedError, setMatchedError] = useState<boolean>(false);
  const [exportFilePath, setExportFilePath] = useState<any>(null);
  const [exportFileFormat, setExportFileFormat] = useState<any>(null);
  const [scheduledDate, setScheduledDate] = useState<any>(null);
  const [exportType, setExportType] = useState<any>(null);
  const [reportName, setReportName] = useState<string>("");
  const [isVisibleModal, setIsVisibleModal] = useState<boolean>(false);
  const [scheduleFlag, setScheduledFlag] = useState<boolean>(false);
  const [editReportName, setEditReportName] = useState<boolean>(
    location?.state?.isEdit ? true : false
  );
  const [errorField, setErrorField] = useState<any>([]);
  const [reportPublished, setReportPublished] = useState<boolean>(false);
  const [isTestNodeRequired, setIsTestNodeRequired] = useState<boolean>(false);
  const [opco, setOpco] = useState<any>([]);
  const [modalSnapshot, setModalSnapshot] = useState<any>(null);
  const [allOpCoResource, setAllOpCoResource] = useState<{
    [key: string]: any;
  }>([]);
  const [scheduleDayType, setScheduleDayType] = useState<{
    scheduledType: "1" | "2" | "";
  }>({
    scheduledType: "1",
  });

  // ---------------------------------------------------------------------
  // Top-level section open/close state for the two MUI Accordions that
  // replace the old outer <Accordion defaultActiveKey="0"><Accordion.Item>
  // wrappers ("Attribute Selection" and "Report Definition"). The
  // attribute selection panel used to default open (defaultActiveKey="0"),
  // and the report-definition panel defaulted closed, so that behaviour is
  // preserved here.
  // ---------------------------------------------------------------------
  const [attributeSectionOpen, setAttributeSectionOpen] =
    useState<boolean>(true);
  const [reportDefSectionOpen, setReportDefSectionOpen] =
    useState<boolean>(false);

  // ---------------------------------------------------------------------
  // MUI Menu (replacing react-bootstrap's Dropdown) needs an anchor
  // element plus a reference to which row's color menu is open, since a
  // single Menu instance is reused for whichever row's swatch was clicked.
  // ---------------------------------------------------------------------
  const [colorAnchorEl, setColorAnchorEl] = useState<null | HTMLElement>(null);
  const [colorMenuItem, setColorMenuItem] = useState<any>(null);

  const openColorMenu = (event: React.MouseEvent<HTMLElement>, item: any) => {
    setColorAnchorEl(event.currentTarget);
    setColorMenuItem(item);
  };

  const closeColorMenu = () => {
    setColorAnchorEl(null);
    setColorMenuItem(null);
  };

  // ---------------------------------------------------------------------
  // Persistent counter for "empty" columns so draggableId/propertyName is
  // always unique for the lifetime of the component, even after add/remove
  // cycles. Using array-length-at-click-time (the old approach) can
  // produce duplicate ids (e.g. two columns named "empty0"), which breaks
  // @hello-pangea/dnd (and react-beautiful-dnd before it) because every
  // Draggable in a list must have a unique id.
  // ---------------------------------------------------------------------
  const emptyColumnCounter = useRef(0);

  let fileFormat = [
    { key: "csv", value: "CSV" },
    { key: "excel", value: "Excel" },
  ];

  let exportFileType = [
    { key: "1", value: "Aggregated" },
    { key: "2", value: "Disaggregated" },
  ];

  useEffect(() => {
    setLoader("ADD", "GetGenericGetReportGrid");
    GetGenericReport().then((res: any) => {
      if (res?.data as GetGenericReportResponse) {
        setTableList(res?.data?.userReportTablesAndProperties);
        setAllOpCoResource(res?.data?.getAllOpcos);
        setLoader("REMOVE", "GetGenericGetReportGrid");
      }
    });
    if (location?.state?.isEdit) {
      setLoader("ADD", "GetGenericEditReportGrid");
      let copy = {
        ...genericPaginationQuery,
      } as GenericViewReportQueryObjectGrid;
      copy.dynamicReportId = [];
      copy.dynamicReportId?.push(location?.state?.reportId);
      PreviewReport(copy, "edit").then((x: any) => {
        if (x) {
          setReportName(x?.gridRender?.reportName ?? "");

          setScheduledDate(
            x?.gridRender?.scheduledType === 1 &&
              x?.gridRender?.scheduledDate !== null
              ? {
                  key: x?.gridRender?.scheduledDate,
                  value: x?.gridRender?.scheduledDate,
                }
              : x?.gridRender?.scheduledDayInWeek !== null
              ? {
                  key: x?.gridRender?.scheduledDayInWeek,
                  value: x?.gridRender?.scheduledDayInWeek,
                }
              : null
          );
          setExportFilePath(x?.gridRender?.exportFilePath ?? null);
          setScheduledFlag(x?.gridRender?.isExportReport ?? false);
          setScheduleDayType({
            scheduledType: x?.gridRender?.scheduledType?.toString(),
          });
          setExportFileFormat(
            x?.gridRender?.exportFileFormat
              ? fileFormat.filter(
                  (val) =>
                    val.value.toLowerCase() ===
                    x?.gridRender?.exportFileFormat?.toLowerCase()
                )[0]
              : null
          );
          setExportType(
            x?.gridRender?.exportType
              ? exportFileType.filter(
                  (val) => val.key == x?.gridRender?.exportType
                )[0]
              : null
          );
          setOpco(x?.gridRender?.opcoId ?? null);
          setReportPublished(x?.gridRender?.published ?? false);
          setIsTestNodeRequired(x?.gridRender?.isTestNodeRequired ?? false);
          let maxEmptyIndex = -1;
          x?.gridRender?.render?.map((x) => {
            if (x.show) {
              let obj = {
                ...x,
                updatedPropertyName:
                  x.propertyName === x.updatedPropertyName
                    ? ""
                    : x.updatedPropertyName,
                checked: false,
                fieldEdit: false,
              };
              endGenericReport?.push(obj);

              // Keep the empty-column counter ahead of any empty columns
              // that were already saved on this report, so newly added
              // ones never collide with existing ids.
              if (
                x.tableName === "emptyGrid" &&
                typeof x.propertyName === "string" &&
                x.propertyName.startsWith("empty")
              ) {
                const n = parseInt(x.propertyName.replace("empty", ""), 10);
                if (!isNaN(n) && n > maxEmptyIndex) {
                  maxEmptyIndex = n;
                }
              }
            }
          });
          emptyColumnCounter.current = maxEmptyIndex + 1;
          setLoader("REMOVE", "GetGenericEditReportGrid");
        }
      });
    }
  }, []);

  const toggleAccordian = (id) => {
    if (activeKey === id) {
      setActiveKey(null);
    } else setActiveKey(id);
  };

  const transfertAddRow = (list: GenericReportAssociated[], set: Function) => {
    let toAdd = [...startGenericReport] as GenericReportAssociated[];
    startGenericReport?.map((x) => {
      if (x.propertyName && list.includes(x.propertyName)) {
        toAdd.push(x);
      }
    });
    if (endGenericReport != undefined) {
      endGenericReport?.push(...list);
    } else {
      setEndGenericReport(toAdd);
    }
    set([]);
  };

  const transfertRemoveRow = (
    list: GenericReportAssociated[],
    set: Function
  ) => {
    setEndGenericReport(
      endGenericReport.filter(
        (obj1) => !list.some((obj2) => obj1.propertyName === obj2.propertyName)
      )
    );
    set([]);
  };

  const checkedColumn = (
    list: GenericReportAssociated[],
    set: Function,
    check: boolean,
    tableName: string,
    item: any,
    colName: string,
    type: string,
    colorHeader: string
  ) => {
    let copy = [...list];
    if (check) {
      let payload = {
        archive: item?.archive ?? false,
        colorHeader: colorHeader ?? "white",
        ignore: item?.ignore ?? false,
        order: null,
        show: false,
        tab: item?.tab ?? "",
        type: item?.type ?? 1,
        tableName: tableName,
        propertyName: colName,
        checked: type === "add" ? false : true,
        updatedPropertyName: "",
        fieldEdit: false,
      };
      copy.push(payload);
      let updateCol = [...endGenericReport];
      updateCol.map((x) => {
        if (x.propertyName == item?.propertyName)
          x.checked = type === "add" ? false : true;
      });
      setEndGenericReport(updateCol);
    } else {
      let index = copy.findIndex((x) =>
        tableName === "emptyGrid"
          ? x.propertyName === colName
          : x.propertyName == item?.propertyName
      );
      let updateCol = [...endGenericReport];
      updateCol.map((x) => {
        if (x.propertyName == item?.propertyName) x.checked = false;
      });
      setEndGenericReport(updateCol);
      if (index != -1) {
        copy.splice(index, 1);
      }
    }
    set(copy);
    return;
  };

  // ---------------------------------------------------------------------
  // Adds a new empty column with a permanently unique propertyName/id.
  // Replaces the old inline `empty${count}` computation, which derived the
  // id from the current array length and could produce duplicate ids
  // (breaking drag-and-drop) after add/remove cycles.
  // ---------------------------------------------------------------------
  const addEmptyColumn = () => {
    const id = `empty${emptyColumnCounter.current}`;
    emptyColumnCounter.current += 1;
    checkedColumn(
      selectedColumn,
      setSelectedColumn,
      true,
      "emptyGrid",
      null,
      id,
      "add",
      "white"
    );
  };

  // Function to handle the drag-and-drop reorder
  const onDragEnd = (result) => {
    if (!result.destination) return; // Not dropped in a valid droppable
    const updatedItems = [...endGenericReport];
    const [reorderedItem] = updatedItems.splice(result.source.index, 1);
    updatedItems.splice(result.destination.index, 0, reorderedItem);
    setEndGenericReport(updatedItems);
  };

  const onChangeHandle = (val, id) => {
    setValue(val);
    setValueId(id);
  };

  const submitValue = (val, id) => {
    const newstate = [...startGenericReport];
    newstate?.map((item) => {
      if (item.id === valueId) {
        item.newContent = value;
      } else if (item.id === id) {
        item.newContent = item.newContent
          ? item.newContent
          : item.content
          ? item.content
          : "";
      }
    });
    setStartGenericReport(newstate);
    setIsChecked(!isChecked);
    setTimeout(() => {
      setIsChecked(false);
      setValue("");
      setValueId("");
    }, 100);
  };

  const isDisabled = (val, key) => {
    if (
      selectedColumn.some(
        (x) => x?.propertyName === val && x?.tableName !== key
      ) ||
      endGenericReport.some((item) => item?.propertyName === val)
    )
      return true;
    else return false;
  };

  const isCheckable = (val, key) => {
    if (
      selectedColumn.some(
        (x) => x?.propertyName === val && x?.tableName === key
      ) ||
      endGenericReport.some(
        (item) => item?.propertyName === val && item?.tableName === key
      )
    )
      return true;
    else return false;
  };

  const onChangeEdit = (item) => {
    let updateCol = [...endGenericReport];

    updateCol.map((x) => {
      if (x.propertyName == item.propertyName) {
        x.fieldEdit = !x.fieldEdit;
        setValue(item.updatedPropertyName);
        setValueId(item.propertyName);
      } else x.fieldEdit = false;
    });
    setEndGenericReport(updateCol);
    setMatchedError(false);
  };

  const onChangeSave = (item) => {
    // check the entered value matching with any other updated column name if it matches throw an error
    const matched = endGenericReport.some(
      (x) =>
        item.propertyName !== x.propertyName &&
        x.updatedPropertyName.toLowerCase() === value.toLowerCase() &&
        value !== ""
    );
    if (!matched) {
      let updateCol = [...endGenericReport];

      updateCol.map((x) => {
        if (x.propertyName == valueId) {
          x.updatedPropertyName = value;
        }
      });

      setEndGenericReport(updateCol);
      setMatchedError(false);
      onChangeEdit(item);
    } else setMatchedError(true);
  };

  const onChangeCancel = (item) => {
    let updateCol = [...endGenericReport];

    updateCol.map((x) => {
      if (x.propertyName == item.propertyName) {
        x.updatedPropertyName = "";
      }
    });

    setEndGenericReport(updateCol);
    setMatchedError(false);
    setValue("");
    setValueId("");
  };

  const onReset = () => {
    setEndGenericReport([]);
    setSelectedColumn([]);
    setUnselectedColumn([]);
    setReportName("");
    setIsEdit(false);
    setEditReportName(false);
    setReportPublished(false);
    setIsTestNodeRequired(false);
    setOpco(null);
    location?.state?.isEdit && navigate("/genericreporting");
  };

  const onSaveReport = () => {
    setLoader("ADD", "SaveReport");
    if (reportName !== "") {
      let newList = [] as Array<RenderDetail>;
      endGenericReport.length > 0 &&
        endGenericReport.map((item, index) => {
          newList.push({
            order: index,
            archive: item?.archive,
            colorHeader: item?.colorHeader,
            tableName: item.tableName,
            propertyName: item.propertyName,
            updatedPropertyName: item.updatedPropertyName ?? item.propertyName,
            show: true,
            type: item.type,
            tab: item.tab ?? "",
            ignore: item.ignore,
          });
        });
      endGenericReport.length > 0 &&
        tableList &&
        Object.keys(tableList)?.map((key) => {
          tableList[key].map((col) => {
            if (
              !endGenericReport.some(
                (x) => x?.propertyName === col?.propertyName
              )
            ) {
              newList.push({
                order: -1,
                archive: col?.archive,
                colorHeader: "",
                tableName: key,
                propertyName: col?.propertyName,
                updatedPropertyName: "",
                show: false,
                type: col?.type ?? 1,
                tab: col?.tab ?? "",
                ignore: col?.ignore,
              });
            }
          });
        });
      let payload = {
        reportName: [reportName],
        published: [reportPublished],
        render: newList,
        dynamicReportId: [
          location?.state?.isEdit === true && location?.state?.reportId,
        ],
        tsrOpcoId: opco ?? null,
        exportFileFormat: exportFileFormat?.value ?? null,
        exportFilePath: exportFilePath ?? null,
        exportType: exportType?.key ?? null,
        scheduledType: scheduleDayType.scheduledType,
        isExportReport: scheduleFlag,
        isTestNodeRequired: isTestNodeRequired,
      } as unknown as CreateGenericReportBody;
      if (scheduleDayType.scheduledType === "1") {
        payload.scheduledDate = scheduledDate?.value ?? null;
      }
      if (scheduleDayType.scheduledType === "2") {
        payload.scheduledDayInWeek = scheduledDate?.value ?? null;
      }

      CreateGenericReport(payload)
        .then((res) => {
          rootStore.dispatch(
            setNotification({
              message: res?.info,
              notifyType: NotifyType.success,
            })
          );
          if (res) {
            onReset();
            navigate("/genericreports");
          }
        })
        .catch((err) => {
          setNotification({
            message: "Error Occurs !",
            notifyType: NotifyType.error,
          });
        });
    }
    setLoader("REMOVE", "SaveReport");
  };

  const selectAll = (check) => {
    let updateCol = [...endGenericReport];

    updateCol.map((x) => {
      x.checked = check;
    });

    setEndGenericReport(updateCol);
    if (check) setUnselectedColumn(updateCol);
    else setUnselectedColumn([]);
  };

  const changeColumnColor = (item, color) => {
    let updateCol = [...endGenericReport];
    updateCol.map((x) => {
      if (x.propertyName == item.propertyName) {
        x.colorHeader = color;
      }
    });
    setEndGenericReport(updateCol);
  };

  const handleOpCoSelect = (selected) => {
    setOpco(selected.map((val) => val.value));
  };
  const resetMoreOptionData = () => {
    setExportFileFormat(null);
    setExportFilePath(null);
    setExportType(null);
    setScheduledDate(null);
  };

  const handleModalCancel = () => {
    if (modalSnapshot) {
      setExportFileFormat(modalSnapshot.exportFileFormat);
      setExportFilePath(modalSnapshot.exportFilePath);
      setExportType(modalSnapshot.exportType);
      setScheduledDate(modalSnapshot.scheduledDate);
      setScheduledFlag(modalSnapshot.scheduleFlag);
      setScheduleDayType(modalSnapshot.scheduleDayType);
      setOpco(modalSnapshot.opco);
      setIsTestNodeRequired(modalSnapshot.isTestNodeRequired);
    }
    setErrorField([]);
    setIsVisibleModal(false);
  };

  const handleMoreOptions = () => {
    const errors: string[] = [];
    if (scheduleFlag) {
      if (exportFileFormat === null) {
        errors.push("exportFileFormat");
      }
      if (exportType === null) {
        errors.push("exportType");
      }
      if (scheduledDate === null) {
        errors.push("scheduledDate");
      }
      if (scheduledDate?.value === undefined) {
        errors.push("scheduledDate");
      }
      if (
        exportFileFormat !== null &&
        exportType !== null &&
        scheduledDate !== null
      ) {
        setIsVisibleModal(false);
      }
    } else {
      setIsVisibleModal(false);
    }

    setErrorField(errors);
  };

  // scheduled message formatter
  const scheduledMsgFormat = (val: string) => {
    const num = Number(val);
    if (num) {
      const suffixes = ["th", "st", "nd", "rd"];
      const v = num % 100;
      return num + (suffixes[(v - 20) % 10] || suffixes[v] || suffixes[0]);
    } else return "";
  };

  return (
    <div className="pageContainer">
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">New Report Definition</h3>
        </div>
        <div className="d-flex"></div>
      </div>
      <div className="d-flex justify-content-between">
        <div className="w-40">
          <MuiAccordion expanded={true} disableGutters square>
            <MuiAccordionSummary>
              <div className="w-100 d-flex justify-content-start voda-bold">
                <h5 className="mb-0">{"Attribute Selection"}</h5>
              </div>
            </MuiAccordionSummary>
            <MuiAccordionDetails className="px-0 py-4">
              <div className="colScroll">
                {tableList &&
                  Object.keys(tableList)?.map((key: any) => {
                    const isOpen = activeKey === key.toLowerCase();
                    const tableItems = tableList[key] ?? [];
                    return (
                      <MuiAccordion
                        key={key}
                        expanded={isOpen}
                        onChange={() => toggleAccordian(key.toLowerCase())}
                        className="my-2 mx-3 pointer colScrollCard"
                        disableGutters
                      >
                        <MuiAccordionSummary
                          expandIcon={
                            isOpen ? (
                              <MdKeyboardArrowDown />
                            ) : (
                              <MdKeyboardArrowUp />
                            )
                          }
                        >
                          <div className="w-100 d-flex align-items-center justify-content-between voda-bold">
                            <div className="d-flex align-items-center">
                              <span>{LabelsDictionary[key]?.Full ?? key} </span>
                            </div>
                          </div>
                        </MuiAccordionSummary>
                        <MuiAccordionDetails className="cardBody px-1 py-1">
                          <List dense component="div" role="list">
                            {tableItems.map((value) => {
                              const labelId = `transfer-list-item-${key}-${value?.propertyName}-label`;
                              const disabled = isDisabled(
                                value?.propertyName,
                                key
                              );
                              const checked = isCheckable(
                                value?.propertyName,
                                key
                              );
                              return (
                                <ListItemButton
                                  key={`${key}-${value?.propertyName}`}
                                  role="listitem"
                                  disabled={disabled}
                                  onClick={() =>
                                    key &&
                                    checkedColumn(
                                      selectedColumn,
                                      setSelectedColumn,
                                      !checked,
                                      key,
                                      value,
                                      value?.propertyName,
                                      "add",
                                      "white"
                                    )
                                  }
                                >
                                  <ListItemIcon>
                                    <Checkbox
                                      checked={checked}
                                      disabled={disabled}
                                      tabIndex={-1}
                                      disableRipple
                                      aria-labelledby={labelId}
                                    />
                                  </ListItemIcon>
                                  <ListItemText
                                    id={labelId}
                                    primary={
                                      LabelsDictionary[value?.propertyName]
                                        ?.Full ?? value?.propertyName
                                    }
                                  />
                                </ListItemButton>
                              );
                            })}
                          </List>
                        </MuiAccordionDetails>
                      </MuiAccordion>
                    );
                  })}
                <MuiAccordion
                  key={"emptyGrid"}
                  expanded={activeKey === "emptyGrid"}
                  onChange={() => toggleAccordian("emptyGrid")}
                  className="my-2 mx-3 pointer"
                  disableGutters
                >
                  <MuiAccordionSummary
                    expandIcon={
                      activeKey === "emptyGrid" ? (
                        <MdKeyboardArrowDown />
                      ) : (
                        <MdKeyboardArrowUp />
                      )
                    }
                  >
                    <div className="w-100 d-flex justify-content-between voda-bold">
                      <span>{"Add Empty Column"} </span>
                    </div>
                  </MuiAccordionSummary>
                  <MuiAccordionDetails className="cardBody px-1 py-1">
                    <List dense component="div" role="list">
                      <ListItemButton
                        role="listitem"
                        onClick={() => addEmptyColumn()}
                      >
                        <ListItemIcon>
                          <Checkbox
                            checked={selectedColumn.some(
                              (item) => item.tableName === "emptyGrid"
                            )}
                            tabIndex={-1}
                            disableRipple
                          />
                        </ListItemIcon>
                        <ListItemText primary={"Add a new blank column"} />
                      </ListItemButton>
                    </List>
                  </MuiAccordionDetails>
                </MuiAccordion>
              </div>
            </MuiAccordionDetails>
          </MuiAccordion>
        </div>
        <div className="align-self-center px-0 w-8rem">
          <Stack direction="column" spacing={1} sx={{ alignItems: "center" }}>
            {" "}
            <Button
              variant="outlined"
              size="small"
              className="mig-bt voda-bold"
              disabled={selectedColumn.length === 0}
              onClick={() => transfertAddRow(selectedColumn, setSelectedColumn)}
              aria-label="move selected right"
            >
              Add <MdKeyboardArrowRight />
            </Button>
            <Button
              variant="outlined"
              size="small"
              className="mig-bt voda-bold"
              disabled={unselectedColumn.length === 0}
              onClick={() =>
                transfertRemoveRow(unselectedColumn, setUnselectedColumn)
              }
              aria-label="move selected left"
            >
              <MdKeyboardArrowLeft /> Remove
            </Button>
          </Stack>
        </div>
        <div className="w-50">
          <MuiAccordion expanded={true} disableGutters square>
            <MuiAccordionSummary>
              <div className="w-100 d-flex justify-content-start voda-bold">
                <h5 className="mb-0">{"Report Definition"}</h5>
              </div>
            </MuiAccordionSummary>
            <MuiAccordionDetails className="py-3">
              <div className="w-100 headSticky">
                <div className="d-flex w-100">
                  <label
                    className={`labelForm voda-bold mb-0 ${
                      isEdit ? "w-95" : "w-100"
                    }`}
                  >
                    <input
                      type="text"
                      className={`inputForm form-control w-100 mt-0 ${
                        endGenericReport.length > 0
                          ? reportName !== ""
                            ? "is-valid"
                            : "is-invalid"
                          : ""
                      } border-radius-none ${
                        (endGenericReport.length === 0 ||
                          (isEdit && editReportName)) &&
                        "disabledCursor"
                      }`}
                      placeholder="*Add Report Name....."
                      required
                      value={reportName ?? ""}
                      disabled={
                        editReportName
                          ? true
                          : endGenericReport.length > 0
                          ? false
                          : true
                      }
                      onChange={(e) => {
                        const sanitized = e.target.value.replace(
                          /[\\/:*?"<>|]/g,
                          ""
                        );
                        setReportName(sanitized);
                      }}
                    />
                  </label>
                  {location && location?.state?.isEdit && (
                    <button
                      type="button"
                      title={"Edit Report Name"}
                      className="btn btn-link"
                      onClick={() => setEditReportName(!editReportName)}
                    >
                      <img
                        className="btnEdit op-55"
                        src={require("../img/edit.png")}
                      />
                    </button>
                  )}
                </div>
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    width: "100%",
                    gap: "8px",
                    padding: "8px 4px",
                    margin: "16px 0",
                  }}
                >
                  <Box
                    sx={{
                      width: REPORT_COL_WIDTHS.checkbox,
                      flexShrink: 0,
                      display: "flex",
                      justifyContent: "center",
                    }}
                  >
                    <Checkbox
                      checked={
                        endGenericReport.length === 0
                          ? false
                          : endGenericReport.every(
                              (item) => item.checked === true
                            )
                      }
                      indeterminate={
                        endGenericReport.length > 0 &&
                        endGenericReport.some((item) => item.checked) &&
                        !endGenericReport.every((item) => item.checked)
                      }
                      onChange={(e) => selectAll(e.target.checked)}
                      slotProps={{
                        input: {
                          "aria-label": "Checkbox for following text input",
                        },
                      }}
                    />
                  </Box>
                  <Box
                    sx={{
                      width: REPORT_COL_WIDTHS.columnName,
                      flexShrink: 0,
                      fontSize: "14px",
                      fontWeight: 700,
                      color: "#0D0D0D",
                    }}
                  >
                    Column Name
                  </Box>
                  <Box
                    sx={{
                      width: REPORT_COL_WIDTHS.customName,
                      flexShrink: 0,
                      fontSize: "14px",
                      fontWeight: 700,
                      color: "#0D0D0D",
                    }}
                  >
                    Custom Name
                  </Box>
                  <Box
                    sx={{
                      width: REPORT_COL_WIDTHS.color,
                      flexShrink: 0,
                      fontSize: "14px",
                      fontWeight: 700,
                      color: "#0D0D0D",
                      textAlign: "center",
                    }}
                  >
                    Color
                  </Box>
                </Box>
                <div className="headBorder"></div>
              </div>
              <div className="reportScroll">
                <DragDropContext onDragEnd={onDragEnd}>
                  <Droppable droppableId="droppable-1">
                    {(droppableProvided) => (
                      <div
                        ref={droppableProvided.innerRef}
                        {...droppableProvided.droppableProps}
                      >
                        {endGenericReport.map((item, index) => (
                          <Draggable
                            key={"draggable-" + item?.propertyName}
                            draggableId={"draggable-" + item?.propertyName}
                            index={index}
                          >
                            {(draggableProvided, snapshot) => (
                              <div
                                className="dragDiv"
                                ref={draggableProvided.innerRef}
                                {...draggableProvided.draggableProps}
                                style={{
                                  ...draggableProvided.draggableProps.style,
                                }}
                                key={item.propertyName}
                              >
                                <Box
                                  sx={{
                                    display: "flex",
                                    alignItems: "center",
                                    width: "100%",
                                    gap: "8px",
                                    padding: "6px 4px",
                                    marginBottom: "12px",
                                    background: "#fff",
                                    border: "1px solid #E5E5E5",
                                    borderRadius: "8px",
                                    boxShadow: snapshot.isDragging
                                      ? "0 0 .4rem #666"
                                      : "none",
                                  }}
                                >
                                  <Box
                                    sx={{
                                      width: REPORT_COL_WIDTHS.checkbox,
                                      flexShrink: 0,
                                      display: "flex",
                                      justifyContent: "center",
                                    }}
                                  >
                                    <Checkbox
                                      checked={item?.checked ?? false}
                                      onChange={(e) =>
                                        item?.propertyName &&
                                        checkedColumn(
                                          unselectedColumn,
                                          setUnselectedColumn,
                                          e.target.checked,
                                          item.tableName,
                                          item,
                                          item.propertyName,
                                          "remove",
                                          item.colorHeader
                                        )
                                      }
                                    />
                                  </Box>
                                  <Box
                                    sx={{
                                      width: REPORT_COL_WIDTHS.columnName,
                                      flexShrink: 0,
                                      display: "flex",
                                      alignItems: "center",
                                      gap: "4px",
                                      cursor: "pointer",
                                    }}
                                    {...draggableProvided.dragHandleProps}
                                  >
                                    <img
                                      title={`Table Name : ${
                                        LabelsDictionary[item?.tableName]
                                          ?.Full ?? item?.tableName
                                      }\nColumn Name : ${
                                        item.tableName !== "emptyGrid"
                                          ? LabelsDictionary[item?.propertyName]
                                              ?.Full ?? item?.propertyName
                                          : item.propertyName
                                      }`}
                                      style={{
                                        width: 22,
                                        height: 22,
                                        objectFit: "cover",
                                        cursor: "pointer",
                                        flexShrink: 0,
                                      }}
                                      src={require("../../src/img/icon_info.png")}
                                    />
                                    <span
                                      style={{
                                        overflow: "hidden",
                                        textOverflow: "ellipsis",
                                        whiteSpace: "nowrap",
                                      }}
                                    >{`${
                                      item.tableName !== "emptyGrid"
                                        ? LabelsDictionary[item?.propertyName]
                                            ?.Full ?? item?.propertyName
                                        : item.propertyName
                                    }`}</span>
                                  </Box>
                                  <Box
                                    sx={{
                                      width: REPORT_COL_WIDTHS.customName,
                                      flexShrink: 0,
                                      display: "flex",
                                      alignItems: "center",
                                      gap: "6px",
                                      padding: "4px 8px",
                                      borderRadius: "6px",
                                      border: "1px solid #E5E5E5",
                                      background: item.fieldEdit
                                        ? "#fff"
                                        : "#F5F5F5",
                                    }}
                                  >
                                    <Box sx={{ flex: 1, minWidth: 0 }}>
                                      {!item.fieldEdit ? (
                                        <input
                                          type="text"
                                          className="form-control textBorderBg disabledCursor border-radius-none h-100 noStyle px-0"
                                          value={item?.updatedPropertyName}
                                          placeholder={
                                            item?.updatedPropertyName === ""
                                              ? "Enter custom column name......"
                                              : ""
                                          }
                                          disabled
                                        />
                                      ) : (
                                        <input
                                          type="text"
                                          className="form-control textBorderBg border-radius-none h-100 noStyle px-0"
                                          placeholder="Enter custom column name......"
                                          value={
                                            valueId === item.propertyName
                                              ? value
                                              : ""
                                          }
                                          onChange={(e) =>
                                            onChangeHandle(
                                              e.target.value,
                                              item?.propertyName
                                            )
                                          }
                                          id={item.propertyName.toLowerCase()}
                                          aria-describedby={item.propertyName.toLowerCase()}
                                          autoFocus
                                          required
                                        />
                                      )}
                                    </Box>
                                    <Box
                                      sx={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "6px",
                                        flexShrink: 0,
                                      }}
                                    >
                                      {item.fieldEdit && (
                                        <Box
                                          sx={{
                                            cursor: "pointer",
                                            display: "flex",
                                          }}
                                          onClick={() => onChangeSave(item)}
                                        >
                                          <BsCheck2Circle color="green" />
                                        </Box>
                                      )}
                                      <Box
                                        sx={{
                                          cursor: "pointer",
                                          display: "flex",
                                        }}
                                        onClick={() => onChangeEdit(item)}
                                      >
                                        {!item.fieldEdit ? (
                                          <BiSolidPencil />
                                        ) : (
                                          <MdOutlineCancel color="red" />
                                        )}
                                      </Box>
                                    </Box>
                                  </Box>
                                  <Box
                                    sx={{
                                      width: REPORT_COL_WIDTHS.color,
                                      flexShrink: 0,
                                      display: "flex",
                                      justifyContent: "center",
                                    }}
                                  >
                                    <IconButton
                                      className="colorButton px-2"
                                      onClick={(e) => openColorMenu(e, item)}
                                    >
                                      <div
                                        className={`colorDiv ${
                                          item?.colorHeader ?? "white"
                                        }`}
                                      ></div>
                                    </IconButton>
                                    <Menu
                                      anchorEl={colorAnchorEl}
                                      open={
                                        Boolean(colorAnchorEl) &&
                                        colorMenuItem?.propertyName ===
                                          item.propertyName
                                      }
                                      onClose={closeColorMenu}
                                      className="mt-1 bgLightGrey"
                                    >
                                      {columnColorOptions.map((opt) => (
                                        <MenuItem
                                          key={opt.key}
                                          onClick={() => {
                                            changeColumnColor(item, opt.key);
                                            closeColorMenu();
                                          }}
                                          className={
                                            opt.key === "white"
                                              ? "hoverWhiteBg"
                                              : ""
                                          }
                                        >
                                          <div className="w-100 mx-0 py-1 d-flex align-items-center">
                                            <div
                                              className={`legendaElement ${opt.swatchClass}`}
                                            ></div>
                                            <span className="legendaElement">
                                              {opt.label}
                                            </span>
                                          </div>
                                        </MenuItem>
                                      ))}
                                    </Menu>
                                  </Box>
                                </Box>
                                {matchedError &&
                                  valueId == item.propertyName && (
                                    <div
                                      id={item.propertyName}
                                      className="invalid-feedback text-align-right pr-2"
                                    >
                                      {`Column name is already exist.`}
                                    </div>
                                  )}
                              </div>
                            )}
                          </Draggable>
                        ))}
                        {droppableProvided.placeholder}
                      </div>
                    )}
                  </Droppable>
                </DragDropContext>
                {endGenericReport.length == 0 && (
                  <div className="voda-bold pt-14rem">
                    <span>No Table Record Found....</span>
                  </div>
                )}
              </div>
              <div className="headBorder"></div>
              <div className="my-2 h-60 alignCenter">
                {endGenericReport.length > 0 && (
                  <div className="flex justify-content-between">
                    <div>
                      <button
                        type="button"
                        className="btn px-4 btnHeader clearBtn br-20 voda-bold"
                        onClick={() => {
                          setModalSnapshot({
                            exportFileFormat,
                            exportFilePath,
                            exportType,
                            scheduledDate,
                            scheduleFlag,
                            scheduleDayType,
                            opco,
                            isTestNodeRequired,
                          });
                          setIsVisibleModal(true);
                        }}
                      >
                        More Option
                      </button>
                    </div>
                    <div>
                      <button
                        type="button"
                        className="btn px-4 btnHeader clearBtn br-20 voda-bold"
                        onClick={() => onReset()}
                      >
                        Reset
                      </button>
                      <button
                        type="button"
                        className={`${
                          reportName === "" ? "disabledCursor" : "pointer"
                        } btn voda-bold btn-danger px-4 btnHeader `}
                        disabled={reportName === "" ? true : false}
                        onClick={() => onSaveReport()}
                      >
                        Save
                      </button>
                    </div>
                  </div>
                )}
              </div>
            </MuiAccordionDetails>
          </MuiAccordion>
        </div>
      </div>
      <Dialog
        open={isVisibleModal}
        onClose={(_, reason) => {
          // Mirrors the old react-bootstrap Modal's `backdrop="static"` +
          // `keyboard={false}`: ignore backdrop clicks and Escape, only
          // close via the explicit Cancel/close-icon handlers.
          if (reason !== "backdropClick" && reason !== "escapeKeyDown") {
            handleModalCancel();
          }
        }}
        maxWidth="lg"
        fullWidth
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12 d-flex justify-content-between align-items-center">
              <h4 className="mb-0 mt-1">More Options</h4>
              <IconButton onClick={() => handleModalCancel()}>
                <MdClose />
              </IconButton>
            </div>
          </div>
        </DialogTitle>
        <DialogContent className="mx-2">
          <div className="col-12 p-0 mt-4 mx-2">
            <fieldset className="fieldset p-0">
              <div className="row">
                <div className="form-group col-6 pl-0">
                  <div className="col-12">
                    <MultiSelectComponent
                      label={"Select OpCo"}
                      labelCSS="mb-0"
                      inputCSS="labelForm voda-bold mb-2"
                      required={false}
                      value={
                        (opco &&
                          allOpCoResource &&
                          dictionaryToArray(allOpCoResource)
                            .filter((item) => opco.includes(item.key))
                            .map((item) => ({
                              label: item.value,
                              value: item.key,
                            }))) ??
                        []
                      }
                      options={
                        allOpCoResource
                          ? dictionaryToArray(allOpCoResource).map((item) => ({
                              label: item.value,
                              value: item.key,
                            }))
                          : []
                      }
                      onChange={(e: any) => e && handleOpCoSelect(e)}
                    />
                  </div>
                </div>
                <div className="form-group col-6 pl-0">
                  <div className="col-12">
                    <ToggleInputComponent
                      label={"Include Lab Nodes in this report?"}
                      value={isTestNodeRequired ?? false}
                      required={false}
                      onChange={(e: any) => {
                        setIsTestNodeRequired(!isTestNodeRequired);
                      }}
                    />
                  </div>
                </div>

                <div className="d-flex flex-column">
                  <div className="mb-2">
                    <ToggleInputComponent
                      label={"Do you want to schedule this report?"}
                      value={scheduleFlag ?? false}
                      required={false}
                      onChange={(e: any) => {
                        setScheduledFlag(!scheduleFlag);
                        !e.target.checked && resetMoreOptionData();
                      }}
                    />
                  </div>

                  {scheduleFlag && (
                    <div className="btn-group" role="group">
                      <button
                        type="button"
                        style={{
                          backgroundColor:
                            scheduleDayType.scheduledType === "1"
                              ? "red"
                              : "transparent",
                          color:
                            scheduleDayType.scheduledType === "1"
                              ? "white"
                              : "black",
                          border: "1px solid red",
                        }}
                        className="btn btn-sm "
                        onClick={() => {
                          setScheduleDayType({ scheduledType: "1" });
                          setScheduledDate(null);
                        }}
                      >
                        Monthly
                      </button>
                      <button
                        type="button"
                        style={{
                          backgroundColor:
                            scheduleDayType.scheduledType === "2"
                              ? "red"
                              : "transparent",
                          color:
                            scheduleDayType.scheduledType === "2"
                              ? "white"
                              : "black",
                          border: "1px solid red",
                        }}
                        className="btn btn-sm "
                        onClick={() => {
                          setScheduleDayType({ scheduledType: "2" });
                          setScheduledDate(null);
                        }}
                      >
                        Weekly
                      </button>
                    </div>
                  )}
                </div>
              </div>
              {scheduleFlag && (
                <div className="row pt-4">
                  <div className="form-group col-6 pl-0">
                    <div className="col-12">
                      <DropdownInputComponent
                        label={"Scheduled Day"}
                        labelCSS="mb-0"
                        inputCSS="labelForm voda-bold mb-2"
                        isSearchable={true}
                        isClearable={true}
                        required={scheduleFlag ? true : false}
                        isError={
                          ((scheduledDate?.value === undefined ||
                            scheduledDate === null) &&
                            errorField.includes("scheduledDate")) ??
                          false
                        }
                        error={
                          scheduleDayType.scheduledType === "2"
                            ? "Schedule day is required."
                            : "Schedule date is required."
                        }
                        inputType={
                          scheduleDayType.scheduledType === "1"
                            ? "number"
                            : undefined
                        }
                        min={
                          scheduleDayType.scheduledType === "1" ? 0 : undefined
                        }
                        max={
                          scheduleDayType.scheduledType === "1" ? 31 : undefined
                        }
                        value={scheduledDate?.value ? scheduledDate : null}
                        options={
                          scheduleDayType.scheduledType === "2"
                            ? weekdayOptions
                            : null
                        }
                        onChange={(e: any) => {
                          setScheduledDate(e);
                        }}
                        successMessage={
                          scheduleDayType.scheduledType === "1"
                            ? scheduledDate?.value === "0"
                              ? "Report Scheduled on All days of the Month"
                              : `Report Scheduled on ${scheduledMsgFormat(
                                  scheduledDate?.value
                                )} day of the Month`
                            : `Report scheduled on ${scheduledDate?.value}`
                        }
                      />
                    </div>
                  </div>
                  <div className="form-group col-6 pl-0">
                    <div className="col-12">
                      <DropdownInputComponent
                        label={"Export File Format"}
                        labelCSS="mb-0"
                        inputCSS="labelForm voda-bold mb-2"
                        isSearchable={true}
                        isClearable={true}
                        required={scheduleFlag ? true : false}
                        isError={
                          (exportFileFormat === null &&
                            errorField.includes("exportFileFormat")) ??
                          false
                        }
                        error="Export file format is required."
                        value={exportFileFormat}
                        options={fileFormat}
                        onChange={(e: any) => setExportFileFormat(e)}
                      />
                    </div>
                  </div>
                  <div className="form-group col-6 pl-0">
                    <div className="col-12">
                      <DropdownInputComponent
                        label={"Export Type"}
                        labelCSS="mb-0"
                        inputCSS="labelForm voda-bold mb-2"
                        isSearchable={true}
                        isClearable={true}
                        required={scheduleFlag ? true : false}
                        isError={
                          (exportType === null &&
                            errorField.includes("exportType")) ??
                          false
                        }
                        error="Export file type is required."
                        value={exportType}
                        options={exportFileType}
                        onChange={(e: any) => setExportType(e)}
                      />
                    </div>
                  </div>
                </div>
              )}
              <div className="col-12 justify-content-end d-flex footerModal">
                <button
                  className="  voda-bold btn btn-link px-4 btnHeader cancel"
                  onClick={() => handleModalCancel()}
                  type="button"
                >
                  Cancel
                </button>
                <button
                  className="  voda-bold btn btn-link px-4 btnHeader cancel-mr"
                  type="button"
                  onClick={() => handleMoreOptions()}
                >
                  Save
                </button>
              </div>
            </fieldset>
          </div>
        </DialogContent>
      </Dialog>
    </div>
  );
};

export default GenericReportContainer;
