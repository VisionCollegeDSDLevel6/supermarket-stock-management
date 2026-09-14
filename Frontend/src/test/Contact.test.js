import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'
import Contact from '../components/Contact'

const renderContact = () =>
  render(
    <MemoryRouter>
      <Contact />
    </MemoryRouter>
  )

beforeEach(() => {
  jest.clearAllMocks()
  global.fetch = jest.fn()
})

const fillForm = async (user, overrides = {}) => {
  await user.type(screen.getByLabelText(/Name/), overrides.name || 'John Doe')
  await user.type(screen.getByLabelText(/Email/), overrides.email || 'john@example.com')
  await user.type(screen.getByLabelText(/Subject/), overrides.subject || 'Question')
  await user.type(screen.getByLabelText(/Message/), overrides.message || 'Hello there')
}

describe('F17 - The contact form handles successful and unsuccessful submissions', () => {
  it('shows a success message when the contact request succeeds', async () => {
    global.fetch.mockResolvedValue({
      json: () => Promise.resolve({ success: true, message: "Your message has been sent. We'll get back to you soon!" }),
    })
    const user = userEvent.setup()
    renderContact()
    await fillForm(user)
    await user.click(screen.getByRole('button', { name: /Send Message/ }))

    expect(await screen.findByText(/message has been sent/i)).toBeInTheDocument()
  })

  it('shows an error message and does not clear the form when the request fails', async () => {
    global.fetch.mockResolvedValue({
      json: () => Promise.resolve({ success: false, message: 'All required fields must be filled.' }),
    })
    const user = userEvent.setup()
    renderContact()
    await fillForm(user)
    await user.click(screen.getByRole('button', { name: /Send Message/ }))

    expect(await screen.findByText('All required fields must be filled.')).toBeInTheDocument()
  })
})