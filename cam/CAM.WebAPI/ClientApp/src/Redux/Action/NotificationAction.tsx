import { HIDE_ERROR, SET_ERROR, ErrorType, NotifyType } from '../Reducer/NotificationReducer'

// export const SET_ERROR = "SET_ERROR";
// export const HIDE_ERROR = "HIDE_ERROR";
interface MessageTypeComponent {
    message: string[] | null | string | undefined,
    notifyType: NotifyType,
}


export function setNotification(message: MessageTypeComponent) {
    var obj: { type: string, payload: ErrorType } = {
        type: SET_ERROR,
        payload: { error: message.message ?? "", isOpen: true, notifyType: message.notifyType },
    }
    return obj;
}

export function hideError() {
    return {
        type: HIDE_ERROR
    }
}