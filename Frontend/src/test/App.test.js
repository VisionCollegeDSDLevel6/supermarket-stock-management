import React from 'react'
import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import App from '../App'

const renderApp = (path = '/') =>
  render(
    <MemoryRouter initialEntries={[path]}>
      <App />
    </MemoryRouter>
  )

describe('F01 - Home page loads and main navigation links work', () => {
  it('renders the Home page hero and feature cards', () => {
    renderApp()
    expect(screen.getAllByText('FreshMart').length).toBeGreaterThan(0)
    expect(screen.getByText('Product Catalog')).toBeInTheDocument()
    expect(screen.getByText('Shop by Category')).toBeInTheDocument()
    expect(screen.getByText('Real-time Stock')).toBeInTheDocument()
  })

  it('renders navigation links to Home, Products, About, Categories and Contact', () => {
    renderApp()
    expect(screen.getByRole('link', { name: 'Products' })).toHaveAttribute('href', '/products')
    expect(screen.getByRole('link', { name: 'About' })).toHaveAttribute('href', '/about')
    expect(screen.getByRole('link', { name: 'Categories' })).toHaveAttribute('href', '/categories')
    expect(screen.getByRole('link', { name: 'Contact' })).toHaveAttribute('href', '/contact')
    expect(screen.getByRole('link', { name: 'Login' })).toHaveAttribute('href', '/login')
    expect(screen.getByRole('link', { name: 'Sign Up' })).toHaveAttribute('href', '/signup')
  })
})