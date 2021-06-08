import React, { useState } from "react";
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
import statementListStyles from "./statement-list.style";
import EcashTablePaginationActions from "../../../../../../shared/components/ecash-table-pagination-actions/ecash-table-pagination-actions";
import Columns from "../../../../../../core/models/columns-table.model";
import sortingTable from "../../../../../../core/utils/sorting-table.util";
import EcashTableHead from "../../../../../../shared/components/ecash-table-head/ecash-table-head.component";

let columns: Columns[] = [
  {
    id: "index",
    label: "#",
    align: "left",
    type: "index",
    width: "5",
  },
  // {
  //   id: "bankAccount.nickname",
  //   label: "Conta",
  //   align: "left",
  //   type: "string",
  //   width: "5",
  // },
  {
    id: "description",
    label: "Descrição",
    align: "left",
    type: "string",
    width: "55",
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
    applyColorMonetary: true,
    width: "35",
  },
  {
    id: "checkedStatement",
    label: "OK",
    align: "center",
    type: "checkbox",
    width: "5",
  },
];

interface StatementListInterface {
  data: any;
  setData: any;
  totalAmount: any;
}

const StatementList = (props: StatementListInterface) => {
  const { data, setData, totalAmount } = props;
  const classes = statementListStyles();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(data.length);

  const width = 100 / columns.length;

  columns.forEach((element) => {
    element.width = element.width
      ? element.width.toString() + "%"
      : width.toString() + "%";
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
    const name = event.target.name;
    const checked = event.target.checked;

    const newData = data.map((item: any, index: number) => {

      if (item.index === i) {
        const updatedItem = {
          ...item,
          [name]: checked,
        };

        return updatedItem;
      }

      return item;
    });

    setData(newData);
  };

  const [order, setOrder] = React.useState<Order>("asc");
  const [orderBy, setOrderBy] = React.useState<Columns>(columns[0]);

  type Order = "asc" | "desc";
  const handleRequestSort = (
    event: React.MouseEvent<unknown>,
    head: Columns
  ) => {
    const isAsc = orderBy?.id === head.id && order === "asc";
    setOrder(isAsc ? "desc" : "asc");
    setOrderBy(head);
  };

  return (
    <div className={classes.root}>
      <TableContainer className={classes.containertable}>
        <Table size={"small"} stickyHeader aria-label="sticky table">
          <EcashTableHead
            order={order}
            titleTable={"EXTRATO BANCÁRIO"}
            orderBy={orderBy}
            headCells={columns}
            onRequestSort={handleRequestSort}
          />
          <TableBody>
            {(rowsPerPage > 0
              ? sortingTable
                  .stableSort(
                    data,
                    sortingTable.getComparator(order, orderBy.id)
                  )
                  .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
              : sortingTable.stableSort(
                  data,
                  sortingTable.getComparator(order, orderBy.id)
                )
            ).map((row: any, index: any) => {
              return (
                <TableRow hover role="checkbox" tabIndex={-1} key={index}>
                  {columns.map((column: any) => {
                    let value = row[column.id];
                    let name = column.id;
                    let names = column.id.split(".");
                    if (data.length > 0 && names.length === 2) {
                      const obj = row[names[0]][names[1]];
                      name = names[1];
                      value = obj;
                    } else if (data.length > 0 && names.length === 3) {
                      const obj = row[names[0]][names[1]][names[2]];
                      name = names[2];
                      value = obj;
                    }

                    if (column.type === "checkbox") {
                      return (
                        <TableCell
                          className={classes.cellText}
                          key={column.id}
                          align={column.align}
                          //style={{ width: column.width }}
                        >
                          <FormGroup>
                            <Typography align={column.align} component="div">
                              <Checkbox
                                checked={value}
                                size="small"
                                name={name}
                                onChange={(e) => handleChange(e, row.index)}
                                inputProps={{
                                  "aria-label": "primary checkbox",
                                }}
                              />
                            </Typography>
                          </FormGroup>
                        </TableCell>
                      );
                    } else {
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
                            //style={{ width: column.width }}
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
                            //style={{ width: column.width }}
                          >
                            {column.format ? column.format(value) : value}
                          </TableCell>
                        );
                      }
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
      <TableContainer>
        <Table size={"small"} stickyHeader aria-label="sticky table">
          <TableHead>
            {(() => {
              const classeText =
                totalAmount < 0 ? classes.cellTextRed : classes.cellTextBlue;
              return (
                <TableRow>
                  {/* <TableCell style={{ width: "25%" }}> </TableCell> */}
                  <TableCell align="right" style={{ width: "25%" }} colSpan={2}>
                    TOTAL:
                  </TableCell>
                  {/* <TableCell style={{ width: "65%" }}> </TableCell> */}
                  <TableCell
                    align="right"
                    style={{ width: "65%" }}
                    colSpan={2}
                    className={classeText}
                  >
                    {totalAmount.toLocaleString("pt-BR", {
                      minimumFractionDigits: 2,
                      maximumFractionDigits: 2,
                    })}
                  </TableCell>
                  <TableCell
                    align="left"
                    style={{ width: "10%" }}
                    colSpan={1}
                    className={classeText}
                  >
                    {" "}
                  </TableCell>
                </TableRow>
              );
            })()}
          </TableHead>
        </Table>
      </TableContainer>
      <TablePagination
        rowsPerPageOptions={[5, 10, 25, { label: "All", value: data.length }]}
        component="div"
        count={data.length}
        className={classes.head}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={handleChangePage}
        onChangeRowsPerPage={handleChangeRowsPerPage}
        ActionsComponent={EcashTablePaginationActions}
      />
    </div>
  );
};

export default StatementList;
