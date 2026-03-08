import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import Pagination from '../components/Pagination'
import { shiftTruckEntryApi, shiftApi, extractPaginatedData, extractObjectFromResponse, extractArrayFromResponse } from '../services/apiService'
import { useSites, useDumpTrucks, useDrivers } from '../hooks/useLookups'
import '../pages/styles.css'

const validationSchema = Yup.object({
  shiftId: Yup.string().required('مطلوب'),
  dumpTruckId: Yup.string().required('مطلوب'),
  driverId: Yup.string().required('مطلوب'),
  tripsCount: Yup.number().required('مطلوب').min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  tripUnitPrice: Yup.number().required('مطلوب').min(0, 'يجب أن يكون أكبر من أو يساوي صفر'),
  notes: Yup.string()
})

export default function ShiftTruckEntries() {
  const [items, setItems] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [editingItem, setEditingItem] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [shifts, setShifts] = useState([])
  const { sites } = useSites()
  const { dumpTrucks } = useDumpTrucks()
  const { drivers } = useDrivers()

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [pageSize] = useState(10)

  const initialValues = {
    shiftId: '',
    dumpTruckId: '',
    driverId: '',
    tripsCount: 0,
    tripUnitPrice: '',
    notes: ''
  }

  useEffect(() => {
    loadData()
    loadShifts()
  }, [currentPage])

  async function loadShifts() {
    try {
      const data = await shiftApi.getAll()
      setShifts(extractArrayFromResponse(data))
    } catch (err) {
      console.error('Error loading shifts:', err)
    }
  }

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await shiftTruckEntryApi.getAll({
        pageNumber: currentPage,
        pageSize: pageSize
      })
      const paginatedData = extractPaginatedData(data)
      setItems(paginatedData.items)
      setTotalPages(paginatedData.totalPages)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      console.error('Error loading shift truck entries:', err)
    } finally {
      setLoading(false)
    }
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      const submitData = {
        ...values,
        tripsCount: Number.parseInt(values.tripsCount) || 0,
        tripUnitPrice: Number.parseFloat(values.tripUnitPrice) || 0
      }
      if (editingItem) {
        await shiftTruckEntryApi.update({ ...submitData, id: editingItem.id })
      } else {
        await shiftTruckEntryApi.create(submitData)
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
      const response = await shiftTruckEntryApi.getById(item.id)
      const fullItem = extractObjectFromResponse(response)

      const itemToEdit = {
        id: fullItem?.id || item.id,
        shiftId: fullItem?.shiftId || item.shiftId || '',
        dumpTruckId: fullItem?.dumpTruckId || item.dumpTruckId || '',
        driverId: fullItem?.driverId || item.driverId || '',
        tripsCount: fullItem?.tripsCount !== undefined ? fullItem.tripsCount : (item.tripsCount !== undefined ? item.tripsCount : 0),
        tripUnitPrice: fullItem?.tripUnitPrice !== undefined ? fullItem.tripUnitPrice : (item.tripUnitPrice !== undefined ? item.tripUnitPrice : ''),
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
        await shiftTruckEntryApi.delete(id)
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

  function getShiftName(shiftId) {
    const shift = shifts.find(s => s.id === shiftId)
    if (!shift) return shiftId
    const site = sites.find(s => s.id === shift.siteId)
    return site ? `${shift.shiftDate} - ${site.name}` : shift.shiftDate
  }

  function getDumpTruckName(dumpTruckId) {
    const truck = dumpTrucks.find(t => t.id === dumpTruckId)
    return truck ? truck.truckNumber : dumpTruckId
  }

  function getDriverName(driverId) {
    const driver = drivers.find(d => d.id === driverId)
    return driver ? driver.fullName : driverId
  }

  return (
    <div className="page">
      <h2>تشغيل القلابات — Shift Truck Entries</h2>

      {error && (
        <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title">
          <span>قائمة التشغيلات</span>
          <button onClick={handleAdd} disabled={loading} className="primary">
            + إضافة تشغيل جديد
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
                      <td>{getShiftName(it.shiftId)}</td>
                      <td>{getDumpTruckName(it.dumpTruckId)}</td>
                      <td>{getDriverName(it.driverId)}</td>
                      <td>{it.tripsCount}</td>
                      <td>{it.tripUnitPrice}</td>
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
                <FormField name="shiftId" type="select" label="الوردية / Shift">
                  <option value="">اختر الوردية</option>
                  {shifts.map(shift => {
                    const site = sites.find(s => s.id === shift.siteId)
                    const displayName = site ? `${shift.shiftDate} - ${site.name}` : shift.shiftDate
                    return (
                      <option key={shift.id} value={shift.id}>{displayName}</option>
                    )
                  })}
                </FormField>
                <FormField name="dumpTruckId" type="select" label="القلاب / Dump Truck">
                  <option value="">اختر القلاب</option>
                  {dumpTrucks.map(truck => (
                    <option key={truck.id} value={truck.id}>{truck.truckNumber}</option>
                  ))}
                </FormField>
                <FormField name="driverId" type="select" label="السائق / Driver">
                  <option value="">اختر السائق</option>
                  {drivers.filter(d => d.isActive).map(driver => (
                    <option key={driver.id} value={driver.id}>{driver.fullName}</option>
                  ))}
                </FormField>
                <FormField name="tripsCount" type="number" label="عدد الرحلات / Trips Count" />
                <FormField name="tripUnitPrice" type="number" label="سعر الرحلة / Trip Unit Price" />
              </div>

              <div className="form-grid" style={{ gridTemplateColumns: '1fr', marginTop: '1rem' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" rows="3" />
              </div>

              <div className="actions">
                <button type="button" onClick={handleClose} className="secondary">
                  إلغاء
                </button>
                <button type="submit" disabled={isSubmitting} className="primary">
                  {editingItem ? 'حفظ التعديلات' : 'إضافة تشغيل'}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}

