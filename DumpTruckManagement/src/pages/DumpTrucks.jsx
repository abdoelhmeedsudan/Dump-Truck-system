import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import Pagination from '../components/Pagination'
import { dumpTruckApi, extractPaginatedData, extractObjectFromResponse } from '../services/apiService'
import '../pages/styles.css'

// DumpTruckStatus enum: 1=Active, 2=Inactive, 3=Maintenance
const statusOptions = [
  { value: 1, label: 'Active' },
  { value: 2, label: 'Inactive' },
  { value: 3, label: 'Maintenance' }
]

const validationSchema = Yup.object({
  truckNumber: Yup.string().required('مطلوب'),
  plateNumber: Yup.string().required('مطلوب'),
  truckType: Yup.string().required('مطلوب'),
  model: Yup.string().required('مطلوب'),
  loadCapacity: Yup.number().required('مطلوب').min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  status: Yup.number().required('مطلوب'),
  notes: Yup.string()
})

export default function DumpTrucks() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [pageSize] = useState(10)

  const initialValues = {
    truckNumber: '',
    plateNumber: '',
    truckType: '',
    model: '',
    loadCapacity: '',
    status: 1,
    notes: ''
  }

  useEffect(() => {
    loadData()
  }, [currentPage])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await dumpTruckApi.getAll({
        pageNumber: currentPage,
        pageSize: pageSize
      })
      const paginatedData = extractPaginatedData(data)
      setItems(paginatedData.items)
      setTotalPages(paginatedData.totalPages)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      console.error('Error loading dump trucks:', err)
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      const submitData = {
        ...values,
        loadCapacity: Number.parseFloat(values.loadCapacity) || 0
      }
      if (editingItem) {
        await dumpTruckApi.update({ ...submitData, id: editingItem.id })
      } else {
        await dumpTruckApi.create(submitData)
      }
      await loadData()
      resetForm()
      setIsModalOpen(false)
      setEditingItem(null)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء الحفظ')
      setSubmitting(false)
    }
  }

  async function handleEdit(item) {
    try {
      const response = await dumpTruckApi.getById(item.id)
      const fullItem = extractObjectFromResponse(response)

      // Ensure we have the required fields with proper defaults
      const itemToEdit = {
        id: fullItem?.id || item.id,
        truckNumber: fullItem?.truckNumber || item.truckNumber || '',
        plateNumber: fullItem?.plateNumber || item.plateNumber || '',
        truckType: fullItem?.truckType || item.truckType || '',
        model: fullItem?.model || item.model || '',
        loadCapacity: fullItem?.loadCapacity !== undefined ? fullItem.loadCapacity : (item.loadCapacity !== undefined ? item.loadCapacity : ''),
        status: fullItem?.status !== undefined ? fullItem.status : (item.status !== undefined ? item.status : 1),
        notes: fullItem?.notes || item.notes || ''
      }

      setEditingItem(itemToEdit)
      setIsModalOpen(true)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
    }
  }

  async function handleDelete(id) {
    if (globalThis.confirm('هل أنت متأكد من حذف هذا العنصر؟')) {
      try {
        setError(null)
        await dumpTruckApi.delete(id)
        await loadData()
      } catch (err) {
        setError(err.message || 'حدث خطأ أثناء الحذف')
      }
    }
  }

  function handleAdd() {
    setEditingItem(null)
    setIsModalOpen(true)
  }

  function handleClose() {
    setIsModalOpen(false)
    setEditingItem(null)
    setError(null)
  }

  // Handler for page change
  function handlePageChange(newPage) {
    if (newPage >= 1 && newPage <= totalPages) {
      setCurrentPage(newPage)
    }
  }

  function getStatusLabel(status) {
    const option = statusOptions.find(opt => opt.value === status)
    return option ? option.label : status
  }

  return (
    <div className="page">
      <h2>القلابات — Dump Trucks</h2>

      {error && (
        <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title">
          <span>قائمة القلابات</span>
          <button onClick={handleAdd} disabled={loading} className="primary">
            + إضافة قلاب جديد
          </button>
        </div>

        <div className="table-container">
          {loading ? (
            <div style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
              <div className="loading"></div> جاري التحميل...
            </div>
          ) : (
            <table className="table">
              <thead>
                <tr>
                  <th>رقم القلاب</th>
                  <th>لوحة</th>
                  <th>النوع</th>
                  <th>الطراز</th>
                  <th>السعة</th>
                  <th>الحالة</th>
                  <th>ملاحظات</th>
                  <th>الإجراءات</th>
                </tr>
              </thead>
              <tbody>
                {items.length === 0 ? (
                  <tr>
                    <td colSpan="8" style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
                      لا توجد بيانات
                    </td>
                  </tr>
                ) : (
                  items.map((it) => (
                    <tr key={it.id}>
                      <td>{it.truckNumber}</td>
                      <td>{it.plateNumber}</td>
                      <td>{it.truckType}</td>
                      <td>{it.model}</td>
                      <td>{it.loadCapacity}</td>
                      <td>
                        <span className={`badge ${it.status === 1 ? 'badge-success' :
                            it.status === 3 ? 'badge-warning' : 'badge-error'
                          }`}>
                          {getStatusLabel(it.status)}
                        </span>
                      </td>
                      <td>{it.notes}</td>
                      <td>
                        <div style={{ display: 'flex', gap: '0.5rem' }}>
                          <button onClick={() => handleEdit(it)} className="secondary" style={{ padding: '0.4rem 0.8rem', fontSize: '0.85rem' }}>تعديل</button>
                          <button onClick={() => handleDelete(it.id)} className="secondary" style={{ padding: '0.4rem 0.8rem', fontSize: '0.85rem', color: 'var(--error)', borderColor: 'var(--error)' }}>حذف</button>
                        </div>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          )}
        </div>
      </div>

      {!loading && (
        <Pagination
          currentPage={currentPage}
          totalPages={totalPages}
          onPageChange={handlePageChange}
        />
      )}

      <Modal
        isOpen={isModalOpen}
        onClose={handleClose}
        title={editingItem ? 'تعديل قلاب' : 'إضافة قلاب جديد'}
      >
        <Formik
          initialValues={editingItem || initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
          enableReinitialize
        >
          {({ isSubmitting }) => (
            <Form>
              <div className="form-grid">
                <FormField name="truckNumber" label="رقم القلاب / Truck Number" />
                <FormField name="plateNumber" label="لوحة / Plate Number" />
                <FormField name="truckType" label="نوع القلاب / Truck Type" />
                <FormField name="model" label="الطراز / Model" />
                <FormField name="loadCapacity" type="number" label="سعة الحمولة / Load Capacity" />
                <FormField name="status" type="select" label="الحالة / Status">
                  {statusOptions.map(opt => (
                    <option key={opt.value} value={opt.value}>{opt.label}</option>
                  ))}
                </FormField>
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr', marginTop: '1rem' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" rows="3" />
              </div>

              <div className="actions">
                <button type="button" onClick={handleClose} className="secondary">
                  إلغاء
                </button>
                <button type="submit" disabled={isSubmitting} className="primary">
                  {editingItem ? 'حفظ التعديلات' : 'إضافة قلاب'}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}

