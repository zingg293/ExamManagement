import { useEffect, useState } from "react";
import { WindowResize } from "./interface";

export const useWindowResize = () => {
  const [windowSize, setWindowSize] = useState<WindowResize>({
    width: 0,
    height: 0
  });

  useEffect(() => {
    function handleResize() {
      setWindowSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    }

    window.addEventListener("resize", handleResize);

    handleResize();

    return () => window.removeEventListener("resize", handleResize);
  }, []);

  return windowSize;
};
