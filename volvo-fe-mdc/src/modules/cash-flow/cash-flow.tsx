import React, { useState } from 'react';
import clsx from 'clsx';
import { makeStyles, Theme, createStyles, useTheme } from '@material-ui/core/styles';
import { useMediaQuery } from '@material-ui/core';
import SideBar from '../layout/sidebar/sidebar';
import CashFlowRoutes from './cash-flow.route';
import AppBar from '../layout/appber/appbar';
import SubHeader from '../layout/subheader/subheader';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
    },
    drawerHeader: {
      display: 'flex',
      alignItems: 'center',
      padding: theme.spacing(0, 1),
      minHeight: '104px',
      justifyContent: 'flex-end',
    },
    content: {
      flexGrow: 1,
      padding: theme.spacing(3),
      transition: theme.transitions.create('margin', {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
      }),
    },
    contentShift: {
      transition: theme.transitions.create('margin', {
        easing: theme.transitions.easing.easeOut,
        duration: theme.transitions.duration.enteringScreen,
      }),
      marginLeft: 0,
    },
  }),
);

export default function CashFlow(props: any) {
  const classes = useStyles();
  const [open, setOpen] = useState(false);
  const theme = useTheme();

  //let userAccess: any = localStorage.getItem("access");
  //userAccess = JSON.parse(userAccess);

  // console.log("CashFlow");
  // console.log(userAccess);
  
  const handleClick = (state: boolean) => {
    setOpen(state);
  };

  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  return (
    <>
      <div className={classes.root}>
        <AppBar
          open={open}
          handleClick={handleClick}
        />
        <SideBar open={open} handleClick={handleClick}/>
        {!isMobile && <SubHeader />}
        <main
          className={
            clsx(
              classes.content,
              {
                [classes.contentShift]: open,
              }
            )
          }
        >
          <div className={classes.drawerHeader} />
          <CashFlowRoutes></CashFlowRoutes>
        </main>
      </div>
    </>
  );
}