import React from "react";
import TableCell from "@material-ui/core/TableCell";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import TableSortLabel from "@material-ui/core/TableSortLabel";
import ecashTableHeadStyles from "./ecash-table-head.style";
import Columns from "../../../core/models/columns-table.model";
import Checkbox from "@material-ui/core/Checkbox";
import { FormControl, FormControlLabel, FormGroup } from "@material-ui/core";

type Order = "asc" | "desc";

interface EcashTableHeadProps {
  onSelectAllClick?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  numSelected?: number;
  onRequestSort: (event: React.MouseEvent<unknown>, head: Columns) => void;
  order: Order;
  orderBy: Columns;
  headCells: Columns[];
  titleTable?: string;
  rowCount?: number;
}

const EcashTableHead = (props: EcashTableHeadProps) => {
  const {
    order,
    orderBy,
    numSelected,
    rowCount,
    onRequestSort,
    onSelectAllClick,
    headCells,
    titleTable,
  } = props;

  const createSortHandler =
    (head: Columns) => (event: React.MouseEvent<unknown>) => {
      onRequestSort(event, head);
    };
  const classes = ecashTableHeadStyles();

  return (
    <TableHead>
      {titleTable && (
        <TableRow>
          <TableCell
            colSpan={headCells.length}
            style={{ width: "100%" }}
            align="center"
          >
            {titleTable}
          </TableCell>
        </TableRow>
      )}
      <TableRow>
        {headCells.map((headCell) => {
          if (headCell.type === "checkbox" && headCell.witnCheckedAll ) {
            return (
              <TableCell
                key={headCell.id}
                size="small"
                align={headCell.align}
                style={{ width: headCell.width }}
                padding={headCell.disablePadding ? "none" : "default"}
              >
                <FormControl
                  component="fieldset"
                  style={{
                    display: "flex",
                    alignItems: "center",
                  }}
                >
                  <FormGroup>
                    <FormControlLabel
                      control={
                        <Checkbox
                          size="small"
                          name={headCell.id}
                          indeterminate={
                            (numSelected ?? 0) > 0 &&
                            (numSelected ?? 0) < (rowCount ?? 0)
                          }
                          checked={
                            (rowCount ?? 0) > 0 &&
                            (numSelected ?? 0) === (rowCount ?? 0)
                          }
                          onChange={onSelectAllClick}
                          inputProps={{ "aria-label": "select all desserts" }}
                        />
                      }
                      label="Todos"
                      labelPlacement="end"
                    />
                  </FormGroup>
                </FormControl>
              </TableCell>
            );
          } else {
            return (
              <TableCell
                className={classes.head}
                key={headCell.id}
                align={headCell.align}
                style={{ width: headCell.width }}
                padding={headCell.disablePadding ? "none" : "default"}
                sortDirection={orderBy?.id === headCell.id ? order : false}
              >
                <TableSortLabel
                  style={{ width: headCell.width }}
                  active={orderBy?.id === headCell.id}
                  direction={orderBy?.id === headCell.id ? order : "asc"}
                  onClick={createSortHandler(headCell)}
                >
                  {headCell.label}
                  {orderBy?.id === headCell.id ? (
                    <span className={classes.visuallyHidden}>
                      {order === "desc"
                        ? "sorted descending"
                        : "sorted ascending"}
                    </span>
                  ) : null}
                </TableSortLabel>
              </TableCell>
            );
          }
        })}
      </TableRow>
    </TableHead>
  );
};

export default EcashTableHead;
