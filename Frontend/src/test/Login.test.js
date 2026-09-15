import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'
import Login from '../components/Login'

const renderLogin = () =>
  render(
    <MemoryRouter>
      <Login />
    </MemoryRouter>
  )

beforeEach(() => {
  jest.clearAllMocks()
  global.fetch = jest.fn()
})

describe('F18 - Login displays the correct result for valid and invalid credentials', () => {
  it('shows the welcome message and role for valid credentials', async () => {
    global.fetch.mockResolvedValue({
      json: () => Promise.resolve({
        success: true,
        message: 'Login successful.',
        email: 'admin@stockflow.co.nz',
        userId: '123',
        roles: ['Admin'],
      }),
    })
    const user = userEvent.setup()
    renderLogin()

    await user.type(screen.getByLabelText(/Email/), 'admin@stockflow.co.nz')
    await user.type(screen.getByLabelText(/Password/), 'Admin123!')
    await user.click(screen.getByRole('button', { name: 'Sign In' }))

    expect(await screen.findByText(/Welcome Back/)).toBeInTheDocument()
    expect(screen.getByText('admin@stockflow.co.nz')).toBeInTheDocument()
    expect(screen.getByText('Admin')).toBeInTheDocument()
  })

  it('shows an error message for invalid credentials', async () => {
    global.fetch.mockResolvedValue({
      json: () => Promise.resolve({ success: false, message: 'Invalid email or password.' }),
    })
    const user = userEvent.setup()
    renderLogin()

    await user.type(screen.getByLabelText(/Email/), 'wrong@test.com')
    await user.type(screen.getByLabelText(/Password/), 'wrongpass')
    await user.click(screen.getByRole('button', { name: 'Sign In' }))

    expect(await screen.findByText('Invalid email or password.')).toBeInTheDocument()
  })
})