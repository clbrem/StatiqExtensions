module.exports = {
  purge: {
    enabled: true,
    content: [
        '../**/input/**/*.html',
        '../*.fs',
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
