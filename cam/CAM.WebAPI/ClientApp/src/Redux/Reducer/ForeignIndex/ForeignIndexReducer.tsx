import { useSelector } from "react-redux";
import { RootState } from "../../Store/rootStore";
import { ForeignIndexDto } from '../../../Model/ForeignIndexModel';

interface ForeignIndexResult{
	data: ForeignIndexDto | null
}

const initialState : ForeignIndexResult = {
	data: null,
};

export const ForeignIndexReducer = (state = initialState, action: { type: string, payload: ForeignIndexDto }) => {
	switch (action.type) {
		case 'GET_ORPHANS_FOREIGN_INDEX':
			return { data: action.payload };
		default:
			return state;
	}
};
