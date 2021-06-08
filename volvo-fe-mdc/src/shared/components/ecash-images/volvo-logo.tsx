import Logo from "../../../../src/assets/images/Volvo_Ironmark_online_RGB.svg";

const VolvoLogo = (props: any) => {
  return (
    <div
      style={{
        ...props
      }}
    >
      <img src={Logo} alt="logo" />
    </div>
  );
};

export default VolvoLogo;
