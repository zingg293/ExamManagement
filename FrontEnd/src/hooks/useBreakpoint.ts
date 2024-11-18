import { useEffect, useState } from "react";

export const useBreakPoint = () => {
  const [breakPoint, setBreakPoint] = useState({
    isMobile: false,
    isTablet: false,
    isDesktop: false
  });

  useEffect(() => {
    const handleResize = () => {
      const width = window.innerWidth;
      if (width < 768) {
        setBreakPoint({ isMobile: true, isTablet: false, isDesktop: false });
      } else if (width >= 768 && width < 1024) {
        setBreakPoint({ isMobile: false, isTablet: true, isDesktop: false });
      } else {
        setBreakPoint({ isMobile: false, isTablet: false, isDesktop: true });
      }
    };
    window.addEventListener("resize", handleResize);
    handleResize();
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  return breakPoint;
};
