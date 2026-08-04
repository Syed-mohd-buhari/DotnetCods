import React, { useState, useEffect } from "react";
import { Modal, Button, Tab, Nav } from "react-bootstrap";
import Menu from "../../Components/Menu";

import {
  useNetworkPlans,
  useReports,
  productAndDesignMenus,
} from "../../Containers/landingPage/MenuItem";
import useProductAndDesign from "../../Containers/landingPage/MenuItem";
import { useAdministration } from "../../Containers/landingPage/MenuItem";
import {
  useManageNetworkPlans,
  useManageTransformation,
} from "../../Containers/landingPage/MenuItem";

interface Props {
  show: boolean;
  onClose: () => void;
  userId?: number;
  moduleResources: any[];
  permissionResource?: any[];
  onSavePermissions?: (selectedMenus: any[]) => void;
}

const MenuManagementModal: React.FC<Props> = ({
  show,
  onClose,
  userId,
  moduleResources = [],
  permissionResource = [],
  onSavePermissions,
}) => {
  const [selectedMenus, setSelectedMenus] = useState<string[]>([]);
  const [editMode, setEditMode] = useState(false);
  const [activeTab, setActiveTab] = useState("administration");
  const [openDropdown, setOpenDropdown] = useState<string | null>(null);
  const [permissionMap, setPermissionMap] = useState<Record<string, number>>(
    {}
  );
  const [isManageScreen, setIsManageScreen] = useState(false);
  const [showPermissionFeatures, setShowPermissionFeatures] = useState(false);

  useEffect(() => {
    if (!show) {
      setEditMode(false);
      setIsManageScreen(false);
      return;
    }
    setEditMode(true);
    // if (!isManageScreen) {
    //   setEditMode(true);
    // }

    const isUserPreferences =
      Array.isArray(permissionResource) &&
      permissionResource.length > 0 &&
      permissionResource[0].path !== undefined;

    if (isUserPreferences) {
      const pathsFromPreferences = permissionResource
        .filter((item) => item && item.path)
        .map((item) => item.path);
      const permMap: Record<string, number> = {};
      permissionResource.forEach((item: any) => {
        if (item?.path)
          permMap[item.path] =
            item.screenPermission || item.permissionLevel || 7;
      });
      setPermissionMap(permMap);
      setSelectedMenus(pathsFromPreferences);
      return;
    }

    if (
      Array.isArray(permissionResource) &&
      permissionResource.length > 0 &&
      Array.isArray(moduleResources)
    ) {
      const moduleIdString = permissionResource[0].moduleId;
      const matches = moduleIdString.matchAll(/\((\d+),(\d+)\)/g);

      const selectedPaths: string[] = [];
      const permMap: Record<string, number> = {};

      for (const match of matches) {
        const moduleId = parseInt(match[1]);
        const permissionLevel = parseInt(match[2]);

        const module = moduleResources.find((m: any) => m.key === moduleId);
        if (module) {
          let path = module.modulePath;
          if (path?.startsWith("_")) {
            path = path.substring(1);
          }
          selectedPaths.push(path);
          permMap[path] = permissionLevel;
        }
      }

      setSelectedMenus(selectedPaths);
      setPermissionMap(permMap);
    }
  }, [permissionResource, moduleResources, show]);
  useEffect(() => {
    const handleClickOutside = () => {
      if (openDropdown) {
        setOpenDropdown(null);
      }
    };

    document.addEventListener("click", handleClickOutside);
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  }, [openDropdown]);

  const networkPlans = useNetworkPlans();
  const administration = useAdministration();
  const managenetworkPlans = useManageNetworkPlans();
  const managetranformation = useManageTransformation();
  const reports = useReports();

  const menuTabs = [
    {
      key: "productAndDesignMenus",
      title: "Product & Design",
      menus: productAndDesignMenus,
    },
    {
      key: "networkPlan",
      title: "Network Plan",
      menus: networkPlans,
    },
    {
      key: "managenetworkPlans",
      title: "Manage Network Plan",
      menus: managenetworkPlans,
    },
    {
      key: "managetranformation",
      title: "Manage Transformation",
      menus: managetranformation?.map((res) => ({
        ...res,
        path:
          res.path === "setIsVisibleModalInitializeNewProduct(true)" ||
          res.path === "setIsVisibleModalManage(true)" ||
          res.path === "setIsVisibleModalStatus(true)" ||
          res.path === "setIsVisibleModalProductLifecycle(true)"
            ? `-${res?.path}`
            : res.path,
      })),
    },
    // {
    //   key: "managetranformation",
    //   title: "Manage Transformation",
    //   menus: managetranformation,
    // },
    {
      key: "reports",
      title: "Reports",
      menus: reports,
    },
    {
      key: "administration",
      title: "Administration",
      menus: administration,
    },
  ];

  const getActiveMenus = (menus: any[]) => {
    return menus.filter((menu) => !menu.disable);
  };

  const visibleTabs = menuTabs.filter((tab) => {
    const activeMenus = getActiveMenus(tab.menus);
    return activeMenus.length > 0;
  });
  const handleManageScreen = () => {
    setIsManageScreen(true);
    setEditMode(true);
    setShowPermissionFeatures(true);
  };
  const handleBackScreen = () => {
    setIsManageScreen(false);
    // setEditMode(false);
    setShowPermissionFeatures(false);
  };
  const handleMenuClick = (path: string) => {
    setSelectedMenus((prev) =>
      prev.includes(path) ? prev.filter((p) => p !== path) : [...prev, path]
    );

    setPermissionMap((prev) => {
      if (prev[path]) {
        const updated = { ...prev };
        delete updated[path];
        return updated;
      }
      return { ...prev, [path]: 7 }; // default master
    });
  };

  const handleMenuAdd = (path: string) => {
    setSelectedMenus((prev) =>
      prev.includes(path) ? prev.filter((p) => p !== path) : [...prev, path]
    );

    setPermissionMap((prev) => {
      if (prev[path]) {
        const updated = { ...prev };
        delete updated[path];
        return updated;
      }

      // default = Master
      return {
        ...prev,
        [path]: 7,
      };
    });
  };

  const handleCategorySelect = (categoryMenus: any[]) => {
    const categoryPaths = categoryMenus.map((menu) => menu.path);
    const allSelected = categoryPaths.every((path) =>
      selectedMenus.includes(path)
    );

    if (allSelected) {
      setSelectedMenus((prev) =>
        prev.filter((path) => !categoryPaths.includes(path))
      );

      setPermissionMap((prev) => {
        const updated = { ...prev };
        categoryPaths.forEach((path) => {
          delete updated[path];
        });
        return updated;
      });
    } else {
      const newSelections = categoryPaths.filter(
        (path) => !selectedMenus.includes(path)
      );
      setSelectedMenus((prev) => [...prev, ...newSelections]);

      setPermissionMap((prev) => {
        const updated = { ...prev };
        newSelections.forEach((path) => {
          updated[path] = 7;
        });
        return updated;
      });
    }
  };

  const handleSelectAllInTab = (tabMenus: any[]) => {
    // console.log("tabMenus", tabMenus);
    const tabPaths = tabMenus.map((menu) => menu.path);
    // console.log("allTabMenusSelected", tabPaths, selectedMenus);
    const allTabMenusSelected = tabPaths.every((path) =>
      selectedMenus.includes(path)
    );

    // console.log("allTabMenusSelected", allTabMenusSelected);
    if (allTabMenusSelected) {
      setSelectedMenus((prev) =>
        prev.filter((path) => !tabPaths.includes(path))
      );
    } else {
      const newSelections = tabPaths.filter(
        (path) => !selectedMenus.includes(path)
      );
      setSelectedMenus((prev) => [...prev, ...newSelections]);
    }
  };

  const handleSave = () => {
    let userPrefrenceDetails: any[] = [];
    // console.log("selectedMenus", selectedMenus, moduleResources);
    userPrefrenceDetails = selectedMenus
      .map((path) => {
        const pref = moduleResources?.find((p: any) => p.modulePath === path);
        if (!pref) return;
        return {
          id: pref.key,
          menu: pref.menu,
          order: pref.order ?? 0,
          path: pref.modulePath,
          text: pref.value,
          screenPermission: permissionMap[path] || 7,
        };
      })
      .filter(Boolean);

    if (onSavePermissions) {
      // console.log("selectedMenus", userPrefrenceDetails);
      onSavePermissions(userPrefrenceDetails);
    }

    setEditMode(false);
    onClose();
  };

  const allActiveMenus = [
    ...getActiveMenus(administration),
    ...getActiveMenus(networkPlans),
    ...getActiveMenus(productAndDesignMenus),
    ...getActiveMenus(managenetworkPlans),
    ...getActiveMenus(reports),
    ...getActiveMenus(managetranformation),
  ];

  const setAllSelectedToPermission = (screenPermission: number) => {
    setPermissionMap((prev) => {
      const updated = { ...prev };
      selectedMenus.forEach((path) => {
        updated[path] = screenPermission;
      });
      return updated;
    });
  };

  //end here//

  return (
    <Modal
      show={show}
      backdropClassName="backdropGrid"
      dialogClassName="modal-lg-custom"
      className="modalGrid"
      centered
      keyboard={false}
      size="xl"
      onHide={onClose}
    >
      <Modal.Header className="d-flex justify-content-center" closeButton>
        <div className="d-flex align-items-center w-100 justify-content-between p-2 mr-5">
          <h4 className="modal-title-custom voda-bold">Manage Access</h4>

          <Button
            style={{ minWidth: "6rem", maxWidth: "12rem" }}
            onClick={isManageScreen ? handleBackScreen : handleManageScreen}
            size="sm"
            className="voda-bold btn btn-danger px-4 btnHeader btn btn-primary btn-sm"
          >
            {isManageScreen ? "Back" : "Manage Permission"}
          </Button>
        </div>
      </Modal.Header>

      <Modal.Body
        style={{ height: "70vh", overflow: "hidden" }}
        className="p-0"
      >
        <div className="d-flex h-100">
          <div
            className="flex-grow-1 p-3"
            style={{ overflowY: "auto", overflowX: "hidden" }}
          >
            {visibleTabs.length > 0 && (
              <Tab.Container
                activeKey={activeTab}
                onSelect={(k) => setActiveTab(k || visibleTabs[0].key)}
              >
                <div
                  className="tabs-container mb-3"
                  style={{ display: "flex", justifyContent: "center" }}
                >
                  <Nav variant="tabs" className="compact-tabs">
                    {visibleTabs.map((tab) => {
                      // console.log("Tab", tab);
                      const activeMenus = getActiveMenus(tab.menus);
                      // console.log("activeMenus", activeMenus);
                      const tabPaths = activeMenus.map((menu) => menu.path);
                      // console.log("tabPaths", tabPaths);
                      const allTabMenusSelected = tabPaths.every((path) =>
                        selectedMenus.includes(path)
                      );
                      // console.log("allTabMenusSelected", allTabMenusSelected);

                      return (
                        <Nav.Item
                          key={tab.key}
                          style={{ position: "relative" }}
                          className="p-0"
                        >
                          <Nav.Link eventKey={tab.key} className="p-2">
                            <span className="tab-title">{tab.title}</span>
                            {editMode && (
                              <span
                                className="mx-3"
                                style={{
                                  cursor: "pointer",
                                }}
                                onClick={(e) => {
                                  e.stopPropagation();
                                  e.preventDefault();
                                  setOpenDropdown(
                                    openDropdown === tab.key ? null : tab.key
                                  );
                                }}
                              >
                                ⋮
                                {openDropdown === tab.key && (
                                  <div
                                    style={{
                                      position: "absolute",
                                      top: "100%",
                                      right: 0,
                                      backgroundColor: "white",
                                      border: "1px solid #dee2e6",
                                      borderRadius: "4px",
                                      boxShadow: "0 2px 8px rgba(0,0,0,0.15)",
                                      zIndex: 1000,
                                      minWidth: "150px",
                                      marginTop: "4px",
                                    }}
                                  >
                                    <button
                                      style={{
                                        width: "100%",
                                        padding: "8px 12px",
                                        border: "none",
                                        background: "transparent",
                                        textAlign: "left",
                                        cursor: "pointer",
                                        fontSize: "14px",
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "8px",
                                      }}
                                      onMouseEnter={(e) => {
                                        e.currentTarget.style.backgroundColor =
                                          "#f8f9fa";
                                      }}
                                      onMouseLeave={(e) => {
                                        e.currentTarget.style.backgroundColor =
                                          "transparent";
                                      }}
                                      onClick={(e) => {
                                        e.stopPropagation();
                                        e.preventDefault();
                                        handleSelectAllInTab(activeMenus);
                                        setOpenDropdown(null);
                                      }}
                                    >
                                      <input
                                        type="checkbox"
                                        checked={allTabMenusSelected}
                                        readOnly
                                        style={{ accentColor: "#dc3545" }}
                                      />
                                      {allTabMenusSelected
                                        ? "Unselect All"
                                        : "Select All"}
                                    </button>
                                    <div
                                      style={{
                                        height: "1px",
                                        backgroundColor: "#e9ecef",
                                        margin: "4px 0",
                                      }}
                                    />

                                    {/* Bulk Permission Buttons */}
                                    {showPermissionFeatures && (
                                      <div style={{ padding: "4px 0" }}>
                                        <div
                                          style={{
                                            padding: "4px 12px",
                                            fontSize: "11px",
                                            color: "#6c757d",
                                            fontWeight: 500,
                                          }}
                                        >
                                          Set permission After Select All:
                                        </div>

                                        <button
                                          style={{
                                            width: "100%",
                                            padding: "6px 12px",
                                            border: "none",
                                            background: "transparent",
                                            textAlign: "left",
                                            cursor: "pointer",
                                            fontSize: "13px",
                                            display: "flex",
                                            alignItems: "center",
                                            gap: "8px",
                                            color: "#0d6efd",
                                          }}
                                          onClick={() => {
                                            setPermissionMap((prev) => {
                                              const updated = { ...prev };
                                              activeMenus.forEach((menu) => {
                                                const path = menu.path;
                                                if (
                                                  selectedMenus.includes(path)
                                                ) {
                                                  updated[path] = 3;
                                                }
                                              });
                                              return updated;
                                            });
                                            setOpenDropdown(null);
                                          }}
                                        >
                                          <input
                                            type="checkbox"
                                            checked={
                                              activeMenus.some((menu) =>
                                                selectedMenus.includes(
                                                  menu.path
                                                )
                                              ) &&
                                              activeMenus
                                                .filter((menu) =>
                                                  selectedMenus.includes(
                                                    menu.path
                                                  )
                                                )
                                                .every(
                                                  (menu) =>
                                                    permissionMap[menu.path] ===
                                                    3
                                                )
                                            }
                                            readOnly
                                            style={{ accentColor: "#0d6efd" }}
                                          />
                                          <span style={{ color: "#0d6efd" }}>
                                            R All (Read)
                                          </span>
                                        </button>
                                        <button
                                          style={{
                                            width: "100%",
                                            padding: "6px 12px",
                                            border: "none",
                                            background: "transparent",
                                            textAlign: "left",
                                            cursor: "pointer",
                                            fontSize: "13px",
                                            display: "flex",
                                            alignItems: "center",
                                            gap: "8px",
                                            color: "#198754",
                                          }}
                                          onClick={() => {
                                            setPermissionMap((prev) => {
                                              const updated = { ...prev };
                                              tabPaths.forEach((path) => {
                                                if (
                                                  selectedMenus.includes(path)
                                                ) {
                                                  updated[path] = 4;
                                                }
                                              });
                                              return updated;
                                            });
                                            setOpenDropdown(null);
                                          }}
                                        >
                                          <input
                                            type="checkbox"
                                            checked={
                                              tabPaths.some((path) =>
                                                selectedMenus.includes(path)
                                              ) &&
                                              tabPaths
                                                .filter((path) =>
                                                  selectedMenus.includes(path)
                                                )
                                                .every(
                                                  (path) =>
                                                    permissionMap[path] === 4
                                                )
                                            }
                                            readOnly
                                            style={{ accentColor: "#198754" }}
                                          />
                                          <span style={{ color: "#198754" }}>
                                            W All (Write)
                                          </span>
                                        </button>
                                        <button
                                          style={{
                                            width: "100%",
                                            padding: "6px 12px",
                                            border: "none",
                                            background: "transparent",
                                            textAlign: "left",
                                            cursor: "pointer",
                                            fontSize: "13px",
                                            display: "flex",
                                            alignItems: "center",
                                            gap: "8px",
                                            color: "#dc3545",
                                          }}
                                          onClick={() => {
                                            setPermissionMap((prev) => {
                                              const updated = { ...prev };
                                              tabPaths.forEach((path) => {
                                                if (
                                                  selectedMenus.includes(path)
                                                ) {
                                                  updated[path] = 7;
                                                }
                                              });
                                              return updated;
                                            });
                                            setOpenDropdown(null);
                                          }}
                                        >
                                          <input
                                            type="checkbox"
                                            checked={
                                              tabPaths.some((path) =>
                                                selectedMenus.includes(path)
                                              ) &&
                                              tabPaths
                                                .filter((path) =>
                                                  selectedMenus.includes(path)
                                                )
                                                .every(
                                                  (path) =>
                                                    permissionMap[path] === 7
                                                )
                                            }
                                            readOnly
                                            style={{ accentColor: "#dc3545" }}
                                          />
                                          <span style={{ color: "#dc3545" }}>
                                            M All (Master)
                                          </span>
                                        </button>
                                      </div>
                                    )}
                                  </div>
                                )}
                              </span>
                            )}
                          </Nav.Link>
                        </Nav.Item>
                      );
                    })}
                  </Nav>
                </div>

                <Tab.Content>
                  {visibleTabs.map((tab) => {
                    const tabActiveMenus = getActiveMenus(tab.menus);
                    const tabGroupedMenus = tabActiveMenus.reduce(
                      (acc, item) => {
                        if (!acc[item.category]) {
                          acc[item.category] = [];
                        }
                        acc[item.category].push(item);
                        return acc;
                      },
                      {} as Record<string, typeof tabActiveMenus>
                    );

                    const tabPaths = tabActiveMenus.map((menu) => menu.path);
                    const allTabMenusSelected = tabPaths.every((path) =>
                      selectedMenus.includes(path)
                    );

                    return (
                      <Tab.Pane key={tab.key} eventKey={tab.key}>
                        {/* {editMode && selectedMenus.length > 0 && (
                          <div
                            className="mb-3 d-flex gap-2 align-items-center"
                            style={{
                              borderBottom: "1px solid #e9ecef",
                              paddingBottom: "8px",
                            }}
                          >
                            <span style={{ fontSize: "12px", fontWeight: 500 }}>
                              Bulk permissions:
                            </span>
                            <button
                              onClick={() => setAllSelectedToPermission(3)}
                              className="btn btn-sm"
                              style={{
                                backgroundColor: "#0d6efd",
                                color: "white",
                                border: "none",
                                padding: "4px 12px",
                                fontSize: "12px",
                                borderRadius: "4px",
                              }}
                            >
                              👁️ Read (3)
                            </button>
                            <button
                              onClick={() => setAllSelectedToPermission(4)}
                              className="btn btn-sm"
                              style={{
                                backgroundColor: "#198754",
                                color: "white",
                                border: "none",
                                padding: "4px 12px",
                                fontSize: "12px",
                                borderRadius: "4px",
                              }}
                            >
                              ✏️ Write (4)
                            </button>
                            <button
                              onClick={() => setAllSelectedToPermission(7)}
                              className="btn btn-sm"
                              style={{
                                backgroundColor: "#dc3545",
                                color: "white",
                                border: "none",
                                padding: "4px 12px",
                                fontSize: "12px",
                                borderRadius: "4px",
                              }}
                            >
                              🔧 Master (7)
                            </button>
                          </div>
                        )} */}
                        <div style={{ maxWidth: "100%", overflowX: "hidden" }}>
                          <Menu
                            groupedMenus={tabGroupedMenus}
                            handleMenuClick={handleMenuClick}
                            editMode={editMode}
                            selectedMenus={selectedMenus}
                            onMenuAdd={handleMenuAdd}
                            onCategorySelect={handleCategorySelect}
                            permissionMap={permissionMap}
                            setPermissionMap={setPermissionMap}
                            showPermissionFeatures={showPermissionFeatures}
                          />
                        </div>
                      </Tab.Pane>
                    );
                  })}
                </Tab.Content>
              </Tab.Container>
            )}
          </div>
        </div>
      </Modal.Body>

      <Modal.Footer
        style={{ display: "flex", justifyContent: "flex-end", gap: "10px" }}
      >
        <Button
          onClick={onClose}
          size="sm"
          className="voda-bold btn btn-link px-4 btnHeader cancel"
        >
          Cancel
        </Button>
        {editMode && (
          <Button
            onClick={handleSave}
            size="sm"
            className="voda-bold btn btn-danger px-4 btnHeader"
          >
            Save
          </Button>
        )}
      </Modal.Footer>

      <style>{`
        .modal-lg-custom {
    max-width: 95vw !important;
    width: 95vw !important;
  }
 
  .modalGrid .modal-dialog {
    max-width: 95vw !important;
  }
      .circular-badge {
  width: 40px;
  height: 40px;
  background-color: #dc3545;
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  font-size: 1.2rem;
  box-shadow: 0 2px 8px rgba(220, 53, 69, 0.4);
  animation: pulse 2s infinite;
}
 
@keyframes pulse {
  0% {
    box-shadow: 0 0 0 0 rgba(220, 53, 69, 0.7);
  }
  70% {
    box-shadow: 0 0 0 10px rgba(220, 53, 69, 0);
  }
  100% {
    box-shadow: 0 0 0 0 rgba(220, 53, 69, 0);
  }
}
       
       
       
        .selected-menus-sidebar {
          background-color: #f8f9fa;
          height: 100%;
          width: 280px;
        }
       
        .selected-menu-item {
          background-color: white;
          transition: all 0.2s;
          cursor: default;
        }
       
        .selected-menu-item:hover {
          background-color: #fff3f3;
          border-color: #dc3545 !important;
        }
       
       
       .compact-tabs {
  border-bottom: 1px solid #dee2e6;
  flex-wrap: wrap;
  gap: 2px;
  justify-content: center !important;
  display: flex !important;
}
 
.compact-tabs .nav-link {
  border: 1px solid transparent;
  border-bottom: none;
  background: transparent;
  color: #010911ff;
  font-weight: 900;
  font-size: 1.3rem;
  padding: 0.9rem 2rem;
  transition: all 0.2s ease;
  border-radius: 4px 4px 0 0;
  white-space: nowrap;
}
       
        .compact-tabs .nav-link:hover {
          border-color: #e9ecef #e9ecef #dee2e6;
          background: #f8f9fa;
        }
       
        .compact-tabs .nav-link.active {
          color: #dc3545;
          background: #fff;
          border-color: #dee2e6 #dee2e6 #fff;
          font-weight: 800;
           border-bottom: 3px solid #dc3545 !important;
  position: relative;
        }
       
        .tab-badge {
          background-color: #e9ecef;
          color: #495057;
          padding: 0.15rem 0.5rem;
          border-radius: 12px;
          font-size: 0.75rem;
          font-weight: 500;
          display: inline-block;
        }
       
        .nav-link.active .tab-badge {
          background-color: #dc3545;
          color: white;
        }
       
        /* Menu Container */
        .menu-container {
          max-width: 100%;
          overflow-x: hidden;
        }
       
        /* Override Menu component styles to prevent horizontal scroll */
        :global(.menu-items-container) {
          flex-wrap: wrap !important;
          max-width: 100% !important;
        }
       
        :global(.category-column) {
          max-width: 100% !important;
        }
       
        :global(.menu-item-inline) {
          max-width: 100% !important;
        }
       
        /* Badges */
        .badge.bg-primary {
          background-color: #dc3545 !important;
        }
       
        /* Scrollbar Styling */
        ::-webkit-scrollbar {
          width: 6px;
        }
       
        ::-webkit-scrollbar-track {
          background: #f1f1f1;
        }
       
        ::-webkit-scrollbar-thumb {
          background: #888;
          border-radius: 3px;
        }
       
        ::-webkit-scrollbar-thumb:hover {
          background: #555;
        }
      `}</style>
    </Modal>
  );
};

export default MenuManagementModal;
