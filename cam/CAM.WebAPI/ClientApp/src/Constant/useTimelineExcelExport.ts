import ExcelJS from "exceljs";

type RawActivity = {
  activityOrder: number;
  activityDescription: string;
  activityStartDate: string | null;
  activityEndDate: string | null;
  actualColumnName: string | null;
  mileStoneDescription: string | null;
};

export type PhaseDef = {
  startKey: string;
  endKey: string;
  label: string;
  short: string;
  code: string;
  bg: string;
  color: string;
};

export type MilestoneDef = {
  key: string;
  color: string;
  short: string;
  full: string;
};

export type ExcelExportOptions = {
  rows: Record<string, any>[];
  months: Date[];
  phases?: PhaseDef[];
  milestones?: MilestoneDef[];
  milestoneKeys?: string[];
  fyStartMonth: number;
  filterHeadings?: { title: string; value: string[] }[];
  leftColumns?: { header: string; field: string; width?: number }[];
  title?: string;
  fileName?: string;
  sheetName?: string;
};

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

const colorToArgb = (c: string) => {
  let h = c.replace("#", "");
  if (h.length === 3)
    h = h
      .split("")
      .map((ch) => ch + ch)
      .join("");
  return "FF" + h.toUpperCase();
};

const textArgb = (hex: string) => {
  const c = hex.replace("#", "");
  const r = parseInt(c.slice(0, 2), 16);
  const g = parseInt(c.slice(2, 4), 16);
  const b = parseInt(c.slice(4, 6), 16);
  return (r * 299 + g * 587 + b * 114) / 1000 > 150 ? "FF000000" : "FFFFFFFF";
};

const stripHtml = (v: any) =>
  String(v ?? "")
    .replace(/<[^>]*>/g, "")
    .replace(/&nbsp;/g, " ")
    .replace(/&amp;/g, "&")
    .replace(/&lt;/g, "<")
    .replace(/&gt;/g, ">")
    .replace(/\s+/g, " ")
    .trim();

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

