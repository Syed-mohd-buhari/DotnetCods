// CommonInputField.tsx

import React, { useState, ChangeEvent, useEffect, useCallback } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import Select, { MultiValue, components, GroupBase } from "react-select";
import DatePicker from "react-datepicker";
import { MultiSelect } from "react-multi-select-component";
import { useTheme } from "../Context/ThemeContext";

declare module "react-select/dist/declarations/src/Select" {
  export interface Props<
    Option,
    IsMulti extends boolean,
    Group extends GroupBase<Option>
  > {
    descriptions?: any;
    showTooltip?: boolean;
  }
}

interface InputProps {
  label?: string;
  value: any;
  onChange: (value: any, date?: any) => any;
  error?: string;
  required?: boolean;
  disabled?: boolean;
  labelCSS?: string;
  inputCSS?: string;
  labelFormCSS?: string;
  inputEdit?: boolean;
  addButton?: boolean;
  isSearchable?: boolean;
  disableSearch?: boolean;
  isClearable?: boolean;
  minDate?: Date;
  maxDate?: Date;
  dateFormat?: string;
  placeholderText?: string;
  defaultValue?: any;
  areaRow?: any;
  isError?: boolean;
  isAdd?: boolean;
  showTimeSelect?: boolean;
  isList?: boolean;
  position?: any;
  validationError?: boolean;
  disableYears?: any;
  onAddClicked?: () => any;
  onBlur?: () => any;
}

export const InputLabelComponent = ({ label, required, labelCSS }) => {
  const { darkMode, selectDarkMode } = useTheme();
  return (
    <label
      className={`${
        darkMode ? "text-color-white" : "labelForm"
      } voda-bold w-100 ${labelCSS} `}
    >
      {label}
      {required && <span className="red">*</span>}
    </label>
  );
};

export const TextInputComponent: React.FC<InputProps> = ({
  label,
  value,
  onChange,
  error,
  required,
  disabled,
  labelCSS,
  inputCSS,
  addButton,
  inputEdit,
  isError,
  isList,
  validationError,
}) => {
  // Split the value into individual chips if isList is true
  const chips =
    isList && value !== "" ? value.split(",").map((item) => item.trim()) : null;
  return (
    <>
      <InputLabelComponent
        label={label}
        required={required ?? false}
        labelCSS={labelCSS}
      />
      <label className={`labelForm ${!isList && "voda-bold"} w-100 `}>
        {isList && chips !== null ? (
          <div className={`chip-input-container ${inputCSS}`}>
            {chips.map((chip, index) => (
              <span key={index} className="chip">
                {chip}
              </span>
            ))}
          </div>
        ) : (
          <input
            type="text"
            key={label}
            onChange={(e) => onChange(e)}
            className={`inputForm w-100 ${inputCSS}`}
            value={value ?? ""}
            disabled={disabled ?? false}
          />
        )}
        <div className="w-100">
          {((required && isError) || validationError) && (
            <label className="validation">{error}</label>
          )}
        </div>
      </label>
    </>
  );
};

export const TextAreaInputComponent: React.FC<InputProps> = ({
  label,
  value,
  onChange,
  error,
  required,
  disabled,
  labelCSS,
  inputCSS,
  addButton,
  inputEdit,
  areaRow,
  isError,
}) => {
  return (
    <>
      <InputLabelComponent
        label={label}
        required={required ?? false}
        labelCSS={labelCSS}
      />
      <label className="labelForm voda-bold w-100 ">
        <textarea
          key={label}
          onChange={(e) => onChange(e)}
          className={`inputForm w-100 ${inputCSS}`}
          value={value ?? ""}
          rows={areaRow}
          disabled={disabled ?? false}
        />
        <div className="w-100">
          {required && isError && <label className="validation">{error}</label>}
        </div>
      </label>
    </>
  );
};

export const DropdownInputComponent: React.FC<
  InputProps & {
    options: any;
    inputType?: string;
    min?: number;
    max?: number;
    successMessage?: string;
    showAllOption?: boolean;
  }
