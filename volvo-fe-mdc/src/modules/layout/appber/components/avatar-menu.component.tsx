import React, { useState } from 'react';
import Avatar from '@material-ui/core/Avatar';
import { Menu, MenuItem, ListItemIcon } from '@material-ui/core';
import { SettingsOutlined, ExitToApp, HelpOutlineOutlined } from '@material-ui/icons';
import avatarMenuStyles from './avatar-menu.styles';
import LogoutService from '../../../../core/services/logout.service';
import ImgHolder from "../../../../assets/users/holderImg.png"

export default function AvatarMenu() {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const classes = avatarMenuStyles();
  const logoutService = new LogoutService();
  const [avatar] = useState(localStorage.getItem('avatar') || ImgHolder);
  
  
  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };
  
  const handleClose = () => {
    setAnchorEl(null);
  };
  
  const logout = () => {
    logoutService.logout();
  }
  
  return (
    <div>
      <Avatar 
        onClick={handleClick}
        aria-controls="customized-menu"
        aria-haspopup="true"
        src={avatar}
        className={classes.avatar}
        />
      <Menu
        id="customized-menu"
        anchorEl={anchorEl}
        keepMounted
        open={Boolean(anchorEl)}
        onClose={handleClose}
        getContentAnchorEl={null}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'center',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'center',
        }}
        >
        <MenuItem 
          classes={{ root: classes.menuListItem }}
          onClick={handleClose}>
          <ListItemIcon className={classes.listItemIcon}>
            <SettingsOutlined fontSize="small" className={classes.icon}/>
          </ListItemIcon>
          <span className={classes.text}>Configurações</span>
        </MenuItem>
        <MenuItem 
          classes={{ root: classes.menuListItem }}
          onClick={handleClose}>
          <ListItemIcon className={classes.listItemIcon}>
            <HelpOutlineOutlined fontSize="small" className={classes.icon}/>
          </ListItemIcon>
          <span className={classes.text}>Ajuda</span>
        </MenuItem>
        <MenuItem 
          classes={{ root: classes.menuListItem }}
          onClick={logout}>
          <ListItemIcon className={classes.listItemIcon}>
            <ExitToApp fontSize="small" className={classes.icon}/>
          </ListItemIcon>
          <span className={classes.text}>Sair</span>
        </MenuItem>
      </Menu>
    </div>
  );
}