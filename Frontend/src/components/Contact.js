import React, { useState } from 'react'
import { Link } from 'react-router-dom'

function Contact() {
  const [form, setForm] = useState({
    name: '', email: '', phone: '', subject: '', message: ''
  })
  const [loading, setLoading] = useState(false)
  const [success, setSuccess] = useState('')
  const [error, setError] = useState('')

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setSuccess('')
    setLoading(true)

    try {
      const res = await fetch('/api/contact', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(form)
      })
      const data = await res.json()

      if (!data.success) {
        setError(data.message || 'Failed to send message')
        return
      }

      setSuccess(data.message)
      setForm({ name: '', email: '', phone: '', subject: '', message: '' })
    } catch {
      setError('Unable to connect to server. Please try again later.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="container my-4">
      <nav aria-label="breadcrumb">
        <ol className="breadcrumb">
          <li className="breadcrumb-item"><Link to="/">Home</Link></li>
          <li className="breadcrumb-item active">Contact</li>
        </ol>
      </nav>

      <div className="row justify-content-center">
        <div className="col-lg-8">
          <h1 className="fw-bold mb-4">Contact Us</h1>

          <div className="row g-4">
            <div className="col-md-5">
              <div className="card shadow-sm border-0">
                <div className="card-body p-4">
                  <h5 className="fw-semibold mb-2">Quick Links</h5>
                  <div className="d-flex flex-column gap-1">
                    <Link to="/about" className="small text-decoration-none">About Us</Link>
                    <Link to="/products" className="small text-decoration-none">Browse Products</Link>
                    <Link to="/categories" className="small text-decoration-none">Categories</Link>
                  </div>
                </div>
              </div>
            </div>

            <div className="col-md-7">
              <div className="card shadow-sm border-0">
                <div className="card-body p-4">
                  <h5 className="fw-semibold mb-3">Send a Message</h5>

                  {success && (
                    <div className="alert alert-success py-2 small">{success}</div>
                  )}
                  {error && (
                    <div className="alert alert-danger py-2 small">{error}</div>
                  )}

                  <form onSubmit={handleSubmit}>
                    <div className="row g-3">
                      <div className="col-md-6">
                        <label className="form-label">Name *</label>
                        <input type="text" className="form-control" name="name"
                          value={form.name} onChange={handleChange} required />
                      </div>
                      <div className="col-md-6">
                        <label className="form-label">Email *</label>
                        <input type="email" className="form-control" name="email"
                          value={form.email} onChange={handleChange} required />
                      </div>
                      <div className="col-md-6">
                        <label className="form-label">Phone</label>
                        <input type="tel" className="form-control" name="phone"
                          value={form.phone} onChange={handleChange} />
                      </div>
                      <div className="col-md-6">
                        <label className="form-label">Subject *</label>
                        <input type="text" className="form-control" name="subject"
                          value={form.subject} onChange={handleChange} required />
                      </div>
                      <div className="col-12">
                        <label className="form-label">Message *</label>
                        <textarea className="form-control" name="message" rows="4"
                          value={form.message} onChange={handleChange} required />
                      </div>
                      <div className="col-12">
                        <button type="submit" className="btn btn-success w-100 py-2 fw-semibold"
                          disabled={loading}>
                          {loading ? 'Sending...' : 'Send Message'}
                        </button>
                      </div>
                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Contact