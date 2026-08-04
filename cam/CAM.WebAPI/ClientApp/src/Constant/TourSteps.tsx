import { Avatar, Box, Button, Stack, Tabs, Typography } from "@mui/material";
import React, { useRef } from "react";
import {
  FaArrowLeft,
  FaCaretRight,
  FaHandPointUp,
  FaLocationArrow,
} from "react-icons/fa";
import { MdChevronRight } from "react-icons/md";
import type { Step as JoyrideStep } from "react-joyride";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router";
import { setMajorSwBuildIndex } from "../Redux/Action/TourGuide/tourAction";
import { setMajorHwBuildIndex } from "../Redux/Action/TourGuide/tourAction";
import { setSystemTypeIndex } from "../Redux/Action/TourGuide/tourAction";

import { GoDotFill } from "react-icons/go";

type Step = JoyrideStep & {
  hideOverlay?: boolean;
  skipBeacon?: boolean;
  skipScroll?: boolean;
  zIndex?: number;
  spotlightPadding?: number;
  overlayClickAction?: "close" | "next" | "replay" | false;
  dismissKeyAction?: "close" | "next" | "replay" | false;
  blockTargetInteraction?: boolean;
};

type TourHandlers = {
  navigate?: (path: string) => void;
  goToNextStep?: () => void;
  goToPrevStep?: () => void;
  endTour?: () => void;
  stepChange?: (step: number) => void;
};
const buttonStyle = {
  m: 0.5,
  px: 2,
  py: 1,
  borderRadius: 2,
  lineHeight: "1.5",
  gap: 1,
  fontSize: "14px",
  whiteSpace: "nowrap",
  textTransform: "none",
  width: "max-content",
  color: "darkblue",
  fontWeight: "bold",
  background: "linear-gradient(to right, #e9ecef, #ccc)",
};

