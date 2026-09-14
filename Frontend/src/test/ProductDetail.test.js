import React from 'react'
import { render, screen } from '@testing-library/react'
import { MemoryRouter, Routes, Route } from 'react-router-dom'
import { CartProvider } from '../context/CartContext'
import ProductDetail from '../components/ProductDetail'

jest.mock('../api/productsApi', () => ({
  getProduct: jest.fn(),
}))

import { getProduct } from '../api/productsApi'

const renderDetail = (id) =>
  render(
    <MemoryRouter initialEntries={[`/products/${id}`]}>
      <CartProvider>
        <Routes>
          <Route path="/products/:id" element={<ProductDetail />} />
        </Routes>
      </CartProvider>
    </MemoryRouter>
  )

beforeEach(() => {
  jest.clearAllMocks()
})

describe('F09 - Product details show correct name, price, category, image and stock status', () => {
  it('displays the product name, price, category and stock badge', async () => {
    getProduct.mockResolvedValue({
      productId: 1,
      name: 'Apple',
      price: 5.5,
      description: 'Fresh red apples',
      category: { name: 'Fruit' },
      stock: { quantity: 10, lowStockThreshold: 5 },
      imageUrl: 'https://example.com/apple.png',
    })
    renderDetail(1)

    expect(await screen.findByRole('heading', { name: 'Apple' })).toBeInTheDocument()
    expect(screen.getByText('$5.50')).toBeInTheDocument()
    // Category badge
    expect(screen.getByText('Fruit')).toBeInTheDocument()
    // Image shown
    const img = screen.getByRole('img')
    expect(img).toHaveAttribute('src', 'https://example.com/apple.png')
    // Stock status with quantity
    expect(screen.getByText('In Stock (10 available)')).toBeInTheDocument()
  })

  it('shows "Product not found" when the product does not exist', async () => {
    getProduct.mockRejectedValue(new Error('404'))
    renderDetail(999)
    expect(await screen.findByText('Product not found')).toBeInTheDocument()
  })
})

describe('F10 - An out-of-stock product cannot be added to the cart', () => {
  it('does not show an Add to Cart button when there is no stock', async () => {
    getProduct.mockResolvedValue({
      productId: 2,
      name: 'Gone',
      price: 1.0,
      description: 'Sold out',
      category: { name: 'Test1' },
      stock: { quantity: 0, lowStockThreshold: 5 },
      imageUrl: null,
    })
    renderDetail(2)

    await screen.findByRole('heading', { name: 'Gone' })
    expect(screen.getByText('Out of Stock')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /Add to Cart/ })).not.toBeInTheDocument()
  })
})