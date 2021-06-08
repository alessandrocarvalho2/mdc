import React, { useEffect, useState } from "react";
import { TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";
const initialValue = [
  {
    id: undefined,
    description: "Todos",
  },
  {
    id: "IN",
    description: "IN",
  },
  {
    id: "OUT",
    description: "OUT",
  },
];

export default function InOutSelect(props: any) {
  const [value, setValue] = React.useState<any>(
    props?.initialValue ?? initialValue[0]
  );
  const [data] = useState<any[]>(initialValue);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div style={{ width: "100%", minWidth: "126px" }}>
      <Autocomplete
        value={props?.value ?? value}
        options={data}
        loading
        getOptionLabel={(option: any) =>
          option?.description ? option.description : ""
        }
        onChange={(event: any, option: any) => {
          setValue(option);
          props.handlerChangeSelect("inOut", option);
        }}
        //inputValue={inputValue}
        onInputChange={(event, newInputValue) => {
          //setInputValue(newInputValue);
        }}
        renderInput={(params) => (
          <TextField
            {...params}
            name="inOut"
            variant="standard"
            label="IN/OUT:"
            error={props.error}
            placeholder="Informa IN/OUT"
            helperText={props.helperText}
          />
        )}
      />
    </div>
  );
}
