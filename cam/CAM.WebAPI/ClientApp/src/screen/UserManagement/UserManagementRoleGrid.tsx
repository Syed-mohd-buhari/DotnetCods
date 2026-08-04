import React, { useEffect, useState, useCallback } from "react";
import {
  FaPlus,
  FaSave,
  FaTrash,
  FaTimes,
  FaPaperPlane,
  FaList,
} from "react-icons/fa";
import Select from "react-select";
import ModalConfirm from "../../Components/ModalConfirm";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { stateConfirm } from "../../Model/Common";
import {
  IoCheckmarkDoneCircle,
  IoCloseCircleOutline,
  IoTrashOutline,
} from "react-icons/io5";
import { Box, Paper, Tooltip } from "@mui/material";

const UserManagementRoleGrid = (props: any) => {
  const [rows, setRows] = useState<any[]>([]);
  const [confirm, setConfirm] = useState<any>(stateConfirm);
  const [errors, setErrors] = useState<{
    [key: number]: { [key: string]: string };
  }>({});

  useEffect(() => {
    if (
      !props.data ||
      !props.allROVList?.opCoResource ||
      !props.allROVList?.verticalResource
    ) {
      return;
    }

    const formatted = props.data.map((item: any) => ({
      ...item,
      role: item.role ? { label: item.role, value: item.role } : null,
      opCo: item.opCo
        ? item.opCo
            .split(",")
            .map((op) => {
              const found = dictionaryToArray(
                props.allROVList?.opCoResource
              )?.find((x) => x.value === op.trim());

              if (!found) return null;

              return {
                key: found.key,
                label: found.value,
                value: found.value,
              };
            })
            .filter(Boolean)
        : [],
      verticalRes: item.verticalResponsible
        ? item.verticalResponsible
            .split(",")
            .map((v) => {
              const found = dictionaryToArray(
                props.allROVList?.verticalResource
              )?.find((x) => x.value === v.trim());

              if (!found) return null;

              return {
                key: found.key,
                label: found.value,
                value: found.value,
              };
            })
            .filter(Boolean)
        : [],
      isNew: false,
    }));

    setRows(formatted);
  }, [props.data, props.allROVList]);

  const getOptions = useCallback((resource: any) => {
    if (!resource) return [];
    return dictionaryToArray(resource).map((item: any) => ({
      key: item.key,
      label: item.value,
      value: item.value,
    }));
  }, []);

  const multiSelectStyles = {
    menuPortal: (base: any) => ({
      ...base,
      zIndex: 9999,
    }),
    menuPosition: "absolute",
    control: (base: any) => ({
      ...base,
      minHeight: "38px",
      borderColor: "#ced4da",
      "&:hover": {
        borderColor: "#dc3545",
      },
    }),
    multiValue: (base: any) => ({
      ...base,
      backgroundColor: "#dc3545",
      borderRadius: "4px",
      padding: "2px 4px",
    }),
    multiValueLabel: (base: any) => ({
      ...base,
      color: "white",
      fontSize: "0.85rem",
      fontWeight: "500",
    }),
    multiValueRemove: (base: any) => ({
      ...base,
      color: "white",
      "&:hover": {
        backgroundColor: "#c82333",
        color: "white",
      },
    }),
    placeholder: (base: any) => ({
      ...base,
      color: "#6c757d",
      fontSize: "0.9rem",
    }),
  };

  const handleAddNewRow = () => {
    const newIndex = rows.length;
    setRows((prev) => [
      ...prev,
      {
        role: null,
        opCo: [],
        verticalRes: [],
        isNew: true,
      },
    ]);
    setErrors((prev) => ({
      ...prev,
      [newIndex]: {
        role: "Please select a role",
        opCo: "Please select OpCo",
        verticalRes: "Please select Vertical",
      },
    }));
  };

  const handleInputChange = (value: any, index: number, field: string) => {
    let newValue = value;

    if (field === "opCo") {
      const allOptions = getOptions(props.allROVList?.opCoResource);
      const isSelectAllSelected = value?.some((v) => v.value === "__all__");
      if (isSelectAllSelected) newValue = allOptions;
    }

    if (field === "verticalRes") {
      const allOptions = getOptions(props.allROVList?.verticalResource);
      const isSelectAllSelected = value?.some((v) => v.value === "__all__");
      if (isSelectAllSelected) newValue = allOptions;
    }

    setRows((prev) => {
      const updatedRows = prev.map((row, i) =>
        i === index ? { ...row, [field]: newValue } : row
      );

      const row = updatedRows[index];
      if (row.role && row.opCo?.length > 0 && row.verticalRes?.length > 0) {
        handleSaveRow(row, index);
      }

      return updatedRows;
    });

    setErrors((prev) => {
      const updated = { ...prev };
      if (!newValue || (Array.isArray(newValue) && newValue.length === 0)) {
        updated[index] = {
          ...updated[index],
          [field]:
            field === "role"
              ? "Please select a role"
              : field === "opCo"
              ? "Please select OpCo"
              : "Please select Vertical",
        };
      } else if (updated[index]) {
        delete updated[index][field];
        if (Object.keys(updated[index]).length === 0) delete updated[index];
      }
      return updated;
    });
  };

  const validateRow = (row: any, index: number): boolean => {
    const newErrors: { [key: string]: string } = {};

    if (!row.role) {
      newErrors.role = "Please select a role";
    }

    if (!row.opCo || row.opCo.length === 0) {
      newErrors.opCo = "Please select OpCo";
    }

    if (!row.verticalRes || row.verticalRes.length === 0) {
      newErrors.verticalRes = "Please select Vertical";
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors((prev) => ({
        ...prev,
        [index]: newErrors,
      }));
      return false;
    }

    return true;
  };
  useEffect(() => {
    const hasErrors = Object.keys(errors).length > 0;
    if (props.onValidationError) {
      props.onValidationError(hasErrors);
    }
  }, [errors, props.onValidationError]);
  const handleSaveRow = async (row, index) => {
    const roleKey = dictionaryToArray(props.allROVList?.roleResource)?.find(
      (x) => x.value === row.role.value
    )?.key;

    const opCoIds = row.opCo
      ?.map((o) => {
        if (typeof o.value === "number") return o.value;
        if (o.key) return o.key;

        const found = dictionaryToArray(props.allROVList?.opCoResource)?.find(
          (x) => x.value === o.value || x.value === o.label
        );

        return found?.key;
      })
      .filter(Boolean)
      .join(",");

    const verticalResponsibleIds = row.verticalRes
      ?.map((v) => {
        if (typeof v.value === "number") return v.value;
        if (v.key) return v.key;

        const found = dictionaryToArray(
          props.allROVList?.verticalResource
        )?.find((x) => x.value === v.value || x.value === v.label);

        return found?.key;
      })
      .filter(Boolean)
      .join(",");

    const payload = {
      userId: props.userId,
      aspNetUserRoleId: row.aspNetUserRoleId,
      roleId: roleKey?.toString(),
      opCoIds: opCoIds,
      verticalResponsibleIds: verticalResponsibleIds,
    };

    if (props.action?.SaveRole) {
      console.log("gridPay", payload);
      await props.action.SaveRole(payload);
    }

    setRows((prev) =>
      prev.map((r, i) => (i === index ? { ...r, isNew: false } : r))
    );

    setErrors((prev) => {
      const newErrors = { ...prev };
      delete newErrors[index];
      return newErrors;
    });
  };

  const handleCancelRow = (index: number) => {
    setRows((prev) => prev.filter((_, i) => i !== index));
  };

  const handleDelete = (row: any, index: number) => {
    setConfirm({
      title: "Confirm Delete",
      message: "Are you sure you want to delete?",
      button: "Delete",
      cancelText: "Cancel",
      isOpen: true,
      actions: {
        confirm: async () => {
          if (!row.aspNetUserRoleId) {
            setRows((prev) => prev.filter((_, i) => i !== index));

            if (props.action?.RemoveRole) {
              props.action.RemoveRole(row);
            }
          } else {
            if (props.action?.Delete) {
              await props.action.Delete(row.aspNetUserRoleId);
            }

            setRows((prev) => prev.filter((_, i) => i !== index));

            if (props.action?.RemoveRole) {
              props.action.RemoveRole(row);
            }
          }

          setConfirm(stateConfirm);
        },
        cancel: () => setConfirm(stateConfirm),
      },
    });
  };

  const getAvailableRoles = (currentIndex: number) => {
    const selectedRoles = rows
      .filter((_, i) => i !== currentIndex)
      .map((r) => r.role?.value)
      .filter(Boolean);

    return dictionaryToArray(props.allROVList?.roleResource)
      .filter((r) => !selectedRoles.includes(r.value))
      .map((item) => ({ label: item.value, value: item.value }));
  };
  const getOptionsWithSelectAll = (resource: any) => {
    const options = getOptions(resource);

    return [
      { label: "Select All", value: "__all__" }, // special select all option
      ...options,
    ];
  };
  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <div
        className="table-container position-relative"
        style={{ width: "100%", overflowX: "auto" }}
      >
        <div
          className="d-flex justify-content-end mb-3"
          style={{ gap: "10px" }}
        >
          <button
            className="btn btn-danger px-4 d-flex align-items-center gap-2"
            onClick={() => props.onManageMenuClick(rows)}
            type="button"
          >
            <FaList />

            <span style={{ paddingLeft: "3px" }}>Manage Access</span>
          </button>
          <button
            className="btn btn-danger px-4 d-flex align-items-center gap-2"
            onClick={handleAddNewRow}
          >
            <FaPlus /> Add New Role
          </button>
        </div>

        <table className="table" style={{ width: "100%" }}>
          <thead>
            <tr className="intestazione">
              <th style={{ width: "20%" }}>
                <div className="h-100 d-flex align-items-center divFilter">
                  <label>Role</label>
                </div>
              </th>
              <th style={{ width: "30%" }}>
                <div className="h-100 d-flex align-items-center divFilter">
                  <label>OpCo</label>
                </div>
              </th>
              <th style={{ width: "30%" }}>
                <div className="h-100 d-flex align-items-center divFilter">
                  <label>Vertical</label>
                </div>
              </th>
              <th style={{ width: "20%" }}>
                <div className="h-100 d-flex align-items-center divFilter">
                  <label>Actions</label>
                </div>
              </th>
            </tr>
          </thead>

          <tbody>
            {rows.map((row, index) => (
              <tr key={index} className="dati">
                <td style={{ padding: "12px 10px" }}>
                  <Select
                    menuPortalTarget={document.body}
                    menuPosition="absolute"
                    styles={{
                      menuPortal: (base) => ({
                        ...base,
                        zIndex: 9999,
                      }),
                      control: (base) => ({
                        ...base,
                        minHeight: "42px",
                        borderColor: "#ced4da",
                      }),
                    }}
                    options={getAvailableRoles(index)}
                    value={row.role}
                    onChange={(e) => handleInputChange(e, index, "role")}
                    isSearchable
                    placeholder="Select Role"
                  />
                  {errors[index]?.role && (
                    <small
                      style={{
                        color: "#dc3545",
                        marginTop: "4px",
                        display: "block",
                      }}
                    >
                      {errors[index].role}
                    </small>
                  )}
                </td>

                <td style={{ padding: "12px 10px" }}>
                  <Select
                    isMulti
                    menuPortalTarget={document.body}
                    menuPosition="absolute"
                    styles={multiSelectStyles}
                    options={getOptionsWithSelectAll(
                      props.allROVList?.opCoResource
                    )}
                    value={row.opCo}
                    onChange={(e) => handleInputChange(e, index, "opCo")}
                    isSearchable
                    placeholder="Select OpCo(s)"
                    closeMenuOnSelect={false}
                    hideSelectedOptions={false}
                  />
                  {errors[index]?.opCo && (
                    <small
                      style={{
                        color: "#dc3545",
                        marginTop: "4px",
                        display: "block",
                      }}
                    >
                      {errors[index].opCo}
                    </small>
                  )}
                </td>

                <td style={{ padding: "12px 10px" }}>
                  <Select
                    isMulti
                    menuPortalTarget={document.body}
                    menuPosition="absolute"
                    styles={multiSelectStyles}
                    options={getOptionsWithSelectAll(
                      props.allROVList?.verticalResource
                    )}
                    value={row.verticalRes}
                    onChange={(e) => handleInputChange(e, index, "verticalRes")}
                    isSearchable
                    placeholder="Select Vertical(s)"
                    closeMenuOnSelect={false}
                    hideSelectedOptions={false}
                  />
                  {errors[index]?.verticalRes && (
                    <small
                      style={{
                        color: "#dc3545",
                        marginTop: "4px",
                        display: "block",
                      }}
                    >
                      {errors[index].verticalRes}
                    </small>
                  )}
                </td>

                <td style={{ padding: "12px 10px" }}>
                  <div
                    style={{
                      display: "flex",
                      gap: "12px",
                      alignItems: "center",
                    }}
                  >
                    {/* <Tooltip title="Save" placement="top" arrow>
                      <Box
                        onClick={() => handleSaveRow(row, index)}
                        sx={{
                          cursor: "pointer",
                          color: "#28a745",
                          fontWeight: 600,
                          transition: "all 0.2s ease",
                          "&:hover": {
                            transform: "scale(1.1)",
                          },
                        }}
                      >
                        <IoCheckmarkDoneCircle size={28} color="#28a745" />
                      </Box>
                    </Tooltip> */}

                    {row.isNew && (
                      <Tooltip title="Cancel" placement="top" arrow>
                        <Box
                          onClick={() => handleCancelRow(index)}
                          sx={{
                            cursor: "pointer",
                            "&:hover": {
                              transform: "scale(1.1)",
                            },
                          }}
                        >
                          <IoCloseCircleOutline size={24} color="#535455" />
                        </Box>
                      </Tooltip>
                    )}

                    <Tooltip title="Delete" placement="top" arrow>
                      <Box
                        onClick={() => handleDelete(row, index)}
                        sx={{
                          cursor: "pointer",
                          "&:hover": {
                            transform: "scale(1.1)",
                          },
                        }}
                      >
                        <IoTrashOutline size={24} color="#DC3545" />
                      </Box>
                    </Tooltip>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
};

export default UserManagementRoleGrid;
