import * as React from 'react';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';


interface Props {
    cateogories: Array<any>;
    handleChange: (_: any, value: any) => void
}

const CategoriesAutoComplete: React.FunctionComponent<Props> = ({ cateogories, handleChange }) => {
    return (
        <Autocomplete
          disablePortal
          id="combo-box-categories"
          options={cateogories}
          sx={{ width: 'auto' }}
          onChange={handleChange}
          isOptionEqualToValue={(option, value) => option.id === value.id}
          renderInput={(params) => <TextField {...params} label="Category" />}
        />
      );
}

export default CategoriesAutoComplete;