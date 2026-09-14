import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { CartProvider, useCart } from '../context/CartContext'
import Cart from '../components/Cart'

// Helper that exposes cart actions for assertions
let cartApi = {}
function Probe() {
  const cart = useCart()
  cartApi = cart
  return (
    <div>
      <span data-testid="count">{cart.cartCount}</span>
      <span data-testid="total">{cart.cartTotal.toFixed(2)}</span>
      <span data-testid="count-items">{cart.items.length}</span>
    </div>
  )
}

const renderCart = (initialItems) => {
  if (initialItems) {
    localStorage.setItem('freshmart_cart', JSON.stringify(initialItems))
  }
  return render(
    <MemoryRouter>
      <CartProvider>
        <Probe />
        <Cart />
      </CartProvider>
    </MemoryRouter>
  )
}

const apple = { productId: 1, name: 'Apple', price: 5.00, imageUrl: null }
const banana = { productId: 2, name: 'Banana', price: 2.00, imageUrl: null }

beforeEach(() => {
  jest.clearAllMocks()
  localStorage.clear()
  cartApi = {}
})

describe('F11 - A product can be added to the cart', () => {
  it('adds a product and updates the cart count', async () => {
    renderCart()
    cartApi.addToCart(apple)
    await waitFor(() => expect(screen.getByTestId('count').textContent).toBe('1'))
    expect(screen.getByText('Apple')).toBeInTheDocument()
  })
})

describe('F12 - Adding the same product increases its quantity', () => {
  it('increments quantity instead of duplicating the item', async () => {
    renderCart()
    cartApi.addToCart(apple)
    cartApi.addToCart(apple)
    await waitFor(() => expect(cartApi.items.length).toBe(1))
    await waitFor(() => expect(cartApi.items[0].quantity).toBe(2))
    await waitFor(() => expect(cartApi.cartCount).toBe(2))
  })
})

describe('F13 - Cart quantity can be increased and decreased', () => {
  it('increments and decrements the quantity using the buttons', async () => {
    renderCart([{ ...apple, quantity: 2 }])
    await screen.findByText('Apple')

    const plusButtons = screen.getAllByRole('button')
    fireEvent.click(plusButtons.find(b => b.textContent === '+'))
    await waitFor(() => expect(cartApi.items[0].quantity).toBe(3))

    fireEvent.click(screen.getAllByRole('button').find(b => b.textContent === '−'))
    await waitFor(() => expect(cartApi.items[0].quantity).toBe(2))
  })
})

describe('F14 - Reducing quantity to zero removes the item', () => {
  it('removes the item when quantity reaches zero', async () => {
    renderCart()
    cartApi.addToCart(apple)
    cartApi.updateQuantity(apple.productId, 0)
    expect(cartApi.items.length).toBe(0)
    expect(await screen.findByText('Your Cart is Empty')).toBeInTheDocument()
  })
})

describe('F15 - The cart total is calculated correctly', () => {
  it('computes the sum of price * quantity', async () => {
    renderCart()
    cartApi.addToCart(apple, 2) // 10
    cartApi.addToCart(banana, 1) // 2
    await waitFor(() => expect(cartApi.cartTotal).toBe(12))
    expect(screen.getAllByText('$12.00').length).toBeGreaterThan(0)
  })
})

describe('F16 - Cart data remains available after refreshing the browser', () => {
  it('hydrates the cart from localStorage on mount', async () => {
    localStorage.setItem('freshmart_cart', JSON.stringify([{ ...apple, quantity: 3 }]))
    renderCart()
    await waitFor(() => expect(cartApi.cartCount).toBe(3))
    expect(cartApi.items[0].quantity).toBe(3)
  })
})