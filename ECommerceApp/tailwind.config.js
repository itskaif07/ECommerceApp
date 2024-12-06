/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./ECommerceApp/Views/**/*.cshtml",  // Razor views
        "./ECommerceApp/wwwroot/**/*.html",  // HTML files (if any)
        "./ECommerceApp/wwwroot/js/**/*.js",  // JS files (if Tailwind classes are used in JS)
    ],
  theme: {
    extend: {},
  },
  plugins: [],
}

