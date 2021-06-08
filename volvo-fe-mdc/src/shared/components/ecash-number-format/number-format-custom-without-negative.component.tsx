import React from "react";
import NumberFormat from "react-number-format";

interface NumberFormatCustomWithOutNegativeProps {
  inputRef: (instance: NumberFormat | null) => void;
  onChange: (event: { target: { name: string; value: string } }) => void;
  name: string;
}

const MAX_VAL = 10000000000.00;
const withValueLimit = (inputObj: any) => {
  const { value } = inputObj;
  if (value < MAX_VAL) return inputObj;
};

export  const NumberFormatCustomWithOutNegative = (props: NumberFormatCustomWithOutNegativeProps) => {
  const { inputRef, onChange, ...other } = props;

  return (
    <NumberFormat
      {...other}
      getInputRef={inputRef}
      onValueChange={(values) => {
        onChange({
          target: {
            name: props.name,
            value: values.value,
          },
        });
      }}
      isAllowed={withValueLimit}
      decimalScale={2}
      decimalSeparator={','} 
      thousandSeparator={'.'}
      fixedDecimalScale
      isNumericString
      allowNegative={false}
    />
  );
};

export default NumberFormatCustomWithOutNegative
