export const calculateBodyWidths = (
  thRefs,
  firstIndex,
  secondIndex,
  thirdIndex
) => {
  // Remove previous styles
  document
    .querySelectorAll<HTMLTableDataCellElement>("tbody > tr > td")
    .forEach((td) => {
      td.style.backgroundColor = "";
      td.style.left = "";
      td.style.position = "";
      td.style.zIndex = "";
      td.style.borderRight = "";
      td.style.boxShadow = "";
      td.classList.remove("table_tr_bg");
      td.classList.remove("table_tr_even_bg");
    });

  thRefs.current.forEach((ref, index) => {
    if (ref) {
      const bodyElements = document.querySelectorAll<HTMLTableDataCellElement>(
        `tbody > tr > td:nth-child(${index + 1})`
      );
      const columnIndex = index + 1;
      const totalGridElements =
        document.querySelectorAll<HTMLTableDataCellElement>(
          `thead > tr > th`
        ).length;

      if (totalGridElements && totalGridElements > 6) {
        if (columnIndex === 1) {
          // Apply styles for the first column
          firstIndex = ref.getBoundingClientRect().width;
          bodyElements.forEach((td, index) => {
            if (index % 2 === 0) {
              td.classList.add("table_tr_bg");
            } else {
              td.classList.add("table_tr_even_bg");
            }
            td.style.left = `${0}px`;
            td.style.position = "sticky";
            td.style.zIndex = td.classList?.contains("hasModal") ? "2" : "1";
          });
        } else if (columnIndex === 2) {
          // Apply styles for the second column
          secondIndex = ref.getBoundingClientRect().width;
          bodyElements.forEach((td, index) => {
            if (index % 2 === 0) {
              td.classList.add("table_tr_bg");
            } else {
              td.classList.add("table_tr_even_bg");
            }
            td.style.left = `${firstIndex}px`;
            td.style.position = "sticky";
            td.style.zIndex = td.classList?.contains("hasModal") ? "2" : "1";
          });
        } else if (columnIndex === 3) {
          // Apply styles for the third column
          thirdIndex = ref.getBoundingClientRect().width;
          bodyElements.forEach((td, index) => {
            if (index % 2 === 0) {
              td.classList.add("table_tr_bg");
            } else {
              td.classList.add("table_tr_even_bg");
            }
            td.style.left = `${firstIndex + secondIndex}px`;
            td.style.position = "sticky";
            td.style.zIndex = td.classList?.contains("hasModal") ? "2" : "1";
            td.style.borderRight = "1px solid #ccc";
            td.style.boxShadow = "5px 0 5px rgba(0,0,0,0.1)";
            td.style.filter = "drop-shadow(2px 0px 0px rgba(0,0,0,0.1))";
          });
        }
      }
    }
  });
};
