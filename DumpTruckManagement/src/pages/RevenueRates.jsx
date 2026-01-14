import React, { useState } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import '../pages/styles.css'

const validationSchema = Yup.object({
  site: Yup.string().required('مطلوب'),
  effectiveFrom: Yup.string().required('مطلوب'),
  ratePerTrip: Yup.number().required('مطلوب').min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  currencyCode: Yup.string().required('مطلوب'),
  exchangeRateToSAR: Yup.number().min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  notes: Yup.string()
})

export default function RevenueRates() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)

  const initialValues = {
    site: '',
    effectiveFrom: '',
    ratePerTrip: '',
    currencyCode: 'SAR',
    exchangeRateToSAR: '',
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
      <h2>أسعار النقلات — Revenue Rates</h2>

      <div className="table-section">
        <div className="table-section-title" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span>قائمة الأسعار</span>
          <button onClick={handleAdd}>إضافة سعر جديد</button>
        </div>
        <table className="table">
          <thead>
            <tr>
              <th>الموقع</th>
              <th>من تاريخ</th>
              <th>السعر</th>
              <th>العملة</th>
              <th>سعر الصرف</th>
              <th>الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr>
                <td colSpan="6" style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
                  لا توجد بيانات
                </td>
              </tr>
            ) : (
              items.map((it) => (
                <tr key={it.id}>
                  <td>{it.site}</td>
                  <td>{it.effectiveFrom}</td>
                  <td>{it.ratePerTrip}</td>
                  <td>{it.currencyCode}</td>
                  <td>{it.exchangeRateToSAR}</td>
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
        title={editingItem ? 'تعديل سعر' : 'إضافة سعر جديد'}
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
                <FormField name="site" label="الموقع / Site" />
                <FormField name="effectiveFrom" type="date" label="سريان السعر من / Effective From" />
                <FormField name="ratePerTrip" type="number" label="سعر الرحلة / Rate Per Trip" />
                <FormField name="currencyCode" label="رمز العملة / Currency Code" />
                <FormField name="exchangeRateToSAR" type="number" label="سعر الصرف (إلى SAR) / Exchange Rate To SAR" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" />
              </div>

              <div className="actions">
                <button type="submit" disabled={isSubmitting}>
                  {editingItem ? 'حفظ التعديلات' : 'إضافة سعر'}
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
