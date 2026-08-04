import React, { useEffect, useRef, useState } from "react";
import { Swiper, SwiperSlide } from "swiper/react";

// Import Swiper styles
import "swiper/css";
import "swiper/css/pagination";
import "swiper/css/navigation";
import "swiper/css/effect-fade";

import "./slider.css";

// Import required modules
import {
  Mousewheel,
  EffectFade,
  Autoplay,
  Pagination,
  Navigation,
  Keyboard,
} from "swiper/modules";
import { BsFillPauseFill, BsFillPlayFill } from "react-icons/bs";
import { DropdownInputComponent } from "../FormField";

interface CustomSwiperProps {
  slides: React.ReactNode[]; // Array of strings or JSX for slides
  autoplayDelay?: number; // Delay for autoplay in milliseconds
  showPagination?: boolean; // Toggle for pagination
  showNavigation?: boolean; // Toggle for navigation arrows
  loadDropDownComponent?: React.ReactNode[]; // Array of strings or JSX for slides
}

const CustomSwiper: React.FC<CustomSwiperProps> = ({
  slides,
  autoplayDelay = 2500,
  showPagination = true,
  showNavigation = true,
  loadDropDownComponent,
}) => {
  const progressCircle = useRef<SVGSVGElement>(null);
  const progressContent = useRef<HTMLSpanElement>(null);
  const swiperRef = useRef<any>(null);
  const [isPlaying, setIsPlaying] = useState(true); // State to track autoplay
  const [currentSlideIndex, setCurrentSlideIndex] = useState(0); // Track current slide index

  // This will get called when Swiper is initialized
  const handleSwiperInit = (swiper: any) => {
    swiperRef.current = swiper; // Save the Swiper instance to the ref
  };
  const handleSlideChange = (swiper: any) => {
    setCurrentSlideIndex(swiper.activeIndex);
  };
  const onAutoplayTimeLeft = (s: any, time: number, progress: number) => {
    if (progressCircle.current) {
      progressCircle.current.style.setProperty("--progress", `${1 - progress}`);
    }
    if (progressContent.current) {
      progressContent.current.textContent = `${Math.ceil(time / 1000)}s`;
    }
  };

  // Custom pagination with Play/Pause button at the start
  const pagination = {
    clickable: true,
    // renderBullet: function (index, className) {
    //   return '<span class="' + className + '">' + (index + 1) + "</span>";
    // },
  };

  // Toggle autoplay on play/pause button click
  const handlePlayPause = () => {
    if (!swiperRef.current) return;

    if (isPlaying) {
      swiperRef.current.autoplay.stop();
    } else {
      swiperRef.current.autoplay.start();
    }

    setIsPlaying(!isPlaying);
  };

  useEffect(() => {
    handlePlayPause();
  }, []);

  // console.log("swiperRef", swiperRef);
  return (
    <Swiper
      onSwiper={handleSwiperInit}
      spaceBetween={30}
      centeredSlides={true}
      speed={1000} // Transition speed for fade effect
      effect={"fade"}
      autoplay={{
        delay: autoplayDelay,
        disableOnInteraction: false,
      }}
      pagination={showPagination ? pagination : false}
      navigation={showNavigation}
      modules={[EffectFade, Autoplay, Pagination, Navigation, Keyboard]}
      onAutoplayTimeLeft={onAutoplayTimeLeft}
      allowTouchMove={false}
      // slidesPerView={1}
      // loop={true} // Optional: Enable looping
      onSlideChange={handleSlideChange}
      className="mySwiper"
    >
      {slides.map((slide, index) => (
        <SwiperSlide key={index} style={{ height: "80vh" }}>
          {slide}
        </SwiperSlide>
      ))}
      <div className="autoplay-progress" slot="container-end">
        <svg viewBox="0 0 48 48" ref={progressCircle}>
          <circle cx="24" cy="24" r="20"></circle>
        </svg>
        <span ref={progressContent}></span>
      </div>

      {/* Custom Play/Pause Button */}
      <button className="play-pause-btn" onClick={() => handlePlayPause()}>
        {isPlaying ? (
          <BsFillPauseFill size={20} />
        ) : (
          <BsFillPlayFill size={20} />
        )}
      </button>
      {loadDropDownComponent &&
        loadDropDownComponent.length > 0 &&
        currentSlideIndex === 0 && (
          <div className="paDropdown">{loadDropDownComponent?.[0]}</div>
        )}
    </Swiper>
  );
};

export default CustomSwiper;
