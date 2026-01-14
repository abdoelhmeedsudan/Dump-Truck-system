import React, { useState } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import '../pages/styles.css'

const validationSchema = Yup.object({
  shift: Yup.string().required('مطلوب'),
  dumpTruck: Yup.string().required('مطلوب'),
  driver: Yup.string().required('مطلوب'),
  tripsCount: Yup.number().required('مطلوب').min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  tripUnitPrice: Yup.number().required('مطلوب').min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  notes: Yup.string()
})

export default function ShiftTruckEntries() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)

  const initialValues = {
    shift: '',
    dumpTruck: '',
    driver: '',
    tripsCount: 0,
    tripUnitPrice: '',
    notes: ''
  }

  function handleSubmit(values, { resetForm }) {
    if (editingItem) {
      setItems(items.map(item => item.id === editingItem.id ? { ...values, id: editingItem.id } : item))
      setEditingItem(null)
    } else {
      setItems([...items, { ...values, id: Date.now() }])
    }
    resetForm()
    setIsModalOpen(false)
  }

  function handleEdit(item) {
    setEditingItem(item)
    setIsModalOpen(true)
  }

  function handleDelete(id) {
    if (globalThis.confirm('هل أنت متأكد من حذف هذا العنصر؟')) {
      setItems(items.filter(item => item.id !== id))
    }
  }

  function handleAdd() {
    setEditingItem(null)
    setIsModalOpen(true)
  }

  function handleClose() {
    setIsModalOpen(false)
    setEditingItem(null)
  }

  return (
    <div className="page">
      <h2>تشغيل القلابات — Shift Truck Entries</h2>

      <div className="table-section">
        <div className="table-section-title" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span>قائمة التشغيلات</span>
          <button onClick={handleAdd}>إضافة تشغيل جديد</button>
        </div>
        <table className="table">
          <thead>
            <tr>
              <th>الوردية</th>
              <th>القلاب</th>
              <th>السائق</th>
              <th>الرحلات</th>
              <th>سعر الرحلة</th>
              <th>ملاحظات</th>
              <th>الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr>
                <td colSpan="7" style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
                  لا توجد بيانات
                </td>
              </tr>
            ) : (
              items.map((it) => (
                <tr key={it.id}>
                  <td>{it.shift}</td>
                  <td>{it.dumpTruck}</td>
                  <td>{it.driver}</td>
                  <td>{it.tripsCount}</td>
                  <td>{it.tripUnitPrice}</td>
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
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={handleClose}
        title={editingItem ? 'تعديل تشغيل' : 'إضافة تشغيل جديد'}
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
                <FormField name="shift" label="الوردية / Shift" />
                <FormField name="dumpTruck" label="القلاب / Dump Truck" />
                <FormField name="driver" label="السائق / Driver" />
                <FormField name="tripsCount" type="number" label="عدد الرحلات / Trips Count" />
                <FormField name="tripUnitPrice" type="number" label="سعر الرحلة / Trip Unit Price" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" />
              </div>

              <div className="actions">
                <button type="submit" disabled={isSubmitting}>
                  {editingItem ? 'حفظ التعديلات' : 'إضافة تشغيل'}
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
