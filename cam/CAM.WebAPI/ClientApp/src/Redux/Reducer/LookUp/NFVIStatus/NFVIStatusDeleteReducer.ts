import { ResultDto } from "../../../../Model/CommonModels"


const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const NFVIStatusDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case "DELETE_NFVI_STATUS":
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
