import React, { useState, useEffect } from "react";
import {
  AiOutlinePlusCircle,
  AiOutlineSearch,
  AiOutlineMinus,
  AiOutlineEdit,
} from "react-icons/ai";
import { useNavigate } from "react-router-dom";
import { Box, Paper, Tooltip } from "@mui/material";
import { MdAdd } from "react-icons/md";
import { IoCheckmarkDoneCircle, IoCloseCircleOutline } from "react-icons/io5";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { GetUserPrefrenceDetails } from "../../Redux/Action/LandingPage/AbstractionLayerAction";
import { safeNumber } from "../../Hook/Common";
import { SaveCustomGridRender } from "../../Redux/Action/Grid/SaveGridCustom";
import { useAuth } from "../../Hook/useAuth";
import { useModal } from "../../Hook/useModal";
import { DataModalConfirm, stateConfirm } from "../../Model/Common";
import { ResetForeignIndex } from "../../Redux/Action/ForeignIndex/ForeignIndexCommonAction";
import ModalConfirm from "../../Components/ModalConfirm";
export interface QuickLink {
  id: number;
  text: string;
  path: string;
  default: boolean;
  order: number;
  menu?: string;
}

interface QuickLinksProps {
  onAddLink?: () => void;
  monthRangeData?: any;
  onApiSuccess?: () => void;
}
const reAssignOrder = (links: QuickLink[]): QuickLink[] =>
  links?.map((link, index) => ({ ...link, order: index }));

const sortByOrder = (links: QuickLink[]): QuickLink[] =>
  [...links]?.sort((a, b) => a.order - b.order);

const deduplicateById = (links: QuickLink[]): QuickLink[] =>
  links?.filter(
    (link, index, self) =>
      self.findIndex((l) => safeNumber(l.id) === safeNumber(link.id)) === index
  );

