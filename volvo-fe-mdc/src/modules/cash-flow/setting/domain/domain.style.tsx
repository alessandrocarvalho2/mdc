import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const domainStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%'
    },
    title: {
      fontSize: '2em',
      fontWeight: 300,
    },
    icon: {
      marginRight: '6px'
    },
    buttonText: {
      textTransform: 'initial'
    },
    filters: {
      paddingBottom: '24px'
    },
    search: {
      height: '72px',
      display: 'flex',
      alignItems: 'flex-end',
    }
  }),
);

export default domainStyles;