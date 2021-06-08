const HasPermission = (props: any) => {
  let userAccess: any = localStorage.getItem("access");
  userAccess = JSON.parse(userAccess);

  const hasPermission = () => {
    return (
      userAccess &&
      userAccess.length > 0 &&
      props.rules.find((item: string) => userAccess.includes(item))
    );
  };

  return hasPermission() ? props.children : null;
};

export default HasPermission;
