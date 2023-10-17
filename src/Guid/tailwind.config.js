/** @type {import('tailwindcss').Config} */
module.exports = {
    darkMode: 'class',
  content: ["./wwwroot/**/*.{html,js}", "./Pages/*.cshtml", "./Pages/**/*.cshtml"],
  theme: {
    extend: {},
  },
  plugins: [
  require('@tailwindcss/typography'),
  ],
}