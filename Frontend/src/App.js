import React, { useState } from 'react'
import { Routes, Route, Link, useLocation } from 'react-router-dom'
import ProductList from './components/ProductList'
import ProductDetail from './components/ProductDetail'
import CategoryList from './components/CategoryList'
import Cart from './components/Cart'
import Checkout from './components/Checkout'
import Login from './components/Login'
import About from './components/About'
import Contact from './components/Contact'
import { CartProvider, useCart } from './context/CartContext'
import './App.css'

function App() {
  return (
    <CartProvider>
      <AppContent />
    </CartProvider>
  )
}

function AppContent() {
  const location = useLocation()
  const [menuOpen, setMenuOpen] = useState(false)
  const { cartCount } = useCart()

  return (
    <div className="d-flex flex-column min-vh-100">
      <nav className="navbar navbar-expand-lg navbar-dark bg-success">
        <div className="container">
          <Link className="navbar-brand fw-bold" to="/">FreshMart</Link>
          <button className="navbar-toggler" type="button" aria-expanded={menuOpen}
            aria-controls="navbarNav" onClick={() => setMenuOpen(open => !open)}>
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className={`navbar-collapse ${menuOpen ? 'show' : ''}`} id="navbarNav">
            <ul className="navbar-nav">
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname === '/' ? 'active' : ''}`} to="/"
                  onClick={() => setMenuOpen(false)}>Home</Link>
              </li>
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname.startsWith('/products') ? 'active' : ''}`} to="/products"
                  onClick={() => setMenuOpen(false)}>Products</Link>
              </li>
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname.startsWith('/categories') ? 'active' : ''}`} to="/categories"
                  onClick={() => setMenuOpen(false)}>Categories</Link>
              </li>
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname.startsWith('/about') ? 'active' : ''}`} to="/about"
                  onClick={() => setMenuOpen(false)}>About</Link>
              </li>
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname.startsWith('/contact') ? 'active' : ''}`} to="/contact"
                  onClick={() => setMenuOpen(false)}>Contact</Link>
              </li>
            </ul>
            <ul className="navbar-nav ms-auto">
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname.startsWith('/login') ? 'active' : ''}`} to="/login"
                  onClick={() => setMenuOpen(false)}>Login</Link>
              </li>
              <li className="nav-item">
                <Link className={`nav-link position-relative ${location.pathname.startsWith('/cart') ? 'active' : ''}`} to="/cart"
                  onClick={() => setMenuOpen(false)}>
                  Cart
                  {cartCount > 0 && (
                    <span className="badge bg-warning text-dark rounded-pill ms-1">{cartCount}</span>
                  )}
                </Link>
              </li>
            </ul>
          </div>
        </div>
      </nav>

      <main className="flex-grow-1">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/products" element={<ProductList />} />
          <Route path="/products/:id" element={<ProductDetail />} />
          <Route path="/categories" element={<CategoryList />} />
          <Route path="/cart" element={<Cart />} />
          <Route path="/checkout" element={<Checkout />} />
          <Route path="/login" element={<Login />} />
          <Route path="/about" element={<About />} />
          <Route path="/contact" element={<Contact />} />
        </Routes>
      </main>

      <footer className="bg-dark text-white text-center py-3 mt-auto">
        <div className="container">
          <p className="mb-0 small">&copy; 2026 FreshMart. All rights reserved.</p>
        </div>
      </footer>
    </div>
  )
}

function Home() {
  return (
    <>
      <section className="bg-success text-white py-5">
        <div className="container text-center">
          <h1 className="display-4 fw-bold">FreshMart</h1>
          <p className="lead mb-4">Your trusted supermarket for fresh products at great prices.</p>
          <Link to="/products" className="btn btn-light btn-lg px-4">Browse Products</Link>
        </div>
      </section>

      <section className="container my-5">
        <div className="row g-4">
          <div className="col-md-4">
            <Link to="/products" className="text-decoration-none">
              <div className="card border-0 shadow-sm text-center h-100 card-hover">
                <div className="card-body py-4">
                  <div className="text-success mb-3">
                    <svg width="40" height="40" fill="currentColor" viewBox="0 0 16 16">
                      <path d="M8 1a2.5 2.5 0 0 1 2.5 2.5V4h-5v-.5A2.5 2.5 0 0 1 8 1zm3.5 3v-.5a3.5 3.5 0 1 0-7 0V4H1v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V4h-3.5zM2 5h12v9a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V5z"/>
                    </svg>
                  </div>
                  <h5 className="fw-semibold text-dark">Product Catalog</h5>
                  <p className="text-muted small mb-0">Browse our full selection of products with detailed info and pricing</p>
                </div>
              </div>
            </Link>
          </div>
          <div className="col-md-4">
            <Link to="/categories" className="text-decoration-none">
              <div className="card border-0 shadow-sm text-center h-100 card-hover">
                <div className="card-body py-4">
                  <div className="text-success mb-3">
                    <svg width="40" height="40" fill="currentColor" viewBox="0 0 16 16">
                      <path d="M1 2.5A1.5 1.5 0 0 1 2.5 1h3A1.5 1.5 0 0 1 7 2.5v3A1.5 1.5 0 0 1 5.5 7h-3A1.5 1.5 0 0 1 1 5.5v-3zm8 0A1.5 1.5 0 0 1 10.5 1h3A1.5 1.5 0 0 1 15 2.5v3A1.5 1.5 0 0 1 13.5 7h-3A1.5 1.5 0 0 1 9 5.5v-3zm-8 8A1.5 1.5 0 0 1 2.5 9h3A1.5 1.5 0 0 1 7 10.5v3A1.5 1.5 0 0 1 5.5 15h-3A1.5 1.5 0 0 1 1 13.5v-3zm8 0A1.5 1.5 0 0 1 10.5 9h3a1.5 1.5 0 0 1 1.5 1.5v3a1.5 1.5 0 0 1-1.5 1.5h-3A1.5 1.5 0 0 1 9 13.5v-3z"/>
                    </svg>
                  </div>
                  <h5 className="fw-semibold text-dark">Shop by Category</h5>
                  <p className="text-muted small mb-0">Find exactly what you need with our easy category browsing</p>
                </div>
              </div>
            </Link>
          </div>
          <div className="col-md-4">
            <Link to="/products" className="text-decoration-none">
              <div className="card border-0 shadow-sm text-center h-100 card-hover">
                <div className="card-body py-4">
                  <div className="text-success mb-3">
                    <svg width="40" height="40" fill="currentColor" viewBox="0 0 16 16">
                      <path d="M2.5 0a.5.5 0 0 1 .5.5V2h10V.5a.5.5 0 0 1 1 0V2h1a1 1 0 0 1 1 1v11a1 1 0 0 1-1 1H1a1 1 0 0 1-1-1V3a1 1 0 0 1 1-1h1V.5a.5.5 0 0 1 .5-.5zM1 4v10h14V4H1zm3 2.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm0 3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm0 3a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5z"/>
                    </svg>
                  </div>
                  <h5 className="fw-semibold text-dark">Real-time Stock</h5>
                  <p className="text-muted small mb-0">Check product availability and stock levels instantly</p>
                </div>
              </div>
            </Link>
          </div>
        </div>
      </section>
    </>
  )
}

export default App