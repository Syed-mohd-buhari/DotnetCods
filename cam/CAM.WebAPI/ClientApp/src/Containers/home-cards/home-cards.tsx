import React, { useState } from "react";
import { Modal, OverlayTrigger, Tooltip } from "react-bootstrap";
import { Link } from "react-router-dom";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import {
  LcmEngineeringDtoCreate,
  LcmEngineeringDtoUpdate,
} from "../../Model/LcmEngineering";
import { GetLcmEngineeringCreateResource } from "../../Redux/Action/LcmEngineering/LcmEngineeringCreateAction";
import {
  deleteLcmEngineering,
  RestoreLcmEngineering,
} from "../../Redux/Action/LcmEngineering/LcmEngineeringDeleteAction";
import { GetLcmEngineeringEditResource } from "../../Redux/Action/LcmEngineering/LcmEngineeringEditAction";
import setLoader from "../../Redux/Action/LoaderAction";
import LcmEngineeringModal from "../../screen/LcmEngineering/LcmEngineeringModal";
import ManageMigration from "../../screen/PlannedActivities/ManageMigrationModal";
import UpdatePlannedActivityStatusModal from "../../screen/PlannedActivities/UpdatePlannedActivityStatusModal";
import InitializeNewProductModal from "../../screen/Shared/InitializeNewProductModal";
import "./home-cards.css";
import { useAuth } from "./../../Hook/useAuth";

