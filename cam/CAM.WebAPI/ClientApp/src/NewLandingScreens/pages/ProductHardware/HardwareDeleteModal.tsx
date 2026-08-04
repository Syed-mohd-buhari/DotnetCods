import React, { useEffect, useRef, useState } from "react";
import { Box, Divider, Typography } from "@mui/material";
import { FiX, FiAlertTriangle } from "react-icons/fi";
import { createPortal } from "react-dom";
import { MajorHardwareBuildDtoGrid } from "../../../Model/MajorHardwareBuild";
import { GetRelatedRecordsMajorHardwareBuild } from "../../../Redux/Action/MajorHardwareBuild/MajorHardwareBuildDeleteAction";
import { DeleteStep } from "../../utils/common";
import {
  DeleteModalCard,
  T,
  OutlineBtn,
  RedBtn,
  Overlay,
  CloseButton,
} from "../../utils/styled";
import { RelatedRecordsResponse } from "./ProductHardwareTypes";

export interface DeleteModalProps {
  open: boolean;
  row: MajorHardwareBuildDtoGrid | null;
  onClose: () => void;
  onConfirmed: (row: MajorHardwareBuildDtoGrid) => void;
}

const DeleteModal: React.FC<DeleteModalProps> = ({
  open,
  row,
  onClose,
  onConfirmed,
}) => {
  const [step, setStep] = useState<DeleteStep>("checking");
  const [relatedData, setRelatedData] = useState<RelatedRecordsResponse | null>(
    null
  );
  const overlayRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!open || !row) return;
    setStep("checking");
    setRelatedData(null);
    checkRelations();
  }, [open, row]);

  useEffect(() => {
    if (!open) return;
    const onKey = (e: KeyboardEvent) => {
      if (e.key === "Escape") onClose();
    };
    document.addEventListener("keydown", onKey);
    return () => document.removeEventListener("keydown", onKey);
  }, [open, onClose]);

  const checkRelations = async () => {
    try {
      const result: any = await GetRelatedRecordsMajorHardwareBuild(
        (row as any)?.majorHardwareBuildId
      );
      setRelatedData(result);
      setStep(result.data !== null ? "has-relations" : "safe-to-delete");
    } catch {
      setStep("safe-to-delete");
    }
  };

  if (!open || !row) return null;

  const recordLabel = `"${(row as any).originalEquipmentManufacturer ?? ""} - ${
    (row as any).platform ?? ""
  } - ${(row as any).hardwareType ?? ""}"`;

  return createPortal(
    <Overlay
      ref={overlayRef}
      onClick={(e) => {
        if (e.target === overlayRef.current) onClose();
      }}
    >
      {step === "checking" && (
        <DeleteModalCard>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              p: "20px 24px 16px",
            }}
          >
            <Typography
              sx={{ fontWeight: 700, fontSize: "18px", color: T.black }}
            >
              Delete Entry
            </Typography>
            <CloseButton onClick={onClose}>
              <FiX style={{ fontSize: "20px", color: "#000" }} />
            </CloseButton>
          </Box>
          <Divider />
          <Box sx={{ p: "24px" }}>
            <Typography sx={{ fontSize: "15px", color: T.black }}>
              Checking related records…
            </Typography>
          </Box>
          <Box
            sx={{
              display: "flex",
              justifyContent: "flex-end",
              gap: "12px",
              p: "16px 24px",
              borderTop: "1px solid #E5E5E5",
            }}
          >
            <OutlineBtn onClick={onClose} disabled>
              Close
            </OutlineBtn>
            <RedBtn disabled sx={{ opacity: 0.6 }}>
              Checking…
            </RedBtn>
          </Box>
        </DeleteModalCard>
      )}

      {step === "has-relations" && relatedData && (
        <DeleteModalCard sx={{ maxWidth: "560px" }}>
          <Box
            sx={{
              background: T.red,
              borderRadius: "8px 8px 0 0",
              p: "12px 16px",
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              gap: "10px",
            }}
          >
            <Box sx={{ display: "flex", alignItems: "center", gap: "10px" }}>
              <FiAlertTriangle
                style={{ color: "#fff", fontSize: "18px", flexShrink: 0 }}
              />
              <Typography
                sx={{ fontSize: "14px", color: "#fff", fontWeight: 600 }}
              >
                {relatedData.info || "The record is not orphaned."}
              </Typography>
            </Box>
            <CloseButton onClick={onClose} sx={{ flexShrink: 0 }}>
              <FiX style={{ fontSize: "18px", color: "#fff" }} />
            </CloseButton>
          </Box>

          <Box sx={{ p: "20px 24px 12px" }}>
            <Typography
              sx={{ fontWeight: 700, fontSize: "18px", color: T.black }}
            >
              Delete Entry
            </Typography>
          </Box>
          <Divider />

          <Box
            sx={{
              p: "16px 24px",
              display: "flex",
              flexDirection: "column",
              gap: "12px",
            }}
          >
            <Typography sx={{ fontSize: "14px", color: T.black }}>
              The {relatedData.data?.entityName ?? "Major Hardware Build"}
            </Typography>
            <Typography sx={{ fontSize: "14px", color: T.black }}>
              <strong>{recordLabel}</strong> is related to:
            </Typography>

            {relatedData.data?.dataRelatedList?.map((section, si) => (
              <Box key={si}>
                <Box
                  sx={{
                    display: "inline-flex",
                    borderBottom: `2px solid ${T.red}`,
                    pb: "4px",
                    mb: "8px",
                  }}
                >
                  <Typography
                    sx={{ fontWeight: 700, fontSize: "14px", color: T.black }}
                  >
                    {section.table}
                  </Typography>
                </Box>
                <Box
                  sx={{
                    border: "1px solid rgba(0,0,0,0.1)",
                    borderRadius: "6px",
                    maxHeight: "200px",
                    overflowY: "auto",
                    p: "10px 14px",
                    background: "#FAFAFA",
                    "&::-webkit-scrollbar": { width: "6px" },
                    "&::-webkit-scrollbar-thumb": {
                      background: "rgba(0,0,0,0.15)",
                      borderRadius: "4px",
                    },
                  }}
                >
                  {section.values.map((val, vi) => (
                    <Typography
                      key={vi}
                      sx={{
                        fontSize: "13px",
                        color: T.black,
                        lineHeight: "22px",
                      }}
                      dangerouslySetInnerHTML={{ __html: val }}
                    />
                  ))}
                </Box>
              </Box>
            ))}
          </Box>

          <Box
            sx={{
              display: "flex",
              justifyContent: "flex-end",
              p: "16px 24px",
              borderTop: "1px solid #E5E5E5",
            }}
          >
            <RedBtn onClick={onClose}>Close</RedBtn>
          </Box>
        </DeleteModalCard>
      )}

      {step === "safe-to-delete" && (
        <DeleteModalCard>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              p: "20px 24px 16px",
            }}
          >
            <Typography
              sx={{ fontWeight: 700, fontSize: "18px", color: T.black }}
            >
              Delete Entry
            </Typography>
            <CloseButton onClick={onClose}>
              <FiX style={{ fontSize: "20px", color: "#000" }} />
            </CloseButton>
          </Box>
          <Divider />
          <Box sx={{ p: "24px" }}>
            <Typography sx={{ fontSize: "15px", color: T.black }}>
              <strong>Do you want to delete this item?</strong>
            </Typography>
          </Box>
          <Box
            sx={{
              display: "flex",
              justifyContent: "flex-end",
              gap: "12px",
              p: "16px 24px",
              borderTop: "1px solid #E5E5E5",
            }}
          >
            <OutlineBtn onClick={onClose}>Close</OutlineBtn>
            <RedBtn
              onClick={() => {
                onClose();
                onConfirmed(row!);
              }}
            >
              Delete
            </RedBtn>
          </Box>
        </DeleteModalCard>
      )}
    </Overlay>,
    document.body
  );
};

export default DeleteModal;
