import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const drawerWidthWeb = 95;

const subHeaderStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
      width: `calc(100% - ${drawerWidthWeb}px)`,
      marginLeft: drawerWidthWeb,
      position: 'fixed',
      zIndex: 1099,
      top: '64px',
      backgroundColor: '#fff',
    },
    tabs: {
      boxShadow: 'none',
      borderBottom: 'solid 1px #D4D4D4'
    },
    tab: {
      textTransform: 'initial'
    },
    activeTab: {
      borderBottom: '2px solid',
      borderBottomColor: theme.palette.secondary.main,
      opacity: 1
    },
  }),
);

export default subHeaderStyles;