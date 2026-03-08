import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import Pagination from '../components/Pagination'
import { revenueRateApi, extractPaginatedData, extractObjectFromResponse } from '../services/apiService'
import { useSites } from '../hooks/useLookups'
import '../pages/styles.css'

const validationSchema = Yup.object({
  siteId: Yup.string().required('مطلوب'),
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
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const { sites } = useSites()

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [pageSize] = useState(10)

  const initialValues = {
    siteId: '',
    effectiveFrom: '',
    ratePerTrip: '',
    currencyCode: 'SDG',
    exchangeRateToSAR: '',
    notes: ''
  }

  useEffect(() => {
    loadData()
  }, [currentPage])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await revenueRateApi.getAll({
        pageNumber: currentPage,
        pageSize: pageSize
      })
      const paginatedData = extractPaginatedData(data)
      setItems(paginatedData.items)
      setTotalPages(paginatedData.totalPages)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      console.error('Error loading revenue rates:', err)
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      const submitData = {
        ...values,
        ratePerTrip: Number.parseFloat(values.ratePerTrip) || 0,
        exchangeRateToSAR: Number.parseFloat(values.exchangeRateToSAR) || 0
      }
      if (editingItem) {
        await revenueRateApi.update({ ...submitData, id: editingItem.id })
      } else {
        await revenueRateApi.create(submitData)
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
      const response = await revenueRateApi.getById(item.id)
      const fullItem = extractObjectFromResponse(response)

      const itemToEdit = {
        id: fullItem?.id || item.id,
        siteId: fullItem?.siteId || item.siteId || '',
        effectiveFrom: fullItem?.effectiveFrom || item.effectiveFrom || '',
        ratePerTrip: fullItem?.ratePerTrip !== undefined ? fullItem.ratePerTrip : (item.ratePerTrip !== undefined ? item.ratePerTrip : ''),
        currencyCode: fullItem?.currencyCode || item.currencyCode || 'SDG',
        exchangeRateToSAR: fullItem?.exchangeRateToSAR !== undefined ? fullItem.exchangeRateToSAR : (item.exchangeRateToSAR !== undefined ? item.exchangeRateToSAR : ''),
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
        await revenueRateApi.delete(id)
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
      <h2>أسعار النقلات — Revenue Rates</h2>

      {error && (
        <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title">
          <span>قائمة الأسعار</span>
          <button onClick={handleAdd} disabled={loading} className="primary">
            + إضافة سعر جديد
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
                      <td>{getSiteName(it.siteId)}</td>
                      <td>{it.effectiveFrom}</td>
                      <td>{it.ratePerTrip}</td>
                      <td>{it.currencyCode}</td>
                      <td>{it.exchangeRateToSAR}</td>
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
                <FormField name="siteId" type="select" label="الموقع / Site">
                  <option value="">اختر الموقع</option>
                  {sites.map(site => (
                    <option key={site.id} value={site.id}>{site.name}</option>
                  ))}
                </FormField>
                <FormField name="effectiveFrom" type="date" label="سريان السعر من / Effective From" />
                <FormField name="ratePerTrip" type="number" label="سعر الرحلة / Rate Per Trip" />
                <FormField name="currencyCode" label="رمز العملة / Currency Code" />
                <FormField name="exchangeRateToSAR" type="number" label="سعر الصرف (إلى SAR) / Exchange Rate To SAR" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr', marginTop: '1rem' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" rows="3" />
              </div>

              <div className="actions">
                <button type="button" onClick={handleClose} className="secondary">
                  إلغاء
                </button>
                <button type="submit" disabled={isSubmitting} className="primary">
                  {editingItem ? 'حفظ التعديلات' : 'إضافة سعر'}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}
