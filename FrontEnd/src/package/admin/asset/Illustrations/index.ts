import DrawKitVectorIllustrationTeamWork4 from "./DrawKitVectorIllustrationProjectManager4.png";
import Illustration7 from "./search-hacker.svg";
import Illustration1 from "./concept-of-data-analysis-and-maintenance.svg";
import Illustration2 from "./man-developing-website-on-desk.svg";
import Illustration3 from "./cloud-storage.svg";
import Illustration4 from "./social-media-users.svg";
import Illustration5 from "./concept-of-website-recovery.svg";
import Illustration6 from "./design-and-development-process.svg";

const RandomIllustration = () => {
  const Illustrations = [
    Illustration1,
    Illustration2,
    Illustration3,
    Illustration4,
    Illustration5,
    Illustration6,
    Illustration7
  ];
  return Illustrations[Math.floor(Math.random() * Illustrations.length)];
};

export {
  DrawKitVectorIllustrationTeamWork4,
  Illustration1,
  Illustration2,
  RandomIllustration,
  Illustration7,
  Illustration3,
  Illustration4,
  Illustration5,
  Illustration6
};
