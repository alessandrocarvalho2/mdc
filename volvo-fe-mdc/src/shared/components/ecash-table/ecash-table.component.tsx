import React, { forwardRef, useState, useEffect } from "react";
import { Paper, TablePagination } from "@material-ui/core";
import ecashTableStyles from "./ecash-table.style";
//import { useTranslation } from "react-i18next";
import KeyboardArrowUpIcon from "@material-ui/icons/KeyboardArrowUp";
import MaterialTable from "material-table";
import resetMaterialTable from "./reset.ecash-table.style";
import { MuiThemeProvider } from "@material-ui/core";

export default function EcashTable(props: any) {
  //let userAccess: any = localStorage.getItem("access");
  //userAccess = JSON.parse(userAccess);
  const classes = ecashTableStyles();
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [data, setData] = useState(props.data);
  //const { t } = useTranslation();

  const handleChangePage = (event: unknown, newPage: number) => {
    props.setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    props.setRowsPerPage(+event.target.value);
    setRowsPerPage(+event.target.value);
    props.setPage(0);
  };

  useEffect(() => {
    setData(props.data);
  }, [props.data]);

  return (
    <>
      <Paper>
        <MuiThemeProvider theme={resetMaterialTable}>
          <MaterialTable
            isLoading={props.loading}
            columns={props.columns}
            data={data}
            options={{
              toolbar: false,
              paging: false,
              minBodyHeight: "322px",
              actionsColumnIndex: -1,
            }}
            icons={{
              SortArrow: forwardRef((props, ref) => (
                <KeyboardArrowUpIcon {...props} ref={ref} />
              )),
            }}
            localization={{
              body: {
                emptyDataSourceMessage: "Resultado não encontrado.",
              },
              header: {
                actions: "Ações",
              },
            }}
          />
        </MuiThemeProvider>

        {props.withPagination ? (
          <TablePagination
            rowsPerPageOptions={[5, 10, 15]}
            component="div"
            onChangePage={handleChangePage}
            onChangeRowsPerPage={handleChangeRowsPerPage}
            count={props.total}
            rowsPerPage={rowsPerPage}
            page={props.page}
            className={classes.pagination}
            labelRowsPerPage={`${rowsPerPage} ${"linhas"}`}
          />
        ) : (
          <>
           </>
        )}
      </Paper>
    </>
  );
}
