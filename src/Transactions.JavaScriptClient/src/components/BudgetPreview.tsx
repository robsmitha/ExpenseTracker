import * as React from 'react';
import { useNavigate } from 'react-router-dom';

import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardActions from '@mui/material/CardActions';
import Avatar from '@mui/material/Avatar';
import { green } from '@mui/material/colors';
import Button from '@mui/material/Button';
import PreviewMenu from './PreviewMenu';

interface Props {
  item: any;
}

const BudgetPreview: React.FunctionComponent<Props> = ({ item }) => {
  const navigate = useNavigate();

  return (
    <Card sx={{ maxWidth: 345 }}>
      <CardHeader
        avatar={
          <Avatar sx={{ bgcolor: green[500] }} aria-label="recipe">
            {item.name.charAt(0).toUpperCase()}
          </Avatar>
        }
        title={item.name}
        subheader={`${new Date(item.startDate).toLocaleDateString()} - ${new Date(item.endDate).toLocaleDateString()}`}
        action={<PreviewMenu items={[
            { text: 'Manage', click: () => navigate(`/budget/${item.id}`) },  
            { text: 'Delete', click: () => { console.log('Not Implemented') } },
        ]} />}
      />
    </Card>
  );
}

export default BudgetPreview;