export function useTimelineExcelExport() {
  const downloadExcel = async (opts: ExcelExportOptions) => {
    const {
      rows,
      months,
      fyStartMonth,
      filterHeadings = [],
      leftColumns = [
        { header: "OpCo", field: "OpCo", width: 18 },
        { header: "Vendor NF", field: "vendorNf", width: 18 },
        { header: "Loc", field: "Location", width: 16 },
        { header: "Asset Name", field: "AssetName", width: 26 },
        { header: "Target Stack", field: "tragetStack", width: 18 },
      ],
      title = "Exodus-Asset Timeline",
      fileName = "Exodus-Asset-Timeline.xlsx",
      sheetName = "Exodus Timeline",
    } = opts;

    if (!months.length) return;

    const start = months[0];
    const monthIndexOf = (d: Date) =>
      (d.getFullYear() - start.getFullYear()) * 12 +
      (d.getMonth() - start.getMonth());

    const fyForMonth = (dt: Date) => {
      let y = dt.getFullYear();
      if (dt.getMonth() < fyStartMonth) y--;
      return `FY${y.toString().slice(-2)}/${(y + 1).toString().slice(-2)}`;
    };

    const sortActs = (d: Record<string, any>): RawActivity[] =>
      [...((d.activities as RawActivity[]) ?? [])].sort(
        (a, b) => (a.activityOrder ?? 0) - (b.activityOrder ?? 0)
      );

    const activityColor = new Map<string, string>();
    const activityLegend: { label: string; color: string; code: string }[] = [];
    const milestoneColor = new Map<string, string>();
    const milestoneLegend: { label: string; color: string }[] = [];
    let milestoneSeq = 0;

    rows.forEach((d) => {
      let prevLabel: string | null = null;
      sortActs(d).forEach((a) => {
        const desc = a.activityDescription || "";
        const isCont =
          prevLabel &&
          desc.toLowerCase().startsWith(prevLabel.toLowerCase() + " ");
        if (!isCont && desc) {
          if (!activityColor.has(desc)) {
            const color = ACTIVITY_COLORS[a.activityOrder] || "#888888";
            activityColor.set(desc, color);
            activityLegend.push({ label: desc, color, code: acronym(desc) });
          }
          prevLabel = desc;
        }
        const cols = (a.actualColumnName || "")
          .split(",")
          .map((s) => s.trim())
          .filter(Boolean);
        const labels = (a.mileStoneDescription || "")
          .split(",")
          .map((s) => s.trim());
        cols.forEach((col, i) => {
          const mlabel =
            labels[i] || COLUMN_LABELS[col.toLowerCase()] || humanize(col);
          if (mlabel && !milestoneColor.has(mlabel)) {
            const color =
              MILESTONE_PALETTE[milestoneSeq % MILESTONE_PALETTE.length];
            milestoneSeq++;
            milestoneColor.set(mlabel, color);
            milestoneLegend.push({ label: mlabel, color });
          }
        });
      });
    });

    const buildBars = (d: Record<string, any>) => {
      const bars: {
        startIdx: number;
        endIdx: number;
        label: string;
        color: string;
        code: string;
      }[] = [];
      let prev: (typeof bars)[number] | null = null;
      sortActs(d).forEach((a) => {
        const desc = a.activityDescription || "";
        const idxs: number[] = [];
        const s = parseDate(a.activityStartDate);
        if (s) idxs.push(monthIndexOf(s));
        const e = parseDate(a.activityEndDate);
        if (e) idxs.push(monthIndexOf(e));
        const isCont =
          prev && desc.toLowerCase().startsWith(prev.label.toLowerCase() + " ");
        if (isCont) {
          if (idxs.length && prev) {
            prev.startIdx = Math.min(prev.startIdx, ...idxs);
            prev.endIdx = Math.max(prev.endIdx, ...idxs);
          }
          return;
        }
        if (!idxs.length) return;
        const bar = {
          startIdx: Math.min(...idxs),
          endIdx: Math.max(...idxs),
          label: desc,
          color:
            activityColor.get(desc) ||
            ACTIVITY_COLORS[a.activityOrder] ||
            "#888888",
          code: acronym(desc),
        };
        bars.push(bar);
        prev = bar;
      });
      return bars;
    };

    const buildDiamonds = (d: Record<string, any>) => {
      const out: { idx: number; color: string }[] = [];
      const colToKey: Record<string, string> = {
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

      sortActs(d).forEach((a) => {
        const cols = (a.actualColumnName || "")
          .split(",")
          .map((s) => s.trim())
          .filter(Boolean);
        const labels = (a.mileStoneDescription || "")
          .split(",")
          .map((s) => s.trim());

        cols.forEach((col, i) => {
          const mlabel =
            labels[i] || COLUMN_LABELS[col.toLowerCase()] || humanize(col);

          let dtStr: string | null = null;
          const mappedKey = colToKey[col.toLowerCase()];

          if (mappedKey) {
            dtStr = d[mappedKey] as string | null;
          } else {
            const primary = i === 0;
            dtStr = primary
              ? a.activityEndDate ?? a.activityStartDate
              : a.activityStartDate ?? a.activityEndDate;
          }

          const dt = parseDate(dtStr);
          if (!dt) return;

          const adjustedDt = new Date(dt);

          if (mappedKey === "MigrationCompletion") {
            adjustedDt.setDate(adjustedDt.getDate() - 1);
          }

          const idx = monthIndexOf(adjustedDt);
          if (idx < 0 || idx >= months.length) return;

          out.push({ idx, color: milestoneColor.get(mlabel) || "#333333" });
        });
      });
      return out;
    };

    const workbook = new ExcelJS.Workbook();
    const ws = workbook.addWorksheet(sheetName);

    const LEFT_COLS = leftColumns.length;
    const FIRST_MONTH_COL = LEFT_COLS + 1;
    const TOTAL_COLS = LEFT_COLS + months.length;
    const HEADER_ROWS = 5;
    const FIRST_DATA_ROW = HEADER_ROWS + 1;

    ws.views = [{ state: "frozen", xSplit: 0, ySplit: HEADER_ROWS }];

    leftColumns.forEach((c, i) => (ws.getColumn(i + 1).width = c.width ?? 18));
    for (let i = 0; i < months.length; i++)
      ws.getColumn(FIRST_MONTH_COL + i).width = 6;

    ws.mergeCells(1, 1, 1, TOTAL_COLS);
    const filterText = filterHeadings
      .map((f) => `${f.title}: ${f.value.join(", ")}`)
      .join("   |   ");
    const titleCell = ws.getCell(1, 1);
    titleCell.value = filterText ? `${title}   (${filterText})` : title;
    titleCell.font = { bold: true, size: 14 };
    titleCell.alignment = { vertical: "middle" };
    ws.getRow(1).height = 22;

    const drawLegend = (
      rowNum: number,
      items: { label: string; color: string; code?: string }[],
      prefix: (it: any) => string
    ) => {
      if (!items.length) return;
      const w = Math.max(1, Math.floor(TOTAL_COLS / items.length));
      items.forEach((it, idx) => {
        const startCol = 1 + idx * w;
        const endCol = idx === items.length - 1 ? TOTAL_COLS : startCol + w - 1;
        ws.mergeCells(rowNum, startCol, rowNum, endCol);
        const cell = ws.getCell(rowNum, startCol);
        cell.value = prefix(it);
        cell.fill = {
          type: "pattern",
          pattern: "solid",
          fgColor: { argb: colorToArgb(it.color) },
        };
        cell.font = {
          bold: true,
          size: 9,
          color: { argb: textArgb(it.color) },
        };
        cell.alignment = { vertical: "middle", horizontal: "center" };
      });
    };

    drawLegend(2, activityLegend, (it) => `${it.code} — ${it.label}`);
    ws.getRow(2).height = 18;
    drawLegend(3, milestoneLegend, (it) => `◆ — ${it.label}`);
    ws.getRow(3).height = 18;

    leftColumns.forEach((c, i) => {
      ws.mergeCells(4, i + 1, 5, i + 1);
      const cell = ws.getCell(4, i + 1);
      cell.value = c.header;
      cell.font = { bold: true };
      cell.alignment = { vertical: "middle", horizontal: "center" };
      cell.fill = {
        type: "pattern",
        pattern: "solid",
        fgColor: { argb: "FFEFEFEF" },
      };
    });

    let mi = 0;
    while (mi < months.length) {
      const label = fyForMonth(months[mi]);
      let mj = mi;
      while (mj + 1 < months.length && fyForMonth(months[mj + 1]) === label)
        mj++;
      ws.mergeCells(4, FIRST_MONTH_COL + mi, 4, FIRST_MONTH_COL + mj);
      const cell = ws.getCell(4, FIRST_MONTH_COL + mi);
      cell.value = label;
      cell.font = { bold: true, color: { argb: "FF6A1B4D" } };
      cell.alignment = { vertical: "middle", horizontal: "center" };
      cell.fill = {
        type: "pattern",
        pattern: "solid",
        fgColor: { argb: "FFE9E9E9" },
      };
      mi = mj + 1;
    }
    ws.getRow(4).height = 20;

    months.forEach((m, idx) => {
      const cell = ws.getCell(5, FIRST_MONTH_COL + idx);
      cell.value = m.toLocaleString("en-US", { month: "short" }).toUpperCase();
      cell.alignment = { vertical: "middle", horizontal: "center" };
      cell.font = { size: 9, color: { argb: "FF555555" } };
      cell.fill = {
        type: "pattern",
        pattern: "solid",
        fgColor: { argb: "FFF0F0F0" },
      };
    });
    ws.getRow(5).height = 18;

    rows.forEach((d, r) => {
      const milestoneRow = FIRST_DATA_ROW + r * 2;
      const activityRow = FIRST_DATA_ROW + r * 2 + 1;

      leftColumns.forEach((c, i) => {
        ws.mergeCells(milestoneRow, i + 1, activityRow, i + 1);
        const cell = ws.getCell(milestoneRow, i + 1);
        cell.value = stripHtml(d[c.field]);
        cell.alignment = {
          wrapText: true,
          vertical: "middle",
          horizontal: "left",
        };
        cell.border = {
          bottom: { style: "thin", color: { argb: "FFDDDDDD" } },
        };
      });

      const bars = buildBars(d);
      const diamonds = buildDiamonds(d);

      const byCol = new Map<number, string[]>();
      diamonds.forEach((dm) => {
        const list = byCol.get(dm.idx) ?? [];
        list.push(dm.color);
        byCol.set(dm.idx, list);
      });
      byCol.forEach((colors, idx) => {
        const cell = ws.getCell(milestoneRow, FIRST_MONTH_COL + idx);
        cell.value = {
          richText: colors.map((c) => ({
            text: "◆",
            font: {
              name: "Calibri",
              size: 11,
              bold: true,
              color: { argb: colorToArgb(c) },
            },
          })),
        };
        cell.fill = {
          type: "pattern",
          pattern: "solid",
          fgColor: { argb: "FFFFFFFF" },
        };
        cell.alignment = { vertical: "middle", horizontal: "center" };
      });
      ws.getRow(milestoneRow).height = 20;

      bars.forEach((seg) => {
        const startVis = Math.max(seg.startIdx, 0);
        const endVis = Math.min(seg.endIdx, months.length - 1);
        if (endVis < startVis) return;
        for (let m = startVis; m <= endVis; m++) {
          const cell = ws.getCell(activityRow, FIRST_MONTH_COL + m);
          cell.fill = {
            type: "pattern",
            pattern: "solid",
            fgColor: { argb: colorToArgb(seg.color) },
          };
          cell.font = {
            bold: true,
            size: 9,
            color: { argb: textArgb(seg.color) },
          };
          cell.alignment = { vertical: "middle", horizontal: "left" };
        }
        const widthMonths = endVis - startVis + 1;
        const cap = widthMonths * 6;
        const text = seg.label.length <= cap ? seg.label : seg.code;
        const labelCell = ws.getCell(activityRow, FIRST_MONTH_COL + startVis);
        labelCell.value = text;
        labelCell.alignment = {
          vertical: "middle",
          horizontal: widthMonths === 1 ? "center" : "left",
        };
      });
      ws.getRow(activityRow).height = 18;

      for (let col = 1; col <= TOTAL_COLS; col++) {
        const cell = ws.getCell(activityRow, col);
        cell.border = {
          ...(cell.border || {}),
          bottom: { style: "thin", color: { argb: "FFCCCCCC" } },
        };
      }
    });

    const lastRow = FIRST_DATA_ROW + rows.length * 2 - 1;
    for (let r = 3; r <= lastRow; r++) {
      const cell = ws.getCell(r, LEFT_COLS);
      cell.border = {
        ...(cell.border || {}),
        right: { style: "medium", color: { argb: "FF000000" } },
      };
    }

    const buf = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buf], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    a.click();
    URL.revokeObjectURL(url);
  };

  return { downloadExcel };
}
