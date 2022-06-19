import * as React from 'react';
import Checkbox from '@mui/material/Checkbox';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import Chip from '@mui/material/Chip';

const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checkedIcon = <CheckBoxIcon fontSize="small" />;

interface Props {
    options: Array<any>;
    value: any | undefined;
    setValue: (value: any) => void;
    label: string;
}

const CategoriesAutocompleteMultiple : React.FunctionComponent<Props> = ({ options, value, setValue, label }) => {
    
    return (
        <React.Fragment>
            <Autocomplete
            value={value}
            multiple
            size="small"
            options={options}
            disableCloseOnSelect
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
            onChange={(event, newValue, _, details) => {
                setValue(newValue);
            }}
            renderOption={(props, option, { selected }) => (
                <li {...props}>
                {option.id &&
                    <Checkbox
                    icon={icon}
                    checkedIcon={checkedIcon}
                    style={{ marginRight: 8 }}
                    checked={selected}
                />}
                {option.name}
                </li>
            )}
            style={{ width: 500 }}
            renderInput={(params) => (
                <TextField {...params} label={label} />
            )}
            renderTags={(tagValue, getTagProps) => {
                return tagValue.map((option, index) => (
                    <Chip {...getTagProps({ index })} label={option.id ? option.name : option.inputValue} size="small" />
                ));
            }}
            limitTags={8}
            />
        </React.Fragment>
    )
}

export default CategoriesAutocompleteMultiple;