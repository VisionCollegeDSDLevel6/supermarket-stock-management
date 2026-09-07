import React, { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import { getProduct } from '../api/productsApi'

function ProductDetail() {
  const { id } = useParams()
  const [product, setProduct] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadProduct()
  }, [id])

  const loadProduct = async () => {
    try {
      const data = await getProduct(id)
      setProduct(data)
    } catch (err) {
      console.error('Failed to load product', err)
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return (
      <div className="container text-center py-5">
        <div className="spinner-border text-success" role="status"></div>
      </div>
    )
  }

  if (!product) {
    return (
      <div className="container text-center py-5">
        <h3 className="text-muted">Product not found</h3>
        <Link to="/products" className="btn btn-success mt-3">Back to Products</Link>
      </div>
    )
  }

  const isLowStock = product.stock && product.stock.quantity <= product.stock.lowStockThreshold
  const inStock = product.stock && product.stock.quantity > 0

  return (
    <div className="container my-4">
      <nav aria-label="breadcrumb">
        <ol className="breadcrumb">
          <li className="breadcrumb-item"><Link to="/">Home</Link></li>
          <li className="breadcrumb-item"><Link to="/products">Products</Link></li>
          <li className="breadcrumb-item active">{product.name}</li>
        </ol>
      </nav>

      <div className="card shadow-sm border-0">
        <div className="row g-0">
          <div className="col-md-5">
            <div className="bg-light d-flex align-items-center justify-content-center"
              style={{ height: '400px' }}>
              {product.imageUrl ? (
                <img src={product.imageUrl} alt={product.name}
                  className="img-fluid" style={{ objectFit: 'cover', height: '100%', width: '100%' }} />
              ) : (
                <span className="text-muted">No Image</span>
              )}
            </div>
          </div>
          <div className="col-md-7">
            <div className="card-body p-4">
              <span className="badge bg-success bg-opacity-10 text-success mb-2">
                {product.category?.name || 'General'}
              </span>
              <h2 className="fw-bold">{product.name}</h2>
              <p className="text-muted mt-3">{product.description || 'No description available.'}</p>

              <hr />

              <div className="d-flex align-items-center mb-3">
                <span className="display-6 fw-bold text-success me-3">${product.price.toFixed(2)}</span>
                {inStock ? (
                  isLowStock ? (
                    <span className="badge bg-warning text-dark fs-6">
                      Low Stock - Only {product.stock.quantity} left
                    </span>
                  ) : (
                    <span className="badge bg-success fs-6">
                      In Stock ({product.stock.quantity} available)
                    </span>
                  )
                ) : (
                  <span className="badge bg-danger fs-6">Out of Stock</span>
                )}
              </div>

              <Link to="/products" className="btn btn-outline-success mt-3">
                Back to Products
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ProductDetail