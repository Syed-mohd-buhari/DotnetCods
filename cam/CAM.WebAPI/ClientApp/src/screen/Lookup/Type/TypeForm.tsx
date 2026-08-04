import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";
import { dictionaryToArray } from "../../../Hook/Dictionary";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatType } from "../../../Redux/Action/LookUp/Type/TypeCreateAction";
import { EditType } from "../../../Redux/Action/LookUp/Type/TypeEditAction";
import { TypeDto, TypeDtoGrid } from "../../../Model/LookUp/Type";
import Select from "react-select";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { GetClasssesByCategoryId } from "../../../Redux/Action/LookUp/Class/ClassCreateAction";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const TypeForm: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");

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
  } = useFormTableCrud<TypeDto>(CreatType, EditType);

  const dtoEditResourceState = (state: RootState) =>
    state.TypeEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.TypeCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      console.log(createResource,"createResource Type")
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: TypeDto) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.description === null ||
      copy?.description === undefined ||
      copy?.description.trim() === ""
    ) {
      addInvalidProperty("description");
    }
    if (
      copy?.classId === null ||
      copy?.classId === undefined ||
      !copy?.classId

    ) {
      addInvalidProperty("classId");
    }
    if (
      copy?.categoryId === null ||
      copy?.categoryId === undefined ||
      !copy?.categoryId

    ) {
      addInvalidProperty("categoryId");
    }
    console.log(copy.classId,"coppy")
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };
useEffect(()=>{
  if(formData?.categoryId){
    let copy = { ...formData } as TypeDto;

  GetClasssesByCategoryId(formData?.categoryId!).then((res)=>{
    copy.classResource=res?.data
    setFormData(copy)
  })
  }
  
},[formData?.categoryId])
  return (
    <div className="mt-4 col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Type<span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("description", e)}
                  onKeyUp={(e) => onChange("description", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.description}
                />
              {validation &&
              validation.response === false &&
              validation.property?.includes("description") ? (
                <label className="validation">*Description is required</label>
              ) : null }
              </label>

            </div>
          </div>
          <div className="col-6">
                <div className="col-12 pr-0 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    Category<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.categoryResource &&
                            dictionaryToArray(formData?.categoryResource)
                          }
                          value={
                            formData?.categoryResource &&
                            dictionaryToArray(
                              formData?.categoryResource
                            ).filter((x) => x.key == formData?.categoryId)
                          }
                          onChange={(e) => onChangeSelect("categoryId", e)}
                          placeholder="Select Category"
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                    
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("categoryId") ? (
                      <label className="validation">
                        *Category must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
              <div className="col-6">
                <div className="col-12 pr-0 pl-0">
                  <label className="labelForm voda-bold w-100 mb-20">
                    Class<span className="red">*</span>
                    <div className="d-flex">
                      <div className="w-100">
                        <Select
                        menuPosition={"fixed"}
                          options={
                            formData?.classResource&&
                            dictionaryToArray(formData?.classResource)
                          }
                          value={
                            formData?.classResource &&
                            dictionaryToArray(
                              formData?.classResource
                            ).filter((x) => x.key == formData?.classId)
                          }
                          onChange={(e) => onChangeSelect("classId", e)}
                          placeholder="Select Category"
                          isSearchable
                          isClearable
                          getOptionLabel={(option) => option.value}
                          getOptionValue={(option) => option["key"].toString()}
                        ></Select>
                      </div>
                    
                    </div>
                    {validation &&
                    validation.response == false &&
                    validation.property?.includes("classId") ? (
                      <label className="validation">
                        *Class must have a value
                      </label>
                    ) : null}
                  </label>
                </div>
              </div>
          <div className="col-12">
            <div className="row">
              {props.edit === true ? (
                <div className="col-6">
                  <div className="form-group">
                    <label className="labelForm voda-bold   w-100">
                      Last Modified
                      <input
                        readOnly={true}
                        className="inputForm w-100 voda-regular"
                        type="text"
                        value={formatDateWithTime(
                          formData?.lastModified
                        )?.toUpperCase()}
                      />
                    </label>
                  </div>
                </div>
              ) : null}
              {props.edit === true ? (
                <div className="col-6">
                  <div className="form-group">
                    <label className="labelForm voda-bold   w-100">
                      Last Modified By
                      <input
                        readOnly={true}
                        className="inputForm w-100 voda-regular"
                        type="text"
                        value={formData?.lastModifiedBy}
                      />
                    </label>
                  </div>
                </div>
              ) : null}
            </div>
          </div>
        </div>
      </form>
      <div className="col-12 justify-content-end mt-3 d-flex ">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="  voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => Save(formData, props.edit, validazioneClient, refresh)}
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default TypeForm;