const QuickLinkNew: React.FC<QuickLinksProps> = ({
  onAddLink,
  monthRangeData,
  onApiSuccess,
}) => {
  const navigate = useNavigate();
  const { userPrefrenceDetails, isPermesso } = useAuth();
  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );
  const [searchQuery, setSearchQuery] = useState("");
  const [initialQuickLinks, setInitialQuickLinks] = useState<QuickLink[]>([]);
  const [quickLinks, setQuickLinks] = useState<QuickLink[]>([]);
  const [newQuickLinks, setNewQuickLinks] = useState<QuickLink[]>([]);
  const [errorMessage, setErrorMessage] = useState("");
  const [isEditMode, setIsEditMode] = useState(false);
  const [draggedItem, setDraggedItem] = useState<QuickLink | null>(null);
  const [draggedOverItem, setDraggedOverItem] = useState<QuickLink | null>(
    null
  );

  const {
    isVisibleModalManage,
    setIsVisibleModalManage,
    isVisibleModalInitializeNewProduct,
    setIsVisibleModalInitializeNewProduct,
    isVisibleModalProductLifecycle,
    setIsVisibleModalProductLifecycle,
    isVisibleModalStatus,
    setIsVisibleModalStatus,
  } = useModal();

  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  useEffect(() => {
    if (monthRangeData) {
      handleSave(monthRangeData);
    }
  }, [monthRangeData]);

  useEffect(() => {
    if (userPrefrenceDetails && isPermesso) {
      const combinedUserPrefrenceMenus = deduplicateById(
        [
          ...userPrefrenceDetails?.pagePrefrenceDetail,
          ...userPrefrenceDetails?.popupPagePrefrenceDetail,
        ]?.map((res) => ({ ...res, id: safeNumber(res.id) })) as QuickLink[]
      );
      setInitialQuickLinks(deduplicateById(combinedUserPrefrenceMenus));
      fetchDefaultButtons(deduplicateById(combinedUserPrefrenceMenus));
    }
  }, [userPrefrenceDetails, isPermesso]);

  const fetchDefaultButtons = async (initialLink?: any) => {
    const response = await GetUserPrefrenceDetails(userInfo?.account?.userid);

    const sourceLinks: QuickLink[] =
      response?.length !== 0
        ? response?.map((res) => ({ ...res, id: safeNumber(res.id) })) ?? []
        : initialLink ?? [];

    const uniqueLinks = deduplicateById(sourceLinks);

    const hasExplicitDefaults = uniqueLinks?.some(
      (link) => link.default === true
    );

    let updatedLinks: QuickLink[] = [];

    if (hasExplicitDefaults) {
      let defaultCount = 0;
      updatedLinks =
        uniqueLinks?.map((link) => {
          if (link.default && defaultCount < 10) {
            defaultCount++;
            return link;
          }
          return { ...link, default: false };
        }) ?? [];
    } else {
      updatedLinks =
        uniqueLinks?.map((link, index) => ({
          ...link,
          default: index < 10,
        })) ?? [];
    }

    const newQuickLinkIds = new Set(
      updatedLinks.map((link) => safeNumber(link.id))
    );

    const deduplicatedInitial = deduplicateById(initialLink ?? []);
    const remainingFromInitial =
      deduplicatedInitial?.filter(
        (link) => !newQuickLinkIds.has(safeNumber(link.id))
      ) ?? [];

    setQuickLinks(updatedLinks);
    setNewQuickLinks([...updatedLinks, ...remainingFromInitial]);
  };

  const sortedLinks = (links: QuickLink[]) => sortByOrder(links);

  const filteredLinkList = searchQuery?.trim()
    ? (() => {
        const searchSource = initialQuickLinks?.filter((link) =>
          link.text.toLowerCase().includes(searchQuery.toLowerCase())
        );

        if (isEditMode) {
          return searchSource
            ?.map((link) => {
              const inNew = newQuickLinks?.find(
                (l) => safeNumber(l.id) === safeNumber(link.id)
              );
              return inNew ?? link;
            })
            .slice(0, 10);
        }

        return searchSource?.slice(0, 10);
      })()
    : isEditMode
    ? (() => {
        const newQuickLinkIds = new Set(
          newQuickLinks?.map((link) => safeNumber(link.id))
        );
        const remainingFromInitial = initialQuickLinks?.filter(
          (link) => !newQuickLinkIds.has(safeNumber(link.id))
        );
        return [
          ...sortedLinks(newQuickLinks)?.filter((link) => link.default),
          ...sortedLinks(newQuickLinks)?.filter((link) => !link.default),
          ...remainingFromInitial,
        ].slice(0, 10);
      })()
    : sortedLinks(quickLinks)
        .filter((link) => link.default)
        .slice(0, 10);

  const handleMenuClick = (path: string, newTab: boolean = false) => {
    if (path.includes("/")) {
      window.open(path, "_blank");
    } else {
      try {
        eval(path);
      } catch (error) {
        console.error("Error executing function:", error);
      }
    }
  };

  const handleToggleDefault = (linkId: number) => {
    const currentLink =
      newQuickLinks.find(
        (link) => safeNumber(link.id) === safeNumber(linkId)
      ) ??
      initialQuickLinks.find(
        (link) => safeNumber(link.id) === safeNumber(linkId)
      );

    if (!currentLink) return;

    if (!currentLink.default) {
      const currentDefaultCount = newQuickLinks?.filter(
        (link) => link.default
      ).length;

      if (currentDefaultCount >= 10) {
        setErrorMessage("Only maximum of 10 menus are allowed for Preferences");
        setTimeout(() => setErrorMessage(""), 10000);
        return;
      }
    }

    setErrorMessage("");

    const existsInNewQuickLinks = newQuickLinks?.some(
      (link) => safeNumber(link.id) === safeNumber(linkId)
    );

    if (existsInNewQuickLinks) {
      setNewQuickLinks((prevLinks) =>
        prevLinks.map((link) =>
          safeNumber(link.id) === safeNumber(linkId)
            ? { ...link, default: !link.default }
            : link
        )
      );
    } else {
      setNewQuickLinks((prevLinks) => [
        ...prevLinks,
        { ...currentLink, default: true },
      ]);
    }
  };

  const handleSave = async (dateRangeValue?: any) => {
    try {
      const defaultButtonIds = sortByOrder(newQuickLinks)?.filter(
        (link) => link.default
      );
      const payload: any = {
        userPrefrenceDetails: defaultButtonIds,
        preferencedate: 0,
        ...(dateRangeValue && dateRangeValue),
      };

      const response: any = await SaveCustomGridRender(payload);

      if (response?.ResultDtoCreate?.warning === false) {
        setSearchQuery("");
        setIsEditMode(false);
        setErrorMessage("");
        fetchDefaultButtons(initialQuickLinks);
        onApiSuccess?.();
      } else {
        setErrorMessage("Failed to save changes. Please try again.");
        setTimeout(() => setErrorMessage(""), 5000);
      }
    } catch (error) {
      setErrorMessage("Error saving changes. Please try again.");
      setTimeout(() => setErrorMessage(""), 5000);
    }
  };

  const handleClose = () => {
    setIsEditMode(false);
    setErrorMessage("");
    setSearchQuery("");
    setNewQuickLinks(quickLinks);
  };

  const handleDragStart = (
    e: React.DragEvent<HTMLDivElement>,
    link: QuickLink
  ) => {
    setDraggedItem(link);
    e.dataTransfer.effectAllowed = "move";
  };

  const handleDragOver = (
    e: React.DragEvent<HTMLDivElement>,
    link: QuickLink
  ) => {
    e.preventDefault();
    e.dataTransfer.dropEffect = "move";
    setDraggedOverItem(link);
  };

  const handleDrop = (
    e: React.DragEvent<HTMLDivElement>,
    dropTarget: QuickLink
  ) => {
    e.preventDefault();

    if (!draggedItem || draggedItem.id === dropTarget.id) {
      setDraggedItem(null);
      setDraggedOverItem(null);
      return;
    }

    setNewQuickLinks((prevLinks) => {
      const sorted = sortByOrder(prevLinks);

      const draggedIndex = sorted.findIndex(
        (link) => link.id === draggedItem.id
      );
      const targetIndex = sorted.findIndex((link) => link.id === dropTarget.id);

      if (draggedIndex === -1 || targetIndex === -1) return prevLinks;

      const reordered = [...sorted];
      const [movedItem] = reordered.splice(draggedIndex, 1);
      reordered.splice(targetIndex, 0, movedItem);

      return reAssignOrder(reordered);
    });

    setDraggedItem(null);
    setDraggedOverItem(null);
  };

  const handleDragEnd = () => {
    setDraggedItem(null);
    setDraggedOverItem(null);
  };

  return (
    <>
      <Paper
        elevation={0}
        sx={{
          height: "42.5rem",
          width: "400px",
          minWidth: "400px",
          backgroundColor: "#c4c4c4",
          borderRadius: "20px",
          p: 2,
          display: "flex",
          flexDirection: "column",
          gap: 1,
        }}
      >
        <header className="main-header">
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              width: "100%",
            }}
          >
            <header className="main-header">
              <div className="main-quick-title pl-0">Preferences</div>
            </header>
            {!isEditMode ? (
              <Tooltip title="Edit Preferences" placement="top" arrow>
                <Box
                  onClick={() => {
                    setIsEditMode(true);
                    setNewQuickLinks(quickLinks);
                  }}
                  sx={{
                    cursor: "pointer",
                    display: "flex",
                    alignItems: "center",
                    padding: "4px 8px",
                    borderRadius: "4px",
                    transition: "all 0.2s ease",
                    "&:hover": { backgroundColor: "rgba(0, 0, 0, 0.05)" },
                  }}
                >
                  <AiOutlineEdit size={18} />
                </Box>
              </Tooltip>
            ) : (
              <Box sx={{ display: "flex", gap: 1 }}>
                <Tooltip title="Close without saving" placement="top" arrow>
                  <Box onClick={handleClose} sx={{ cursor: "pointer" }}>
                    <IoCloseCircleOutline size={24} color="#535455" />
                  </Box>
                </Tooltip>

                <Tooltip title="Save Preferences" placement="top" arrow>
                  <Box
                    onClick={() => handleSave()}
                    sx={{
                      cursor: "pointer",
                      color: "#28a745",
                      fontWeight: 600,
                      transition: "all 0.2s ease",
                    }}
                  >
                    <IoCheckmarkDoneCircle size={28} color="#28a745" />
                  </Box>
                </Tooltip>
              </Box>
            )}
          </Box>
        </header>

        <div className="d-flex justify-between">
          <div
            className={`quick-links-search w-100 ${
              errorMessage !== "" ? "mb-0" : ""
            }`}
          >
            <AiOutlineSearch className="quick-links-search-icon" />
            <input
              type="text"
              className="quick-links-search-input"
              placeholder="Search links..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          {onAddLink && (
            <div style={{ paddingLeft: "10px", paddingTop: "5px" }}>
              <button className="quick-links-footer-btn" onClick={onAddLink}>
                <AiOutlinePlusCircle size={20} />
              </button>
            </div>
          )}
        </div>

        {errorMessage && (
          <Box
            sx={{
              backgroundColor: "#ffebee",
              color: "#c62828",
              padding: "8px 12px",
              borderRadius: "8px",
              fontSize: "13px",
              fontWeight: 500,
              textAlign: "center",
              border: "1px solid #ef5350",
              animation: "fadeIn 0.3s ease-in",
              "@keyframes fadeIn": {
                from: { opacity: 0, transform: "translateY(-10px)" },
                to: { opacity: 1, transform: "translateY(0)" },
              },
            }}
          >
            {errorMessage}
          </Box>
        )}

        <Box
          sx={{
            background: "var(--treeview_secondaryrow_background) !important",
            borderRadius: "16px",
            padding: "25px 5px",
            flex: 1,
            overflowY: "auto",
          }}
        >
          <Box sx={{ display: "grid", gridTemplateColumns: "repeat(2, 1fr)" }}>
            {filteredLinkList &&
              filteredLinkList?.map((link) => (
                <Box
                  key={link.id}
                  component="button"
                  draggable={isEditMode && link.default}
                  onDragStart={(e: any) => {
                    if (!link.default) return;
                    handleDragStart(e, link);
                  }}
                  onDragOver={(e: any) => {
                    if (!link.default) return;
                    handleDragOver(e, link);
                  }}
                  onDrop={(e: any) => {
                    if (!link.default) return;
                    handleDrop(e, link);
                  }}
                  onDragEnd={() => {
                    if (!link.default) return;
                    handleDragEnd();
                  }}
                  onClick={(event) => {
                    event.preventDefault();
                    const newTab = event.ctrlKey || event.metaKey;
                    !isEditMode &&
                      handleMenuClick(link.path?.replace("-", ""), newTab);
                  }}
                  onKeyDown={(e) => {
                    if (e.key === "Enter" && !isEditMode) {
                      handleMenuClick(link.path?.replace("-", ""));
                    }
                  }}
                  sx={{
                    position: "relative",
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    height: "60px",
                    borderRadius: "20px !important",
                    border:
                      draggedOverItem?.id === link.id && isEditMode
                        ? "2px dashed #e60000 !important"
                        : "none !important",
                    background:
                      draggedItem?.id === link.id && isEditMode
                        ? "#b0b0b0"
                        : "#c4c4c4",
                    margin: "12px",
                    marginBottom: `${errorMessage !== "" ? "16px" : "26px"}`,
                    cursor:
                      isEditMode && link.default
                        ? "move !important"
                        : "pointer",
                    padding: "30px 16px",
                    textDecoration: "none !important",
                    boxShadow: "var(--landing_main_card_shadow) !important",
                    color: "black !important",
                    textAlign: "center !important",
                    fontFamily: "VodafoneRg, Arial, Helvetica, san-serif",
                    fontSize: "15px",
                    fontStyle: "normal",
                    fontWeight: "bolder",
                    opacity:
                      draggedItem?.id === link.id && isEditMode ? 0.5 : 1,
                    transition: "all 0.2s ease",
                  }}
                >
                  {isEditMode && (
                    <Tooltip
                      title={link.default ? "Remove Menu" : "Add Menu"}
                      placement="top"
                      arrow
                    >
                      <Box
                        onClick={(e) => {
                          e.stopPropagation();
                          e.preventDefault();
                          handleToggleDefault(link.id);
                        }}
                        sx={{
                          position: "absolute",
                          top: "-5px",
                          right: "-8px",
                          display: "flex",
                          justifyContent: "center",
                          alignItems: "center",
                          width: link.default ? "22px" : "20px",
                          height: link.default ? "16px" : "20px",
                          borderRadius: link.default ? "4px" : "50%",
                          backgroundColor: link.default
                            ? "#e60000cf"
                            : "#333333a1",
                          cursor: "pointer",
                          transition: "all 0.2s ease",
                          zIndex: 10,
                          "&:hover": { transform: "scale(1.1)", opacity: 0.9 },
                        }}
                      >
                        {link.default ? (
                          <AiOutlineMinus size={12} color="white" />
                        ) : (
                          <MdAdd size={14} color="white" />
                        )}
                      </Box>
                    </Tooltip>
                  )}
                  {isEditMode ? (
                    <div className="tree_menu_items">
                      <p>{link.text}</p>
                    </div>
                  ) : (
                    <a
                      className="tree_menu_items"
                      href={link.path?.replace("-", "")}
                    >
                      <p>{link.text}</p>
                    </a>
                  )}
                </Box>
              ))}
          </Box>
        </Box>
      </Paper>
      <ModalConfirm data={confirm} />
    </>
  );
};

export default QuickLinkNew;
