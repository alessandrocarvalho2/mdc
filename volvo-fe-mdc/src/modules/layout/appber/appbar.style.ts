import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const drawerWidthWeb = 95;

const appbarStyles = makeStyles((theme: Theme) =>
  createStyles({
    appBar: {
      [theme.breakpoints.up('md')]: {
        width: `calc(100% - ${drawerWidthWeb}px)`,
      },
      transition: theme.transitions.create(['margin', 'width'], {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
      }),
    },
    appBarShift: {
      transition: theme.transitions.create(['margin', 'width'], {
        easing: theme.transitions.easing.easeOut,
        duration: theme.transitions.duration.enteringScreen,
      }),
    },
    menuButton: {
      marginRight: theme.spacing(2),
    },
    hide: {
      display: 'none',
    },
    alignCenter: {
      display: 'flex',
      alignContent: 'center',
      paddingTop: '0.5%',
      fontWeight: 'bold'
    },
    flexEnd: {
      display: 'flex',
      justifyContent: 'flex-end'
    },
    itemCenter: {
      display: 'flex',
      alignContent: 'center',
      justifyContent: 'center'
    }
  }),
);

export default appbarStyles;