> = ({
  defaultValue,
  label,
  value,
  placeholderText,
  onChange,
  onBlur,
  options,
  error,
  required,
  disabled,
  isSearchable,
  isClearable,
  labelCSS,
  labelFormCSS,
  isError,
  isAdd,
  inputType,
  min,
  max,
  onAddClicked,
  successMessage,
  position = "fixed",
  showAllOption = false,
}) => {
  let newOption = options;

  if (inputType === "number" && min !== undefined && max !== undefined) {
    newOption = Array.from({ length: max - min + 1 }, (_, i) => ({
      key: min + i,
      value: min + 1 === 0 ? "All" : (min + i)?.toString(),
    }));
  }

  if (showAllOption) {
    newOption = [{ key: "all", value: "All" }, ...(newOption ?? [])];
  }

  let selectedValue = value;
  if (
    showAllOption &&
    (!selectedValue || Object.keys(selectedValue).length === 0)
  ) {
    selectedValue = { key: "all", value: "All" };
  }

  return (
    <>
      {label !== undefined && label !== null ? (
        <InputLabelComponent
          label={label}
          required={required ?? false}
          labelCSS={labelCSS}
        />
      ) : null}
      <label className={`labelForm w-100 ${labelFormCSS}`}>
        <div
          className={`d-flex ${
            label !== undefined && label !== null ? "mt-1" : ""
          }`}
        >
          <Select
            className="w-100 text-left"
            key={label}
            menuPosition={position}
            options={newOption ?? []}
            value={selectedValue ?? null}
            defaultValue={selectedValue ?? null}
            onChange={(e) => onChange(e)}
            onBlur={onBlur}
            placeholder={placeholderText}
            isSearchable={isSearchable}
            isClearable={isClearable}
            isDisabled={disabled ?? false}
            getOptionLabel={(option) => option.value}
            getOptionValue={(option) => option["key"]?.toString()}
            formatOptionLabel={function (data) {
              return (
                <span
                  dangerouslySetInnerHTML={{
                    __html: data.value,
                  }}
                />
              );
            }}
          ></Select>
          {isAdd && (
            <button className="btn btn-link" type="button">
              <img
                style={{ height: 15 }}
                onClick={onAddClicked}
                src={require("../img/plus_icon.png")}
                alt="plus"
              />
            </button>
          )}
        </div>
        {required && isError && (
          <div className="w-100">
            <label className="validation">{error}</label>
          </div>
        )}
        {value && successMessage && (
          <div className="w-100">
            <label className="text-success">{successMessage}</label>
          </div>
        )}
      </label>
    </>
  );
};

const Input = (props: any) => (
  <components.Input
    {...props}
    style={{ opacity: 0, height: 0, position: "absolute" }}
  />
);

const Menu = (props: any) => {
  const { selectProps } = props;
  return (
    <components.Menu {...props}>
      {selectProps.isSearchable !== false && (
        <div
          style={{
            borderBottom: "1px solid #ccc",
            background: "#fff",
            borderRadius: "4px 4px 0 0",
          }}
        >
          <input
            autoFocus
            type="text"
            placeholder="Search"
            value={selectProps.inputValue}
            onChange={(e) =>
              selectProps.onInputChange(e.target.value, {
                action: "input-change",
                prevInputValue: selectProps.inputValue,
              })
            }
            onMouseDown={(e) => e.stopPropagation()}
            onTouchEnd={(e) => e.stopPropagation()}
            onFocus={(e) => {
              e.target.style.background = "#f1f3f5";
            }}
            style={{
              width: "100%",
              height: "38px",
              border: "none",
              outline: "none",
              color: "#333",
              background: "transparent",
              padding: "0px 10px",
            }}
          />
        </div>
      )}
      {props.children}
    </components.Menu>
  );
};

