import React, { useState, useEffect, useRef, useMemo } from "react";
import { useSelector } from "react-redux";
import { useSearchParams } from "react-router-dom";
import Select from "react-select";
import Typography from "@mui/material/Typography";
import Chip from "@mui/material/Chip";
import Tooltip from "@mui/material/Tooltip";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Button from "@mui/material/Button";
import {
  Button as BootstrapButton,
  ButtonGroup as BootstrapButtonGroup,
  Dropdown as BootstrapDropdown,
} from "react-bootstrap";
import { styled, createTheme, ThemeProvider } from "@mui/material/styles";
import { TooltipProps, tooltipClasses } from "@mui/material/Tooltip";
import { FcInfo } from "react-icons/fc";
import { useAuth } from "../../Hook/useAuth";
import { useTheme } from "../../Context/ThemeContext";
import { safeNumber } from "../../Hook/Common";
import {
  GetAssetLevelReportForExodusGrid,
  GetOpcoAndPlannedDcfDropdown,
} from "../../Redux/Action/Report/AssetLevelReportForExodusAction";
import {
  AssetLevelExodusGrid,
  ExodusAssetDetail,
  DropdownResource,
} from "../../Model/Report/AssetLevelReportForExodus";
import { useTimelineExcelExport } from "../../Constant/useTimelineExcelExport";

const HtmlTooltip = styled(({ className, ...props }: TooltipProps) => (
  <Tooltip {...props} classes={{ popper: className }} placement="right-start" />
))(({ theme }) => ({
  [`& .${tooltipClasses.tooltip}`]: {
    backgroundColor: "#f5f5f9",
    color: "rgba(0, 0, 0, 0.87)",
    minWidth: 220,
    fontSize: theme.typography.pxToRem(12),
    border: "1px solid #dadde9",
  },
}));

const ALL = "All";

type RawActivity = {
  activityOrder: number;
  activityDescription: string;
  activityStartDate: string | null;
  activityEndDate: string | null;
  actualColumnName: string | null;
  mileStoneDescription: string | null;
};

type Row = {
  OpCo: string;
  opcoId: number | null;
  Location: string;
  AssetName: string;
  BOMStart: string | null;
  BOMSubmitted: string | null;
  HWPORaised: string | null;
  HWPOArrived: string | null;
  RFA: string | null;
  RFO: string | null;
  RFS: string | null;
  MigrationCompletion: string | null;
  MigrationStart: string | null;
  VEC: string | null;
  StartOfAppIntegration: string | null;
  vendorNf: string;
  tragetStack: string;
  activities: RawActivity[];
};

type Phase = {
  startKey: keyof Row;
  endKey: keyof Row;
  label: string;
  short: string;
  code: string;
  bg: string;
  color: string;
};

type Milestone = {
  key: string;
  color: string;
  short: string;
  full: string;
};

type BarItem = {
  label: string;
  short: string;
  code: string;
  bg: string;
  color: string;
  startStr: string | null;
  endStr: string | null;
  left: number;
  width: number;
};

type ChipItem = {
  monthIndex: number;
  center: number;
  label: string;
  short: string;
  code: string;
  bg: string;
  color: string;
  startStr: string | null;
  endStr: string | null;
  isBar: boolean;
};

type DiamondItem = {
  center: number;
  color: string;
  short: string;
  full: string;
  dateStr: string | null;
};

const PHASES: Phase[] = [
  {
    startKey: "BOMStart",
    endKey: "BOMSubmitted",
    label: "x-BoM Submission",
    short: "x-BoM",
    code: "B",
    bg: "#8e9b6c",
    color: "#fff",
  },
  {
    startKey: "BOMSubmitted",
    endKey: "HWPORaised",
    label: "HW Delivery",
    short: "HW Del",
    code: "HR",
    bg: "#d9b18f",
    color: "#000",
  },
  {
    startKey: "HWPORaised",
    endKey: "HWPOArrived",
    label: "HW Installation & Integration",
    short: "HW I&I",
    code: "HA",
    bg: "#c96b22",
    color: "#fff",
  },
  {
    startKey: "HWPOArrived",
    endKey: "RFO",
    label: "App Integration",
    short: "App Int",
    code: "O",
    bg: "#4a90d9",
    color: "#fff",
  },
  {
    startKey: "RFO",
    endKey: "RFA",
    label: "TACC && FACC",
    short: "TACC",
    code: "A",
    bg: "#6ecb6e",
    color: "#000",
  },
  {
    startKey: "RFA",
    endKey: "RFS",
    label: "Migration",
    short: "Migration",
    code: "M",
    bg: "#2e8b2e",
    color: "#fff",
  },
  {
    startKey: "RFS",
    endKey: "MigrationCompletion",
    label: "Migration Complete",
    short: "Migr Comp",
    code: "MC",
    bg: "#843C0C",
    color: "#fff",
  },
];

const MILESTONE_KEYS: (keyof Row)[] = [
  "BOMStart",
  "BOMSubmitted",
  "HWPORaised",
  "HWPOArrived",
  "VEC",
  "StartOfAppIntegration",
  "RFO",
  "RFA",
  "MigrationStart",
  "RFS",
  "MigrationCompletion",
];

const MILESTONES: Milestone[] = [
  {
    key: "BOMSubmitted",
    color: "#1F4E79",
    short: "xBOM",
    full: "xBOM Submitted",
  },
  { key: "HWPORaised", color: "#2E75B6", short: "HWPO", full: "HW PO Raised" },
  {
    key: "HWPOArrived",
    color: "#00B0F0",
    short: "HWArr",
    full: "Hardware Arrived",
  },
  { key: "VEC", color: "#E36C09", short: "VEC", full: "VEC infra ready" },
  {
    key: "StartOfAppIntegration",
    color: "#9C27B0",
    short: "SAI",
    full: "App Integration",
  },
  { key: "RFO", color: "#7030A0", short: "RFO", full: "RFO" },
  { key: "RFA", color: "#C00000", short: "RFA", full: "RFA" },
  { key: "RFS", color: "#548235", short: "RFS", full: "RFS" },
  {
    key: "MigrationStart",
    color: "#00897B",
    short: "MigS",
    full: "Migration Start",
  },
  {
    key: "MigrationCompletion",
    color: "#375623",
    short: "MigC",
    full: "Migration Complete",
  },
];

const ACTIVITY_COLORS: Record<number, string> = {
  1: "#8e9b6c",
  2: "#d9b18f",
  3: "#c96b22",
  4: "#7030A0",
  5: "#4a90d9",
  6: "#6ecb6e",
  7: "#843C0C",
  8: "#843C0C",
};

const MILESTONE_PALETTE = [
  "#1F4E79",
  "#2E75B6",
  "#00B0F0",
  "#7030A0",
  "#C00000",
  "#548235",
  "#375623",
  "#E36C09",
  "#9C27B0",
  "#00897B",
];

