import ExcelJS from "exceljs";

interface GanttTask {
  id: number;
  text: string;
  start: Date;
  end: Date;
  progress: number;
  parent: number;
  type?: "summary" | "milestone";
  open?: boolean;
}

// --- Helper: Format date as dd/mm/yyyy ---
function formatDate(date: Date | string): string {
  return new Date(date).toLocaleDateString("en-GB");
}

// --- Helper: Calculate indent level for tree view ---
function getIndentLevel(task: GanttTask, tasks: GanttTask[]): number {
  let level = 0;
  let t = task;
  while (t.parent && t.parent !== 0) {
    level++;
    const parent = tasks.find((row) => row.id === t.parent);
    if (!parent) break;
    t = parent;
  }
  return level;
}

// --- Helper: Bar color by task type ---
function getTaskColor(task: GanttTask): string {
  return task.type === "summary" ? "FF28A745" : "FF2E86AB";
}

const HEADER_STYLE: Partial<ExcelJS.Style> = {
  font: { color: { argb: "FFFFFFFF" }, bold: true, size: 12 },
  fill: { type: "pattern", pattern: "solid", fgColor: { argb: "FFE60000" } },
  alignment: { vertical: "middle", horizontal: "center", wrapText: true },
  border: {
    top: { style: "thin", color: { argb: "FF000000" } },
    left: { style: "thin", color: { argb: "FF000000" } },
    bottom: { style: "thin", color: { argb: "FF000000" } },
    right: { style: "thin", color: { argb: "FF000000" } },
  },
};
const PHASE_CELL_STYLE: Partial<ExcelJS.Style> = {
  alignment: { vertical: "middle", horizontal: "left" },
  fill: { type: "pattern", pattern: "solid", fgColor: { argb: "FFEEEEEE" } },
};
const DATE_STYLE: Partial<ExcelJS.Style> = {
  alignment: { vertical: "middle", horizontal: "center" },
  fill: { type: "pattern", pattern: "solid", fgColor: { argb: "FFEEEEEE" } },
};
const PROGRESS_STYLE: Partial<ExcelJS.Style> = {
  alignment: { vertical: "middle", horizontal: "center" },
  fill: { type: "pattern", pattern: "solid", fgColor: { argb: "FFEEEEEE" } },
  numFmt: "0%",
};
const BORDER_STYLE: Partial<ExcelJS.Borders> = {
  top: { style: "thin", color: { argb: "FF000000" } },
  left: { style: "thin", color: { argb: "FF000000" } },
  bottom: { style: "thin", color: { argb: "FF000000" } },
  right: { style: "thin", color: { argb: "FF000000" } },
};

const pad = (n: number) => n.toString().padStart(2, "0");

