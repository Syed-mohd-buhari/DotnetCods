import React, { useState } from "react";
import { DropdownButton, Nav, Navbar } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import "../Css/index.css";
import "../Css/NavBar.css";
import { rtnVersionApp } from "../Hook/Common";
import { useAuth } from "../Hook/useAuth";
import { DataModalConfirm, stateConfirm } from "../Model/Common";
import { ResetForeignIndex } from "../Redux/Action/ForeignIndex/ForeignIndexCommonAction";
import ModalConfirm from "./ModalConfirm";

interface Props {
  action: {
    logout(): any;
    setIsVisibleModalManage(val: boolean): any;
    setIsVisibleModalInitializeNewProduct(val: boolean): any;
    setIsVisibleModalProductLifecycle(val: boolean): any;
    setIsVisibleModalStatus(val: boolean): any;
  };
}

const NavBarHome: React.FC<Props> = (props) => {
  const navigate = useNavigate();
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const {
    tipologicaPermesso,
    VerifyIsInRole,
    KPIAdmin,
    KPIEditor,
    admin,
    simpleUser,
    role,
  } = useAuth();

  const CancelConfirm = () => {
    setConfirm(stateConfirm);
  };

  const LogOutConfirm = () => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to quit? Unsaved changes will be lost.",
      button: "Logout",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => {
          props.action.logout();
        },
      },
    });
  };

  const ResetForeignIndexConfirm = () => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to reset Refactor Foreign Index Session?",
      button: "Reset",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => CancelConfirm(),
        confirm: () => ConfirmReset(),
      },
    });
  };

  const ConfirmReset = async () => {
    await ResetForeignIndex().then((x) => {
      setConfirm(stateConfirm);
    });
  };

  const [openedMenu, setOpenedMenu] = useState<string[]>([]);

  const Toggle = (property: string, isInsideMenu: boolean = false) => {
    let copy = [...openedMenu] as string[];

    if (openedMenu.includes(property)) {
      const idx = copy.indexOf(property);
      copy.splice(idx, 1);
    } else {
      if (isInsideMenu) {
        if (!openedMenu.includes(property)) {
          copy.push(property);
        }
      } else {
        copy = [property];
      }
    }

    setOpenedMenu(copy);
  };

  return (
    <div className="container-full navBarContainer px-2">
      <Navbar expand="lg">
        <div>
          <Navbar.Brand className="mr-2 ml-2" style={{ color: "#fff" }}>
            <img
              onClick={(): void => navigate("/")}
              alt="Cam"
              title="Home-Cam"
              className="logoBrand pb-1 pointer"
              src={require("../img/logoCAM_W.png")}
            />
          </Navbar.Brand>
          <Navbar.Toggle
            className="navbar-dark ml-2"
            aria-controls="basic-navbar-nav"
          />
        </div>
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="">
            {simpleUser || admin ? (
              <>
                <DropdownButton
                  show={openedMenu.includes("library")}
                  onClick={() => Toggle("library")}
                  navbar={true}
                  bsPrefix=" btn text-light myButtonNav "
                  variant="danger"
                  className="px-1 navLink  d-flex align-items-center"
                  title="Product Library"
                  id="library"
                >
                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement"
                    to={{ pathname: "/majorhardware" }}
                  >
                    Major HW Build
                  </Link>

                  <Link
                    className=" py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Third Party HW Component
                  </Link>

                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/majorsoftware" }}
                  >
                    Major SW Build
                  </Link>

                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Third Party SW Component
                  </Link>

                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Hardware Component (As-Is)
                  </Link>

                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Software Component (As-Is)
                  </Link>

                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Features
                  </Link>

                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Value Packs
                  </Link>
                </DropdownButton>

                <DropdownButton
                  show={openedMenu.includes("design")}
                  onClick={() => Toggle("design")}
                  navbar={true}
                  bsPrefix=" btn text-light myButtonNav "
                  variant="danger"
                  className="px-1 navLink  d-flex align-items-center"
                  title="Design"
                  id="design"
                >
                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement"
                    to={{ pathname: "/designcomponent" }}
                  >
                    Design Component
                  </Link>
                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement"
                    to={{ pathname: "/designcomponentfamily" }}
                  >
                    Design Component Family
                  </Link>

                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/systemtype" }}
                  >
                    System Type
                  </Link>
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/systemverificationproblems" }}
                  >
                    System Verification Problems
                  </Link>
                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Design Aspects
                  </Link>
                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Virtual Routing Networks
                  </Link>
                  <Link
                    className="py-2 px-4  dropdown-item myNavDropDownElement disabled"
                    to={{ pathname: "/" }}
                  >
                    Identities As-Is
                  </Link>
                </DropdownButton>

                <DropdownButton
                  show={openedMenu.includes("Lcm")}
                  onClick={() => Toggle("Lcm")}
                  navbar={true}
                  bsPrefix=" btn text-light myButtonNav "
                  variant="danger"
                  className="px-1 navLink  d-flex align-items-center"
                  title="Planning"
                  id="Lcm"
                >
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/lcmengineering" }}
                  >
                    Lcm Engineering
                  </Link>

                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/asplanned" }}
                  >
                    Assets
                  </Link>

                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/plannedActivities" }}
                  >
                    Planned Activities
                  </Link>
                </DropdownButton>

                <DropdownButton
                  show={openedMenu.includes("LCM Utility")}
                  onClick={() => Toggle("LCM Utility")}
                  navbar={true}
                  bsPrefix=" btn text-light myButtonNav "
                  variant="danger"
                  className="px-1 navLink  d-flex align-items-center"
                  title="LCM Utility"
                  id="utility"
                >
                  <button
                    className="py-2 px-4 dropdown-item  myNavDropDownElement"
                    style={{ borderRadius: 0 }}
                    onClick={() => props.action.setIsVisibleModalManage(true)}
                  >
                    Manage Migrations
                  </button>

                  <button
                    className="py-2 px-4 dropdown-item  myNavDropDownElement"
                    style={{ borderRadius: 0 }}
                    onClick={() => props.action.setIsVisibleModalStatus(true)}
                  >
                    Update Planned Activity Status
                  </button>
                </DropdownButton>

                <DropdownButton
                  show={openedMenu.includes("deployment")}
                  onClick={() => Toggle("deployment")}
                  navbar={true}
                  bsPrefix=" btn text-light myButtonNav "
                  variant="danger"
                  className="px-1 navLink  d-flex align-items-center"
                  title="Deployment"
                  id="deployment"
                >
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/asis" }}
                  >
                    Network Element As-Is
                  </Link>
                </DropdownButton>

                <DropdownButton
                  show={openedMenu.includes("Product Utilities")}
                  onClick={() => Toggle("Product Utilities")}
                  navbar={true}
                  bsPrefix=" btn text-light myButtonNav "
                  variant="danger"
                  className="px-1 navLink  d-flex align-items-center"
                  title="Product Utilities"
                  id="utility"
                >
                  <button
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    style={{ borderRadius: 0 }}
                    onClick={() =>
                      props.action.setIsVisibleModalInitializeNewProduct(true)
                    }
                  >
                    Initialize New Product
                  </button>
                  <button
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    style={{ borderRadius: 0 }}
                    onClick={() =>
                      props.action.setIsVisibleModalProductLifecycle(true)
                    }
                  >
                    Product Lifecycle Constraints
                  </button>
                </DropdownButton>

                <DropdownButton
                  show={openedMenu.includes("new")}
                  onClick={() => Toggle("new")}
                  navbar={true}
                  bsPrefix=" btn text-light myButtonNav "
                  variant="danger"
                  className="px-1 navLink  d-flex align-items-center"
                  title="Virtualization Bundles"
                  id="new"
                >
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/bundleupgradeinitiative" }}
                  >
                    Bundle Upgrade Initiative
                  </Link>
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/nfvi" }}
                  >
                    NFVI (Transition)
                  </Link>
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/vnf" }}
                  >
                    VNF (Transition)
                  </Link>
                </DropdownButton>

                {tipologicaPermesso && (
                  <DropdownButton
                    show={openedMenu.includes("MainAdmin")}
                    onClick={() => Toggle("MainAdmin")}
                    navbar={true}
                    bsPrefix=" btn text-light myButtonNav "
                    variant="danger"
                    className="px-1 navLink  d-flex align-items-center"
                    title="Administration"
                    id="admin"
                  >
                    <DropdownButton
                      onClick={(e) => {
                        e.stopPropagation();
                      }}
                      navbar={true}
                      bsPrefix=" btn text-dark navLink myButtonNav "
                      variant="danger"
                      className="px-1 navLink  d-flex align-items-center"
                      title="Roles"
                      id="admin"
                    >
                      <Link
                        onClick={() => {
                          Toggle("MainAdmin");
                        }}
                        className="py-2 px-4 dropdown-item myNavDropDownElement"
                        to={{ pathname: "/settingsupdateplannedactivity" }}
                        style={{ borderRadius: 0 }}
                      >
                        Settings Update Planned Activity
                      </Link>
                    </DropdownButton>

                    <DropdownButton
                      onClick={(e) => {
                        e.stopPropagation();
                      }}
                      navbar={true}
                      bsPrefix=" btn text-dark navLink myButtonNav "
                      variant="danger"
                      className="py-2 px-4 dropdown-item myNavDropDownElement"
                      title="Refactor"
                      id="refactor"
                    >
                      <button
                        className="py-2 px-4 dropdown-item myNavDropDownElement"
                        style={{ borderRadius: 0 }}
                        onClick={() => ResetForeignIndexConfirm()}
                      >
                        Reset Foreign Index Session
                      </button>
                    </DropdownButton>
                  </DropdownButton>
                )}
              </>
            ) : null}

            {(KPIAdmin || KPIEditor) && (
              <DropdownButton
                show={openedMenu.includes("VoLTE KPI")}
                onClick={() => Toggle("VoLTE KPI")}
                navbar={true}
                bsPrefix=" btn text-light myButtonNav "
                variant="danger"
                className="px-1 navLink  d-flex align-items-center"
                title="VoLTE KPI"
                id="VoLTE KPI"
              >
                <Link
                  className="py-2 px-4 dropdown-item myNavDropDownElement"
                  to={{ pathname: "/dashboard" }}
                >
                  Dashboard
                </Link>
                {(KPIAdmin || admin) && (
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/targetmonthlyapprovals" }}
                  >
                    Worklog and Approvals
                  </Link>
                )}
              </DropdownButton>
            )}

            {simpleUser || admin || KPIAdmin ? (
              <DropdownButton
                show={openedMenu.includes("Exports")}
                onClick={() => Toggle("Exports")}
                navbar={true}
                bsPrefix=" btn text-light myButtonNav "
                variant="danger"
                className="px-1 navLink  d-flex align-items-center"
                title="Exports"
                id="reports"
              >
                {(!KPIAdmin || admin || simpleUser) && (
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/generatelcmdb" }}
                  >
                    Generate LCM DB
                  </Link>
                )}
                {admin && (
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/generatevoltedashboard" }}
                  >
                    Generate VoLTE Dashboard
                  </Link>
                )}
                {(!KPIAdmin || admin || simpleUser) && (
                  <Link
                    className="py-2 px-4 dropdown-item myNavDropDownElement"
                    to={{ pathname: "/vaiexport" }}
                  >
                    Generate VAI
                  </Link>
                )}
              </DropdownButton>
            ) : null}
          </Nav>
        </Navbar.Collapse>

        <Navbar style={{ position: "absolute", right: 0, top: 5 }}>
          <Nav className="mr-auto">
            <label
              className="mb-0 mr-2 text-white"
              style={{ marginTop: "3px" }}
            >
              {rtnVersionApp()}
            </label>
            <Nav.Link className="mx-2 py-0" onClick={() => LogOutConfirm()}>
              <img
                style={{ height: 20 }}
                src={require("../img/user.png")}
                alt="userIcon"
              />
            </Nav.Link>
          </Nav>
        </Navbar>
      </Navbar>
      <ModalConfirm data={confirm} />
    </div>
  );
};

export default NavBarHome;
