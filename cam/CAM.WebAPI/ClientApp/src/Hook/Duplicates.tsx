import { DataModalConfirm, DataRemediationConfirm, QueryObjectGrid } from "../Model/Common";
import { DataRemediationDto } from "../Model/CommonModels";
import setLoader from "../Redux/Action/LoaderAction";

export const rtnColorDuplicate = (id: number, data: DataRemediationDto) => {
	let duplicatoInserito = data.duplicatesId?.includes(id);
	let correctSelezionato = data.correctId == id;

	if (correctSelezionato) {
		return "greenCorrect";
	} else if (duplicatoInserito) {
		return "redDuplicate";
	} else {
		return "";
	}
};

export const AddRemoveDuplicates = (id: number, checked: boolean, data: DataRemediationDto, setAction: Function) => {
	let copy = { ...data } as DataRemediationDto;

	if (checked) {
		if (copy.duplicatesId != undefined) {
			copy.duplicatesId.push(id);
		} else {
			let arr = [] as Array<number>;
			arr.push(id);
			copy.duplicatesId = arr;
		}
		if (copy.correctId == id) {
			copy.correctId = 0;
		}
	} else {
		if (copy.duplicatesId != undefined) {
			let index = copy.duplicatesId.findIndex((x) => x === id);
			if (index != undefined && index != -1) {
				copy.duplicatesId.splice(index, 1);
			}
		}
	}
	setAction(copy);
};

export const cancelDuplicates = (setDataFunction: Function, setBooleanState: Function, getGrid: Function, query: QueryObjectGrid, setQuery: Function) => {
	let initialstate = { duplicatesId: [], correctId: 0 };
	let copy = { ...query } as QueryObjectGrid;
	copy.pageSize = 10;
	copy.page = 1;
	setQuery(copy);
	getGrid(copy).then((x) => {
		setBooleanState(false);
		setDataFunction(initialstate);
	});

	return;
};

export const GetGridSetupDuplicates = (setBooleanState: Function, getGrid: Function, query: QueryObjectGrid, setQuery: Function) => {
	let copy = { ...query } as QueryObjectGrid;
	copy.pageSize = 0;
	copy.page = 1;

	getGrid(copy).then((x) => setBooleanState(true));
	setQuery(copy);
};

export const SetApplyRemediationConfirm = (dataConfirm: DataModalConfirm, setConfirmData: Function, confirmFunc: Function, data: DataRemediationDto) => {
	let copy = { ...dataConfirm } as DataModalConfirm;
	copy.actions.confirm = () => confirmFunc(data);
	copy.actions.cancel = () => setConfirmData(DataRemediationConfirm);
	copy.isOpen = true;
	setConfirmData(copy);
};

export const EndOrphanDuplicates = (reset: Function, setSetupDuplicates: Function, setConfirmDataRemediation: Function, setDuplicatesData: Function) => {
	reset();
	setSetupDuplicates(false);
	setConfirmDataRemediation(DataRemediationConfirm);
	setDuplicatesData({ duplicatesId: [], correctId: 0 });
};

export const setCorrect = (id: number, checked: boolean, setAction: Function, data: DataRemediationDto) => {
	let copy = { ...data } as DataRemediationDto;
	if (checked) {
		copy.correctId = id;
		if (copy.duplicatesId != undefined) {
			let index = copy.duplicatesId.findIndex((x) => x === id);
			if (index != undefined && index != -1) {
				copy.duplicatesId.splice(index, 1);
			}
		}
	} else {
		copy.correctId = undefined;
	}
	setAction(copy);
};
