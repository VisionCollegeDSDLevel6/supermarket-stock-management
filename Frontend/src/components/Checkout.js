import React, { useState } from 'react'
import { Link, Navigate } from 'react-router-dom'
import { useCart } from '../context/CartContext'

function Checkout() {
  const { items, cartTotal, clearCart } = useCart()
  const [form, setForm] = useState({ name: '', email: '', address: '', phone: '' })
  const [submitted, setSubmitted] = useState(false)
  const [errors, setErrors] = useState({})

  if (items.length === 0 && !submitted) {
    return <Navigate to="/cart" />
  }

  if (submitted) {
    return (
      <div className="container my-5 text-center">
        <div className="card shadow-sm border-0 mx-auto" style={{ maxWidth: '500px' }}>
          <div className="card-body py-5">
            <div className="text-success mb-3">
              <svg width="64" height="64" fill="currentColor" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
              </svg>
            </div>
            <h2 className="fw-bold text-success mb-2">Order Placed!</h2>
            <p className="text-muted mb-4">Thank you, <strong>{form.name}</strong>! Your order has been placed successfully.</p>
            <p className="small text-muted mb-4">A confirmation email will be sent to <strong>{form.email}</strong></p>
            <Link to="/products" className="btn btn-success">Continue Shopping</Link>
          </div>
        </div>
      </div>
    )
  }

  const validate = () => {
    const errs = {}
    if (!form.name.trim()) errs.name = 'Name is required'
    if (!form.email.trim()) errs.email = 'Email is required'
    if (!form.address.trim()) errs.address = 'Address is required'
    if (!form.phone.trim()) errs.phone = 'Phone is required'
    setErrors(errs)
    return Object.keys(errs).length === 0
  }

  const handleSubmit = (e) => {
    e.preventDefault()
    if (validate()) {
      clearCart()
      setSubmitted(true)
    }
  }

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  return (
    <div className="container my-4">
      <nav aria-label="breadcrumb">
        <ol className="breadcrumb">
          <li className="breadcrumb-item"><Link to="/">Home</Link></li>
          <li className="breadcrumb-item"><Link to="/cart">Cart</Link></li>
          <li className="breadcrumb-item active">Checkout</li>
        </ol>
      </nav>

      <h1 className="fw-bold mb-4">Checkout</h1>

      <div className="row g-4">
        <div className="col-lg-7">
          <div className="card shadow-sm border-0">
            <div className="card-body p-4">
              <h5 className="fw-bold mb-3">Shipping Information</h5>
              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label className="form-label">Full Name</label>
                  <input type="text" className={`form-control ${errors.name ? 'is-invalid' : ''}`}
                    name="name" value={form.name} onChange={handleChange} placeholder="John Doe" />
                  {errors.name && <div className="invalid-feedback">{errors.name}</div>}
                </div>
                <div className="mb-3">
                  <label className="form-label">Email</label>
                  <input type="email" className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                    name="email" value={form.email} onChange={handleChange} placeholder="john@example.com" />
                  {errors.email && <div className="invalid-feedback">{errors.email}</div>}
                </div>
                <div className="mb-3">
                  <label className="form-label">Phone</label>
                  <input type="tel" className={`form-control ${errors.phone ? 'is-invalid' : ''}`}
                    name="phone" value={form.phone} onChange={handleChange} placeholder="021 123 4567" />
                  {errors.phone && <div className="invalid-feedback">{errors.phone}</div>}
                </div>
                <div className="mb-3">
                  <label className="form-label">Delivery Address</label>
                  <textarea className={`form-control ${errors.address ? 'is-invalid' : ''}`}
                    name="address" rows="2" value={form.address} onChange={handleChange}
                    placeholder="123 Main Street, Auckland" />
                  {errors.address && <div className="invalid-feedback">{errors.address}</div>}
                </div>
                <button type="submit" className="btn btn-success w-100 py-2 fw-semibold mt-2">
                  Place Order — ${cartTotal.toFixed(2)}
                </button>
              </form>
            </div>
          </div>
        </div>

        <div className="col-lg-5">
          <div className="card shadow-sm border-0">
            <div className="card-body p-4">
              <h5 className="fw-bold mb-3">Order Summary</h5>
              <hr />
              {items.map(item => (
                <div key={item.productId} className="d-flex justify-content-between align-items-center mb-2">
                  <div className="small">
                    <span className="fw-semibold">{item.name}</span>
                    <span className="text-muted"> × {item.quantity}</span>
                  </div>
                  <span className="small fw-bold">${(item.price * item.quantity).toFixed(2)}</span>
                </div>
              ))}
              <hr />
              <div className="d-flex justify-content-between mb-1">
                <span className="text-muted">Subtotal</span>
                <span>${cartTotal.toFixed(2)}</span>
              </div>
              <div className="d-flex justify-content-between mb-1">
                <span className="text-muted">Shipping</span>
                <span className="text-success">Free</span>
              </div>
              <hr />
              <div className="d-flex justify-content-between">
                <span className="fw-bold">Total</span>
                <span className="fw-bold fs-5 text-success">${cartTotal.toFixed(2)}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Checkout