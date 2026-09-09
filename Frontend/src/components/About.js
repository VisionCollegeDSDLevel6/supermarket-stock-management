import React from 'react'
import { Link } from 'react-router-dom'

function About() {
  return (
    <div className="container my-4">
      <nav aria-label="breadcrumb">
        <ol className="breadcrumb">
          <li className="breadcrumb-item"><Link to="/">Home</Link></li>
          <li className="breadcrumb-item active">About</li>
        </ol>
      </nav>

      <div className="row justify-content-center">
        <div className="col-lg-8">
          <h1 className="fw-bold mb-4">About FreshMart</h1>

          <div className="card shadow-sm border-0 mb-4">
            <div className="card-body p-4">
              <h5 className="fw-semibold text-success">Our Story</h5>
              <p className="text-muted">
                FreshMart was founded with a simple mission: to provide fresh, high-quality products
                at competitive prices. We are committed to making grocery shopping convenient and
                enjoyable for everyone in our community.
              </p>
              <p className="text-muted">
                Our system is built with modern technology to ensure real-time stock tracking,
                efficient inventory management, and a seamless shopping experience for our customers.
              </p>
            </div>
          </div>

          <div className="card shadow-sm border-0 mb-4">
            <div className="card-body p-4">
              <h5 className="fw-semibold text-success">Our Features</h5>
              <div className="row g-3 mt-2">
                <div className="col-md-6">
                  <div className="d-flex">
                    <span className="text-success me-2">✓</span>
                    <div>
                      <strong>Smart Inventory</strong>
                      <p className="text-muted small mb-0">Real-time stock tracking with low-stock alerts</p>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="d-flex">
                    <span className="text-success me-2">✓</span>
                    <div>
                      <strong>Product Categories</strong>
                      <p className="text-muted small mb-0">Well-organized departments for easy browsing</p>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="d-flex">
                    <span className="text-success me-2">✓</span>
                    <div>
                      <strong>Role-Based Access</strong>
                      <p className="text-muted small mb-0">Secure admin, manager, and staff accounts</p>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="d-flex">
                    <span className="text-success me-2">✓</span>
                    <div>
                      <strong>Stock Reports</strong>
                      <p className="text-muted small mb-0">Detailed reports for inventory analysis</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div className="card shadow-sm border-0">
            <div className="card-body p-4 text-center">
              <h5 className="fw-semibold text-success">Contact Us</h5>
              <p className="text-muted mb-3">Have questions? We'd love to hear from you.</p>
              <Link to="/contact" className="btn btn-success">Get in Touch</Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default About