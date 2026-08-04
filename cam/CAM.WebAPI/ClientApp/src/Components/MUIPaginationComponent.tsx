import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";
import { useTheme } from "../Context/ThemeContext";
import Select, { MultiValue, components } from "react-select";
import React, { useState, useEffect } from "react";
import { FormControl, InputLabel, Button } from "@mui/material";

interface Props {
  pagination:
    | { page: number | undefined; pageSize: number | undefined }
    | undefined;
  totalItems: number | undefined;
  actions: {
    next(pageNumber: number): any | void;
    back(pageNumber: number): any | void;
    updatePageSize(pageSize: number): void; // ✅ Added function
  };
}

const MUIPaginationComponent = (props: Props) => {
  const totalItems = props.totalItems ?? 0;
  const pageSize = props.pagination?.pageSize ?? 10;
  const pageCount = Math.ceil(totalItems / pageSize);
  const currentPage = props.pagination?.page ?? 1;
  const { darkMode } = useTheme();

  const handleOnPageChange = (
    _event: React.ChangeEvent<unknown>,
    page: number
  ) => {
    props.actions.next(page);
  };

  const [menuIsOpen, setMenuIsOpen] = useState(false);
  const [selectedOptions, setSelectedOptions] = useState<MultiValue<any>>([
    { value: "5", label: "5" },
  ]);
  const [inputValue, setInputValue] = useState("");
  const [options, setOptions] = useState([
    { value: "10", label: "10" },
    { value: "15", label: "15" },
  ]);

  useEffect(() => {
    if (selectedOptions.length > 0) {
      setSelectedOptions((prevSelected) => {
        const newSelected = options.filter((opt) =>
          prevSelected.some((sel) => sel.value === opt.value)
        );
        return newSelected;
      });
    }
  }, [options]);

  const handleInputChange = (value: string) => {
    setInputValue(value);
  };

  const handleAddOption = () => {
    if (inputValue.trim() && !options.some((opt) => opt.value === inputValue)) {
      const newOption = { value: inputValue, label: inputValue };

      setOptions((prevOptions) => [...prevOptions, newOption]); // Add new option
      setSelectedOptions([newOption]); // Set as selected
      props.actions.updatePageSize(Number(newOption.value)); // ✅ Notify parent
      setInputValue(""); // Clear input
      setMenuIsOpen(false); // Close dropdown
    }
  };

  const handlePageSizeChange = (selected: any) => {
    setSelectedOptions(selected);
    setMenuIsOpen(false); // Close dropdown
    props.actions.updatePageSize(Number(selected.value)); // ✅ Pass selected value to parent
  };

  const isValueNew =
    inputValue.trim().length > 0 &&
    !options.some(
      (opt) => opt.label.toLowerCase() === inputValue.toLowerCase()
    );

  // Footer with Apply & Close buttons
  const DropdownFooter = ({
    selectProps,
    setMenuIsOpen,
    inputValue,
    handleAddOption,
  }: any) => {
    const handleApply = () => {
      if (isValueNew) {
        handleAddOption();
      }
      setMenuIsOpen(false);
    };

    return (
      <div style={footerStyle}>
        <button onClick={() => setMenuIsOpen(false)} style={buttonStyle}>
          Close
        </button>
        <button
          onClick={handleApply}
          style={{
            ...buttonStyle,
            backgroundColor: "rgb(176 39 39 / 12%)",
            color: "#b02727",
          }}
          disabled={!isValueNew}
        >
          Add
        </button>
      </div>
    );
  };

  // Styles
  const footerStyle = {
    display: "flex",
    justifyContent: "space-between",
    padding: "8px",
    borderTop: "1px solid #ddd",
  };

  const buttonStyle = {
    padding: "5px 10px",
    border: "none",
    borderRadius: "4px",
    cursor: "pointer",
    backgroundColor: "#f0f0f0",
  };

  return (
    <Stack
      spacing={2}
      direction="row"
      sx={{
        marginTop: "1rem",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      {totalItems > 0 && (
        <>
          <Pagination
            count={pageCount}
            page={currentPage}
            onChange={handleOnPageChange}
            variant="outlined"
            shape="rounded"
            color={darkMode ? "primary" : "secondary"}
            siblingCount={0}
            boundaryCount={2}
          />

          {/* <FormControl
            variant="outlined"
            size="small"
            style={{
              display: "flex",
              flexDirection: "row",
              alignItems: "center",
              gap: "8px",
            }}
          >
            <Select
              className="w-100 text-left"
              menuPosition={"fixed"}
              options={options ?? []}
              placeholder={"Rows per page"}
              value={selectedOptions ?? []}
              onChange={handlePageSizeChange} // ✅ Updated
              inputValue={inputValue}
              menuPlacement={"auto"}
              onInputChange={handleInputChange}
              menuIsOpen={menuIsOpen}
              getOptionLabel={(option) => option.label}
              formatOptionLabel={(data) => (
                <span dangerouslySetInnerHTML={{ __html: data.label }} />
              )}
              onMenuOpen={() => setMenuIsOpen(true)}
              onMenuClose={() => setMenuIsOpen(false)}
              closeMenuOnSelect={false}
              styles={{
                control: (provided, { isFocused }) => ({
                  ...provided,
                  minHeight: "40px",
                  width: "150px",
                  borderColor: isFocused ? "#b02727" : provided.borderColor,
                  boxShadow: isFocused
                    ? "0 0 0 1px #b02727"
                    : provided.boxShadow,
                  "&:hover": {
                    borderColor: "#b02727",
                  },
                }),
                option: (provided, { isSelected }) => ({
                  ...provided,
                  backgroundColor: isSelected
                    ? "#e76767  !important"
                    : "transparent",
                  color: isSelected ? "white" : provided.color,
                  "&:hover": {
                    backgroundColor: "rgb(176 39 39 / 12%)",
                  },
                }),
                clearIndicator: (provided) => ({
                  ...provided,
                  display: "none",
                }),
                multiValueRemove: (provided) => ({
                  ...provided,
                  display: "none",
                }),
              }}
              components={{
                Menu: (props) => (
                  <components.Menu {...props}>
                    {props.children}
                    <DropdownFooter
                      selectProps={props.selectProps}
                      setMenuIsOpen={setMenuIsOpen}
                      inputValue={inputValue}
                      handleAddOption={handleAddOption}
                    />
                  </components.Menu>
                ),
              }}
            />
          </FormControl> */}
        </>
      )}
    </Stack>
  );
};

export default MUIPaginationComponent;
