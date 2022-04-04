import * as React from 'react';
import { useNavigate } from 'react-router-dom';
import { useTheme } from '@mui/material/styles';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import MenuItem from '@mui/material/MenuItem';
import { useUserContext } from './../context/UserContext'

interface Props {
    openMenuFunc?: () => void;
}

const ResponsiveAppBar: React.FunctionComponent<Props> = ({ openMenuFunc }) => {
    const navigate = useNavigate();
    const theme = useTheme();
    const [b2cLoginUrl] = React.useState(process.env.REACT_APP_B2C_SIGN_UP_SIGN_IN_ENDPOINT || '');
    const { authenticated } = useUserContext()
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };


    return (
        <AppBar 
          position="fixed" 
          sx={{ zIndex: theme.zIndex.drawer + 1 }}
        >
          <Container maxWidth="xl">
            <Toolbar disableGutters>
            <IconButton
                color="inherit"
                aria-label="open drawer"
                onClick={openMenuFunc}
                edge="start"
                sx={{
                  marginRight: '36px',
                }}
              >
                <MenuIcon />
              </IconButton>
              <Button
                    onClick={() => navigate('/')}
                    sx={{ my: 2, color: 'white', display: 'block' }}
                >
                    Expense Tracker
                </Button>
              <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }} />
              {!authenticated 
              ? <Box sx={{ flexGrow: 0 }}>
                  <Button 
                      href={b2cLoginUrl}
                      sx={{ my: 2, color: 'white', display: 'block' }}
                  >
                      Sign in
                  </Button>
              </Box>
              : <Box sx={{ flexGrow: 0 }}>
                  <Tooltip title="Open settings" placement="left">
                  <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                    <Avatar>E</Avatar>
                  </IconButton>
                  </Tooltip>
                  <Menu
                  sx={{ mt: '45px' }}
                  id="menu-appbar"
                  anchorEl={anchorElUser}
                  anchorOrigin={{
                      vertical: 'top',
                      horizontal: 'right',
                  }}
                  keepMounted
                  transformOrigin={{
                      vertical: 'top',
                      horizontal: 'right',
                  }}
                  open={Boolean(anchorElUser)}
                  onClose={handleCloseUserMenu}
                  >
                  <MenuItem onClick={() => navigate('sign-out')}>
                      <Typography textAlign="center">Sign Out</Typography>
                  </MenuItem>
                  </Menu>
              </Box>}
            </Toolbar>
          </Container>
        </AppBar>
    );
};
export default ResponsiveAppBar;
