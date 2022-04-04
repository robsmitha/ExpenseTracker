import { useState } from 'react';
import { Outlet, useNavigate } from "react-router-dom";

import { UserContext } from './../context/UserContext'
import { authService } from './../services/auth.service'

import { styled, Theme, CSSObject } from '@mui/material/styles';
import Box from '@mui/material/Box';
import MuiDrawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import CssBaseline from '@mui/material/CssBaseline';
import Divider from '@mui/material/Divider';
import MenuIcon from '@mui/icons-material/Menu';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import InboxIcon from '@mui/icons-material/MoveToInbox';

import AppBar from './AppBar';

const drawerWidth = 240;

const openedMixin = (theme: Theme): CSSObject => ({
  width: drawerWidth,
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.enteringScreen,
  }),
  overflowX: 'hidden',
});

const closedMixin = (theme: Theme): CSSObject => ({
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  overflowX: 'hidden',
  width: `calc(${theme.spacing(7)} + 1px)`,
  [theme.breakpoints.up('sm')]: {
    width: `calc(${theme.spacing(9)} + 1px)`,
  },
});

const DrawerHeader = styled('div')(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'flex-end',
  padding: theme.spacing(0, 1),
  // necessary for content to be below app bar
  ...theme.mixins.toolbar,
}));

const Drawer = styled(MuiDrawer, { shouldForwardProp: (prop) => prop !== 'open' })(
  ({ theme, open }) => ({
    width: drawerWidth,
    flexShrink: 0,
    whiteSpace: 'nowrap',
    boxSizing: 'border-box',
    ...(open && {
      ...openedMixin(theme),
      '& .MuiDrawer-paper': openedMixin(theme),
    }),
    ...(!open && {
      ...closedMixin(theme),
      '& .MuiDrawer-paper': closedMixin(theme),
    }),
  }),
);

export default function DefaultLayout() {
  const navigate = useNavigate();
  const [open, setOpen] = useState(true); // TODO: based on screen size
  const [ authenticated, setAuthenticated] = useState<boolean>(() => {
      const appUser = authService.getAppUser()
      return appUser?.authenticated === true
  });
  const [publicPages] = useState([
    {
      text: "Home",
      path: '/',
      icon : <MenuIcon />
    }
  ]);
  const [drawerItems] = useState([
    {
      text: "Accounts",
      path: '/accounts',
      icon : <MenuIcon />
    },
    {
      text: "Link Account",
      path: '/link-account',
      icon : <InboxIcon />
    }
  ]);

  const handleDrawerOpen = () => {
    setOpen(true);
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };

  return (
    <UserContext.Provider value={{
        authenticated,
        setAuthenticated
    }}>
      <Box sx={{ flexGrow: 1, display: 'flex' }}>
        <CssBaseline />
        <AppBar openMenuFunc={() => { open ? handleDrawerClose() : handleDrawerOpen() }} />
        <Drawer 
        variant="permanent" 
        open={open}
        >
          <DrawerHeader />
          <List>
            {publicPages.map(page => (
              <ListItem button key={page.text} onClick={() => navigate(page.path)}>
                <ListItemIcon>
                  {page.icon}
                </ListItemIcon>
                <ListItemText primary={page.text} />
              </ListItem>
            ))}
          </List>
          <Divider />
          <List>
            {authenticated && drawerItems.map(page => (
              <ListItem button key={page.text} onClick={() => navigate(page.path)}>
                <ListItemIcon>
                  {page.icon}
                </ListItemIcon>
                <ListItemText primary={page.text} />
              </ListItem>
            ))}
          </List>
        </Drawer>
        <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
          <DrawerHeader />
          <Outlet />
        </Box>
      </Box>
    </UserContext.Provider>
  );
}
