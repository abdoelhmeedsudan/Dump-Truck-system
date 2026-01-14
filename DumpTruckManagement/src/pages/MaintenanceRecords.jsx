import React, { useState } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import '../pages/styles.css'

const validationSchema = Yup.object({
  maintenanceDate: Yup.string().required('مطلوب'),
  dumpTruck: Yup.string().required('مطلوب'),
  maintenanceType: Yup.string().required('مطلوب'),
  partsCost: Yup.number().min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  laborCost: Yup.number().min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  totalCost: Yup.number(),
  doneBy: Yup.string().required('مطلوب'),
  notes: Yup.string()
})

export default function MaintenanceRecords() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)

  const initialValues = {
    maintenanceDate: '',
    dumpTruck: '',
    maintenanceType: '',
    partsCost: '',
    laborCost: '',
    totalCost: '',
    doneBy: '',
    notes: ''
  }

  function calculateTotal(partsCost, laborCost, setFieldValue) {
    const parts = Number.parseFloat(partsCost) || 0
    const labor = Number.parseFloat(laborCost) || 0
    const total = parts + labor
    setFieldValue('totalCost', total.toFixed(2))
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
      <h2>سجلات الصيانة — Maintenance Records</h2>

      <div className="table-section">
        <div className="table-section-title" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span>قائمة سجلات الصيانة</span>
          <button onClick={handleAdd}>إضافة سجل صيانة جديد</button>
        </div>
        <table className="table">
          <thead>
            <tr>
              <th>التاريخ</th>
              <th>القلاب</th>
              <th>النوع</th>
              <th>قطع الغيار</th>
              <th>العمال</th>
              <th>الإجمالي</th>
              <th>منفذ العمل</th>
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
                  <td>{it.maintenanceDate}</td>
                  <td>{it.dumpTruck}</td>
                  <td>{it.maintenanceType}</td>
                  <td>{it.partsCost}</td>
                  <td>{it.laborCost}</td>
                  <td>{it.totalCost}</td>
                  <td>{it.doneBy}</td>
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
        title={editingItem ? 'تعديل سجل صيانة' : 'إضافة سجل صيانة جديد'}
      >
        <Formik
          initialValues={editingItem || initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
          enableReinitialize
        >
          {({ isSubmitting, values, setFieldValue }) => (
            <Form>
              <div className="form-grid">
                <FormField name="maintenanceDate" type="date" label="تاريخ الصيانة / Maintenance Date" />
                <FormField name="dumpTruck" label="القلاب / Dump Truck" />
                <FormField name="maintenanceType" label="نوع الصيانة / Maintenance Type" />
                <FormField 
                  name="partsCost" 
                  type="number" 
                  label="قطع الغيار / Parts Cost"
                  onBlur={() => calculateTotal(values.partsCost, values.laborCost, setFieldValue)}
                />
                <FormField 
                  name="laborCost" 
                  type="number" 
                  label="تكلفة العمال / Labor Cost"
                  onBlur={() => calculateTotal(values.partsCost, values.laborCost, setFieldValue)}
                />
                <FormField name="totalCost" type="number" label="التكلفة الإجمالية / Total Cost" readOnly style={{ backgroundColor: '#f8fafc' }} />
                <FormField name="doneBy" label="منفذ العمل / Done By" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" />
              </div>

              <div className="actions">
                <button type="submit" disabled={isSubmitting}>
                  {editingItem ? 'حفظ التعديلات' : 'إضافة سجل صيانة'}
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
