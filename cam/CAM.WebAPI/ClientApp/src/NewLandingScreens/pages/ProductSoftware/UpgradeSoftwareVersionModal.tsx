import React, { useEffect, useRef, useState } from "react";
import { Box, Divider, Typography } from "@mui/material";
import { styled } from "@mui/material/styles";
import { FiX } from "react-icons/fi";
import { createPortal } from "react-dom";
import Select from "react-select";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import {
  CloneMajorSoftware,
  GetMajorSoftwareClonePreSubmit,
} from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildCommonAction";
import {
  CloneMajorSoftwareBuildDto,
  MajorSoftwareBuildToCloneDto,
} from "../../../Model/MajorSoftwareBuild";
import { lowerFirstLetter } from "../../../Hook/Common";
import { rtnConfirmMessage } from "../../../Model/Common";
import {
  CloseButton,
  DateField,
  FieldBox,
  HeaderRow,
  INPUT_STYLE,
  LABEL_STYLE,
  ModalCard,
  ModalFooter,
  ModalHeader,
  Overlay,
  REQ,
  ScrollBody,
  SectionBox,
  SELECT_STYLES,
  T,
  ToggleSwitch,
  ValidationLabel,
} from "../../utils/styled";

export interface UpgradeSoftwareVersionModalProps {
  open: boolean;
  onClose: () => void;
  cloneData: MajorSoftwareBuildToCloneDto | undefined;
  onSuccess?: () => void;
}

interface ConfirmDialogProps {
  open: boolean;
  title: string;
  message: string;
  confirmLabel?: string;
  onConfirm: () => void;
  onCancel: () => void;
}

const ConfirmDialog: React.FC<ConfirmDialogProps> = ({
  open,
  title,
  message,
  confirmLabel = "Confirm",
  onConfirm,
  onCancel,
}) => {
  if (!open) return null;
  return createPortal(
    <Overlay sx={{ zIndex: 1400 }}>
      <Box
        sx={{
          background: "#fff",
          borderRadius: "12px",
          padding: "24px",
          maxWidth: "480px",
          width: "90%",
          display: "flex",
          flexDirection: "column",
          gap: "16px",
          boxShadow: "0 8px 32px rgba(0,0,0,0.18)",
        }}
      >
        <Typography
          sx={{
            fontWeight: 700,
            fontSize: "18px",
            color: T.black,
          }}
        >
          {title}
        </Typography>
        <Divider />
        <Typography
          sx={{
            fontSize: "14px",
            color: "#374151",

            lineHeight: "22px",
          }}
          dangerouslySetInnerHTML={{ __html: message }}
        />
        <Box
          sx={{
            display: "flex",
            justifyContent: "flex-end",
            gap: "12px",
            mt: "8px",
          }}
        >
          <Box
            onClick={onCancel}
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              px: "24px",
              py: "8px",
              border: "1px solid #000",
              borderRadius: "6px",
              cursor: "pointer",
              height: "40px",
              "&:hover": { background: "#F5F5F5" },
            }}
          >
            <Typography sx={{ fontSize: "15px", color: "#000" }}>
              Cancel
            </Typography>
          </Box>
          <Box
            onClick={onConfirm}
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              px: "24px",
              py: "8px",
              background: T.red,
              borderRadius: "6px",
              cursor: "pointer",
              height: "40px",
              "&:hover": { background: "#C40000" },
            }}
          >
            <Typography sx={{ fontSize: "15px", color: "#FFF" }}>
              {confirmLabel}
            </Typography>
          </Box>
        </Box>
      </Box>
    </Overlay>,
    document.body
  );
};

function dictToOptions(dict?: Record<string, string>) {
  if (!dict) return [];
  return Object.entries(dict).map(([key, value]) => ({
    key: Number(key),
    value,
  }));
}

type ValidationState = { response: boolean | null; property: string[] };

function validateForm(
  form: CloneMajorSoftwareBuildDto,
  disabledEoSDate: boolean
): ValidationState {
  const result: ValidationState = { response: true, property: [] };

  const fail = (p: string) => {
    result.property.push(p);
    result.response = false;
  };

  if (!form.softwareVersion?.trim()) fail("softwareVersion");
  if (form.endOfMaintenance === undefined) fail("endOfMaintenance");
  if (
    (form.endOfsupport == null || form.endOfsupport === undefined) &&
    !disabledEoSDate
  )
    fail("endOfsupport");
  if (!form.designContactIds || form.designContactIds.length === 0)
    fail("designContactIds");

  return result;
}

