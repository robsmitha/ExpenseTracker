import * as React from 'react';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';

import { budgetService } from './../services/budget.service'

interface Props {
  open: boolean;
  setOpen: (value: boolean) => void;
  category: string;
  budgetId: number;
  estimate: number;
  setEstimate: (estimate: number) => void;
  onCategorySaved: () => void;
}

const AddBudgetCategoryEstimateDialog : React.FunctionComponent<Props> = ({ open, setOpen, category, budgetId, estimate, setEstimate, onCategorySaved }) => {
  const handleSave = async () => {
    var result = await budgetService.updateBudgetCategoryEstimate({
      estimate,
      CategoryName: category,
      budgetId
    });
    onCategorySaved();
    handleClose();
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleEstimateChange = (e: any) => {
    setEstimate(e.target.value);
  };

  return (
    <div>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>{category} Estimate</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Add an estimate for the "{category}" category.
          </DialogContentText>
          <TextField
            value={estimate}
            autoFocus
            margin="dense"
            id="estimate"
            label="Estimate"
            type="number"
            fullWidth
            variant="standard"
            onChange={handleEstimateChange}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button onClick={handleSave}>Save</Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}
export default AddBudgetCategoryEstimateDialog;