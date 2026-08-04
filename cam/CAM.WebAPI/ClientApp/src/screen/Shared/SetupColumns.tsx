import React, { useState, useEffect } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  RenderDetail,
  CustomGridRender,
  DataModalConfirm,
  stateConfirm,
} from "../../Model/Common";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import { SaveGrid } from "../../Model/CommonModels";
import {
  DeleteCustomGridRender,
  SaveCustomGridRender,
} from "../../Redux/Action/Grid/SaveGridCustom";
import ModalConfirm from "../../Components/ModalConfirm";

interface Props {
  action: {
    closeModalSetup(changed?: boolean): any;
  };
  renderGrid: CustomGridRender | undefined;
  tab?: string | "";
}

const SetupColumns: React.FC<Props> = (props) => {
  const [changed, SetChanged] = useState(false);
  const [tabState, SetTabState] = useState(props.tab);
  const [data, setData] = useState<RenderDetail[]>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const[selected,setSelected] = useState(false);

  useEffect(() => {
    // props.renderGrid && setData(props.renderGrid.render);

    if(props.renderGrid){
      let copy = props.renderGrid.render.map((x)=> {
        if(!x.tab){
          x.tab = ""
        }
        if(!x.archive){
          x.archive = false
        }
        if(!x.ignore){
          x.ignore = false
        }
        return x
      })
      setData(copy);
    }

    SetTabState(props.tab ?? "");
    checkCoulmnsStatus();
  }, []);

  function SaveSetupColumns(item: SaveGrid) {
    SaveCustomGridRender(item).then((x) => props.action.closeModalSetup(false));
  }

  function ResetSetupColumns(classe: string) {
    setConfirm(stateConfirm);
    DeleteCustomGridRender(classe).then((x) =>
      props.action.closeModalSetup(false)
    );
  }

  const checkCoulmnsStatus = () => {
    if(data.length<1){
      if(props.renderGrid){
        let columns = props.renderGrid.render;
        let result = columns.filter(item => item.show==false);
        if(result.length>0){
          setSelected(false)
        }else setSelected(true);
      }
      return;
    }
  }

  function SelectAllColumns(classe: string) {
    setConfirm(stateConfirm);
    setSelected(true);
    checkCoulmnsStatus();
    let copy=[...data];

    if(!selected){
      copy.forEach(item =>{
        item.show=true;
      })
      setSelected(true);
    }else{
      copy.forEach(item =>{
        item.show=false;
      })
      setSelected(false);
    }
    
    setData(copy);
    // DeleteCustomGridRender(classe).then((x) =>
    //   props.action.closeModalSetup(false)
    // );
  }

  function Cancel() {
    setConfirm(stateConfirm);
    props.action.closeModalSetup(false);
  }

  const onChangeCheckbox = (property: string | undefined, e: any) => {
    SetChanged(true);
    let checked = e.target.checked;
    let copy = [...data] as RenderDetail[];
    let index = copy.findIndex(
      (x) => x.propertyName === property && x.tab === tabState
    );
    copy[index].show = checked;
    if (tabState != null || tabState !== "") {
      copy[index].tab = tabState;
    }
    setData(copy);
  };

  const upIndex = (name: string | undefined) => {
    SetChanged(true);
    let copy = [...data] as RenderDetail[];
    let index = copy.findIndex(
      (x) => x.propertyName === name && x.tab === tabState
    );
    if (index !== -1) {
      let indexPre = copy.findIndex(
        (x) => x.tab === tabState && x.order === copy[index].order - 1
      );
      copy[indexPre].order = copy[index].order;
      copy[index].order = copy[index].order - 1;

      setData(copy);
    }
  };

  const downIndex = (name: string | undefined) => {
    SetChanged(true);
    let copy = [...data] as RenderDetail[];
    let index = copy.findIndex(
      (x) => x.propertyName === name && x.tab === tabState
    );
    if (index !== -1) {
      let indexPre = copy.findIndex(
        (x) => x.tab === tabState && x.order === copy[index].order + 1
      );
      copy[indexPre].order = copy[index].order;
      copy[index].order = copy[index].order + 1;
      setData(copy);
    }
  };

  const toSave = {
    className: props.renderGrid?.className,
    render: data,
  } as SaveGrid;

  const ConfirmReset = () => {
    setConfirm({
      title: "Confirm",
      message:
        "Are you sure you want to delete your setup and restore to default?",
      button: "Reset",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: () => ResetSetupColumns(props.renderGrid?.className ?? ""),
      },
    });
  };

  const SelectAll = () => {
    // setConfirm({
    //   title: "Confirm",
    //   message:
    //     "Are you sure you want to select all the columns?",
    //   button: "Yes",
    //   item: 0,
    //   isOpen: true,
    //   actions: {
    //     cancel: () => setConfirm(stateConfirm),
    //     confirm: () => SelectAllColumns(props.renderGrid?.className ?? ""),
    //   },
    // });
    SelectAllColumns(props.renderGrid?.className ?? "")
  };

  const ConfirmCancel = () => {
    if (changed) {
      setConfirm({
        title: "Confirm",
        message: "Are you sure you want to quit? UnSaved changes will be lost",
        button: "Exit",
        item: 0,
        isOpen: true,
        actions: {
          cancel: () => setConfirm(stateConfirm),
          confirm: () => Cancel(),
        },
      });
    } else {
      Cancel();
    }
  };

  const selectAll = () => {
    SetChanged(true);
    let copy = [...data] as RenderDetail[];

    copy.forEach((item) => {
      item.show = true;
    });

    setData(copy);
  };

  return (
    <div
      className="mt-3 d-flex justify-content-center row mx-0"
      style={{ boxShadow: "0 0 10px 2px #ccc", borderRadius: "11px" }}
    >
      <ModalConfirm data={confirm} />
      <div className="col-md-12 row mx-0 d-flex justify-content-between align-items-center my-1 pt-10 pb-10">
        <label className="mb-0 voda-bold">Column</label>
        <label className="mb-0 voda-bold">Order</label>
      </div>
      <div className="col-md-12 row mx-0 px-0 setUpColumn bbt-2">
      {data && data.sort((a, b) => a.order - b.order).map((item, i) => {
        return item.tab === tabState || item.tab === "" ? (
          <div
            className="col-md-12 row mx-0 pr-2 justify-content-between align-items-center setupRow bb-1"
            key={item.propertyName}
          >
            <div className="d-flex align-items-center">
              <label className="mb-0">
                <input
                  type="checkbox"
                  checked={item.show}
                  className="mr-2"
                  onChange={(e) => onChangeCheckbox(item.propertyName, e)}
                />
                {item.propertyName && LabelsDictionary[item.propertyName]
                  ? LabelsDictionary[item.propertyName].Short
                  : item.propertyName}
              </label>
            </div>
            <div className="d-flex flex-column">
              <button
                type="button"
                className="btn btn-link h-50 py-1 chevron"
                disabled={item.order === 1}
                onClick={() => upIndex(item.propertyName)}
              >
                <img
                  className="btnEdit"
                  src={require("../../img/up.png")}
                  alt="up"
                />
              </button>
              <button
                type="button"
                className="btn btn-link h-50 py-1 chevron"
                disabled={
                  item.order ===
                  data.filter((x) => x.tab === tabState || x.tab === "")
                    .length
                }
                onClick={() => downIndex(item.propertyName)}
              >
                <img
                  className="btnEdit"
                  src={require("../../img/down.png")}
                  alt="down"
                />
              </button>
            </div>
          </div>
        ) : null;
      })}
      </div>
      <div className="col-12 justify-content-between my-3 d-flex footerModal">
        <div className="">
          <button
            className="voda-bold btn btn-link px-4 btnHeader cancel"
            type="button"
            onClick={() => ConfirmReset()}
          >
            Reset Default
          </button>
          <button
            className="voda-bold btn btn-link px-4 btnHeader cancel"
            type="button"
            onClick={() => SelectAll()}
          >
            {!selected?'Select All':'Unselect All'}
          </button>
        </div>

        <div className="justify-content-end d-flex ">
          <button
            className="voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() => ConfirmCancel()}
            type="button"
          >
            Cancel
          </button>
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader"
            type="button"
            onClick={() => SaveSetupColumns(toSave)}
          >
            Save
          </button>
        </div>
      </div>
    </div>
  );
};

export default SetupColumns;
