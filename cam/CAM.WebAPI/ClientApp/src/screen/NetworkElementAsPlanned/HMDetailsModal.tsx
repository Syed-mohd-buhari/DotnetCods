import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import Select from "react-select";
import { Dropdown } from "react-bootstrap";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import {
  GetAssetHardwareAncillaries,
  SaveAssetHardwareAncillaries,
} from "../../Redux/Action/NetworkElementAsPlanned/AssetAncillariesAction";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import {
  AssetCapacityInfoGridDto,
  AssetHardwareAncillariesResponse,
  AssetHardwareAncillaryGridDto,
  KeyValueItem,
} from "../../Model/NetworkElementAsPlanned";
import setLoader from "../../Redux/Action/LoaderAction";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { DialogTitle, IconButton } from "@mui/material";
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import { IoClose } from "react-icons/io5";
import AssetsCapacityInfoForm from "./AssetsCapacityInfoForm";
import { MdDelete, MdEdit } from "react-icons/md";
import { useTheme } from "../../Context/ThemeContext";
import { unwrapResult } from "@reduxjs/toolkit";
import MajorHardwareMT from "../../Containers/Lookup/MajorHardwareMTContainer";

interface HMDetailsFormProps {
  assetId?: number;
  opCoId?: number;
  action: {
    closeModal: () => void;
    onSaveAndClose: () => void;
  };
}

