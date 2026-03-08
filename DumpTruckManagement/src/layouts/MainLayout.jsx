import React, { useState, useEffect } from 'react'
import { NavLink, Outlet, Navigate } from 'react-router-dom'
import {
  LayoutDashboard, Truck, Users, MapPin, Calendar,
  ClipboardList, Tag, Receipt, Wrench, FileText,
  DollarSign, LogOut, ChevronDown, User, Lock, ShieldCheck
} from 'lucide-react'
import { authApi } from '../services/apiService'
import './layouts.css'

export default function MainLayout() {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false)
  const [currentUser, setCurrentUser] = useState({ username: 'المستخدم', roles: [] })

  // Check authentication
  const isAuthenticated = authApi.isAuthenticated()

  useEffect(() => {
    if (isAuthenticated) {
      setCurrentUser(authApi.getCurrentUser() || { username: 'المستخدم', roles: [] })
    }
  }, [isAuthenticated])

  if (!isAuthenticated) {
    return <Navigate to="/auth" replace />
  }

  const linkClass = ({ isActive }) => (isActive ? 'active' : '')

  return (
    <div className="app-shell">
      <header className="main-header">
        <div className="header-left">
          <div className="logo-circle">
            <span className="logo-text">DT</span>
          </div>
          <div>
            <h1 className="header-title">نظام إدارة القلابات</h1>
            <p className="header-subtitle">إدارة العمليات والأسطول</p>
          </div>
        </div>

        <div className="header-right">
          <div className="user-dropdown">
            <button
              className="user-dropdown-trigger"
              onClick={() => setIsDropdownOpen(!isDropdownOpen)}
            >
              <div className="text-left hidden md:block">
                <div className="welcome-text">{currentUser.username || 'المستخدم'}</div>
                <div className="text-xs text-slate-500 flex items-center justify-end gap-1">
                  <span>مدير النظام</span>
                  <ShieldCheck size={12} className="text-primary" />
                </div>
              </div>
              <div className="relative">
                <div className="w-10 h-10 rounded-full bg-slate-100 flex items-center justify-center text-slate-600 border border-slate-200">
                  <User size={20} />
                </div>
                <div className="absolute bottom-0 right-0 w-3 h-3 bg-green-500 rounded-full border-2 border-white" style={{ backgroundColor: '#10b981' }}></div>
              </div>
              <ChevronDown size={16} className={`text-slate-400 transition-transform ${isDropdownOpen ? 'rotate-180' : ''}`} />
            </button>

            {isDropdownOpen && (
              <div className="dropdown-menu">
                <div className="p-4 border-b border-slate-100 md:hidden">
                  <div className="font-semibold text-slate-900">{currentUser.username || 'المستخدم'}</div>
                  <div className="text-xs text-slate-500">مدير النظام</div>
                </div>
                <NavLink to="/profile/change-username" className="dropdown-item" onClick={() => setIsDropdownOpen(false)}>
                  <User size={18} />
                  <span>تغيير اسم المستخدم</span>
                </NavLink>
                <button className="dropdown-item">
                  <Lock size={18} />
                  <span>تغيير كلمة المرور</span>
                </button>
                <div className="dropdown-divider"></div>
                <button className="dropdown-item text-red" onClick={() => {
                  import('../services/apiService').then(module => module.authApi.logout())
                }}>
                  <LogOut size={18} />
                  <span>تسجيل الخروج</span>
                </button>
              </div>
            )}
          </div>
        </div>
      </header>

      <div className="content-wrapper">
        <aside className="sidebar">
          <nav>
            <ul>
              <li>
                <NavLink className={linkClass} to="/dashboard">
                  <LayoutDashboard size={18} className="ml-3" />
                  <span>لوحة التحكم</span>
                </NavLink>
              </li>
              <li className="my-2 border-t border-slate-100"></li>
              <li>
                <NavLink className={linkClass} to="/dump-trucks">
                  <Truck size={18} className="ml-3" />
                  <span>القلابات</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/drivers">
                  <Users size={18} className="ml-3" />
                  <span>السائقين</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/sites">
                  <MapPin size={18} className="ml-3" />
                  <span>المواقع</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/shifts">
                  <Calendar size={18} className="ml-3" />
                  <span>الورديات</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/shift-truck-entries">
                  <ClipboardList size={18} className="ml-3" />
                  <span>سجلات التشغيل</span>
                </NavLink>
              </li>
              <li className="my-2 border-t border-slate-100"></li>
              <li>
                <NavLink className={linkClass} to="/expense-types">
                  <Tag size={18} className="ml-3" />
                  <span>أنواع المصاريف</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/shift-expenses">
                  <Receipt size={18} className="ml-3" />
                  <span>المصاريف</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/maintenance-types">
                  <Wrench size={18} className="ml-3" />
                  <span>أنواع الصيانة</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/maintenance-records">
                  <FileText size={18} className="ml-3" />
                  <span>سجلات الصيانة</span>
                </NavLink>
              </li>
              <li>
                <NavLink className={linkClass} to="/revenue-rates">
                  <DollarSign size={18} className="ml-3" />
                  <span>أسعار النقلات</span>
                </NavLink>
              </li>
              <li className="my-2 border-t border-slate-100"></li>
              <li>
                <div className="px-4 py-2 text-xs font-semibold text-slate-400">الإدارة</div>
              </li>
              <li>
                <NavLink className={linkClass} to="/users/add">
                  <User size={18} className="ml-3" />
                  <span>إضافة مستخدم</span>
                </NavLink>
              </li>
            </ul>
          </nav>
        </aside>

        <div className="main-layout">
          <main className="main-content">
            <Outlet />
          </main>

          <footer className="main-footer">
            © {new Date().getFullYear()} نظام إدارة القلابات - جميع الحقوق محفوظة
          </footer>
        </div>
      </div>
    </div>
  )
}
