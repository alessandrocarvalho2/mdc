import React from "react";
import { useHistory, useLocation } from "react-router-dom";
import { useTheme } from "@material-ui/core/styles";
import {
  Drawer,
  useMediaQuery,
  Grid,
  Icon,
  List,
  ListItem,
  ListItemText,
  ListItemIcon,
} from "@material-ui/core";
import CssBaseline from "@material-ui/core/CssBaseline";
import sideBarItems from "./sidebar-items";
import sidebarStyles from "./sidebar.style";
import HasPermission from "../../../shared/components/ecash-has-permission/has-permission.component";
import VolvoLogo from "../../../shared/components/ecash-images/volvo-logo";

type SideBarProps = {
  open: boolean;
  handleClick: any;
};

export default function SideBar({ open, handleClick }: SideBarProps) {
  const classes = sidebarStyles();
  const theme = useTheme();
  const history = useHistory();
  const location = useLocation();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  const checkBrowser = navigator.userAgent.indexOf("Firefox") !== -1;

  const activeRoute = (routeName: any) => {
    return location.pathname.indexOf(routeName) > -1;
  };

  const drawerVariant = () => {
    return isMobile ? "temporary" : "permanent";
  };

  const selectModule = (
    url: string,
    event: React.MouseEvent<HTMLDivElement, MouseEvent>,
    index: number
  ) => {
    history.push(`/cash-flow/${url}`);
  };

  return (
    <div className={classes.root}>
      <CssBaseline />
      <Drawer
        className={classes.drawer}
        variant={drawerVariant()}
        anchor="left"
        open={open}
        onClick={() => handleClick(!open)}
        classes={{
          paper: classes.drawerPaper,
        }}
      >
        <Grid container justify="center" className={classes.control}>
          {checkBrowser && <VolvoLogo width="50px" height="30px" marginTop= "-8px"  /> }
          {!checkBrowser && <VolvoLogo width="50px" height="30px" marginTop= "-8px" />}
        </Grid>
        <List>
          {sideBarItems.map((item, index) => (
            <HasPermission key={item.title} rules={item.access}>
              <ListItem
                button
                className={classes.listItem}
                selected={activeRoute(item.url)}
                onClick={(event) => {
                  selectModule(item.url, event, index);
                }}
                classes={{ selected: classes.selected }}
              >
                <ListItemIcon className={classes.listItemIcon}>
                  <Icon>{item.icon}</Icon>
                </ListItemIcon>
                <ListItemText className={classes.listItemText}>
                  {item.title}
                </ListItemText>
              </ListItem>
            </HasPermission>
          ))}
        </List>
      </Drawer>
    </div>
  );
}
