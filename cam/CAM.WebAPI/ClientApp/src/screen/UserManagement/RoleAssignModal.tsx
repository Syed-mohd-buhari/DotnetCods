import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import { useSelector } from "react-redux";

import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { RootState } from "../../Redux/Store/rootStore";
import Select from "react-select";
import { CommonValidation } from "../SettingsUpdatePlannedActivity/SettingsUpdatePlannedActivityModal";
import { CreateUserManagementRole } from "../../Redux/Action/UserManagement/UserManagementGridAction";
import { UserManagementRoleDtoGrid } from "../../Model/UserManagement";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { MultiSelect } from "react-multi-select-component";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
    refresh(): any;
  };
  edit: boolean;
  userInfo: any;
  userRoleId: any;
  role: any;
  opCo: any;
  verticalRes: any;
  keyTab?: string;
  rovList: any;
  allROVList: any;
  roleOptions: any;
  opCoOptions: any;
  verticalResOptions: any;
  editUserDetails: any;
  rules?: { key: number; value: string }[];
}

const RoleAssignModal: React.FC<Props> = (props) => {
  const [keyTabs, setKey] = useState("Lookup");
  const [roleSelected, setRoleSelected] = useState<any>(
    props?.edit && props?.role
  );
  const [opCoSelected, setOpCoSelected] = useState<
    { label: string; value: number }[] | undefined
  >(undefined);
  const [verticalResSelected, setVerticalResSelected] = useState<any>(
    props?.edit && props?.verticalRes
  );

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
  } = useFormTableCrud<UserManagementRoleDtoGrid>(
    CreateUserManagementRole,
    CreateUserManagementRole
  );

  const dtoEditResourceState = (state: RootState) =>
    state.userManagementRoleCreateReducer.LookUpDtoCreate;
  const dtoNewResourceState = (state: RootState) =>
    state.userManagementRoleCreateReducer.LookUpDtoCreate;

  let createResource = useSelector(dtoNewResourceState);
  let editResource = useSelector(dtoEditResourceState);

  //UPDATE ON CHANGE DTO
  // useEffect(() => {
  //   setFormData(createResource);
  // }, [createResource, editResource]);

  useEffect(() => {
    if (props.edit && props.opCo) {
      setOpCoSelected(
        dictionaryToArray(props.allROVList.opCoResource)
          .filter((x) => props.opCo.value === x.value)
          .map((item) => ({ label: item.value, value: item.key }))
      );
    }
  }, [props]);

  const validazioneClient = (copy: UserManagementRoleDtoGrid) => {
    let copyValidation = { response: true, property: [] } as CommonValidation;

    const addInvalidProperty = (property: string) => {
      copyValidation?.property?.push(property);
      copyValidation.response = false;
    };

    if (
      copy?.role === null ||
      copy?.role === undefined ||
      copy?.role.trim() === ""
    ) {
      addInvalidProperty("role");
    }

    setValidation(copyValidation);
    return copyValidation;
  };

  //REFRESH DATI PAGINA
  const refresh = (changed: boolean) => {
    props?.action.closeModal(changed);
    props?.action.refresh();
  };

  const SaveUserRole = () => {
    const result: any = [];

    // Get role keys
    const roleKeys = props?.allROVList?.roleResource
      ? dictionaryToArray(props.allROVList.roleResource)
          .filter((item) => roleSelected?.value === item.value)
          .map((item) => item.key)
      : [];

    // Get opco keys
    const opCoKeys = props?.allROVList?.opCoResource
      ? dictionaryToArray(props.allROVList.opCoResource)
          .filter((item) =>
            props?.edit && opCoSelected
              ? opCoSelected[0].value === item.key
              : opCoSelected?.some(
                  (x) =>
                    JSON.stringify({ label: item.value, value: item.key }) ===
                    JSON.stringify(x)
                )
          )
          .map((item) => item.key)
      : [];

    // Get vertical keys
    const verticalKeys = props?.allROVList?.verticalResource
      ? dictionaryToArray(props.allROVList.verticalResource)
          .filter((item) =>
            props?.edit
              ? verticalResSelected?.value === item.value
              : verticalResSelected.some(
                  (x) =>
                    JSON.stringify({ label: item.value, value: item.value }) ===
                    JSON.stringify(x)
                )
          )
          .map((item) => item.key)
      : [];

    // Create payloads
    for (const rol of roleKeys) {
      for (const opco of opCoKeys) {
        for (const ver of verticalKeys) {
          const payload = { ...props?.userInfo };
          if (props?.edit)
            payload.aspNetUserRoleId = props?.editUserDetails?.aspNetUserRoleId;
          payload.roleId = rol;
          payload.opcoId = opco;
          payload.verticalResponsibleId = ver;
          result.push(payload);
        }
      }
    }

    CreateUserManagementRole(result).then((x) => {
      if (x) {
        props?.action?.closeModal();
        props?.action?.refresh();
      }
    });
  };
  return (
    <div className="col-12">
      <form
        id="formDesignComponent"
        onChange={() => setChanged(true)}
        onSubmit={(e) => e.preventDefault()}
      >
        <div className="row px-0">
          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold w-100 mb-0">
                Role<span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                    menuPosition={"fixed"}
                      options={dictionaryToArray(
                        props?.allROVList?.roleResource
                      ).map((item) => ({
                        label: item.value,
                        value: item.value,
                      }))}
                      value={roleSelected}
                      onChange={setRoleSelected}
                    />
                  </div>
                </div>
              </label>
            </div>
          </div>
          <div className="col-6">
            <div className="form-group">
              <label className="w-100 voda-bold text-left mb-0">
                OpCo<span className="red">*</span>
              </label>
              <div className="flex PA">
                {props.edit ? (
                  <Select
                  menuPosition={"fixed"}
                    className="w-100"
                    options={
                      props?.allROVList?.opCoResource &&
                      dictionaryToArray(props?.allROVList?.opCoResource).map(
                        (item) => ({
                          label: item.value,
                          value: item.key,
                        })
                      )
                    }
                    value={opCoSelected ? opCoSelected : null}
                    onChange={(e: any) => {
                      e && setOpCoSelected([e]);
                    }}
                  />
                ) : (
                  <MultiSelect
                    className="w-100"
                    options={
                      props?.allROVList?.opCoResource
                        ? dictionaryToArray(
                            props?.allROVList?.opCoResource
                          ).map((item) => ({
                            label: item.value,
                            value: item.key,
                          }))
                        : []
                    }
                    value={opCoSelected ?? []}
                    onChange={setOpCoSelected}
                    labelledBy="Select"
                  />
                )}
              </div>
            </div>
          </div>

          <div className="col-6">
            <div className="form-group">
              <label className="labelForm voda-bold w-100 mb-0">
                Vertical Responsible<span className="red">*</span>
                <div className="d-flex">
                  <div className="w-100">
                    <Select
                    menuPosition={"fixed"}
                      options={dictionaryToArray(
                        props?.allROVList?.verticalResource
                      ).map((item) => ({
                        label: item.value,
                        value: item.value,
                      }))}
                      value={verticalResSelected}
                      isMulti={props?.edit ? false : true}
                      onChange={setVerticalResSelected}
                    />
                  </div>
                </div>
              </label>
            </div>
          </div>
        </div>
      </form>
      <div className="col-12 justify-content-end d-flex ">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => props?.action.closeModal(changed)}
          type="button"
        >
          Cancel
        </button>
        <button
          className="voda-bold btn btn-danger px-4 btnHeader"
          onClick={() => SaveUserRole()}
          disabled={
            roleSelected && opCoSelected && verticalResSelected ? false : true
          }
          type="button"
        >
          Submit
        </button>
      </div>
    </div>
  );
};

export default RoleAssignModal;
