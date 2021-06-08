import React, { useEffect, useState } from "react";
import {
  Checkbox,
  FormGroup,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  Typography,
} from "@material-ui/core";
import moment from "moment";
import statementListConciliationStyles from "./conciliation-list.style";

interface ColumnStatement {
  id:
    | "id"
    | "bankAccount.nickname"
    | "description"
    | "amount"
    | "color"
  label: string;
  width?: string;
  align?: "right" | "center" | "left";
  type?: "string" | "textfield" | "checkbox";
  format?: (value: any) => string;
}

let columnsStatement: ColumnStatement[] = [
  {
    id: "bankAccount.nickname",
    label: "Conta Corrente",
    align: "left",
    type: "string",
  },
  {
    id: "description",
    label: "Descrição",
    align: "left",
    type: "string",
  },
  {
    id: "amount",
    label: "Valor",
    align: "right",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
    type: "string",
  },
];

interface StatementListInterface {
  dataStatement: any;
  setDataStatement: any;
}

const ConciliationList = (props: StatementListInterface) => {
  const { dataStatement, setDataStatement } = props;
  const classes = statementListConciliationStyles();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [total, setTotal] = useState(0);
  //const [dataCash, setDataCash] = useState<any>([]);

  const widthCash = 100 / columnsStatement.length;
  const width = 100 / 4;

  columnsStatement.forEach((element) => {
    element.width = widthCash.toString() + "%";
  });

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  const handleChange = (event: any, i: number) => {
    //const name = event.target.name;
    const checked = event.target.checked;

    const newData = dataStatement.map((item: any, index: number) => {
      if (index === i) {
        const updatedItem = {
          ...item,
          checkedStatement: checked,
        };

        return updatedItem;
      }

      return item;
    });

    setDataStatement(newData);
  };

 
  useEffect(() => {

    setTotal(dataStatement.length);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className={classes.root}>
      <TableContainer className={classes.containertable}>
        <Table size={"small"} stickyHeader aria-label="sticky table">
          <TableHead>
            <TableRow>
              <TableCell colSpan={4} style={{ width: "100%" }} align="center">
                EXTRATO BÁNCARIO CONCILIADO
              </TableCell>
            </TableRow>
            <TableRow>
              {columnsStatement.map((column) => (
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
            {dataStatement
              .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
              .map((row: any, index: any) => {
                return (
                  <TableRow hover role="checkbox" tabIndex={-1} key={index}>
                    {columnsStatement.map((column) => {
                      let value = row[column.id];
                      let names = column.id.split(".");
                      if (dataStatement.length > 0 && names.length === 2) {
                        const obj = row[names[0]][names[1]];
                        value = obj;
                      } else if (
                        dataStatement.length > 0 &&
                        names.length === 3
                      ) {
                        const obj = row[names[0]][names[1]][names[2]];
                        value = obj;
                      }

                      if (column.type === "checkbox") {
                        return (
                          <TableCell
                            className={classes.cellText}
                            key={column.id}
                            align={column.align}
                          >
                            <FormGroup>
                              <Typography align={column.align} component="div">
                                <Checkbox
                                  checked={value}
                                  size="small"
                                  name="checkedCash"
                                  onChange={(e) => handleChange(e, index)}
                                  inputProps={{
                                    "aria-label": "primary checkbox",
                                  }}
                                />
                              </Typography>
                            </FormGroup>
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
            {dataStatement.length === 0 && (
              <TableRow>
                <TableCell align="center" colSpan={4}>
                  Resultado não encontrado.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
          <TablePagination
        rowsPerPageOptions={[
          5,
          10,
          25,
          { label: "All", value: dataStatement.length },
        ]}
        component="div"
        count={total}
        className={classes.head}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={handleChangePage}
        onChangeRowsPerPage={handleChangeRowsPerPage}
      />
    </div>
  );
};

export default ConciliationList;
