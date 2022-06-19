import * as React from 'react';
import { useNavigate } from 'react-router-dom';

import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardActions from '@mui/material/CardActions';
import Avatar from '@mui/material/Avatar';
import { red } from '@mui/material/colors';
import Button from '@mui/material/Button';

interface Props {
  item: any
}

const AccountPreview: React.FunctionComponent<Props> = ({ item }) => {
  const navigate = useNavigate();

  return (
    <Card sx={{ maxWidth: 345 }}>
      <CardHeader
        avatar={
          <Avatar sx={{ bgcolor: red[500] }} aria-label="recipe">
            {item.institution.name.charAt(0).toUpperCase()}
          </Avatar>
        }
        title={item.institution.name}
        subheader={`${new Date(item.item.lastSuccessfulUpdate).toLocaleDateString()} ${new Date(item.item.lastSuccessfulUpdate).toLocaleTimeString()}`}
      />
      {/* <CardActions>
        <Button
          onClick={() => navigate(`/transactions/${item.item.itemId}`)}
        >
          Transactions
        </Button>
      </CardActions> */}
    </Card>
  );
}

export default AccountPreview;