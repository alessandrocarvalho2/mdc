import React from "react";
import clsx from "clsx";
import { useLocation, useHistory } from "react-router-dom";
import { AppBar, Tabs, Tab } from "@material-ui/core";
import subHeaderTabs from "./sub-header-tabs";
import { subHeaderItem } from "./sub-header.interface";
import subHeaderStyles from "./sub-header.style";

export default function SubHeader() {
  let classes = subHeaderStyles();
  let [, setValue] = React.useState(0);
  let history = useHistory();
  let location = useLocation();
  let pathname: string[] = location.pathname.split("/");
  let module: string = pathname.slice(2, 3).toString();

  let userAccess: any = localStorage.getItem("access");
  userAccess = JSON.parse(userAccess);

  const handleChange = (event: React.ChangeEvent<{}>, newValue: number) => {
    setValue(newValue);
  };

  const handleClick = (item: subHeaderItem) => {
    history.push(`/cash-flow/${module}/${item.url}`);
  };

  const activeRoute = (routeName: any) => {
    return location.pathname.indexOf(routeName) > -1 ? classes.activeTab : "";
  };

  return (
    <>
      {module && (
        <div className={classes.root}>
          <AppBar className={classes.tabs} position="static" color="inherit">
            <Tabs
              value={false}
              onChange={handleChange}
              variant="scrollable"
              scrollButtons="on"
              indicatorColor="secondary"
              aria-label="scrollable force tabs example"
            >
              {subHeaderTabs[module].map(
                (item: subHeaderItem) =>
                  userAccess.includes(item.access) && (
                    <Tab
                      onClick={() => {
                        handleClick(item);
                      }}
                      label={item.title}
                      value={item.url}
                      key={item.title}
                      className={clsx(classes.tab, activeRoute(item.url))}
                    />
                  )
              )}
            </Tabs>
          </AppBar>
        </div>
      )}
    </>
  );
}
