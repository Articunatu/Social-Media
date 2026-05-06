import type { Metadata } from "next";
import "./globals.css";
import { AuthProvider } from "./components/auth-provider";
import React from 'react';
import ReactQueryProvider from './components/ui/react-query-provider';

export const metadata: Metadata = {
  title: "Social Media",
  description: "Card gamified social media platform",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <ReactQueryProvider>
      <AuthProvider>
        <html lang="en" data-theme="mytheme">
          <head>
            <link
              href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined"
              rel="stylesheet"
            />
          </head>
          <body className="antialiased">{children}</body>
        </html>
      </AuthProvider>
    </ReactQueryProvider>
  );
}
