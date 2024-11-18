import { useState } from "react";

export const useThrottle = (fn: any, delay: number) => {
  const [last, setLast] = useState(0);
  return (...args: any) => {
    const now = new Date().getTime();
    if (now - last > delay) {
      setLast(now);
      fn(...args);
    }
  };
};
