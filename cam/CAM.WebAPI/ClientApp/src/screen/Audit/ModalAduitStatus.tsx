import React, { useEffect, useState } from "react";
import { Modal } from "react-bootstrap";
import "../../Css/NetworkElement.css";
import "../../Css/NavBar.css";
import "../../Css/index.css";

export interface Props {
  show: boolean;
  modalType?: string;
  data?: any | undefined;
  action: {
    closeModal(): any;
    Override: any;
  };
}

const ModalAduitStatus: React.FC<Props> = (props) => {
  let exitButtonClass = [" ", "voda-bold", "btn", "ml-3", "px-4", "btnHeader"];
  const [value, setValue] = useState<string | null>("");
  const [error, setError] = useState<boolean>(false);
  const [editKeyName, setEditKeyName] = useState<boolean>(true);
  const propsValue =  props?.data[0]?.newValue;

  useEffect(()=>{
    if(props?.data){
        setValue(propsValue);
        propsValue !== value ? setError(false) : setError(true)
    }
  },[props?.data])

  const onChangeValue = (e) => {
    setValue(e.target.value)
    propsValue.toString() === e.target.value?.toString() ? setError(true) : setError(false)
  }

  const Submit = () => {
    if(propsValue !== value) {
        setError(false) 
        let payload = {
            "Audithistoryid": [props?.data[0]?.auditHistoryId], "Newvalue": [value]
        }
        props.action.Override(payload)
        props.action.closeModal();
    } else {
        setError(true);
    }
    
  }

  return (
    <Modal
      show={props.show}
      backdrop="static"
      backdropClassName="backdropRelatedModal"
      dialogClassName="dialogRelatedModal"
      className="dialogRelatedModal"
      keyboard={false}
      size="xl"
      centered
      onHide={() => {
        props.action.closeModal();
      }}
    >
      <Modal.Header className="d-flex justify-content-center" closeButton>
        <div className="col-12 px-0">
          <div className="col-12">
            <h4 className="mb-0">
              Audit Override
            </h4>
          </div>
        </div>
      </Modal.Header>
      <Modal.Body>
      <div className="row px-0 mx-0">
        <div className="col-3 mb-4">
          <div className="col-12 pl-0">
            <div className="d-flex">
              <label className="labelForm voda-bold mb-0"> OpCo : </label>
              <label className="labelForm mb-0 pl-1">
                {props?.data[0]?.opCo || "Invalid"}
              </label>
            </div>
          </div>
        </div>
        <div className="col-3 mb-4">
          <div className="col-12 pl-0">
            <div className="d-flex">
              <label className="labelForm voda-bold mb-0"> Oem : </label>
              <label className="labelForm mb-0 pl-1">
                {props?.data[0]?.oem || "Invalid"}
              </label>
            </div>
          </div>
        </div>
        <div className="col-3 mb-4">
          <div className="col-12 pl-0">
            <div className="d-flex">
              <label className="labelForm voda-bold mb-0"> Element Name : </label>
              <label className="labelForm mb-0 pl-1">
                {props?.data[0]?.elementName || "Invalid"}
              </label>
            </div>
          </div>
        </div>
      </div>
      <div className="row col-12 px-0 mx-0 d-flex">
        <div className="col-3 mb-4">
          <div className="col-12 pl-0">
            <div className="">
              <label className="labelForm voda-bold mb-0"> Attribute  </label>
              {/* <label className="labelForm mb-0 pl-1">
                {props?.data[0]?.columnName || "Invalid"}
              </label> */}
              <div className="d-flex">
                <label className="labelForm voda-bold mb-0 w-90">
                    <input
                        type="text"
                        className="inputForm w-100"
                        value={props?.data[0]?.columnName || "Invalid"}
                        disabled
                    />
                </label>
            </div>
            </div>
          </div>
        </div>
        <div className="col-3 mb-4">
          <div className="col-12 pl-0">
            <div className="">
              <label className="labelForm voda-bold mb-0"> Old Value  </label>
              {/* <label className="labelForm mb-0 pl-1">
                {props?.data[0]?.oldValue || "Invalid"}
              </label> */}
              <div className="d-flex">
                <label className="labelForm voda-bold mb-0 w-90">
                    <input
                        type="text"
                        className="inputForm w-100"
                        value={props?.data[0]?.oldValue || ""}
                        disabled
                    />
                </label>
            </div>
            </div>
          </div>
        </div>
        <div className="col-6 mb-4">
          <div className="col-12 pl-0">
            <label className="labelForm voda-bold mb-0">
              New Value<span className="red">*</span> (to Override)
            </label>
            <div className="d-flex">
                <label className="labelForm voda-bold mb-0 w-90">
                    <input
                        onChange={(e) => onChangeValue(e)}
                        type="text"
                        className="inputForm w-100"
                        value={value || ""}
                        required
                    />
                </label>
            </div>
            {error && 
                <label className="validation h-16">
                    *Override value should not be similar to existing value - "{value}".
                </label>
            }
          </div>
        </div>
      </div>
      </Modal.Body>
      <Modal.Footer>
        <div className="col-12 justify-content-end mt-4 d-flex footerModal">
            <button
                className={exitButtonClass.join(" ")}
                onClick={() => props.action.closeModal()}
                type="button"
            >
                Close
            </button>
            <button
                className={` voda-bold btn btn-danger ml-3 px-4 btnHeader ${error ? "disabledCursor" : "pointer"}`}
                type="button"
                disabled={error}
                onClick={()=> Submit()}
            >
                Override
            </button>
        </div>
      </Modal.Footer>
    </Modal>
  );
};

export default ModalAduitStatus;
