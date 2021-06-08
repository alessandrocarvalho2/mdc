import React, { useState } from "react";
import { TextField, InputAdornment, IconButton } from "@material-ui/core";
import Visibility from "@material-ui/icons/Visibility";
import VisibilityOff from "@material-ui/icons/VisibilityOff";

const PasswordInput = (props: any) => {
  const [showPassword, setShowPassword] = useState(false);
  const [name] = useState(props.name || "password");
  
  //console.log(props);

  const passwordValidation = () => {
 

    if ((props.errors.password as any)?.type === "required") {
      return "Campo obrigátorio.";
    }
    if ((props.errors.password as any)?.type === "pattern") {
      return "Formato Inválido";
    }

    return "";
  };

  return (
    <>
      <TextField
        required
        variant= "outlined"
        defaultValue={props.password}
        onChange={(event: any) => props.handlerChange(event)}
        label="Password"
        name={name}
        inputRef={
          props.register &&
          props.register({
            required: !props.userId,
            pattern: {
              value: /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=\S+$).{6,}$/i,
            },
          })
        }
        type={showPassword ? "text" : "password"}
        error={props.errors && !!props.errors.password}
        helperText={props.errors && passwordValidation()}
        InputProps={{
          endAdornment: (
            <InputAdornment position="end">
              <IconButton
                onClick={() => {
                  setShowPassword(!showPassword);
                }}
              >
                {showPassword ? <Visibility /> : <VisibilityOff />}
              </IconButton>
            </InputAdornment>
          ),
        }}
        
      />
    </>
  );
};

export default PasswordInput;
