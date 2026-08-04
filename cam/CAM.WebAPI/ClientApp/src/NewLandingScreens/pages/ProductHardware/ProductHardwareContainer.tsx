import React, { useEffect, useState } from "react";

import {
  MajorHardwareBuildDtoGrid,
  MajorHardwareBuildQueryObjectGrid,
} from "../../../Model/MajorHardwareBuild";
import { DeleteDeepMajorHardwareBuild } from "../../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildDeleteAction";
import HardwareTable from "./HardwareTable";
import { ViewMode } from "../../utils/common";
import DeleteModal from "../ProductHardware/HardwareDeleteModal";
import ProductHardwareModal from "../ProductHardware/ProductHardwareModal";
import { HardwareProduct } from "./ProductHardwareTypes";
import { mapRowToHardwareProduct } from "./ProductHardwareUtils";
import { GetMajorHardwareBuildGridOnly } from "../../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildGridAction";

export const defaultPaginationQuery: MajorHardwareBuildQueryObjectGrid = {
  majorHardwareBuildId: [],
  name: "",
  originalEquipmentManufacturer: [],
  hardwareSolution: [],
  platform: [],
  hardwareType: [],
  otherHardwareInfo: [],
  lastTimeBuyNew: undefined,
  lastTimeBuyUpgrades: undefined,
  lastTimeBuyExpansions: undefined,
  lastModified: undefined,
  endOfMaintenance: undefined,
  endOfsupport: undefined,
  vulnerabilityStatus: [],
  spareFieldsJson: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  buildConstruction: [],
  principalId: undefined,
  principalIdList: [],
  proprietaryHardware: [],
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
};

const ProductHardwareContainer = (props?: any) => {
  const [selectedProduct, setSelectedProduct] =
    useState<HardwareProduct | null>(null);

  const [modalOpen, setModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState<"new" | "edit">("new");
  const [editId, setEditId] = useState<number | undefined>(undefined);

  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteRow, setDeleteRow] = useState<MajorHardwareBuildDtoGrid | null>(
    null
  );
  const [majorId, setMajorId] = useState<any>(null);
  const [refreshKey, setRefreshKey] = useState(0);

  useEffect(() => {
    if (props?.majorId !== null) {
      callApi(props?.majorId);
    }
  }, [props]);

  const callApi = async (id) => {
    const result: any = await GetMajorHardwareBuildGridOnly({
      ...defaultPaginationQuery,
      majorHardwareBuildId: [id],
    });
    if (result) {
      setSelectedProduct(mapRowToHardwareProduct(result?.items[0]));
      setMajorId(props?.majorId);
      setModalMode("edit");
    }
  };

  const triggerRefresh = () => setRefreshKey((prev) => prev + 1);

  const openEditModal = (id: number) => {
    setModalMode("edit");
    setEditId(id);
    setModalOpen(true);
  };

  const closeEditModal = () => {
    setModalOpen(false);
    setEditId(undefined);
  };

  const handleRowClick = (row: MajorHardwareBuildDtoGrid) => {
    setSelectedProduct(mapRowToHardwareProduct(row));
  };

  const handleEdit = (row: MajorHardwareBuildDtoGrid) => {
    openEditModal((row as any).majorHardwareBuildId);
  };

  const handleDelete = (row: MajorHardwareBuildDtoGrid) => {
    setDeleteRow(row);
    setDeleteModalOpen(true);
  };

  const handleDeleteConfirmed = (row: MajorHardwareBuildDtoGrid) => {
    DeleteDeepMajorHardwareBuild((row as any).majorHardwareBuildId);
    triggerRefresh();
  };

  const handleCloseDeleteModal = () => {
    setDeleteModalOpen(false);
    setDeleteRow(null);
  };

  return (
    <>
      <HardwareTable
        title="Hardware"
        refreshKey={refreshKey}
        onRowClick={handleEdit}
        onEdit={handleEdit}
        onDelete={handleDelete}
      />
      {modalOpen && (
        <ProductHardwareModal
          open={modalOpen}
          onClose={closeEditModal}
          mode={modalMode}
          editId={editId}
          action={{
            closeModal: closeEditModal,
            refresh: triggerRefresh,
          }}
        />
      )}
      <DeleteModal
        open={deleteModalOpen}
        row={deleteRow}
        onClose={handleCloseDeleteModal}
        onConfirmed={handleDeleteConfirmed}
      />
    </>
  );
};

export default ProductHardwareContainer;
