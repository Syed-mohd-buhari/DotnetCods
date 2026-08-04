import React from "react";
import { SvgIcon, SvgIconProps } from "@mui/material";

interface DataDateDuotoneIconProps extends SvgIconProps {
  isHover?: boolean;
}

const CustomBoxIcon = ({
  isHover = false,
  ...props
}: DataDateDuotoneIconProps) => (
  <SvgIcon
    {...props}
    viewBox="0 0 36 36"
    sx={{
      width: 36,
      height: 36,
      color: isHover ? "#FFFFFF" : "#E60000",
      transition: "color 0.3s ease",
      ...props.sx,
    }}
  >
    <svg
      width="36"
      height="36"
      viewBox="0 0 36 36"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        opacity="0.16"
        d="M6 12H30V27C30 27.7956 29.6839 28.5587 29.1213 29.1213C28.5587 29.6839 27.7956 30 27 30H9C8.20435 30 7.44129 29.6839 6.87868 29.1213C6.31607 28.5587 6 27.7956 6 27V12Z"
        fill="currentColor"
      />
      <path
        d="M28.5 13.5H7.5V27C7.5 27.3978 7.65815 27.7792 7.93945 28.0605C8.22076 28.3419 8.60218 28.5 9 28.5H27C27.3978 28.5 27.7792 28.3419 28.0605 28.0605C28.3419 27.7792 28.5 27.3978 28.5 27V13.5ZM18 16.5C18.8284 16.5 19.5 17.1716 19.5 18C19.5 18.8284 18.8284 19.5 18 19.5H12C11.1716 19.5 10.5 18.8284 10.5 18C10.5 17.1716 11.1716 16.5 12 16.5H18ZM9.62109 10.5H26.3789L23.3789 7.5H12.6211L9.62109 10.5ZM31.5 27C31.5 28.1935 31.0256 29.3377 30.1816 30.1816C29.3377 31.0256 28.1935 31.5 27 31.5H9C7.80653 31.5 6.66227 31.0256 5.81836 30.1816C4.97445 29.3377 4.5 28.1935 4.5 27V12C4.5 11.6022 4.65815 11.2208 4.93945 10.9395L10.9395 4.93945L11.0493 4.83984C11.3163 4.62109 11.6519 4.5 12 4.5H24C24.3978 4.5 24.7792 4.65815 25.0605 4.93945L31.0605 10.9395C31.3419 11.2208 31.5 11.6022 31.5 12V27Z"
        fill="currentColor"
      />
    </svg>
  </SvgIcon>
);

export default CustomBoxIcon;
