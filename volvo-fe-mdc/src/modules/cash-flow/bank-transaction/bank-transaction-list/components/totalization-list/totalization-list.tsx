import React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@material-ui/core";
import totalizationListStyles from "./totalization-list.style";
import validation from "../../../../../../core/utils/validation.util";

interface TotalizationListInterfaceProp {
  data: any;
  columns: Columns[];
  titleTable?: string;
  totalData?: any;
}

interface Columns {
  id: string;
  label: string;
  applyColorMonetary?: boolean;
  width?: string;
  align?: "left" | "center" | "right";
  type?: "index" | "string" | "textfield" | "checkbox" | "monetary";
  format?: (value: any) => string;
}

const TotalizationList = (props: TotalizationListInterfaceProp) => {
  const { data, columns, titleTable, totalData } = props;
  const classes = totalizationListStyles();

  const width = 100 / columns.length;

  columns.forEach((element) => {
    element.width = element.width ?? width.toString() + "%";
  });

  return (
    <div>
      <TableContainer className={classes.containertable}>
        <Table size={"small"} stickyHeader aria-label="sticky table">

          <TableHead>
            {titleTable && (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  style={{ width: "100%" }}
                  align="center"
                >
                  {titleTable}
                </TableCell>
              </TableRow>
            )}
            <TableRow>
              {columns.map((column) => (
                <TableCell
                  className={classes.head}
                  key={column.id}
                  align={column.align}
                  style={{ width: column.width }}
                >
                  {column.label}
                </TableCell>
              ))}
            </TableRow>
          </TableHead>
          <TableBody>
            {data.map((row: any, index: any) => {
              return (
                <TableRow hover role="checkbox" tabIndex={-1} key={index}>
                  {columns.map((column: any) => {
                    let value =
                      column?.type && column?.type === "index"
                        ? index + 1
                        : row[column.id];
                    let names = column.id.split(".");
                    if (data.length > 0 && names.length === 2) {
                      const obj = row[names[0]][names[1]];
                      value = obj;
                    } else if (data.length > 0 && names.length === 3) {
                      const obj = row[names[0]][names[1]][names[2]];
                      value = obj;
                    }

                    if (column.applyColorMonetary) {
                      const classeText = column.applyColorMonetary
                        ? parseFloat(value) < 0
                          ? classes.cellTextRed
                          : classes.cellTextBlue
                        : classes.cellTextRed;
                      return (
                        <TableCell
                          className={classeText}
                          key={column.id}
                          align={column.align}
                        >
                          {column.format ? column.format(value) : value}
                        </TableCell>
                      );
                    } else {
                      return (
                        <TableCell
                          className={classes.cellText}
                          key={column.id}
                          align={column.align}
                        >
                          {column.format ? column.format(value) : value}
                        </TableCell>
                      );
                    }
                  })}
                </TableRow>
              );
            })}
            {data.length === 0 && (
              <TableRow>
                <TableCell align="center" colSpan={columns.length}>
                  Resultado não encontrado.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
      {totalData !== undefined && totalData !== null && (
        <TableContainer className={classes.containertable}>
          <Table size={"small"} stickyHeader aria-label="sticky table">
            <TableHead>
              <TableRow>
                {totalData.map((value: any, index: number) => {
                  if (validation.isNumber(value)) {
                    const classeText =
                      parseFloat(value) < 0
                        ? classes.cellTextRed
                        : classes.cellTextBlue;
                    return (
                      <TableCell
                        className={classeText}
                        key={index}
                        align={"right"}
                        style={{ width: width + "%" }}
                      >
                        {value.toLocaleString("pt-BR", {
                          minimumFractionDigits: 2,
                          maximumFractionDigits: 2,
                        })}
                      </TableCell>
                    );
                  } else {
                    return (
                      <TableCell
                        className={classes.head}
                        key={index}
                        align={"right"}
                        style={{ width: width + "%" }}
                      >
                        {value}
                      </TableCell>
                    );
                  }
                })}
              </TableRow>
            </TableHead>
          </Table>
        </TableContainer>
      )}
    </div>
  );
};

export default TotalizationList;
