import React, { useEffect, useState } from "react";
import {
  MajorSoftwareBuildDtoGrid,
  MajorSoftwareBuildProductBasedQueryObjectGrid,
} from "../../../Model/MajorSoftwareBuild";
import { GetMajorSoftwareBuildProductBasedGrid } from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildGridAction";
import { useResourceTableCrud } from "../../../Hook/useResourceTableCrud";
import { useAuth } from "../../../Hook/useAuth";
import LabelsDictionary from "../../../Constant/LabelsAndDescriptions.json";
import { Product } from "./ProductSoftwareTypes";
import CustomTable from "../../components/table/CustomTable";
import {
  ApiGridRenderItem,
  ColumnStyleRule,
  CustomColumnDef,
} from "../../components/table/CustomTable.types";
import { buildColumnsFromApi } from "../../components/table/CustomTable.utils";
import { productBasedQuery } from "./ProductSoftwareContainer";

interface SoftwareVersionTableProps {
  title?: string;
  product: Product;
  linkedData: (hardware: any, services: any) => void;
  isComplaint: (value: boolean) => void;
}

const SoftwareVersionTable: React.FC<SoftwareVersionTableProps> = ({
  title,
  product,
  linkedData,
  isComplaint,
}) => {
  const [tableData, setTableData] = useState<MajorSoftwareBuildDtoGrid[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [columns, setColumns] = useState<CustomColumnDef[]>([]);
  const [loading, setLoading] = useState(false);
  const { isPermesso, pageSize } = useAuth();

  const {
    query,
    setQuery,
    loading: resourceLoading,
  } = useResourceTableCrud(
    {
      ...productBasedQuery,
      currentVersionSwId: product?.id,
      productId: product?.productId,
    } as MajorSoftwareBuildProductBasedQueryObjectGrid,
    undefined,
    false
  );

  useEffect(() => {
    if (!product) return;
    setQuery({
      ...productBasedQuery,
      currentVersionSwId: product?.id,
      productId: product?.productId,
      page: 1,
    } as MajorSoftwareBuildProductBasedQueryObjectGrid);
  }, [product?.id, product?.productId]);

  useEffect(() => {
    if (!isPermesso || !query?.["currentVersionSwId"]) return;
    fetchVersions(query);
  }, [isPermesso, query]);

  const fetchVersions = async (
    q: MajorSoftwareBuildProductBasedQueryObjectGrid
  ) => {
    setLoading(true);
    const response: any = await GetMajorSoftwareBuildProductBasedGrid(q);
    setLoading(false);
    if (!response) return;

    linkedData(response?.linkedHardware ?? [], response?.linkedServices ?? []);
    isComplaint(response?.currentSwComplaintValue);
    setTableData(response?.softwareGridRecords?.items ?? []);
    setTotalItems(response?.softwareGridRecords?.totalItems ?? 0);

    const allowed = [
      "softwareVersion",
      "lifeCycleStatus",
      "networkStatus",
      "endOfMaintenance",
      "endOfsupport",
      "designContactEmail",
    ];
    const renderList =
      response?.softwareGridRecords?.gridRender?.render?.filter(
        (c: any) => c.propertyName && allowed.includes(c.propertyName)
      );

    if (!renderList) return;

    const apiCols: ApiGridRenderItem[] = renderList.map((c: any) => ({
      propertyName: c.propertyName,
      label:
        (LabelsDictionary as Record<string, any>)[c.propertyName]?.Full ??
        c.propertyName,
      show: true,
      order: c.order,
      width: undefined,
      sortable: true,
    }));

    const styleRules: ColumnStyleRule[] = [
      {
        propertyName: "softwareVersion",
        width: 170,
        label: "Version",
      },
      {
        propertyName: "lifeCycleStatus",
        label: "Lifecycle Status",
        renderAs: "dot-text-border-badge",
      },
      {
        propertyName: "networkStatus",
        label: "Network Status",
        renderAs: "dot-text-border-badge",
      },
      {
        propertyName: "endOfsupport",
        width: 160,
        label: "EOS",
      },
      {
        propertyName: "endOfMaintenance",
        width: 180,
        label: "EOM",
      },
      {
        propertyName: "designContactEmail",
        width: 180,
        label: "Software Product Owner",
        renderAs: "avatar-email-stack",
      },
    ];

    setColumns(buildColumnsFromApi(apiCols, styleRules));
  };

  const goToPage = (page: number) => {
    setQuery({
      ...query,
      page,
    } as MajorSoftwareBuildProductBasedQueryObjectGrid);
  };

  return (
    <CustomTable
      title={title}
      columns={columns}
      data={tableData}
      totalItems={totalItems}
      currentPage={query?.page ?? 1}
      pageSize={pageSize}
      loading={loading || resourceLoading}
      onPageChange={goToPage}
      fixColumn={0}
      showSearch={false}
      showSortCustomise={false}
    />
  );
};

export default SoftwareVersionTable;
