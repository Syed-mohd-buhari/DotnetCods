import React, { FC } from "react";
import { useTheme } from "../../Context/ThemeContext";
import "./DarkMode.css";

const DarkMode: FC = () => {
  const { darkMode, toggleDarkMode } = useTheme();

  return (
    <div className="dark_mode" tabIndex={0}>
      <input
        className="dark_mode_input"
        type="checkbox"
        id="darkmode-toggle"
        onChange={toggleDarkMode}
        checked={darkMode}
      />
      <label className="dark_mode_label" htmlFor="darkmode-toggle"></label>
    </div>
  );
};

export default DarkMode;
