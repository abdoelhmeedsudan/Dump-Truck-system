const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5004/api'

// Get auth token from localStorage
function getAuthToken() {
  return localStorage.getItem('authToken')
}

// Set auth token
function setAuthToken(token) {
  localStorage.setItem('authToken', token)
}

// Remove auth token
function removeAuthToken() {
  localStorage.removeItem('authToken')
}

// Helper function to extract array from API response
export function extractArrayFromResponse(data) {
  console.log('[extractArrayFromResponse] Input data:', data)
  console.log('[extractArrayFromResponse] Data type:', typeof data)
  console.log('[extractArrayFromResponse] Is null/undefined?', data == null)
  console.log('[extractArrayFromResponse] Is array?', Array.isArray(data))

  // Handle null or undefined
  if (data == null) {
    console.log('[extractArrayFromResponse] Data is null/undefined, returning empty array')
    return []
  }

  // If it's already an array, return it
  if (Array.isArray(data)) {
    console.log('[extractArrayFromResponse] Data is array, returning as is')
    return data
  }

  // If it's an object, check for common array properties
  if (typeof data === 'object') {
    console.log('[extractArrayFromResponse] Data is object, checking properties')

    // Check for nested structure: data.data.items (API response structure)
    if (data.data && typeof data.data === 'object') {
      if (Array.isArray(data.data.items)) {
        console.log('[extractArrayFromResponse] Found data.data.items array')
        return data.data.items
      }
      if (Array.isArray(data.data.data)) {
        console.log('[extractArrayFromResponse] Found data.data.data array')
        return data.data.data
      }
    }

    // Check for direct properties
    if (Array.isArray(data.items)) {
      console.log('[extractArrayFromResponse] Found data.items array')
      return data.items
    }
    if (Array.isArray(data.data)) {
      console.log('[extractArrayFromResponse] Found data.data array')
      return data.data
    }
    if (Array.isArray(data.results)) {
      console.log('[extractArrayFromResponse] Found data.results array')
      return data.results
    }

    console.log('[extractArrayFromResponse] Object has no array properties, keys:', Object.keys(data))
  }

  // For any other case (primitive, unexpected structure), return empty array
  console.log('[extractArrayFromResponse] Unexpected structure, returning empty array')
  return []
}

// Helper function to extract paginated data from API response
export function extractPaginatedData(data) {
  console.log('[extractPaginatedData] Input data:', data)

  if (data == null) {
    return { items: [], currentPage: 1, totalPages: 1, totalCount: 0 }
  }

  // Check api response structure (data.data is the payload)
  if (data.data && typeof data.data === 'object') {
    // If it has items array, it's likely the paginated object
    if (Array.isArray(data.data.items)) {
      return {
        items: data.data.items,
        currentPage: data.data.currentPage || 1,
        totalPages: data.data.totalPages || 1,
        totalCount: data.data.totalCount || 0,
        pageSize: data.data.pageSize || 10,
        hasPrevious: data.data.hasPrevious || false,
        hasNext: data.data.hasNext || false
      }
    }
  }

  // Fallback for non-standard responses
  const items = extractArrayFromResponse(data)
  return {
    items,
    currentPage: 1,
    totalPages: 1,
    totalCount: items.length
  }
}

// Helper function to extract object from API response (for getById)
export function extractObjectFromResponse(data) {
  console.log('[extractObjectFromResponse] Input data:', data)
  console.log('[extractObjectFromResponse] Data type:', typeof data)
  console.log('[extractObjectFromResponse] Is null/undefined?', data == null)

  // Handle null or undefined
  if (data == null) {
    console.log('[extractObjectFromResponse] Data is null/undefined, returning null')
    return null
  }

  // If it's already an object (not array), check if it has the data we need
  if (typeof data === 'object' && !Array.isArray(data)) {
    // Check for nested structure: data.data (API response structure)
    if (data.data && typeof data.data === 'object' && !Array.isArray(data.data)) {
      // Check if data.data has the actual object properties (not just metadata)
      if (data.data.id || Object.keys(data.data).length > 0) {
        console.log('[extractObjectFromResponse] Found data.data object')
        return data.data
      }
    }

    // Check if the object itself has the properties we need (like id, name, etc.)
    // If it has common entity properties, return it as is
    if (data.id || Object.keys(data).length > 0) {
      console.log('[extractObjectFromResponse] Data is already the object, returning as is')
      return data
    }

    console.log('[extractObjectFromResponse] Object structure:', Object.keys(data))
  }

  // For any other case, return the data as is
  console.log('[extractObjectFromResponse] Returning data as is')
  return data
}

