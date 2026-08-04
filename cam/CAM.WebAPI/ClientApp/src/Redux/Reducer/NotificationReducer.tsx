export const HIDE_ERROR = "HIDE_ERROR";
export const SET_ERROR = "SET_ERROR";

export enum NotifyType {
    success = 1,
    error = 2,
    warning = 3,
    noNotify = 0
}
export interface ErrorType {
    error: string[] | null | string,
    isOpen: boolean,
    notifyType: NotifyType
};

const initState: ErrorType = {
    error: null,
    isOpen: false,
    notifyType: NotifyType.noNotify
};

export const errorReducer = (state = initState, action: { type: string, payload: ErrorType }) => {
    let notify = action.payload;

    if (action.type === SET_ERROR) {
        return {
            error: notify.error,
            isOpen: true,
            notifyType: notify.notifyType
        }
    } else if (action.type === HIDE_ERROR) {
        return {
            error: null,
            isOpen: false,
            notifyType: NotifyType.noNotify
        }
    }

    return state;
}



// const HIDE_ERROR = "HIDE_ERROR";
// const SET_ERROR = "SET_ERROR";

// export enum NotifyType {
//     success,
//     error,
//     warning,
//     default
// }
// export interface ErrorType {
//     error: string[],
//     isOpen: boolean,
//     notifyType: NotifyType
// };
// const initState:ErrorType = {
//     error: [],
//     isOpen: false,
//     notifyType: NotifyType.default,
// };

// export const errorReducer = (state = initState, action: any) => {
//     const error:ErrorType = action.errorType;
//     if (action.type === SET_ERROR) {
//         return {
//             error: error.error,
//             isOpen: true,
//             notifyType:error.notifyType
//         }
//     } else if (action.type === HIDE_ERROR) {
//         return {
//             error: null,
//             isOpen: false,
//             notifyType:NotifyType.default
//         }
//     }
//     return state;

// }