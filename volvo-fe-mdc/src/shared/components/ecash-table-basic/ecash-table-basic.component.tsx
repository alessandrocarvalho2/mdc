import React, { useEffect, useState } from "react";
import {
  Checkbox,
  FormGroup,
  Icon,
  IconButton,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  Tooltip,
  Typography,
} from "@material-ui/core";
import ecashTableBasicStyles from "./ecash-table-basic.style";
import EcashTablePaginationActions from "../ecash-table-pagination-actions/ecash-table-pagination-actions";
import Columns from "../../../core/models/columns-table.model";
import EcashTableHead from "../ecash-table-head/ecash-table-head.component";
import sortingTable from "../../../core/utils/sorting-table.util";
import EcashTableToolbar from "../ecash-table-toolbar/ecash-table-toolbar.component";

interface EcashTableBasicInterfaceProp {
  data: any;
  selected?: any;
  setSelected?: any;
  columns: Columns[];
  titleTable?: string;
  totalAmount?: any;
  withPagination?: any;
  onDelete?: (item: any, event?: React.MouseEvent<HTMLDivElement>) => void;
  onEdit?: (item: any, event?: React.MouseEvent<HTMLDivElement>) => void;
  actionButtonAfterChecked?: (item: any) => void;
}

const EcashTableBasicList = (props: EcashTableBasicInterfaceProp) => {
  const {
    data,
    selected,
    setSelected,
    columns,
    titleTable,
    totalAmount,
    withPagination,
    actionButtonAfterChecked,
    onEdit,
    onDelete,
  } = props;
  const classes = ecashTableBasicStyles();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(
    withPagination !== undefined ? 10 : data.length
  );

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

  const actionDelete = (item?: any) => {
    if (onDelete) onDelete(item);
  };

  const actionEdit = (item?: any, event?: React.MouseEvent<HTMLDivElement>) => {
    if (onEdit) onEdit(item, event);
  };

  const handleChange = (event: any, row: any) => {
    const selectedIndex = selected.indexOf(row);
    let newSelected: any[] = [];
    console.log(selectedIndex);
    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, row);
    } else if (selectedIndex === 0) {
      newSelected = newSelected.concat(selected.slice(1));
    } else if (selectedIndex === selected.length - 1) {
      newSelected = newSelected.concat(selected.slice(0, -1));
    } else if (selectedIndex > 0) {
      newSelected = newSelected.concat(
        selected.slice(0, selectedIndex),
        selected.slice(selectedIndex + 1)
      );
    }

    setSelected(newSelected);
  };

  const isSelected = (row: any) => selected?.indexOf(row) !== -1;

  const handleSelectAllClick = (event: React.ChangeEvent<HTMLInputElement>) => {
    const checked = event.target.checked;

    if (checked) {
      const newSelecteds = data.map((n: any, index: number) => n);
      setSelected(newSelecteds);
      return;
    }
    setSelected([]);
  };

  useEffect(() => {
    //if (nameChecked) checkdAll(nameChecked, selected.length === data.length);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data]);

  return (
    <div className={classes.root}>
      <EcashTableToolbar
        onClickButton={actionButtonAfterChecked}
        titleTable={titleTable}
        numSelected={selected?.length}
      />
      <TableContainer className={classes.containertable}>
        <Table size={"small"} stickyHeader aria-label="sticky table">
          <EcashTableHead
            titleTable={titleTable}
            onSelectAllClick={handleSelectAllClick}
            rowCount={data.length}
            numSelected={selected?.length}
            order={order}
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
              const isItemSelected = isSelected(row);
              return (
                <TableRow hover role="checkbox" tabIndex={-1} key={index}>
                  {columns.map((column: any) => {
                    let value =
                      column?.type && column?.type === "index"
                        ? index + 1
                        : row[column.id];
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
                          style={{ width: column.width }}
                        >
                          <FormGroup>
                            <Typography align={column.align} component="div">
                              <Checkbox
                                checked={isItemSelected}
                                size="small"
                                name={name}
                                onChange={(e) => handleChange(e, row)}
                                inputProps={{
                                  "aria-label": "primary checkbox",
                                }}
                              />
                            </Typography>
                          </FormGroup>
                        </TableCell>
                      );
                    } else if (column.type === "button") {
                      return (
                        <TableCell
                          className={classes.cellText}
                          key={column.id}
                          align={column.align}
                          style={{ width: column.width }}
                        >
                          {column.children.map(
                            (columnChild: any, index: any) => {
                              return (
                                <Tooltip
                                  title={columnChild.label}
                                  aria-label="delete"
                                >
                                  <IconButton
                                    onClick={(e: any) => {
                                      if (columnChild.icon === "delete") {
                                        actionDelete(row);
                                      } else if (columnChild.icon === "edit") {
                                        actionEdit(row, e);
                                      }
                                    }}
                                    aria-label="delete"
                                  >
                                    <Icon>{columnChild.icon}</Icon>
                                  </IconButton>
                                </Tooltip>
                              );
                            }
                          )}
                        </TableCell>
                      );

                      // if (column.children.length > 0) {
                      //   column.children.map((columnChild: any, index: any) => {

                      //     return (
                      //       <TableCell
                      //         className={classes.cellText}
                      //         key={index}
                      //         align={columnChild.align}
                      //       >
                      //         {/* <IconButton
                      //           // onClick={() => onDetail(row)}
                      //           aria-label="delete"
                      //         >
                      //           <Icon>{columnChild.icon}</Icon>
                      //         </IconButton> */}
                      //         {"dnjsndjsdnjs sndjsn"}
                      //       </TableCell>
                      //     );
                      //   });
                      // }
                    } else {
                      if (column.applyColorMonetary) {
                        const classeText = column.applyColorMonetary
                          ? parseFloat(value) < 0
                            ? classes.cellTextRed
                            : classes.cellTextBlue
                          : classes.cellTextRed;
                        return (
                          <TableCell
                            style={{ width: column.width }}
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
                            style={{ width: column.width }}
                            className={classes.cellText}
                            key={column.id}
                            align={column.align}
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
      {totalAmount !== undefined && totalAmount !== null && (
        <TableContainer className={classes.containertable}>
          <Table size={"small"} stickyHeader aria-label="sticky table">
            <TableHead>
              {(() => {
                const classeText =
                  totalAmount < 0 ? classes.cellTextRed : classes.cellTextBlue;
                return (
                  <TableRow>
                    <TableCell
                      align="center"
                      style={{ minWidth: width }}
                      colSpan={2}
                    >
                      TOTAL:
                    </TableCell>
                    <TableCell
                      align="center"
                      style={{ minWidth: width }}
                      colSpan={2}
                      className={classeText}
                    >
                      {" "}
                      {totalAmount.toLocaleString("pt-BR", {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                      })}
                    </TableCell>
                  </TableRow>
                );
              })()}
            </TableHead>
          </Table>
        </TableContainer>
      )}
      {withPagination !== undefined && withPagination !== null && (
        <TablePagination
          rowsPerPageOptions={[5, 10, 25, { label: "All", value: data.length }]}
          component="div"
          count={data.length}
          className={classes.head}
          rowsPerPage={rowsPerPage}
          page={page}
          SelectProps={{
            inputProps: { "aria-label": "rows per page" },
            native: true,
          }}
          onChangePage={handleChangePage}
          onChangeRowsPerPage={handleChangeRowsPerPage}
          ActionsComponent={EcashTablePaginationActions}
        />
      )}
    </div>
  );
};

export default EcashTableBasicList;
