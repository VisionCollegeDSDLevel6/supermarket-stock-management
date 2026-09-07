import React from 'react'
import { Routes, Route, Link, useLocation } from 'react-router-dom'
import ProductList from './components/ProductList'
import ProductDetail from './components/ProductDetail'

function App() {
  const location = useLocation()

  return (
    <div className="d-flex flex-column min-vh-100">
      <nav className="navbar navbar-expand-lg navbar-dark bg-success">
        <div className="container">
          <Link className="navbar-brand fw-bold" to="/">FreshMart</Link>
          <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="navbar-nav ms-auto">
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname === '/' ? 'active' : ''}`} to="/">Home</Link>
              </li>
              <li className="nav-item">
                <Link className={`nav-link ${location.pathname.startsWith('/products') ? 'active' : ''}`} to="/products">Products</Link>
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
            <div className="card border-0 shadow-sm text-center h-100">
              <div className="card-body py-4">
                <h5 className="fw-semibold">Fresh Products</h5>
                <p className="text-muted small mb-0">Quality guaranteed every day</p>
              </div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="card border-0 shadow-sm text-center h-100">
              <div className="card-body py-4">
                <h5 className="fw-semibold">Best Prices</h5>
                <p className="text-muted small mb-0">Competitive prices on all items</p>
              </div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="card border-0 shadow-sm text-center h-100">
              <div className="card-body py-4">
                <h5 className="fw-semibold">Fast Delivery</h5>
                <p className="text-muted small mb-0">Same-day delivery available</p>
              </div>
            </div>
          </div>
        </div>
      </section>
    </>
  )
}

export default App