const HomeCards = () => {
  const [
    isVisibleModalInitializeNewProduct,
    setIsVisibleModalInitializeNewProduct,
  ] = useState<boolean>(false);

  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);

  const [isVisibleModalManage, setIsVisibleModalManage] =
    useState<boolean>(false);

  const { readonly, isPermesso } = useAuth();

  const newClick = () => {
    if (readonly) {
      return;
    } else {
      New();
    }
  };

  //REFRESH PAGINA DOPO IL SALVATAGGIO ALLA CHIUSURA DELLA MODALE
  const refresh = () => {
    setLoader("ADD", "GetLcmEngineeringGrid");
    closeModal();
    setLoader("REMOVE", "GetLcmEngineeringGrid");
  };

  const { New, Edit, isVisibleModal, edit, closeModal, localStateHistory } =
    useOperationTableCrud<LcmEngineeringDtoUpdate, LcmEngineeringDtoCreate>(
      GetLcmEngineeringCreateResource,
      GetLcmEngineeringEditResource,
      deleteLcmEngineering,
      refresh,
      RestoreLcmEngineering
    );

  return (
    <>
      <Modal
        show={isVisibleModalInitializeNewProduct}
        backdrop="static"
        keyboard={false}
        size="lg"
        dialogClassName="dialogWizard"
        onHide={() => setIsVisibleModalInitializeNewProduct(false)}
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Initialize New Product</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <InitializeNewProductModal
            action={{ setIsVisibleModalInitializeNewProduct }}
          ></InitializeNewProductModal>
        </Modal.Body>
      </Modal>

      <Modal
        show={isVisibleModalStatus}
        backdrop="static"
        keyboard={false}
        size="xl"
        onHide={() => setIsVisibleModalStatus(false)}
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Update Planned Activity Status</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <UpdatePlannedActivityStatusModal
            isFromPlannedActivityModal={false}
            action={{ setIsVisibleModalStatus }}
            planningActivityDetailsResourceId={undefined}
          />
        </Modal.Body>
      </Modal>

      <Modal
        show={isVisibleModalManage}
        backdrop="static"
        keyboard={false}
        size="xl"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Manage Migrations</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <ManageMigration
            isFromPlannedActivityModal={false}
            action={{ setIsVisibleModalManage }}
            plannedActivityId={undefined}
          ></ManageMigration>
        </Modal.Body>
      </Modal>

      <Modal
        show={isVisibleModal}
        onHide={closeModal}
        backdrop="static"
        keyboard={false}
        size="xl"
      >
        <Modal.Header className="d-flex justify-content-center">
          <div className="col-12 px-0 mb-2">
            <div className="col-12 mt-3">
              <h4>New LCM ENGINEERING</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <LcmEngineeringModal
            keyTab={localStateHistory?.tab}
            edit={edit}
            action={{ closeModal, refresh, Edit }}
          />
        </Modal.Body>
      </Modal>

      <div className="container">
        <div className="row">
          <div className="col-12 col-md-3">
            <div className="card-w">
              <h1>Product & Design Library</h1>
              <div className="card-body-w">
                <img src={require("../../img/Product_Design.png")} />
                <div className="links">
                  <p>
                    The Products and Building Blocks that make up our Voice Core
                    Networks.
                  </p>
                  <ul>
                    <OverlayTrigger
                      key={"majorhardware"}
                      placement={"top"}
                      overlay={<Tooltip id={`tooltip-top`}>Hardware</Tooltip>}
                    >
                      <li>
                        <Link to="/majorhardware" title="karem">
                          Hardware
                        </Link>
                        {/* <img src={require("../../img/Vector_1.png")} /> */}
                      </li>
                    </OverlayTrigger>
                    <OverlayTrigger
                      key={"majorsoftware"}
                      placement={"top"}
                      overlay={<Tooltip id={`tooltip-top`}>Software</Tooltip>}
                    >
                      <li>
                        <Link to="/majorsoftware">Software</Link>
                      </li>
                    </OverlayTrigger>
                    <OverlayTrigger
                      key={"systemtype"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>System Type</Tooltip>
                      }
                    >
                      <li>
                        <Link to="/systemtype">System Type</Link>
                      </li>
                    </OverlayTrigger>
                    <OverlayTrigger
                      key={"testinfo"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>
                          System Verification Problems
                        </Tooltip>
                      }
                    >
                      <li>
                        <Link to="/systemverificationproblems">
                          System Verification Problems
                        </Link>
                      </li>
                    </OverlayTrigger>
                    <div className="w-100 mt-4">
                      <OverlayTrigger
                        key={"designcomponent"}
                        placement={"top"}
                        overlay={
                          <Tooltip id={`tooltip-top`}>Design Component</Tooltip>
                        }
                      >
                        <li className="w-fc">
                          <Link to="/designcomponent">Design Component</Link>
                        </li>
                      </OverlayTrigger>
                    </div>
                  </ul>
                </div>
              </div>
            </div>
          </div>

          <div className="col-12 col-md-3">
            <div className="card-w">
              <h1>Network Plan</h1>
              <div className="card-body-w">
                <img src={require("../../img/Network_plan.png")} />
                <div className="links">
                  <p>
                    Implementation & Evolution: Network Transformation is at the
                    heart of Engineering. Essential to this are the planned
                    activities that will evolve the current deployments – the
                    network element instances and their build levels.
                  </p>
                  <ul>
                    <OverlayTrigger
                      key={"lcmengineering"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>LCM Engineering</Tooltip>
                      }
                    >
                      <li>
                        <Link to="/lcmengineering">LCM Engineering</Link>
                        {/* <img src={require("../../img/Vector_1.png")} /> */}
                      </li>
                    </OverlayTrigger>
                    <div className="w-100">
                      <OverlayTrigger
                        key={"asplanned"}
                        placement={"top"}
                        overlay={<Tooltip id={`tooltip-top`}>Assets</Tooltip>}
                      >
                        <li className="w-fc">
                          <Link to="/asplanned">Assets</Link>
                          {/* <img src={require("../../img/Vector_1.png")} /> */}
                        </li>
                      </OverlayTrigger>
                    </div>
                    <OverlayTrigger
                      key={"plannedActivities"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>Planned Activities</Tooltip>
                      }
                    >
                      <li>
                        <Link to="/plannedActivities"> Planned Activities</Link>
                        {/* <img src={require("../../img/Vector_1.png")} /> */}
                      </li>
                    </OverlayTrigger>
                  </ul>
                </div>
              </div>
            </div>
          </div>

          <div className="col-12 col-md-3">
            <div className="card-w">
              <h1>Manage Transformation</h1>
              <div className="card-body-w">
                <img src={require("../../img/manage_transformation.png")} />
                <div className="links">
                  <p>
                    Better Informed: Recording the Planning, Design and
                    Implementation decisions and outcomes as the Engineering
                    Community makes them leads to better decision making.
                  </p>
                  <ul>
                    <OverlayTrigger
                      key={"Initialize New Product"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>
                          Initialize New Product
                        </Tooltip>
                      }
                    >
                      <li
                        onClick={() =>
                          setIsVisibleModalInitializeNewProduct(
                            readonly ? false : true
                          )
                        }
                      >
                        Initialize New Product
                        {/* <img src={require("../../img/Vector_1.png")} /> */}
                      </li>
                    </OverlayTrigger>
                    <OverlayTrigger
                      key={"Deploy Design Component"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>
                          Deploy Design Component
                        </Tooltip>
                      }
                    >
                      <li onClick={newClick}>Deploy Design Component</li>
                    </OverlayTrigger>
                    <OverlayTrigger
                      key={"Update Planned Activity Status"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>
                          Update Planned Activity Status
                        </Tooltip>
                      }
                    >
                      <li
                        onClick={() =>
                          setIsVisibleModalStatus(readonly ? false : true)
                        }
                      >
                        Update Planned Activity Status
                        {/* <img src={require("../../img/Vector_1.png")} /> */}
                      </li>
                    </OverlayTrigger>
                    <OverlayTrigger
                      key={"Manage Migrations"}
                      placement={"top"}
                      overlay={
                        <Tooltip id={`tooltip-top`}>Manage Migrations</Tooltip>
                      }
                    >
                      <li
                        onClick={() =>
                          setIsVisibleModalManage(readonly ? false : true)
                        }
                      >
                        Manage Migrations
                        {/* <img src={require("../../img/Vector_1.png")} /> */}
                      </li>
                    </OverlayTrigger>
                  </ul>
                </div>
              </div>
            </div>
          </div>

          <div className="col-12 col-md-3">
            <div className="card-w">
              <h1>Reports</h1>
              <div className="card-body-w">
                <img src={require("../../img/reports.png")} />
                <div className="links">
                  <p>
                    Assurance: Reporting is a means through which we demonstrate
                    the quality of the Engineering that takes place within our
                    organization. These are the formal reports and data we issue
                    to management.
                  </p>
                  <ul>
                    <OverlayTrigger
                      key={"Explore"}
                      placement={"top"}
                      overlay={<Tooltip id={`tooltip-top`}>Explore</Tooltip>}
                    >
                      <li>
                        <a
                          href="https://vodafone.sharepoint.com/sites/TEMS-Application/SitePages/TEMS-BI-Reports.aspx"
                          target="_blank"
                        >
                          Explore
                        </a>
                      </li>
                    </OverlayTrigger>
                    <OverlayTrigger
                      key={"TEMS Home"}
                      placement={"top"}
                      overlay={<Tooltip id={`tooltip-top`}>TEMS Home</Tooltip>}
                    >
                      <li>
                        <a
                          href="https://vodafone.sharepoint.com/sites/TEMS-Application"
                          target="_blank"
                        >
                          TEMS Home
                        </a>
                      </li>
                    </OverlayTrigger>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default HomeCards;
