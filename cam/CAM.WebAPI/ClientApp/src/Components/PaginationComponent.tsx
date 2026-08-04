import React from "react";
import "../Css/App.css";
import ReactPaginate from "react-paginate";
import { useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";

import { FaAngleLeft, FaAngleRight } from "react-icons/fa";
import { useTheme } from "../Context/ThemeContext";

interface Props {
  pagination:
    | { page: number | undefined; pageSize: number | undefined }
    | undefined;
  totalItems: number | undefined;
  actions: {
    next(pageNumber: number): any | void;
    back(pageNumber: number): any | void;
  };
}

const Paginate: React.FC<Props> = (props) => {
  const totalItems = props.totalItems ?? 0;
  const pageSize = props.pagination?.pageSize ?? 1;
  const pageCount = Math.ceil(totalItems / pageSize);
  const currentPage = (props.pagination?.page ?? 1) - 1;
  const backTrue =
    props.pagination?.page && props.pagination?.page > 1 ? "" : "notDisplay";
  const forwardTrue =
    props?.pagination?.page &&
    props?.totalItems &&
    props?.pagination?.pageSize &&
    props.totalItems > props.pagination.pageSize &&
    props?.pagination?.page <
      Math.ceil(props.totalItems / props.pagination.pageSize)
      ? ""
      : "notDisplay";

  const handelOnPageChange = (pageNumber: number) => {
    console.log("page number => ", pageNumber);
    props.actions.next(pageNumber + 1);
  };

  const { darkMode } = useTheme();

  return (
    <div className="col-12 d-flex justify-content-center align-items-center">
      {/* <div className="col-1 d-flex justify-content-center p-0">
        <button
          type="button"
          className={`pagination btn btn-link mr-3 ${backTrue}`}
          onClick={props.actions.back}
        >
          <img
            title="back"
            alt="Back"
            className="pagination"
            src={require("../img/left-arrow.png")}
          />
        </button>
      </div>
      <div className="col-1 d-flex justify-content-center p-0">
        <label className="pagination mb-0 voda-bold">
          {props.pagination?.page}
        </label>
      </div>
      <div className="col-1 d-flex justify-content-center p-0">
        <button
          type="button"
          className={`pagination btn btn-link mr-3 ${forwardTrue}`}
          onClick={props.actions.next}
        >
          <img
            title="next"
            alt="Next"
            className="pagination"
            src={require("../img/right-arrow.png")}
          />
        </button>
      </div> */}
      {totalItems ? (
        <ReactPaginate
          breakLabel="...."
          nextLabel={
            <FaAngleRight size={25} color={`${darkMode ? "white" : "black"}`} />
          }
          onPageChange={(e) => handelOnPageChange(e.selected)}
          pageRangeDisplayed={3}
          pageCount={pageCount}
          previousLabel={
            <FaAngleLeft size={25} color={`${darkMode ? "white" : "black"}`} />
          }
          className="pagenation"
          activeClassName="active"
          forcePage={Math.min(currentPage, pageCount - 1)}
          disableInitialCallback={true}
        />
      ) : (
        ""
      )}
    </div>
  );
};

export default Paginate;
