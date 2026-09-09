import { defineConfig, transformWithEsbuild } from 'vite'
import react from '@vitejs/plugin-react-swc'

const jsxInJsPlugin = {
  name: 'jsx-in-js',
  enforce: 'pre',
  async transform(code, id) {
    if (!id.includes('/src/') || !id.endsWith('.js')) {
      return null
    }

    return transformWithEsbuild(code, id, {
      loader: 'jsx',
      jsx: 'automatic',
    })
  },
}

export default defineConfig({
  plugins: [jsxInJsPlugin, react()],
  optimizeDeps: {
    esbuildOptions: {
      loader: {
        '.js': 'jsx',
      },
    },
  },
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        secure: false,
      },
    },
  },
})