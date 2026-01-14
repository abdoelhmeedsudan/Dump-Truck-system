import React from 'react'
import './layouts.css'

export default function AuthLayout({ children }) {
  return (
    <div className="auth-layout">
      <div className="auth-card">
        <h2 className="auth-title">Welcome back</h2>
        {children}
      </div>
    </div>
  )
}
