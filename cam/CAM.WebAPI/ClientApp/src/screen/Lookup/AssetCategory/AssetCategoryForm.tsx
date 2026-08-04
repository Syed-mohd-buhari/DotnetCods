import React, { useState, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import {
  GetAssetCategoryCreateResource,
  CreatAssetCategory,
} from "../../../Redux/Action/LookUp/AssetCategory/AssetCategoryCreateAction";
import { deleteAssetCategory } from "../../../Redux/Action/LookUp/AssetCategory/AssetCategoryDeleteAction";
import {
  GetAssetCategoryEditResource,
  EditAssetCategory,
} from "../../../Redux/Action/LookUp/AssetCategory/AssetCategoryEditAction";
import {
  GetAssetCategoryGrid,
  GetFilterColumAssetCategory,
} from "../../../Redux/Action/LookUp/AssetCategory/AssetCategoryGridAction";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import {
  AssetCategoryQueryObjectGrid,
  AssetCategoryDto,
  AssetCategoryDtoGrid,
} from "../../../Model/LookUp/AssetCategory";
import Select from "react-select";
import { dictionaryToArray } from "../../../Hook/Dictionary";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const AssetCategoryForm: React.FC<Props> = (props) => {
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
  } = useFormTableCrud<AssetCategoryDto>(CreatAssetCategory, EditAssetCategory);

  const dtoEditResourceState = (state: RootState) =>
    state.assetCategoryEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.assetCategoryCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      setFormData(editResource);
    } else {
      // let copy = {...createResource} as AssetCategoryDto
      // copy.takeFromAssetTypeTable = true
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: AssetCategoryDtoGrid) => {
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
      copy?.idAssetClass === null ||
      copy?.idAssetClass === undefined ||
      copy?.idAssetClass === 0
    ) {
      addInvalidProperty("idAssetClass");
    }
    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  return (
    <div className="mt-4 col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Asset Category <span className="red">*</span>
                <input
                  type="text"
                  onChange={(e) => onChange("description", e)}
                  onKeyUp={(e) => onChange("description", e)}
                  className="inputForm w-100"
                  defaultValue={formData?.description}
                />
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("description") ? (
                <label className="validation">*Description is required</label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                Asset Class Related<span className="red">*</span>
                <Select
                menuPosition={"fixed"}
                  options={
                    formData?.assetClassResource &&
                    dictionaryToArray(formData?.assetClassResource)
                  }
                  value={
                    formData?.assetClassResource &&
                    dictionaryToArray(formData?.assetClassResource).filter(
                      (x) => x.key == formData?.idAssetClass
                    )
                  }
                  onChange={(e) => onChangeSelect("idAssetClass", e)}
                  isSearchable
                  isClearable
                  getOptionLabel={(option) => option.value}
                  getOptionValue={(option) => option["key"].toString()}
                ></Select>
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("idAssetClass") ? (
                <label className="validation">*Asset Class is required</label>
              ) : null}
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   d-flex align-items-center  w-100">
                <input
                  type="checkbox"
                  className="mr-1"
                  checked={formData?.takeFromAssetTypeTable}
                  onChange={(e) => onChange("takeFromAssetTypeTable", e)}
                ></input>
                Select Asset Type from dropdown<span className="red">*</span>
              </label>
              {validation &&
              validation.response === false &&
              validation.property?.includes("assetClassId") ? (
                <label className="validation">
                  *Asset Category is required
                </label>
              ) : null}
            </div>
          </div>
        </div>

        {props.edit === true ? (
          <div className="row">
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
          </div>
        ) : null}
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

export default AssetCategoryForm;
