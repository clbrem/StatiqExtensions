module.exports = {
  purge: {
    enabled: false,
    content: [
        '../**/input/**/*.html',
        '../*.fs',
        '../**/input/*.yaml'
    ]
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {},
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