const MenuList = (props: any) => {
  const { children, getValue, options, setValue } = props;
  const selected: any[] = getValue();
  const allSelected =
    options.length > 0 &&
    options.every((o: any) => selected.some((v: any) => v.value === o.value));

  const toggleAll = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setValue(
      allSelected ? [] : options,
      allSelected ? "deselect-option" : "select-option"
    );
  };

  return (
    <components.MenuList {...props}>
      <div
        onMouseDown={toggleAll}
        style={{
          display: "flex",
          alignItems: "center",
          gap: 8,
          padding: "10px",
          cursor: "pointer",
          background: allSelected ? "#fde8e8" : "#fff",
          marginBottom: "0.5rem",
          position: "sticky",
          top: 0,
          zIndex: 1,
        }}
      >
        <input
          type="checkbox"
          checked={allSelected}
          onChange={() => null}
          style={{
            width: 14,
            height: 14,
            accentColor: "#e24b4a",
            cursor: "pointer",
            flexShrink: 0,
          }}
        />
        <span style={{ color: "#333" }}>Select All</span>
      </div>

      {children}
    </components.MenuList>
  );
};
const OptionTooltip = ({
  description,
  position,
}: {
  description: any;
  position: { x: number; y: number };
}) => (
  <div
    style={{
      position: "fixed",
      left: position.x + 8,
      top: position.y,
      zIndex: 99999,
      background: "#fff",
      border: "1px solid #ddd",
      borderRadius: 4,
      boxShadow: "0 4px 12px rgba(0,0,0,0.15)",
      minWidth: 200,
      maxWidth: 280,
      pointerEvents: "none",
    }}
  >
    <div
      style={{
        display: "flex",
        alignItems: "center",
        gap: 6,
        padding: "8px 12px",
        borderBottom: "1px solid #eee",
      }}
    >
      <span style={{ fontWeight: "bolder", color: "#333" }}>Description</span>
    </div>
    <div
      style={{
        padding: "8px 12px",
        fontSize: 12.5,
        color: description ? "#444" : "#999",
        lineHeight: 1.5,
      }}
    >
      {description || "No Description Available"}
    </div>
  </div>
);

const CheckboxOption = (props: any) => {
  const {
    data,
    isSelected,
    isFocused,
    innerRef,
    innerProps,
    options,
    selectProps,
  } = props;
  const { descriptions = {}, showTooltip = true } = selectProps;
  const [tooltipPos, setTooltipPos] = React.useState<{
    x: number;
    y: number;
  } | null>(null);

  const index = options.findIndex((o: any) => o.value === data.value);
  const isEven = index % 2 !== 0;
  const description = descriptions[String(data.value)] ?? null;
  return (
    <div
      ref={innerRef}
      {...innerProps}
      onMouseEnter={(e) => {
        if (!showTooltip) return;
        const rect = (e.currentTarget as HTMLElement).getBoundingClientRect();
        setTooltipPos({ x: rect.right, y: rect.top });
      }}
      onMouseLeave={() => setTooltipPos(null)}
      style={{
        display: "flex",
        alignItems: "center",
        gap: 8,
        padding: "10px",
        marginBottom: "0.5rem",
        cursor: "pointer",
        position: "relative",
        background: isSelected ? "#fde8e8" : isFocused ? "#f0f0f0" : "#fff",
      }}
    >
      <input
        type="checkbox"
        checked={isSelected}
        onChange={() => null}
        style={{
          width: 14,
          height: 14,
          accentColor: "#e24b4a",
          cursor: "pointer",
          flexShrink: 0,
        }}
      />
      <span style={{ color: "#333", flex: 1 }}>{data.label}</span>

      {showTooltip && (
        <i
          className="ti ti-info-circle"
          style={{ fontSize: 14, color: "#bbb", flexShrink: 0 }}
          aria-hidden="true"
        />
      )}

      {tooltipPos && showTooltip && (
        <OptionTooltip description={description} position={tooltipPos} />
      )}
    </div>
  );
};

