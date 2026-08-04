// components/JoyrideWrapper.tsx

import React from "react";
import { Joyride, EventData, STATUS, Step } from "react-joyride";
import { useDispatch, useSelector } from "react-redux";
import {
  startTour,
  stopTour,
  setStepIndex,
} from "../../Redux/Action/tourActions";

import { Tooltip, Paper, Button, Typography, Box } from "@mui/material";
import { MdNavigateNext, MdClose } from "react-icons/md";
import { RootState } from "../../Redux/Store/rootStore";

const JoyrideWrapper = ({ steps }) => {
  const dispatch = useDispatch();
  const { run, stepIndex } = useSelector((state: RootState) => state.tour);

  const handleJoyrideCallback = (data: EventData) => {
    const { status, index, type } = data;

    if (status === STATUS.FINISHED || status === STATUS.SKIPPED) {
      dispatch(stopTour());
    } else if (type === "step:after") {
      dispatch(setStepIndex(index + 1));
    }
  };

  return (
    <Joyride
      steps={steps}
      stepIndex={stepIndex}
      run={run}
      continuous
      scrollToFirstStep
      onEvent={handleJoyrideCallback}
      options={{
        arrowColor: "#fff",
        backgroundColor: "#fff",
        overlayColor: "rgba(0,0,0,0.5)",
        primaryColor: "#1976d2",
        textColor: "#000",
        zIndex: 10000,
        showProgress: true,
        buttons: ["skip", "back", "close"],
      }}
      tooltipComponent={({
        step,
        index,
        size,
        backProps,
        closeProps,
        primaryProps,
        skipProps,
      }) => (
        <Paper elevation={4} sx={{ p: 2, maxWidth: 350 }}>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 1,
            }}
          >
            <Typography variant="h6">{step.title}</Typography>
            <Button {...closeProps} sx={{ minWidth: 0 }}>
              <MdClose />
            </Button>
          </Box>
          <Typography
            variant="body2"
            sx={{
              mb: 2,
            }}
          >
            {step.content}
          </Typography>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            <Button {...skipProps} variant="text" color="secondary">
              Skip
            </Button>
            <Button
              {...primaryProps}
              variant="contained"
              endIcon={<MdNavigateNext />}
            >
              {index === size - 1 ? "Finish" : "Next"}
            </Button>
          </Box>
        </Paper>
      )}
    />
  );
};

export default JoyrideWrapper;
