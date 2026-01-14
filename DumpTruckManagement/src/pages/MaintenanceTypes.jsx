import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import { maintenanceTypeApi, extractArrayFromResponse } from '../services/apiService'
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

  const initialValues = {
    name: '',
    isActive: true,
    notes: ''
  }

  useEffect(() => {
    loadData()
  }, [])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      console.log('[MaintenanceTypes] Loading data...')
      const data = await maintenanceTypeApi.getAll()
      console.log('[MaintenanceTypes] Raw API response:', data)
      console.log('[MaintenanceTypes] Data type:', typeof data)
      console.log('[MaintenanceTypes] Is array?', Array.isArray(data))
      const itemsArray = extractArrayFromResponse(data)
      console.log('[MaintenanceTypes] Extracted array:', itemsArray)
      console.log('[MaintenanceTypes] Items array type:', typeof itemsArray)
      console.log('[MaintenanceTypes] Items array is array?', Array.isArray(itemsArray))
      // Ensure itemsArray is always an array
      const finalItems = Array.isArray(itemsArray) ? itemsArray : []
      console.log('[MaintenanceTypes] Final items to set:', finalItems)
      setItems(finalItems)
    } catch (err) {
      console.error('[MaintenanceTypes] Error loading maintenance types:', err)
      console.error('[MaintenanceTypes] Error details:', {
        message: err.message,
        stack: err.stack,
        name: err.name
      })
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      setItems([]) // Ensure items is always an array
    } finally {
      setLoading(false)
      console.log('[MaintenanceTypes] Loading completed')
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      console.log('[MaintenanceTypes] Submitting form:', values)
      console.log('[MaintenanceTypes] Editing item:', editingItem)
      setError(null)
      if (editingItem) {
        console.log('[MaintenanceTypes] Updating item with id:', editingItem.id)
        await maintenanceTypeApi.update({ ...values, id: editingItem.id })
        console.log('[MaintenanceTypes] Update successful')
      } else {
        console.log('[MaintenanceTypes] Creating new item')
        await maintenanceTypeApi.create(values)
        console.log('[MaintenanceTypes] Create successful')
      }
      await loadData()
      resetForm()
      setIsModalOpen(false)
      setEditingItem(null)
    } catch (err) {
      console.error('[MaintenanceTypes] Error submitting form:', err)
      console.error('[MaintenanceTypes] Submit error details:', {
        message: err.message,
        stack: err.stack,
        values: values
      })
      setError(err.message || 'حدث خطأ أثناء الحفظ')
      setSubmitting(false)
    }
  }

  async function handleEdit(item) {
    try {
      console.log('[MaintenanceTypes] Editing item:', item)
      const fullItem = await maintenanceTypeApi.getById(item.id)
      console.log('[MaintenanceTypes] Full item loaded:', fullItem)
      setEditingItem(fullItem)
      setIsModalOpen(true)
    } catch (err) {
      console.error('[MaintenanceTypes] Error loading item for edit:', err)
      console.error('[MaintenanceTypes] Edit error details:', {
        message: err.message,
        stack: err.stack,
        itemId: item?.id
      })
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
    }
  }

  async function handleDelete(id) {
    if (globalThis.confirm('هل أنت متأكد من حذف هذا العنصر؟')) {
      try {
        console.log('[MaintenanceTypes] Deleting item with id:', id)
        setError(null)
        await maintenanceTypeApi.delete(id)
        console.log('[MaintenanceTypes] Delete successful')
        await loadData()
      } catch (err) {
        console.error('[MaintenanceTypes] Error deleting item:', err)
        console.error('[MaintenanceTypes] Delete error details:', {
          message: err.message,
          stack: err.stack,
          itemId: id
        })
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

  return (
    <div className="page">
      <h2>أنواع الصيانة — Maintenance Types</h2>

      {error && (
        <div style={{ 
          padding: '1rem', 
          background: 'rgba(239, 68, 68, 0.1)', 
          color: 'var(--error)', 
          borderRadius: '8px', 
          marginBottom: '1rem',
          border: '1px solid var(--error)'
        }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span>قائمة أنواع الصيانة</span>
          <button onClick={handleAdd} disabled={loading}>إضافة نوع صيانة جديد</button>
        </div>
        {loading ? (
          <div style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
            جاري التحميل...
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
              {(() => {
                console.log('[MaintenanceTypes] Rendering tbody, items:', items)
                console.log('[MaintenanceTypes] Items is array?', Array.isArray(items))
                console.log('[MaintenanceTypes] Items length:', items?.length)
                return !Array.isArray(items) || items.length === 0 ? (
                  <tr>
                    <td colSpan="4" style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
                      لا توجد بيانات
                    </td>
                  </tr>
                ) : (
                  items.map((it) => {
                    console.log('[MaintenanceTypes] Mapping item:', it)
                    return (
                  <tr key={it.id}>
                    <td>{it.name}</td>
                    <td>{it.isActive ? 'نشط' : 'غير نشط'}</td>
                    <td>{it.notes}</td>
                    <td>
                      <div style={{ display: 'flex', gap: '0.5rem' }}>
                        <button onClick={() => handleEdit(it)} style={{ padding: '0.5rem 1rem', fontSize: '0.85rem' }}>تعديل</button>
                        <button onClick={() => handleDelete(it.id)} style={{ padding: '0.5rem 1rem', fontSize: '0.85rem', background: 'var(--error)' }}>حذف</button>
                      </div>
                    </td>
                  </tr>
                    )
                  })
                )
              })()}
            </tbody>
          </table>
        )}
      </div>

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

              <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" />
              </div>

              <div className="actions">
                <button type="submit" disabled={isSubmitting}>
                  {editingItem ? 'حفظ التعديلات' : 'إضافة نوع صيانة'}
                </button>
                <button type="button" onClick={handleClose} style={{ background: 'var(--muted)' }}>
                  إلغاء
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}
