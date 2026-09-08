import React from 'react'
import { Link } from 'react-router-dom'
import { useCart } from '../context/CartContext'

function Cart() {
  const { items, removeFromCart, updateQuantity, cartTotal, clearCart } = useCart()

  if (items.length === 0) {
    return (
      <div className="container my-5 text-center">
        <h2 className="fw-bold mb-3">Your Cart is Empty</h2>
        <p className="text-muted mb-4">Looks like you haven't added anything yet.</p>
        <Link to="/products" className="btn btn-success btn-lg">Browse Products</Link>
      </div>
    )
  }

  return (
    <div className="container my-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="fw-bold mb-0">Shopping Cart</h1>
          <p className="text-muted mb-0">{items.length} item(s) in your cart</p>
        </div>
        <button className="btn btn-outline-danger btn-sm" onClick={clearCart}>
          Clear Cart
        </button>
      </div>

      <div className="row">
        <div className="col-lg-8">
          {items.map(item => (
            <div key={item.productId} className="card shadow-sm border-0 mb-3">
              <div className="card-body">
                <div className="row align-items-center">
                  <div className="col-3 col-md-2">
                    {item.imageUrl ? (
                      <img src={item.imageUrl} alt={item.name}
                        className="img-fluid rounded" style={{ objectFit: 'cover', height: '80px', width: '80px' }} />
                    ) : (
                      <div className="bg-light rounded d-flex align-items-center justify-content-center"
                        style={{ height: '80px', width: '80px' }}>
                        <span className="text-muted small">No img</span>
                      </div>
                    )}
                  </div>
                  <div className="col-4 col-md-4">
                    <Link to={`/products/${item.productId}`} className="text-decoration-none text-dark">
                      <h6 className="fw-semibold mb-1">{item.name}</h6>
                    </Link>
                    <p className="text-success fw-bold mb-0">${item.price.toFixed(2)}</p>
                  </div>
                  <div className="col-3 col-md-3">
                    <div className="input-group input-group-sm">
                      <button className="btn btn-outline-secondary"
                        onClick={() => updateQuantity(item.productId, item.quantity - 1)}>−</button>
                      <input type="text" className="form-control text-center"
                        value={item.quantity} readOnly style={{ maxWidth: '50px' }} />
                      <button className="btn btn-outline-secondary"
                        onClick={() => updateQuantity(item.productId, item.quantity + 1)}>+</button>
                    </div>
                  </div>
                  <div className="col-2 col-md-2 text-end">
                    <p className="fw-bold mb-0">${(item.price * item.quantity).toFixed(2)}</p>
                    <button className="btn btn-sm text-danger p-0 mt-1"
                      onClick={() => removeFromCart(item.productId)}>Remove</button>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>

        <div className="col-lg-4">
          <div className="card shadow-sm border-0">
            <div className="card-body">
              <h5 className="fw-bold mb-3">Order Summary</h5>
              <hr />
              <div className="d-flex justify-content-between mb-2">
                <span className="text-muted">Subtotal ({items.length} items)</span>
                <span>${cartTotal.toFixed(2)}</span>
              </div>
              <div className="d-flex justify-content-between mb-2">
                <span className="text-muted">Shipping</span>
                <span className="text-success">Free</span>
              </div>
              <hr />
              <div className="d-flex justify-content-between mb-3">
                <span className="fw-bold">Total</span>
                <span className="fw-bold fs-5 text-success">${cartTotal.toFixed(2)}</span>
              </div>
              <Link to="/checkout" className="btn btn-success w-100 py-2 fw-semibold">
                Proceed to Checkout
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Cart