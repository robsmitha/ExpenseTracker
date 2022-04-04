import * as React from 'react';
import {
  Grid
} from '@mui/material';
import AccountPreview from './AccountPreview'


interface Props {
    items: Array<any>;
}

const AccountList: React.FunctionComponent<Props> = ({ items }) => {
  return (
    <Grid container spacing={2}>
      {items.map((item) => {
        return <Grid item md={3} key={item.item.itemId}>
          <AccountPreview item={item} />
          </Grid>
      })}
      
    </Grid>
  )
}

export default AccountList;