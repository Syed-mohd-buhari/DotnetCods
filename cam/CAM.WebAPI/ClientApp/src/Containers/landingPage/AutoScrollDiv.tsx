import React, { useState, useRef, useEffect, useMemo } from "react";
import "./AutoScrollDiv.css";

type Props = {
  children: React.ReactNode;
  speed?: number;
  enabled?: boolean;
};

const AutoScrollDiv: React.FC<Props> = ({
  children,
  speed = 0.5,
  enabled = true,
}) => {
  const [isHovering, setIsHovering] = useState(false);
  const [contentHeight, setContentHeight] = useState(0);
  const contentRef = useRef<HTMLDivElement | null>(null);
  const containerRef = useRef<HTMLDivElement | null>(null);
  const measureTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  useEffect(() => {
    const updateHeight = () => {
      if (contentRef.current && containerRef.current) {
        const height = contentRef.current.scrollHeight;
        const containerHeight = containerRef.current.clientHeight;

        const newHeight = height > containerHeight ? height : 0;

        // Only update if height actually changed to prevent re-renders
        setContentHeight((prev) => {
          if (Math.abs(prev - newHeight) > 1) {
            return newHeight;
          }
          return prev;
        });
      }
    };

    // Debounced update function
    const debouncedUpdate = () => {
      if (measureTimeoutRef.current) {
        clearTimeout(measureTimeoutRef.current);
      }
      measureTimeoutRef.current = setTimeout(updateHeight, 100);
    };

    // Initial measurement
    updateHeight();

    // Re-measure on window resize
    window.addEventListener("resize", debouncedUpdate);

    // Re-measure when content changes
    const resizeObserver = new ResizeObserver(debouncedUpdate);
    if (contentRef.current) {
      resizeObserver.observe(contentRef.current);
    }

    return () => {
      window.removeEventListener("resize", debouncedUpdate);
      resizeObserver.disconnect();
      if (measureTimeoutRef.current) {
        clearTimeout(measureTimeoutRef.current);
      }
    };
  }, []);

  // Memoize duration to prevent recalculation on every render
  const duration = useMemo(() => {
    return contentHeight > 0 ? contentHeight / (speed * 100) : 50;
  }, [contentHeight, speed]);

  const shouldAnimate =
    enabled === true ? !isHovering && contentHeight > 0 : false;
  return (
    <div
      ref={containerRef}
      onMouseEnter={() => setIsHovering(true)}
      onMouseLeave={() => setIsHovering(false)}
      style={{
        height: "100%",
        overflow: isHovering ? "auto" : "hidden",
        scrollbarWidth: "thin",
        padding: "12px",
        // paddingBottom: "3rem",
        position: "relative",
      }}
    >
      <div
        key={String(enabled)}
        className={shouldAnimate ? "auto-scroll-content" : ""}
        style={{
          animationDuration: `${duration}s`,
          animationPlayState: isHovering ? "paused" : "running",
          willChange: shouldAnimate ? "transform" : "auto",
        }}
      >
        <div ref={contentRef}>{children}</div>
        {/* Duplicate content for seamless loop */}
        {shouldAnimate && <div aria-hidden="true">{children}</div>}
      </div>
    </div>
  );
};

export default AutoScrollDiv;
