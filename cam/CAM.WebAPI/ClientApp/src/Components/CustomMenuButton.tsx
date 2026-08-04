import React, { useState } from "react";
import {
  Button,
  MenuItem,
  Divider,
  Paper,
  MenuList,
  ListItemIcon,
  ListItemText,
  Typography,
  useTheme,
} from "@mui/material";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import ClickAwayListener from "@mui/material/ClickAwayListener";

interface MenuOption {
  label?: string;
  iconOrContent?: React.ReactNode;
  title?: string;
  onClick?: () => void;
  divider?: boolean;
}

interface CustomMenuButtonProps {
  buttonLabel?: string;
  options: MenuOption[];
  endIcon?: React.ReactNode;
  darkTheme?: boolean;
  buttonProps?: React.ComponentProps<typeof Button>;
}

const CustomMenuButton: React.FC<CustomMenuButtonProps> = ({
  buttonLabel = "Options",
  options,
  endIcon,
  darkTheme = false,
  buttonProps,
}) => {
  const [open, setOpen] = useState(false);
  const toggleMenu = () => setOpen((prev) => !prev);

  // Create the theme based on darkMode state
  const theme = createTheme({
    palette: {
      mode: darkTheme ? "dark" : "light", // Adjust for dark/light mode
      primary: {
        main: "#00c2e4", // Primary color for both dark and light mode
      },
      background: {
        default: darkTheme ? "#121212" : "#ffffff", // Dark/light background colors
      },
      text: {
        primary: darkTheme ? "#ffffff" : "#000000", // Text color for dark and light mode
        secondary: darkTheme ? "#aaaaaa" : "#555555", // Secondary text color
      },
    },
  });

  return (
    <ThemeProvider theme={theme}>
      <div style={{ position: "relative", display: "inline-block" }}>
        <Button
          variant="contained"
          disableElevation
          onClick={toggleMenu}
          endIcon={endIcon}
          sx={{
            backgroundColor: darkTheme ? "#424242" : "#f5f5f5", // dark gray for dark mode, light gray for light mode
            color: darkTheme ? "#fff" : "#000", // white text on dark, black on light
            "&:hover": {
              backgroundColor: darkTheme ? "#616161" : "#e0e0e0", // slightly darker gray on hover
            },
          }}
          {...buttonProps}
        >
          {buttonLabel}
        </Button>

        {open && (
          <ClickAwayListener onClickAway={() => setOpen(false)}>
            <Paper
              sx={{
                position: "absolute",
                top: "100%",
                right: 0,
                zIndex: 10,
                mt: 1,
                borderRadius: 1,
                backgroundColor: theme.palette.background.default, // Background color from theme
                color: theme.palette.text.primary, // Text color from theme
                boxShadow: `0px 4px 6px ${theme.palette.action.selectedOpacity}`, // Adjust shadow
              }}
            >
              <MenuList>
                {options.flatMap((option, index) => {
                  const items = [
                    <MenuItem
                      key={`menu-item-${index}`}
                      onClick={() => {
                        if (option.onClick) {
                          option.onClick?.();
                          toggleMenu();
                        }
                      }}
                      sx={{
                        backgroundColor: theme.palette.background.paper,
                        "&:hover": {
                          backgroundColor: theme.palette.action.hover,
                        },
                      }}
                    >
                      {option.iconOrContent && (
                        <ListItemIcon
                          sx={{ color: theme.palette.text.primary }}
                        >
                          {option.iconOrContent}
                        </ListItemIcon>
                      )}
                      {option.title && (
                        <ListItemText
                          sx={{
                            textAlign: "left",
                            fontWeight: "bolder",
                            color: theme.palette.text.primary,
                          }}
                        >
                          {option.title}
                        </ListItemText>
                      )}
                      {option.label && (
                        <ListItemText
                          sx={{
                            textAlign: "left",
                            color: theme.palette.text.primary,
                          }}
                        >
                          {option.label}
                        </ListItemText>
                      )}
                    </MenuItem>,
                  ];

                  if (option.divider) {
                    items.push(<Divider key={`divider-${index}`} />);
                  }

                  return items;
                })}
              </MenuList>
            </Paper>
          </ClickAwayListener>
        )}
      </div>
    </ThemeProvider>
  );
};

export default CustomMenuButton;
