import * as React from "react";
import OutlinedInput from "@mui/material/OutlinedInput";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import ListItemText from "@mui/material/ListItemText";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import Checkbox from "@mui/material/Checkbox";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { Box, Menu, TextField } from "@mui/material";
import { useState } from "react";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;

interface Option {
  key: string;
  value: string;
}

interface MultiSelectCheckmarksProps {
  options: Option[];
  selectedValues: string[];
  onChange: (selected: string[]) => void;
  label?: string;
  widthSize?: any;
  darkTheme?: boolean;
  size?: string;
  isComponent?: boolean;
}

interface MultiSingleSelectProps {
  options: Option[];
  selectedValues: string[];
  onChange: (selected: any) => void;
  label?: string;
  widthSize?: any;
  darkTheme?: boolean;
  size?: string;
  isComponent?: boolean;
  placeholder?: string;
  disabled?: boolean;
}

export const MultiSelectCheckmarks: React.FC<MultiSelectCheckmarksProps> = ({
  options,
  selectedValues,
  onChange,
  label = "Select",
  widthSize,
  darkTheme,
  size = "medium",
  isComponent,
}) => {
  const theme = createTheme({
    palette: {
      mode: darkTheme ? "dark" : "light",
    },
  });
  const [searchTerm, setSearchTerm] = useState("");
  const isAllSelected = selectedValues.length === options.length;

  const filteredOptions = options.filter(({ value }) =>
    value.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleChange = (event: SelectChangeEvent<string[]>) => {
    const {
      target: { value },
    } = event;
    if (value.includes("select_all")) {
      onChange(isAllSelected ? [] : options.map(({ key }) => key));
    } else {
      onChange(typeof value === "string" ? value.split(",") : value);
    }
  };
  return (
    <ThemeProvider theme={theme}>
      <FormControl
        size={size as "small" | "medium"}
        fullWidth
        sx={{ alignSelf: "center", width: widthSize }}
      >
        <InputLabel
          id="demo-simple-select-standard-label"
          sx={{ color: "#ccc" }}
        >
          {label}
        </InputLabel>
        <Select
          multiple
          value={selectedValues ?? []}
          onChange={handleChange}
          input={<OutlinedInput label={label} />}
          MenuProps={
            {
              PaperProps: {
                style: {
                  maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
                  width: widthSize ?? "250px",
                  marginTop: "10px",
                },
              },
            } as any
          }
          renderValue={(selected) => (
            <span
              dangerouslySetInnerHTML={{
                __html: isAllSelected
                  ? "Selected All"
                  : options
                      .filter(({ key }) => selected?.includes(key))
                      .map(({ value }) => value)
                      .join(", "),
              }}
            />
          )}
          sx={{ width: widthSize, textAlign: "left" }}
        >
          <MenuItem value="select_all" sx={{ padding: 0 }}>
            <Checkbox checked={isAllSelected} />
            <ListItemText primary="Select All" />
          </MenuItem>
          {options.map(({ key, value }) => (
            <MenuItem key={key} value={key} sx={{ padding: 0 }}>
              <Checkbox
                checked={
                  selectedValues?.length > 0 && selectedValues?.includes(key)
                    ? true
                    : false
                }
              />
              <ListItemText
                sx={{ padding: 0 }}
                primary={
                  <span
                    style={{
                      display: "block",
                      whiteSpace: "normal",
                      wordBreak: "break-word",
                    }}
                    dangerouslySetInnerHTML={{ __html: value }}
                  />
                }
              />
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </ThemeProvider>
  );
};

export const MultiSingleSelect: React.FC<MultiSingleSelectProps> = ({
  options,
  selectedValues,
  onChange,
  label = "Select",
  widthSize,
  darkTheme,
  size = "medium",
  isComponent,
  placeholder,
  disabled,
}) => {
  const theme = createTheme({
    palette: {
      mode: darkTheme ? "dark" : "light",
    },
  });

  const handleChange = (event: any) => {
    const {
      target: { value },
    } = event;
    onChange(value ?? null);
  };
  return (
    <ThemeProvider theme={theme}>
      <FormControl
        size={size as "small" | "medium"}
        fullWidth
        sx={{ alignSelf: "center", width: widthSize }}
      >
        <InputLabel
          id="demo-simple-select-standard-label"
          sx={{ color: "#ccc" }}
        >
          {label}
        </InputLabel>
        <Select
          value={selectedValues ?? ""}
          onChange={handleChange}
          input={<OutlinedInput label={label} />}
          displayEmpty
          MenuProps={
            {
              PaperProps: {
                style: {
                  maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
                  width: widthSize ?? "250px",
                  marginTop: "10px",
                },
              },
            } as any
          }
          disabled={disabled}
          renderValue={(selected: any) => {
            if (!selected || selected.length === 0) {
              return <em></em>;
            }
            return (
              <span
                dangerouslySetInnerHTML={{
                  __html: options
                    .filter(({ key }) => selected == key)
                    .map(({ value }) => value)
                    .join(", "),
                }}
              />
            );
          }}
          sx={{
            width: widthSize,
            textAlign: "left",
            cursor: disabled ? "not-allowed" : "pointer",
          }}
        >
          {options.map(({ key, value }) => (
            <MenuItem key={key} value={key}>
              <ListItemText
                sx={{ padding: 0 }}
                primary={
                  <span
                    style={{
                      display: "block",
                      whiteSpace: "normal",
                      wordBreak: "break-word",
                    }}
                    dangerouslySetInnerHTML={{ __html: value }}
                  />
                }
              />
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </ThemeProvider>
  );
};
