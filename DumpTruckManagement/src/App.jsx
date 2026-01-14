import React from 'react'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import './pages/styles.css'

import MainLayout from './layouts/MainLayout'
import AuthLayout from './layouts/AuthLayout'

import Dashboard from './pages/Dashboard'
import DumpTrucks from './pages/DumpTrucks'
import Drivers from './pages/Drivers'
import Sites from './pages/Sites'
import Shifts from './pages/Shifts'
import ShiftTruckEntries from './pages/ShiftTruckEntries'
import ExpenseTypes from './pages/ExpenseTypes'
import ShiftExpenses from './pages/ShiftExpenses'
import MaintenanceTypes from './pages/MaintenanceTypes'
import MaintenanceRecords from './pages/MaintenanceRecords'
import RevenueRates from './pages/RevenueRates'
import AuthPage from './pages/AuthPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/auth" element={<AuthLayout><AuthPage /></AuthLayout>} />

        <Route element={<MainLayout />}>
          <Route index element={<Dashboard />} />
          <Route path="dashboard" element={<Dashboard />} />
          <Route path="dump-trucks" element={<DumpTrucks />} />
          <Route path="drivers" element={<Drivers />} />
          <Route path="sites" element={<Sites />} />
          <Route path="shifts" element={<Shifts />} />
          <Route path="shift-truck-entries" element={<ShiftTruckEntries />} />
          <Route path="expense-types" element={<ExpenseTypes />} />
          <Route path="shift-expenses" element={<ShiftExpenses />} />
          <Route path="maintenance-types" element={<MaintenanceTypes />} />
          <Route path="maintenance-records" element={<MaintenanceRecords />} />
          <Route path="revenue-rates" element={<RevenueRates />} />
        </Route>

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
