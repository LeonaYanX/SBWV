const path = require('path');
const { WebpackManifestPlugin } = require('webpack-manifest-plugin');

module.exports = (env, argv) => {
    const mode = argv.mode || 'development';

    return {
        mode: mode,
        entry: './wwwroot/js/site.js',
        output: {
            filename: '[name].[contenthash].js',
            path: path.resolve(__dirname, 'wwwroot/dist'),
            publicPath: '/dist/',
        },
        plugins: [
            new WebpackManifestPlugin({
                fileName: 'manifest.json',
                publicPath: '/dist/',
            }),
        ],
    };
};