import React, { useEffect, useState, useRef } from "react";
import { Carousel } from "react-bootstrap";
import { BsFillPauseFill, BsFillPlayFill } from "react-icons/bs";
import "./carousel.css";

interface CustomCarouselProps {
  slides: React.ComponentType[]; // Array of JSX components for slides
  autoplayDelay?: number; // Delay for autoplay in milliseconds
}

const CustomCarousel: React.FC<CustomCarouselProps> = ({
  slides,
  autoplayDelay = 2500,
}) => {
  const [isPlaying, setIsPlaying] = useState(true);
  const [currentIndex, setCurrentIndex] = useState(0);
  const progressCircle = useRef<SVGSVGElement>(null);
  const progressContent = useRef<HTMLSpanElement>(null);
  const intervalRef = useRef<NodeJS.Timeout | null>(null);

  const startAutoplay = () => {
    intervalRef.current = setInterval(() => {
      setCurrentIndex((prev) => (prev + 1) % slides.length);
    }, autoplayDelay);
  };

  const stopAutoplay = () => {
    if (intervalRef.current) {
      clearInterval(intervalRef.current);
    }
  };

  const handlePlayPause = () => {
    if (isPlaying) {
      stopAutoplay();
    } else {
      startAutoplay();
    }
    setIsPlaying(!isPlaying);
  };

  useEffect(() => {
    if (isPlaying) {
      startAutoplay();
    }
    return () => stopAutoplay(); // Cleanup on unmount
  }, [isPlaying, autoplayDelay]);

  useEffect(() => {
    if (progressCircle.current) {
      progressCircle.current.style.setProperty("--progress", "0");
    }
    let elapsed = 0;
    const progressInterval = setInterval(() => {
      elapsed += 100;
      const progress = elapsed / autoplayDelay;
      if (progressCircle.current) {
        progressCircle.current.style.setProperty("--progress", `${progress}`);
      }
      if (elapsed >= autoplayDelay) {
        clearInterval(progressInterval);
      }
    }, 100);
    return () => clearInterval(progressInterval);
  }, [currentIndex]);

  return (
    <div className="custom-carousel-container">
      <Carousel
        activeIndex={currentIndex}
        onSelect={(selectedIndex) => setCurrentIndex(selectedIndex)}
        interval={null} // Disable default autoplay
      >
        {slides.map((Slide, index) => (
          <Carousel.Item key={index} style={{ height: "610px" }}>
            {<Slide />}
          </Carousel.Item>
        ))}
      </Carousel>

      {/* <div className="autoplay-progress">
        <svg viewBox="0 0 48 48" ref={progressCircle}>
          <circle cx="24" cy="24" r="20"></circle>
        </svg>
        <span ref={progressContent}></span>
      </div> */}

      <button className="play-pause-btn" onClick={handlePlayPause}>
        {isPlaying ? (
          <BsFillPauseFill size={20} />
        ) : (
          <BsFillPlayFill size={20} />
        )}
      </button>
    </div>
  );
};

export default CustomCarousel;
