import type { Config } from "tailwindcss";

const config: Config = {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['"Segoe UI"', "ui-sans-serif", "system-ui"], 
        mono: ['"Geist Mono"', "ui-monospace"],
      },
      colors: {
        background: "#f0d0a9ff",
        foreground: "#362316",
      },
    },
  },
  plugins: [require("daisyui")],
  daisyui: {
    themes: [
      {
        mytheme: {
          primary: "#4f46e5",
          secondary: "#f59e0b",
          accent: "#10b981",
          neutral: "#3d4451",
          "base-100": "#f0d0a9ff", // hook in your background
          info: "#3abff8",
          success: "#36d399",
          warning: "#fbbd23",
          error: "#f87272",
        },
      },
    ],
  },
};

export default config;
