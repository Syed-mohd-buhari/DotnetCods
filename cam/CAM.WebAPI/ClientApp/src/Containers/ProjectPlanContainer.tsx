import React, {
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import { useSearchParams } from "react-router-dom";
import { useAuth } from "../Hook/useAuth";
import { Gantt } from "@svar-ui/react-gantt";
import "@svar-ui/react-gantt/all.css";
import "../Css/ganttChart.css";
import * as htmlToImage from "html-to-image";
import { FaChevronDown, FaChevronRight } from "react-icons/fa";
import { GanttExportToExcel } from "../Constant/GanttExportToExcel";
import { GetProjectPlanReport } from "../Redux/Action/ProjectPlanning/ProjectPlanCommonAction";
import { EditProjectPlan } from "../Redux/Action/ProjectPlanning/ProjectPlanEditAction";
import { safeNumber } from "../Hook/Common";
import { Alert, Box, Button, Collapse, Divider } from "@mui/material";
import Typography from "@mui/material/Typography";
import { GanttForm } from "./GanttForm";
import {
  MdExpandLess,
  MdExpandMore,
  MdOutlineExpandLess,
  MdOutlineExpandMore,
} from "react-icons/md";
import Tooltip, { TooltipProps, tooltipClasses } from "@mui/material/Tooltip";
import { ButtonGroup, Dropdown, ToggleButton } from "react-bootstrap";
import { FcInfo } from "react-icons/fc";
import { styled } from "@mui/material/styles";

/* Interfaces */
interface Status {
  projectPlanId?: number;
  deliveryStatusId: number;
  deliveryStatusText: string;
  startDate: string;
  endDate: string;
  isMileStone: string;
  progress?: number | string | null;
  description?: string | null;
}

interface Milestone {
  milestoneId: number;
  milestoneName: string;
  milestoneDuration: number | null;
  statuses: Status[];
}

interface InputData {
  milestones: Milestone[];
  activityDetail: string;
  colorCoding: string;
  eduSpoc: string;
  paId: number;
}

interface GanttTask {
  id: number;
  text: string;
  start: Date;
  end: Date;
  progress: number;
  parent: number;
  type?: "summary" | "milestone" | any;
  open?: boolean;
  description: string;
  style?: any;
}

const HtmlTooltip = styled(({ className, ...props }: TooltipProps) => (
  <Tooltip
    {...props}
    classes={{ popper: className }}
    placement="bottom-start"
  />
))(({ theme }) => ({
  [`& .${tooltipClasses.tooltip}`]: {
    backgroundColor: "#f5f5f9",
    color: "rgba(0, 0, 0, 0.87)",
    minWidth: 220,
    fontSize: theme.typography.pxToRem(12),
    border: "1px solid #dadde9",
  },
}));
/* Helper - Convert backend InputData into tasks for Gantt view */
const convertToTasks = (data: InputData): GanttTask[] => {
  const tasks: GanttTask[] = [];
  let taskId = 1;

  const sortedMilestones = data.milestones
    ?.filter((m) => m.milestoneId !== 0)
    .sort((a, b) => a.milestoneId - b.milestoneId);

  sortedMilestones?.forEach((milestone) => {
    let minStart: Date | null = null;
    let maxEnd: Date | null = null;

    milestone.statuses.forEach((status) => {
      const start = new Date(status.startDate);
      const end = new Date(status.endDate);
      start.setHours(0, 0, 0, 0);
      end.setHours(0, 0, 0, 0);
      if (!minStart || start < minStart) minStart = start;
      if (!maxEnd || end > maxEnd) maxEnd = end;
    });

    if (!minStart || !maxEnd) return;

    tasks.push({
      id: taskId,
      text: milestone.milestoneName,
      start: minStart,
      end: maxEnd,
      progress: 0,
      parent: 0,
      type: "summary",
      description: "",
      open: true,
    });
    const milestoneTaskId = taskId++;

    milestone.statuses.forEach((status) => {
      let childStart = new Date(status.startDate);
      const newDate = new Date(status.endDate);
      newDate.setHours(newDate.getHours() + 4);
      let childEnd = new Date(status.endDate);
      childStart.setHours(0, 0, 0, 0);
      childEnd.setHours(0, 0, 0, 0);

      tasks.push({
        id: taskId++,
        text: status.deliveryStatusText,
        start: childStart,
        end: childEnd,
        progress: safeNumber(status.progress),
        parent: milestoneTaskId,
        description: status.description ?? "",
      });
      if (status?.isMileStone === "Yes") {
        tasks.push({
          id: taskId++,
          text: "",
          start: childEnd,
          end: newDate,
          type: "milestone",
          progress: safeNumber(status.progress),
          parent: milestoneTaskId,
          description: status.description ?? "",
        });
      }
    });
  });

  setTimeout(() => {
    tasks.forEach((task) => {
      if (task.type === "milestone") {
        const matchedCells = document.querySelectorAll(
          `.wx-cell[data-row-id="${task.id}"]`
        );
        matchedCells.forEach((cell) => {
          (cell as HTMLElement).style.visibility = "hidden";
        });
      }
    });
  }, 200);
  return tasks;
};

/* Groups flat task array into milestone/status array structure */
const groupTasks = (tasks: any[]): any[] => {
  const summaries = tasks.filter((t) => t.type === "summary");
  const children = tasks.filter((t) => t.type !== "summary");
  const summaryMap = new Map<number, any>();
  for (const summary of summaries) {
    summaryMap.set(summary.id, { text: summary.text, statuses: [] });
  }
  const unknown: any = { text: "Unknown", statuses: [] };
  for (const child of children) {
    const status: any = {
      text: child.text,
      startDate: child.start,
      endDate: child.end,
      progress: child.progress,
      description: child.description,
      duration: child.duration,
    };
    const parentSummary = summaryMap.get(child.parent);
    if (parentSummary) parentSummary.statuses.push(status);
    else unknown.statuses.push(status);
  }
  const result: any[] = Array.from(summaryMap.values());
  if (unknown.statuses.length) result.push(unknown);
  return result;
};

/* Updates rawData structure based on modified tasks */
const convertTaskList = ({
  data,
  tasks,
  rawData,
}: {
  data: any;
  tasks: GanttTask[];
  rawData: InputData;
}): InputData => {
  const { id, parent } = data;
  const updatedTasks = tasks.map((task) =>
    task.id === id && task.parent === parent ? data : task
  );
  const groupedData = groupTasks(updatedTasks);

  return {
    ...rawData,
    milestones: rawData.milestones.map((milestone) => {
      const matchedMilestone = groupedData.find(
        (g) => g.text.toLowerCase() === milestone.milestoneName.toLowerCase()
      );
      if (!matchedMilestone) return milestone;
      const updatedStatuses = milestone.statuses.map((status) => {
        const matchedStatus = matchedMilestone.statuses.find(
          (s: any) =>
            s.text.toLowerCase() === status.deliveryStatusText.toLowerCase()
        );
        if (!matchedStatus) return status;
        // Date formatting helper
        const formatDateTime = (dateInput: string | Date): string => {
          const date =
            typeof dateInput === "string" ? new Date(dateInput) : dateInput;
          if (!(date instanceof Date) || isNaN(date.getTime())) return "";
          const yyyy = date.getFullYear();
          const mm = String(date.getMonth() + 1).padStart(2, "0");
          const dd = String(date.getDate()).padStart(2, "0");
          const hh = String(date.getHours()).padStart(2, "0");
          const min = String(date.getMinutes()).padStart(2, "0");
          const ss = String(date.getSeconds()).padStart(2, "0");
          return `${yyyy}-${mm}-${dd}T${hh}:${min}:${ss}`;
        };

        return {
          ...status,
          startDate: formatDateTime(matchedStatus.startDate),
          endDate: formatDateTime(matchedStatus.endDate),
          progress:
            matchedStatus.progress != null
              ? String(matchedStatus.progress)
              : status.progress,
          description:
            matchedStatus.description !== undefined
              ? matchedStatus.description
              : status.description,
          duration:
            matchedStatus.duration !== undefined
              ? matchedStatus.duration
              : (status as any).duration,
        };
      });
      return {
        ...milestone,
        statuses: updatedStatuses,
      };
    }),
  };
};

/* Main Component */
const GanttDemo = () => {
  const { isPermesso } = useAuth();
  const [searchParams] = useSearchParams();
  const [open, setOpen] = useState(false);
  const ganttRef = useRef<HTMLDivElement>(null);
  const apiRef = useRef<any>(null);
  const [apiFlag, setApiFlag] = useState(false);
  const [plannedData, setPlannedData] = useState<InputData | null>(null);
  const [baselineData, setBaselineData] = useState<InputData | null>(null);
  const [rawData, setRawData] = useState<InputData | null>(null);
  const [tasks, setTasks] = useState<GanttTask[]>([]);
  const [task, setTask] = useState<GanttTask | null>(null);
  const [radioValue, setRadioValue] = useState("planned");
  // Load project plan data
  const GetProjectPlanData = useCallback(async () => {
    const paId = searchParams.get("paId");
    if (!paId) return;
    const result: any = await GetProjectPlanReport(paId);
    if (result?.data?.[0]) {
      const data = result.data[0] as InputData;
      const plannedMilestoneData = data?.milestones?.map((res) => {
        return {
          ...res,
          statuses: res?.statuses?.map((stat: any) => {
            const { ...rest } = stat;
            return {
              ...rest,
              startDate: stat?.planningStartDate,
              endDate: stat?.planningEndDate,
            } as any;
          }),
        };
      });
      const baseLineMilestoneData = data?.milestones?.map((res) => {
        return {
          ...res,
          statuses: res?.statuses?.map((stat: any) => {
            const { ...rest } = stat;
            return {
              ...rest,
              startDate: stat?.baseLineStartDate,
              endDate: stat?.baseLineEndDate,
            } as any;
          }),
        };
      });
      setPlannedData({ ...data, milestones: plannedMilestoneData });
      setBaselineData({ ...data, milestones: baseLineMilestoneData });
    }
    if (result?.data.length === 0) {
      setApiFlag(true);
    }
  }, [searchParams]);

  useEffect(() => {
    if (radioValue === "planned") {
      setRawData(plannedData);
      plannedData && setTasks(convertToTasks(plannedData));
    } else {
      setRawData(baselineData);
      baselineData && setTasks(convertToTasks(baselineData));
    }
  }, [plannedData, baselineData, radioValue]);

  useEffect(() => {
    if (isPermesso) GetProjectPlanData();
  }, [isPermesso, GetProjectPlanData]);

  // Export Gantt chart as Excel file
  const handleExporting = useCallback(async () => {
    if (!ganttRef.current) return;
    try {
      const dataUrl = await htmlToImage.toPng(ganttRef.current, {
        pixelRatio: 2,
        cacheBust: true,
      });
      // Strip HTML tags for title
      const stripHtmlTags = (htmlString: string) => {
        const tmp = document.createElement("DIV");
        tmp.innerHTML = htmlString;
        return tmp.textContent || tmp.innerText || "";
      };
      const title = rawData?.activityDetail
        ? stripHtmlTags(rawData.activityDetail)
        : "Gantt Chart";
      await GanttExportToExcel(tasks, dataUrl, title, radioValue);
    } catch (error) {
      console.error("Export failed:", error);
    }
  }, [rawData?.activityDetail, tasks]);

  const columns = useMemo(
    () => [
      {
        id: "text",
        header: "Delivery Phase",
        flexgrow: 2,
        render: (task: any) => {
          const isParent =
            Array.isArray(task.children) && task.children.length > 0;
          const isOpen = task.open;
          return (
            <div style={{ display: "flex", alignItems: "center", gap: 5 }}>
              {isParent ? (
                isOpen ? (
                  <FaChevronDown />
                ) : (
                  <FaChevronRight />
                )
              ) : null}
              {task.text}
            </div>
          );
        },
      },
      {
        id: "start",
        header: `${
          radioValue === "planned" ? "Planned" : "Baseline"
        } Start Date`,
        flexgrow: 1,
        align: "center",
        render: (task: any) => {
          if (task.type === "milestone") return null;
          return task.start?.toLocaleDateString("en-GB");
        },
      },
      {
        id: "end",
        header: `${
          radioValue === "planned" ? "Planned" : "Baseline"
        } Completion Date`,
        flexgrow: 1,
        align: "center",
        render: (task: any) => {
          if (task.type === "milestone") return null;
          return task.end?.toLocaleDateString("en-GB");
        },
      },
    ],
    [radioValue]
  );

  // Collect and set Gantt API, disable built-in drag/edit features
  const initApi = useCallback((api: any) => {
    apiRef.current = api;
    api.intercept("move-task", () => false);
    api.intercept("resize-task", () => false);
    api.intercept("drag-task", () => false);
    api.intercept("update-task", () => false);
  }, []);

  // Custom editor: show form, not built-in editor
  useEffect(() => {
    if (!apiRef.current) return;
    const interceptor = apiRef.current.intercept(
      "show-editor",
      (data: { id: number }) => {
        const selected = tasks.find(
          (t) =>
            t.id === data.id &&
            t.type !== "summary" &&
            t.type !== "milestone" &&
            radioValue !== "baseline"
        );
        if (selected) setTask(selected);
        return false;
      }
    );
    return () => {
      interceptor?.remove?.();
    };
  }, [tasks]);

  // Handles form events (update, close) from GanttForm
  const formAction = useCallback(
    (ev: { action: string; data?: any }) => {
      const { action, data } = ev;
      switch (action) {
        case "close-form":
          setTask(null);
          break;
        case "update-task":
          if (!apiRef.current || !data || typeof data.id !== "number") return;
          const { id, ...restData } = data;
          apiRef.current.exec(action, { id, task: { ...restData } });
          if (rawData) {
            const newChanges = convertTaskList({
              data,
              tasks,
              rawData,
            });
            updateEditApi(newChanges);
          }
          setTask(null);
          break;
        default:
          if (apiRef.current) apiRef.current.exec(action, data);
          break;
      }
    },
    [rawData, tasks]
  );

  // Calls backend to update and reload data
  const updateEditApi = async (updatedRawData: InputData) => {
    const plannedMilestoneData = updatedRawData?.milestones?.map((res) => {
      return {
        ...res,
        statuses: res?.statuses?.map((stat: any) => {
          const { startDate, endDate, ...rest } = stat;
          return {
            ...rest,
            planningStartDate: startDate,
            planningEndDate: endDate,
          } as any;
        }),
      };
    });
    const baseLineMilestoneData = updatedRawData?.milestones?.map((res) => {
      return {
        ...res,
        statuses: res?.statuses?.map((stat: any) => {
          const { startDate, endDate, ...rest } = stat;
          return {
            ...rest,
            baseLineStartDate: startDate,
            baseLineEndDate: endDate,
          } as any;
        }),
      };
    });
    const payload = {
      ...updatedRawData,
      milestones:
        radioValue === "planned" ? plannedMilestoneData : baseLineMilestoneData,
    };
    console.log("payload", payload);
    const result = await EditProjectPlan(payload);
    if (result?.warning === false) {
      setOpen(true);
      GetProjectPlanData();
    }
  };

  const CustomTaskTemplate = ({ task }) => {
    console.log("Task", task);
    const start = task?.start ? new Date(task.start).toLocaleDateString() : "-";
    const end = task?.end ? new Date(task.end).toLocaleDateString() : "-";

    return (
      <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
        <span
          style={{
            background: "#1976d2",
            color: "#fff",
            padding: "3px 10px",
            borderRadius: 3,
            whiteSpace: "nowrap",
          }}
        >
          {task?.text ?? ""}
        </span>
        <span style={{ fontSize: 12, color: "#eee", marginLeft: 10 }}>
          {start} → {end}
        </span>
      </div>
    );
  };

  return (
    <div className="pageContainer">
      {/* Header */}
      <div className="headerPage row mx-0 justify-content-between position-relative">
        <div className="col-8 pl-0 d-flex">
          <h5
            className="voda-bold text-left mb-0"
            dangerouslySetInnerHTML={{
              __html: rawData?.activityDetail ?? "",
            }}
          />
        </div>
        <div className="col-4 pr-0 d-flex justify-content-end">
          {rawData && tasks?.length > 0 && (
            <button
              className="download-to-excel mrl-10"
              style={{ width: "fit-content" }}
              onClick={handleExporting}
            >
              Download to Excel
            </button>
          )}

          <Dropdown className="pl-2 d-inline more-options grid-main-btn">
            <Dropdown.Toggle id="dropdown-autoclose-inside">
              More Options
            </Dropdown.Toggle>

            <Dropdown.Menu
              className="grid-main-btn"
              style={{ marginTop: "10px" }}
            >
              <>
                <ButtonGroup
                  className="patBtnGroup"
                  style={{ width: "max-content" }}
                >
                  {[
                    { name: "Planned Date", value: "planned" },
                    { name: "Baseline Date", value: "baseline" },
                  ].map((radio, idx) => (
                    <ToggleButton
                      key={idx}
                      id={`radio-${idx}`}
                      type="radio"
                      variant={
                        radioValue === radio.value
                          ? "outline-primary"
                          : "outline-secondary"
                      }
                      style={{ width: "max-content" }}
                      name="radio"
                      value={radio.value}
                      checked={radioValue === radio.value}
                      onChange={(e) => {
                        setRadioValue(e.currentTarget.value);
                      }}
                    >
                      {radio.name}
                    </ToggleButton>
                  ))}
                </ButtonGroup>
                <Dropdown.Item>Legend</Dropdown.Item>
                <div className="bubbleMenuLegenda">
                  <div className="triangleBubbleTop-right"></div>
                  <div className="col-12 row mx-0 px-2 my-2">
                    <div className="w-100 mx-0 py-1 d-flex align-items-center">
                      <div
                        className="legendaElement green"
                        style={{ backgroundColor: "#00ba94" }}
                      ></div>
                      <span className="legendaElement">Milestone</span>
                    </div>
                    <div className="w-100 mx-0 py-1 d-flex align-items-center">
                      <div
                        className="legendaElement yellow"
                        style={{ backgroundColor: "#37a9ef" }}
                      ></div>
                      <span className="legendaElement">Delivery Status</span>
                    </div>
                    <div className="w-100 mx-0 py-2 d-flex align-items-center">
                      <div
                        className="legendaElement red"
                        style={{
                          backgroundColor: "#ad44ab",
                          transform: "rotate(45deg)",
                        }}
                      ></div>
                      <span className="legendaElement">Delivery Milestone</span>
                    </div>
                  </div>
                </div>
              </>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      {rawData && tasks?.length > 0 && (
        <div className="row mx-0 justify-content-between position-relative mb-2">
          <Typography
            component="div"
            sx={{ textAlign: "left", alignItems: "center", fontWeight: "bold" }}
          >
            Edu Spoc : {rawData?.eduSpoc}
          </Typography>
          <div className="p-0 d-flex">
            <Typography
              variant="h4"
              sx={{
                margin: 0,
                display: "flex",
                alignItems: "center",
                fontSize: "1.8rem !important",
                color: (theme) => theme.palette.text.primary,
              }}
              className={`p-1 px-2 ${
                rawData?.colorCoding === "red"
                  ? "ppRed"
                  : rawData?.colorCoding === "green"
                  ? "ppGreen"
                  : "ppAmber"
              }`}
            >
              <HtmlTooltip
                style={{
                  padding: "0px !important",
                  margin: "0px !important",
                  width: "max-content",
                }}
                title={
                  <>
                    {" "}
                    <Box
                      sx={{
                        borderRadius: 2,
                        boxShadow: "0 3px 12px rgba(0,0,0,0.1)",
                        padding: 2,
                        fontFamily: "Roboto, Arial, sans-serif",
                        fontSize: 14,
                        color: "#222",
                      }}
                    >
                      <Typography
                        component="div"
                        sx={{ fontWeight: "bold", mb: 1 }}
                      >
                        Legend
                      </Typography>

                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          gap: 1,
                          mb: 1,
                        }}
                      >
                        <Box
                          sx={{
                            width: 18,
                            height: 18,
                            bgcolor: "#00c300",
                            borderRadius: 1,
                          }}
                        />
                        <Typography sx={{ fontSize: 14, userSelect: "none" }}>
                          Within 1 Month
                        </Typography>
                      </Box>

                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          gap: 1,
                          mb: 1,
                        }}
                      >
                        <Box
                          sx={{
                            width: 18,
                            height: 18,
                            bgcolor: "#ffff00",
                            borderRadius: 1,
                          }}
                        />
                        <Typography sx={{ fontSize: 14, userSelect: "none" }}>
                          Within 2 Months
                        </Typography>
                      </Box>

                      <Box
                        sx={{ display: "flex", alignItems: "center", gap: 1 }}
                      >
                        <Box
                          sx={{
                            width: 18,
                            height: 18,
                            bgcolor: "#ed1b1b",
                            borderRadius: 1,
                          }}
                        />
                        <Typography sx={{ fontSize: 14, userSelect: "none" }}>
                          More than 2 Months
                        </Typography>
                      </Box>
                    </Box>
                  </>
                }
              >
                <Button
                  className={`ppInfo ${
                    rawData?.colorCoding === "red"
                      ? "ppRed"
                      : rawData?.colorCoding === "green"
                      ? "ppGreen"
                      : "ppAmber"
                  } p-0`}
                  sx={{
                    width: "max-content",
                    fontWeight: "bold",
                    color: "black",
                    textTransform: "none",
                  }}
                >
                  Last Update Status
                </Button>
              </HtmlTooltip>
            </Typography>
          </div>
        </div>
      )}
      <Collapse in={open}>
        <Alert
          severity="success"
          action={
            <Button
              variant="outlined"
              color="inherit"
              size="small"
              onClick={() => setOpen(false)}
            >
              OK
            </Button>
          }
        >
          Changes are updated successfully. Kindly update them in Delivery
          tracking too if required.
        </Alert>
      </Collapse>
      {/* Gantt Chart and Form */}
      {tasks?.length > 0 && (
        <div
          ref={ganttRef}
          style={{ height: "calc(90vh - 120px)", position: "relative" }}
          className="wx-willow-dark-theme"
        >
          <Gantt
            tasks={tasks}
            scales={[
              {
                unit: "month",
                step: 1,
                format: (date) =>
                  new Date(date).toLocaleDateString("en-GB", {
                    month: "short",
                    year: "numeric",
                  }),
              },
            ]}
            columns={columns as any}
            readonly={true}
          />
          {task && <GanttForm task={task} onAction={formAction} />}
        </div>
      )}
      {apiFlag && <Alert severity="info">Information Not Found.</Alert>}
    </div>
  );
};

export default GanttDemo;