// Base fetch function with auth
async function apiRequest(endpoint, options = {}) {
  const token = getAuthToken()
  const url = `${API_BASE_URL}${endpoint}`

  const config = {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
      ...options.headers,
    },
  }

  try {
    console.log('[apiRequest] Making request to:', url)
    console.log('[apiRequest] Request config:', {
      method: config.method || 'GET',
      headers: config.headers,
      hasBody: !!config.body
    })
    const response = await fetch(url, config)
    console.log('[apiRequest] Response status:', response.status)
    console.log('[apiRequest] Response ok:', response.ok)

    if (response.status === 401) {
      console.log('[apiRequest] Unauthorized, redirecting to auth')
      removeAuthToken()
      globalThis.location.href = '/auth'
      throw new Error('Unauthorized')
    }

    if (!response.ok) {
      console.error('[apiRequest] Response not ok, status:', response.status)
      const error = await response.json().catch(() => {
        console.error('[apiRequest] Failed to parse error response as JSON')
        return { message: 'An error occurred' }
      })
      console.error('[apiRequest] Error response:', error)
      throw new Error(error.message || `HTTP error! status: ${response.status}`)
    }

    // Handle empty responses
    const contentType = response.headers.get('content-type')
    console.log('[apiRequest] Response content-type:', contentType)
    if (contentType?.includes('application/json')) {
      const jsonData = await response.json()
      console.log('[apiRequest] Response JSON data:', jsonData)
      return jsonData
    }
    console.log('[apiRequest] Response is not JSON, returning null')
    return null
  } catch (error) {
    console.error('[apiRequest] API Request Error:', error)
    console.error('[apiRequest] Error details:', {
      message: error.message,
      stack: error.stack,
      name: error.name,
      url: url
    })
    throw error
  }
}

// Driver API
export const driverApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.isActive !== undefined) queryParams.append('IsActive', params.isActive)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/Driver?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/Driver/${id}`),
  create: (data) => apiRequest('/Driver', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/Driver', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/Driver/${id}`, { method: 'DELETE' }),
}

// DumpTruck API
export const dumpTruckApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/DumpTruck?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/DumpTruck/${id}`),
  create: (data) => apiRequest('/DumpTruck', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/DumpTruck', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/DumpTruck/${id}`, { method: 'DELETE' }),
}

// Site API
export const siteApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/Site?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/Site/${id}`),
  create: (data) => apiRequest('/Site', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/Site', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/Site/${id}`, { method: 'DELETE' }),
}

// Shift API
export const shiftApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.siteId) queryParams.append('SiteId', params.siteId)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/Shift?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/Shift/${id}`),
  create: (data) => apiRequest('/Shift', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/Shift', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/Shift/${id}`, { method: 'DELETE' }),
}

// ShiftTruckEntry API
export const shiftTruckEntryApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.shiftId) queryParams.append('ShiftId', params.shiftId)
    if (params.dumpTruckId) queryParams.append('DumpTruckId', params.dumpTruckId)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/ShiftTruckEntry?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/ShiftTruckEntry/${id}`),
  create: (data) => apiRequest('/ShiftTruckEntry', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/ShiftTruckEntry', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/ShiftTruckEntry/${id}`, { method: 'DELETE' }),
}

// ExpenseType API
export const expenseTypeApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.isActive !== undefined) queryParams.append('IsActive', params.isActive)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/ExpenseType?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/ExpenseType/${id}`),
  create: (data) => apiRequest('/ExpenseType', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/ExpenseType', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/ExpenseType/${id}`, { method: 'DELETE' }),
}

