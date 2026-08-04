import React, { useState } from "react";
import { BarChart } from "@mui/x-charts/BarChart";
import ReactPaginate from "react-paginate";
import { FaAngleLeft, FaAngleRight } from "react-icons/fa";
import { useTheme } from "../../Context/ThemeContext";
import { safeNumber } from "../../Hook/Common";

const ProductCompliance = ({ data }) => {
  const itemsPerPage = 10;
  const [currentProductPage, setCurrentProductPage] = useState(0);

  const offset = currentProductPage * itemsPerPage;

  const currentItems = [...data]
    .slice(offset, offset + itemsPerPage)
    .map((product) => ({
      id: product.productId,
      value: 100,
      label: product.productName,
      green: safeNumber(product.greenCompatibilityPercentage),
      red: safeNumber(product.redCompatibilityPercentage),
      amber: safeNumber(product.amberCompatibilityPercentage),
    }));

  const pageCount = Math.ceil(data.length / itemsPerPage);

  const handlePageChange = (selectedPage) => {
    setCurrentProductPage(selectedPage);
  };

  const { darkMode } = useTheme();

  return (
    <div>
      <BarChart
        dataset={currentItems}
        xAxis={[{ scaleType: "band", dataKey: "label" }]}
        series={[
          {
            dataKey: "green",
            highlightScope: { highlighted: "item", faded: "global" },
            valueFormatter: (element) => `${element} %`,
            stack: "Product Compliance",
            color: "#00c300a1",
          },
          {
            dataKey: "amber",
            highlightScope: { highlighted: "item", faded: "global" },
            valueFormatter: (element) => `${element} %`,
            stack: "Product Compliance",
            color: "#FFFF00a1",
          },
          {
            dataKey: "red",
            highlightScope: { highlighted: "item", faded: "global" },
            valueFormatter: (element) => `${element} %`,
            stack: "Product Compliance",
            color: "#e60000a1",
          },
        ]}
        barLabel={(item) => (item.value ? item.value + "%" : undefined)}
        height={450}
        width={currentItems.length < 5 ? 600 : 1100}
      />

      <div className="col-12 d-flex justify-content-center align-items-center">
        <ReactPaginate
          breakLabel="...."
          nextLabel={
            <FaAngleRight size={25} color={darkMode ? "white" : "black"} />
          }
          onPageChange={(e) => handlePageChange(e.selected)}
          pageRangeDisplayed={3}
          pageCount={pageCount}
          previousLabel={
            <FaAngleLeft size={25} color={darkMode ? "white" : "black"} />
          }
          className="pagenation"
          activeClassName="active"
          forcePage={currentProductPage}
          disableInitialCallback={true}
        />
      </div>
    </div>
  );
};

export default ProductCompliance;
