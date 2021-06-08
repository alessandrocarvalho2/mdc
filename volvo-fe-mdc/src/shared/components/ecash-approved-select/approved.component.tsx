import React, { useEffect, useState } from "react";
import { TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";
const initialValue = [
  {
    id: undefined,
    description: "Todos",
  },
  {
    id: 0,
    description: "Pré Aprovado",
  },
  {
    id: 1,
    description: "A Aprovar",
  },
  {
    id: 2,
    description: "Aprovado",
  },
];

export default function ApprovedSelect(props: any) {
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
          props.handlerChangeSelect("approved", option);
        }}
        onInputChange={(event, newInputValue) => {}}
        renderInput={(params) => (
          <TextField
            {...params}
            name="approved"
            variant="standard"
            label="Aprovação:"
            error={props.error}
            placeholder="Informe a Aprovação"
            helperText={props.helperText}
          />
        )}
      />
    </div>
  );
}