const ValueContainer = ({ children, getValue, selectProps, ...props }: any) => {
  const selected: any[] = getValue();
  const total = selectProps.options?.length ?? 0;

  const displayText =
    selected.length === 0
      ? ""
      : selected.length === total
      ? "All items selected"
      : selected.map((v: any) => v.label).join(", ");

  return (
    <components.ValueContainer
      {...props}
      getValue={getValue}
      selectProps={selectProps}
    >
      {React.Children.map(children, (child) =>
        child?.type === components.Input ? child : null
      )}
      <span
        style={{
          color: selected.length === 0 ? "#aaa" : "#333",
          whiteSpace: "nowrap",
          overflow: "hidden",
          textOverflow: "ellipsis",
          maxWidth: "calc(100% - 8px)",
        }}
      >
        {displayText || "Select"}
      </span>
    </components.ValueContainer>
  );
};

const MultiSelectValue = () => null;

const getStyles = (isOpen: boolean) => ({
  control: (base: any, state: any) => ({
    ...base,
    padding: "0 10px",
    height: "38px",
    border: `1px solid ${isOpen || state.isFocused ? "#4285f4" : "#ccc"}`,
    borderRadius: 4,
    boxShadow: `${isOpen || state.isFocused ? "#4285f4 0 0 0 1px" : "none"}`,
    cursor: "pointer",
    background: state.isDisabled ? "#f5f5f5" : "#fff",
  }),
  valueContainer: (base: any) => ({
    ...base,
    flexWrap: "nowrap",
    padding: "2px 8px",
    overflow: "hidden",
  }),
  menu: (base: any) => ({
    ...base,
    borderRadius: 4,
    zIndex: 9999,
    marginTop: "8px",
    boxShadow: "0 0 0 1px #0000001a,0 4px 11px #0000001a",
    overflow: "hidden",
    padding: 0,
  }),
  menuPortal: (base: any) => ({ ...base, zIndex: 9999 }),
  menuList: (base: any) => ({
    ...base,
    padding: 0,
    maxHeight: 260,
    overflowY: "auto",
  }),
  option: () => ({}),
  input: (base: any) => ({ ...base, margin: 0, padding: 0 }),
  dropdownIndicator: (base: any) => ({
    ...base,
    padding: "4px 8px",
    color: "#666",
  }),
  indicatorSeparator: () => ({ display: "none" }),
  clearIndicator: () => ({ display: "none" }),
  placeholder: () => ({ display: "none" }),
});

const Control = ({ children, ...props }: any) => (
  <components.Control {...props}>
    <div
      style={{
        display: "flex",
        alignItems: "center",
        width: "100%",
        cursor: "pointer",
      }}
      onMouseDown={(e) => {
        e.preventDefault(); // prevents label/focus conflicts
        if (props.selectProps.menuIsOpen) {
          props.selectProps.onMenuClose?.();
        } else {
          props.selectProps.onMenuOpen?.();
        }
      }}
    >
      {children}
    </div>
  </components.Control>
);

export const MultiSelectComponent: React.FC<
  InputProps & {
    options: { label: string; value: any }[];
    descriptions?: any;
    showTooltip?: boolean;
  }
