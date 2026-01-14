import React from 'react'
import '../pages/styles.css'

export default function AuthPage() {
  return (
    <div className="page" style={{ maxWidth: 420 }}>
      <h2>Sign in</h2>
      <form onSubmit={(e) => e.preventDefault()}>
        <div className="form-grid">
          <div className="form-field">
            <label className="form-label">Email</label>
            <div className="form-control"><input type="email" /></div>
          </div>

          <div className="form-field">
            <label className="form-label">Password</label>
            <div className="form-control"><input type="password" /></div>
          </div>
        </div>

        <div className="actions">
          <button type="submit">Sign in</button>
        </div>
      </form>
    </div>
  )
}
