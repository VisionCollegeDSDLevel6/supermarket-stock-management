import React, { useState } from 'react'
import { Link } from 'react-router-dom'

function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [loggedIn, setLoggedIn] = useState(false)

  const handleLogin = (e) => {
    e.preventDefault()
    // Demo login — no backend API, just UI demo
    if (email && password) {
      setLoggedIn(true)
    }
  }

  if (loggedIn) {
    return (
      <div className="container my-5 text-center">
        <div className="card shadow-sm border-0 mx-auto" style={{ maxWidth: '400px' }}>
          <div className="card-body py-5">
            <div className="text-success mb-3">
              <svg width="48" height="48" fill="currentColor" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
              </svg>
            </div>
            <h4 className="fw-bold text-success mb-2">Welcome Back!</h4>
            <p className="text-muted mb-4">You are now logged in as <strong>{email}</strong></p>
            <button className="btn btn-outline-danger btn-sm" onClick={() => setLoggedIn(false)}>
              Logout
            </button>
            <Link to="/products" className="btn btn-success ms-2">Browse Products</Link>
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

              <form onSubmit={handleLogin}>
                <div className="mb-3">
                  <label className="form-label">Email</label>
                  <input type="email" className="form-control"
                    value={email} onChange={e => setEmail(e.target.value)}
                    placeholder="customer@example.com" required />
                </div>
                <div className="mb-3">
                  <label className="form-label">Password</label>
                  <input type="password" className="form-control"
                    value={password} onChange={e => setPassword(e.target.value)}
                    placeholder="Enter password" required />
                </div>
                <button type="submit" className="btn btn-success w-100 py-2 fw-semibold">
                  Sign In
                </button>
              </form>

              <p className="text-center text-muted small mt-3 mb-0">
                Demo: enter any email + password to login
              </p>

              <hr />
              <p className="text-center small mb-0">
                <Link to="/products" className="text-decoration-none">Continue as Guest</Link>
              </p>
            </div>
          </div>

          <div className="card shadow-sm border-0 mt-3">
            <div className="card-body p-3">
              <h6 className="fw-bold mb-2">Admin Login</h6>
              <p className="small text-muted mb-1">For staff, use the admin dashboard:</p>
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