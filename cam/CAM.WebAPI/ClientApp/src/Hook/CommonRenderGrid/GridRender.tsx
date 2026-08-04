import React, { useEffect, useRef } from "react";
import FilterMenuDateRange from "../../Components/FilterMenuDateRange";
import FilterMenuCheckbox from "../../Components/FilterMenuCheckbox";
import { FilterValueDto } from "../../Business/Common/CommonBusiness";
import {
  formatDate,
  formatDateWithTime,
  formatTimeLocal,
  lowerFirstLetter,
  stringIsNullOrEmpty,
  upperFirstLetter,
} from "../Common";
import TH from "../../Components/TableCrud/TableCrudTH";

export function SelectFilterType(
  property: string,
  type: number,
  isSortAscending: boolean | undefined,
  filtriAttivi: any,
  actionFilterDate: any,
  sortBy: string | undefined,
  filterData: FilterValueDto[] | null,
  count: string[],
  actionFilterCK: any,
  thAction: any,
  thActionDate: any,
  isVisibleFiltriString: string,
  setupDuplicates?: boolean,
  color?: string,
  hideFilter?: boolean,
  overrideName?: string,
  apiType?: string,
  index?: number,
  freezeHeader?: boolean
) {
  switch (type) {
    //DATA
    case 3:
    case 6:
      return (
        <TH
          index={index}
          spanClassName={color ?? ""}
          overridePropertyName={overrideName}
          apiType={apiType}
          propertyName={property}
          action={thActionDate}
          key={property}
          isVisibleFiltriString={isVisibleFiltriString}
          setupDuplicates={setupDuplicates ?? false}
          hideFilter={hideFilter ?? false}
          freezeHeader={freezeHeader}
        >
          {setupDuplicates == true || hideFilter === true ? null : (
            <FilterMenuDateRange
              orderAscending={isSortAscending}
              from={filtriAttivi?.[property + "StartDate"]}
              to={filtriAttivi?.[property + "EndDate"]}
              property={lowerFirstLetter(property)}
              action={actionFilterDate}
            />
          )}
        </TH>
      );
    case 7:
      return (
        <TH
          index={index}
          spanClassName={color ?? ""}
          overridePropertyName={overrideName}
          propertyName={property}
          apiType={apiType}
          action={thActionDate}
          key={property}
          isVisibleFiltriString={isVisibleFiltriString}
          setupDuplicates={setupDuplicates ?? false}
          hideFilter={hideFilter ?? false}
          freezeHeader={freezeHeader}
        ></TH>
      );
    default:
      return (
        <TH
          index={index}
          spanClassName={color ?? ""}
          overridePropertyName={overrideName}
          apiType={apiType}
          propertyName={property}
          action={thAction}
          key={property}
          isVisibleFiltriString={isVisibleFiltriString}
          setupDuplicates={setupDuplicates ?? false}
          hideFilter={hideFilter ?? false}
          freezeHeader={freezeHeader}
        >
          {setupDuplicates === true || hideFilter === true ? null : (
            <FilterMenuCheckbox
              orderAscending={isSortAscending}
              propertyInOrder={sortBy}
              filterData={filterData ?? undefined}
              overrideProperty={overrideName ?? ""}
              property={property}
              FiltriAttivi={filtriAttivi?.[property]}
              count={count}
              action={actionFilterCK}
            />
          )}
        </TH>
      );
  }
}

const rtnStringTooltip = (text: string) => {
  let length = (text ?? "").length;
  if (length > 81) {
    return text.slice(0, 80) + "...";
  } else {
    return text;
  }
};