const ScrollableButtonGroup = ({ items }) => {
  const dispatch = useDispatch();
  const scrollRef = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();
  const scroll = (direction: "left" | "right") => {
    if (scrollRef.current) {
      const scrollAmount = 150;
      scrollRef.current.scrollBy({
        left: direction === "left" ? -scrollAmount : scrollAmount,
        behavior: "smooth",
      });
    }
  };
  const handleOnChange = (itm: any) => {
    if (itm.jumpStep !== null && itm.jumpStep >= 0) {
      switch (itm.action) {
        case "editSystemType":
        case "addSystemType":
          dispatch(setSystemTypeIndex(Number(itm.jumpStep)));
          break;

        case "upgradeHardwareVersion":
        case "addMajorHwBuild":
          dispatch(setMajorHwBuildIndex(Number(itm.jumpStep)));
          break;

        case "upgradeSoftwareVersion":
        case "addMajorSwBuild":
          dispatch(setMajorSwBuildIndex(Number(itm.jumpStep)));
          break;

        default:
          break;
      }
    } else {
      navigate(itm.link ?? "/");
    }
  };

  return (
    <Stack
      sx={{
        marginLeft: "1rem",
        display: "flex",
        flexDirection: "row",
        spacing: 1,
        alignItems: "center",
      }}
    >
      {/* <Button variant="text" onClick={() => scroll("left")}>
        ◀
      </Button> */}

      <Box
        ref={scrollRef}
        sx={{
          display: "flex",
          flexWrap: "wrap",
          px: 1,
          // scrollbarWidth: "none", // Firefox
          // "&::-webkit-scrollbar": { display: "none" }, // Webkit
          flex: 1,
          whiteSpace: "nowrap",
          bgcolor: "background.paper",
          borderRadius: 2,
          background: "#fad0c4", // your gradient
        }}
      >
        {items?.map((itm, index) => (
          <Button
            key={index}
            startIcon={<FaLocationArrow size={15} />}
            variant="text"
            sx={buttonStyle}
            onClick={() => handleOnChange(itm)}
          >
            {itm?.text}
          </Button>
        ))}
      </Box>

      {/* <Button variant="text" onClick={() => scroll("right")}>
        ▶
      </Button> */}
    </Stack>
  );
};
export const getMainTourSteps = ({
  navigate,
  goToNextStep,
  goToPrevStep,
  endTour,
  stepChange,
}: TourHandlers): Step[] => [
  {
    content: (
      <>
        <Box sx={{ display: "flex", mb: 4, justifyContent: "left" }}>
          {/* Logo */}
          <Avatar
            src={require("../img/logoCAM_R.png")}
            alt="TEMS Logo"
            sx={{ width: 50, height: 50, marginRight: 2 }}
            variant="square"
          />

          {/* Text Content */}
          <Box>
            <Typography
              variant="subtitle1"
              sx={{ fontWeight: "bold", color: "red" }}
            >
              WELCOME TO TEMS
            </Typography>
            <Typography variant="subtitle1">
              Telecoms Engineering Management System
            </Typography>
          </Box>
        </Box>
        <Box sx={{ gap: 1 }}>
          <Box sx={{ display: "flex", flexDirection: "row", mb: 2 }}>
            <div
              style={{
                marginLeft: "0.8rem",
                marginTop: "2px",
                display: "flex",
              }}
            >
              <GoDotFill size={13} />
            </div>
            {/* <FaCaretRight size={20} /> */}
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Take a quick tour to discover the key tools and features that will
              help you work more efficiently within the TEMS platform.
            </Typography>
          </Box>
          <Box sx={{ display: "flex", flexDirection: "row", mb: 1 }}>
            <div
              style={{
                marginLeft: "0.8rem",
                marginTop: "2px",
                display: "flex",
              }}
            >
              <GoDotFill size={13} />
            </div>
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Tour guide has controls, and clicking below buttons navigates to
              other pages.
            </Typography>
          </Box>
          <ScrollableButtonGroup
            items={[
              {
                text: "Major Software Build",
                link: "/majorsoftware",
              },
              // { text: "Lcm Engineering", link: "/lcmengineering" },
              {
                text: "Major Hardware Build",
                link: "/majorhardware",
              },
              {
                text: "System Type",
                link: "/systemtype",
              },
            ]}
          />
        </Box>
      </>
    ),
    locale: { skip: <strong aria-label="skip">End</strong> },
    hideOverlay: false,
    styles: { tooltip: { width: 700 } },
    placement: "center",
    target: "body",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the LCM Engineering screen for
            creating/maintaining Life Cycle Management of assets.
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
          <Button variant="outlined" color="error" onClick={endTour}>
            End
          </Button>

          <Box display="flex" gap={2}>
            <Button
              variant="outlined"
              startIcon={<FaArrowLeft />}
              onClick={goToPrevStep}
            >
              Back
            </Button>
            <Button variant="contained" onClick={goToNextStep}>
              Next
            </Button>
          </Box>
        </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_lcm_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the Asset Screen and manage network
            configurations.
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
          <Button variant="outlined" color="error" onClick={endTour}>
            End
          </Button>

          <Box display="flex" gap={2}>
            <Button
              variant="outlined"
              startIcon={<FaArrowLeft />}
              onClick={goToPrevStep}
            >
              Back
            </Button>
            <Button variant="contained" onClick={endTour}>
              Finish
            </Button>
          </Box>
        </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_assets_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the Major HW Build screen to define/update the
            Hardware Version of Assets
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
          <Button variant="outlined" color="error" onClick={endTour}>
            End
          </Button>

          <Box display="flex" gap={2}>
            <Button
              variant="outlined"
              startIcon={<FaArrowLeft />}
              onClick={goToPrevStep}
            >
              Back
            </Button>
            <Button variant="contained" onClick={endTour}>
              Finish
            </Button>
          </Box>
        </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_hwBuild_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the Major SW Build screen to define/update the
            Software Version of Assets
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
          <Button variant="outlined" color="error" onClick={endTour}>
            End
          </Button>

          <Box display="flex" gap={2}>
            <Button
              variant="outlined"
              startIcon={<FaArrowLeft />}
              onClick={goToPrevStep}
            >
              Back
            </Button>
            <Button variant="contained" onClick={endTour}>
              Finish
            </Button>
          </Box>
        </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_swBuild_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the System Type Build screen to define/update
            the System Type
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
          <Button variant="outlined" color="error" onClick={endTour}>
            End
          </Button>

          <Box display="flex" gap={2}>
            <Button
              variant="outlined"
              startIcon={<FaArrowLeft />}
              onClick={goToPrevStep}
            >
              Back
            </Button>
            <Button variant="contained" onClick={endTour}>
              Finish
            </Button>
          </Box>
        </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_systemType_tour",
    title: "",
  },
];

