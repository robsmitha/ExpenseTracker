import React, { useCallback, useState, FunctionComponent } from "react";
import {
  usePlaidLink,
  PlaidLinkOptions,
  PlaidLinkOnSuccess,
} from "react-plaid-link";
import {
  Button
} from '@mui/material';
import { accessItemService } from '../services/accessItem.service'

interface Props {
  token: string;
  buttonText: string;
  reloadAccessItems: () => void;
}

const PlaidLink: FunctionComponent<Props> = ({ token, buttonText, reloadAccessItems }) => {
  const onSuccess = useCallback<PlaidLinkOnSuccess>(
    async (public_token, metadata) => {
      await accessItemService.setAccessToken(public_token)
      reloadAccessItems();
    },
    []
  );

  const config: PlaidLinkOptions = {
    token,
    onSuccess,
    // onExit
    // onEvent
  };

  const { open, ready, error } = usePlaidLink(config);

  return (
    <Button onClick={() => open()} disabled={!ready}>
      {buttonText}
    </Button>
  );
};

export default PlaidLink;