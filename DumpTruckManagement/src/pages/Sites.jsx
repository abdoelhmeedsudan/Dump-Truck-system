import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import { siteApi, extractArrayFromResponse } from '../services/apiService'
import '../pages/styles.css'

const validationSchema = Yup.object({
  name: Yup.string().required('مطلوب'),
  code: Yup.string().required('مطلوب'),
  notes: Yup.string()
})

export default function Sites() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  const initialValues = {
    name: '',
    code: '',
    notes: ''
  }

  useEffect(() => {
    loadData()
  }, [])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await siteApi.getAll()
      setItems(extractArrayFromResponse(data))
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      console.error('Error loading sites:', err)
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      if (editingItem) {
        await siteApi.update({ ...values, id: editingItem.id })
      } else {
        await siteApi.create(values)
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
      const fullItem = await siteApi.getById(item.id)
      setEditingItem(fullItem)
      setIsModalOpen(true)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
    }
  }

  async function handleDelete(id) {
    if (globalThis.confirm('هل أنت متأكد من حذف هذا العنصر؟')) {
      try {
        setError(null)
        await siteApi.delete(id)
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

  return (
    <div className="page">
      <h2>المواقع — Sites</h2>

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
          <span>قائمة المواقع</span>
          <button onClick={handleAdd} disabled={loading}>إضافة موقع جديد</button>
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
                <th>الكود</th>
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
                    <td>{it.code}</td>
                    <td>{it.notes}</td>
                    <td>
                      <div style={{ display: 'flex', gap: '0.5rem' }}>
                        <button onClick={() => handleEdit(it)} style={{ padding: '0.5rem 1rem', fontSize: '0.85rem' }}>تعديل</button>
                        <button onClick={() => handleDelete(it.id)} style={{ padding: '0.5rem 1rem', fontSize: '0.85rem', background: 'var(--error)' }}>حذف</button>
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        )}
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={handleClose}
        title={editingItem ? 'تعديل موقع' : 'إضافة موقع جديد'}
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
                <FormField name="name" label="الاسم / Name" />
                <FormField name="code" label="الكود / Code" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" />
              </div>

              <div className="actions">
                <button type="submit" disabled={isSubmitting}>
                  {editingItem ? 'حفظ التعديلات' : 'إضافة موقع'}
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
