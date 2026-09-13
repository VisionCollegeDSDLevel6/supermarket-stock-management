import React from 'react'
import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'
import { CartProvider } from '../context/CartContext'
import ProductList from '../components/ProductList'

vi.mock('../api/productsApi', () => ({
  getProducts: vi.fn(),
}))

import { getProducts } from '../api/productsApi'

const mockCategories = [{ categoryId: 1, name: 'Test1', description: '123' }]

const mockProducts = [
  { productId: 1, name: 'Apple', price: 5.00, description: 'Fresh', category: { name: 'Test1' }, stock: { quantity: 10, lowStockThreshold: 5 }, imageUrl: null },
  { productId: 2, name: 'Banana', price: 2.00, description: 'Sweet', category: { name: 'Test1' }, stock: { quantity: 1, lowStockThreshold: 5 }, imageUrl: null },
]

const renderList = () =>
  render(
    <MemoryRouter>
      <CartProvider>
        <ProductList />
      </CartProvider>
    </MemoryRouter>
  )

beforeEach(() => {
  vi.clearAllMocks()
  getProducts.mockResolvedValue(mockProducts)
  global.fetch = vi.fn(() =>
    Promise.resolve({ json: () => Promise.resolve(mockCategories) })
  )
})

describe('F02 - Product list loads data from the backend API', () => {
  it('displays products returned by the API', async () => {
    renderList()
    expect(await screen.findByText('Apple')).toBeInTheDocument()
    expect(await screen.findByText('Banana')).toBeInTheDocument()
  })
})

describe('F03 - A loading state is shown while data is being retrieved', () => {
  it('shows a spinner before products load', async () => {
    let resolvePromise
    getProducts.mockImplementation(() => new Promise(resolve => { resolvePromise = resolve }))
    renderList()
    expect(screen.getByText('Loading products...')).toBeInTheDocument()
    resolvePromise(mockProducts)
    await waitFor(() => expect(screen.getByText('Apple')).toBeInTheDocument())
  })
})

describe('F04 - An appropriate message is displayed when the API request fails', () => {
  it('shows "No products found" when the API call fails', async () => {
    getProducts.mockRejectedValue(new Error('network error'))
    renderList()
    expect(await screen.findByText('No products found')).toBeInTheDocument()
  })
})

describe('F05 - Product search returns matching products', () => {
  it('calls the API with the search term when the search button is clicked', async () => {
    const user = userEvent.setup()
    renderList()
    await screen.findByText('Apple')

    await user.type(screen.getByPlaceholderText('Search products...'), 'Apple')
    await user.click(screen.getByRole('button', { name: 'Search' }))

    await waitFor(() => {
      expect(getProducts).toHaveBeenCalledWith(expect.objectContaining({ searchTerm: 'Apple' }))
    })
  })
})

describe('F06 - Category filtering displays products from the selected category', () => {
  it('calls the API with the selected category id', async () => {
    const user = userEvent.setup()
    renderList()
    await screen.findByText('Apple')

    await user.selectOptions(screen.getAllByRole('combobox')[0], '1')

    await waitFor(() => {
      expect(getProducts).toHaveBeenCalledWith(expect.objectContaining({ categoryId: '1' }))
    })
  })
})

describe('F07 - Products can be sorted alphabetically', () => {
  it('calls the API with sortBy name (ascending default)', async () => {
    const user = userEvent.setup()
    renderList()
    await screen.findByText('Apple')

    await user.selectOptions(screen.getAllByRole('combobox')[1], 'name')

    await waitFor(() => {
      expect(getProducts).toHaveBeenCalledWith(expect.objectContaining({ sortBy: 'name', sortOrder: 'asc' }))
    })
  })
})

describe('F08 - Products can be sorted by price from high to low', () => {
  it('calls the API with sortBy price and sortOrder desc', async () => {
    const user = userEvent.setup()
    renderList()
    await screen.findByText('Apple')

    await user.selectOptions(screen.getAllByRole('combobox')[1], 'price-desc')

    await waitFor(() => {
      expect(getProducts).toHaveBeenCalledWith(expect.objectContaining({ sortBy: 'price', sortOrder: 'desc' }))
    })
  })
})