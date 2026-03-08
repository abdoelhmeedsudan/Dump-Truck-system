import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import Pagination from '../components/Pagination'
import { shiftExpenseApi, shiftTruckEntryApi, extractPaginatedData, extractArrayFromResponse, extractObjectFromResponse } from '../services/apiService'
import { useExpenseTypes } from '../hooks/useLookups'
import '../pages/styles.css'

const validationSchema = Yup.object({
  shiftTruckEntryId: Yup.string().required('مطلوب'),
  expenseTypeId: Yup.string().required('مطلوب'),
  amount: Yup.number().required('مطلوب').positive('يجب أن يكون المبلغ أكبر من صفر'),
  notes: Yup.string()
})

export default function ShiftExpenses() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [shiftTruckEntries, setShiftTruckEntries] = useState([])
  const { expenseTypes } = useExpenseTypes()

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [pageSize] = useState(10)

  const initialValues = {
    shiftTruckEntryId: '',
    expenseTypeId: '',
    amount: '',
    notes: ''
  }

  useEffect(() => {
    loadData()
    loadShiftTruckEntries()
  }, [currentPage])

  async function loadShiftTruckEntries() {
    try {
      const data = await shiftTruckEntryApi.getAll()
      setShiftTruckEntries(extractArrayFromResponse(data))
    } catch (err) {
      console.error('Error loading shift truck entries:', err)
    }
  }

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await shiftExpenseApi.getAll({
        pageNumber: currentPage,
        pageSize: pageSize
      })
      const paginatedData = extractPaginatedData(data)
      setItems(paginatedData.items)
      setTotalPages(paginatedData.totalPages)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      console.error('Error loading shift expenses:', err)
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      const submitData = {
        ...values,
        amount: Number.parseFloat(values.amount) || 0
      }
      if (editingItem) {
        await shiftExpenseApi.update({ ...submitData, id: editingItem.id })
      } else {
        await shiftExpenseApi.create(submitData)
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
      const response = await shiftExpenseApi.getById(item.id)
      const fullItem = extractObjectFromResponse(response)

      const itemToEdit = {
        id: fullItem?.id || item.id,
        shiftTruckEntryId: fullItem?.shiftTruckEntryId || item.shiftTruckEntryId || '',
        expenseTypeId: fullItem?.expenseTypeId || item.expenseTypeId || '',
        amount: fullItem?.amount !== undefined ? fullItem.amount : (item.amount !== undefined ? item.amount : ''),
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
        await shiftExpenseApi.delete(id)
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

  function getShiftTruckEntryName(entryId) {
    const entry = shiftTruckEntries.find(e => e.id === entryId)
    return entry ? `Entry #${entry.id}` : entryId
  }

  function getExpenseTypeName(expenseTypeId) {
    const expenseType = expenseTypes.find(e => e.id === expenseTypeId)
    return expenseType ? expenseType.name : expenseTypeId
  }

  return (
    <div className="page">
      <h2>المصاريف اليومية — Shift Expenses</h2>

      {error && (
        <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title">
          <span>قائمة المصاريف</span>
          <button onClick={handleAdd} disabled={loading} className="primary">
            + إضافة مصروف جديد
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
                      <td>{getShiftTruckEntryName(it.shiftTruckEntryId)}</td>
                      <td>{getExpenseTypeName(it.expenseTypeId)}</td>
                      <td>{it.amount}</td>
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
                <FormField name="shiftTruckEntryId" type="select" label="تشغيل القلاب / Shift Truck Entry">
                  <option value="">اختر التشغيل</option>
                  {shiftTruckEntries.map(entry => (
                    <option key={entry.id} value={entry.id}>Entry #{entry.id}</option>
                  ))}
                </FormField>
                <FormField name="expenseTypeId" type="select" label="نوع المصروف / Expense Type">
                  <option value="">اختر نوع المصروف</option>
                  {expenseTypes.map(expenseType => (
                    <option key={expenseType.id} value={expenseType.id}>{expenseType.name}</option>
                  ))}
                </FormField>
                <FormField name="amount" type="number" label="المبلغ / Amount" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr', marginTop: '1rem' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" rows="3" />
              </div>

              <div className="actions">
                <button type="button" onClick={handleClose} className="secondary">
                  إلغاء
                </button>
                <button type="submit" disabled={isSubmitting} className="primary">
                  {editingItem ? 'حفظ التعديلات' : 'إضافة مصروف'}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}
