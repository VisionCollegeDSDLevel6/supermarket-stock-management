import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'

function CategoryList() {
  const [categories, setCategories] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadCategories()
  }, [])

  const loadCategories = async () => {
    try {
      setLoading(true)
      const res = await fetch('/api/categories')
      const data = await res.json()
      setCategories(data)
    } catch (err) {
      console.error('Failed to load categories', err)
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return (
      <div className="container text-center py-5">
        <div className="spinner-border text-success" role="status"></div>
        <p className="mt-2 text-muted">Loading categories...</p>
      </div>
    )
  }

  return (
    <div className="container my-4">
      <div className="mb-4">
        <h1 className="fw-bold">Categories</h1>
        <p className="text-muted mb-0">Browse products by category</p>
      </div>

      {categories.length === 0 ? (
        <div className="text-center py-5">
          <h4 className="text-muted">No categories available</h4>
        </div>
      ) : (
        <div className="row g-4">
          {categories.map(c => (
            <div key={c.categoryId} className="col-12 col-sm-6 col-md-4 col-lg-3">
              <div className="card h-100 shadow-sm border-0">
                <div className="card-body d-flex flex-column">
                  <h5 className="card-title fw-semibold">{c.name}</h5>
                  <p className="card-text text-muted small flex-grow-1">
                    {c.description || 'No description'}
                  </p>
                  <Link
                    to={`/products?categoryId=${c.categoryId}`}
                    className="btn btn-outline-success mt-2 w-100">
                    View Products
                  </Link>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default CategoryList