> = ({
  label,
  value = [],
  onChange,
  options = [],
  error,
  required,
  disabled,
  disableSearch,
  labelCSS,
  isError,
  isAdd,
  onAddClicked,
  descriptions,
  showTooltip = false,
}) => {
  const [isOpen, setIsOpen] = React.useState(false);
  const [inputValue, setInputValue] = React.useState("");
  const wrapRef = React.useRef<HTMLDivElement>(null);
  const selectRef = React.useRef<any>(null);

  React.useEffect(() => {
    const handleOutsideClick = (e: MouseEvent) => {
      if (wrapRef.current && !wrapRef.current.contains(e.target as Node)) {
        setIsOpen(false);
        setInputValue("");
      }
    };
    document.addEventListener("mousedown", handleOutsideClick);
    return () => document.removeEventListener("mousedown", handleOutsideClick);
  }, []);
  return (
    <>
      <InputLabelComponent
        label={label}
        required={required ?? false}
        labelCSS={labelCSS}
      />
      <label className="labelForm w-100">
        <div className="d-flex PA mt-1">
          <div ref={wrapRef} style={{ width: "100%" }}>
            <Select
              {...({} as any)}
              ref={selectRef}
              isMulti
              className="w-100"
              options={options}
              value={value ?? []}
              onChange={(e) => onChange(e as any)}
              isDisabled={disabled ?? false}
              isSearchable={!disableSearch}
              inputValue={inputValue}
              onInputChange={(val, action) => {
                if (action.action === "input-change") setInputValue(val);
              }}
              closeMenuOnSelect={false}
              hideSelectedOptions={false}
              isClearable={false}
              // menuPortalTarget={document.body}
              // menuPosition="fixed"
              menuPlacement="auto"
              menuIsOpen={isOpen}
              onMenuOpen={() => {
                setIsOpen(true);
                setInputValue("");
              }}
              onMenuClose={() => {
                setIsOpen(false);
                setInputValue("");
              }}
              descriptions={descriptions}
              showTooltip={showTooltip}
              components={{
                Control,
                Option: CheckboxOption,
                Menu,
                MenuList,
                ValueContainer,
                MultiValue: MultiSelectValue,
                Input,
              }}
              styles={getStyles(isOpen)}
            />
          </div>
          {isAdd && (
            <button className="btn btn-link" type="button">
              <img
                style={{ height: 15 }}
                onClick={onAddClicked}
                src={require("../img/plus_icon.png")}
                alt="plus"
              />
            </button>
          )}
        </div>
        {required && isError && (
          <div className="w-100">
            <label className="validation">{error}</label>
          </div>
        )}
      </label>
    </>
  );
};
export const MultiReactSelect: React.FC<InputProps & { options: any }> = ({
  defaultValue,
  label,
  value,
  onChange,
  onBlur,
  options,
  error,
  required,
  disabled,
  disableSearch,
  isClearable,
  labelCSS,
  isError,
  isAdd,
  onAddClicked,
}) => {
  const [menuIsOpen, setMenuIsOpen] = useState(false);
  const [selectedOptions, setSelectedOptions] = useState<MultiValue<any>>([]);

  useEffect(() => {
    if (value) {
      setSelectedOptions(
        value?.map((opt) => ({
          value: opt.key,
          label: opt.value,
        }))
      );
    }
  }, [value]);

  // Convert to react-select format
  const formattedOptions = options?.map((opt) => ({
    value: opt.key,
    label: opt.value,
  }));
  // Custom option with checkbox
  const CustomOption = (props: any) => {
    const { data, isSelected, innerRef, innerProps } = props;

    return (
      <div ref={innerRef} {...innerProps} style={optionStyle}>
        <input
          type="checkbox"
          checked={isSelected}
          readOnly
          style={{ marginRight: 8 }}
        />
        {data.label}
      </div>
    );
  };

  // Footer with Apply & Close buttons
  const DropdownFooter = ({ selectProps, setMenuIsOpen }: any) => {
    const handleApply = () => {
      const selectedData = (selectProps.value || []).map((opt: any) => ({
        key: opt.value,
        value: opt.label,
      }));
      onChange(selectedData);
      setMenuIsOpen(false);
    };

    return (
      <div style={footerStyle}>
        <button onClick={() => setMenuIsOpen(false)} style={buttonStyle}>
          Close
        </button>
        <button
          onClick={handleApply}
          style={{ ...buttonStyle, backgroundColor: "#007bff", color: "white" }}
        >
          Apply
        </button>
      </div>
    );
  };

  // Styles
  const optionStyle = {
    display: "flex",
    alignItems: "center",
    padding: "8px",
  };

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
    <>
      <InputLabelComponent
        label={label}
        required={required ?? false}
        labelCSS={labelCSS}
      />
      <div className="d-flex PA mt-1">
        <Select
          isMulti
          className="w-100 text-left"
          menuPosition={"fixed"}
          options={formattedOptions ?? []}
          value={
            selectedOptions?.filter((selected) =>
              formattedOptions?.some(
                (option) => option.value === selected.value
              )
            ) ?? []
          } // Ensures only valid selected values are shown
          onChange={setSelectedOptions}
          menuIsOpen={menuIsOpen}
          getOptionLabel={(option) => option.label}
          formatOptionLabel={function (data) {
            return (
              <span
                dangerouslySetInnerHTML={{
                  __html: data.label,
                }}
              />
            );
          }}
          onMenuOpen={() => setMenuIsOpen(true)}
          onMenuClose={() => setMenuIsOpen(false)}
          closeMenuOnSelect={false} // Keep menu open when selecting options
          styles={{
            control: (provided) => ({
              ...provided,
              minHeight: "40px",
            }),
            clearIndicator: (provided) => ({
              ...provided,
              display: "none", // Hides dropdown arrow
            }),
            multiValueRemove: (provided) => ({
              ...provided,
              display: "none", // Hides close (X) button next to selected items
            }),
          }}
          components={{
            // Option: CustomOption, // Custom checkbox option
            Menu: (props) => (
              <components.Menu {...props}>
                {props.children}
                <DropdownFooter
                  selectProps={props.selectProps}
                  setMenuIsOpen={setMenuIsOpen}
                />
              </components.Menu>
            ),
          }}
        />
        {isAdd && (
          <button className="btn btn-link" type="button">
            <img
              style={{ height: 15 }}
              onClick={onAddClicked}
              src={require("../img/plus_icon.png")}
              alt="plus"
            />
          </button>
        )}
      </div>
      {required && isError && (
        <div className="w-100">
          <label className="validation">{error}</label>
        </div>
      )}
    </>
  );
};

