import React, { useState } from "react";
import { Modal } from "react-bootstrap";
import ManageMigration from "../../screen/PlannedActivities/ManageMigrationModal";
import UpdatePlannedActivityStatusModal from "../../screen/PlannedActivities/UpdatePlannedActivityStatusModal";
import InitializeNewProductModal from "../../screen/Shared/InitializeNewProductModal";
import ProductLifecycleConstraints from "../../screen/Shared/ProductLifecycleConstraints";
import NavHeader from "./nav-header/nav-header";
import Sidebar from "../landingPage/sidebar";
import LandingPageHeader from "../landingPage/landingPageHeader";
import { useModal } from "../../Hook/useModal";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import NewLandingPageHeader from "../landingPage/NewLandingPageHeader";
import { useLocation } from "react-router-dom";
import { useAuth } from "../../Hook/useAuth";

const Header = () => {
  // const [isVisibleModalManage, setIsVisibleModalManage] =
  //   useState<boolean>(false);
  // const [
  //   isVisibleModalInitializeNewProduct,
  //   setIsVisibleModalInitializeNewProduct,
  // ] = useState<boolean>(false);
  // const [isVisibleModalProductLifecycle, setIsVisibleModalProductLifecycle] =
  //   useState<boolean>(false);
  // const [isVisibleModalStatus, setIsVisibleModalStatus] =
  //   useState<boolean>(false);
  const location: any = useLocation();
  const { isPermesso } = useAuth();

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

  return (
    <>
      {/* <NavHeader /> */}

      {/* {location?.pathname !== "/home" ? (
        <LandingPageHeader />
      ) : (
        <NewLandingPageHeader />
      )} */}
      {location?.pathname !== "/overview" && isPermesso ? (
        <>
          <LandingPageHeader />
          <Sidebar />
        </>
      ) : null}
      <Dialog
        open={isVisibleModalInitializeNewProduct}
        onClose={() => setIsVisibleModalInitializeNewProduct(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mb-0">Initialize New Product</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalInitializeNewProduct(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <InitializeNewProductModal
            action={{ setIsVisibleModalInitializeNewProduct }}
          ></InitializeNewProductModal>
        </DialogContent>
      </Dialog>

      <Dialog
        open={isVisibleModalStatus}
        onClose={() => setIsVisibleModalStatus(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mb-0">Update Planned Activity Status</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalStatus(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <UpdatePlannedActivityStatusModal
            isFromPlannedActivityModal={false}
            action={{ setIsVisibleModalStatus }}
            planningActivityDetailsResourceId={undefined}
          />
        </DialogContent>
      </Dialog>

      <Dialog
        open={isVisibleModalManage}
        onClose={() => setIsVisibleModalManage(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12">
            <h4 className="mb-0">Manage Migration</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalManage(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <ManageMigration
            isFromPlannedActivityModal={false}
            action={{ setIsVisibleModalManage }}
            plannedActivityId={undefined}
          ></ManageMigration>
        </DialogContent>
      </Dialog>

      <Dialog
        open={isVisibleModalProductLifecycle}
        onClose={() => setIsVisibleModalProductLifecycle(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 ">
            <h4 className="mb-0">Product Lifecycle Constraint</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalProductLifecycle(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <ProductLifecycleConstraints
            action={{ setIsVisibleModalProductLifecycle }}
          ></ProductLifecycleConstraints>
        </DialogContent>
      </Dialog>
    </>
  );
};

export default Header;
