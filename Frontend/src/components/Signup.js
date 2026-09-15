import React, { useState } from 'react'
import { Link } from 'react-router-dom'

function Signup() {
  const [form, setForm] = useState({ fullName: '', email: '', password: '', confirmPassword: '' })
  const [loading, setLoading] = useState(false)
  const [success, setSuccess] = useState('')
  const [error, setError] = useState('')
  const [errors, setErrors] = useState({})

  const validate = () => {
    const errs = {}
    if (!form.fullName.trim()) errs.fullName = 'Full name is required'
    if (!form.email.trim()) errs.email = 'Email is required'
    if (form.password.length < 6) errs.password = 'Password must be at least 6 characters'
    if (form.password !== form.confirmPassword) errs.confirmPassword = 'Passwords do not match'
    setErrors(errs)
    return Object.keys(errs).length === 0
  }

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setSuccess('')
    if (!validate()) return

    setLoading(true)
    try {
      const res = await fetch('/api/auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          email: form.email,
          password: form.password,
          fullName: form.fullName
        })
      })
      const data = await res.json()

      if (!data.success) {
        setError(data.message || 'Registration failed')
        setLoading(false)
        return
      }

      setSuccess(data.message)
      setForm({ fullName: '', email: '', password: '', confirmPassword: '' })
    } catch {
      setError('Unable to connect to server. Please try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="container my-5">
      <div className="row justify-content-center">
        <div className="col-md-5">
          <div className="card shadow-sm border-0">
            <div className="card-body p-4">
              <h3 className="fw-bold text-center mb-1">Sign Up</h3>
              <p className="text-muted text-center small mb-4">Create your FreshMart account</p>

              {success && <div className="alert alert-success py-2 small">{success}</div>}
              {error && <div className="alert alert-danger py-2 small">{error}</div>}

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label className="form-label">Full Name</label>
                  <input type="text" className={`form-control ${errors.fullName ? 'is-invalid' : ''}`}
                    name="fullName" value={form.fullName} onChange={handleChange}
                    placeholder="John Doe" />
                  {errors.fullName && <div className="invalid-feedback">{errors.fullName}</div>}
                </div>
                <div className="mb-3">
                  <label className="form-label">Email</label>
                  <input type="email" className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                    name="email" value={form.email} onChange={handleChange}
                    placeholder="john@example.com" />
                  {errors.email && <div className="invalid-feedback">{errors.email}</div>}
                </div>
                <div className="mb-3">
                  <label className="form-label">Password</label>
                  <input type="password" className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                    name="password" value={form.password} onChange={handleChange}
                    placeholder="At least 6 characters" />
                  {errors.password && <div className="invalid-feedback">{errors.password}</div>}
                </div>
                <div className="mb-3">
                  <label className="form-label">Confirm Password</label>
                  <input type="password" className={`form-control ${errors.confirmPassword ? 'is-invalid' : ''}`}
                    name="confirmPassword" value={form.confirmPassword} onChange={handleChange}
                    placeholder="Re-enter password" />
                  {errors.confirmPassword && <div className="invalid-feedback">{errors.confirmPassword}</div>}
                </div>
                <button type="submit" className="btn btn-success w-100 py-2 fw-semibold"
                  disabled={loading}>
                  {loading ? 'Creating Account...' : 'Create Account'}
                </button>
              </form>

              <p className="text-center small text-muted mt-3 mb-0">
                Already have an account?{" "}
                <Link to="/login" className="text-decoration-none fw-semibold">Login</Link>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Signup