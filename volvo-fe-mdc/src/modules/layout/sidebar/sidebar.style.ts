import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const drawerWidth = 240;
const drawerWidthWeb = 95;

const sidebarStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
    },
    drawer: {
      [theme.breakpoints.up('md')]: {
        width: drawerWidthWeb,
      },
      width: drawerWidth,
      flexShrink: 0,
    },
    drawerPaper: {
      [theme.breakpoints.up('md')]: {
        width: drawerWidthWeb,
      },
      width: drawerWidth,
      background: theme.palette.primary.main
    },
    drawerHeader: {
      display: 'flex',
      alignItems: 'center',
      padding: theme.spacing(0, 1),
      ...theme.mixins.toolbar,
      justifyContent: 'flex-end',
    },
    listItem: {
      display: 'flex',
      flexFlow: 'column',
      justifyContent: 'center',
      alignItems: 'center',
    },
    listItemIcon: {
      display: 'flex',
      justifyContent: 'center',
      color: '#fff'
    },
    listItemText: {
      textAlign: 'center',
      color: '#fff'
    },
    control: {
      padding: '16px',

    },
    selected: {
      backgroundColor: 'rgb(10, 76, 150) !important'
    },
  }),
);

export default sidebarStyles;