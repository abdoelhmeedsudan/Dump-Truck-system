import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import Pagination from '../components/Pagination'
import { maintenanceTypeApi, extractPaginatedData, extractObjectFromResponse } from '../services/apiService'
import '../pages/styles.css'

const validationSchema = Yup.object({
  name: Yup.string().required('مطلوب'),
  isActive: Yup.boolean(),
  notes: Yup.string()
})

export default function MaintenanceTypes() {
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
    name: '',
    isActive: true,
    notes: ''
  }

  useEffect(() => {
    loadData()
  }, [currentPage])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await maintenanceTypeApi.getAll({
        pageNumber: currentPage,
        pageSize: pageSize
      })
      const paginatedData = extractPaginatedData(data)
      setItems(paginatedData.items)
      setTotalPages(paginatedData.totalPages)
    } catch (err) {
      console.error('Error loading maintenance types:', err)
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      if (editingItem) {
        await maintenanceTypeApi.update({ ...values, id: editingItem.id })
      } else {
        await maintenanceTypeApi.create(values)
      }
      await loadData()
      resetForm()
      setIsModalOpen(false)
      setEditingItem(null)
    } catch (err) {
      console.error('Error submitting form:', err)
      setError(err.message || 'حدث خطأ أثناء الحفظ')
      setSubmitting(false)
    }
  }

  async function handleEdit(item) {
    try {
      const response = await maintenanceTypeApi.getById(item.id)
      const fullItem = extractObjectFromResponse(response)

      // Ensure we have the required fields with proper defaults
      let isActiveValue = true
      if (fullItem?.isActive !== undefined) {
        isActiveValue = fullItem.isActive
      } else if (item.isActive !== undefined) {
        isActiveValue = item.isActive
      }

      const itemToEdit = {
        id: fullItem?.id || item.id,
        name: fullItem?.name || item.name || '',
        isActive: isActiveValue,
        notes: fullItem?.notes || item.notes || ''
      }

      setEditingItem(itemToEdit)
      setIsModalOpen(true)
    } catch (err) {
      console.error('Error loading item for edit:', err)
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
    }
  }

  async function handleDelete(id) {
    if (globalThis.confirm('هل أنت متأكد من حذف هذا العنصر؟')) {
      try {
        setError(null)
        await maintenanceTypeApi.delete(id)
        await loadData()
      } catch (err) {
        console.error('Error deleting item:', err)
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

  return (
    <div className="page">
      <h2>أنواع الصيانة — Maintenance Types</h2>

      {error && (
        <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title">
          <span>قائمة أنواع الصيانة</span>
          <button onClick={handleAdd} disabled={loading} className="primary">
            + إضافة نوع صيانة جديد
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
                  <th>الاسم</th>
                  <th>الحالة</th>
                  <th>ملاحظات</th>
                  <th>الإجراءات</th>
                </tr>
              </thead>
              <tbody>
                {items.length === 0 ? (
                  <tr>
                    <td colSpan="4" style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
                      لا توجد بيانات
                    </td>
                  </tr>
                ) : (
                  items.map((it) => (
                    <tr key={it.id}>
                      <td>{it.name}</td>
                      <td>{it.isActive ? 'نشط' : 'غير نشط'}</td>
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
        title={editingItem ? 'تعديل نوع صيانة' : 'إضافة نوع صيانة جديد'}
      >
        <Formik
          initialValues={editingItem || initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
          enableReinitialize
        >
          {({ isSubmitting, values }) => (
            <Form>
              <div className="form-grid">
                <FormField name="name" label="الاسم / Name" />
                <FormField name="isActive" type="checkbox" label={values.isActive ? 'نشط' : 'غير نشط'} />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr', marginTop: '1rem' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" rows="3" />
              </div>

              <div className="actions">
                <button type="button" onClick={handleClose} className="secondary">
                  إلغاء
                </button>
                <button type="submit" disabled={isSubmitting} className="primary">
                  {editingItem ? 'حفظ التعديلات' : 'إضافة نوع صيانة'}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}
