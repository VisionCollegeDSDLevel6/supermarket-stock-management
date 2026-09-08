import React, { useState } from 'react'
import { Link } from 'react-router-dom'

function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleLogin = async (e) => {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      const res = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      })

      const data = await res.json()

      if (!data.success) {
        setError(data.message || 'Login failed')
        setLoading(false)
        return
      }

      setUser(data)
    } catch {
      setError('Unable to connect to server. Make sure the backend is running.')
    } finally {
      setLoading(false)
    }
  }

  if (user) {
    return (
      <div className="container my-5 text-center">
        <div className="card shadow-sm border-0 mx-auto" style={{ maxWidth: '450px' }}>
          <div className="card-body py-5">
            <div className="text-success mb-3">
              <svg width="48" height="48" fill="currentColor" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
              </svg>
            </div>
            <h4 className="fw-bold text-success mb-2">Welcome Back!</h4>
            <p className="text-muted mb-1">Logged in as <strong>{user.email}</strong></p>
            {user.roles && user.roles.length > 0 && (
              <div className="mb-3">
                {user.roles.map(r => (
                  <span key={r} className="badge bg-success bg-opacity-10 text-success me-1">{r}</span>
                ))}
              </div>
            )}
            <div className="d-flex justify-content-center gap-2">
              <button className="btn btn-outline-danger btn-sm" onClick={() => setUser(null)}>
                Logout
              </button>
              <Link to="/products" className="btn btn-success btn-sm">Browse Products</Link>
            </div>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div className="container my-5">
      <div className="row justify-content-center">
        <div className="col-md-5">
          <div className="card shadow-sm border-0">
            <div className="card-body p-4">
              <h3 className="fw-bold text-center mb-1">Login</h3>
              <p className="text-muted text-center small mb-4">Sign in to your FreshMart account</p>

              {error && (
                <div className="alert alert-danger py-2 small">{error}</div>
              )}

              <form onSubmit={handleLogin}>
                <div className="mb-3">
                  <label className="form-label">Email</label>
                  <input type="email" className="form-control"
                    value={email} onChange={e => setEmail(e.target.value)}
                    placeholder="admin@stockflow.co.nz" required />
                </div>
                <div className="mb-3">
                  <label className="form-label">Password</label>
                  <input type="password" className="form-control"
                    value={password} onChange={e => setPassword(e.target.value)}
                    placeholder="Enter password" required />
                </div>
                <button type="submit" className="btn btn-success w-100 py-2 fw-semibold"
                  disabled={loading}>
                  {loading ? 'Signing in...' : 'Sign In'}
                </button>
              </form>

              <hr />
              <p className="text-center small mb-0">
                <Link to="/products" className="text-decoration-none">Continue as Guest</Link>
              </p>
            </div>
          </div>

          <div className="card shadow-sm border-0 mt-3">
            <div className="card-body p-3">
              <h6 className="fw-bold mb-2">Demo Accounts</h6>
              <div className="small">
                <p className="mb-1"><strong>Admin:</strong> admin@stockflow.co.nz / Admin123!</p>
                <p className="mb-1"><strong>Admin:</strong> ngthanh123426@gmail.com / Admin123!</p>
                <p className="mb-1"><strong>Manager:</strong> manager@stockflow.com / Admin123!</p>
                <p className="mb-0"><strong>Staff:</strong> staff@stockflow.com / Admin123!</p>
              </div>
            </div>
          </div>

          <div className="card shadow-sm border-0 mt-3">
            <div className="card-body p-3">
              <h6 className="fw-bold mb-2">Admin Panel</h6>
              <p className="small text-muted mb-1">For staff dashboard and management:</p>
              <a href="https://localhost:5001" className="btn btn-outline-success btn-sm w-100"
                target="_blank" rel="noreferrer">
                Go to Admin Panel
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Login