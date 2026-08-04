import {
  ApiGridRenderItem,
  ColumnStyleRule,
  CustomColumnDef,
} from "./CustomTable.types";
import LabelsDictionary from "../../../Constant/LabelsAndDescriptions.json";

const DATE_FILTER_TYPES = new Set([4]);

export function buildColumnsFromApi(
  renderItems: ApiGridRenderItem[],
  styleRules: ColumnStyleRule[] = [],
  extraColumns: CustomColumnDef[] = []
): CustomColumnDef[] {
  const styleRuleMap = new Map(styleRules.map((r) => [r.propertyName, r]));

  const columns: CustomColumnDef[] = renderItems
    .filter((item) => item.show)
    .sort((a, b) => a.order - b.order)
    .map((item) => {
      const rule = styleRuleMap.get(item.propertyName);
      const filterVariant: "checkbox" | "date" =
        item.type === 3 ? "date" : "checkbox";
      const labelEntry = (LabelsDictionary as Record<string, any>)[
        item.propertyName
      ];
      const defaultLabel =
        labelEntry?.Full ?? labelEntry?.Short ?? item.propertyName;

      const column: CustomColumnDef = {
        key: item.propertyName,
        label: rule?.label ?? item.label ?? defaultLabel,
        width: rule?.width ?? item.width ?? 160,
        sticky: rule?.sticky,
        sortable: rule?.sortable ?? item.sortable ?? true,
        renderAs: rule?.renderAs,
        colorMap: rule?.colorMap,
        dotColorMap: rule?.dotColorMap,
        onLinkClick: rule?.onLinkClick,
        actions: rule?.actions,
        upgradeCondition: rule?.upgradeCondition,
        onUpgradeClick: rule?.onUpgradeClick,
        filterKey: item.propertyName,
        filterVariant,
      };

      return column;
    });

  return [...columns, ...extraColumns];
}
