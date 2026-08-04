import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import "../../Css/Toggle.css";
import { formatDateWithTime, formatTimeLocal } from "../../Hook/Common";
import {
  dictionaryToArray,
  dictionaryToArrayAssetCategoryDto,
} from "../../Hook/Dictionary";

import { InizializeNewProductCreateDto } from "../../Model/InizializeNewProduct";
import { CreateInizializeNewProduct } from "../../Redux/Action/InizializeNewProduct/InizializeNewProductCreateAction";

import LabelViewFormString from "../../Components/LabelViewFormString";
import { rootStore } from "../../Redux/Store/rootStore";

interface Props {
  action: {
    wizardBackFunction?(formData: InizializeNewProductCreateDto): any;
    setConfirmExitWizard(override?: boolean): any;
  };

  dataWizard: InizializeNewProductCreateDto | null;
  isBack?: boolean;
}

const ReviewNewProductModal: React.FC<Props> = (props) => {
  const [formData, setFormData] =
    useState<InizializeNewProductCreateDto | null>(props.dataWizard);

  const [netWorkFunctions, setNetworkFunctions] = useState(
    formData?.majorSoftwareBuildDto.networkFunctionsResource &&
      dictionaryToArray(
        formData?.majorSoftwareBuildDto?.networkFunctionsResource
      ).filter((el) =>
        formData?.majorSoftwareBuildDto.networkFunctionsIds?.includes(el.key)
      )
  );

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (props.dataWizard !== undefined) {
      setFormData(props.dataWizard);
    }
  }, [props.dataWizard]);

  const submit = async () => {
    if (formData) {
      await CreateInizializeNewProduct({
        ...formData,
        designComponentDto: {
          ...formData?.designComponentDto,
          supportedAllServices:
            formData?.designComponentDto?.subNetworkBoundaryIds?.includes(-1)!,
          subNetworkBoundaryIds:
            formData?.designComponentDto?.subNetworkBoundaryIds?.includes(-1)!
              ? formData?.designComponentDto?.subNetworkBoundaryIds?.filter(
                  (item) => item !== -1
                )
              : formData?.designComponentDto?.subNetworkBoundaryIds,
        },
      }).then((x) => {
        if (x && !x.warning) {
          rootStore.dispatch({ type: "REFRESH", payload: true });
          props.action.setConfirmExitWizard(true);
          rootStore.dispatch({ type: "REFRESH", payload: false });
        }
      });
    }
  };

  const subDomainSpocs =
    formData?.systemTypeDto.subDomainSpocResource &&
    dictionaryToArray(formData?.systemTypeDto.subDomainSpocResource).filter(
      (x) => {
        return (
          formData.systemTypeDto.subDomainSpocIds &&
          formData.systemTypeDto.subDomainSpocIds.indexOf(x.key) != -1 &&
          formData.systemTypeDto.subDomainSpocIds.indexOf(x.key) != undefined
        );
      }
    );

  return (
    <div className="mt-4 col-12 p-0">
      {/* --------------------------SOFTWARE SECTION ----------------------------------- */}
      <div className="row col-12 px-0 mx-0">
        <label className="voda-bold labelFinalPreview">Final Preview</label>
        <fieldset className="col-12 mt-3 row mx-0">
          <legend className="text-bb mb-40">Software Details</legend>
          <div className="col-12 px-2 d-flex row p-0">
            <LabelViewFormString
              title="Equipment Manufacturer"
              value={
                (formData?.majorSoftwareBuildDto
                  .originalEquipmentManufacturerResource &&
                  dictionaryToArray(
                    formData?.majorSoftwareBuildDto
                      ?.originalEquipmentManufacturerResource
                  ).find(
                    (el) =>
                      el.key ==
                      formData?.majorSoftwareBuildDto
                        .originalEquipmentManufacturerId
                  )?.value) ||
                "---"
              }
            />
            <LabelViewFormString
              title="Product Name"
              value={formData?.majorSoftwareBuildDto.productName || "---"}
            />

            <div className="form-group col-4">
              <label className="labelForm voda-bold   w-100">
                General Availability Date
              </label>
              <label className="  w-100">
                {formatTimeLocal(
                  formData?.majorSoftwareBuildDto.generaAvailableDate
                ) || "NOT SPECIFIED"}
              </label>
            </div>

            <LabelViewFormString
              title="One Track"
              value={
                formData?.majorSoftwareBuildDto.deliveryMethod == "One Track"
                  ? "Yes"
                  : "No"
              }
            />
            <LabelViewFormString
              title="Software Application"
              value={formData?.majorSoftwareBuildDto.softwareVersion || "---"}
            />

            <div className="form-group col-4">
              <label className="labelForm voda-bold   w-100">
                End of Maintenance
              </label>
              <label className="w-100">
                {formData?.majorSoftwareBuildDto.eomStatus === 0
                  ? "NOT ANNOUNCED"
                  : formData?.majorSoftwareBuildDto.eomStatus === 1
                  ? "NOT SPECIFIED"
                  : formatTimeLocal(
                      formData?.majorSoftwareBuildDto.endOfMaintenance!
                    )}
              </label>
            </div>

            <div className="form-group col-4">
              <label className="labelForm voda-bold   w-100">
                End of Support
              </label>
              <label className="  w-100">
                {formatTimeLocal(
                  formData?.majorSoftwareBuildDto.endOfsupport
                ) || "---"}
              </label>
            </div>

            <div className="form-group col-4">
              <label className="labelForm voda-bold   w-100">
                Last time buy
              </label>
              <label className="  w-100">
                {formatTimeLocal(
                  formData?.majorSoftwareBuildDto.lastTimeBuyNew
                ) || "---"}
              </label>
            </div>

            <LabelViewFormString
              title="Operating System"
              value={
                (formData?.majorSoftwareBuildDto.operatingSystemResource &&
                  dictionaryToArray(
                    formData?.majorSoftwareBuildDto.operatingSystemResource
                  ).find(
                    (el) =>
                      el.key ==
                      formData?.majorSoftwareBuildDto.operatingSystemId
                  )?.value) ||
                "---"
              }
            />
            <LabelViewFormString
              title="Vulnerability Status"
              value={
                formData?.majorSoftwareBuildDto.vulnerabilityStatus || "---"
              }
            />

            <LabelViewFormString
              title="Functional Entity"
              value={
                netWorkFunctions?.length
                  ? netWorkFunctions.map((item) => item.value)
                  : "---"
              }
            />

            <LabelViewFormString
              title="Description"
              value={formData?.majorSoftwareBuildDto.description || "---"}
            />
          </div>
        </fieldset>
      </div>

      {/* --------------------------HARDWARE SECTIONNN ----------------------------------- */}
      <div className="row col-12 px-0 mx-0">
        <fieldset className="col-12 mt-3 row mx-0">
          <legend className="text-bb mb-40">Hardware Details</legend>
          <div className="col-12 px-2 d-flex row p-0">
            {formData?.majorHardwareBuildDto?.originalEquipmentManufacturerId !=
            0 ? (
              <LabelViewFormString
                title="Equipment Manufacturer"
                value={
                  (formData?.majorHardwareBuildDto
                    ?.originalEquipmentManufacturerResource &&
                    dictionaryToArray(
                      formData?.majorHardwareBuildDto
                        ?.originalEquipmentManufacturerResource
                    ).find(
                      (el) =>
                        el.key ==
                        formData?.majorHardwareBuildDto
                          ?.originalEquipmentManufacturerId
                    )?.value) ||
                  "---"
                }
              />
            ) : null}

            {formData?.majorHardwareBuildDto?.buildConstructionId != 0 ? (
              <LabelViewFormString
                title="Build Construction"
                value={
                  (formData?.majorHardwareBuildDto?.buildConstructionResource &&
                    dictionaryToArray(
                      formData?.majorHardwareBuildDto?.buildConstructionResource
                    ).find(
                      (el) =>
                        el.key ==
                        formData?.majorHardwareBuildDto?.buildConstructionId
                    )?.value) ||
                  "---"
                }
              />
            ) : null}

            {formData?.majorHardwareBuildDto?.platformId != 0 ? (
              <LabelViewFormString
                title="Platform"
                value={
                  (formData?.majorHardwareBuildDto?.platformResource &&
                    dictionaryToArray(
                      formData?.majorHardwareBuildDto?.platformResource
                    ).find(
                      (el) =>
                        el.key == formData?.majorHardwareBuildDto?.platformId
                    )?.value) ||
                  "---"
                }
              />
            ) : null}

            <div className="form-group col-4">
              <label className="labelForm voda-bold   w-100">
                Hardware Solution
              </label>
              {formData?.majorHardwareBuildDto?.hardwareSolution ? (
                <label
                  className="w-100"
                  dangerouslySetInnerHTML={{
                    __html: formData?.majorHardwareBuildDto?.hardwareSolution,
                  }}
                ></label>
              ) : (
                <label className="w-100">---</label>
              )}
            </div>

            <div className="form-group col-4">
              <label className="labelForm voda-bold   w-100">
                Hardware Type
              </label>
              {formData?.majorHardwareBuildDto?.hardwareType ? (
                <label
                  className="w-100"
                  dangerouslySetInnerHTML={{
                    __html: formData?.majorHardwareBuildDto?.hardwareType,
                  }}
                ></label>
              ) : (
                <label className="w-100">---</label>
              )}
            </div>

            <LabelViewFormString
              title="Other Hw Info"
              value={
                formData?.majorHardwareBuildDto?.otherHardwareInfo || "---"
              }
            />

            <div className="form-group col-4">
              <label className="labelForm voda-bold w-100">
                General Availability Date
              </label>
              <label className="  w-100">
                {formatTimeLocal(
                  formData?.majorHardwareBuildDto.generaAvailableDate
                ) || "NOT SPECIFIED"}
              </label>
            </div>

            <div className="form-group col-4">
              <label className="labelForm voda-bold  w-100">
                End of Maintenance
              </label>
              <label className=" w-100">
                {formData?.majorHardwareBuildDto.eomStatus === 0
                  ? "NOT ANNOUNCED"
                  : formData?.majorHardwareBuildDto.eomStatus === 1
                  ? "NOT SPECIFIED"
                  : formatTimeLocal(
                      formData?.majorHardwareBuildDto.endOfMaintenance!
                    )}
                {/* {formatTimeLocal(
                    formData?.majorHardwareBuildDto?.endOfMaintenance
                  ) ?? "---"} */}
              </label>
            </div>

            {formData?.majorHardwareBuildDto?.endOfsupport ? (
              <div className="form-group col-4">
                <label className="labelForm voda-bold   w-100">
                  End of Support
                </label>
                <label className="  w-100">
                  {formatTimeLocal(
                    formData?.majorHardwareBuildDto?.endOfsupport
                  ) || "---"}
                </label>
              </div>
            ) : null}

            {formData?.majorHardwareBuildDto?.lastTimeBuyNew ? (
              <div className="form-group col-4">
                <label className="labelForm voda-bold   w-100">
                  Last Time Buy - New
                </label>
                <label className="  w-100">
                  {formatTimeLocal(
                    formData?.majorHardwareBuildDto?.lastTimeBuyNew
                  ) || "---"}
                </label>
              </div>
            ) : null}

            {formData?.majorHardwareBuildDto?.lastTimeBuyUpgrades ? (
              <div className="form-group col-4">
                <label className="labelForm voda-bold   w-100">
                  Last Time Buy - Upgrades
                </label>
                <label className="  w-100">
                  {formatTimeLocal(
                    formData?.majorHardwareBuildDto?.lastTimeBuyUpgrades
                  ) || "---"}
                </label>
              </div>
            ) : null}

            {formData?.majorHardwareBuildDto?.lastTimeBuyExpansions ? (
              <div className="form-group col-4">
                <label className="labelForm voda-bold   w-100">
                  Last Time Buy - Expansions
                </label>
                <label className="  w-100">
                  {formatTimeLocal(
                    formData?.majorHardwareBuildDto?.lastTimeBuyExpansions
                  ) || "---"}
                </label>
              </div>
            ) : null}

            {formData?.majorHardwareBuildDto?.description ? (
              <div className="form-group col-4">
                <label className="labelForm voda-bold   w-100">
                  Description
                </label>
                <label className="  w-100">
                  {formData?.majorHardwareBuildDto?.description}
                </label>
              </div>
            ) : null}

            <LabelViewFormString
              title="Vulnerability Status"
              value={
                formData?.majorHardwareBuildDto?.vulnerabilityStatus || "---"
              }
            />
            {/* -------------------------MODIFIED------------------- */}
            {formData?.majorHardwareBuildDto?.majorHardwareId ? (
              <>
                <div className="form-group col-4">
                  <label className="labelForm voda-bold   w-100">
                    Last Modified
                  </label>
                  <label className="  w-100">
                    {formatDateWithTime(
                      formData?.majorHardwareBuildDto?.lastModified
                    ) || "---"}
                  </label>
                </div>

                <div className="form-group col-4">
                  <label className="labelForm voda-bold   w-100">
                    Last Modified By
                  </label>
                  <label className="  w-100">
                    {formData?.majorHardwareBuildDto?.lastModifiedBy || "---"}
                  </label>
                </div>
              </>
            ) : null}
          </div>
        </fieldset>
      </div>

      {/* --------------------------SYSTEM SECTION ----------------------------------- */}
      <div className="row col-12 px-0 mx-0">
        <fieldset className="col-12 mt-3 row mx-0">
          <legend className="text-bb mb-40">System Details</legend>
          <div className="col-12 px-2 d-flex row p-0">
            <LabelViewFormString
              title="Asset Category"
              value={
                (formData?.systemTypeDto.assetCategoryResource &&
                  dictionaryToArrayAssetCategoryDto(
                    formData?.systemTypeDto.assetCategoryResource
                  ).find(
                    (el) => el.key == formData?.systemTypeDto.assetCategoryId
                  )?.value.description) ||
                "---"
              }
            />
            <LabelViewFormString
              title="Vodafone Name"
              value={formData?.systemTypeDto.systemTypeNameVodafone || "---"}
            />
            <LabelViewFormString
              title="Standard Name"
              value={formData?.systemTypeDto.systemTypeName3Gpp || "---"}
            />
            <LabelViewFormString
              title="Product Importance"
              value={
                (formData?.systemTypeDto.productImportanceResource &&
                  dictionaryToArray(
                    formData?.systemTypeDto.productImportanceResource
                  ).find(
                    (el) =>
                      el.key == formData?.systemTypeDto.productImportanceId
                  )?.value) ||
                "---"
              }
            />
            <LabelViewFormString
              title="Asset Class"
              value={formData?.systemTypeDto?.assetClassDescription ?? "---"}
            />
            <LabelViewFormString
              title="Asset Type"
              value={
                formData?.systemTypeDto.assetTypeResource &&
                formData.systemTypeDto.assetTypeId
                  ? formData.systemTypeDto.assetTypeResource[
                      formData.systemTypeDto.assetTypeId
                    ].value
                  : formData?.systemTypeDto.assetType
                  ? formData?.systemTypeDto.assetType
                  : "---"
              }
            />
            <LabelViewFormString
              title="Vertical Responsible"
              value={
                (formData?.systemTypeDto.verticalResponsibleResource &&
                  dictionaryToArray(
                    formData?.systemTypeDto.verticalResponsibleResource
                  ).find(
                    (el) =>
                      el.key == formData?.systemTypeDto.verticalResponsibleId
                  )?.value) ||
                "---"
              }
            />
            <LabelViewFormString
              title="Sub-Domain Responsible"
              value={
                (formData?.systemTypeDto.subDomainResponsibleResource &&
                  dictionaryToArray(
                    formData?.systemTypeDto.subDomainResponsibleResource
                  ).find(
                    (el) =>
                      el.key == formData?.systemTypeDto.subDomainResponsibleId
                  )?.value) ||
                "---"
              }
            />

            <div className="form-group col-4">
              <label className="labelForm voda-bold   w-100">
                Sub-Domain Spoc
              </label>
              <label className=" overflow-auto w-100">
                {subDomainSpocs && subDomainSpocs.map((x) => x.value + ", ")}
              </label>
            </div>
          </div>
        </fieldset>
      </div>

      {/* ---------------DESIGN COMPONENT ----------------------------- */}

      <div className="row col-12 px-0 mx-0">
        <fieldset className="col-12 mt-3 row mx-0">
          <legend className="text-bb mb-40">Design Component</legend>
          <div className="col-12 px-2 d-flex row p-0">
            <LabelViewFormString
              title="System Solution"
              value={formData?.systemTypeDto.systemSolution ?? "---"}
            />

            <LabelViewFormString
              title="Subnetwork Boundary"
              value={
                formData?.designComponentDto.subNetworkBoundaryResource &&
                formData?.designComponentDto.subNetworkBoundaryResource.filter(
                  (x) => {
                    return (
                      formData &&
                      formData?.designComponentDto.subNetworkBoundaryIds?.indexOf(
                        x.subNetworkBoundaryId
                      ) != -1 &&
                      formData?.designComponentDto.subNetworkBoundaryIds?.indexOf(
                        x.subNetworkBoundaryId
                      ) != undefined
                    );
                  }
                )
              }
            />
            <LabelViewFormString
              title="GDPR Relevant"
              value={
                formData?.designComponentDto.gdprRelevant == true
                  ? "Yes"
                  : formData?.designComponentDto.gdprRelevant == false
                  ? "No"
                  : "Unspecified"
              }
            />
          </div>
        </fieldset>
      </div>

      <div className="col-12 d-flex justify-content-between py-4 mt-4">
        <button
          className="  voda-bold btn btn-link px-4 btnHeader exit"
          type="button"
          onClick={() => props.action.setConfirmExitWizard()}
        >
          Exit
        </button>
        <div className="">
          <button
            className="  voda-bold btn btn-link px-4 btnHeader cancel"
            type="button"
            onClick={() =>
              props.action.wizardBackFunction &&
              formData &&
              props.action.wizardBackFunction(formData)
            }
          >
            Back
          </button>
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            type="button"
            onClick={() => submit()}
          >
            Continue
          </button>
        </div>
      </div>
    </div>
  );
};

export default ReviewNewProductModal;
