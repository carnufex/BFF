const PROXY_CONFIG = [
  {
    context: [
      '/weatherforecast',
      '/bff',
      '/signin-oidc',
      '/signout-callback-oidc',
    ],
    target: 'https://localhost:7282',
    secure: false,
    headers: {
      Connection: 'Keep-Alive',
    },
  },
];

module.exports = PROXY_CONFIG;
