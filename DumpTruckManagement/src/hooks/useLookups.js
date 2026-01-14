import { useState, useEffect } from 'react'
import { siteApi, driverApi, dumpTruckApi, expenseTypeApi, maintenanceTypeApi, extractArrayFromResponse } from '../services/apiService'

export function useSites() {
  const [sites, setSites] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    async function load() {
      try {
        const data = await siteApi.getAll()
        setSites(extractArrayFromResponse(data))
      } catch (err) {
        console.error('Error loading sites:', err)
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  return { sites, loading }
}

export function useDrivers() {
  const [drivers, setDrivers] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    async function load() {
      try {
        const data = await driverApi.getAll()
        setDrivers(extractArrayFromResponse(data))
      } catch (err) {
        console.error('Error loading drivers:', err)
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  return { drivers, loading }
}

export function useDumpTrucks() {
  const [dumpTrucks, setDumpTrucks] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    async function load() {
      try {
        const data = await dumpTruckApi.getAll()
        setDumpTrucks(extractArrayFromResponse(data))
      } catch (err) {
        console.error('Error loading dump trucks:', err)
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  return { dumpTrucks, loading }
}

export function useExpenseTypes() {
  const [expenseTypes, setExpenseTypes] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    async function load() {
      try {
        const data = await expenseTypeApi.getAll({ isActive: true })
        setExpenseTypes(extractArrayFromResponse(data))
      } catch (err) {
        console.error('Error loading expense types:', err)
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  return { expenseTypes, loading }
}

export function useMaintenanceTypes() {
  const [maintenanceTypes, setMaintenanceTypes] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    async function load() {
      try {
        const data = await maintenanceTypeApi.getAll({ isActive: true })
        setMaintenanceTypes(extractArrayFromResponse(data))
      } catch (err) {
        console.error('Error loading maintenance types:', err)
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  return { maintenanceTypes, loading }
}