export function SelectGridType(
  value: any,
  property: string,
  type: number,
  color?: string | "",
  id?: number | undefined,
  additionalParamValue?: any,
  additionalParamKey?: any,
  index?: any,
  thRefs?: any,
  thRefss?: any
) {
  if (
    property === "statusUrl" ||
    property === "testReport" ||
    property === "standardNir" ||
    property === "ericssonSecReport" ||
    property === "swAndStEntries" ||
    property === "penTestingReport"
  ) {
    const formattedUrl = value?.startsWith("http") ? value : `https://${value}`;
    return (
      <td
        key={property}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        <div className="hover-link">
          <a href={formattedUrl} target="_blank" rel="noopener noreferrer">
            {value}
          </a>
        </div>
      </td>
    );
  } else if (property === "lastModified") {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        {formatTimeLocal(value)}
      </td>
    );
  } else if (
    type === 3 &&
    additionalParamKey === "dcf" &&
    (property === "creationDate" || property === "modificationDate")
  ) {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        {formatDateWithTime(value)}
      </td>
    );
  } else if (
    type === 3 &&
    (property === "endOfMaintenanceValue" ||
      property === "lastTimeBuyNewValue" ||
      property === "lastTimeBuyExpansionsValue" ||
      property === "constraintScaling" ||
      property === "lastModifiedValue" ||
      property === "softwareProductionDateValue" ||
      property === "softwareInstallDateValue" ||
      property === "dataAcquisitionDateValue" ||
      property === "opsMaintenanceConractEndValue" ||
      property === "plannedCompletionValue" ||
      property === "plannedCompletion" ||
      property === "projectEndDateValue" ||
      property === "vendorEndOfMaintenanceDateValue" ||
      property === "vendorEndOfVulnerabilitySecuritySupportDateValue" ||
      property === "lastTimeBuyUpgradesValue" ||
      property === "assetLiveStatusDateValue" ||
      property === "dateAssetDecommissionedAssetValue" ||
      property === "vendorEndOfVulnerabilitySecuritySupportDateValueLcm" ||
      property === "opsMaintenanceConractEndValueLcm")
  ) {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        {value === undefined || value === null || value === "" ? "---" : value}
      </td>
    );
  } else if (
    type === 3 &&
    (property === "generaAvailableDateValue" ||
      property === "startDateValue" ||
      property === "endOfsupportValue")
  ) {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        {value === undefined || value === null || value === ""
          ? "Not Specified"
          : value}
      </td>
    );
  } else if (
    type === 3 &&
    property !== "endOfMaintenance" &&
    property !== "eoslContractDate" &&
    property !== "osEoslContractDate"
  ) {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        {value == undefined || value == null || value === ""
          ? "Not Specified"
          : formatTimeLocal(value)}
      </td>
    );
  } else if (
    property === "endOfMaintenance" ||
    property === "eoslContractDate"
  ) {
    if (additionalParamKey === "eomStatus") {
      switch (additionalParamValue) {
        case 0:
          return (
            <td
              key={property}
              className={`${color}`}
              ref={(ref) => {
                if (index === 0 && thRefs) thRefss(ref, 0);
                else if (index <= 3 && thRefs) thRefss(ref, index);
              }}
            >
              Not Announced
            </td>
          );
        case 1:
          if (property !== "endOfMaintenance") {
            return (
              <td
                key={property}
                className={`${color}`}
                ref={(ref) => {
                  if (index === 0 && thRefs) thRefss(ref, 0);
                  else if (index <= 3 && thRefs) thRefss(ref, index);
                }}
              ></td>
            );
          } else {
            return (
              <td
                key={property}
                className={`${color}`}
                ref={(ref) => {
                  if (index === 0 && thRefs) thRefss(ref, 0);
                  else if (index <= 3 && thRefs) thRefss(ref, index);
                }}
              >
                Not Specified
              </td>
            );
          }
        case 2:
          return (
            <td
              key={property}
              className={`${color}`}
              ref={(ref) => {
                if (index === 0 && thRefs) thRefss(ref, 0);
                else if (index <= 3 && thRefs) thRefss(ref, index);
              }}
            >
              {formatTimeLocal(value)}
            </td>
          );
      }
    } else {
      if (
        value.toLowerCase() == "not specified" ||
        value.toLowerCase() == "not announced"
      ) {
        return (
          <td
            key={property}
            className={`text-uppercase ${color}`}
            ref={(ref) => {
              if (index === 0 && thRefs) thRefss(ref, 0);
              else if (index <= 3 && thRefs) thRefss(ref, index);
            }}
          >
            {value}
          </td>
        );
      } else {
        return (
          <td
            key={property}
            className={`text-uppercase ${color}`}
            ref={(ref) => {
              if (index === 0 && thRefs) thRefss(ref, 0);
              else if (index <= 3 && thRefs) thRefss(ref, index);
            }}
          >
            {value == undefined || value == null
              ? "Not Specified"
              : formatTimeLocal(value)}
          </td>
        );
      }
    }
  } else if (type === 6) {
    if (property === "osEoslContractDate") {
      if (value == "Not Specified") {
        return (
          <td
            key={property}
            className={`${color}`}
            ref={(ref) => {
              if (index === 0 && thRefs) thRefss(ref, 0);
              else if (index <= 3 && thRefs) thRefss(ref, index);
            }}
          ></td>
        );
      } else {
        if (additionalParamKey === "eomStatus") {
          switch (additionalParamValue) {
            case 0:
              return (
                <td
                  key={property}
                  className={`${color}`}
                  ref={(ref) => {
                    if (index === 0 && thRefs) thRefss(ref, 0);
                    else if (index <= 3 && thRefs) thRefss(ref, index);
                  }}
                >
                  Not Announced
                </td>
              );
            case 1:
              return (
                <td
                  key={property}
                  className={`${color}`}
                  ref={(ref) => {
                    if (index === 0 && thRefs) thRefss(ref, 0);
                    else if (index <= 3 && thRefs) thRefss(ref, index);
                  }}
                ></td>
              );
            case 2:
              return (
                <td
                  key={property}
                  className={`${color}`}
                  ref={(ref) => {
                    if (index === 0 && thRefs) thRefss(ref, 0);
                    else if (index <= 3 && thRefs) thRefss(ref, index);
                  }}
                >
                  {formatTimeLocal(value)}
                </td>
              );
          }
        }
      }
    } else {
      return (
        <td
          key={property}
          className={`red`}
          ref={(ref) => {
            if (index === 0 && thRefs) thRefss(ref, 0);
            else if (index <= 3 && thRefs) thRefss(ref, index);
          }}
        >
          {value}
        </td>
      );
    }
  } else if (type === 4) {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        <a href={"mailto:" + value} tabIndex={-1}>
          {value}
        </a>
      </td>
    );
  } else if (type === 5) {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        <span style={{ cursor: "pointer" }} title={value ?? ""}>
          {rtnStringTooltip(value ?? "---")}
        </span>
      </td>
    );
  } else if (property === "active") {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        {value ? "Active" : "In-Active"}
      </td>
    );
  } else if (value === true || value === false) {
    return (
      <td
        key={property}
        className={`${color}`}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      >
        {value ? "YES" : "NO"}
      </td>
    );
  } else {
    return (
      <td
        key={property}
        className={`${color}`}
        dangerouslySetInnerHTML={{
          __html: stringIsNullOrEmpty(value) ? "---" : value,
        }}
        ref={(ref) => {
          if (index === 0 && thRefs) thRefss(ref, 0);
          else if (index <= 3 && thRefs) thRefss(ref, index);
        }}
      ></td>
    );
  }
}
