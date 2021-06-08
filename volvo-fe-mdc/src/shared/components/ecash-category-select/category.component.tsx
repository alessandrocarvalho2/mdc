import React, { useEffect, useState } from "react";
import { TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";

import CategoryService from "../../../core/services/category.service";
import CategoryModel from "../../../core/models/category.model";

const initialValue: CategoryModel = {
  id: undefined,
  description: "Todos",
};

export default function CategorySelect(props: any) {
  const categoryService = new CategoryService();
  const [value, setValue] = React.useState<CategoryModel | null>(
    props?.initialValue ?? initialValue
  );
  const [data, setData] = useState<CategoryModel[]>([]);

  const getList = () => {
    
    let newData = data;
    newData.push(props?.initialValue ?? initialValue);
    categoryService.getList(props?.filter).then((resp: any) => {
      resp.forEach((element: CategoryModel) => {
        newData.push(element);
      });
      setData(newData);
    });
  };

  useEffect(() => {
    if (data.length === 0) {
      getList();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div style={{ width: "100%", minWidth: "126px" }}>
      <Autocomplete
        disabled={props?.disabled}
        value={props?.value ?? value}
        options={data}
        loading
        getOptionLabel={(option: any) =>
          option?.description ? option.description : ""
        }
        onChange={(event: any, option: any) => {
          setValue(option);
          props.handlerChangeSelect("category", option);
        }}
        onInputChange={(event, newInputValue) => {
        }}
        renderInput={(params) => (
          <TextField
            {...params}
            name="category"
            variant="standard"
            label="Categoria:"
            error={props.error}
            placeholder="Informa categoria"
            helperText={props.helperText}
          />
        )}
      />
    </div>
  );
}
