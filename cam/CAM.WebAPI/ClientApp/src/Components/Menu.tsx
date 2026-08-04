import React from "react";
import { Container } from "react-bootstrap";

interface MenuItem {
  title: string;
  path: string;
  category?: string;
  disable?: boolean;
  bracket?: string;
}

interface MenuProps {
  groupedMenus: Record<string, MenuItem[]>;
  handleMenuClick: (path: string, newTab?: boolean) => void;
  editMode?: boolean;
  selectedMenus?: string[];
  onMenuAdd?: (path: string) => void;
  onCategorySelect?: (categoryMenus: MenuItem[]) => void;
  permissionMap?: Record<string, number>;
  setPermissionMap?: React.Dispatch<
    React.SetStateAction<Record<string, number>>
  >;
  showPermissionFeatures?: boolean;
}

const MAX_PER_ROW = 5;

const Menu: React.FC<MenuProps> = ({
  groupedMenus,
  handleMenuClick,
  editMode = false,
  selectedMenus = [],
  onMenuAdd,
  onCategorySelect,
  permissionMap = {},
  setPermissionMap,
  showPermissionFeatures = false,
}) => {
  type CatWithMenus = { category: string; menus: MenuItem[] };
  let mergedRows: {
    categoriesWithMenus: CatWithMenus[];
    totalMenus: number;
  }[] = [];

  const categoryEntries = Object.entries(groupedMenus);

  for (let i = 0; i < categoryEntries.length; i++) {
    const [category, menus] = categoryEntries[i];
    const activeMenus = menus.filter((m) => !m.disable);
    if (activeMenus.length === 0) continue;

    if (activeMenus.length > 4) {
      mergedRows.push({
        categoriesWithMenus: [{ category, menus: activeMenus }],
        totalMenus: activeMenus.length,
      });
      continue;
    }

    const lastRow = mergedRows[mergedRows.length - 1];

    if (lastRow && lastRow.totalMenus + activeMenus.length <= MAX_PER_ROW) {
      lastRow.categoriesWithMenus.push({ category, menus: activeMenus });
      lastRow.totalMenus += activeMenus.length;
    } else {
      mergedRows.push({
        categoriesWithMenus: [{ category, menus: activeMenus }],
        totalMenus: activeMenus.length,
      });
    }
  }

  const chunkMenus = (menus: MenuItem[]) => {
    const chunks: MenuItem[][] = [];
    for (let i = 0; i < menus.length; i += MAX_PER_ROW) {
      chunks.push(menus.slice(i, i + MAX_PER_ROW));
    }
    return chunks;
  };

  const setCategoryPermission = (categoryMenus: MenuItem[], level: number) => {
    if (!setPermissionMap) return;

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

  return (
    <div data-nameid="MainContent-Wireframe" className="main_menus">
      <Container>
        <div className="mb-4" />

        {mergedRows.map((row, rowIndex) => (
          <React.Fragment key={rowIndex}>
            <div className="merged-row">
              {row.categoriesWithMenus.map((cw, ci) => {
                const hasSelected = cw.menus.some((menu) =>
                  selectedMenus.includes(menu.path)
                );

                return (
                  <div className="category-column" key={ci}>
                    <div
                      className="category-title"
                      style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "10px",
                      }}
                    >
                      {editMode && onCategorySelect && (
                        <input
                          type="checkbox"
                          checked={cw.menus.every((menu) =>
                            selectedMenus.includes(menu.path)
                          )}
                          onChange={(e) => {
                            e.stopPropagation();
                            onCategorySelect(cw.menus);
                          }}
                          style={{
                            width: "18px",
                            height: "18px",
                            cursor: "pointer",
                            accentColor: "#dc3545",
                          }}
                          title={
                            cw.menus.every((menu) =>
                              selectedMenus.includes(menu.path)
                            )
                              ? `Unselect all in ${cw.category}`
                              : `Select all in ${cw.category}`
                          }
                        />
                      )}
                      <strong className="category-label">{cw.category}</strong>

                      {editMode &&
                        setPermissionMap &&
                        showPermissionFeatures &&
                        hasSelected && (
                          <div
                            style={{
                              display: "flex",
                              gap: "4px",
                              marginLeft: "8px",
                            }}
                          >
                            <button
                              onClick={() => setCategoryPermission(cw.menus, 3)}
                              style={{
                                fontSize: "10px",
                                padding: "2px 6px",
                                borderRadius: "3px",
                                border: "1px solid #0d6efd",
                                backgroundColor: "#fff",
                                color: "#0d6efd",
                                cursor: "pointer",
                              }}
                              title="Set all selected to Read"
                            >
                              R All
                            </button>
                            <button
                              onClick={() => setCategoryPermission(cw.menus, 4)}
                              style={{
                                fontSize: "10px",
                                padding: "2px 6px",
                                borderRadius: "3px",
                                border: "1px solid #198754",
                                backgroundColor: "#fff",
                                color: "#198754",
                                cursor: "pointer",
                              }}
                              title="Set all selected to Write"
                            >
                              W All
                            </button>
                            <button
                              onClick={() => setCategoryPermission(cw.menus, 7)}
                              style={{
                                fontSize: "10px",
                                padding: "2px 6px",
                                borderRadius: "3px",
                                border: "1px solid #dc3545",
                                backgroundColor: "#fff",
                                color: "#dc3545",
                                cursor: "pointer",
                              }}
                              title="Set all selected to Master"
                            >
                              M All
                            </button>
                          </div>
                        )}
                    </div>

                    {chunkMenus(cw.menus).map((menuRow, rIndex) => (
                      <div
                        key={rIndex}
                        className="category-menus"
                        role="submenu"
                        style={{
                          display: "flex",
                          gap: "12px",
                          marginBottom: "6px",
                        }}
                      >
                        {menuRow.map((menu, mi) => {
                          const isSelected = selectedMenus.includes(menu.path);
                          const currentPermission = permissionMap?.[menu.path];

                          return (
                            <div
                              key={mi}
                              className="menu-item-inline"
                              style={{ position: "relative" }}
                            >
                              {editMode &&
                                onMenuAdd &&
                                !showPermissionFeatures && (
                                  <input
                                    type="checkbox"
                                    checked={selectedMenus.includes(menu.path)}
                                    onChange={(e) => {
                                      e.stopPropagation();
                                      onMenuAdd(menu.path);
                                    }}
                                    style={{
                                      position: "absolute",
                                      top: "4px",
                                      right: "4px",
                                      width: "18px",
                                      height: "18px",
                                      cursor: "pointer",
                                      zIndex: 11,
                                      accentColor: "#dc3545",
                                    }}
                                  />
                                )}
                              <a
                                tabIndex={2}
                                className="menu_items"
                                href={menu.path}
                                onClick={(e) => {
                                  e.preventDefault();
                                  handleMenuClick(
                                    menu.path,
                                    e.ctrlKey || e.metaKey
                                  );
                                }}
                              >
                                <p className="menu-title">{menu.title}</p>
                                {menu.bracket && (
                                  <p className="menu-bracket">
                                    ({menu.bracket})
                                  </p>
                                )}
                              </a>
                              {editMode && showPermissionFeatures && (
                                <div
                                  style={{
                                    position: "absolute",
                                    top: "80px",
                                    right: "4px",
                                    display: "flex",
                                    gap: "6px",
                                    background: "#fff",
                                    padding: "2px 4px",
                                    borderRadius: "4px",
                                    zIndex: 10,
                                  }}
                                  onClick={(e) => e.stopPropagation()}
                                >
                                  {[
                                    { label: "R", value: 3, title: "Read" },
                                    { label: "W", value: 4, title: "Write" },
                                    { label: "M", value: 7, title: "Master" },
                                  ].map((perm) => (
                                    <label
                                      key={perm.value}
                                      style={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: "2px",
                                        fontSize: "10px",
                                        cursor: "pointer",
                                        opacity: isSelected ? 1 : 0.5,
                                      }}
                                      title={perm.title}
                                    >
                                      <input
                                        type="checkbox"
                                        checked={
                                          isSelected &&
                                          currentPermission === perm.value
                                        }
                                        onChange={() => {
                                          if (!isSelected && onMenuAdd) {
                                            onMenuAdd(menu.path);

                                            Promise.resolve().then(() => {
                                              if (setPermissionMap) {
                                                setPermissionMap((prev) => ({
                                                  ...prev,
                                                  [menu.path]: perm.value,
                                                }));
                                              }
                                            });
                                          } else if (
                                            setPermissionMap &&
                                            isSelected
                                          ) {
                                            setPermissionMap((prev) => {
                                              if (
                                                prev[menu.path] === perm.value
                                              ) {
                                                const updated = { ...prev };
                                                delete updated[menu.path];
                                                if (onMenuAdd) {
                                                  onMenuAdd(menu.path);
                                                }
                                                return updated;
                                              }
                                              return {
                                                ...prev,
                                                [menu.path]: perm.value,
                                              };
                                            });
                                          }
                                        }}
                                        style={{
                                          cursor: "pointer",
                                          accentColor: "#dc3545",
                                        }}
                                      />
                                      {perm.label}
                                    </label>
                                  ))}
                                </div>
                              )}
                            </div>
                          );
                        })}
                      </div>
                    ))}
                  </div>
                );
              })}
            </div>

            <div className="merged-row-spacer" />
          </React.Fragment>
        ))}
      </Container>
    </div>
  );
};

export default Menu;
//working now
