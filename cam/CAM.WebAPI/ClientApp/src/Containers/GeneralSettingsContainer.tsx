import React, { useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";

import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { CustomGridRender } from "../Model/Common";
import setLoader from "../Redux/Action/LoaderAction";

import { RootState } from "../Redux/Store/rootStore";
import {
  GeneralSettingsDtoGrid,
  GeneralSettingsDtoUpdate,
  GeneralSettingsQueryObjectGrid,
} from "../Model/GeneralSettingsModal";
import GeneralSettingsGrid from "../screen/GeneralSettings/GeneralSettingsGrid";
import { GetGeneralSettingsGrid } from "../Redux/Action/GeneralSettings/GeneralSettingsGrid";
import { useAuth } from "../Hook/useAuth";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { GeneralSettingsApi } from "../Business/GeneralSettingsBusiness";
import { TextInputComponent } from "../Components/FormField";
import Paginate from "../Components/PaginationComponent";
import { EditGeneralSettings } from "../Redux/Action/GeneralSettings/EditGeneralSettings";
import { verifyPermesso } from "../Redux/Action/AuthenticationCheckAction";

const paginationQuery: GeneralSettingsQueryObjectGrid = {
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  lastModifiedValue: undefined,
  principalId: undefined,
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
  appSettingsConfiguartionId: [],
  appSettingsId: [],
  settingsValue: [],
};

const GeneralSettingsContainer = () => {
  const [data, setData] = useState<GeneralSettingsDtoGrid[] | undefined>([]);
  const [renderGridState, setRenderGridState] = useState<any>();
  const [orphanColor, setOrphanColor] = useState(false);
  const [editRowData, setEditRowData] =
    useState<GeneralSettingsDtoUpdate | null>(null);
  const [validationError, setValidationError] = useState<string | undefined>(
    ""
  );

  const { readonly, isPermesso, pageSize } = useAuth();
  const navigate = useNavigate();
  const location: any = useLocation();

  const GridDto = useSelector(
    (state: RootState) => state.generalSettingsReducer.GeneralSettingsGridResult
  );

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const { query, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetGeneralSettingsGrid : undefined
  );

  const api = new GeneralSettingsApi();

  const getEditData = async (id: number) => {
    setLoader("ADD", "GetGeneralSettingsEditResource");
    const result = await api.generalSettingsGetGrid({ appSettingsId: [id] });
    const singleItem = result?.items?.[0];
    if (singleItem) {
      setEditRowData(singleItem);
    }
    setLoader("REMOVE", "GetGeneralSettingsEditResource");
  };

  const { Edit, isVisibleModal, closeModal } = useOperationTableCrud<
    any,
    GeneralSettingsDtoUpdate
  >(
    async () => {}, // Not used here
    getEditData,
    async () => ({ warning: false }), // Dummy delete function
    () => {} // No post-delete action
  );

  useEffect(() => {
    if (GridDto) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetGeneralSettingsGrid");
    }
  }, [GridDto]);

  const handleInputChange = (
    key: keyof GeneralSettingsDtoUpdate,
    value: string
  ) => {
    if (validationError && value.trim() !== "") {
      setValidationError(undefined);
    }
    setEditRowData((prev) => (prev ? { ...prev, [key]: value } : prev));
  };

  const handleSave = async () => {
    if (
      !editRowData?.settingsValue ||
      editRowData.settingsValue.trim() === ""
    ) {
      setValidationError("Value must have a value.");
      return;
    }
    const value = Number(editRowData.settingsValue);

    if (isNaN(value)) {
      setValidationError("Value must be a valid number.");
      return;
    }

    // Check if the value is greater than 1 and less than 100
    if (value <= 1 || value >= 100) {
      setValidationError("Value must be greater than 1 and less than 100.");
      return;
    }
    setValidationError(undefined);
    const updatedData = {
      ...editRowData,
      settingsValue: String(editRowData.settingsValue).trim(),
    };
    // Save the edited general settings
    await EditGeneralSettings(updatedData);
    await verifyPermesso();
    // Reload the data by calling GetGeneralSettingsGrid directly
    setLoader("ADD", "GetGeneralSettingsGrid");
    await GetGeneralSettingsGrid(paginationQuery); // You can pass the existing paginationQuery if required

    // Close the modal after saving
    closeModal();
  };

  return (
    <div className="pageContainer">
      <Dialog
        open={isVisibleModal}
        onClose={() => closeModal()}
        maxWidth="md"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 mt-3">
            <h4>Edit No. of records per page</h4>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => closeModal()}
          sx={{ position: "absolute", right: 8, top: 8 }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          {editRowData ? (
            <div>
              <div className="col-6">
                <div className="form-group">
                  <TextInputComponent
                    label="Value"
                    required={true}
                    labelCSS="mb-0"
                    inputCSS="labelForm voda-bold mb-2"
                    value={editRowData.settingsValue}
                    onChange={(e: React.ChangeEvent<HTMLTextAreaElement>) =>
                      handleInputChange("settingsValue", e.target.value)
                    }
                    isError={!!validationError}
                    error={validationError}
                  />
                </div>
              </div>
              <div className="col-12 justify-content-end d-flex">
                <button
                  className="voda-bold btn btn-link px-4 btnHeader cancel"
                  onClick={() => closeModal()}
                  type="button"
                >
                  Cancel
                </button>
                <button
                  className="voda-bold btn btn-danger px-4 btnHeader"
                  type="button"
                  onClick={handleSave}
                >
                  Save
                </button>
              </div>
            </div>
          ) : (
            <p>Loading...</p>
          )}
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">Customize Page Size</h3>
        </div>
      </div>

      <GeneralSettingsGrid
        data={data}
        orphanColor={orphanColor}
        renderGrid={renderGridState?.render ?? []}
        action={{ Edit }}
      />

      <Paginate
        pagination={{ page: query.page, pageSize: query.pageSize }}
        totalItems={GridDto?.totalItems}
        actions={{ next, back }}
      />
    </div>
  );
};

export default GeneralSettingsContainer;
