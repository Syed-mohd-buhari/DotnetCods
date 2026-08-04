import { RenderDetail } from "../Model/Common";
import {
  RelatedResource,
  RelatedResourceListValue,
} from "../Model/CommonModels";
import { TipologicaGridDtoRule } from "../Model/LookUp/LookUpGenericModel";
import {
  PlannedActivityResourceDto,
  PlannedActivityResourceDtoGrid,
  TipologicaGridDtoForVirtualized,
} from "../Model/LookUp/PlannedActivityResource";
import { ReasonCheckboxDto } from "../Model/LookUp/ReasonCheckbox";
import {
  CrossSettingsDto,
  PlannedActivityToConnect,
  PlannedActivityToConnectData,
} from "../Model/PlannedActivity";
import {
  SettingsUpdatePlannedActivityDto,
  SettingsUpdatePlannedActivityDtoUpdate,
} from "../Model/SettingsUpdatePlannedActivity";
import { PlannedActivityNetworkElementDtoUpdate } from "../Model/LookUp/PlannedActivityNetworkElement";
import { DeploymentStatusDto } from "../Model/LookUp/DeploymentStatus";
import { AssetCategoryDto } from "../Model/LookUp/AssetCategory";
import { FIGroupRecord } from "../Model/ForeignIndexModel";
import { LocationDto } from "../Model/LookUp/Location";
import { AssetTypeDto } from "../Model/LookUp/AssetType";

export function dictionaryToArray(dictionary: { [key: string]: string }) {
  var arr = [] as { key: number; value: string }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function resourceArrayRefactor(
  arrayList: { key: number; text: string }[]
) {
  var arr = [] as { key: number; value: string }[];
  if (arrayList != null && arrayList !== undefined) {
    arr = arrayList?.map(({ key, text }) => {
      return { key: key, value: text };
    });
  }
  return arr;
}

export function resourceArrayRefactorMajorSW(
  arrayList: { key: number; value: string,isSelected:boolean }[]
) {
  var arr = [] as { key: number; value: string,isSelected:boolean}[];
  if (arrayList != null && arrayList !== undefined) {
    arr = arrayList?.map(({ key, value,isSelected  }) => {
      return { key: key, value: value,isSelected };
    });
  }
  return arr;
}

export function dictionaryToArrayLocationDto(dictionary: {
  [key: string]: LocationDto;
}) {
  var arr = [] as { key: number; value: LocationDto }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function dictionaryToArraySimilarityGroup(dictionary: {
  [key: string]: FIGroupRecord;
}) {
  var arr = [] as { key: number; value: FIGroupRecord }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}
export function dictionaryToArrayAssetCategoryDto(dictionary: {
  [key: string]: AssetCategoryDto;
}) {
  var arr = [] as { key: number; value: AssetCategoryDto }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function dictionaryToArrayDeploymentStatusDto(dictionary: {
  [key: string]: DeploymentStatusDto;
}) {
  var arr = [] as { key: number; value: DeploymentStatusDto }[];
  if (dictionary !== null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}
export function dictionaryToArrayPlannedActivityNetworkElementReosurce(dictionary: {
  [key: string]: PlannedActivityNetworkElementDtoUpdate;
}) {
  var arr = [] as {
    key: number;
    value: PlannedActivityNetworkElementDtoUpdate;
  }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}
export function dictionaryToArrayPlannedActivityToConnect(dictionary: {
  [key: string]: PlannedActivityToConnect;
}) {
  var arr = [] as { key: number; value: PlannedActivityToConnect }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}
export function dictionaryToArrayPlannedActivityToConnectData(dictionary: {
  [key: string]: PlannedActivityToConnectData;
}) {
  var arr = [] as { key: number; value: PlannedActivityToConnectData }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function dictionaryToArrayGridDtoRule(dictionary: {
  [key: string]: TipologicaGridDtoRule;
}) {
  var arr = [] as { key: number; value: TipologicaGridDtoRule }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function dictionaryToArrayReasonCheckbox(dictionary: {
  [key: string]: ReasonCheckboxDto;
}) {
  var arr = [] as { key: number; value: ReasonCheckboxDto }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function dictionaryToObjectPlannedResource(dictionary: {
  [key: string]: string;
}) {
  if (dictionary != null && dictionary !== undefined) {
    const obj = { key: dictionary["label"], value: dictionary["JsonForm"] };
    return obj as { key: string; value: string };
  } else {
    return { key: "", value: "" };
  }
}
export function dictionaryToArrayRelatedResource(dictionary: {
  [key: string]: RelatedResource;
}) {
  var arr = [] as { key: number; value: RelatedResource }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}
export function dictionaryToArrayCrossSettingsDto(dictionary: {
  [key: string]: CrossSettingsDto;
}) {
  var arr = [] as { key: number; value: CrossSettingsDto }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}
export function dictionaryToArraySettingsUpdatePlannedActivityDto(dictionary: {
  [key: string]: SettingsUpdatePlannedActivityDtoUpdate;
}) {
  var arr = [] as {
    key: number;
    value: SettingsUpdatePlannedActivityDtoUpdate;
  }[];

  if (dictionary !== null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}
export function dictionaryToArrayRelatedResourceListValue(dictionary: {
  [key: string]: RelatedResourceListValue;
}) {
  var arr = [] as { key: number; value: RelatedResourceListValue }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function dictionaryToArrayPlannedActivityResourceDto(dictionary: {
  [key: string]: PlannedActivityResourceDto;
}) {
  var arr = [] as { key: number; value: PlannedActivityResourceDto }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: parseInt(key), value: value });
    }
  }
  return arr;
}

export function dictionaryToArrayRenderDetail(
  dictionary: { [key: string]: RenderDetail } | undefined
) {
  var arr = [] as { key: string; value: RenderDetail }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: key, value: value });
    }
  }
  return arr;
}
export function dictionaryToArrayActivityDetail(dictionary: {
  [key: string]: TipologicaGridDtoForVirtualized;
}) {
  var arr = [] as { key: string; value: TipologicaGridDtoForVirtualized }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key];
      arr.push({ key: key, value: value });
    }
  }

  return arr;
}

export function dictionaryToArrayLinkedLCMPLannedActivities(dictionary: {
  [key: number]: PlannedActivityToConnectData;
}) {
  var arr = [] as { key: number; value: PlannedActivityToConnectData }[];
  if (dictionary != null && dictionary !== undefined) {
    for (const key in dictionary) {
      const value = dictionary[key.toString()];
      arr.push({ key: +key, value: value });
    }
  }
  return arr;
}
