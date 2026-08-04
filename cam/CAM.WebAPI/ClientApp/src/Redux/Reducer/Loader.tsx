import { useSelector } from "react-redux";
import { RootState } from "../Store/rootStore";

const initialState = {
	listOfCall: [] as string[],
	show: false,
};

export const LoaderReducer = (state = initialState, action: { type: string; payload: string }) => {
	const copy = { ...state };

	switch (action.type) {
		case "ADD":
			copy.listOfCall.push(action.payload);
			copy.show = true;
			// console.log("LOADER LIST in ADD", copy.listOfCall);

			return copy;
		case "REMOVE":
			const idx = copy.listOfCall.indexOf(action.payload);
			copy.listOfCall.splice(idx, 1);
			copy.show = copy.listOfCall.length === 0 ? false : true;
			// console.log("LOADER LIST In REMOVE", copy.listOfCall);
			// console.log("LOADER LIST GRID", copy.listOfCall.length);

			return copy;
		default:
			return state;
	}
};
