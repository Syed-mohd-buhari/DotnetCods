import React, { useCallback, useEffect, useState } from "react";
import { Box, CircularProgress, Tooltip, Typography } from "@mui/material";
import {
  FiChevronRight,
  FiChevronLeft,
  FiEdit2,
  FiCheck,
} from "react-icons/fi";
import { Product } from "./ProductSoftwareTypes";
import SoftwareVersionTable from "./SoftwareVersionTable";
import CustomCalendarIcon from "../../../img/CustomCalendarIcon";
import CustomBoxIcon from "../../../img/CustomBoxIcon";
import CustomPeopleIcon from "../../../img/CustomPeopleIcon";
import {
  CustomTooltip,
  TruncText,
} from "../../components/table/CustomTable.atoms";
import { T } from "../../utils/styled";
import { InfoCard, RedButton } from "../../utils/styled";
import { TbEdit } from "react-icons/tb";
import { MdDeleteOutline, MdOutlineRefresh } from "react-icons/md";
import LinksCard from "./LinksCard";
import WarningTriangleFlat from "../../../img/WarningTriangleFlat";
import {
  EditMajorSoftwareBuild,
  GetMajorSoftwareBuildEditResource,
} from "../../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildEditAction";
import VendorIcon from "../../../img/userIcon.png";
import ProductIcon from "../../../img/tagIcon.png";
import PlatformIcon from "../../../img/platformIcon.png";
import LastUpdatedIcon from "../../../img/updateIcon.png";

interface ProductDetailProps {
  product: Product;
  onBack: () => void;
  onEditSoftware: () => void;
  onDeleteSoftware?: (product: any) => void;
  onUpgradeSoftwareVersion?: (product: any) => void;
  onProductUpdated?: (productId: any) => Promise<void> | void;
}