export async function GanttExportToExcel(
  tasks: GanttTask[],
  ganttImageDataUrl?: string,
  title?: string,
  dateType?: string
): Promise<void> {
  try {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet("Gantt Chart");
    let currentRow = 1;

    // Calculate timeline month columns
    const minDate = new Date(Math.min(...tasks.map((t) => t.start.getTime())));
    const maxDate = new Date(Math.max(...tasks.map((t) => t.end.getTime())));
    const dateArray: Date[] = [];
    for (
      let d = new Date(minDate.getFullYear(), minDate.getMonth(), 1);
      d <= new Date(maxDate.getFullYear(), maxDate.getMonth(), 1);
      d = new Date(d.getFullYear(), d.getMonth() + 1, 1)
    ) {
      dateArray.push(new Date(d));
    }

    // Title row
    if (title) {
      worksheet.mergeCells(currentRow, 1, currentRow, 4 + dateArray.length);
      const cell = worksheet.getCell(currentRow, 1);
      cell.value = title;
      cell.font = { size: 14, bold: true };
      cell.alignment = { vertical: "middle", horizontal: "center" };
      worksheet.getRow(currentRow).height = 28;
      currentRow++;
    }

    // Header names & column widths
    const headers = [
      "Delivery Phase",
      `${dateType === "planned" ? "Planned" : "Baseline"} Start Date`,
      `${dateType === "planned" ? "Planned" : "Baseline"}  Completion Date`,
      "Progress %",
      ...dateArray.map((d) =>
        d.toLocaleDateString("en-GB", { month: "short", year: "numeric" })
      ),
    ];
    const headerWidths = headers.map((h, i) =>
      i === 0
        ? Math.min(Math.max(h.length + 4, 9), 40)
        : Math.min(Math.max(h.length + 4, 9), 30)
    );
    worksheet.columns = headerWidths.map((w) => ({ width: w }));

    worksheet.addRow(headers);
    const hr = worksheet.getRow(currentRow);
    hr.height = 23;
    hr.eachCell((cell) => Object.assign(cell, HEADER_STYLE));
    currentRow++;

    const maxNameLength = Math.max(...tasks.map((t) => t.text.length), 20);
    worksheet.getColumn(1).width = Math.min(maxNameLength + 4, 40);

    const newTasks = tasks?.filter((val) => val.type !== "milestone");
    // Data rows
    for (const task of newTasks) {
      const startIdx = dateArray.findIndex(
        (d) =>
          d.getFullYear() === task.start.getFullYear() &&
          d.getMonth() === task.start.getMonth()
      );
      const endIdx = dateArray.findIndex(
        (d) =>
          d.getFullYear() === task.end.getFullYear() &&
          d.getMonth() === task.end.getMonth()
      );
      const hasValidBar =
        startIdx !== -1 && endIdx !== -1 && endIdx >= startIdx;

      // Conditional start/end date display: blank if milestone
      const startDateDisplay =
        task.type === "milestone" ? "" : formatDate(task.start);
      const endDateDisplay =
        task.type === "milestone" ? "" : formatDate(task.end);

      const row = worksheet.addRow([
        task.text,
        startDateDisplay,
        endDateDisplay,
        task.type === "milestone" ? "" : task.progress / 100,
        ...Array(dateArray.length).fill(""),
      ]);
      row.height = 22;

      // Delivery phase style
      const phaseCell = row.getCell(1);
      Object.assign(phaseCell, PHASE_CELL_STYLE);
      phaseCell.alignment = {
        ...phaseCell.alignment!,
        indent: getIndentLevel(task, newTasks),
      };
      if (task.type === "summary")
        phaseCell.font = { ...phaseCell.font, bold: true };

      // Dates style
      [2, 3].forEach((col) => Object.assign(row.getCell(col), DATE_STYLE));

      // Progress style
      Object.assign(row.getCell(4), PROGRESS_STYLE);

      // Borders for all cells
      row.eachCell((cell) => {
        cell.border = BORDER_STYLE;
      });

      // Colored bars or milestone diamond
      if (hasValidBar) {
        const colStart = 5 + startIdx;
        const colEnd = 5 + endIdx;
        if (task.type === "milestone") {
          const diamondCell = row.getCell(colStart);
          diamondCell.value = "◆"; // Unicode diamond
          diamondCell.font = {
            color: { argb: "FFB5651D" }, // orange-brown color (adjust as needed)
            bold: true,
            size: 20,
          };
          diamondCell.fill = {
            type: "pattern",
            pattern: "solid",
            fgColor: { argb: "FFFFFFFF" }, // white background
          };
          diamondCell.border = {
            top: undefined,
            left: undefined,
            bottom: undefined,
            right: undefined,
          }; // No border for a clean look
          diamondCell.alignment = {
            vertical: "middle",
            horizontal: "center",
            wrapText: true,
          };
        } else {
          worksheet.mergeCells(row.number, colStart, row.number, colEnd);
          const barCell = row.getCell(colStart);
          barCell.value = task.text;
          barCell.font = { color: { argb: "FFFFFFFF" }, bold: true };
          barCell.fill = {
            type: "pattern",
            pattern: "solid",
            fgColor: { argb: getTaskColor(task) },
          };
          barCell.alignment = {
            vertical: "middle",
            horizontal: "center",
            wrapText: true,
          };
        }
      }

      currentRow++;
    }

    // --- Optional: Add Gantt screenshot as image ---
    // if (ganttImageDataUrl) {
    //   const base64 = ganttImageDataUrl.replace(/^data:image\/png;base64,/, "");
    //   const imageId = workbook.addImage({ base64, extension: "png" });
    //   worksheet.addImage(imageId, {
    //     tl: { col: 0, row: currentRow },
    //     ext: { width: 900, height: 400 },
    //   });
    //   worksheet.getRow(currentRow).height = 80;
    // }

    // --- Save to file ---
    const now = new Date();
    const timestamp = `${pad(now.getDate())}-${pad(now.getMonth() + 1)}-${now
      .getFullYear()
      .toString()
      .slice(-2)}`;
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = `ProjectPlan_${timestamp}.xlsx`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
  } catch (error) {
    console.error("Failed to export Excel:", error);
    throw error;
  }
}
