import "./LoadingProgress.css";

export function LoadingProgress({ isDarkMode }: { isDarkMode: boolean }) {
  const loadingStyle = {
    backgroundColor: isDarkMode ? "#535252D1" : "#ccc"
  };
  const progressStyle = {
    backgroundColor: isDarkMode ? "#ffffff" : "#000000"
  };
  return (
    <div className="loader" style={loadingStyle}>
      <div className="progress" style={progressStyle}></div>
    </div>
  );
}
