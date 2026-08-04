import React, { useContext, useEffect, useState } from "react";
import { Joyride, STATUS, EventData, Step } from "react-joyride";
import { JoyrideSteps } from "../Model/tourTypes";
import { OptionContext } from "../Context/MenuOptionContext";
import { useNavigate } from "react-router";
import {
  getLcmModalTourSteps,
  getLcmTourSteps,
  getMainTourSteps,
} from "../Constant/TourSteps";
import { Box, Button, Paper, Tooltip, Typography } from "@mui/material";
import { MdClose, MdNavigateNext } from "react-icons/md";
import { setMajorSwBuildIndex } from "../Redux/Action/TourGuide/tourAction";
import { setMajorHwBuildIndex } from "../Redux/Action/TourGuide/tourAction";
import { setSystemTypeIndex } from "../Redux/Action/TourGuide/tourAction";

import { useDispatch } from "react-redux";

interface State {
  run: boolean;
  steps: Step[];
}

interface TourGuideProps {
  page: string;
  start: boolean;
  jumpStep?: number | null;
  tourSteps: Step[];
  setStartTour: (value: boolean) => void;
  onTourEnd: () => void;
}

const TourGuide = ({
  page,
  start,
  jumpStep,
  tourSteps,
  setStartTour,
  onTourEnd,
}: TourGuideProps) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { selectedOption, setSelectedOption } = useContext(OptionContext);
  const [stepIndex, setStepIndex] = useState<number>(1);
  const [progress, setProgress] = useState<number>(0);
  const isModalTour = [
    "lcmModal",
    "majorSwModal",
    "majorSwAddModal",
    "systemTypeAddModal",
    // "initializeNewProd",
    "systemTypeModal",
    "majorHwModal",
    "majorHwAddModal",
  ].includes(page);
  const generateSteps = (val: number): Step[] => tourSteps;
  const [state, setState] = useState<State>({
    run: start,
    steps: generateSteps(progress),
  });
  useEffect(() => {
    setState((prevState) => ({
      ...prevState,
      run: true,
      steps: generateSteps(progress),
    }));
  }, [progress]);

  useEffect(() => {
    const resolvedIndex =
      jumpStep !== null && jumpStep !== undefined && jumpStep >= 0
        ? jumpStep
        : Number(localStorage.getItem(page)) || 0;
    setStepIndex(resolvedIndex);
    setProgress(resolvedIndex + 1);
    setState((prevState) => ({
      ...prevState,
      run: true,
      steps: generateSteps(resolvedIndex + 1),
    }));
  }, [start, jumpStep]);

  const handleTourEndNavigation = (page) => {
    switch (page) {
      case "lcm":
      case "majorHwBuild":
      case "majorSwBuild":
      case "systemType":
        navigate("/home");
        break;
      case "lcmModal":
        navigate("/lcmengineering");
        break;
      case "majorSwAddModal":
        navigate("/majorsoftware");
        break;
      case "systemTypeAddModal":
        navigate("/systemtype");
        break;
      case "majorHwAddModal":
        navigate("/majorhardware");
        break;
      case "majorSwModal":
        navigate("/majorsoftware");
        break;
      case "systemTypeModal":
        navigate("/systemtype");
        break;

      case "majorHwModal":
        navigate("/majorhardware");
        break;
      default:
        break;
    }
    setSelectedOption("");
  };
  const handleJoyrideCallback = (data: EventData) => {
    const { status, action, index, lifecycle } = data;
    if (status === STATUS.FINISHED || status === STATUS.SKIPPED) {
      localStorage.removeItem(page);
      setTimeout(() => {
        handleTourEndNavigation(page);
        setState({ steps: state.steps, run: false });
        setStepIndex(0);
        setProgress(1);
        if (page === "main" || status === STATUS.SKIPPED) {
          onTourEnd();
          setStartTour(false);
        }
      }, 200);

      return;
    }

    if (lifecycle === "complete") {
      let newIndex = index;

      const resolvedIndex =
        jumpStep !== null && jumpStep !== undefined && jumpStep >= 0
          ? jumpStep
          : null;

      if (action === "next") {
        newIndex = resolvedIndex ? resolvedIndex : index + 1;
      } else if (action === "prev") {
        newIndex = resolvedIndex ? resolvedIndex : index - 1;
      }

      // If this was the last step and user clicked next, finish the tour
      if (action === "next" && index === state.steps.length - 1) {
        localStorage.removeItem(page);
        setTimeout(() => {
          setState({ steps: state.steps, run: false });
          setStepIndex(0);
          setProgress(1);
          if (page === "main") {
            onTourEnd();
            setStartTour(false);
          }
          handleTourEndNavigation(page);
        }, 200);

        return;
      }

      // Clamp newIndex
      newIndex = Math.max(0, Math.min(newIndex, state.steps.length - 1));

      setStepIndex(newIndex);
      setProgress(newIndex + 1);

      localStorage.setItem(page, String(newIndex));
      setState((prevState) => ({
        ...prevState,
        steps: generateSteps(newIndex + 1),
      }));
    }
    dispatch(setMajorSwBuildIndex(null));
    dispatch(setMajorHwBuildIndex(null));
    dispatch(setSystemTypeIndex(null));
  };

  const handleSkipToFinal = () => {
    setStepIndex(state.steps.length); // Jump to last step
  };

  return (
    <Joyride
      run={state.run}
      steps={state.steps}
      stepIndex={stepIndex}
      onEvent={handleJoyrideCallback}
      continuous
      scrollToFirstStep={!isModalTour ? true : false}
      options={{
        showProgress: true,
        blockTargetInteraction: true,
        overlayClickAction: false,
        dismissKeyAction: false,
        spotlightPadding: 10,
        skipScroll: true,
        buttons: ["skip", "close"],
        zIndex: isModalTour ? 2000 : 1000,
      }}
      locale={{
        nextWithProgress: "Next ({step}/{steps})",
        last: "Finish",
        skip: "End",
      }}
      styles={{
        overlay: { zIndex: isModalTour ? 1999 : 999 },
        floater: {
          zIndex: isModalTour ? 2100 : 1001,
          borderRadius: "20px",
          // boxShadow: "0 0 0 4px red",
        },
        buttonClose: {
          marginTop: "5px",
          marginRight: "5px",
          width: "12px",
        },
        buttonPrimary: {
          outline: "2px solid transparent",
          outlineOffset: "2px",
          backgroundColor: "#1c7bd4",
          borderRadius: "5px",
          color: "#FFFFFF",
        },
        buttonSkip: {
          color: "#A3A3A3",
        },
        tooltipFooter: {
          margin: "0px 16px 10px 10px",
        },
        buttonBack: {
          outline: "2px solid transparent",
          outlineOffset: "2px",
        },
      }}
      tooltipComponent={({
        step,
        index,
        size,
        backProps,
        closeProps,
        primaryProps,
        skipProps,
      }) => {
        const { title, ...filteredSkipProps } = skipProps || {};
        return (
          <Paper
            elevation={0}
            sx={{
              maxWidth: "50rem",
              minWidth: "30rem",
              borderRadius: 4,
              overflow: "hidden",
              background: "#fff",
              boxShadow: "0px 10px 30px rgba(0, 0, 0, 0.2)",
              position: "relative",
            }}
          >
            {/* Banner Area */}
            <Box
              sx={{
                background: "#fad0c4",
                p: 2,
                position: "relative",
              }}
            >
              {step.content}
            </Box>

            {/* Bottom area with controls */}
            <Box
              sx={{
                p: 2,
                mt: "1px",
                background: "#fad0c4",
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                }}
              >
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    gap: 1,
                  }}
                >
                  <Tooltip
                    key={"endTour"}
                    disableFocusListener
                    disableTouchListener
                    title={"Finish the tour and return to your workflow."}
                    placement="bottom"
                    arrow
                  >
                    <Button
                      {...filteredSkipProps}
                      variant="text"
                      sx={{
                        bgcolor: "#00000082",
                        borderRadius: 2,
                        fontWeight: "bold",
                        color: "#fff",
                        "&:hover": {
                          bgcolor: "#000000a3",
                        },
                      }}
                    >
                      End Tour
                    </Button>
                  </Tooltip>
                  {page !== "main" && (
                    <Tooltip
                      disableFocusListener
                      disableTouchListener
                      title={`${
                        page !== "main"
                          ? "Jump back to the previous screen tour."
                          : "Finish the tour and return to your workflow."
                      }`}
                      placement="right"
                      key={"skip"}
                      arrow
                    >
                      <Button
                        onClick={handleSkipToFinal}
                        variant="text"
                        sx={{ color: "#343a40", fontWeight: "bolder" }}
                      >
                        Skip
                      </Button>
                    </Tooltip>
                  )}
                </Box>
                {/* Dots */}
                {/* <Box display="flex" alignItems="center" gap={0.5}>
                {[...Array(size)].map((_, i) => (
                  <Box
                    key={i}
                    sx={{
                      width: 8,
                      height: 8,
                      borderRadius: "50%",
                      bgcolor: index === i ? "#b71c1c" : "#ffffff",
                    }}
                  />
                ))}
              </Box> */}

                {/* Step Counter as Button */}
                {/* <Button
                disableRipple
                sx={{
                  minWidth: "40px",
                  px: 1,
                  background: "none",
                  border: "none",
                  color: "#b71c1c",
                  cursor: "default",
                  "&:hover": {
                    background: "none",
                  },
                }}
              >
                {index + 1} / {size}
              </Button> */}

                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    gap: 1,
                  }}
                >
                  {/* Back Button */}
                  {index !== 0 && (
                    <Button
                      {...backProps}
                      variant="outlined"
                      sx={{
                        borderColor: "#b71c1c",
                        color: "#b71c1c",
                        fontWeight: "bold",
                        borderRadius: 2,
                        "&:hover": {
                          borderColor: "#a31515",
                          color: "#a31515",
                        },
                        visibility: index === 0 ? "hidden" : "visible",
                      }}
                    >
                      Back
                    </Button>
                  )}
                  {/* Next / Finish */}
                  <Button
                    {...primaryProps}
                    variant="contained"
                    endIcon={<MdNavigateNext />}
                    sx={{
                      bgcolor: "#b71c1c",
                      borderRadius: 2,
                      color: "#fff",
                      "&:hover": {
                        bgcolor: "#a31515",
                      },
                    }}
                  >
                    {index === size - 1
                      ? "Finish"
                      : `Next (${index + 1}/${size})`}
                  </Button>
                </Box>
              </Box>
            </Box>

            {/* Close Button */}
            {/* <Button
            {...closeProps}
            sx={{
              position: "absolute",
              top: 8,
              right: 8,
              minWidth: 0,
              p: 1,
              color: "#8e0000",
              "&:hover": {
                color: "#b71c1c",
              },
              zIndex: 1,
            }}
          >
            <MdClose />
          </Button> */}
          </Paper>
        );
      }}
    />
  );
};

export default TourGuide;
