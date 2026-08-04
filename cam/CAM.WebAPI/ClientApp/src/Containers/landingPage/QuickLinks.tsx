import React, { useState } from "react";
import { AiOutlinePlusCircle, AiOutlineSearch } from "react-icons/ai";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { endGuideTour, startGuideTour } from "../../Redux/Action/tourActions";
// import "./NewLandingPage.css";
import "../../Components/TreeViewMenu/TreeViewMenu.css";
import { RootState } from "../../Redux/Store/rootStore";
import { getMainTourSteps } from "../../Constant/TourSteps";
import TourGuide from "../../Components/TourGuide";

export interface QuickLink {
  icon?: React.ReactNode;
  text: string;
  path: string;
  category?: string;
  subtitle?: string;
  categoryId?: string;
  tourId?: string;
}

type QuickLinksData = { [category: string]: QuickLink[] };

interface QuickLinksProps {
  links: QuickLinksData;
  visibleCount?: number;
  onAddLink?: () => void;
}

const QuickLinks: React.FC<QuickLinksProps> = ({
  links,
  visibleCount = 2,
  onAddLink,
}) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const [searchQuery, setSearchQuery] = useState("");

  const categoryEntries = Object.entries(links);

  const sourceCategories =
    searchQuery.trim().length > 0
      ? categoryEntries
      : categoryEntries.slice(0, visibleCount);

  const filteredLinks = sourceCategories
    .map(([category, categoryLinks]) => {
      const filteredCategoryLinks = categoryLinks.filter(
        (link: QuickLink) =>
          link.text.toLowerCase().includes(searchQuery.toLowerCase()) ||
          category.toLowerCase().includes(searchQuery.toLowerCase()) ||
          (link.subtitle &&
            link.subtitle.toLowerCase().includes(searchQuery.toLowerCase()))
      );

      return {
        category,
        categoryId: categoryLinks[0]?.categoryId,
        links: filteredCategoryLinks,
      };
    })
    .filter(({ links }) => links.length > 0)
    .slice(0, visibleCount);

  const handleMenuClick = (path: string, newTab: boolean = false) => {
    if (path.includes("/")) {
      if (newTab) {
        window.open(path, "_blank");
      } else {
        navigate(path);
      }
    } else {
      try {
        eval(path);
      } catch (error) {
        console.error("Error executing function:", error);
      }
    }
  };
  const handleEndTour = () => dispatch(endGuideTour());

  return (
    <div className="quick-links-section">
      <header className="main-header mb-1">
        <div className="main-quick-title pl-0">QUICK LINKS</div>
      </header>

      <div className="d-flex justify-between">
        <div className="quick-links-search w-100">
          <AiOutlineSearch className="quick-links-search-icon" />
          <input
            type="text"
            className="quick-links-search-input"
            placeholder="Search links..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>

        {onAddLink && (
          <div style={{ paddingLeft: "10px", paddingTop: "5px" }}>
            <button className="quick-links-footer-btn" onClick={onAddLink}>
              <AiOutlinePlusCircle size={20} />
            </button>
          </div>
        )}
      </div>

      <div className="new_section-container">
        {filteredLinks.map(({ category, categoryId, links }, index: number) => (
          <div key={category || index}>
            <div className="new_tree_heading">{category}</div>
            <div className={`${categoryId || ""} category-row`}>
              {links.map((link: QuickLink, linkIndex: number) => (
                <div
                  key={linkIndex}
                  id={link.tourId}
                  className="category-cell"
                  onClick={(event) => {
                    event.preventDefault();
                    const newTab = event.ctrlKey || event.metaKey;
                    handleMenuClick(link.path, newTab);
                  }}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      handleMenuClick(link.path, false);
                    }
                  }}
                >
                  <a className="tree_menu_items" href={link.path} tabIndex={1}>
                    <p>
                      {link.text}
                      <br />
                      {link.subtitle && (
                        <span className="quick-link-subtitle">
                          ({link.subtitle})
                        </span>
                      )}
                    </p>
                  </a>
                </div>
              ))}
            </div>
            <div className="tree_space"></div>
          </div>
        ))}
        {tourStarted && (
          <TourGuide
            page="main"
            start={tourStarted}
            tourSteps={getMainTourSteps({ navigate })}
            setStartTour={(val: boolean) =>
              dispatch(val ? startGuideTour() : endGuideTour())
            }
            onTourEnd={handleEndTour}
          />
        )}
      </div>
    </div>
  );
};

export default QuickLinks;
