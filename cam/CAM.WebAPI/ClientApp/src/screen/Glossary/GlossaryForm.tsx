import React, { useState } from "react";
import "../../Css/App.css";
import { setNotification } from "../../Redux/Action/NotificationAction";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { rootStore } from "../../Redux/Store/rootStore";

const GlossaryForm = ({ cancel, submit, editItem }) => {
  const [item, setItem] = useState({
    description: editItem?.description ?? "",
    header: editItem?.header ?? "",
    glossaryItemsId: editItem?.glossaryItemsId ?? null,
  });

  const onCancel = () => {
    cancel();
  };

  const onChangeInput = (e: any) => {
    e.persist();
    if (e.target.name === "description" && e.target.value.length <= 2000) {
      setItem((oldItem) => ({ ...oldItem, [e.target.name]: e.target.value }));
    } else if (e.target.name === "header" && e.target.value.length <= 50) {
      setItem((oldItem) => ({
        ...oldItem,
        [e.target.name]: e.target.value,
      }));
    } else {
      e.preventDefault();
      return;
    }
  };
  const onSubmit = (e: any) => {
    e.preventDefault();
    if (
      item?.header !== "" &&
      item?.header &&
      item?.description !== "" &&
      item?.description
    ) {
      submit(
        item.glossaryItemsId
          ? item
          : { header: item.header, description: item.description }
      );
    } else {
      rootStore.dispatch(
        setNotification({
          message: "Enter required Fields",
          notifyType: NotifyType.warning,
        })
      );
    }
  };

  return (
    <form onSubmit={onSubmit}>
      <div className="row">
        <div className="col-6">
          <label className="labelForm voda-bold w-100">
            Header
            <span className="red">*</span>
            <input
              type="text"
              className="inputForm w-100"
              value={item.header}
              name="header"
              onChange={onChangeInput}
            />
          </label>
        </div>
        <div className="col-12">
          <label className="labelForm voda-bold w-100">
            Description
            <span className="red">*</span>
            <textarea
              className="inputForm w-100"
              name="description"
              value={item.description}
              onChange={onChangeInput}
            />
          </label>
        </div>
        <div className="col-12 justify-content-end pr-4 d-flex footerModal">
          <button
            className="voda-bold btn btn-link px-4 btnHeader cancel"
            type="button"
            onClick={onCancel}
          >
            Cancel
          </button>
          <button
            className="voda-bold btn btn-danger px-4 btnHeader"
            type="submit"
          >
            Submit
          </button>
        </div>
      </div>
    </form>
  );
};

export default GlossaryForm;
