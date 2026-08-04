import React, { useEffect, useState } from "react";
import { Alert, Box, Fade, IconButton, Slide, Typography } from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import {
  FiCheckCircle,
  FiAlertCircle,
  FiAlertTriangle,
  FiInfo,
  FiX,
} from "react-icons/fi";

const HIDE_ERROR = "HIDE_ERROR";

interface Props {
  OnModal: boolean;
  ShowFixed?: boolean;
}

const ErrorNotification: React.FC<Props> = ({ ShowFixed }) => {
  const dispatch = useDispatch();

  const error = useSelector((state: RootState) => state.errorReducer.error);

  const isOpen = useSelector((state: RootState) => state.errorReducer.isOpen);

  const type = useSelector((state: RootState) => state.errorReducer.notifyType);

  const [visible, setVisible] = useState(false);

  const handleClose = () => {
    setVisible(false);

    setTimeout(() => {
      dispatch({ type: HIDE_ERROR });
    }, 250);
  };

  useEffect(() => {
    if (isOpen) {
      setVisible(true);

      if (!ShowFixed) {
        const timer = setTimeout(handleClose, 4000);
        return () => clearTimeout(timer);
      }
    }
  }, [isOpen, ShowFixed]);

  const notification = (() => {
    switch (type) {
      case 1:
        return {
          title: "Success",
          background: "#C6F3D0",
          border: "#2E9E57",
          color: "#107C41",
          icon: <FiCheckCircle size={22} />,
        };

      case 2:
        return {
          title: "Error",
          background: "#FEE4E2",
          border: "#F04438",
          color: "#B42318",
          icon: <FiAlertCircle size={22} />,
        };

      case 3:
        return {
          title: "Warning",
          background: "#FEF0C7",
          border: "#F79009",
          color: "#B54708",
          icon: <FiAlertTriangle size={22} />,
        };

      default:
        return {
          title: "Information",
          background: "#D1E9FF",
          border: "#2E90FA",
          color: "#175CD3",
          icon: <FiInfo size={22} />,
        };
    }
  })();

  if (!isOpen) return null;

  return (
    <Slide
      direction="left"
      in={visible}
      mountOnEnter
      unmountOnExit
      timeout={300}
    >
      <Fade in={visible} timeout={250}>
        <Box
          sx={{
            position: "fixed",
            top: 24,
            right: 24,
            width: 400,
            zIndex: 9999,
          }}
        >
          <Alert
            variant="filled"
            icon={notification.icon}
            action={
              <IconButton
                size="small"
                onClick={handleClose}
                sx={{
                  color: notification.color,
                  transition: "all .25s ease",

                  "&:hover": {
                    bgcolor: "rgba(255,255,255,.35)",
                    transform: "rotate(90deg)",
                  },
                }}
              >
                <FiX size={18} />
              </IconButton>
            }
            sx={{
              backgroundColor: notification.background,
              color: notification.color,

              borderLeft: `5px solid ${notification.border}`,
              border: `1px solid ${notification.border}`,

              borderRadius: 2,

              boxShadow: "0 10px 28px rgba(0,0,0,.14)",

              transition: ".25s",

              alignItems: "flex-start",

              "&:hover": {
                transform: "translateY(-2px)",
                boxShadow: "0 16px 36px rgba(0,0,0,.18)",
              },

              "& .MuiAlert-icon": {
                color: notification.border,
                marginTop: "2px",
                alignSelf: "flex-start",
                fontSize: 24,
              },

              "& .MuiAlert-message": {
                width: "100%",
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-start",
                justifyContent: "center",
                textAlign: "left",
                padding: 0,
              },

              "& .MuiAlert-action": {
                alignSelf: "flex-start",
                paddingTop: 0,
                marginTop: "-2px",
                marginRight: "-4px",
              },
            }}
          >
            <Typography
              sx={{
                width: "100%",
                fontSize: 15,
                fontWeight: 700,
                color: notification.color,
                textAlign: "left",
                lineHeight: 1.2,
              }}
            >
              {notification.title}
            </Typography>

            <Typography
              sx={{
                width: "100%",
                mt: 0.5,
                fontSize: 14,
                fontWeight: 400,
                color: notification.color,
                textAlign: "left",
                lineHeight: 1.5,
                wordBreak: "break-word",
              }}
            >
              {error}
            </Typography>
          </Alert>
        </Box>
      </Fade>
    </Slide>
  );
};

export default ErrorNotification;