export const DateInputComponent: React.FC<InputProps> = ({
  label,
  value,
  onChange,
  minDate,
  maxDate,
  dateFormat,
  placeholderText,
  error,
  required,
  labelCSS,
  isError,
  showTimeSelect,
}) => {
  return (
    <>
      <InputLabelComponent
        label={label}
        required={required ?? false}
        labelCSS={labelCSS}
      />
      <label className="labelForm voda-bold w-100 ">
        <DatePicker
          key={label}
          selected={value ? new Date(value) : null}
          onChange={(newDate, e) => onChange(e, newDate)}
          className="inputForm w-100"
          minDate={minDate}
          maxDate={maxDate}
          showTimeSelect={showTimeSelect}
          dateFormat={dateFormat}
          placeholderText={placeholderText}
        />
        <div className="w-100">
          {required && isError && <label className="validation">{error}</label>}
        </div>
      </label>
    </>
  );
};

export const ShowYearInputComponent: React.FC<InputProps> = ({
  label,
  value,
  onChange,
  minDate,
  maxDate,
  dateFormat,
  placeholderText,
  error,
  required,
  labelCSS,
  isError,
  isClearable,
  showTimeSelect,
  disableYears,
}) => {
  const { darkMode, selectDarkMode } = useTheme();
  const [localError, setLocalError] = useState("");
  const handleChange = (e: any, newDate: Date | null) => {
    if (newDate && disableYears?.includes(newDate.getFullYear())) {
      setLocalError(
        `${newDate?.getFullYear()} already used in another capacity.`
      );
      setTimeout(() => {
        setLocalError("");
      }, 6000);
    } else {
      setLocalError("");
      onChange(e, newDate);
    }
  };
  return (
    <>
      {localError ? (
        <label
          className={`${
            darkMode ? "text-color-white" : "labelForm"
          } voda-bold w-100 ${labelCSS} `}
        >
          {label}
          {required && <span className="red">*</span>}
          <span
            className="text-danger small mr-0"
            style={{ fontSize: "14px", float: "inline-end" }}
          >
            {localError || error}
          </span>
        </label>
      ) : (
        <InputLabelComponent
          label={label}
          required={required ?? false}
          labelCSS={labelCSS}
        />
      )}
      <label className="labelForm voda-bold w-100 ">
        <DatePicker
          key={label}
          className="inputForm w-100"
          selected={value ? new Date(value) : null}
          onChange={(newDate, e) => handleChange(e, newDate)}
          isClearable={isClearable}
          showYearPicker
          dateFormat="yyyy"
          // filterDate={(date: Date) => !disableYear.includes(date.getFullYear())}
        />
        <div className="w-100">
          {!localError && required && isError && (
            <label className="validation">{error}</label>
          )}
        </div>
      </label>
    </>
  );
};

