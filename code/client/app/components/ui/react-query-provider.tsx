"use client";
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import React, { ReactNode, useState } from 'react';

export default function ReactQueryProvider({ children }: { children: ReactNode }) {
  // Create the QueryClient on the client only
  const [queryClient] = useState(() => new QueryClient());
  return <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>;
}
