import React, { useEffect, useState } from "react";
import { TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";
const initialValue = [
  {
    id: undefined,
    description: "Todos",
  },
  {
    id: false,
    description: "Preenchido",
  },
  {
    id: true,
    description: "Não preenchido",
  },
];

export default function IncludeSelect(props: any) {
  const [value, setValue] = React.useState<any>(
    props?.initialValue ?? initialValue[0]
  );
  const [data] = useState<any>(initialValue);
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
          props.handlerChangeSelect("include", option);
        }}
        onInputChange={(event, newInputValue) => {}}
        renderInput={(params) => (
          <TextField
            {...params}
            name="include"
            variant="standard"
            label="Preenchido:"
            error={props.error}
            placeholder="Informe se preenchido"
            helperText={props.helperText}
          />
        )}
      />
    </div>
  );
}
