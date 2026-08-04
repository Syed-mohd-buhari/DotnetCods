import React, { useEffect, useState } from "react";

interface Props {
  redirect?: string;
  bagId?: number | null;
  bagName?: string;
  buildBagResources?: Array<{ key: number; text: string }> | null;
  mappedComponentDetails?: any;
  modal?: {
    isModal: boolean | false;
    setViewBagFlag(flag: boolean): any;
  };
}

const ViewMappedComponent = (props: Props) => {
  const mappedData = props?.mappedComponentDetails
    ? props?.mappedComponentDetails
    : props?.buildBagResources
        ?.filter((res) => res.key === props?.bagId)
        ?.map((res: any) => res.mappedComponentDetails)[0];
  const updatedBagName = props?.bagName
    ? props?.bagName
    : props?.buildBagResources?.filter((res) => res.key === props?.bagId)?.[0]
        ?.text;

  const onCloseModal = () => {
    props.modal && props.modal.setViewBagFlag(false);
  };

  return (
    <>
      <fieldset className="fieldset">
        <div className="row">
          <div className="form-group col-12">
            <div className="col-8 pl-0">
              <label className={"labelForm  voda-bold w-100 mb-1"}>
                {props?.redirect === "lcm"
                  ? "Current Bag"
                  : props?.redirect === "lcmPA" || props?.redirect === "asset"
                  ? "Planned Bag"
                  : "Bag Name"}
                <span className="red">*</span>
              </label>
              <label className={`labelForm voda-bold `}>
                <div
                  style={{
                    display: "flex",
                    alignItems: "center",
                    border: "1px solid #ccc",
                    borderRadius: "4px",
                    width: "100% !important",
                  }}
                >
                  <span
                    style={{
                      padding: "10.5px 10px",
                      height: "2.8rem",
                      width: "100%",
                      background: "#eee", // Same as disabled input background
                      color: "#6c757d", // Disabled text color
                    }}
                  >
                    {updatedBagName}
                  </span>
                </div>
              </label>
            </div>
            <div className="col-12 pl-0">
              <legend className="voda-bold mb-4 fz-18">
                Software Component Details
              </legend>
              <div className="row col-12 mb-4 pb-3">
                <table className="w-80 ml-5">
                  <thead>
                    <tr className="mt-4 head">
                      <th className="pl-2 ptb-12">Software Component</th>
                    </tr>
                  </thead>
                  <tbody>
                    {mappedData?.map((item, i) => (
                      <tr className={`dati`} key={i}>
                        <td>{item.text}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </fieldset>
      {props.modal && props.modal.isModal ? (
        <div className="col-12 justify-content-end d-flex footerModal">
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            type="button"
            onClick={() => onCloseModal()}
          >
            Close
          </button>
        </div>
      ) : null}
    </>
  );
};

export default ViewMappedComponent;