export const ToggleInputComponent: React.FC<InputProps> = ({
  label,
  value,
  onChange,
  disabled,
  required,
  labelCSS,
}) => {
  return (
    <>
      <InputLabelComponent
        label={label}
        required={required ?? false}
        labelCSS={labelCSS}
      />
      <label className="labelForm voda-bold mb-2">
        <div className="switchSmall ml-2">
          <input
            type="checkbox"
            key={label}
            onChange={(e) => onChange(e)}
            className="mr-1"
            checked={value}
            disabled={disabled ?? false}
          />
          <span className="sliderSmall round"></span>
        </div>
      </label>
    </>
  );
};

export const MultiSelectWithDescription: React.FC<
  InputProps & {
    options: any;
    descriptions?: Record<string, string>;
    showTooltip?: boolean;
  }
> = ({
  defaultValue,
  label,
  value,
  onChange,
  onBlur,
  options,
  error,
  required,
  disabled,
  disableSearch,
  isClearable,
  labelCSS,
  isError,
  isAdd,
  onAddClicked,
  descriptions = {},
  showTooltip = true,
}) => {
  const [tooltip, setTooltip] = useState({
    visible: false,
    text: "",
    x: 0,
    y: 0,
  });

  const getSelectedDescriptions = () => {
    if (!value || !Array.isArray(value) || value.length === 0) return [];

    return value
      .map((item: any) => {
        const key = item?.value || item?.key || "";
        return descriptions[key] || "";
      })
      .filter(Boolean);
  };

  const selectedDescriptions = getSelectedDescriptions();

  const handleMouseOver = (e: React.MouseEvent) => {
    const target = e.target as HTMLElement;
    const optionElement =
      target.closest("li") ||
      target.closest('[class*="option"]') ||
      target.closest('[class*="item"]');

    if (optionElement) {
      const optionText = optionElement.textContent?.trim() || "";
      const matchedOption = Array.isArray(options)
        ? options.find((opt: any) => opt.label === optionText)
        : null;

      if (matchedOption) {
        const key = matchedOption.value || matchedOption.key || "";
        const desc = descriptions[key] || "";

        setTooltip({
          visible: true,
          text: desc && desc.trim() !== "" ? desc : "No description available",
          x: e.clientX,
          y: e.clientY,
        });
      }
    }
  };

  const handleMouseLeave = () => {
    setTooltip({ ...tooltip, visible: false });
  };

  return (
    <div
      onMouseOver={handleMouseOver}
      onMouseLeave={handleMouseLeave}
      style={{ position: "relative" }}
    >
      <style>
        {`
          .description-tooltip {
            position: fixed;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 12px 16px;
            border-radius: 8px;
            font-size: 13px;
            max-width: 320px;
            z-index: 10000;
            pointer-events: none;
            box-shadow: 0 10px 25px rgba(102, 126, 234, 0.4);
            transform: translateY(-100%);
            line-height: 1.5;
            animation: tooltipFadeIn 0.3s ease-out;
          }
          
          .description-tooltip::after {
            content: '';
            position: absolute;
            bottom: -6px;
            left: 20px;
            width: 12px;
            height: 12px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            transform: rotate(45deg);
            border-radius: 2px;
          }
          
          .tooltip-header {
            display: flex;
            align-items: center;
            margin-bottom: 6px;
            font-weight: 600;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 1px;
            opacity: 0.9;
          }
          
          .tooltip-icon {
            margin-right: 6px;
            font-size: 14px;
          }
          
          .tooltip-content {
            font-size: 13px;
            line-height: 1.6;
            opacity: 0.95;
          }
          
          @keyframes tooltipFadeIn {
            0% {
              opacity: 0;
              transform: translateY(-90%) scale(0.95);
            }
            100% {
              opacity: 1;
              transform: translateY(-100%) scale(1);
            }
          }
          
          @keyframes tooltipPulse {
            0%, 100% {
              box-shadow: 0 10px 25px rgba(102, 126, 234, 0.4);
            }
            50% {
              box-shadow: 0 10px 30px rgba(102, 126, 234, 0.6);
            }
          }
          
          .selected-description {
            background: linear-gradient(to right, #f8f9fa, #e9ecef);
            border-left: 3px solid #667eea;
            padding: 8px 12px;
            margin-top: 6px;
            border-radius: 0 6px 6px 0;
            font-size: 12px;
            color: #495057;
            animation: slideIn 0.3s ease-out;
          }
          
          @keyframes slideIn {
            0% {
              opacity: 0;
              transform: translateX(-10px);
            }
            100% {
              opacity: 1;
              transform: translateX(0);
            }
          }
        `}
      </style>
      <InputLabelComponent
        label={label}
        required={required ?? false}
        labelCSS={labelCSS}
      />
      <label className={`labelForm w-100`}>
        <div className="d-flex PA mt-1">
          <MultiSelect
            className="w-100"
            options={options ?? []}
            value={value ?? null}
            onChange={(e) => onChange(e)}
            labelledBy="Select"
            disableSearch={disableSearch}
            disabled={disabled ?? false}
            ClearSelectedIcon={null}
            isOpen={true}
          />
          {isAdd && (
            <button className="btn btn-link" type="button">
              <img
                style={{ height: 15 }}
                onClick={onAddClicked}
                src={require("../img/plus_icon.png")}
                alt="plus"
              />
            </button>
          )}
        </div>
        {required && isError && (
          <div className="w-100">
            <label className="validation">{error}</label>
          </div>
        )}
      </label>

      {!showTooltip && selectedDescriptions.length > 0 && (
        <div>
          {selectedDescriptions.map((desc, index) => (
            <div key={index} className="selected-description">
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ marginRight: "6px", fontSize: "14px" }}>💡</span>
                <span>{desc}</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {showTooltip && tooltip.visible && (
        <div
          style={{
            position: "fixed",
            left: `${tooltip.x + 15}px`,
            top: `${tooltip.y - 15}px`,
            background: "#ffffff",
            color: "#333333",
            padding: "16px",
            borderRadius: "12px",
            fontSize: "13px",
            maxWidth: "300px",
            zIndex: 10000,
            pointerEvents: "none",
            boxShadow: "0 8px 30px rgba(0,0,0,0.12)",
            transform: "translateY(-100%)",
            lineHeight: "1.5",
            border: "1px solid rgba(0,0,0,0.08)",
            animation: "tooltipFadeIn 0.2s ease-out",
          }}
        >
          <div
            style={{
              display: "flex",
              alignItems: "center",
              marginBottom: "8px",
              borderBottom: "2px solid #f0f0f0",
              paddingBottom: "8px",
            }}
          >
            <span
              style={{
                width: "24px",
                height: "24px",
                background: "#6c757d",
                borderRadius: "50%",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                marginRight: "8px",
                color: "white",
                fontSize: "12px",
                fontWeight: "bold",
              }}
            >
              i
            </span>
            <span
              style={{
                fontWeight: "600",
                fontSize: "12px",
                textTransform: "uppercase",
                letterSpacing: "0.5px",
                color: "#666",
              }}
            >
              Description
            </span>
          </div>
          <div
            style={{
              fontSize: "14px",
              color: "#444",
              lineHeight: "1.6",
            }}
          >
            {tooltip.text}
          </div>
        </div>
      )}
    </div>
  );
};