const ProductDetail: React.FC<ProductDetailProps> = ({
  product,
  onBack,
  onEditSoftware,
  onDeleteSoftware,
  onUpgradeSoftwareVersion,
  onProductUpdated,
}) => {
  const [hardwareLinks, setHardwareLinks] = useState<any>([]);
  const [servicesLinks, setServicesLinks] = useState<any>([]);
  const [isComplaintLogic, setIsComplaintLogic] = useState<boolean>(false);
  const [isHoveringVulnerability, setIsHoveringVulnerability] = useState(false);
  const [isEditingVulnerability, setIsEditingVulnerability] = useState(false);
  const [vulnerabilityValue, setVulnerabilityValue] = useState<string>(
    product?.vulnerabilityStatus ?? ""
  );
  const [isSavingVulnerability, setIsSavingVulnerability] = useState(false);
  const [vulnerabilityChanged, setVulnerabilityChanged] = useState(false);
  useEffect(() => {
    setVulnerabilityValue(product?.vulnerabilityStatus ?? "");
    setIsEditingVulnerability(false);
    setVulnerabilityChanged(false);
  }, [product?.vulnerabilityStatus, product?.id]);

  const contacts = (product?.designContact || "")
    .split(",")
    .map((x) => x.trim())
    .filter(Boolean);
  const updateLinkedData = useCallback((hardware, services) => {
    setHardwareLinks([...hardware]);
    setServicesLinks([...services]);
  }, []);

  const isComplaint = useCallback((value) => {
    setIsComplaintLogic(value);
  }, []);

  const handleStartEditVulnerability = (e?: React.MouseEvent) => {
    e?.preventDefault();
    e?.stopPropagation();
    setVulnerabilityValue(product?.vulnerabilityStatus ?? "");
    setIsEditingVulnerability(true);
    setVulnerabilityChanged(false);
  };

  const handleChangeVulnerability = (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    const newValue = e.target.value;
    setVulnerabilityValue(newValue);
    setVulnerabilityChanged(newValue !== (product?.vulnerabilityStatus ?? ""));
  };

  const handleSaveVulnerability = async (e?: React.MouseEvent) => {
    e?.preventDefault();
    e?.stopPropagation();
    if (!product?.id) return;
    setIsSavingVulnerability(true);
    try {
      const editResource: any = await GetMajorSoftwareBuildEditResource(
        product.id
      );
      const payload = {
        ...editResource,
        majorSoftwareBuildId: product.id,
        vulnerabilityStatus: vulnerabilityValue,
      };
      await EditMajorSoftwareBuild(payload);

      setIsEditingVulnerability(false);
      setVulnerabilityChanged(false);
      await onProductUpdated?.(product.id);
    } catch (err) {
      console.error("Failed to update vulnerability status:", err);
    } finally {
      setIsSavingVulnerability(false);
    }
  };
  const handleEditSoftwareClick = async () => {
    if (product?.id) {
      try {
        await GetMajorSoftwareBuildEditResource(product.id);
      } catch (err) {
        console.error("Failed to pre-fetch edit resource:", err);
      }
    }
    onEditSoftware();
  };

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        gap: "22px",
        padding: "0 24px",
      }}
    >
      <Box
        sx={{
          background: "#F0F0F3",
          borderRadius: "0 0 10px 10px",
          p: "16px 24px",
          display: "flex",
          flexDirection: "column",
          gap: "34px",
        }}
      >
        <Box
          sx={{
            display: "flex",
            flexWrap: "wrap",
            justifyContent: "space-between",
            alignItems: "center",
            gap: "16px",
          }}
        >
          <FiChevronLeft
            size={18}
            style={{ cursor: "pointer" }}
            onClick={onBack}
          />
          <Box sx={{ display: "flex", alignItems: "center", gap: "16px" }}>
            <Typography
              sx={{
                fontWeight: 700,
                fontSize: "30px",
                lineHeight: "26px",
                color: "#000",
              }}
            >
              {product?.productRecord}
            </Typography>
            <Box
              sx={{
                display: "inline-flex",
                alignItems: "center",
                gap: "6px",
                border: `1px solid ${isComplaintLogic ? "#6CDC00" : "#E60000"}`,
                borderRadius: "5px",
                p: "4px 14px",
              }}
            >
              {isComplaintLogic ? (
                <Box
                  sx={{
                    width: "12px",
                    height: "12px",
                    borderRadius: "50%",
                    background: "#6CDC00",
                  }}
                />
              ) : (
                <Box
                  sx={{
                    width: "12px",
                    height: "12px",
                    borderRadius: "50%",
                    background: "#E60000",
                  }}
                />
              )}
              <Typography sx={{ fontSize: "14px", color: "#000" }}>
                {isComplaintLogic ? "Complaint" : "Non Complaint"}
              </Typography>
            </Box>
          </Box>
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              gap: "8px",
              cursor: "pointer",
            }}
            onClick={onBack}
          >
            <Typography
              sx={{
                fontSize: "14px",
                textDecoration: "underline",
                color: "rgba(0,0,0,0.7)",
              }}
            >
              Product
            </Typography>
            <FiChevronRight size={14} style={{ color: T.red }} />
            <Typography sx={{ fontWeight: 700, fontSize: "14px" }}>
              Software
            </Typography>
          </Box>
        </Box>

        <Box
          sx={{
            display: "flex",
            flexWrap: "wrap",
            justifyContent: "space-between",
            alignItems: "center",
            gap: "16px",
          }}
        >
          <Box sx={{ display: "flex", flexWrap: "wrap", gap: "16px" }}>
            {[
              { label: "Vendor:", value: product?.vendor, icon: VendorIcon },
              {
                label: "Product Name:",
                value: product?.productRecord,
                icon: ProductIcon,
              },
              {
                label: "Is Platform?",
                value: product?.isPlatform ? "Y" : "N",
                icon: PlatformIcon,
              },
              {
                label: "Last updated:",
                value: product?.ownerName,
                icon: LastUpdatedIcon,
              },
            ].map((item, i) => (
              <Box
                key={i}
                sx={{ display: "flex", alignItems: "center", gap: "6px" }}
              >
                <Box
                  component="img"
                  src={item.icon}
                  alt=""
                  sx={{ width: "13px", height: "13px", objectFit: "contain" }}
                />
                <Typography sx={{ fontSize: "14px", color: "#79797A" }}>
                  {item.label}
                </Typography>
                <Typography sx={{ fontSize: "14px", color: "#272727" }}>
                  {item.value}
                </Typography>
                {i < 3 && (
                  <Box
                    sx={{
                      width: "1px",
                      height: "20px",
                      background: "rgba(0,0,0,0.3)",
                      ml: "12px",
                    }}
                  />
                )}
              </Box>
            ))}
          </Box>
          {/* <RedButton onClick={onEditSoftware}>
            <FiEdit2 size={12} style={{ color: "#fff" }} />
            <Typography sx={{ fontSize: "14px", color: "#fff" }}>
              Edit Software
            </Typography>
          </RedButton> */}
          <Box
            sx={{
              display: "flex",
              gap: "22px",
            }}
          >
            <Tooltip
              title="Edit Software"
              slotProps={{
                tooltip: {
                  sx: {
                    backgroundColor: "black",
                    color: "white",
                    fontSize: "12px",
                    borderRadius: "6px",
                    px: 2,
                    py: 1,
                  },
                },
              }}
            >
              <Box onClick={handleEditSoftwareClick} sx={{ cursor: "pointer" }}>
                <TbEdit size={20} />
              </Box>
            </Tooltip>
            <Tooltip
              title="Upgrade Software Version"
              slotProps={{
                tooltip: {
                  sx: {
                    backgroundColor: "black",
                    color: "white",
                    fontSize: "12px",
                    borderRadius: "6px",
                    px: 2,
                    py: 1,
                  },
                },
              }}
            >
              <Box
                onClick={() =>
                  onUpgradeSoftwareVersion && onUpgradeSoftwareVersion(product)
                }
                sx={{ cursor: "pointer" }}
              >
                <MdOutlineRefresh size={20} />
              </Box>
            </Tooltip>
            <Tooltip
              title="Delete"
              slotProps={{
                tooltip: {
                  sx: {
                    backgroundColor: "black",
                    color: "white",
                    fontSize: "12px",
                    borderRadius: "6px",
                    px: 2,
                    py: 1,
                  },
                },
              }}
            >
              <Box
                onClick={() => onDeleteSoftware && onDeleteSoftware(product)}
                sx={{ cursor: "pointer" }}
              >
                <MdDeleteOutline size={20} color="#E60000" />
              </Box>
            </Tooltip>
          </Box>
        </Box>
      </Box>

      <Box
        sx={{
          display: "flex",
          gap: "22px",
          flexWrap: "nowrap",
          overflow: "hidden",
          "&:hover": { overflowX: "auto", height: "92px" },
          "&::-webkit-scrollbar": { height: "6px" },
          "&::-webkit-scrollbar-thumb": {
            background: "rgba(0,0,0,0.18)",
            borderRadius: "4px",
          },
        }}
      >
        <InfoCard>
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              gap: "16px",
              flex: 1,
              cursor: "default",
            }}
          >
            <Box
              sx={{ width: "36px", height: "36px", flexShrink: 0 }}
              className="alert-icon"
            >
              <CustomBoxIcon
                className="alert-icon"
                sx={{
                  color: "#E60000",
                  ".alert-card:hover &": {
                    color: "#FFFFFF",
                  },
                }}
              />
            </Box>
            <Box>
              <Typography
                sx={{ fontSize: "16px", color: T.black }}
                className="alert-label"
              >
                Current release
              </Typography>
              <Typography
                sx={{
                  fontWeight: 700,
                  fontSize: "16px",
                  color: T.black,
                  textAlign: "left",
                }}
                className="alert-label"
              >
                {product?.softwareVersion}
              </Typography>
            </Box>
          </Box>
          <Box
            sx={{
              background: "rgba(33,132,83,0.1)",
              borderRadius: "10px",
              p: "3.5px 6.5px",
              cursor: "default",
            }}
          >
            <Typography
              sx={{ fontWeight: 700, fontSize: "12px", color: "#218453" }}
              className="alert-label"
            >
              Active
            </Typography>
          </Box>
        </InfoCard>

        <InfoCard>
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              gap: "16px",
              flex: 1,
              cursor: "default",
            }}
          >
            <Box
              className="cal-icon"
              sx={{
                width: "36px",
                height: "36px",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                borderRadius: "50%",
                flexShrink: 0,
              }}
            >
              <CustomCalendarIcon
                className="alert-icon"
                sx={{
                  color: "#E60000",
                  ".alert-icon:hover &": {
                    color: "#FFFFFF",
                  },
                }}
              />
            </Box>
            <Box>
              <Typography
                sx={{ fontSize: "16px", color: T.black, textAlign: "left" }}
                className="alert-label"
              >
                Support timeline
              </Typography>
              <Box sx={{ display: "flex", gap: "12px", textAlign: "left" }}>
                {[
                  { label: "EOS:", value: product?.endOfsupportValue },
                  { label: "EOM:", value: product?.endOfMaintenanceValue },
                ].map(({ label, value }) => (
                  <Typography
                    key={label}
                    sx={{ fontSize: "16px", color: T.black }}
                    className="alert-label"
                  >
                    <span>{label}</span>{" "}
                    <span style={{ fontWeight: 700 }}>{value}</span>
                  </Typography>
                ))}
              </Box>
            </Box>
          </Box>
        </InfoCard>

        <InfoCard sx={{ flex: "0 0 378px", cursor: "default" }}>
          <Box
            sx={{ display: "flex", alignItems: "center", gap: "16px", flex: 1 }}
          >
            <Box sx={{ width: "36px", height: "36px", flexShrink: 0 }}>
              <WarningTriangleFlat
                className="alert-icon"
                sx={{
                  color: "#E60000",
                  ".alert-card:hover &": {
                    color: "#FFFFFF",
                  },
                }}
              />
            </Box>
            <Box sx={{ flex: 1, minWidth: 0 }}>
              <Typography
                sx={{ fontSize: "16px", color: T.black, textAlign: "left" }}
                className="alert-label"
              >
                Vulnerability
              </Typography>
              <Box
                className="vuln-hover-row"
                sx={{
                  display: "flex",
                  alignItems: "center",
                  gap: "8px",
                  minHeight: "24px",
                }}
              >
                {isEditingVulnerability ? (
                  <input
                    autoFocus
                    value={vulnerabilityValue}
                    onChange={handleChangeVulnerability}
                    disabled={isSavingVulnerability}
                    style={{
                      fontSize: "12px",
                      fontWeight: 700,
                      color: "#853525",
                      background: "#FEF3EB",
                      border: "1px solid #E9C4A8",
                      borderRadius: "6px",
                      padding: "3.5px 6.5px",
                      outline: "none",
                      width: "180px",
                    }}
                  />
                ) : (
                  <Box
                    onMouseDown={handleStartEditVulnerability}
                    sx={{
                      background: "#FEF3EB",
                      borderRadius: "6px",
                      p: "3.5px 6.5px",
                      cursor: "pointer",
                    }}
                  >
                    <Typography
                      sx={{
                        fontWeight: 700,
                        fontSize: "12px",
                        color: "#853525",
                      }}
                    >
                      {vulnerabilityValue || "---"}
                    </Typography>
                  </Box>
                )}

                {isSavingVulnerability ? (
                  <CircularProgress size={14} sx={{ color: "#853525" }} />
                ) : isEditingVulnerability && vulnerabilityChanged ? (
                  <Tooltip title="Save">
                    <Box
                      onMouseDown={handleSaveVulnerability}
                      sx={{ cursor: "pointer", display: "flex" }}
                    >
                      <FiCheck size={16} style={{ color: "#218453" }} />
                    </Box>
                  </Tooltip>
                ) : !isEditingVulnerability ? (
                  <Tooltip title="Edit">
                    <Box
                      onMouseDown={handleStartEditVulnerability}
                      sx={{
                        cursor: "pointer",
                        display: "flex",
                        opacity: 0,
                        transition: "opacity 0.15s ease",
                        ".vuln-hover-row:hover &": {
                          opacity: 1,
                        },
                      }}
                    >
                      <FiEdit2 size={14} style={{ color: "#79797A" }} />
                    </Box>
                  </Tooltip>
                ) : null}
              </Box>
            </Box>
          </Box>
        </InfoCard>

        <InfoCard sx={{ flex: "0 0 378px", cursor: "default" }}>
          <Box
            sx={{ display: "flex", alignItems: "center", gap: "16px", flex: 1 }}
          >
            <Box sx={{ width: "36px", height: "36px", flexShrink: 0 }}>
              <CustomPeopleIcon
                className="alert-icon"
                sx={{
                  color: "#E60000",
                  ".alert-card:hover &": {
                    color: "#FFFFFF",
                  },
                }}
              />
            </Box>
            <Box>
              <Typography
                sx={{ fontSize: "16px", color: T.black, textAlign: "left" }}
                className="alert-label"
              >
                Ownership
              </Typography>
              <Box sx={{ display: "flex", alignItems: "center" }}>
                <Typography
                  sx={{
                    fontSize: "14px",
                    color: "rgba(0,0,0,0.7)",
                    width: "116px",
                    textAlign: "left",
                  }}
                  className="alert-label"
                >
                  Software contact:
                </Typography>
                <CustomTooltip text={contacts.join("\n")}>
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: "2px",
                      color: T.black,
                    }}
                    className="alert-label"
                  >
                    <TruncText
                      text={contacts[0] || "—"}
                      maxWidth={150}
                      sx={{
                        fontWeight: 700,
                        fontSize: "14px",
                        color: T.black,
                        "&:hover": {
                          color: "white",
                        },
                      }}
                    />
                  </Box>
                </CustomTooltip>
              </Box>
            </Box>
          </Box>
        </InfoCard>
      </Box>

      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          gap: "22px",
          alignItems: "flex-start",
          justifyContent: "space-between",
        }}
      >
        <SoftwareVersionTable
          title="Software Version"
          product={product}
          linkedData={updateLinkedData}
          isComplaint={isComplaint}
        />

        <LinksCard
          hardwareLinks={hardwareLinks}
          servicesLinks={servicesLinks}
        />
      </Box>
    </Box>
  );
};

export default ProductDetail;
