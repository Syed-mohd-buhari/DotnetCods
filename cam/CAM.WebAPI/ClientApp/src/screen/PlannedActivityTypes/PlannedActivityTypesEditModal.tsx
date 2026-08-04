import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import {
  PlannedActivityTypesDtoCreate,
  PlannedActivityTypesDtoUpdate,
} from "../../Model/PlannedActivityTypes";
import { CreatPlannedActivityTypes } from "../../Redux/Action/PlannedActivityTypes/PlannedActivityTypesCreateAction";
import { EditPlannedActivityTypes } from "../../Redux/Action/PlannedActivityTypes/PlannedActivityTypesEditAction";

interface Props {
  action: {
    closeModal(): any;
    EditPlanned(item: PlannedActivityTypesDtoUpdate): any;
  };
  editData: any;
}

const PlannedActivityTypesEditModal: React.FC<Props> = (props) => {
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeSelect,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
    confirmForm,
  } = useFormTableCrud<PlannedActivityTypesDtoUpdate>(
    CreatPlannedActivityTypes,
    EditPlannedActivityTypes
  );

  useEffect(() => {
    setFormData(props?.editData);
  }, []);

  const onSubmit = () => {
    if (formData) {
      let payload = {
        forAsset: formData?.forAsset,
        forDesignAspect: formData?.forDesignAspect,
        forLcm: formData?.forLcm,
        hwOem: formData?.hwOem,
        hwPlatform: formData?.hwPlatform,
        hwSolution: formData?.hwSolution,
        linkedDcRule: formData?.linkedDcRule,
        plannedActivityTypeDescription:
          formData?.plannedActivityTypeDescription,
        plannedActivityTypesId: formData?.plannedActivityTypesId,
        subNetworkService: formData?.subNetworkService,
        swOem: formData?.swOem,
        swProductname: formData?.swProductname,
        swVersion: formData?.swVersion,
      };
      props.action.EditPlanned(payload);
    }
  };
  return (
    <div className="col-12">
      <form id="formPlannedActivityTypes" onSubmit={(e) => e.preventDefault()}>
        <div className="row col-12 w-100">
          <div className="col-12">
            <div className="row">
              <div className="col-12 px-0">
                <label className="labelForm voda-bold w-100">
                  Planned Activity Type
                  <input
                    readOnly={true}
                    className="inputForm w-100 voda-regular"
                    type="text"
                    value={formData?.plannedActivityTypeDescription}
                  />
                </label>
              </div>
            </div>
            <div className="row">
              <label className="text-bb">Enable Rules</label>
              <div className="col-12 pl-0">
                <div className="row">
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("hwOem", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.hwOem}
                      />
                      <span className="labelForm m-0">Hardware Oem</span>
                    </label>
                  </div>
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("hwSolution", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.hwSolution}
                      />
                      <span className="labelForm m-0">Hardware Solution</span>
                    </label>
                  </div>
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("hwPlatform", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.hwPlatform}
                      />
                      <span className="labelForm m-0">Hardware Platform</span>
                    </label>
                  </div>
                </div>
                <div className="row">
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("swOem", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.swOem}
                      />
                      <span className="labelForm m-0">Software Oem</span>
                    </label>
                  </div>
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("swVersion", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.swVersion}
                      />
                      <span className="labelForm m-0">Software Version</span>
                    </label>
                  </div>
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("swProductname", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.swProductname}
                      />
                      <span className="labelForm m-0">
                        Software ProductName
                      </span>
                    </label>
                  </div>
                </div>
                <div className="row">
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("subNetworkService", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.subNetworkService}
                      />
                      <span className="labelForm m-0">Sub Network Service</span>
                    </label>
                  </div>
                </div>
              </div>
            </div>
            <div className="row">
              <label className="text-bb">
                Enable Planned Activity Type for
              </label>
              <div className="col-12 pl-0">
                <div className="row">
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("forLcm", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.forLcm}
                      />
                      <span className="labelForm m-0">LCM Engineering</span>
                    </label>
                  </div>
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("forAsset", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.forAsset}
                      />
                      <span className="labelForm m-0">Asset</span>
                    </label>
                  </div>
                  <div className="col-4">
                    <label className="labelForm w-100 h-100 d-flex align-items-center">
                      <input
                        type="checkbox"
                        onChange={(e) => onChange("forDesignAspect", e)}
                        style={{ height: "15px" }}
                        className="inputForm mb-1 mr-1"
                        checked={formData?.forDesignAspect}
                      />
                      <span className="labelForm m-0">Design Aspect</span>
                    </label>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </form>
      <div className="col-12 justify-content-end mt-3 d-flex ">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={props.action.closeModal}
          type="button"
        >
          Cancel
        </button>
        <button
          className="voda-bold btn btn-danger px-4 btnHeader"
          onClick={onSubmit}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default PlannedActivityTypesEditModal;
