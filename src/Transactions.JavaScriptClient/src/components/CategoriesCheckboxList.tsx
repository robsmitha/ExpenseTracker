import * as React from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Checkbox from '@mui/material/Checkbox';

interface Props {
    categories: Array<any>;
    checked: any | undefined;
    setChecked: (value: any) => void;
    label: string;
}

const CategoriesCheckboxList : React.FunctionComponent<Props> = ({ categories, checked, setChecked, label }) => {
    const [selectAllChecked, setSelectAllChecked] = React.useState<boolean>(true);
    
    const handleToggle = (value: number) => () => {
        const currentIndex = checked.indexOf(value);
        const newChecked = [...checked];

        if (currentIndex === -1) {
            newChecked.push(value);
        } else {
            newChecked.splice(currentIndex, 1);
        }

        setChecked(newChecked);
    };

    const handleSelectAllChecked = () => {
        setChecked(selectAllChecked ? [] : categories);
        setSelectAllChecked(!selectAllChecked);
      };
    
  return (
    <List>
    <ListItemButton role={undefined} onClick={handleSelectAllChecked}>
        <ListItemIcon>
            <Checkbox
                edge="start"
                checked={selectAllChecked}
                tabIndex={-1}
            />
        </ListItemIcon>
        <ListItemText
            sx={{ my: 0 }}
            primary={label}
            secondary="Select All"
        />
    </ListItemButton>
      {categories.map((c) => {
        const labelId = `checkbox-list-label-${c.id}`;

        return (
          <ListItem
            key={c.id}
            disablePadding
          >
            <ListItemButton role={undefined} onClick={handleToggle(c)} dense>
              <ListItemIcon>
                <Checkbox
                  edge="start"
                  checked={checked.indexOf(c) !== -1}
                  tabIndex={-1}
                  disableRipple
                  inputProps={{ 'aria-labelledby': labelId }}
                />
              </ListItemIcon>
              <ListItemText id={labelId} primary={c.name} />
            </ListItemButton>
          </ListItem>
        );
      })}
    </List>
  );
}

export default CategoriesCheckboxList;