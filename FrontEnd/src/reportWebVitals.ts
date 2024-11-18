import { getCLS, getFID, getLCP, Metric } from "web-vitals";

type ReportHandler = (metric: Metric) => void;

export function reportWebVitals(onPerfEntry?: ReportHandler) {
  if (onPerfEntry && typeof onPerfEntry === "function") {
    getCLS(onPerfEntry);
    getFID(onPerfEntry);
    getLCP(onPerfEntry);
  }
}
