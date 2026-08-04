import { MajorHardwareBuildDtoGrid } from "../../../Model/MajorHardwareBuild";
import { HardwareProduct } from "./ProductHardwareTypes";

const parseDate = (value: any): Date | null => {
  if (!value || value === "Not Announced") return null;
  const d = new Date(value);
  return isNaN(d.getTime()) ? null : d;
};

const isCompliant = (endOfMaintenance: any, endOfsupport: any): boolean => {
  const now = new Date();
  const eom = parseDate(endOfMaintenance);
  const eos = parseDate(endOfsupport);
  if (eom === null && eos === null) return true;
  return (eom !== null && eom >= now) || (eos !== null && eos >= now);
};

export const mapRowToHardwareProduct = (
  row: MajorHardwareBuildDtoGrid
): HardwareProduct => ({
  id: (row as any).majorHardwareBuildId,
  type: "Hardware",
  productRecord: (row as any).productName ?? "Unknown",
  isPlatform: (row as any).isPlatform ?? false,
  versionModel: (row as any).hardwareVersion ?? "",
  lifecycleStage: (row as any).lifecycleStage ?? "Validate",
  lifecycleColor: "#E5F1FB",
  lifecycleTextColor: "#083774",
  priority: (row as any).priority ?? "High",
  status: (row as any).status ?? "Needs review",
  statusBg: "#FEF3EB",
  statusTextColor: "#853525",
  ownerInitials: "YT",
  ownerName: (row as any).lastModifiedBy ?? "",
  nextMilestone: (row as any).nextMilestone ?? "",
  lastUpdated: (row as any).lastModified ?? "",
  endOfsupportValue: (row as any).endOfsupportValue,
  endOfMaintenanceValue: (row as any).endOfMaintenanceValue,
  vendor: (row as any).originalEquipmentManufacturer,
  designContact: (row as any).designContact,
  hardwareVersion: (row as any).hardwareVersion,
  eomId: (row as any).originalEquipmentManufacturerId,
  productId: (row as any).productNameId,
  isCompliant: isCompliant(
    (row as any).endOfMaintenance,
    (row as any).endOfsupport
  ),
});
