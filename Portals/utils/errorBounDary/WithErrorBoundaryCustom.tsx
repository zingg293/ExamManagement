// eslint-disable-next-line no-use-before-define
import React from 'react';
import { withErrorBoundary } from 'react-error-boundary';
import Image from 'next/image';
import ErrorLampRobot from './ErrorLampRobot.png';
import { useRouter } from 'next/router';

// eslint-disable-next-line node/handle-callback-err
function ErrorFallback({
  error,
  resetErrorBoundary,
}: {
  error: Error;
  resetErrorBoundary: () => void;
}): any {
  const navigate = useRouter();
  return (
    <div className='error-boundary-container '>
      <div className='error-boundary'>
        <Image
          src={ErrorLampRobot}
          alt='error'
          className='error-boundary-image'
        />
      </div>
      <div className='error-boundary-content'>
        <h1 className='featured-title'>Something went wrong</h1>
        <p className='featured-desc'>
          We are working on getting this fixed as soon as we can. You can try
          refreshing the page. If the problem persists feel free to contact us.
        </p>
        <button
          className='ttm-btn ttm-btn-size-md ttm-btn-style-border ttm-icon-btn-left ttm-btn-color-blue'
          onClick={() => {
            resetErrorBoundary();
            // Sử dụng Link để điều hướng đến trang chủ
            navigate.push('/');
          }}
        >
          Go to Home
        </button>
      </div>
    </div>
  );
}

// Thay đổi tên thành withErrorBoundaryCustom và sử dụng withErrorBoundary từ react-error-boundary
export default function WithErrorBoundaryCustom<T>(component: React.FC<T>) {
  return withErrorBoundary(component, {
    FallbackComponent: ErrorFallback,
  });
}
