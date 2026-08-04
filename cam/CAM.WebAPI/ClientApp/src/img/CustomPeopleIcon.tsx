import React from "react";
import { SvgIcon, SvgIconProps } from "@mui/material";

interface CustomPeopleIconProps extends SvgIconProps {
  isHover?: boolean;
}

const CustomPeopleIcon = ({
  isHover = false,
  ...props
}: CustomPeopleIconProps) => (
  <SvgIcon
    {...props}
    viewBox="0 0 36 36"
    sx={{
      width: 36,
      height: 36,
      color: "#E60000",
      transition: "color 0.3s ease",
      ...props.sx,
      "&:hover": {
        color: "#FFFFFF",
      },
    }}
  >
    <path
      d="M25.005 19.695C27.06 21.09 28.5 22.98 28.5 25.5V30H34.5V25.5C34.5 22.23 29.145 20.295 25.005 19.695ZM22.5 18C25.815 18 28.5 15.315 28.5 12C28.5 8.685 25.815 6 22.5 6C21.795 6 21.135 6.15 20.505 6.36C21.7958 7.95633 22.5 9.9471 22.5 12C22.5 14.0529 21.7958 16.0437 20.505 17.64C21.135 17.85 21.795 18 22.5 18ZM13.5 18C16.815 18 19.5 15.315 19.5 12C19.5 8.685 16.815 6 13.5 6C10.185 6 7.5 8.685 7.5 12C7.5 15.315 10.185 18 13.5 18ZM13.5 9C15.15 9 16.5 10.35 16.5 12C16.5 13.65 15.15 15 13.5 15C11.85 15 10.5 13.65 10.5 12C10.5 10.35 11.85 9 13.5 9ZM13.5 19.5C9.495 19.5 1.5 21.51 1.5 25.5V30H25.5V25.5C25.5 21.51 17.505 19.5 13.5 19.5ZM22.5 27H4.5V25.515C4.8 24.435 9.45 22.5 13.5 22.5C17.55 22.5 22.2 24.435 22.5 25.5V27Z"
      fill="currentColor"
    />
    <path
      opacity="0.3"
      d="M13.5 15C15.1569 15 16.5 13.6569 16.5 12C16.5 10.3431 15.1569 9 13.5 9C11.8431 9 10.5 10.3431 10.5 12C10.5 13.6569 11.8431 15 13.5 15Z"
      fill="currentColor"
      fill-opacity="0.5"
    />
    <path
      opacity="0.3"
      d="M13.5 22.5C9.45 22.5 4.8 24.435 4.5 25.515V27H22.5V25.5C22.2 24.435 17.55 22.5 13.5 22.5Z"
      fill="currentColor"
      fill-opacity="0.5"
    />
  </SvgIcon>
);

export default CustomPeopleIcon;
