import React, { useState } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import '../pages/styles.css'

const validationSchema = Yup.object({
  shiftTruckEntry: Yup.string().required('مطلوب'),
  expenseType: Yup.string().required('مطلوب'),
  amount: Yup.number().required('مطلوب').positive('يجب أن يكون المبلغ أكبر من صفر'),
  notes: Yup.string()
})

export default function ShiftExpenses() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)

  const initialValues = {
    shiftTruckEntry: '',
    expenseType: '',
    amount: '',
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
      <h2>المصاريف اليومية — Shift Expenses</h2>

      <div className="table-section">
        <div className="table-section-title" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span>قائمة المصاريف</span>
          <button onClick={handleAdd}>إضافة مصروف جديد</button>
        </div>
        <table className="table">
          <thead>
            <tr>
              <th>التشغيل</th>
              <th>النوع</th>
              <th>المبلغ</th>
              <th>ملاحظات</th>
              <th>الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr>
                <td colSpan="5" style={{ textAlign: 'center', padding: '3rem', color: 'var(--muted)' }}>
                  لا توجد بيانات
                </td>
              </tr>
            ) : (
              items.map((it) => (
                <tr key={it.id}>
                  <td>{it.shiftTruckEntry}</td>
                  <td>{it.expenseType}</td>
                  <td>{it.amount}</td>
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
        title={editingItem ? 'تعديل مصروف' : 'إضافة مصروف جديد'}
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
                <FormField name="shiftTruckEntry" label="تشغيل القلاب / Shift Truck Entry" />
                <FormField name="expenseType" label="نوع المصروف / Expense Type" />
                <FormField name="amount" type="number" label="المبلغ / Amount" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" />
              </div>

              <div className="actions">
                <button type="submit" disabled={isSubmitting}>
                  {editingItem ? 'حفظ التعديلات' : 'إضافة مصروف'}
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
