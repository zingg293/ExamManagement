import { useEffect } from "react";
import { useState } from "react";
import { timer } from "./interface";
import dayjs from "dayjs";

export function useClock() {
  const [time, setTime] = useState<timer>({
    Day: dayjs().format("dd, DD-MM-YYYY"),
    Timer: dayjs().format("HH:mm:ss"),
    Hour: dayjs().format("HH"),
    Minute: dayjs().format("mm"),
    Second: dayjs().format("ss"),
    Period: dayjs().format("A"),
    Year: dayjs().format("YYYY"),
    Month: dayjs().format("MM"),
    nd: dayjs().format("Do"),
    DayNumber: dayjs().format("DD")
  });

  useEffect(() => {
    const timer = setInterval(() => {
      setTime((prev) => ({
        ...prev,
        Day: dayjs().format("dd, DD-MM-YYYY")
      }));
      setTime((prev) => ({ ...prev, Timer: dayjs().format("HH:mm:ss") }));
      setTime((prev) => ({ ...prev, Hour: dayjs().format("HH") }));
      setTime((prev) => ({ ...prev, Minute: dayjs().format("mm") }));
      setTime((prev) => ({ ...prev, Second: dayjs().format("ss") }));
      setTime((prev) => ({ ...prev, Period: dayjs().format("A") }));
      setTime((prev) => ({ ...prev, Year: dayjs().format("YYYY") }));
      setTime((prev) => ({ ...prev, Month: dayjs().format("MM") }));
      setTime((prev) => ({ ...prev, nd: dayjs().format("dddd") }));
      setTime((prev) => ({ ...prev, DayNumber: dayjs().format("DD") }));
    }, 1000);
    return () => clearInterval(timer);
  }, []);
  return time;
}
