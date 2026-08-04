import React, { useState, useCallback, useEffect } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import { Modal } from "react-bootstrap";
import { formatDateWithTime } from "../../../Hook/Common";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../../Hook/useFormTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { CreatVodafoneName } from "../../../Redux/Action/LookUp/VodafoneName/VodafoneNameCreateAction";
import { EditVodafoneName } from "../../../Redux/Action/LookUp/VodafoneName/VodafoneNameEditAction";
import {
  ProductNameObj,
  TipologicaGridDto,
} from "../../../Model/LookUp/LookUpGenericModel";
import { CommonValidation } from "../../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { dictionaryToArray } from "../../../Hook/Dictionary";
import Select from "react-select";
import { useAuth } from "../../../Hook/useAuth";
import RiskCluster from "../../../Containers/Lookup/RiskClusterContainer";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { DialogActions } from "@mui/material";
import { IoClose } from "react-icons/io5";
import { Box } from "@mui/material";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  keyTab?: string;
}

const VodafoneNameForm: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");

  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    setChanged,
    inputValue,
    setInputValue,
    promiseSelect,
  } = useFormTableCrud<TipologicaGridDto>(CreatVodafoneName, EditVodafoneName);

  const { tipologicaPermesso } = useAuth();

  const [isVisibleModalLookup, setIsVisibleModalLookup] = useState<number>(0);

  const dtoEditResourceState = (state: RootState) =>
    state.vodafoneNameEditReducer.LookUpDtoEdit;
  const dtoNewResourceState = (state: RootState) =>
    state.vodafoneNameCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  const [deletedProductNames, setDeletedProductNames] = useState<
    Array<ProductNameObj>
  >([]);

  const [unDeletedID, setUnDeletedIDS] = useState<number[]>([]);

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.edit) {
      if (editResource?.productNameResource) {
        setDeletedProductNames(
          editResource.productNameResource.filter((item) => item.enableDelete)
        );

        setUnDeletedIDS(
          editResource?.productNameResource
            .filter((item) => !item.enableDelete)
            .map((sw) => sw.id)
        );
      }

      setFormData(editResource);
    } else {
      if (createResource?.productNameResource) {
        setDeletedProductNames(
          createResource.productNameResource.filter((item) => item.enableDelete)
        );
      }
      setFormData(createResource);
    }
  }, [createResource, editResource, props.edit]);

  const validazioneClient = (copy: TipologicaGridDto) => {
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
      copy?.productNameIds === null ||
      copy?.productNameIds === undefined ||
      copy?.productNameIds?.length === 0
    ) {
      addInvalidProperty("productNameIds");
    }
    if (
      copy?.riskClusterId === null ||
      copy?.riskClusterId === undefined ||
      copy?.riskClusterId === 0
    ) {
      addInvalidProperty("riskClusterId");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props.action.closeModal(changed);
    props.action.refresh();
  };

  const OnChangeMultiSelect = (property: string, e: any) => {
    let array = [] as Array<number>;
    let copy = { ...formData } as TipologicaGridDto;
    if (e !== null && e.length > 0 && e !== undefined) {
      for (let i = 0; i < e.length; i++) {
        array.push(e[i].id);
      }

      copy[property] = array;
    } else {
      copy[property] = undefined;
    }
    setFormData(copy);

    if (validation?.property?.includes(property)) {
      let copy = { ...validation, property: [...validation.property] };
      let idxOfProperty = copy.property.indexOf(property);
      copy.property.splice(idxOfProperty, 1);
      setValidation(copy);
    }
  };

  const onChangeRiskCluster = (property: string, e: any) => {
    let copy = { ...formData } as TipologicaGridDto;
    if (e !== null && e !== undefined) {
      copy[property] = e.key;
    }
    setFormData(copy);
  };
  const RiskClusterRefillData = (value: Array<any>) => {
    var obj = value.reduce(
      (acc, item) => ({
        ...acc,
        [item.riskClusterId]: item.description,
      }),
      {}
    );
    if (formData) {
      setFormData({
        ...formData,
        riskClusterResource: obj as { [key: string]: string },
      });
    }
  };

  const ReturnLookupContainer = useCallback(
    (value: number) => {
      switch (value) {
        case 1:
          return (
            <RiskCluster
              returnObject={RiskClusterRefillData}
              modal={{ isModal: true, setIsVisibleModalLookup }}
            ></RiskCluster>
          );
        default:
          return;
      }
    },
    [isVisibleModalLookup]
  );
  return (
    <div className="mt-4 col-12">
      <Dialog
        open={isVisibleModalLookup > 0}
        onClose={() => setIsVisibleModalLookup(0)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogContent>
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <IconButton
              aria-label="close"
              onClick={() => {
                setIsVisibleModalLookup(0);
              }}
            >
              <IoClose size={25} />
            </IconButton>
          </Box>
          {ReturnLookupContainer(isVisibleModalLookup)}
        </DialogContent>
      </Dialog>
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row col-12 px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   mb-0 w-100">
                VF Name<span className="red">*</span>
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
                ) : null}
              </label>
            </div>
          </div>

          <div className="col-6 pr-0">
            {formData?.productNameResource &&
              formData?.productNameResource.filter((item) => !item.enableDelete)
                ?.length > 0 && (
                <>
                  <div className="row col-12 ml-0 pr-0">
                    <div className="form-group w-100">
                      <label className="labelForm voda-bold mb-0 w-100">
                        Associated SW App Types that can't be unlinked
                        <div className="form-group mb-0">
                          <ul>
                            {formData?.productNameResource
                              .filter((item) => !item.enableDelete)
                              .map((swItem) => (
                                <li key={swItem.id}>{swItem.description}</li>
                              ))}
                          </ul>
                        </div>
                      </label>
                    </div>
                  </div>
                </>
              )}
            <div className="row col-12 pr-0">
              <div className="form-group w-100">
                <label className="labelForm voda-bold mb-0 w-100">
                  Product Name<span className="red">*</span>
                  <div className="form-group mb-0">
                    <Select
                      menuPosition={"fixed"}
                      options={deletedProductNames}
                      value={deletedProductNames?.filter((x) => {
                        return (
                          formData &&
                          formData?.productNameIds?.indexOf(x.id) !== -1 &&
                          formData?.productNameIds?.indexOf(x.id) !== undefined
                        );
                      })}
                      onChange={(e) => OnChangeMultiSelect("productNameIds", e)}
                      isSearchable
                      isClearable
                      isMulti
                      getOptionLabel={(option) => option.description}
                      getOptionValue={(option) => option.id.toString()}
                    ></Select>
                  </div>
                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("productNameIds") ? (
                    <label className="validation">
                      *Product Name is required
                    </label>
                  ) : null}
                </label>
              </div>
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold mb-0 w-100">
                Risk Cluster<span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                      menuPosition={"fixed"}
                      options={
                        formData?.riskClusterResource &&
                        dictionaryToArray(formData?.riskClusterResource)
                      }
                      value={
                        formData?.riskClusterResource &&
                        dictionaryToArray(formData?.riskClusterResource).filter(
                          (el) => formData.riskClusterId === el.key
                        )
                      }
                      onChange={(e) => onChangeRiskCluster("riskClusterId", e)}
                      //onKeyUp={(e) => OnChangeVFName("vodafoneNameId", e)}
                      onBlur={() => setInputValue("")}
                      isSearchable
                      // isMulti
                      getOptionLabel={(option) => option.value.toString()}
                      getOptionValue={(option) => option["key"].toString()}
                    ></Select>
                  </div>
                  {validation &&
                  validation.response === false &&
                  validation.property?.includes("riskClusterId") ? (
                    <label className="validation h-6">
                      *Risk Cluster is required
                    </label>
                  ) : null}
                  {tipologicaPermesso && (
                    <button
                      className="btn btn-link"
                      onClick={() => setIsVisibleModalLookup(1)}
                      type="button"
                    >
                      <img
                        style={{ height: 15 }}
                        src={require("../../../img/plus_icon.png")}
                        alt="plus"
                      />
                    </button>
                  )}
                </div>
              </label>
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold   w-100">
                Risk Level
                <input
                  readOnly={true}
                  className="inputForm w-100 voda-regular"
                  type="text"
                  value={
                    formData?.riskClusterSeverityResource &&
                    dictionaryToArray(formData?.riskClusterSeverityResource)
                      .filter((el) => formData.riskClusterId === el.key)
                      .map((el) => el.value)
                  }
                />
              </label>
            </div>
          </div>
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
          onClick={() => {
            let uniqueArray: any[] = [];
            const dublicatedArr = [
              ...(formData?.productNameIds ? formData?.productNameIds : []),
              ...unDeletedID,
            ];

            for (let i = 0; i < dublicatedArr.length; i++) {
              if (uniqueArray.indexOf(dublicatedArr[i]) === -1) {
                uniqueArray.push(dublicatedArr[i]);
              }
            }

            Save(
              {
                ...formData,
                productNameIds: uniqueArray,
              },
              props.edit,
              validazioneClient,
              refresh
            );
          }}
          type="button"
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default VodafoneNameForm;
