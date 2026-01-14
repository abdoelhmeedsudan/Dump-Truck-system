import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import { dumpTruckApi, extractArrayFromResponse } from '../services/apiService'
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
  }, [])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await dumpTruckApi.getAll()
      setItems(extractArrayFromResponse(data))
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
      const fullItem = await dumpTruckApi.getById(item.id)
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

  function getStatusLabel(status) {
    const option = statusOptions.find(opt => opt.value === status)
    return option ? option.label : status
  }

  return (
    <div className="page">
      <h2>القلابات — Dump Trucks</h2>

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
          <span>قائمة القلابات</span>
          <button onClick={handleAdd} disabled={loading}>إضافة قلاب جديد</button>
        </div>
        {loading ? (
          <div style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
            جاري التحميل...
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
                    <td>{getStatusLabel(it.status)}</td>
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

              <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" />
              </div>

              <div className="actions">
                <button type="submit" disabled={isSubmitting}>
                  {editingItem ? 'حفظ التعديلات' : 'إضافة قلاب'}
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
