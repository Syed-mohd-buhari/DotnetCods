import React, { useState, useEffect } from "react";
import { Tab, Nav, Button } from "react-bootstrap";
import Menu from "../../Components/Menu";

import {
  useNetworkPlans,
  useReports,
  productAndDesignMenus,
} from "../../Containers/landingPage/MenuItem";
import { useAdministration } from "../../Containers/landingPage/MenuItem";
import {
  useManageNetworkPlans,
  useManageTransformation,
} from "../../Containers/landingPage/MenuItem";

interface Props {
  userId?: number;
  permissionResource?: any[];
  moduleResources?: any[];
  onMenuChange?: (menus: string) => void;
}

const RoleMenuManagement: React.FC<Props> = ({
  permissionResource = [],
  moduleResources,
  onMenuChange,
}) => {
  const [selectedMenus, setSelectedMenus] = useState<string[]>([]);
  const [permissionMap, setPermissionMap] = useState<Record<string, number>>(
    {}
  );
  const [activeTab, setActiveTab] = useState("administration");
  const [editMode] = useState(true);
  const [openDropdown, setOpenDropdown] = useState<string | null>(null);
  const [showPermissionFeatures, setShowPermissionFeatures] = useState(false);

  useEffect(() => {
    if (permissionResource && permissionResource.length > 0) {
      const selectedPaths = permissionResource
        .map((item: any) => item.path)
        .filter(Boolean);

      const permMap: Record<string, number> = {};
      permissionResource.forEach((item: any) => {
        if (item?.path) {
          permMap[item.path] =
            item.screenPermission || item.permissionLevel || 7;
        }
      });

      setSelectedMenus(selectedPaths);
      setPermissionMap(permMap);

      console.log("Initial selected menus:", selectedPaths);
      console.log("Initial permission map:", permMap);
    }
  }, [permissionResource]);

  useEffect(() => {
    sendModuleIdsWithPermissions(selectedMenus, permissionMap);
  }, [selectedMenus, permissionMap]);

  useEffect(() => {
    const handleClickOutside = () => {
      if (openDropdown) setOpenDropdown(null);
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
      title: "Manage Network",
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
    setShowPermissionFeatures(true);
  };

  const handleBackScreen = () => {
    setShowPermissionFeatures(false);
  };

  const setCategoryPermission = (categoryMenus: any[], level: number) => {
    setPermissionMap((prev) => {
      const updated = { ...prev };
      categoryMenus.forEach((menu) => {
        if (selectedMenus.includes(menu.path)) {
          updated[menu.path] = level;
        }
      });
      return updated;
    });
  };

  /**
   * Send moduleIds with permissions
   */
  const sendModuleIdsWithPermissions = (
    menus: string[],
    permMap: Record<string, number>
  ) => {
    if (!moduleResources || !onMenuChange) return;

    const moduleIdsWithPerms = menus
      .map((path) => {
        const module = moduleResources.find((m: any) => m.modulePath === path);
        const permissionLevel = permMap[path] || 7;
        return module ? `(${module.key},${permissionLevel})` : null;
      })
      .filter((item) => item !== null);

    const moduleIdString = moduleIdsWithPerms.join(",");

    console.log("Module ID String with Permissions:", moduleIdString);

    onMenuChange(moduleIdString);
  };

  const handleMenuAdd = (path: string) => {
    let updatedMenus: string[];
    let updatedPermMap = { ...permissionMap };

    if (selectedMenus.includes(path)) {
      updatedMenus = selectedMenus.filter((p) => p !== path);
      delete updatedPermMap[path];
    } else {
      updatedMenus = [...selectedMenus, path];
      updatedPermMap[path] = 7; // Default to Master
    }

    setSelectedMenus(updatedMenus);
    setPermissionMap(updatedPermMap);
  };

  const handlePermissionChange = (path: string, level: number) => {
    setPermissionMap((prev) => {
      if (prev[path] === level) {
        const updated = { ...prev };
        delete updated[path];
        return updated;
      }
      return { ...prev, [path]: level };
    });
  };

  const handleCategorySelect = (categoryMenus: any[]) => {
    const categoryPaths = categoryMenus.map((menu) => menu.path);
    const allSelected = categoryPaths.every((path) =>
      selectedMenus.includes(path)
    );

    let updatedMenus: string[];
    let updatedPermMap = { ...permissionMap };

    if (allSelected) {
      updatedMenus = selectedMenus.filter(
        (path) => !categoryPaths.includes(path)
      );
      categoryPaths.forEach((path) => {
        delete updatedPermMap[path];
      });
    } else {
      const newSelections = categoryPaths.filter(
        (path) => !selectedMenus.includes(path)
      );
      updatedMenus = [...selectedMenus, ...newSelections];
      newSelections.forEach((path) => {
        updatedPermMap[path] = 7; // Default to Master
      });
    }

    setSelectedMenus(updatedMenus);
    setPermissionMap(updatedPermMap);
  };

  const handleSelectAllInTab = (tabMenus: any[]) => {
    const tabPaths = tabMenus.map((menu) => menu.path);
    const allSelected = tabPaths.every((path) => selectedMenus.includes(path));

    let updatedMenus: string[];
    let updatedPermMap = { ...permissionMap };

    if (allSelected) {
      updatedMenus = selectedMenus.filter((path) => !tabPaths.includes(path));
      tabPaths.forEach((path) => {
        delete updatedPermMap[path];
      });
    } else {
      const newSelections = tabPaths.filter(
        (path) => !selectedMenus.includes(path)
      );
      updatedMenus = [...selectedMenus, ...newSelections];
      newSelections.forEach((path) => {
        updatedPermMap[path] = 7; // Default to Master
      });
    }

    setSelectedMenus(updatedMenus);
    setPermissionMap(updatedPermMap);
  };

  const setAllSelectedToPermission = (level: number) => {
    setPermissionMap((prev) => {
      const updated = { ...prev };
      selectedMenus.forEach((path) => {
        updated[path] = level;
      });
      return updated;
    });
  };

  return (
    <div className="role-menu-management-container">
      <div className="d-flex justify-content-between align-items-center mt-5 px-3 pt-2">
        <Button
          onClick={
            showPermissionFeatures ? handleBackScreen : handleManageScreen
          }
          size="sm"
          className="voda-bold btn btn-danger px-4"
        >
          {showPermissionFeatures ? "Back" : "Manage Permission"}
        </Button>
      </div>
      <div style={{ height: "70vh", overflow: "hidden" }} className="p-0">
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
                  style={{ display: "flex", justifyContent: "left" }}
                >
                  <Nav variant="tabs" className="compact-tabs">
                    {visibleTabs.map((tab) => {
                      const activeMenus = getActiveMenus(tab.menus);
                      const tabPaths = activeMenus.map((menu) => menu.path);
                      const allTabMenusSelected = tabPaths.every((path) =>
                        selectedMenus.includes(path)
                      );

                      return (
                        <Nav.Item
                          key={tab.key}
                          style={{ position: "relative" }}
                          className="p-0"
                        >
                          <Nav.Link eventKey={tab.key} className="p-2">
                            {tab.title}
                            {editMode && (
                              <span
                                className="mx-3"
                                style={{ cursor: "pointer" }}
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
                                      minWidth: "180px",
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
                                        borderBottom: "1px solid #e9ecef",
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

                    return (
                      <Tab.Pane key={tab.key} eventKey={tab.key}>
                        <Menu
                          groupedMenus={tabGroupedMenus}
                          editMode={editMode}
                          selectedMenus={selectedMenus}
                          onMenuAdd={handleMenuAdd}
                          onCategorySelect={handleCategorySelect}
                          handleMenuClick={() => {}}
                          permissionMap={permissionMap}
                          setPermissionMap={setPermissionMap}
                          showPermissionFeatures={showPermissionFeatures}
                        />
                      </Tab.Pane>
                    );
                  })}
                </Tab.Content>
              </Tab.Container>
            )}
          </div>
        </div>
      </div>
      <style>{`
        .modal-lg-custom {
          max-width: 95vw !important;
          width: 95vw !important;
        }
        .modalGrid .modal-dialog {
          max-width: 95vw !important;
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
      `}</style>
    </div>
  );
};

export default RoleMenuManagement;