export const mainTourGuide: Step[] = [
  {
    content: (
      <Box
        sx={{
          p: 3,
          display: "flex",
          flexDirection: "column",
          justifyContent: "space-between",
          gap: 3,
        }}
      >
        <Typography variant="h4" align="center">
          Welcome to Tems Application
        </Typography>

        {/* <Box
            display="flex"
            justifyContent="space-between"
            alignItems="center"
            alignSelf="center"
          >
            <Button variant="outlined" color="error" onClick={endTour}>
              End
            </Button>
  
            <Box display="flex" gap={2}>
              <Button
                variant="contained"
                color="primary"
                onClick={() => navigate && navigate("/majorsoftware")}
              >
                Tour Major Software Build
              </Button>
            </Box>
          </Box> */}
      </Box>
    ),
    locale: { skip: <strong aria-label="skip">End</strong> },
    hideOverlay: false,
    styles: { tooltip: { width: 700 } },
    placement: "center",
    target: "body",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the LCM Engineering screen for
            creating/maintaining Life Cycle Management of assets.
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
            <Button variant="outlined" color="error" onClick={endTour}>
              End
            </Button>
  
            <Box display="flex" gap={2}>
              <Button
                variant="outlined"
                startIcon={<FaArrowLeft />}
                onClick={goToPrevStep}
              >
                Back
              </Button>
              <Button variant="contained" onClick={goToNextStep}>
                Next
              </Button>
            </Box>
          </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_lcm_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the Asset Screen and manage network
            configurations.(Remove Double time screen)
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
            <Button variant="outlined" color="error" onClick={endTour}>
              End
            </Button>
  
            <Box display="flex" gap={2}>
              <Button
                variant="outlined"
                startIcon={<FaArrowLeft />}
                onClick={goToPrevStep}
              >
                Back
              </Button>
              <Button variant="contained" onClick={endTour}>
                Finish
              </Button>
            </Box>
          </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_assets_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to navigate to the Major SW Build screen to define/update the
            Software Version of Assets
          </Typography>
        </Box>

        {/* <Box mt={3} display="flex" justifyContent="space-between">
            <Button variant="outlined" color="error" onClick={endTour}>
              End
            </Button>
  
            <Box display="flex" gap={2}>
              <Button
                variant="outlined"
                startIcon={<FaArrowLeft />}
                onClick={goToPrevStep}
              >
                Back
              </Button>
              <Button variant="contained" onClick={endTour}>
                Finish
              </Button>
            </Box>
          </Box> */}
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#landing_swBuild_tour",
    title: "",
  },
];

export const getLcmTourSteps: Step[] = [
  {
    content: (
      <>
        <Box sx={{ display: "flex", mb: 4, justifyContent: "center" }}>
          <Box>
            <Typography
              variant="subtitle1"
              sx={{ fontWeight: "bold", color: "red" }}
            >
              LCM Engineering
            </Typography>
          </Box>
        </Box>
        <Box sx={{ gap: 1 }}>
          <Box sx={{ display: "flex", flexDirection: "row", mb: 1 }}>
            <div
              style={{
                marginLeft: "0.8rem",
                marginTop: "2px",
                display: "flex",
              }}
            >
              <GoDotFill size={13} />
            </div>
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Tour guide has controls, and clicking below buttons navigates to
              other pages.
            </Typography>
          </Box>
          <ScrollableButtonGroup
            items={[{ text: "Add LCM Engineering", link: "/" }]}
          />
        </Box>
      </>
    ),
    locale: { skip: <strong aria-label="skip">End</strong> },
    styles: { tooltip: { width: 700 } },
    hideOverlay: false,
    placement: "center",
    target: "body",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to create Life Cycle Management of assets
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#lcm_addButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to Import LCM Engineering Report
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#lcm_importButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to Download LCM Engineering Excel Report
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#lcm_downloadButton_tour",
    title: "",
  },
];

