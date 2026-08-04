import React, { useRef, useState } from "react";
import { T, ACTIONS_TRIGGER_ATTR } from "./CustomTable.tokens";
import { CustomColumnDef, RowActionsMenuHandle } from "./CustomTable.types";
import { builtInRender } from "./CustomTable.renderers";

const HIGHLIGHT_BG = (T as any).highlightBg ?? "#E9F9EF";
const HIGHLIGHT_BORDER = (T as any).highlightBorder ?? T.red;

interface DataRowProps {
  row: any;
  rowIdx: number;
  columns: CustomColumnDef[];
  stickyOffsets: Record<string, number>;
  rowHeight?: number;
  onRowClick?: (row: any) => void;
  onRowDoubleClick?: (row: any) => void;
  highlighted?: boolean;
}

const DataRow: React.FC<DataRowProps> = ({
  row,
  rowIdx,
  columns,
  stickyOffsets,
  rowHeight = 52,
  onRowClick,
  onRowDoubleClick,
  highlighted = false,
}) => {
  const [hovered, setHovered] = useState(false);
  const menuHandleRef = useRef<RowActionsMenuHandle | null>(null);
  const rowRef = useRef<HTMLTableRowElement>(null);

  const registerMenuRef = (handle: RowActionsMenuHandle | null) => {
    menuHandleRef.current = handle;
  };

  const handleClick = () => {
    onRowClick?.(row);
  };

  const handleDoubleClick = (e: React.MouseEvent) => {
    e.preventDefault();

    if (menuHandleRef.current && rowRef.current) {
      const realTrigger = rowRef.current.querySelector<HTMLElement>(
        `[${ACTIONS_TRIGGER_ATTR}]`
      );
      const triggerRect = realTrigger?.getBoundingClientRect();

      const isOnscreen =
        !!triggerRect &&
        triggerRect.right > 0 &&
        triggerRect.left < window.innerWidth &&
        triggerRect.bottom > 0 &&
        triggerRect.top < window.innerHeight;

      const anchorRect: DOMRect = isOnscreen
        ? (triggerRect as DOMRect)
        : ({
            top: e.clientY,
            bottom: e.clientY,
            left: e.clientX,
            right: e.clientX,
            width: 0,
            height: 0,
            x: e.clientX,
            y: e.clientY,
            toJSON() {},
          } as DOMRect);

      menuHandleRef.current.positionAndOpen(anchorRect);
    }

    onRowDoubleClick?.(row);
  };

  const cellBackground = highlighted
    ? HIGHLIGHT_BG
    : hovered
    ? T.rowHover
    : T.white;

  return (
    <tr
      ref={rowRef}
      onMouseEnter={() => setHovered(true)}
      onMouseLeave={() => setHovered(false)}
      // onClick={handleClick}
      onDoubleClick={handleDoubleClick}
      style={{ cursor: "pointer" }}
    >
      {columns.map((col, colIdx) => {
        const isSticky = !!col.sticky;
        return (
          <td
            key={col.key}
            style={{
              width: col.width ?? 160,
              minWidth: col.width ?? 160,
              position: isSticky ? "sticky" : "relative",
              left: isSticky ? stickyOffsets[col.key] : undefined,
              zIndex: isSticky ? 2 : 0,
              background: cellBackground,
              padding: "0 12px",
              height: `${rowHeight}px`,
              borderBottom: `1px solid ${T.rowBorder}`,
              borderLeft:
                highlighted && colIdx === 0
                  ? `3px solid ${HIGHLIGHT_BORDER}`
                  : undefined,
              overflow: "hidden",
              transition: "background 0.1s",
              verticalAlign: "middle",
              textAlign: "left",
            }}
          >
            {col.render
              ? col.render((row as any)[col.key], row, rowIdx)
              : builtInRender(
                  col,
                  (row as any)[col.key],
                  row,
                  rowIdx,
                  registerMenuRef
                )}
          </td>
        );
      })}
    </tr>
  );
};

export default DataRow;
