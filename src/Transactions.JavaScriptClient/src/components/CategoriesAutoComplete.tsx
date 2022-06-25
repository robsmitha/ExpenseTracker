import * as React from 'react';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';

const filter = createFilterOptions<CategoryOptionType>();

interface Props {
    cateogories: Array<any>;
    value: any | undefined;
    disabled?: boolean | undefined;
    setValue: (value: any) => void;
    label: string;
    errorText: string | null;
}

const CategoryAutoComplete: React.FunctionComponent<Props> = ({ cateogories, value, setValue, disabled, label, errorText }) => {
  const [open, toggleOpen] = React.useState(false);

  const handleClose = () => {
    setDialogValue({
      name: '',
      estimate: 0
    });
    toggleOpen(false);
  };

  const [dialogValue, setDialogValue] = React.useState({
    name: '',
    estimate: 0
  });

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setValue({
      name: dialogValue.name,
      estimate: dialogValue.estimate,
    });
    handleClose();
  };

  return (
    <React.Fragment>
      <Autocomplete
        value={value}
        disabled={disabled}
        onChange={(event, newValue) => {
          if (typeof newValue === 'string') {
            // timeout to avoid instant validation of the dialog's form.
            setTimeout(() => {
              toggleOpen(true);
              setDialogValue({
                name: newValue,
                estimate: 0
              });
            });
          } else if (newValue && newValue.inputValue) {
            toggleOpen(true);
            setDialogValue({
              name: newValue.inputValue,
              estimate: 0
            });
          } else {
            setValue(newValue);
          }
        }}
        filterOptions={(options, params) => {
          const filtered = filter(options, params);

          if (filtered.length === 0 && params.inputValue !== '') {
            filtered.push({
              inputValue: params.inputValue,
              name: `Add "${params.inputValue}"`
            });
          }

          return filtered;
        }}
        id="categories-autocomplete"
        options={cateogories}
        getOptionLabel={(option) => {
          // e.g value selected with enter, right from the input
          if (typeof option === 'string') {
            return option;
          }
          if (option.inputValue) {
            return option.inputValue;
          }
          return option.name;
        }}
        selectOnFocus
        clearOnBlur
        handleHomeEndKeys
        renderOption={(props, option) => <li {...props}>{option.name}</li>}
        // sx={{ width: 300 }}
        freeSolo
        renderInput={(params) => <TextField {...params} error={(errorText?.length ?? 0) > 0} helperText={errorText} label={label} />}
      />
      <Dialog open={open} onClose={handleClose}>
        <form onSubmit={handleSubmit}>
          <DialogTitle>Add a new category</DialogTitle>
          <DialogContent>
            <DialogContentText>
              Need a new category? Please, add it!
            </DialogContentText>
            <TextField
              autoFocus
              id="name"
              value={dialogValue.name}
              onChange={(event) =>
                setDialogValue({
                  name: event.target.value,
                  estimate: dialogValue.estimate
                })
              }
              label="name"
              type="text"
              variant="standard"
            />
            <TextField
              id="estimate"
              value={dialogValue.estimate}
              onChange={(event) =>
                setDialogValue({
                  name: dialogValue.name,
                  estimate: Number(event.target.value)
                })
              }
              label="estimate"
              type="number"
              variant="standard"
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button type="submit">Add</Button>
          </DialogActions>
        </form>
      </Dialog>
    </React.Fragment>
  );
}

export default CategoryAutoComplete;

interface CategoryOptionType {
  inputValue?: string;
  name: string;
}
