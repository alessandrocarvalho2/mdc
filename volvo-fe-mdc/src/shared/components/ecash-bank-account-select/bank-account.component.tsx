import React, { useEffect, useState } from "react";
import { TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";

import BankAccountService from "../../../core/services/bank-account.service";

const options = [{ id: undefined, name: "Selecione" }];

export default function BankAccountSelect(props: any) {
  const bankAccountService = new BankAccountService();
  const [bankAccountList, setBankAccountList] = useState<any>([]);
  const [value, setValue] = React.useState<any>(
    props?.initialValue ?? options[0]
  );

  const getListBankAccount = () => {
    bankAccountService.getAll().then((resp: any) => {
      let data = [];
      data.push(options[0]);
      resp.forEach((element: any) => {
        data.push(element);
      });

      setBankAccountList(data);
      // if (inputValue) {
      //   setValue(data.filter((x: any) => x.id === inputValue)[0]);
      // }
    });
  };

  useEffect(() => {
    if (bankAccountList.length === 0) {
      getListBankAccount();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div style={{ width: "100%", minWidth: "126px" }}>
      <Autocomplete
        disabled={props?.disabled}
        value={props?.value ?? value}
        options={bankAccountList}
        loading
        getOptionLabel={(option: any) =>
          option.account
            ? option.nickname +
              " | Ag: " +
              option.agency +
              " | C/C: " +
              option.account
            : "Selecione"
        }
        //inputValue={inputValue}
        onChange={(event: any, option: any) => {
          setValue(option);
          props.handlerChangeSelect("bankAccount", option);
        }}
        onInputChange={(event, newInputValue) => {
        }}
        renderInput={(params) => (
          <TextField
            {...params}
            name="bankAccount"
            variant="standard"
            label="Conta Corrente:"
            placeholder="Informe uma conta"
            error={props.error}
            helperText={props.helperText}
          />
        )}
      />
    </div>
  );
}
