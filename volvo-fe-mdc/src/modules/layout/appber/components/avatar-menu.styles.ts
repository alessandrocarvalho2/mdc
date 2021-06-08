import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const avatarMenuStyles = makeStyles((theme: Theme) =>
  createStyles({
    menuListItem: {
      display: 'flex',
      alignContent: 'center',
      padding: '4px 16px',
      lineHeight: '1.7',
    },
    listItemIcon: {
      minWidth: '30px',
    },
    icon: {
      fontSize: '1em',
      color: theme.palette.primary.main
    },
    text: {
      fontSize: '1em',
    },
    avatar: {
      cursor: 'pointer'
    }
  }),
);

export default avatarMenuStyles;