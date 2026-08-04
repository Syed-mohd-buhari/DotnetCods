import * as React from "react";
import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
  AccordionActions,
  Typography,
  Button,
  Paper,
  Box,
} from "@mui/material";
import { MdOutlineExpandMore } from "react-icons/md";

type CustomAccordionProps = {
  title: string;
  children: React.ReactNode;
  actions?: { label: string; onClick: () => void }[];
  defaultExpanded?: boolean;
  disableToggle?: boolean;
  checkExpanded?: boolean;
};

const CustomAccordion: React.FC<CustomAccordionProps> = ({
  title,
  children,
  actions,
  defaultExpanded = false,
  checkExpanded,
  disableToggle = false,
}) => {
  const [expanded, setExpanded] = React.useState(defaultExpanded);

  const handleChange = (_event: React.SyntheticEvent, isExpanded: boolean) => {
    if (!disableToggle) {
      setExpanded(isExpanded);
    }
  };

  return (
    <Paper elevation={4} sx={{ marginBottom: "10px", overflow: "hidden" }}>
      <Accordion
        expanded={disableToggle || checkExpanded ? true : expanded}
        onChange={handleChange}
        disableGutters={disableToggle}
      >
        <AccordionSummary
          expandIcon={
            !disableToggle && (
              <Box
                sx={{
                  backgroundColor: "#f0f0f0",
                  borderRadius: "50%",
                  padding: "4px",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                }}
              >
                <MdOutlineExpandMore />
              </Box>
            )
          }
          aria-controls={`${title}-content`}
          id={`${title}-header`}
          sx={{
            minHeight: "25px !important",
            height: expanded ? "25px" : "55px",
            top: expanded ? "5px" : "undefined",
            right: expanded ? "-12px" : "undefined",
            "& .MuiAccordionSummary-content": {
              margin: 0,
              alignItems: "center",
            },
          }}
        >
          {!expanded && (
            <Typography
              component="span"
              sx={{ fontWeight: 600, color: "#6c757d" }}
            >
              {title}
            </Typography>
          )}
        </AccordionSummary>

        <AccordionDetails>{children}</AccordionDetails>

        {actions && actions.length > 0 && (
          <AccordionActions>
            {actions.map((action, index) => (
              <Button key={index} onClick={action.onClick}>
                {action.label}
              </Button>
            ))}
          </AccordionActions>
        )}
      </Accordion>
    </Paper>
  );
};

export default CustomAccordion;
