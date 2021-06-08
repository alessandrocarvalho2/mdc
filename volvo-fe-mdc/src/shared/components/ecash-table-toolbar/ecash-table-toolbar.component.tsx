import React from "react";
import FilterListIcon from "@material-ui/icons/FilterList";
import clsx from "clsx";
import ecashTableToolbarStyles from "./ecash-table-toolbar.style";
import {
  Icon,
  IconButton,
  Toolbar,
  Tooltip,
  Typography,
} from "@material-ui/core";

interface EcashTableToolbarProps {
  onClickButton?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  titleTable?: string;
  titleButton?: string;
  iconButton?: string;
  numSelected: number;
}

const EcashTableToolbar = (props: EcashTableToolbarProps) => {
  const { onClickButton, titleTable, numSelected} = props;

  const classes = ecashTableToolbarStyles();

  return (
    <Toolbar
      className={clsx(classes.root, {
        [classes.highlight]: numSelected > 0,
      })}
    >
      {numSelected > 0 ? (
        <Typography
          className={classes.title}
          color="inherit"
          variant="subtitle1"
          component="div"
        >
          {numSelected} selecionado(s)
        </Typography>
      ) : (
        <Typography
          className={classes.title}
          variant="h6"
          id="tableTitle"
          component="div"
        >
          {titleTable}
        </Typography>
      )}
      {numSelected > 0 ? (
        <Tooltip title="Deletar Itens" aria-label="add">
          <IconButton
            onClick={(e: any) => {
              if (onClickButton) onClickButton(e);
            }}
            aria-label="delete"
          >
            <Icon>delete</Icon>
          </IconButton>
        </Tooltip>
      ) : (
        <Tooltip title="Filter list">
          <IconButton aria-label="filter list">
            <FilterListIcon />
          </IconButton>
        </Tooltip>
      )}
    </Toolbar>
  );
};

export default EcashTableToolbar;