export const getLcmModalTourSteps: Step[] = [
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Opco</b> and click <b>+</b> button to add
            new opco
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#lcmModal_opco_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Current Design Component</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#lcmModal_currentDC_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Cuurent Bag</b> and click <b>+</b> button to
            add new Bag
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#lcmModal_currentBag_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Deployment Status</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#lcmModal_deploymentStatus_tour",
    title: "",
  },
];

export const getSwBuildTourSteps: Step[] = [
  {
    content: (
      <div
        style={{
          minHeight: 200,
          maxWidth: "50rem",
        }}
      >
        <Box sx={{ display: "flex", mb: 4, justifyContent: "center" }}>
          <Box>
            <Typography
              variant="subtitle1"
              sx={{ fontWeight: "bold", color: "red" }}
            >
              Major Software Build
            </Typography>
            {/* <Typography variant="subtitle1">
              Telecoms Engineering Management System
            </Typography> */}
          </Box>
        </Box>
        <Box sx={{ gap: 1 }}>
          <Box sx={{ display: "flex", flexDirection: "row", mb: 1 }}>
            <div
              style={{
                marginLeft: "0.8rem",
                marginTop: "2px",
                display: "flex",
              }}
            >
              <GoDotFill size={13} />
            </div>
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Tour guide has controls, and clicking below buttons navigates to
              other pages.
            </Typography>
          </Box>
          <ScrollableButtonGroup
            items={[
              {
                text: "Upgrade Software Version",
                action: "upgradeSoftwareVersion",
                jumpStep: 6,
              },
              {
                text: "Add Major Sw Build",
                action: "addMajorSwBuild",
                jumpStep: 1,
              },
            ]}
          />
        </Box>
      </div>
    ),
    locale: { skip: <strong aria-label="skip">End</strong> },
    styles: { tooltip: { width: 200 } },
    hideOverlay: false,
    placement: "center",
    target: "body",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to begin creating a new Major Software Build. This will help
            you define and manage significant software changes.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#swBuild_addButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to download the current data as an Excel file for further
            analysis or record-keeping.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#swBuild_downloadButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Equipment Manufacturer
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_originalEquipmentManufacturer",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Product Name
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_productName",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Software Version
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_softwareVersion",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Double click the row to "Edit", "Upgrade" or "Delete" the Software.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#tourGrid_doubleClickRow",
    title: "",
  },
];

export const getMajorSwModalTourSteps: Step[] = [
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Equipment Manufacturer </b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_equipManufactur_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Product Name </b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_productName_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Software Version Number</b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_swVersion_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Design Contact</b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_designContact_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>End of Maintenance</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_endOfMain_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>General Availability Date</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_avialDate_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>End Of Support</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_endOfSupport_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Last Time Buy</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_lastTimeBuy_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to submit
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwAddModal_submit_tour",
    title: "",
  },
];

