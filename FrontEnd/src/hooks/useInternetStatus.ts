import { useEffect, useState } from "react";

export const useInterNetStatus = () => {
  const [isOnline, setIsOnline] = useState(() => {
    if (navigator.onLine) {
      return true;
    } else {
      return false;
    }
  });

  useEffect(() => {
    window.ononline = () => {
      setIsOnline(true);
    };
    window.onoffline = () => {
      setIsOnline(false);
    };
  }, [isOnline]);

  return isOnline;
};