const EMPTY_FORM: CloneMajorSoftwareBuildDto = {
  eomStatus: 1,
  softwareVersion: "",
  designContactIds: [],
  tcpSoftwareCompatibilityIdList: [],
  tciSoftwareCompatibilityIdList: [],
  endOfMaintenance: undefined,
  endOfsupport: undefined,
};

function buildInitialForm(
  src: MajorSoftwareBuildToCloneDto
): CloneMajorSoftwareBuildDto {
  return {
    majorSoftwareBuildsId: src.majorSoftwareBuildsId,
    tcpBundleVersion: src.tcpBundleVersion,
    tciBundleVersion: src.tciBundleVersion,
    designContactIds: src.designContactIds ?? [],
    designContacts: src.designContacts,
    tcpSoftwareCompatibilityIdList: src.tcpSoftwareCompatibilityIdList ?? [],
    tciSoftwareCompatibilityIdList: src.tciSoftwareCompatibilityIdList ?? [],
    eomStatus: 1,
    softwareVersion: "",
    endOfMaintenance: undefined,
    endOfsupport: undefined,
  };
}

const UpgradeSoftwareVersionModal: React.FC<
  UpgradeSoftwareVersionModalProps
> = ({ open, onClose, cloneData, onSuccess }) => {
  const overlayRef = useRef<HTMLDivElement>(null);

  const [formData, setFormData] = useState<CloneMajorSoftwareBuildDto>(() =>
    cloneData != null ? buildInitialForm(cloneData) : EMPTY_FORM
  );
  const [disabledDate, setDisabledDate] = useState(false);
  const [disabledEoSDate, setDisabledEoSDate] = useState(false);
  const [changed, setChanged] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const [validation, setValidation] = useState<ValidationState | null>(null);

  const [confirmDialog, setConfirmDialog] = useState<{
    open: boolean;
    title: string;
    message: string;
    confirmLabel?: string;
    onConfirm: () => void;
  }>({ open: false, title: "", message: "", onConfirm: () => {} });

  const closeConfirm = () =>
    setConfirmDialog((prev) => ({ ...prev, open: false }));

  useEffect(() => {
    if (open && cloneData != null) {
      setFormData(buildInitialForm(cloneData));
      setDisabledDate(false);
      setDisabledEoSDate(false);
      setChanged(false);
      setSubmitting(false);
      setValidation(null);
      closeConfirm();
    }
  }, [open, cloneData]);

  useEffect(() => {
    if (cloneData?.isvmware) {
      setFormData((prev) => ({
        ...prev,
        tcpSoftwareCompatibilityIdList: [],
        tciSoftwareCompatibilityIdList: [],
      }));
    }
  }, [cloneData?.isvmware]);

  useEffect(() => {
    if (!open) return;
    const handler = (e: KeyboardEvent) => {
      if (e.key === "Escape") handleClose();
    };
    document.addEventListener("keydown", handler);
    return () => document.removeEventListener("keydown", handler);
  }, [open, changed]);

  if (!open) return null;

  const designContactOptions = dictToOptions(cloneData?.designContacts);
  const tciOptions = dictToOptions(cloneData?.tciBundleVersion);
  const tcpOptions = dictToOptions(cloneData?.tcpBundleVersion);

  const usedInEntries = cloneData?.designComponentResource
    ? Object.entries(cloneData.designComponentResource)
    : [];

  const hasError = (field: string) =>
    validation?.response === false && validation.property.includes(field);

  const clearValidationProperty = (property: string) => {
    setValidation((prev) => {
      if (!prev) return prev;
      return { ...prev, property: prev.property.filter((p) => p !== property) };
    });
  };

  const handleClose = () => {
    if (changed) {
      setConfirmDialog({
        open: true,
        title: "Exit",
        confirmLabel: "Exit",
        message: "Are you sure you want to exit? Unsaved changes will be lost.",
        onConfirm: () => {
          closeConfirm();
          onClose();
        },
      });
    } else {
      onClose();
    }
  };

  const onHandleChangeAnnounced = (checked: boolean) => {
    setFormData((prev) => {
      const next = { ...prev };
      if (checked) {
        next.eomStatus = 0;
        next.endOfMaintenance = null;
        if (disabledEoSDate) next.endOfsupport = null;
      } else {
        next.eomStatus = 1;
      }
      return next;
    });
    setDisabledDate(checked);
    clearValidationProperty("endOfMaintenance");
  };

  const onHandleCopyEoM = (checked: boolean) => {
    setFormData((prev) => {
      const next = { ...prev };
      if (checked) {
        next.endOfsupport = (next.endOfMaintenance as Date) ?? null;
      }
      return next;
    });
    setDisabledEoSDate(checked);
    if (checked) clearValidationProperty("endOfsupport");
  };

  const onChangeDate = (
    property: "endOfMaintenance" | "endOfsupport",
    newDate: Date | null
  ) => {
    setChanged(true);
    setFormData((prev) => {
      const copy = { ...prev };

      if (property === "endOfMaintenance") {
        if (newDate !== null) {
          copy["eomStatus"] = 2;
        } else {
          copy["eomStatus"] = 0;
        }
      }

      if (property === "endOfsupport") {
        if (newDate !== null) {
          copy["endOfsupport"] = newDate;
        }
      }

      if (newDate == null) {
        copy[lowerFirstLetter(property)] = null;
      } else {
        const DateString = ` ${newDate.getFullYear()}/${
          newDate.getMonth() + 1
        }/${newDate.getDate()}`;
        copy[lowerFirstLetter(property)] = DateString;
      }
      if (disabledEoSDate) {
        copy["endOfsupport"] = (copy?.endOfMaintenance as Date) ?? null;
      }
      return copy;
    });
    clearValidationProperty(property);
  };

  const onChange = (property: string, value: string) => {
    setChanged(true);
    setFormData((prev) => ({ ...prev, [property]: value }));
    clearValidationProperty(property);
  };

  const onChangeMultiSelect = (property: string, selected: any) => {
    const ids =
      selected && (selected as any[]).length > 0
        ? (selected as any[]).map((x) => x.key)
        : undefined;
    setFormData((prev) => ({ ...prev, [property]: ids }));
    clearValidationProperty(property);
  };

  const doSubmit = async () => {
    const finalForm: CloneMajorSoftwareBuildDto = {
      ...formData,
      endOfsupport: disabledEoSDate
        ? formData.endOfMaintenance
        : formData.endOfsupport,
    };

    if (!finalForm.majorSoftwareBuildsId) return;

    setSubmitting(true);
    try {
      const result = await CloneMajorSoftware(finalForm);
      if (result && !result.warning) {
        closeConfirm();
        onSuccess?.();
        onClose();
      }
    } finally {
      setSubmitting(false);
    }
  };

  const handleSubmitClick = async () => {
    const result = validateForm(formData, disabledEoSDate);
    setValidation(result);
    if (!result.response) return;

    if (cloneData?.majorSoftwareBuildsId != null) {
      const preSubmit = await GetMajorSoftwareClonePreSubmit(
        cloneData.majorSoftwareBuildsId,
        false
      );
      if (preSubmit && preSubmit !== undefined) {
        setConfirmDialog({
          open: true,
          title: "Confirm",
          confirmLabel: "Confirm",
          message: rtnConfirmMessage(preSubmit?.info ?? ""),
          onConfirm: () => {
            closeConfirm();
            doSubmit();
          },
        });
        return;
      }
    }

    doSubmit();
  };

  return createPortal(
    <>
      <ConfirmDialog
        open={confirmDialog.open}
        title={confirmDialog.title}
        message={confirmDialog.message}
        confirmLabel={confirmDialog.confirmLabel}
        onConfirm={confirmDialog.onConfirm}
        onCancel={closeConfirm}
      />

      <Overlay
        ref={overlayRef}
        onClick={(e) => {
          if (e.target === overlayRef.current) handleClose();
        }}
      >
        <ModalCard>
          <ModalHeader>
            <HeaderRow>
              <Typography
                sx={{
                  fontWeight: 700,
                  fontSize: "18px",
                  lineHeight: "28px",
                  color: T.black,
                }}
              >
                Create Upgraded Software Product
              </Typography>
              <CloseButton onClick={handleClose}>
                <FiX style={{ fontSize: "20px", color: "#000" }} />
              </CloseButton>
            </HeaderRow>
            <Divider />
          </ModalHeader>

          <ScrollBody>
            <SectionBox>
              <Typography
                sx={{
                  fontWeight: 700,
                  fontSize: "16px",
                  color: T.black,
                }}
              >
                Based on
              </Typography>

              <Box sx={{ display: "flex", flexWrap: "wrap", gap: "24px" }}>
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: "2px",
                    flex: "1 1 280px",
                    minWidth: 0,
                  }}
                >
                  <Typography
                    sx={{
                      fontSize: "13px",
                      fontWeight: 700,
                      color: T.black,
                    }}
                  >
                    Equipment Manufacturer
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "14px",
                      color: "#4B5563",
                    }}
                  >
                    {cloneData?.originalEquipmentManufacturer ?? "—"}
                  </Typography>
                </Box>

                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: "2px",
                    flex: "1 1 280px",
                    minWidth: 0,
                  }}
                >
                  <Typography
                    sx={{
                      fontSize: "13px",
                      fontWeight: 700,
                      color: T.black,
                    }}
                  >
                    Sw Application Type
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "14px",
                      color: "#4B5563",
                    }}
                  >
                    {cloneData?.productName ?? "—"}
                  </Typography>
                </Box>

                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: "2px",
                    flex: "1 1 280px",
                    minWidth: 0,
                  }}
                >
                  <Typography
                    sx={{
                      fontSize: "13px",
                      fontWeight: 700,
                      color: T.black,
                    }}
                  >
                    Release No.
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "14px",
                      color: "#4B5563",
                    }}
                  >
                    {cloneData?.softwareVersion ?? "—"}
                  </Typography>
                </Box>
              </Box>

              <Divider sx={{ borderColor: "rgba(0,0,0,0.08)" }} />

              <Box
                sx={{ display: "flex", flexDirection: "column", gap: "4px" }}
              >
                <Typography
                  sx={{
                    fontSize: "13px",
                    fontWeight: 700,
                    color: T.black,
                  }}
                >
                  Used in
                </Typography>
                <Box
                  sx={{
                    border: "1px solid rgba(0,0,0,0.15)",
                    borderRadius: "6px",
                    background: "#FFFFFF",
                    minHeight: "60px",
                    maxHeight: "160px",
                    overflowY: "auto",
                    padding: "10px 14px",
                    "&::-webkit-scrollbar": { width: "6px" },
                    "&::-webkit-scrollbar-thumb": {
                      background: "rgba(0,0,0,0.15)",
                      borderRadius: "4px",
                    },
                  }}
                >
                  {usedInEntries.length > 0 ? (
                    usedInEntries.map(([key, val]) => (
                      <Typography
                        key={key}
                        sx={{
                          fontSize: "14px",
                          color: T.black,

                          lineHeight: "22px",
                        }}
                        dangerouslySetInnerHTML={{
                          __html: val === "" || val == null ? "---" : val,
                        }}
                      />
                    ))
                  ) : (
                    <Typography
                      sx={{
                        fontSize: "14px",
                        color: "#6B7280",

                        lineHeight: "22px",
                      }}
                    >
                      The record selected is not used in any Design Component
                    </Typography>
                  )}
                </Box>
              </Box>
            </SectionBox>

            <SectionBox>
              <Typography
                sx={{
                  fontWeight: 700,
                  fontSize: "16px",
                  color: T.black,
                }}
              >
                What is the Upgraded Software?
              </Typography>

              <Box
                sx={{
                  display: "flex",
                  flexWrap: "wrap",
                  gap: "20px",
                  alignItems: "flex-start",
                }}
              >
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: "4px",
                    flex: "0.5 1 240px",
                    minWidth: 0,
                  }}
                >
                  <label style={LABEL_STYLE}>
                    What is the Software Version Number? {REQ}
                  </label>
                  <input
                    style={{
                      ...INPUT_STYLE,
                      borderColor: hasError("softwareVersion")
                        ? "#E30000"
                        : "#7E7E7E",
                    }}
                    value={formData.softwareVersion ?? ""}
                    onChange={(e) =>
                      onChange("softwareVersion", e.target.value)
                    }
                  />
                  {hasError("softwareVersion") && (
                    <Typography
                      sx={{
                        fontSize: "12px",
                        color: "#E30000",
                      }}
                    >
                      *Software Version must have a value
                    </Typography>
                  )}
                </Box>
                <Box
                  sx={{ display: "flex", flexDirection: "column", gap: "4px" }}
                >
                  <DateField
                    label={<>End of Maintenance {REQ}</>}
                    value={
                      formData.endOfMaintenance
                        ? new Date(formData.endOfMaintenance)
                        : null
                    }
                    onChange={(newDate) => {
                      onChangeDate("endOfMaintenance", newDate);
                    }}
                    placeholder={
                      formData.eomStatus === 0
                        ? "NOT ANNOUNCED"
                        : "NOT SPECIFIED"
                    }
                    disabled={disabledDate}
                  />
                  {hasError("endOfMaintenance") && (
                    <ValidationLabel>
                      *Cannot be empty. Please select a date or tick Not
                      Announced
                    </ValidationLabel>
                  )}
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: "6px",
                      mt: "4px",
                    }}
                  >
                    <input
                      type="checkbox"
                      checked={formData.eomStatus === 0}
                      onChange={(e) =>
                        onHandleChangeAnnounced(e.target.checked)
                      }
                      style={{
                        width: "16px",
                        height: "16px",
                        cursor: "pointer",
                      }}
                    />
                    <label
                      style={{
                        ...LABEL_STYLE,
                        cursor: "pointer",
                        marginBottom: 0,
                      }}
                    >
                      Not Announced
                    </label>
                  </Box>
                </Box>
                <Box
                  sx={{ display: "flex", flexDirection: "column", gap: "4px" }}
                >
                  <DateField
                    label={<>End of support {REQ}</>}
                    value={
                      disabledEoSDate
                        ? formData.endOfMaintenance
                          ? new Date(formData.endOfMaintenance)
                          : null
                        : formData.endOfsupport
                        ? new Date(formData.endOfsupport)
                        : null
                    }
                    onChange={(newDate) => {
                      onChangeDate("endOfsupport", newDate);
                    }}
                    placeholder={
                      formData.eomStatus === 0 && disabledEoSDate
                        ? "NOT ANNOUNCED"
                        : "NOT SPECIFIED"
                    }
                    disabled={disabledEoSDate}
                  />
                  {hasError("endOfsupport") && !disabledEoSDate && (
                    <ValidationLabel>
                      *End of Support cannot be empty
                    </ValidationLabel>
                  )}
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: "6px",
                      flexShrink: 0,
                    }}
                  >
                    <input
                      type="checkbox"
                      id="eos-same-as-eom"
                      style={{
                        width: "16px",
                        height: "16px",
                        cursor: "pointer",
                        accentColor: T.red,
                      }}
                      checked={disabledEoSDate}
                      onChange={(e) => onHandleCopyEoM(e.target.checked)}
                    />
                    <label
                      htmlFor="eos-same-as-eom"
                      style={{
                        ...LABEL_STYLE,
                        cursor: "pointer",
                        marginBottom: 0,
                      }}
                    >
                      Same as End Of Maintenance
                    </label>
                  </Box>
                </Box>
              </Box>

              <Box
                sx={{
                  display: "flex",
                  flexWrap: "wrap",
                  gap: "20px",
                  alignItems: "flex-start",
                }}
              >
                <Box
                  sx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: "4px",
                    flex: "0.5 1 240px",
                    minWidth: 0,
                  }}
                >
                  <label style={LABEL_STYLE}>
                    Software Product Owner {REQ}
                  </label>
                  <Select
                    menuPortalTarget={document.body}
                    styles={{
                      ...SELECT_STYLES,
                      control: (b: any) => ({
                        ...SELECT_STYLES.control(b, {}),
                        borderColor: hasError("designContactIds")
                          ? "#E30000"
                          : "#7E7E7E",
                      }),
                    }}
                    options={designContactOptions}
                    value={designContactOptions.filter((o) =>
                      formData.designContactIds?.includes(o.key)
                    )}
                    onChange={(selected) =>
                      onChangeMultiSelect("designContactIds", selected)
                    }
                    isMulti
                    isSearchable
                    isClearable
                    getOptionLabel={(o: any) => o.value}
                    getOptionValue={(o: any) => o.key.toString()}
                    formatOptionLabel={(o: any) => (
                      <span dangerouslySetInnerHTML={{ __html: o.value }} />
                    )}
                    placeholder="Select contacts..."
                  />
                  {hasError("designContactIds") && (
                    <Typography
                      sx={{
                        fontSize: "12px",
                        color: "#E30000",
                      }}
                    >
                      *Software Product Owner must have a value
                    </Typography>
                  )}
                </Box>
              </Box>
              <Box
                sx={{
                  display: "flex",
                  flexWrap: "wrap",
                  gap: "20px",
                  alignItems: "flex-start",
                }}
              >
                {" "}
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: "12px",
                    flex: "0.4 1 240px",
                    minWidth: 0,
                  }}
                >
                  <ToggleSwitch
                    checked={cloneData?.isvmware ? true : false}
                    disabled
                    label={`This is ${
                      cloneData?.isvmware ? "" : "not"
                    } a Platform Software (e.g. Broadcom)`}
                    showToggle={false}
                  />
                </Box>
              </Box>
            </SectionBox>

            {/* {!cloneData?.isvmware && (
              <SectionBox>
                <Typography
                  sx={{
                    fontWeight: 700,
                    fontSize: "16px",
                    color: T.black,
                  }}
                >
                  VmWare Compatibility Attribute
                </Typography>

                <Box sx={{ display: "flex", flexWrap: "wrap", gap: "20px" }}>
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      gap: "4px",
                      flex: "1 1 240px",
                      minWidth: 0,
                    }}
                  >
                    <label style={LABEL_STYLE}>TCI</label>
                    <Select
                      menuPortalTarget={document.body}
                      styles={SELECT_STYLES}
                      options={tciOptions}
                      value={tciOptions.filter((o) =>
                        formData.tciSoftwareCompatibilityIdList?.includes(o.key)
                      )}
                      onChange={(selected) =>
                        onChangeMultiSelect(
                          "tciSoftwareCompatibilityIdList",
                          selected
                        )
                      }
                      isMulti
                      isSearchable
                      isClearable
                      getOptionLabel={(o: any) => o.value.toString()}
                      getOptionValue={(o: any) => o.key.toString()}
                      placeholder="Select..."
                    />
                  </Box>

                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      gap: "4px",
                      flex: "1 1 240px",
                      minWidth: 0,
                    }}
                  >
                    <label style={LABEL_STYLE}>TCP</label>
                    <Select
                      menuPortalTarget={document.body}
                      styles={SELECT_STYLES}
                      options={tcpOptions}
                      value={tcpOptions.filter((o) =>
                        formData.tcpSoftwareCompatibilityIdList?.includes(o.key)
                      )}
                      onChange={(selected) =>
                        onChangeMultiSelect(
                          "tcpSoftwareCompatibilityIdList",
                          selected
                        )
                      }
                      isMulti
                      isSearchable
                      isClearable
                      getOptionLabel={(o: any) => o.value.toString()}
                      getOptionValue={(o: any) => o.key.toString()}
                      placeholder="Select..."
                    />
                  </Box>
                </Box>
              </SectionBox>
            )} */}
          </ScrollBody>

          <ModalFooter>
            <Box
              onClick={handleClose}
              sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                px: "24px",
                py: "8px",
                border: "1px solid #000",
                borderRadius: "6px",
                cursor: "pointer",
                minWidth: "100px",
                height: "40px",
                "&:hover": { background: "#F5F5F5" },
              }}
            >
              <Typography sx={{ fontSize: "16px", color: "#000" }}>
                Cancel
              </Typography>
            </Box>
            <Box
              onClick={submitting ? undefined : handleSubmitClick}
              sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                px: "24px",
                py: "8px",
                background: submitting ? "#999" : T.red,
                borderRadius: "6px",
                cursor: submitting ? "not-allowed" : "pointer",
                minWidth: "100px",
                height: "40px",
                "&:hover": { background: submitting ? "#999" : "#C40000" },
              }}
            >
              <Typography sx={{ fontSize: "16px", color: "#FFF" }}>
                {submitting ? "Submitting..." : "Submit"}
              </Typography>
            </Box>
          </ModalFooter>
        </ModalCard>
      </Overlay>
    </>,
    document.body
  );
};

export default UpgradeSoftwareVersionModal;