export const getMajorSwVersionModalTourSteps: Step[] = [
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Input the <b>Software Version Number</b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwModal_swVersion_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>End of Maintenance</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwModal_endOfMain_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>End Of Support</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwModal_endOfSupport_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Box sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Please Submit the form to upgrade S/W to new version and
              associated Design libraries.
            </Typography>
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              "Design Component" will be available in LCM/Planned Activities
              Screens.
            </Typography>
          </Box>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorSwModal_submit_tour",
    title: "",
  },
];
export const getHwBuildTourSteps: Step[] = [
  {
    content: (
      <div
        style={{
          minHeight: 200,
          maxWidth: "50rem",
        }}
      >
        <Box sx={{ display: "flex", mb: 4, justifyContent: "center" }}>
          <Box>
            <Typography
              variant="subtitle1"
              sx={{ fontWeight: "bold", color: "red" }}
            >
              Major Hardware Build
            </Typography>
            {/* <Typography variant="subtitle1">
              Telecoms Engineering Management System
            </Typography> */}
          </Box>
        </Box>
        <Box sx={{ gap: 1 }}>
          <Box sx={{ display: "flex", flexDirection: "row", mb: 1 }}>
            <div
              style={{
                marginLeft: "0.8rem",
                marginTop: "2px",
                display: "flex",
              }}
            >
              <GoDotFill size={13} />
            </div>
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Tour guide has controls, and clicking below buttons navigates to
              other pages.
            </Typography>
          </Box>
          <ScrollableButtonGroup
            items={[
              {
                text: "Modify Hardware Detail",
                action: "upgradeHardwareVersion",
                jumpStep: 6,
              },
              {
                text: "Add Major Hw Build",
                action: "addMajorHwBuild",
                jumpStep: 1,
              },
            ]}
          />
        </Box>
      </div>
    ),
    locale: { skip: <strong aria-label="skip">End</strong> },
    styles: { tooltip: { width: 200 } },
    hideOverlay: false,
    placement: "center",
    target: "body",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to begin creating a new Major Hardware Build. This will help
            you define and manage significant hardware changes.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#hwBuild_addButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to download the current data as an Excel file for further
            analysis or record-keeping.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#hwBuild_downloadButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Equipment Manufacturer
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_originalEquipmentManufacturer",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Platform
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_platform",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Hardware Type
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_hardwareType",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Double click the row to "Edit", or "Delete" the Hardware Details.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#tourGrid_doubleClickHwRow",
    title: "",
  },
];
export const getMajorHwModalTourSteps: Step[] = [
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Equipment Manufacturer </b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorHwAddModal_equipManufactur_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Build Construction </b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorHwAddModal_BuildConstruction_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>Design Contact</b> here.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorHwAddModal_DesignContact_tour",
    title: "",
  },

  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to select <b>End of Maintenance</b>
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorHwAddModal_endOfMain_tour",
    title: "",
  },

  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click here to submit
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "bottom",
    target: "#majorHwAddModal_submit_tour",
    title: "",
  },
];