const HMDetailsForm: React.FC<HMDetailsFormProps> = ({
  assetId,
  opCoId,
  action,
}) => {
  const [isAddCapacityDialogVisible, setAddCapacityDialogVisible] =
    useState(false);
  const [selectedCapacity, setSelectedCapacity] = useState<{
    capacity: AssetCapacityInfoGridDto | null;
    index: number | null;
  }>({
    capacity: null,
    index: null,
  });

  // ── NEW: controls the IntraVMType lookup modal for Major Hardware ──
  const [isMajorHardwareCrudModalOpen, setIsMajorHardwareCrudModalOpen] =
    useState(false);

  const { darkMode } = useTheme();
  const ancillariesData = useSelector(
    (state: RootState) =>
      state.assetHardwareAncillariesReducer.assetHardwareAncillariesResult
  );

  const handleEdit = (capacity: AssetCapacityInfoGridDto, index: number) => {
    if (capacity) {
      setSelectedCapacity({
        capacity,
        index,
      });
      setAddCapacityDialogVisible(true);
    }
  };

  const handleAddCapacityInfo = (newCapacity: any) => {
    const selectedHardware = formData?.majorHardwareResource?.find(
      (hw) =>
        hw.key === formData?.assetHardwareAncillaryGridDto?.majorHardwareId
    );
    if (selectedHardware) {
      newCapacity.physicalServerHwModel =
        selectedHardware.physicalServerHwModel;
      newCapacity.physicalServerVendor = selectedHardware.physicalServerVendor;
    }
    setFormData((prev) => ({
      ...prev,
      assetHardwareAncillaryGridDto: {
        ...prev?.assetHardwareAncillaryGridDto,
        assetCapacityInfoGridDtos: [
          ...(prev?.assetHardwareAncillaryGridDto?.assetCapacityInfoGridDtos ||
            []),
          newCapacity,
        ],
      },
    }));
    setAddCapacityDialogVisible(false);
  };

  const handleUpdateCapacityInfo = (
    updatedCapacity: AssetHardwareAncillaryGridDto
  ) => {
    if (!formData) return;

    const updatedGridData =
      formData?.assetHardwareAncillaryGridDto?.assetCapacityInfoGridDtos?.map(
        (item, index) => {
          const idToCheck = item.assetCapacityInfoId || index;
          if (
            idToCheck ===
            (selectedCapacity?.capacity?.assetCapacityInfoId ||
              selectedCapacity?.index)
          ) {
            return { ...item, ...updatedCapacity };
          }
          return item;
        }
      );
    setFormData({
      ...formData,
      assetHardwareAncillaryGridDto: {
        ...formData.assetHardwareAncillaryGridDto,
        assetCapacityInfoGridDtos: updatedGridData,
      },
    });

    setAddCapacityDialogVisible(false);
  };

  const { formData, setFormData, setInputValue } =
    useFormTableCrud<AssetHardwareAncillariesResponse>(
      SaveAssetHardwareAncillaries,
      SaveAssetHardwareAncillaries
    );

  useEffect(() => {
    if (assetId && opCoId) {
      const reqObj = { assetId, opCoId };
      GetAssetHardwareAncillaries(reqObj);
    }
  }, [assetId, opCoId]);

  useEffect(() => {
    if (ancillariesData) {
      const updatedAncillariesData = {
        ...ancillariesData,
        dataCenterResource: transformDataCenterResource(
          ancillariesData?.dataCenterResource as any
        ),
        assetHardwareAncillaryGridDto: {
          ...ancillariesData.assetHardwareAncillaryGridDto,
          networkElementAsPlannedId: assetId,
        },
      };
      setFormData((prev) => ({
        ...prev,
        ...updatedAncillariesData,
        assetId: assetId,
      }));
    }
  }, [ancillariesData, setFormData]);

  const handleDelete = (rowId: number) => {
    const updatedGridData =
      formData?.assetHardwareAncillaryGridDto?.assetCapacityInfoGridDtos?.filter(
        (item, index) => index !== rowId
      );

    setFormData({
      ...formData,
      assetHardwareAncillaryGridDto: {
        ...formData?.assetHardwareAncillaryGridDto,
        assetCapacityInfoGridDtos: updatedGridData || [],
      },
    });
  };
  const isFormValid = () => {
    return dropdowns.some((dd) => {
      const selectedValue = formData?.assetHardwareAncillaryGridDto?.[dd.key];
      return selectedValue !== null && selectedValue !== undefined;
    });
  };

  const removeEmptyObjectsFromPayload = (arr: any[]) => {
    return arr.filter((obj) => {
      return Object.values(obj).some(
        (val) => val !== null && val !== undefined && val !== ""
      );
    });
  };

  const handleSave = async () => {
    if (!formData) return;

    const requestBody: AssetHardwareAncillariesResponse = { ...formData };
    delete requestBody.assetHardwareAncillaryGridDto?.physicalServerHwModel;
    delete requestBody.assetHardwareAncillaryGridDto?.physicalServerVendor;
    console.log("requestBody", requestBody);
    if (requestBody.assetHardwareAncillaryGridDto?.assetCapacityInfoGridDtos) {
      requestBody.assetHardwareAncillaryGridDto.assetCapacityInfoGridDtos =
        removeEmptyObjectsFromPayload(
          requestBody.assetHardwareAncillaryGridDto.assetCapacityInfoGridDtos
        );
    }
    try {
      setLoader("ADD", "SaveAssetHardwareAncillaries");
      const result = await SaveAssetHardwareAncillaries(requestBody);

      if (result) {
        rootStore.dispatch(
          setNotification({
            message: result?.info,
            notifyType:
              result?.warning === true ? NotifyType.success : NotifyType.error,
          })
        );
      }
    } catch (error) {
      console.error("Save failed:", error);
      rootStore.dispatch(
        setNotification({
          message: "Failed to save asset data",
          notifyType: NotifyType.error,
        })
      );
    } finally {
      setLoader("REMOVE", "SaveAssetHardwareAncillaries");
      action.onSaveAndClose();
    }
  };

  const transformDataCenterResource = (
    dataCenterResource: any[]
  ): KeyValueItem[] => {
    if (!dataCenterResource || dataCenterResource.length === 0) return [];

    if (
      dataCenterResource[0].key !== undefined &&
      dataCenterResource[0].text !== undefined
    ) {
      return dataCenterResource as KeyValueItem[];
    }

    return dataCenterResource.map((dc: any) => ({
      key: dc.datacenterid,
      text: dc.description,
    }));
  };

  const updateMajorHardwareResourceValue = (value: any) => {
    console.log("Received Major Hardware data:", value);

    const newItems =
      value?.map((res) => ({
        key: res?.id || res?.majorHardwareBuildAsisId,
        text: res?.hardwareSolution,
        physicalServerHwModel: res?.buildConstructionDesc,
        physicalServerVendor: res?.orgEqpManuFacturerDesc,
      })) || [];

    setFormData((prev) => {
      const existingItems = prev?.majorHardwareResource || [];

      const existingKeys = new Set(existingItems.map((item) => item.key));
      const uniqueNewItems = newItems.filter(
        (item) => !existingKeys.has(item.key)
      );

      return {
        ...prev,
        majorHardwareResource: [...existingItems, ...uniqueNewItems],
      };
    });
  };

  if (!formData) return <p>Loading...</p>;

  const dropdowns = [
    {
      label: "Assets Cluster",
      key: "assetClusterId",
      resource: formData.assetClusterResource,
    },
    {
      label: "Cluster Name",
      key: "clusterNameId",
      resource: formData.clusterNameResource,
    },
    {
      label: "Asset Cluster Type",
      key: "assetClusterTypeId",
      resource: formData.assetClusterTypeResource,
    },

    {
      label: "Data Center",
      key: "dataCenterId",
      resource: formData.dataCenterResource,
    },
  ] as const;

  const handleSelectChange = (
    key: string,
    selectedOption: KeyValueItem | null
  ) => {
    const value = selectedOption ? selectedOption.key : null;

    setFormData((prev) => {
      const newFormData = {
        ...prev,
        assetHardwareAncillaryGridDto: {
          ...prev?.assetHardwareAncillaryGridDto,
          [key]: value,
        },
      };

      if (key === "majorHardwareId" && selectedOption) {
        const selectedHardware = prev?.majorHardwareResource?.find(
          (hw) => hw.key === selectedOption.key
        );

        const gridRows =
          newFormData.assetHardwareAncillaryGridDto.assetCapacityInfoGridDtos;

        if (selectedHardware && gridRows && gridRows.length > 0) {
          const updatedGrid = gridRows.map((row) => ({
            ...row,
            physicalServerHwModel: selectedHardware.physicalServerHwModel,
            physicalServerVendor: selectedHardware.physicalServerVendor,
          }));

          newFormData.assetHardwareAncillaryGridDto.assetCapacityInfoGridDtos =
            updatedGrid;
        }
      }

      return newFormData;
    });
  };

  const renderCapacityInfoTable = () => {
    const gridData =
      formData?.assetHardwareAncillaryGridDto?.assetCapacityInfoGridDtos;

    const filteredGridData = gridData?.filter((row) =>
      Object.values(row).some(
        (value) => value !== null && value != 0 && value !== "null"
      )
    );
    const shouldShowTableBody =
      Array.isArray(filteredGridData) && filteredGridData.length > 0;

    return (
      <table className="w-100 table-responsive mt-4">
        <thead>
          <tr className="intestazione">
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>Host Name</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>IP Address</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>Serial Number</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>No of Instances</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>vCPU</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>Memory</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>Storage</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>HW Model</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter">
                <label>Vendor</label>
              </div>
            </th>
            <th>
              <div className="h-100 d-flex align-items-center divFilter"></div>
            </th>
          </tr>
        </thead>

        <tbody>
          {shouldShowTableBody ? (
            filteredGridData.map((item, rowId) => (
              <tr className="dati" key={rowId}>
                <td>{item.physicalServerHostName}</td>
                <td>{item.physicalServerIpAddress}</td>
                <td>{item.physicalServerSerialNumber}</td>
                <td>{item.noOfInstances}</td>
                <td>{item.vcpu}</td>
                <td>{item.memory}</td>
                <td>{item.storage}</td>
                <td>{item.physicalServerHwModel ?? "-"}</td>
                <td>{item.physicalServerVendor ?? "-"}</td>
                <td className="actions">
                  <div className="d-flex flex-row">
                    <button
                      type="button"
                      title="Edit"
                      className="btn btn-link"
                      onClick={() => handleEdit(item, rowId)}
                    >
                      <MdEdit color={`${darkMode ? "white" : "black"}`} />
                    </button>
                    <button
                      type="button"
                      title="Delete"
                      className="btn btn-link"
                      onClick={() => handleDelete(rowId)}
                    >
                      <MdDelete color={`${darkMode ? "white" : "black"}`} />
                    </button>
                  </div>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td className="text-center py-3">No capacity info available.</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  };

  return (
    <div className="d-flex justify-content-center row">
      <form className="col-12 px-0">
        <div className="row mx-0">
          {/* ── Generic dropdowns (no + button) ── */}
          {dropdowns.map((dd) => (
            <div className="col-6 mb-3" key={dd.key}>
              <label className="labelForm voda-bold w-100">
                {dd.label}
                <Select
                  menuPosition="fixed"
                  options={dd.resource?.sort((a, b) =>
                    (a?.text ?? "").localeCompare(b?.text ?? "")
                  )}
                  value={
                    dd.resource?.find(
                      (x) =>
                        x.key ===
                        formData.assetHardwareAncillaryGridDto?.[dd.key]
                    ) || null
                  }
                  onChange={(selectedOption) =>
                    handleSelectChange(dd.key, selectedOption)
                  }
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option?.text ?? ""}
                  getOptionValue={(option: KeyValueItem) =>
                    option?.key?.toString() ?? ""
                  }
                />
              </label>
            </div>
          ))}

          <div className="col-6 mb-3">
            <label className="labelForm voda-bold w-100">Major Hardware</label>
            <div className="d-flex align-items-end">
              <div className="flex-grow-1 pr-2">
                <Select
                  menuPosition="fixed"
                  options={formData.majorHardwareResource?.sort((a, b) =>
                    (a?.text ?? "").localeCompare(b?.text ?? "")
                  )}
                  value={
                    formData.majorHardwareResource?.find(
                      (x) =>
                        x.key ===
                        formData.assetHardwareAncillaryGridDto?.majorHardwareId
                    ) || null
                  }
                  onChange={(selectedOption) =>
                    handleSelectChange("majorHardwareId", selectedOption)
                  }
                  onBlur={() => setInputValue("")}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option?.text ?? ""}
                  getOptionValue={(option: KeyValueItem) =>
                    option?.key?.toString() ?? ""
                  }
                />
              </div>

              <button
                className="btn btn-link"
                onClick={() => setIsMajorHardwareCrudModalOpen(true)}
                type="button"
              >
                <img
                  style={{ height: 15 }}
                  src={require("../../img/plus_icon.png")}
                  alt="+"
                />
              </button>
            </div>
          </div>
        </div>

        <hr className="my-4" />
        <div className="row mx-0">
          <div className="col-12 mb-3">
            <div className="row mt-4">
              <div className="col-12 d-flex justify-content-between align-items-center">
                <h5 className="voda-bold mb-3">Asset Capacity Info</h5>
                <button
                  className="  voda-bold btn btn-danger px-4 btnHeader"
                  onClick={() => {
                    setSelectedCapacity({ capacity: null, index: null });
                    setAddCapacityDialogVisible(true);
                  }}
                  type="button"
                >
                  Add Capacity
                </button>
              </div>
            </div>

            <div className="row mx-0 col-12 p-0 d-flex justify-content-center">
              {renderCapacityInfoTable()}
            </div>
          </div>
        </div>
        <div
          className="col-12 mx-0"
          style={{
            padding: "1rem",
            paddingLeft: "1rem !important",
            display: "flex",
            justifyContent: "right",
          }}
        >
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() => action.closeModal()}
            type="button"
          >
            Cancel
          </button>
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            onClick={handleSave}
            type="button"
            disabled={!isFormValid()}
          >
            Submit
          </button>
        </div>

        <Dialog
          open={isAddCapacityDialogVisible}
          onClose={() => setAddCapacityDialogVisible(false)}
          maxWidth="md"
          scroll="body"
          fullWidth
          slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
        >
          <DialogTitle className="d-flex justify-content-center">
            <div className="col-12">
              <h4 className="mb-0">Add Capacity Info</h4>
            </div>
          </DialogTitle>
          <IconButton
            aria-label="close"
            onClick={() => setAddCapacityDialogVisible(false)}
            sx={{ position: "absolute", right: 8, top: 8 }}
          >
            <IoClose size={25} />
          </IconButton>
          <DialogContent>
            <AssetsCapacityInfoForm
              isVisible={isAddCapacityDialogVisible}
              closeModal={() => setAddCapacityDialogVisible(false)}
              onAdd={
                selectedCapacity.capacity
                  ? handleUpdateCapacityInfo
                  : handleAddCapacityInfo
              }
              existingData={selectedCapacity?.capacity}
            />
          </DialogContent>
        </Dialog>

        {isMajorHardwareCrudModalOpen && (
          <Dialog
            open={true}
            onClose={() => {
              setIsMajorHardwareCrudModalOpen(false);
              if (assetId && opCoId) {
                GetAssetHardwareAncillaries({ assetId, opCoId });
              }
            }}
            maxWidth="md"
            scroll="body"
            fullWidth
            slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
          >
            <DialogContent>
              <MajorHardwareMT
                modal={{
                  isModal: true,
                  setIsVisibleModalLookup: () => {
                    setIsMajorHardwareCrudModalOpen(false);
                    if (assetId && opCoId) {
                      GetAssetHardwareAncillaries({ assetId, opCoId });
                    }
                  },
                }}
                returnObject={updateMajorHardwareResourceValue}
              />
            </DialogContent>
          </Dialog>
        )}
      </form>
    </div>
  );
};

export default HMDetailsForm;
