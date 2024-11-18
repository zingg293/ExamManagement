type Props = {
  requiredRole: string;
  children: any;
};

export const WithAuthorization: any = ({ children }: Props) => {
  // const { data: user, isSuccess } = useGetUserQuery({ fetch: false });
  // if (isSuccess && !check) {
  //   return <Navigate to="/admin/403" />;
  // }

  return <>{children}</>;
};

export default WithAuthorization;
