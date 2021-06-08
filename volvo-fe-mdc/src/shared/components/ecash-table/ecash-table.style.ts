import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const ecashTableStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
      position: 'relative'
    },
    table: {
      minWidth: 500,
    },
    tableContainer: {
    },
    materialTable: {
      boxShadow: ''
    },
    pagination: {
      borderTop: '.3px solid #ddd',
    }
  }),
);

export default ecashTableStyles;
