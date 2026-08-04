import React, { useState, useEffect } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const fieldBg = "#24272b";
const labelColor = "#fff";
const inputStyle = {
  width: "100%",
  borderRadius: 7,
  border: "1.5px solid #282d37",
  background: fieldBg,
  color: "#fff",
  fontSize: 15.5,
  outline: "none",
  padding: "12px 13px",
  marginBottom: 4,
  marginTop: 2,
  fontWeight: 500,
};
const labelStyle = {
  color: labelColor,
  fontWeight: 600,
  marginBottom: 2,
  marginTop: 4,
  fontSize: 15,
  letterSpacing: ".01em",
};
const disabledInput = {
  ...inputStyle,
  color: "#b2b6b8",
  background: "#2e3340",
  cursor: "not-allowed",
};

export function GanttForm({ task, onAction }) {
  const [form, setForm] = useState({
    text: task.text || "",
    description: task.description ?? "",
    start: task.start ? new Date(task.start) : null,
    end: task.end ? new Date(task.end) : null,
    progress: typeof task.progress === "number" ? task.progress : 0,
  });

  const [errors, setErrors] = useState({ startEnd: "" });

  useEffect(() => {
    setForm({
      text: task.text || "",
      description: task.description ?? "",
      start: task.start ? new Date(task.start) : null,
      end: task.end ? new Date(task.end) : null,
      progress: typeof task.progress === "number" ? task.progress : 0,
    });
    setErrors({ startEnd: "" });
  }, [task]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((f) => ({
      ...f,
      [name]: value,
    }));
  };

  const handleProgressChange = (e) => {
    setForm((f) => ({
      ...f,
      progress: Number(e.target.value),
    }));
  };

  const handleDateChange = (field, date) => {
    setForm((f) => ({
      ...f,
      [field]: date,
    }));
  };

  const validate = () => {
    if (form.start && form.end && form.end <= form.start) {
      setErrors({ startEnd: "End date should be greater than Start date" });
      return false;
    }
    setErrors({ startEnd: "" });
    return true;
  };

  const handleSave = () => {
    if (!validate()) return;

    onAction({
      action: "update-task",
      data: {
        ...task,
        text: form.text.trim(),
        description: form.description,
        start: form.start,
        end: form.end,
        progress: form.progress,
      },
    });
  };

  const handleClose = () => {
    onAction({ action: "close-form" });
  };

  return (
    <div
      style={{
        position: "absolute",
        right: 0,
        top: 0,
        width: 380,
        height: "100%",
        background: fieldBg,
        color: "#fff",
        padding: "30px 30px 24px 30px",
        boxShadow: "0 6px 30px #0008",
        borderRadius: "0px 7px 7px 0px",
        display: "flex",
        flexDirection: "column",
        zIndex: 2000,
        overflowY: "scroll",
      }}
    >
      {/* Name field */}
      <div style={{ marginBottom: 17 }}>
        <div style={labelStyle}>Project Status</div>
        <input
          type="text"
          name="text"
          value={form.text}
          disabled
          style={disabledInput}
        />
      </div>

      {/* Description */}
      <div style={{ marginBottom: 18 }}>
        <div style={labelStyle}>Description</div>
        <textarea
          name="description"
          value={form.description}
          onChange={handleChange}
          placeholder="Add description"
          style={{
            ...inputStyle,
            minHeight: 54,
            maxHeight: 110,
            resize: "vertical",
            fontFamily: "inherit",
          }}
        />
      </div>

      {/* Start Date */}
      <div style={{ marginBottom: 14 }}>
        <div style={labelStyle}>Start date</div>
        <DatePicker
          selected={form.start}
          onChange={(date) => handleDateChange("start", date)}
          placeholderText="Select start date"
          dateFormat="dd/MM/yyyy"
          customInput={
            <input style={inputStyle} readOnly={false} autoComplete="off" />
          }
          calendarClassName="gantt-date-picker"
          popperPlacement="bottom-end"
          wrapperClassName="gantt-datepicker-wrapper"
          showDisabledMonthNavigation
        />
      </div>

      {/* End Date */}
      <div style={{ marginBottom: 14 }}>
        <div style={labelStyle}>End date</div>
        <DatePicker
          selected={form.end}
          onChange={(date) => handleDateChange("end", date)}
          placeholderText="Select end date"
          dateFormat="dd/MM/yyyy"
          customInput={
            <input style={inputStyle} readOnly={false} autoComplete="off" />
          }
          calendarClassName="gantt-date-picker"
          popperPlacement="bottom-end"
          wrapperClassName="gantt-datepicker-wrapper"
          showDisabledMonthNavigation
        />
      </div>

      {errors.startEnd && (
        <div style={{ color: "red", fontSize: 12, marginBottom: 12 }}>
          {errors.startEnd}
        </div>
      )}

      {/* Progress */}
      <div
        style={{
          display: "flex",
          alignItems: "center",
          marginBottom: 2,
          marginTop: 17,
        }}
      >
        <div style={labelStyle}>Progress</div>
        <div
          style={{
            fontWeight: 700,
            fontSize: 15,
            color: "#e60000",
            marginLeft: "auto",
          }}
        >
          {form.progress}%
        </div>
      </div>
      <input
        type="range"
        name="progress"
        value={form.progress}
        min={0}
        max={100}
        step={1}
        onChange={handleProgressChange}
        style={{
          width: "100%",
          accentColor: "#e60000",
          marginBottom: 8,
          background: "transparent",
        }}
      />

      {/* Buttons */}
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          gap: 10,
          marginTop: "auto",
          paddingTop: 21,
        }}
      >
        <button
          style={{
            background: "none",
            border: "1.5px solid #395680",
            borderRadius: 2,
            color: "#fff",
            fontWeight: 700,
            padding: "8px 20px",
            fontSize: "14px",
            letterSpacing: ".02em",
            cursor: "pointer",
          }}
          onClick={handleClose}
        >
          Close
        </button>
        <button
          style={{
            background: "#e60000",
            color: "#fff",
            border: "none",
            borderRadius: 2,
            fontWeight: 700,
            padding: "8px 20px",
            fontSize: "14px",
            letterSpacing: ".02em",
            cursor: "pointer",
          }}
          onClick={handleSave}
        >
          Update
        </button>
      </div>
    </div>
  );
}
