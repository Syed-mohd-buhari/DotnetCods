import { GEt_USERS_LOGGING_LEVEL } from "../../../Business/UsersLogLevelsBusiness";

import { GetUsersLoggingLevels } from "../../../Model/UsersLoggingLevels";

const initState: GetUsersLoggingLevels = {
  deleted: false,
  orphan: false,
  lastModified: "",
  lastModifiedBy: "",
};

// export const GetUSersLoggingLevelsReducer = (
//   state = initState,
//   action: { type: string; payload: GetUsersLoggingLevels }
// ) => {
//   switch (action.type) {
//     case GEt_USERS_LOGGING_LEVEL: {
//       return {
//         ...state,
//         items: action.payload?.aspnetUsers,
//       };
//     }

//     default:
//       return state;
//   }
// };
