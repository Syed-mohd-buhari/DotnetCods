import React, { useEffect, useState } from "react";

import {
  MajorSoftwareBuildDtoGrid,
  MajorSoftwareBuildToCloneDto,
  MajorSoftwareBuildQueryObjectGrid,
  MajorSoftwareBuildProductBasedQueryObjectGrid,
} from "../../../Model/MajorSoftwareBuild";
import { DeleteDeepMajorSoftwareBuild } from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildDeleteAction";
import { GetMajorSoftwareToClone } from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCommonAction";
import DeleteModal from "./SoftwareDeleteModal";
import ProductDetail from "./ProductDetail";
import ProductSoftwareModal from "./ProductSoftwareModal";
import { Product } from "./ProductSoftwareTypes";
import { mapRowToProduct } from "./ProductSoftwareUtils";
import SoftwareTable from "./SoftwareTable";
import UpgradeSoftwareVersionModal from "./UpgradeSoftwareVersionModal";
import { ViewMode } from "../../utils/common";
import { GetMajorSoftwareBuildGridOnly } from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildGridAction";

export const defaultPaginationQuery: MajorSoftwareBuildQueryObjectGrid = {
  majorSoftwareBuildId: [],
  originalEquipmentManufacturer: [],
  softwareVersion: [],
  productNamesId: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  lastModified: undefined,
  endOfMaintenance: undefined,
  endOfsupport: undefined,
  generaAvailableDate: undefined,
  deliveryMethod: [],
  vulnerabilityStatus: [],
  operatingSystem: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

export const productBasedQuery: MajorSoftwareBuildProductBasedQueryObjectGrid =
  {
    currentVersionSwId: 0,
    productId: 0,
    lastModified: undefined,
    sortBy: "",
    isSortAscending: false,
    page: 1,
    pageSize: 10,
    principalId: undefined,
    orphan: false,
    deleted: false,
    lastModifiedBy: [],
  };

const ProductSoftwareContainer = (props: any) => {
  const [viewMode, setViewMode] = useState<ViewMode>("list");
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState<"new" | "edit">("new");
  const [editId, setEditId] = useState<number | undefined>(undefined);

  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteRow, setDeleteRow] = useState<MajorSoftwareBuildDtoGrid | null>(
    null
  );

  const [upgradeModalOpen, setUpgradeModalOpen] = useState(false);
  const [dataToClone, setDataToClone] = useState<
    MajorSoftwareBuildToCloneDto | undefined
  >(undefined);
  const [majorId, setMajorId] = useState<any>(null);
  const [refreshKey, setRefreshKey] = useState(0);

  useEffect(() => {
    if (props?.majorId !== null) {
      callApi(props?.majorId);
    }
  }, [props]);

  const callApi = async (id) => {
    const result: any = await GetMajorSoftwareBuildGridOnly({
      ...defaultPaginationQuery,
      majorSoftwareBuildId: [id],
    });
    if (result) {
      setSelectedProduct(mapRowToProduct(result?.items[0]));
      setMajorId(props?.majorId);
      setViewMode("detail");
    }
  };

  const triggerRefresh = () => setRefreshKey((prev) => prev + 1);
  const refreshSelectedProduct = async (productId?: any) => {
    const idToUse = productId ?? majorId;
    if (idToUse === null || idToUse === undefined) return;
    const result: any = await GetMajorSoftwareBuildGridOnly({
      ...defaultPaginationQuery,
      majorSoftwareBuildId: [idToUse],
    });
    if (result?.items?.[0]) {
      setSelectedProduct(mapRowToProduct(result.items[0]));
    }
    triggerRefresh();
  };

  const openEditModal = (id: number) => {
    setModalMode("edit");
    setEditId(id);
    setModalOpen(true);
  };

  const closeEditModal = () => {
    setModalOpen(false);
    setEditId(undefined);
  };

  const handleRowClick = (row: MajorSoftwareBuildDtoGrid) => {
    setSelectedProduct(mapRowToProduct(row));
    setViewMode("detail");
  };

  const handleEdit = (row: MajorSoftwareBuildDtoGrid) => {
    openEditModal((row as any).majorSoftwareBuildId);
  };

  const handleDelete = (row: MajorSoftwareBuildDtoGrid) => {
    setDeleteRow(row);
    setDeleteModalOpen(true);
  };

  const handleDeleteConfirmed = async (row: MajorSoftwareBuildDtoGrid) => {
    const result: any = await DeleteDeepMajorSoftwareBuild(
      (row as any).majorSoftwareBuildId
    );
    if (result) {
      setViewMode("list");
      setSelectedProduct(null);
      setRefreshKey(0);
    }
  };

  const handleCloseDeleteModal = () => {
    setDeleteModalOpen(false);
    setDeleteRow(null);
  };

  const handleUpgrade = async (row: MajorSoftwareBuildDtoGrid) => {
    const result: any = await GetMajorSoftwareToClone(
      (row as any).majorSoftwareBuildId
    );
    if (!result?.warning) {
      setDataToClone(result?.data);
      setUpgradeModalOpen(true);
    }
  };

  const handleUpgradeSuccess = () => {
    setViewMode("list");
    setRefreshKey(0);
    setMajorId(null);
  };

  const handleCloseUpgradeModal = () => {
    setUpgradeModalOpen(false);
    setDataToClone(undefined);
  };

  const handleBackToList = () => {
    setViewMode("list");
    setSelectedProduct(null);
  };

  return (
    <>
      {viewMode === "list" ? (
        <SoftwareTable
          title="Software List"
          refreshKey={refreshKey}
          onRowClick={handleRowClick}
          onEdit={handleEdit}
          onDelete={handleDelete}
          onUpgrade={handleUpgrade}
        />
      ) : (
        <ProductDetail
          product={selectedProduct!}
          onBack={handleBackToList}
          onUpgradeSoftwareVersion={(product) =>
            handleUpgrade({ ...product, majorSoftwareBuildId: product.id })
          }
          onEditSoftware={() => openEditModal(selectedProduct!.id)}
          onDeleteSoftware={(product) =>
            handleDelete({ ...product, majorSoftwareBuildId: product.id })
          }
          onProductUpdated={refreshSelectedProduct}
        />
      )}

      {modalOpen && (
        <ProductSoftwareModal
          open={modalOpen}
          onClose={closeEditModal}
          mode={modalMode}
          editId={editId}
          action={{
            closeModal: closeEditModal,
            refresh: () => {
              triggerRefresh();
              refreshSelectedProduct(editId);
            },
          }}
        />
      )}

      <DeleteModal
        open={deleteModalOpen}
        row={deleteRow}
        onClose={handleCloseDeleteModal}
        onConfirmed={handleDeleteConfirmed}
      />

      <UpgradeSoftwareVersionModal
        open={upgradeModalOpen}
        onClose={handleCloseUpgradeModal}
        cloneData={dataToClone}
        onSuccess={handleUpgradeSuccess}
      />
    </>
  );
};

export default ProductSoftwareContainer;
