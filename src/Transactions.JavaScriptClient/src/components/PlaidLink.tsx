import React, { useCallback, useState, FunctionComponent } from "react";
import {
  usePlaidLink,
  PlaidLinkOptions,
  PlaidLinkOnSuccess,
} from "react-plaid-link";
import {
  Button
} from '@mui/material';
import { transactionService } from './../services/transaction.service'

interface Props {
  token: string;
}

const PlaidLink: FunctionComponent<Props> = ({ token }) => {
  const onSuccess = useCallback<PlaidLinkOnSuccess>(
    async (public_token, metadata) => {
      await transactionService.setAccessToken(public_token)
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
    <Button fullWidth variant="contained" size="large" onClick={() => open()} disabled={!ready}>
      Connect a bank account
    </Button>
  );
};

export default PlaidLink;