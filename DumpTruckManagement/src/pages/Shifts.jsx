import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import Pagination from '../components/Pagination'
import { shiftApi, extractPaginatedData, extractObjectFromResponse } from '../services/apiService'
import { useSites } from '../hooks/useLookups'
import '../pages/styles.css'

const validationSchema = Yup.object({
  shiftDate: Yup.string().required('مطلوب'),
  siteId: Yup.string().required('مطلوب'),
  notes: Yup.string()
})

export default function Shifts() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const { sites } = useSites()

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [pageSize] = useState(10)

  const initialValues = {
    shiftDate: '',
    siteId: '',
    notes: ''
  }

  useEffect(() => {
    loadData()
  }, [currentPage])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await shiftApi.getAll({
        pageNumber: currentPage,
        pageSize: pageSize
      })
      const paginatedData = extractPaginatedData(data)
      setItems(paginatedData.items)
      setTotalPages(paginatedData.totalPages)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      console.error('Error loading shifts:', err)
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      if (editingItem) {
        await shiftApi.update({ ...values, id: editingItem.id })
      } else {
        await shiftApi.create(values)
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
      const response = await shiftApi.getById(item.id)
      const fullItem = extractObjectFromResponse(response)

      // Ensure we have the required fields with proper defaults
      const itemToEdit = {
        id: fullItem?.id || item.id,
        shiftDate: fullItem?.shiftDate || item.shiftDate || '',
        siteId: fullItem?.siteId || item.siteId || '',
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
        await shiftApi.delete(id)
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

  function getSiteName(siteId) {
    const site = sites.find(s => s.id === siteId)
    return site ? site.name : siteId
  }

  return (
    <div className="page">
      <h2>الورديات — Shifts</h2>

      {error && (
        <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title">
          <span>قائمة الورديات</span>
          <button onClick={handleAdd} disabled={loading} className="primary">
            + إضافة وردية جديدة
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
                  <th>التاريخ</th>
                  <th>الموقع</th>
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
                      <td>{it.shiftDate}</td>
                      <td>{getSiteName(it.siteId)}</td>
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
        title={editingItem ? 'تعديل وردية' : 'إضافة وردية جديدة'}
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
                <FormField name="shiftDate" type="date" label="تاريخ الوردية / Shift Date" />
                <FormField name="siteId" type="select" label="الموقع / Site">
                  <option value="">اختر الموقع</option>
                  {sites.map(site => (
                    <option key={site.id} value={site.id}>{site.name}</option>
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
                  {editingItem ? 'حفظ التعديلات' : 'إضافة وردية'}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}