const COLUMN_LABELS: Record<string, string> = {
  startofappintegration: "Start of app Integration",
  migrationstart: "Migration Start",
};

const MIN_TIMELINE_MONTHS = 24;

const stripHtml = (s: string) =>
  s
    .replace(/<[^>]*>/g, "")
    .replace(/\s+/g, " ")
    .trim();

const fmtDate = (d: Date) =>
  d.toLocaleDateString("en-GB", {
    day: "2-digit",
    month: "short",
    year: "2-digit",
  });

const humanize = (s: string) => {
  const t = s
    .replace(/date$/i, "")
    .replace(/([a-z0-9])([A-Z])/g, "$1 $2")
    .replace(/[_-]+/g, " ")
    .trim();
  return t ? t.charAt(0).toUpperCase() + t.slice(1) : s;
};

const acronym = (label: string) =>
  label
    .split(/[\s&/-]+/)
    .filter(Boolean)
    .map((w) => w[0])
    .join("")
    .toUpperCase()
    .slice(0, 3);

const textOn = (hex: string) => {
  const c = hex.replace("#", "");
  const r = parseInt(c.slice(0, 2), 16);
  const g = parseInt(c.slice(2, 4), 16);
  const b = parseInt(c.slice(4, 6), 16);
  return (r * 299 + g * 587 + b * 114) / 1000 > 150 ? "#000" : "#fff";
};

function toRow(a: ExodusAssetDetail, group?: any): Row {
  const x = a as any;
  const g = group ?? {};
  const activities: any[] = x.activityDetails ?? [];

  const getEndDate = (columnName: string): string | null => {
    const target = columnName.toLowerCase();
    const found = activities.find((act: any) =>
      (act.actualColumnName || "")
        .split(",")
        .map((s: string) => s.trim().toLowerCase())
        .includes(target)
    );
    return found?.activityEndDate ?? null;
  };

  const firstActivity = activities
    .slice()
    .sort(
      (a: any, b: any) => (a.activityOrder ?? 0) - (b.activityOrder ?? 0)
    )[0];

  return {
    OpCo: a.opcoDesc ?? (a.opcoId != null ? String(a.opcoId) : ""),
    opcoId: a.opcoId ?? null,
    Location: a.location ?? g.location ?? "",
    AssetName: a.newelEmentName ?? "",
    BOMStart: firstActivity?.activityStartDate ?? x.bomStartDate ?? null,
    BOMSubmitted:
      a.bomSubmittedDate !== undefined
        ? a.bomSubmittedDate
        : getEndDate("Bomsubmitteddate"),
    HWPORaised:
      a.hwPoRaisedDate !== undefined
        ? a.hwPoRaisedDate
        : getEndDate("Hwporaiseddate"),
    HWPOArrived:
      a.hwPoArrivedDate !== undefined
        ? a.hwPoArrivedDate
        : getEndDate("Hwpoarriveddate"),
    RFO: a.rfoDate !== undefined ? a.rfoDate : getEndDate("Rfodate"),
    RFA: a.rfaDate !== undefined ? a.rfaDate : getEndDate("Rfadate"),
    RFS: a.rfsDate !== undefined ? a.rfsDate : getEndDate("Rfsdate"),
    MigrationCompletion:
      a.migrationCompletionDate !== undefined
        ? a.migrationCompletionDate
        : getEndDate("Migrationcompletiondate"),
    VEC: a.vecDate !== undefined ? a.vecDate : getEndDate("Vecdate"),
    StartOfAppIntegration:
      a.startOfAppIntegration !== undefined
        ? a.startOfAppIntegration
        : getEndDate("Startofappintegration"),
    MigrationStart:
      a.migrationStart !== undefined
        ? a.migrationStart
        : getEndDate("Migrationstart"),
    vendorNf: g.productName ?? x.productName ?? "",
    tragetStack: g.platform ?? x.platform ?? "",
    activities: activities as RawActivity[],
  };
}

const pickList = (
  filteredList: DropdownResource[] | undefined,
  base: DropdownResource[],
  selectedKey: number | "All"
) => {
  let list = filteredList && filteredList.length ? filteredList : base;
  if (selectedKey !== ALL && !list.some((o) => o.key === selectedKey)) {
    const found = base.find((o) => o.key === selectedKey);
    if (found) list = [found, ...list];
  }
  return list;
};

const buildOptions = (
  resource: DropdownResource[],
  selected: number | "All"
) => {
  const opts = resource.map((o) => ({
    value: o.key as number | "All",
    label: stripHtml(o.text),
  }));
  return selected === ALL ? [{ value: ALL, label: "All" }, ...opts] : opts;
};

