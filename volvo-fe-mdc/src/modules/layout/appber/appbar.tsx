import React from "react";
import clsx from "clsx";
import { useTheme } from "@material-ui/core/styles";
import {
  AppBar,
  useMediaQuery,
  Grid,
  Icon,
  Toolbar,
  IconButton,
  Typography,
} from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import AvatarMenu from "./components/avatar-menu.component";
import appbarStyles from "./appbar.style";

type AppBarProps = {
  open: boolean;
  handleClick: any;
};

export default function MainAppBar({ open, handleClick }: AppBarProps) {
  const classes = appbarStyles();
  const theme = useTheme();

  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  return (
    <AppBar
      position="fixed"
      color="inherit"
      className={clsx(classes.appBar, {
        [classes.appBarShift]: open,
      })}
    >
      <Toolbar>
        <Grid container alignItems="center">
          <Grid item className={classes.alignCenter} xs={8} md={6}>
            <IconButton
              color="inherit"
              aria-label="open drawer"
              onClick={() => handleClick(!open)}
              edge="start"
              className={clsx(classes.menuButton, !isMobile && classes.hide)}
            >
              <MenuIcon />
            </IconButton>
            <Typography
              align="justify"
              color="primary"
              gutterBottom
              variant="h4"
              component="h1"
            >
              Sistema eCash
            </Typography>
          </Grid>
          <Grid
            container
            justify="flex-end"
            alignItems="center"
            item
            xs={4}
            md={6}
          >
            {!isMobile && (
              // <Grid item md={8} className={classes.flexEnd}>
              //   <TextField
              //     id="standard-search"
              //     placeholder="Busca Rápida"
              //     type="search"
              //     InputProps={{
              //       endAdornment: (
              //         <InputAdornment position="end">
              //           <Icon>search</Icon>
              //         </InputAdornment>
              //       )
              //     }}
              //   />
              // </Grid>
              <></>
            )}
            <Grid item xs={6} md={2} className={classes.itemCenter}>
              <Icon color="primary" fontSize="large">
                notifications_none
              </Icon>
            </Grid>
            <Grid item xs={6} md={2} className={classes.itemCenter}>
              <AvatarMenu />
            </Grid>
          </Grid>
        </Grid>
      </Toolbar>
    </AppBar>
  );
}
