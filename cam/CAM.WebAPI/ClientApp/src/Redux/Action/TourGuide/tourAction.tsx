export const SET_MAJOR_SW_BUILD_INDEX = "SET_MAJOR_SW_BUILD_INDEX";
export const SET_MAJOR_HW_BUILD_INDEX = "SET_MAJOR_HW_BUILD_INDEX";
export const SET_SYSTEM_TYPE_INDEX = "SET_SYSTEM_TYPE_INDEX";

export const setMajorSwBuildIndex = (index: number | null) => ({
  type: SET_MAJOR_SW_BUILD_INDEX,
  payload: index,
});
export const setMajorHwBuildIndex = (index: number | null) => ({
  type: SET_MAJOR_HW_BUILD_INDEX,
  payload: index,
});
export const setSystemTypeIndex = (index: number | null) => ({
  type: SET_SYSTEM_TYPE_INDEX,
  payload: index,
});
