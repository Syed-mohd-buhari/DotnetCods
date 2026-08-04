import React, {
  createContext,
  FC,
  useContext,
  useEffect,
  useState,
} from "react";

interface ThemeContextType {
  darkMode: boolean;
  toggleDarkMode: () => void;
  selectDarkMode: (value: boolean) => void;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (!context) {
    throw new Error("useTheme must be used within a ThemeProvider");
  }
  return context;
};

export const ThemeProvider: FC = ({ children }) => {
  const [darkMode, setDarkMode] = useState<boolean>(() => {
    const savedTheme = localStorage.getItem("Theme");
    return savedTheme === "dark";
  });

  const setThemeWithEffect = (value: boolean) => {
    setDarkMode(value);
    (document.querySelector("body") as HTMLElement).setAttribute(
      "data-theme",
      value ? "dark" : "light"
    );
    localStorage.setItem("Theme", value ? "dark" : "light");
  };

  const toggleDarkMode = (): void => {
    setThemeWithEffect(!darkMode);
  };
  const selectDarkMode = (value: boolean): void => {
    const theme = localStorage?.getItem("Theme");
    if (value) {
      setDarkMode(value);
      (document.querySelector("body") as HTMLElement).setAttribute(
        "data-theme",
        "dark"
      );
    } else {
      (document.querySelector("body") as HTMLElement).setAttribute(
        "data-theme",
        `${theme}`
      );
      setDarkMode(theme === "dark" ? true : false);
    }
  };

  useEffect(() => {
    const savedTheme = localStorage.getItem("Theme");
    if (savedTheme) {
      setDarkMode(savedTheme === "dark");
    }
  }, []);

  useEffect(() => {
    (document.querySelector("body") as HTMLElement).setAttribute(
      "data-theme",
      darkMode ? "dark" : "light"
    );
  }, [darkMode]);

  return (
    <ThemeContext.Provider value={{ darkMode, toggleDarkMode, selectDarkMode }}>
      {children}
    </ThemeContext.Provider>
  );
};