export default function ProductComplianceDetails() {
  const [searchParams] = useSearchParams();
  const { isPermesso } = useAuth();
  const { darkMode } = useTheme();
  const { downloadExcel } = useTimelineExcelExport();

  const lightTheme = useMemo(
    () =>
      createTheme({
        palette: {
          mode: "light",
          primary: { main: "#1976d2" },
          text: { primary: "#000000", secondary: "#555555" },
          background: { default: "#ffffff" },
        },
      }),
    []
  );

  const darkTheme = useMemo(
    () =>
      createTheme({
        palette: {
          mode: "dark",
          primary: { main: "#90caf9" },
          text: { primary: "#ffffff", secondary: "#555555" },
          background: { default: "#121212" },
        },
      }),
    []
  );

  const theme = darkMode ? darkTheme : lightTheme;

  const grid = useSelector(
    (s: any) => s.assetLevelReportForExodusReducer
  ) as AssetLevelExodusGrid;

  const [opcoId, setOpcoId] = useState<number | "All">(ALL as "All");
  const [envId, setEnvId] = useState<number | "All">(ALL as "All");
  const [platformId, setPlatformId] = useState<number | "All">(ALL as "All");
  const [productId, setProductId] = useState<number | "All">(ALL as "All");
  const [filterHeadingObj, setFilterHeadingObj] = useState<
    { title: string; value: string[] }[]
  >([]);

  const isReadOnlyView = !!(
    searchParams.get("opcoId") ||
    searchParams.get("envId") ||
    searchParams.get("platformId") ||
    searchParams.get("productId")
  );

  const baseOpco = useMemo(
    () => grid?.OpcoDcfDropdownResult?.data?.opcoDropdown ?? [],
    [grid?.OpcoDcfDropdownResult?.data?.opcoDropdown]
  );
  const baseEnv = useMemo(
    () => (grid?.OpcoDcfDropdownResult?.data as any)?.environmentDropdown ?? [],
    [(grid?.OpcoDcfDropdownResult?.data as any)?.environmentDropdown]
  );
  const basePlatform = useMemo(
    () => (grid?.OpcoDcfDropdownResult?.data as any)?.platformDropdown ?? [],
    [(grid?.OpcoDcfDropdownResult?.data as any)?.platformDropdown]
  );
  const baseProduct = useMemo(
    () => (grid?.OpcoDcfDropdownResult?.data as any)?.productDopdown ?? [],
    [(grid?.OpcoDcfDropdownResult?.data as any)?.productDopdown]
  );

  const filtered =
    grid?.AssetLevelExodusResult?.data?.filteredOpcoAndDcfDropdown?.data;

  const opcoResources = useMemo(
    () => pickList((filtered as any)?.opcoDropdown, baseOpco, opcoId),
    [filtered, baseOpco, opcoId]
  );
  const envResources = useMemo(
    () => pickList((filtered as any)?.environmentDropdown, baseEnv, envId),
    [filtered, baseEnv, envId]
  );
  const platformResources = useMemo(
    () =>
      pickList((filtered as any)?.platformDropdown, basePlatform, platformId),
    [filtered, basePlatform, platformId]
  );
  const productResources = useMemo(
    () => pickList((filtered as any)?.productDopdown, baseProduct, productId),
    [filtered, baseProduct, productId]
  );

  const opcoOptions = useMemo(
    () => buildOptions(opcoResources, opcoId),
    [opcoResources, opcoId]
  );
  const envOptions = useMemo(
    () => buildOptions(envResources, envId),
    [envResources, envId]
  );
  const platformOptions = useMemo(
    () => buildOptions(platformResources, platformId),
    [platformResources, platformId]
  );
  const productOptions = useMemo(
    () => buildOptions(productResources, productId),
    [productResources, productId]
  );

  useEffect(() => {
    if (!isPermesso) return;
    const opCoParam = searchParams.get("opcoId");
    const envParam = searchParams.get("envId");
    const platformParam = searchParams.get("platformId");
    const productParam = searchParams.get("productId");
    const opCoIds = opCoParam ? opCoParam.split(",").map(safeNumber) : [];
    const envIds = envParam ? envParam.split(",").map(safeNumber) : [];
    const platformIds = platformParam
      ? platformParam.split(",").map(safeNumber)
      : [];
    const productIds = productParam
      ? productParam.split(",").map(safeNumber)
      : [];
    if (opCoIds.length) setOpcoId(opCoIds[0]);
    if (envIds.length) setEnvId(envIds[0]);
    if (platformIds.length) setPlatformId(platformIds[0]);
    if (productIds.length) setProductId(productIds[0]);
    const payload = {
      ...(opCoIds.length ? { opcoId: opCoIds } : {}),
      ...(envIds.length ? { EnvironmentId: envIds } : {}),
      ...(platformIds.length ? { PlatformId: platformIds } : {}),
      ...(productIds.length ? { ProductId: productIds } : {}),
    };
    GetAssetLevelReportForExodusGrid(payload);
    GetOpcoAndPlannedDcfDropdown(payload);
  }, [isPermesso, searchParams]);

  useEffect(() => {
    const opCoParam = searchParams.get("opcoId");
    const envParam = searchParams.get("envId");
    const platformParam = searchParams.get("platformId");
    const productParam = searchParams.get("productId");
    const result: { title: string; value: string[] }[] = [];
    if (opCoParam) {
      const ids = opCoParam.split(",").map(safeNumber);
      const labels = ids
        .map((id) => opcoResources.find((o) => o.key === id))
        .filter(Boolean)
        .map((o) => stripHtml((o as DropdownResource).text));
      if (labels.length) result.push({ title: "OpCo", value: labels });
    }
    if (envParam) {
      const ids = envParam.split(",").map(safeNumber);
      const labels = ids
        .map((id) => envResources.find((e) => e.key === id))
        .filter(Boolean)
        .map((e) => stripHtml((e as DropdownResource).text));
      if (labels.length) result.push({ title: "Environment", value: labels });
    }
    if (platformParam) {
      const ids = platformParam.split(",").map(safeNumber);
      const labels = ids
        .map((id) => platformResources.find((p) => p.key === id))
        .filter(Boolean)
        .map((p) => stripHtml((p as DropdownResource).text));
      if (labels.length) result.push({ title: "Target Stack", value: labels });
    }
    if (productParam) {
      const ids = productParam.split(",").map(safeNumber);
      const labels = ids
        .map((id) => productResources.find((p) => p.key === id))
        .filter(Boolean)
        .map((p) => stripHtml((p as DropdownResource).text));
      if (labels.length) result.push({ title: "Vendor NF", value: labels });
    }
    setFilterHeadingObj((prev) =>
      JSON.stringify(prev) === JSON.stringify(result) ? prev : result
    );
  }, [
    searchParams,
    opcoResources,
    envResources,
    platformResources,
    productResources,
  ]);

  const liveGroups =
    grid?.AssetLevelExodusResult?.data?.daAssetMigrationDetails;
  const pendingFilterRef = useRef(false);
  const [snapshot, setSnapshot] = useState<any[] | null>(null);

  useEffect(() => {
    if (snapshot === null && liveGroups?.length) {
      setSnapshot(liveGroups);
    } else if (pendingFilterRef.current && liveGroups) {
      setSnapshot(liveGroups);
      pendingFilterRef.current = false;
    }
  }, [liveGroups]);

  const rows: Row[] = useMemo(
    () =>
      (snapshot ?? []).flatMap((g: any) =>
        (g.assetDetails ?? []).map((a: any) => toRow(a, g))
      ),
    [snapshot]
  );

  const {
    activityColorMap,
    milestoneColorMap,
    activityLegend,
    milestoneLegend,
  } = useMemo(() => {
    const actMap = new Map<string, string>();
    const msMap = new Map<string, string>();
    const actLegend: { label: string; color: string }[] = [];
    const msLegend: { label: string; color: string }[] = [];
    let msi = 0;

    rows.forEach((d) => {
      const acts = [...(d.activities ?? [])].sort(
        (a, b) => (a.activityOrder ?? 0) - (b.activityOrder ?? 0)
      );
      let prevLabel = "";
      acts.forEach((a) => {
        const cont =
          prevLabel &&
          a.activityDescription
            ?.toLowerCase()
            .startsWith(prevLabel.toLowerCase() + " ");
        if (!cont) {
          if (!actMap.has(a.activityDescription)) {
            const color = ACTIVITY_COLORS[a.activityOrder] || "#888";
            actMap.set(a.activityDescription, color);
            actLegend.push({ label: a.activityDescription, color });
          }
          prevLabel = a.activityDescription;
        }
        const cols = (a.actualColumnName || "")
          .split(",")
          .map((s) => s.trim())
          .filter(Boolean);
        const labels = (a.mileStoneDescription || "")
          .split(",")
          .map((s) => s.trim())
          .filter(Boolean);
        cols.forEach((col, i) => {
          const label =
            labels[i] || COLUMN_LABELS[col.toLowerCase()] || humanize(col);
          if (!msMap.has(label)) {
            const color = MILESTONE_PALETTE[msi % MILESTONE_PALETTE.length];
            msMap.set(label, color);
            msLegend.push({ label, color });
            msi++;
          }
        });
      });
    });

    return {
      activityColorMap: actMap,
      milestoneColorMap: msMap,
      activityLegend: actLegend,
      milestoneLegend: msLegend,
    };
  }, [rows]);

  const handleOpco = (id: number | "All") => {
    setOpcoId(id);
    GetOpcoAndPlannedDcfDropdown(id !== ALL ? { opcoId: [id] } : {});
  };
  const handleEnv = (id: number | "All") => {
    setEnvId(id);
    GetOpcoAndPlannedDcfDropdown(id !== ALL ? { EnvironmentId: [id] } : {});
  };
  const handlePlatform = (id: number | "All") => {
    setPlatformId(id);
    GetOpcoAndPlannedDcfDropdown(id !== ALL ? { PlatformId: [id] } : {});
  };
  const handleProduct = (id: number | "All") => {
    setProductId(id);
    GetOpcoAndPlannedDcfDropdown(id !== ALL ? { ProductId: [id] } : {});
  };

  const hasSelection =
    opcoId !== ALL || envId !== ALL || platformId !== ALL || productId !== ALL;

  const handleFilter = () => {
    pendingFilterRef.current = true;
    GetAssetLevelReportForExodusGrid({
      opcoId: opcoId !== ALL ? [opcoId] : [],
      EnvironmentId: envId !== ALL ? [envId] : [],
      PlatformId: platformId !== ALL ? [platformId] : [],
      ProductId: productId !== ALL ? [productId] : [],
    } as any);
  };

  const handleFilterNewTab = () => {
    if (!hasSelection) return;
    const filterObj: Record<string, string> = {};
    if (opcoId !== ALL) filterObj["opcoId"] = String(opcoId);
    if (envId !== ALL) filterObj["envId"] = String(envId);
    if (platformId !== ALL) filterObj["platformId"] = String(platformId);
    if (productId !== ALL) filterObj["productId"] = String(productId);
    window.open(
      `/exodusassettimeline/?${new URLSearchParams(filterObj).toString()}`,
      "_blank"
    );
  };

  const wrapperRef = useRef<HTMLDivElement>(null);
  const [containerWidth, setContainerWidth] = useState(0);

  useEffect(() => {
    const measure = () => {
      if (wrapperRef.current) setContainerWidth(wrapperRef.current.offsetWidth);
    };
    measure();
    const timer = setTimeout(measure, 50);
    window.addEventListener("resize", measure);
    return () => {
      clearTimeout(timer);
      window.removeEventListener("resize", measure);
    };
  }, []);

  const LEFT_WIDTH = 360;
  const ROW_HEIGHT = 64;
  const BAR_HEIGHT = 18;
  const DIAMOND_SIZE = 14;
  const DIAMOND_TOP_L0 = 30;
  const DIAMOND_TOP_L1 = 14;
  const LANE_MIN_GAP = DIAMOND_SIZE + 2;

  const parseDate = (str?: string | null): Date | null => {
    if (!str) return null;
    if (str.includes("T") || /^\d{4}-\d{2}-\d{2}/.test(str)) {
      const d = new Date(str);
      return isNaN(d.getTime()) ? null : d;
    }
    const parts = str.split("-").map(Number);
    if (parts.length !== 3 || parts.some((n) => isNaN(n))) return null;
    const [d, m, y] = parts;
    return new Date(y, m - 1, d);
  };

  const allDates: Date[] = useMemo(() => {
    const dates: Date[] = [];
    rows.forEach((d) => {
      (d.activities ?? []).forEach((a) => {
        const s = parseDate(a.activityStartDate);
        const e = parseDate(a.activityEndDate);
        if (s) dates.push(s);
        if (e) dates.push(e);
      });
    });
    return dates;
  }, [rows]);

  const { timelineStart, timelineEnd, months } = useMemo(() => {
    const fallback = new Date();
    const minDate = allDates.length
      ? new Date(Math.min(...allDates.map((d) => d.getTime())))
      : fallback;
    const maxDate = allDates.length
      ? new Date(Math.max(...allDates.map((d) => d.getTime())))
      : fallback;
    const start = new Date(minDate);
    start.setMonth(start.getMonth() - 4);

    let end = new Date(maxDate);
    end.setMonth(end.getMonth() + 6);
    const monthsBetween = (a: Date, b: Date) =>
      (b.getFullYear() - a.getFullYear()) * 12 + (b.getMonth() - a.getMonth());
    if (monthsBetween(start, end) < MIN_TIMELINE_MONTHS) {
      end = new Date(start);
      end.setMonth(end.getMonth() + MIN_TIMELINE_MONTHS);
    }
    const ms: Date[] = [];
    const cur = new Date(start);
    while (cur <= end) {
      ms.push(new Date(cur));
      cur.setMonth(cur.getMonth() + 1);
    }
    return { timelineStart: start, timelineEnd: end, months: ms };
  }, [allDates]);

  const effectiveWidth =
    containerWidth > 0
      ? containerWidth
      : typeof window !== "undefined"
      ? window.innerWidth - 80
      : 1000;
  const MONTH_WIDTH =
    months.length > 0
      ? Math.max(25, Math.floor((effectiveWidth - LEFT_WIDTH) / months.length))
      : 25;
  const areaTop = ROW_HEIGHT - BAR_HEIGHT - 6;

  const getMonthIndex = (date: Date) =>
    (date.getFullYear() - timelineStart.getFullYear()) * 12 +
    (date.getMonth() - timelineStart.getMonth());

  const FY_START_MONTH = 3;
  const totalWidth = months.length * MONTH_WIDTH;

  const fiscalYears = useMemo(() => {
    let startFY = timelineStart.getFullYear();
    if (timelineStart.getMonth() < FY_START_MONTH) startFY--;
    let endFY = timelineEnd.getFullYear();
    if (timelineEnd.getMonth() < FY_START_MONTH) endFY--;
    const fys: { label: string; left: number; width: number }[] = [];
    for (let f = startFY; f <= endFY; f++) {
      const startIdx = getMonthIndex(new Date(f, FY_START_MONTH, 1));
      const rawLeft = startIdx * MONTH_WIDTH;
      const left = Math.max(rawLeft, 0);
      const right = Math.min(rawLeft + 12 * MONTH_WIDTH, totalWidth);
      fys.push({
        label: `FY${f.toString().slice(-2)}/${(f + 1).toString().slice(-2)}`,
        left,
        width: right - left,
      });
    }
    return fys;
  }, [timelineStart, timelineEnd, months, totalWidth]);

  const estWidth = (text: string) => text.length * 6.6 + 12;

  const buildTimeline = (d: Row) => {
    const acts = [...(d.activities ?? [])].sort(
      (a, b) => (a.activityOrder ?? 0) - (b.activityOrder ?? 0)
    );

    const multiMonthBars: BarItem[] = [];
    const diamonds: DiamondItem[] = [];

    type Agg = {
      label: string;
      order: number;
      startDt: Date | null;
      endDt: Date | null;
      startStr: string | null;
      endStr: string | null;
    };
    const aggs: Agg[] = [];

    acts.forEach((a) => {
      const s = parseDate(a.activityStartDate);
      const e = parseDate(a.activityEndDate);
      if (!s && !e) return;

      const dts = [s, e].filter(Boolean) as Date[];
      const blockStart = new Date(Math.min(...dts.map((dt) => dt.getTime())));
      const blockEnd = new Date(Math.max(...dts.map((dt) => dt.getTime())));

      const prev = aggs[aggs.length - 1];
      const isContinuation =
        !!prev &&
        a.activityDescription
          ?.toLowerCase()
          .startsWith(prev.label.toLowerCase() + " ");

      if (isContinuation && prev) {
        if (prev.startDt && blockStart < prev.startDt)
          prev.startDt = blockStart;
        if (prev.endDt && blockEnd > prev.endDt) prev.endDt = blockEnd;
        prev.endStr = a.activityEndDate ?? a.activityStartDate ?? prev.endStr;
      } else {
        aggs.push({
          label: a.activityDescription,
          order: a.activityOrder,
          startDt: blockStart,
          endDt: blockEnd,
          startStr: a.activityStartDate ?? a.activityEndDate,
          endStr: a.activityEndDate ?? a.activityStartDate,
        });
      }
    });
    aggs.forEach((b) => {
      if (!b.startDt || !b.endDt) return;

      const bg =
        activityColorMap.get(b.label) || ACTIVITY_COLORS[b.order] || "#888";
      const getStartPos = (dt: Date) => {
        const daysInMonth = new Date(
          dt.getFullYear(),
          dt.getMonth() + 1,
          0
        ).getDate();
        return (
          (getMonthIndex(dt) + (dt.getDate() - 1) / daysInMonth) * MONTH_WIDTH
        );
      };

      const getEndPos = (dt: Date) => {
        const daysInMonth = new Date(
          dt.getFullYear(),
          dt.getMonth() + 1,
          0
        ).getDate();
        return (getMonthIndex(dt) + dt.getDate() / daysInMonth) * MONTH_WIDTH;
      };

      const left = getStartPos(b.startDt);
      const right = getEndPos(b.endDt);
      const width = Math.max(right - left, 10);

      multiMonthBars.push({
        label: b.label,
        short: b.label,
        code: acronym(b.label),
        bg,
        color: textOn(bg),
        startStr: b.startStr,
        endStr: b.endStr,
        left,
        width,
      });
    });

    const colToKey: Record<string, keyof Row> = {
      bomstartdate: "BOMStart",
      bomsubmitteddate: "BOMSubmitted",
      hwporaiseddate: "HWPORaised",
      hwpoarriveddate: "HWPOArrived",
      vecdate: "VEC",
      startofappintegration: "StartOfAppIntegration",
      rfodate: "RFO",
      rfadate: "RFA",
      migrationstart: "MigrationStart",
      rfsdate: "RFS",
      migrationcompletiondate: "MigrationCompletion",
    };

    acts.forEach((a) => {
      const cols = (a.actualColumnName || "")
        .split(",")
        .map((s) => s.trim())
        .filter(Boolean);
      const labels = (a.mileStoneDescription || "")
        .split(",")
        .map((s) => s.trim())
        .filter(Boolean);

      cols.forEach((col, i) => {
        const label =
          labels[i] || COLUMN_LABELS[col.toLowerCase()] || humanize(col);

        let dateStr: string | null = null;
        const mappedKey = colToKey[col.toLowerCase()];

        if (mappedKey) {
          dateStr = d[mappedKey] as string | null;
        } else {
          const primary = i === 0;
          dateStr = primary
            ? a.activityEndDate ?? a.activityStartDate
            : a.activityStartDate ?? a.activityEndDate;
        }

        const dt = parseDate(dateStr);
        if (!dt) return;
        const daysInMonth = new Date(
          dt.getFullYear(),
          dt.getMonth() + 1,
          0
        ).getDate();
        const exactCenter =
          (getMonthIndex(dt) + (dt.getDate() - 1) / daysInMonth) * MONTH_WIDTH;

        diamonds.push({
          center: exactCenter,
          color: milestoneColorMap.get(label) || "#375623",
          short: acronym(label),
          full: label,
          dateStr,
        });
      });
    });
    diamonds.sort((a, b) => a.center - b.center);

    return {
      multiMonthBars,
      chipGroups: [] as { center: number; group: ChipItem[] }[],
      diamonds,
    };
  };

  const allRowsEmpty =
    rows.length === 0 ||
    rows.every((d) => {
      const { multiMonthBars, chipGroups } = buildTimeline(d);
      return multiMonthBars.length === 0 && chipGroups.length === 0;
    });

  const styles: Record<string, React.CSSProperties> = {
    card: {
      border: "1px solid #ddd",
      borderRadius: 8,
      overflow: "hidden",
      boxShadow: "0 1px 4px rgba(0,0,0,0.06)",
      background: "#fff",
      display: "flex",
      flexDirection: "column",
      maxHeight: "calc(100vh - 200px)",
      minHeight: 0,
      width: "100%",
    },
    legend: {
      display: "flex",
      flexWrap: "wrap",
      gap: 10,
      padding: "10px 14px",
      background: "#fff",
      borderBottom: "1px solid #eee",
      fontSize: 11,
      color: "#444",
      alignItems: "center",
    },
    legendItem: {
      display: "flex",
      alignItems: "center",
      gap: 5,
      whiteSpace: "nowrap",
    },
    timelineHeader: {
      position: "sticky",
      top: 0,
      zIndex: 60,
      background: "#f5f5f5",
    },
    badge: {
      minWidth: 20,
      height: 18,
      padding: "0 5px",
      borderRadius: 4,
      display: "inline-flex",
      alignItems: "center",
      justifyContent: "center",
      fontSize: 11,
      fontWeight: 800,
      flexShrink: 0,
      border: "1px solid rgba(0,0,0,0.1)",
    },
    wrapper: {
      overflowX: "auto",
      background: "#f5f5f5",
      flex: 1,
      minHeight: 0,
    },
    container: {
      display: "flex",
      minWidth: months.length * MONTH_WIDTH + LEFT_WIDTH,
    },
    left: {
      width: LEFT_WIDTH,
      borderRight: "2px solid #000",
      background: "#eee",
      flexShrink: 0,
      position: "sticky",
      left: 0,
      zIndex: 70,
      boxShadow: "2px 0 4px rgba(0,0,0,0.12)",
      overflow: "hidden",
    },
    leftHeader: {
      height: 54,
      display: "grid",
      gridTemplateColumns: "60px 60px 70px 1fr 70px",
      alignItems: "end",
      padding: "0 12px 6px",
      boxSizing: "border-box",
      borderBottom: "1px solid #999",
      fontSize: 10,
      fontWeight: 700,
      textTransform: "uppercase",
      letterSpacing: 0.3,
      color: "#888",
      position: "sticky",
      top: 0,
      zIndex: 80,
      background: "#eee",
    },
    leftCell: {
      height: ROW_HEIGHT,
      display: "grid",
      gridTemplateColumns: "60px 60px 70px 1fr 70px",
      alignItems: "center",
      padding: "0 12px",
      boxSizing: "border-box",
      borderBottom: "1px solid #ddd",
      fontSize: 13,
      fontWeight: 600,
      background: "#eee",
    },
    locVal: {
      color: "#666",
      fontWeight: 500,
      whiteSpace: "nowrap",
      overflow: "hidden",
      textOverflow: "ellipsis",
    },
    assetVal: {
      fontWeight: 500,
      color: "#666",
      whiteSpace: "nowrap",
      overflow: "hidden",
      textOverflow: "ellipsis",
    },
    tragetStackVal: {
      color: "#444",
      fontWeight: 500,
      whiteSpace: "nowrap",
      overflow: "hidden",
      textOverflow: "ellipsis",
    },
    vendorNfVal: {
      color: "#666",
      fontWeight: 500,
      whiteSpace: "nowrap",
      overflow: "hidden",
      textOverflow: "ellipsis",
    },
    timeline: {
      position: "relative",
      width: months.length * MONTH_WIDTH,
      background: "#f5f5f5",
    },
    fyRow: {
      position: "relative",
      height: 28,
      background: "#e9e9e9",
      borderBottom: "1px solid #999",
    },
    fy: {
      position: "absolute",
      top: 0,
      textAlign: "center",
      fontWeight: 700,
      fontSize: 12,
      color: "#6a1b4d",
      borderRight: "2px solid #000",
      boxSizing: "border-box",
      lineHeight: "28px",
    },
    monthRow: { display: "flex", height: 26, borderBottom: "1px solid #ccc" },
    month: {
      width: MONTH_WIDTH,
      fontSize: 10,
      textAlign: "center",
      color: "#555",
      lineHeight: "26px",
    },
    grid: { position: "relative", background: "#f5f5f5" },
    tick: {
      position: "absolute",
      top: 0,
      bottom: 0,
      width: 1,
      background: "#e0e0e0",
    },
    monthBand: {
      position: "absolute",
      top: 0,
      bottom: 0,
      boxSizing: "border-box",
    },
    row: {
      position: "relative",
      height: ROW_HEIGHT,
      borderBottom: "1px solid #ddd",
    },
    emptyRowNote: {
      position: "absolute",
      left: 8,
      top: (ROW_HEIGHT - 16) / 2,
      fontSize: 10,
      fontStyle: "italic",
      color: "#c4c4c4",
    },
    bar: {
      position: "absolute",
      height: BAR_HEIGHT,
      top: areaTop,
      borderRadius: 4,
      fontSize: 11,
      fontWeight: 800,
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      padding: "0 4px",
      whiteSpace: "nowrap",
      overflow: "hidden",
      boxSizing: "border-box",
      border: "1px solid rgba(255,255,255,0.85)",
    },
    barText: {
      flex: 1,
      minWidth: 0,
      overflow: "hidden",
      textOverflow: "ellipsis",
      textAlign: "center",
    },
    chip: {
      position: "absolute",
      height: BAR_HEIGHT,
      top: areaTop,
      borderRadius: 4,
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      gap: 3,
      padding: "0 4px",
      whiteSpace: "nowrap",
      boxSizing: "border-box",
      background: "#fff",
      border: "1px solid #ccc",
      cursor: "default",
    },
    chipCode: {
      display: "inline-flex",
      alignItems: "center",
      justifyContent: "center",
      minWidth: 16,
      height: 16,
      borderRadius: 3,
      fontSize: 10,
      fontWeight: 800,
      padding: "0 3px",
    },
    emptyState: {
      padding: "60px 20px",
      textAlign: "center",
      color: "#999",
      fontSize: 14,
    },
  };

  const Legend = (
    <div style={styles.legend}>
      <span style={{ fontWeight: 700, color: "#333", fontSize: 11 }}>
        Activities:
      </span>
      {activityLegend.map((p) => (
        <span key={p.label} style={styles.legendItem}>
          <span
            style={{
              width: 16,
              height: 12,
              borderRadius: 3,
              background: p.color,
              border: "1px solid rgba(0,0,0,0.15)",
              flexShrink: 0,
            }}
          />
          {p.label}
        </span>
      ))}
      <span style={{ width: "100%", height: 0 }} />
      <span style={{ fontWeight: 700, color: "#333", fontSize: 11 }}>
        Milestones:
      </span>
      {milestoneLegend.map((m) => (
        <span key={m.label} style={styles.legendItem}>
          <span
            style={{
              width: 10,
              height: 10,
              background: m.color,
              transform: "rotate(45deg)",
              display: "inline-block",
              border: "1.5px solid #fff",
              boxShadow: "0 0 0 1px rgba(0,0,0,0.2)",
              flexShrink: 0,
            }}
          />
          {m.label}
        </span>
      ))}
    </div>
  );

  const dynamicPhases = useMemo(() => {
    return activityLegend.map((act) => ({
      startKey: "",
      endKey: "",
      label: act.label,
      short: act.label,
      code: acronym(act.label),
      bg: act.color,
      color: textOn(act.color),
    }));
  }, [activityLegend]);

  const dynamicMilestones = useMemo(() => {
    return milestoneLegend.map((m) => ({
      key: acronym(m.label),
      color: m.color,
      short: acronym(m.label),
      full: m.label,
    }));
  }, [milestoneLegend]);

  const handleDownloadExcel = () =>
    downloadExcel({
      rows,
      months,
      phases: dynamicPhases,
      milestones: dynamicMilestones,
      milestoneKeys: MILESTONE_KEYS as string[],
      fyStartMonth: FY_START_MONTH,
      filterHeadings: filterHeadingObj,
      leftColumns: [
        { header: "OpCo", field: "OpCo", width: 18 },
        { header: "Vendor NF", field: "vendorNf", width: 18 },
        { header: "Loc", field: "Location", width: 16 },
        { header: "Asset Name", field: "AssetName", width: 26 },
        { header: "Target Stack", field: "tragetStack", width: 18 },
      ],
      title: "Exodus-Asset Timeline (Under Development)",
      fileName: "Exodus-Asset-Timeline.xlsx",
    });

  return (
    <ThemeProvider theme={theme}>
      <Box
        sx={{
          width: "100%",
          padding: "16px 16px 16px 66px",
          boxSizing: "border-box",
        }}
      >
        <Grid container spacing={2}>
          <Grid size={12}>
            <Grid
              size={12}
              sx={{
                minHeight: "8vh",
                alignItems: "center",
                justifyContent: "space-between",
                display: "flex",
                flexWrap: "wrap",
                rowGap: 1,
              }}
            >
              <Typography
                variant="h4"
                sx={{
                  marginBottom: 0,
                  display: "flex",
                  alignItems: "center",
                  flexWrap: "wrap",
                  fontSize: "1.8rem !important",
                  color: (t) => t.palette.text.primary,
                }}
              >
                <HtmlTooltip
                  title={
                    <React.Fragment>
                      {"Exodus-Asset Timeline (Under Development)"}
                    </React.Fragment>
                  }
                >
                  <Button
                    startIcon={<FcInfo />}
                    sx={{
                      width: "max-content",
                      fontWeight: "bold",
                      fontSize: "24px !important",
                      color: (t) => t.palette.text.primary,
                    }}
                  >
                    Exodus-Asset Timeline (Under Development)
                  </Button>
                </HtmlTooltip>
                {filterHeadingObj.map(({ title, value }) => (
                  <Tooltip
                    key={title}
                    title={
                      value.length > 3 ? (
                        <div
                          style={{
                            maxWidth: 400,
                            maxHeight: 300,
                            overflow: "auto",
                          }}
                        >
                          <Typography
                            variant="subtitle2"
                            sx={{ fontWeight: "bold", mb: 1 }}
                          >
                            {title} ({value.length})
                          </Typography>
                          {value.map((item, index) => (
                            <Chip
                              key={index}
                              label={item}
                              size="small"
                              sx={{ m: "2px", fontSize: "11px" }}
                            />
                          ))}
                        </div>
                      ) : (
                        ""
                      )
                    }
                    arrow
                    placement="bottom"
                  >
                    <Chip
                      sx={{
                        minWidth: "4rem",
                        marginLeft: "0.5rem",
                        maxWidth: "320px",
                        height: "auto",
                        cursor: "default",
                        "& .MuiChip-label": {
                          display: "block",
                          whiteSpace: "normal",
                          padding: "4px 8px",
                        },
                      }}
                      variant="outlined"
                      color="primary"
                      label={
                        value.length > 3
                          ? `${title} : ${value.slice(0, 3).join(", ")} +${
                              value.length - 3
                            } more`
                          : `${title} : ${value.join(", ")}`
                      }
                    />
                  </Tooltip>
                ))}
              </Typography>

              {!isReadOnlyView && (
                <div
                  className="d-flex"
                  style={{
                    gap: "12px",
                    alignItems: "flex-start",
                    textAlign: "left",
                  }}
                >
                  {[
                    {
                      label: "OpCo",
                      options: opcoOptions,
                      value: opcoId,
                      handler: handleOpco,
                      clearable: opcoId !== ALL,
                    },
                    {
                      label: "Environment",
                      options: envOptions,
                      value: envId,
                      handler: handleEnv,
                      clearable: envId !== ALL,
                    },
                    {
                      label: "Target Stack",
                      options: platformOptions,
                      value: platformId,
                      handler: handlePlatform,
                      clearable: platformId !== ALL,
                    },
                    {
                      label: "Vendor NF",
                      options: productOptions,
                      value: productId,
                      handler: handleProduct,
                      clearable: productId !== ALL,
                    },
                  ].map(({ label, options, value, handler, clearable }) => (
                    <label
                      key={label}
                      className="labelForm voda-bold w-100 mb-0"
                      style={{ display: "grid", gap: "4px" }}
                    >
                      <span>{label}</span>
                      <Select
                        options={options}
                        value={options.find((o) => o.value === value) ?? null}
                        onChange={(opt: any) => handler(opt ? opt.value : ALL)}
                        isClearable={clearable}
                        classNamePrefix="react-select"
                        menuPortalTarget={
                          typeof document !== "undefined"
                            ? document.body
                            : undefined
                        }
                        styles={{
                          container: (b) => ({
                            ...b,
                            minWidth: 200,
                            maxWidth: 280,
                          }),
                          menuPortal: (b) => ({ ...b, zIndex: 9999 }),
                        }}
                      />
                    </label>
                  ))}
                  <div style={{ placeSelf: "end" }}>
                    <BootstrapDropdown as={BootstrapButtonGroup}>
                      <BootstrapButton
                        variant="danger"
                        onClick={handleFilter}
                        style={{
                          width: "max-content",
                          minWidth: "4rem",
                          borderRadius: "4px 0px 0px 4px",
                          boxShadow: "none",
                        }}
                      >
                        Apply Filter
                      </BootstrapButton>
                      <BootstrapDropdown.Toggle
                        split
                        variant="danger"
                        style={{
                          borderRadius: "0px 4px 4px 0px",
                          boxShadow: "none",
                        }}
                      />
                      <BootstrapDropdown.Menu
                        align="end"
                        style={{ padding: 0 }}
                      >
                        <BootstrapDropdown.Item
                          onClick={handleFilterNewTab}
                          disabled={!hasSelection}
                        >
                          Filter in New Tab
                        </BootstrapDropdown.Item>
                      </BootstrapDropdown.Menu>
                    </BootstrapDropdown>
                  </div>
                </div>
              )}
              <button
                className="download-to-excel"
                style={{
                  width: "fit-content",
                  marginTop: "20px",
                  marginRight: "15px",
                }}
                disabled={allRowsEmpty}
                onClick={handleDownloadExcel}
              >
                Download to Excel
              </button>
            </Grid>
          </Grid>

          <Grid size={12}>
            <div style={styles.card}>
              {Legend}
              {allRowsEmpty ? (
                <div style={styles.emptyState}>
                  No scheduled milestones for the selected filter.
                </div>
              ) : (
                <div ref={wrapperRef} style={styles.wrapper}>
                  <div style={styles.container}>
                    <div style={styles.left}>
                      <div style={styles.leftHeader}>
                        <span>OpCo</span>
                        <span>Vendor NF</span>
                        <span>Loc</span>
                        <span>Asset Name</span>
                        <span>Target Stack</span>
                      </div>
                      {rows.map((d, i) => (
                        <div key={i} style={styles.leftCell}>
                          <span style={styles.locVal} title={d.OpCo}>
                            {d.OpCo}
                          </span>
                          <span style={styles.vendorNfVal} title={d.vendorNf}>
                            {d.vendorNf}
                          </span>
                          <span style={styles.locVal} title={d.Location}>
                            {d.Location}
                          </span>
                          <span style={styles.assetVal} title={d.AssetName}>
                            {d.AssetName}
                          </span>
                          <span
                            style={styles.tragetStackVal}
                            title={d.tragetStack}
                          >
                            {d.tragetStack}
                          </span>
                        </div>
                      ))}
                    </div>

                    <div style={styles.timeline}>
                      <div style={styles.timelineHeader}>
                        <div style={styles.fyRow}>
                          {fiscalYears.map((fy) => (
                            <div
                              key={fy.label}
                              style={{
                                ...styles.fy,
                                left: fy.left,
                                width: fy.width,
                              }}
                            >
                              {fy.label}
                            </div>
                          ))}
                        </div>
                        <div style={styles.monthRow}>
                          {months.map((m, i) => (
                            <div key={i} style={styles.month}>
                              {m
                                .toLocaleString("en-US", { month: "short" })
                                .toUpperCase()}
                            </div>
                          ))}
                        </div>
                      </div>

                      <div style={styles.grid}>
                        {months.map((_, i) => (
                          <div
                            key={`band-${i}`}
                            style={{
                              ...styles.monthBand,
                              left: i * MONTH_WIDTH,
                              width: MONTH_WIDTH,
                              height: rows.length * ROW_HEIGHT,
                              background: i % 2 === 0 ? "#fafafa" : "#f0f0f0",
                            }}
                          />
                        ))}
                        {months.map((_, i) => (
                          <div
                            key={i}
                            style={{
                              ...styles.tick,
                              left: i * MONTH_WIDTH,
                              height: rows.length * ROW_HEIGHT,
                              opacity: i % 6 === 0 ? 0.4 : 0.15,
                            }}
                          />
                        ))}

                        {rows.map((d, i) => {
                          const { multiMonthBars, chipGroups, diamonds } =
                            buildTimeline(d);
                          const hasAnything =
                            multiMonthBars.length > 0 || chipGroups.length > 0;
                          const sortedBars = [...multiMonthBars].sort(
                            (a, b) => b.width - a.width
                          );

                          return (
                            <div key={i} style={styles.row}>
                              {!hasAnything && (
                                <span style={styles.emptyRowNote}>
                                  No scheduled milestones
                                </span>
                              )}

                              {sortedBars.map((it, k) => {
                                const sd = parseDate(it.startStr);
                                const ed = parseDate(it.endStr);
                                const text =
                                  it.width >= estWidth(it.label)
                                    ? it.label
                                    : it.width >= estWidth(it.short)
                                    ? it.short
                                    : it.code;
                                return (
                                  <div
                                    key={`b-${k}`}
                                    title={`${it.code} · ${it.label}: ${
                                      sd ? fmtDate(sd) : "?"
                                    } → ${ed ? fmtDate(ed) : "?"}`}
                                    style={{
                                      ...styles.bar,
                                      left: it.left,
                                      width: it.width,
                                      background: it.bg,
                                      color: it.color,
                                      zIndex: k,
                                    }}
                                  >
                                    <span style={styles.barText}>{text}</span>
                                  </div>
                                );
                              })}

                              {chipGroups.map((cg, k) => {
                                const chipWidth = 8 + cg.group.length * 19;
                                const left = Math.max(
                                  0,
                                  cg.center - chipWidth / 2
                                );
                                const titleText = cg.group
                                  .map((g) => {
                                    const sd = parseDate(g.startStr);
                                    const ed = parseDate(g.endStr);
                                    return g.isBar
                                      ? `${g.code} · ${g.label}: ${
                                          sd ? fmtDate(sd) : "?"
                                        } → ${ed ? fmtDate(ed) : "?"}`
                                      : `${g.code} · ${g.label}: ${
                                          ed ? fmtDate(ed) : "?"
                                        }`;
                                  })
                                  .join("\n");
                                return (
                                  <div
                                    key={`c-${k}`}
                                    title={titleText}
                                    style={{
                                      ...styles.chip,
                                      left,
                                      zIndex: 30 + k,
                                    }}
                                  >
                                    {cg.group.map((g, gi) => (
                                      <span
                                        key={gi}
                                        style={{
                                          ...styles.chipCode,
                                          background: g.bg,
                                          color: g.color,
                                        }}
                                      >
                                        {g.code}
                                      </span>
                                    ))}
                                  </div>
                                );
                              })}

                              {(() => {
                                const laneEndX = [-Infinity, -Infinity];
                                const placed = diamonds.map((dm) => {
                                  let lane = 0;
                                  if (dm.center - laneEndX[0] < LANE_MIN_GAP)
                                    lane = 1;
                                  if (
                                    dm.center - laneEndX[lane] <
                                    LANE_MIN_GAP
                                  ) {
                                    lane = laneEndX[0] <= laneEndX[1] ? 0 : 1;
                                  }
                                  laneEndX[lane] = dm.center + DIAMOND_SIZE / 2;
                                  return { ...dm, lane };
                                });

                                return placed.map((dm, k) => {
                                  const dt = parseDate(dm.dateStr);
                                  const dTop =
                                    dm.lane === 0
                                      ? DIAMOND_TOP_L0
                                      : DIAMOND_TOP_L1;
                                  const stemHeight = Math.max(
                                    0,
                                    areaTop - (dTop + DIAMOND_SIZE)
                                  );
                                  return (
                                    <React.Fragment key={`d-${k}`}>
                                      {stemHeight > 0 && (
                                        <div
                                          style={{
                                            position: "absolute",
                                            left: dm.center,
                                            top: dTop + DIAMOND_SIZE,
                                            width: 1,
                                            height: stemHeight,
                                            background: dm.color,
                                            opacity: 0.35,
                                            zIndex: 49,
                                          }}
                                        />
                                      )}
                                      <div
                                        title={`${dm.short} · ${dm.full}: ${
                                          dt ? fmtDate(dt) : "?"
                                        }`}
                                        style={{
                                          position: "absolute",
                                          width: DIAMOND_SIZE,
                                          height: DIAMOND_SIZE,
                                          left: dm.center - DIAMOND_SIZE / 2,
                                          top: dTop,
                                          background: dm.color,
                                          transform: "rotate(45deg)",
                                          border:
                                            "1.5px solid rgba(255,255,255,0.9)",
                                          boxShadow:
                                            "0 0 0 1px rgba(0,0,0,0.2)",
                                          zIndex: 50,
                                          cursor: "pointer",
                                          display: "flex",
                                          alignItems: "center",
                                          justifyContent: "center",
                                        }}
                                      >
                                        <span
                                          style={{
                                            transform: "rotate(-45deg)",
                                            fontSize: 7,
                                            fontWeight: 800,
                                            color: "#fff",
                                            lineHeight: 1,
                                            userSelect: "none",
                                            pointerEvents: "none",
                                          }}
                                        >
                                          {dm.short[0]}
                                        </span>
                                      </div>
                                    </React.Fragment>
                                  );
                                });
                              })()}
                            </div>
                          );
                        })}
                      </div>
                    </div>
                  </div>
                </div>
              )}
            </div>
          </Grid>
        </Grid>
      </Box>
    </ThemeProvider>
  );
}
