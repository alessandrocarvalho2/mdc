import React, { useEffect, useState } from "react";
import { TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";

import OperationService from "../../../core/services/operation.service";
import OperationModel from "../../../core/models/operation.model";

const initialValue: OperationModel = {
  id: undefined,
  code: "",
  description: "Todos",
};

export default function OperationSelect(props: any) {
  const operationService = new OperationService();
  const [value, setValue] = React.useState<OperationModel | null>(
    props?.initialValue ?? initialValue
  );
  const [data, setData] = useState<OperationModel[]>([]);

  const getList = () => {
    setData([]);
    let newData: any = [];
    newData.push(props?.initialValue ?? initialValue);
    operationService
      .getList(props?.filter)
      .then((resp: any) => {
        if (props?.value) {
          if (resp.filter((x: any) => x.id === props?.value.id).length === 0) {
            
            //props?.value(props?.initialValue ?? initialValue);
          }
        }

        resp.forEach((element: OperationModel) => {
          newData.push(element);
        });
        setData(newData);
      })
      .catch(() => {
      });
  };

  useEffect(() => {
    if (data.length === 0) {
      getList();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    //setData([]);
    setValue(props?.initialValue ?? initialValue);
    getList();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props?.filter?.categoryId]);

  return (
    <div style={{ width: "100%", minWidth: "126px" }}>
      <Autocomplete
        disabled={props?.disabled ?? false}
        value={props?.value ?? value}
        options={data}
        loading
        getOptionLabel={(option: any) =>
          option?.description && option?.code
            ? option.code + " - " + option.description
            : option.description
        }
        onChange={(event: any, option: any) => {
          setValue(option);
          props.handlerChangeSelect("operation", option);
        }}
        onInputChange={(event, newInputValue) => {
        }}
        renderInput={(params) => (
          <TextField
            {...params}
            name="operation"
            variant="standard"
            label="Operação:"
            error={props.error}
            placeholder="Informa Operação"
            helperText={props.helperText}
          />
        )}
      />
    </div>
  );
}
