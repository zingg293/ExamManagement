import Link from 'next/link';
import WithErrorBoundaryCustom from '@utils/errorBounDary/WithErrorBoundaryCustom';

interface PageTitleProps {
  title: string;
  subTitle: string;
  breadcrumb: string;
  breadcrumbLink: string;
}

function _PageTitle(props: PageTitleProps) {
  return (
    <div className='ttm-page-title-row'>
      <div className='container'>
        <div className='row'>
          <div className='col-md-12'>
            <div className='title-box text-center'>
              <div className='page-title-heading'>
                <h1
                  className='title'
                  style={{
                    textTransform: 'none',
                  }}
                >
                  {' '}
                  {props.title}
                </h1>
              </div>
              <div className='breadcrumb-wrapper'>
                <span>
                  <Link title='Homepage' href={props.breadcrumbLink}>
                    <i className='fa fa-home'></i>&nbsp;&nbsp;{props.breadcrumb}
                  </Link>
                </span>

                <span className='ttm-bread-sep'>&nbsp; : : &nbsp;</span>

                <span> {props.subTitle}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export const PageTitle = WithErrorBoundaryCustom(_PageTitle);