// ShiftExpense API
export const shiftExpenseApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.shiftTruckEntryId) queryParams.append('ShiftTruckEntryId', params.shiftTruckEntryId)
    if (params.expenseTypeId) queryParams.append('ExpenseTypeId', params.expenseTypeId)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/ShiftExpense?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/ShiftExpense/${id}`),
  create: (data) => apiRequest('/ShiftExpense', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/ShiftExpense', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/ShiftExpense/${id}`, { method: 'DELETE' }),
}

// MaintenanceType API
export const maintenanceTypeApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.isActive !== undefined) queryParams.append('IsActive', params.isActive)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/MaintenanceType?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/MaintenanceType/${id}`),
  create: (data) => apiRequest('/MaintenanceType', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/MaintenanceType', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/MaintenanceType/${id}`, { method: 'DELETE' }),
}

// MaintenanceRecord API
export const maintenanceRecordApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.dumpTruckId) queryParams.append('DumpTruckId', params.dumpTruckId)
    if (params.maintenanceTypeId) queryParams.append('MaintenanceTypeId', params.maintenanceTypeId)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/MaintenanceRecord?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/MaintenanceRecord/${id}`),
  create: (data) => apiRequest('/MaintenanceRecord', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/MaintenanceRecord', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/MaintenanceRecord/${id}`, { method: 'DELETE' }),
}

// RevenueRate API
export const revenueRateApi = {
  getAll: (params = {}) => {
    const queryParams = new URLSearchParams()
    if (params.searchTerm) queryParams.append('SearchTerm', params.searchTerm)
    if (params.siteId) queryParams.append('SiteId', params.siteId)
    if (params.pageNumber) queryParams.append('PageNumber', params.pageNumber)
    if (params.pageSize) queryParams.append('PageSize', params.pageSize)
    return apiRequest(`/RevenueRate?${queryParams.toString()}`)
  },
  getById: (id) => apiRequest(`/RevenueRate/${id}`),
  create: (data) => apiRequest('/RevenueRate', { method: 'POST', body: JSON.stringify(data) }),
  update: (data) => apiRequest('/RevenueRate', { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiRequest(`/RevenueRate/${id}`, { method: 'DELETE' }),
}

// Dashboard API
export const dashboardApi = {
  getStats: () => apiRequest('/Dashboard'),
}

// User API
export const userApi = {
  register: (data) => apiRequest('/Auth/register', { method: 'POST', body: JSON.stringify(data) }),
  changeUsername: (data) => apiRequest('/Auth/change-username', { method: 'POST', body: JSON.stringify(data) }),
  changePassword: (data) => apiRequest('/Auth/change-password', { method: 'POST', body: JSON.stringify(data) }),
}

// Auth functions
export const authApi = {
  login: async (email, password) => {
    // The API expects userNameOrEmail
    const response = await apiRequest('/Auth/login', {
      method: 'POST',
      body: JSON.stringify({ userNameOrEmail: email, password }),
    })
    console.log('Login response:', response)

    // Handle response structure { data: { token, userName, ... } }
    const authData = response?.data || response

    if (authData?.token) {
      setAuthToken(authData.token)
      // Store user info
      if (authData.userName) localStorage.setItem('username', authData.userName)
      // Roles might be coming differently or decoded from token, skipping for now unless explicit
    }
    return response
  },
  logout: () => {
    removeAuthToken()
    localStorage.removeItem('username')
    localStorage.removeItem('roles')
    globalThis.location.href = '/auth'
  },
  getToken: getAuthToken,
  isAuthenticated: () => !!getAuthToken(),
  getCurrentUser: () => ({
    username: localStorage.getItem('username'),
    roles: JSON.parse(localStorage.getItem('roles') || '[]')
  })
}

export default {
  driverApi,
  dumpTruckApi,
  siteApi,
  shiftApi,
  shiftTruckEntryApi,
  expenseTypeApi,
  shiftExpenseApi,
  maintenanceTypeApi,
  maintenanceRecordApi,
  revenueRateApi,
  dashboardApi,
  authApi,
  userApi,
}
