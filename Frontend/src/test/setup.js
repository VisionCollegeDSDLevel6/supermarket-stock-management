import '@testing-library/jest-dom'
import { TextEncoder, TextDecoder } from 'util'

// react-router-dom v7 requires TextEncoder/TextDecoder at import time.
global.TextEncoder = TextEncoder
global.TextDecoder = TextDecoder

// @testing-library/react auto-cleans the DOM after each test because Jest
// exposes the global `afterEach` hook. localStorage is reset per test below.
beforeEach(() => {
  localStorage.clear()
})