export const getSystemTypeTourSteps: Step[] = [
  {
    content: (
      <div
        style={{
          minHeight: 200,
          maxWidth: "50rem",
        }}
      >
        <Box sx={{ display: "flex", mb: 4, justifyContent: "center" }}>
          <Box>
            <Typography
              variant="subtitle1"
              sx={{ fontWeight: "bold", color: "red" }}
            >
              System Type
            </Typography>
            {/* <Typography variant="subtitle1">
              Telecoms Engineering Management System
            </Typography> */}
          </Box>
        </Box>
        <Box sx={{ gap: 1 }}>
          <Box sx={{ display: "flex", flexDirection: "row", mb: 1 }}>
            <div
              style={{
                marginLeft: "0.8rem",
                marginTop: "2px",
                display: "flex",
              }}
            >
              <GoDotFill size={13} />
            </div>
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Tour guide has controls, and clicking below buttons navigates to
              other pages.
            </Typography>
          </Box>
          <ScrollableButtonGroup
            items={[
              {
                text: "Edit System Type",
                action: "editSystemType",
                jumpStep: 6,
              },

              {
                text: "Add System Type",
                action: "addSystemType",
                jumpStep: 1,
              },
            ]}
          />
        </Box>
      </div>
    ),
    locale: { skip: <strong aria-label="skip">End</strong> },
    styles: { tooltip: { width: 200 } },
    hideOverlay: false,
    placement: "center",
    target: "body",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to begin creating a new System Type. This will help you define
            and manage significant system changes.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#systemType_addButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Click to download the current data as an Excel file for further
            analysis or record-keeping.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#systemType_downloadButton_tour",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Major HW
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_majorHardwareBuild",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Major SW
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_majorSoftwareBuild",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Filter the Vodafone Name
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: true,
    placement: "right",
    target: "#tourGrid_vodafoneName",
    title: "",
  },
  {
    content: (
      <div
        className="p-3"
        style={{
          minHeight: "auto",
          maxWidth: "30rem",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
          <FaHandPointUp size={20} />
          <Typography variant="body2" sx={{ textAlign: "left" }}>
            Double click the row to "Edit",or "Delete" the System Type.
          </Typography>
        </Box>
      </div>
    ),
    styles: { tooltip: { width: 380 } },
    hideOverlay: false,
    placement: "bottom",
    target: "#tourGrid_doubleClickRow",
    title: "",
  },
];

export const getSystemTypeModalTourSteps = (props): Step[] => {
  const steps: Step[] = [
    {
      content: (
        <div className="p-3" style={{ minHeight: "auto", maxWidth: "30rem" }}>
          <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
            <FaHandPointUp size={20} />
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Click here to select <b>Asset Category first</b>.
            </Typography>
          </Box>
        </div>
      ),
      styles: { tooltip: { width: 380 } },
      hideOverlay: true,
      placement: "bottom",
      target: "#systemTypeAddModal_assetCategory_tour",
      title: "",
    },
  ];

  // Add these steps only if not edit
  if (!props.edit) {
    steps.push(
      {
        content: (
          <div className="p-3" style={{ minHeight: "auto", maxWidth: "30rem" }}>
            <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
              <FaHandPointUp size={20} />
              <Typography variant="body2" sx={{ textAlign: "left" }}>
                Click here to select <b>Major Hardware</b>.
              </Typography>
            </Box>
          </div>
        ),
        styles: { tooltip: { width: 380 } },
        hideOverlay: true,
        placement: "bottom",
        target: "#systemTypeAddModal_majorHardware_tour",
        title: "",
      },
      {
        content: (
          <div className="p-3" style={{ minHeight: "auto", maxWidth: "30rem" }}>
            <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
              <FaHandPointUp size={20} />
              <Typography variant="body2" sx={{ textAlign: "left" }}>
                Click here to select <b>Major Software</b>.
              </Typography>
            </Box>
          </div>
        ),
        styles: { tooltip: { width: 380 } },
        hideOverlay: true,
        placement: "bottom",
        target: "#systemTypeAddModal_majorSoftware_tour",
        title: "",
      }
    );
  }

  // Common steps for both Add & Edit
  steps.push(
    {
      content: (
        <div className="p-3" style={{ minHeight: "auto", maxWidth: "30rem" }}>
          <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
            <FaHandPointUp size={20} />
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Click here to select <b>Vodafone Name</b>.
            </Typography>
          </Box>
        </div>
      ),
      styles: { tooltip: { width: 380 } },
      hideOverlay: true,
      placement: "bottom",
      target: "#systemTypeAddModal_vodafoneName_tour",
      title: "",
    },
    {
      content: (
        <div className="p-3" style={{ minHeight: "auto", maxWidth: "30rem" }}>
          <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
            <FaHandPointUp size={20} />
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Click here to select <b>Product Importance</b>.
            </Typography>
          </Box>
        </div>
      ),
      styles: { tooltip: { width: 380 } },
      hideOverlay: true,
      placement: "bottom",
      target: "#systemTypeAddModal_productImportance_tour",
      title: "",
    },
    {
      content: (
        <div className="p-3" style={{ minHeight: "auto", maxWidth: "30rem" }}>
          <Box sx={{ display: "flex", flexDirection: "row", gap: 1, mb: 1 }}>
            <FaHandPointUp size={20} />
            <Typography variant="body2" sx={{ textAlign: "left" }}>
              Click here to save
            </Typography>
          </Box>
        </div>
      ),
      styles: { tooltip: { width: 380 } },
      hideOverlay: true,
      placement: "bottom",
      target: "#systemTypeAddModal_save_tour",
      title: "",
    }
  );

  return steps;
};
