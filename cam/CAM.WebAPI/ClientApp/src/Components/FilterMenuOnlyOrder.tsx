import React, { useEffect } from "react";
import { cancelDelayClose, delayClose } from "../Hook/Common";

interface Props {
  property: string;
  action: {
    closeAll(): any;
    orderBy(property: string, isAscending: boolean): any;
  };
}

const FilterMenuOnlyOrder: React.FC<Props> = (props) => {
  // const [property, setProperty] = useState("")

  // useEffect(() =>{
  //     setProperty(props.property)
  // },[])

  // const changeHandler = (mode: string) => {
  //     // props.action.setOrder(property, mode)
  // }

  useEffect(() => {
    const tables = document.getElementsByTagName("table");

    for (let i = 0; i < tables.length; i++) {
      tables[i].addEventListener("scroll", props.action.closeAll);
    }

    return () => {
      for (let i = 0; i < tables.length; i++) {
        tables[i].removeEventListener("scroll", props.action.closeAll);
      }
    };
  }, [props.action]);

  return (
    <div
      className="filterMenuOrder pb-2"
      onMouseEnter={cancelDelayClose}
      onMouseLeave={() => delayClose(props.action.closeAll)}
    >
      <div className="col-12 row mx-0 px-2 mt-2">
        <label className="w-100 filterMenu">ORDINAMENTO</label>
        <div className="col-6 pl-0 pr-1">
          <button
            className="w-100 btn btn-light btnOrderBy"
            onClick={() => props.action.orderBy(props.property, true)}
          >
            Ascending (A-Z)
          </button>
        </div>
        <div className="col-6 pr-0 pl-1">
          <button
            className="w-100 btn btn-light btnOrderBy"
            onClick={() => props.action.orderBy(props.property, false)}
          >
            Descending (Z-A)
          </button>
        </div>
      </div>
    </div>
  );
};

export default FilterMenuOnlyOrder;
