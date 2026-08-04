import React from "react";

interface Props {
  title: string;
  value: string | any;
}

const LabelViewFormString: React.FC<Props> = (props) => {
  return (
    <div className="form-group col-4">
      <label className="labelForm voda-bold text-uppercase w-100">
        {props?.title}
      </label>
      {props?.title === "Subnetwork Boundary" ? (
        props?.value?.length ? (
          props?.value.map((x, index) => (
            <p
              key={index}
              dangerouslySetInnerHTML={{ __html: x?.subNetworkBoundaryName }}
              className="w-100"
            ></p>
          ))
        ) : (
          ""
        )
      ) : (
        <label
          dangerouslySetInnerHTML={{ __html: props?.value }}
          className="w-100"
        ></label>
      )}
    </div>
  );
};

export default LabelViewFormString;
