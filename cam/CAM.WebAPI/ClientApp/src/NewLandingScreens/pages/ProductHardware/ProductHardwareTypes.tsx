export interface RelatedRecord {
  table: string;
  values: string[];
}

export interface RelatedRecordsResponse {
  warning: boolean;
  info: string;
  data: {
    entityName: string;
    recordName: string;
    dataRelatedList: RelatedRecord[];
  } | null;
}

export interface HardwareProduct {
  id: any;
  type: string;
  productRecord: string;
  isPlatform: boolean;
  versionModel: string;
  lifecycleStage: string;
  lifecycleColor: string;
  lifecycleTextColor: string;
  priority: string;
  status: string;
  statusBg: string;
  statusTextColor: string;
  ownerInitials: string;
  ownerName: string;
  vendor: string;
  productId: number;
  nextMilestone: string;
  lastUpdated: string;
  hasRedDot?: boolean;
  endOfsupportValue: string;
  endOfMaintenanceValue: string;
  designContact: string;
  hardwareVersion: string;
  isCompliant: boolean;
  eomId: any;
}
