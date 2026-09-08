import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { getProducts } from '../api/productsApi'

function ProductList() {
  const [products, setProducts] = useState([])
  const [categories, setCategories] = useState([])
  const [searchTerm, setSearchTerm] = useState('')
  const [selectedCategory, setSelectedCategory] = useState('')
  const [sortBy, setSortBy] = useState('name')
  const [sortOrder, setSortOrder] = useState('asc')
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    fetchCategories()
  }, [])

  useEffect(() => {
    loadProducts()
  }, [selectedCategory, sortBy, sortOrder])

  const loadProducts = async () => {
    try {
      setLoading(true)
      const params = {}
      if (searchTerm) params.searchTerm = searchTerm
      if (selectedCategory) params.categoryId = selectedCategory
      if (sortBy) params.sortBy = sortBy
      if (sortOrder) params.sortOrder = sortOrder
      const data = await getProducts(params)
      setProducts(data)
    } catch (err) {
      console.error('Failed to load products', err)
    } finally {
      setLoading(false)
    }
  }

  const fetchCategories = async () => {
    try {
      const res = await fetch('/api/categories')
      const data = await res.json()
      setCategories(data)
    } catch (err) {
      console.error('Failed to load categories', err)
    }
  }

  const handleSearch = (e) => {
    e.preventDefault()
    loadProducts()
  }

  return (
    <div className="container my-4">
      <div className="mb-4">
        <h1 className="fw-bold">Our Products</h1>
        <p className="text-muted mb-0">Browse our wide selection of products</p>
      </div>

      <div className="card shadow-sm border-0 mb-4">
        <div className="card-body">
          <form onSubmit={handleSearch} className="row g-3">
            <div className="col-md-5">
              <input type="text" className="form-control" placeholder="Search products..."
                value={searchTerm} onChange={e => setSearchTerm(e.target.value)} />
            </div>
            <div className="col-md-3">
              <select className="form-select" value={selectedCategory}
                onChange={e => setSelectedCategory(e.target.value)}>
                <option value="">All Categories</option>
                {categories.map(c => (
                  <option key={c.categoryId} value={c.categoryId}>{c.name}</option>
                ))}
              </select>
            </div>
            <div className="col-md-2">
              <select className="form-select"
                value={sortOrder === 'desc' ? 'price-desc' : sortBy}
                onChange={e => {
                  const val = e.target.value
                  setSortBy(val === 'price-desc' ? 'price' : val)
                  setSortOrder(val === 'price-desc' ? 'desc' : 'asc')
                }}>
                <option value="name">Name</option>
                <option value="price">Price: Low to High</option>
                <option value="price-desc">Price: High to Low</option>
              </select>
            </div>
            <div className="col-md-2">
              <button type="submit" className="btn btn-success w-100">Search</button>
            </div>
          </form>
        </div>
      </div>

      {loading ? (
        <div className="text-center py-5">
          <div className="spinner-border text-success" role="status"></div>
          <p className="mt-2 text-muted">Loading products...</p>
        </div>
      ) : products.length === 0 ? (
        <div className="text-center py-5">
          <h4 className="text-muted">No products found</h4>
          <p className="text-muted">Try adjusting your search or filter.</p>
        </div>
      ) : (
        <>
          <p className="text-muted mb-3">{products.length} product(s) available</p>
          <div className="row g-4">
            {products.map(p => {
              const isLowStock = p.stock && p.stock.quantity <= p.stock.lowStockThreshold
              return (
                <div key={p.productId} className="col-12 col-sm-6 col-md-4 col-lg-3">
                  <div className="card h-100 shadow-sm border-0">
                    <div className="product-image bg-light d-flex align-items-center justify-content-center">
                      {p.imageUrl ? (
                        <img src={p.imageUrl} alt={p.name} className="img-fluid"
                          style={{ objectFit: 'cover', height: '100%', width: '100%' }} />
                      ) : (
                        <span className="text-muted">No Image</span>
                      )}
                    </div>
                    <div className="card-body d-flex flex-column">
                      <span className="badge bg-success bg-opacity-10 text-success mb-2 align-self-start">
                        {p.category?.name || 'General'}
                      </span>
                      <h5 className="card-title fw-semibold">{p.name}</h5>
                      <p className="card-text text-muted small flex-grow-1">
                        {p.description
                          ? (p.description.length > 80 ? p.description.substring(0, 80) + '...' : p.description)
                          : 'No description'}
                      </p>
                      <div className="d-flex justify-content-between align-items-center mt-2">
                        <span className="fs-5 fw-bold text-success">${p.price.toFixed(2)}</span>
                        {isLowStock ? (
                          <span className="badge bg-warning text-dark">Low Stock</span>
                        ) : p.stock && p.stock.quantity > 0 ? (
                          <span className="badge bg-success">In Stock</span>
                        ) : (
                          <span className="badge bg-danger">Out of Stock</span>
                        )}
                      </div>
                      <Link to={`/products/${p.productId}`}
                        className="btn btn-outline-success mt-3 w-100">
                        View Details
                      </Link>
                    </div>
                  </div>
                </div>
              )
            })}
          </div>
        </>
      )}
    </div>
  )
}

export default ProductList