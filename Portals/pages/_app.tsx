import type { AppProps } from "next/app";
import { Header } from "@components/header";
import { Footer } from "@components/footer";
import LoadingSpinner from "@components/LoadingSpinner/LoadingSpinner";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import Head from "next/head";
import "@styles/css/font-awesome.css";
import "@styles/css/common.css";
import "@styles/css/responsive.css";
import "@styles/globals.css";

function MyApp({ Component, pageProps }: AppProps) {
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  useEffect(() => {
    const handleStart = () => setLoading(true);
    const handleComplete = () => setLoading(false);

    router.events.on("routeChangeStart", handleStart);
    router.events.on("routeChangeComplete", handleComplete);
    router.events.on("routeChangeError", handleComplete);

    return () => {
      router.events.off("routeChangeStart", handleStart);
      router.events.off("routeChangeComplete", handleComplete);
      router.events.off("routeChangeError", handleComplete);
    };
  }, []);
  return (
      <div className="page">
        <Head>
          <meta name="msapplication-TileColor" content="#da532c" />
          <meta name="theme-color" content="#ffffff" />
          <meta
            name="google-site-verification"
            content="1G1PtEj1iYQ_thS4i-yyDV1OnOrYPmRdU9kARzod5A8"
          />
        </Head>
        {loading ? (
          <LoadingSpinner />
        ) : (
          <>
            <Header />
            <Component {...pageProps} />
            <Footer />
          </>
        )}
      </div>
  );
}

export default MyApp;
