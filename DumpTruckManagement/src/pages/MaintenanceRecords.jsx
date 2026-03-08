import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import Modal from '../components/Modal'
import Pagination from '../components/Pagination'
import { maintenanceRecordApi, extractPaginatedData, extractObjectFromResponse } from '../services/apiService'
import { useDumpTrucks, useMaintenanceTypes } from '../hooks/useLookups'
import '../pages/styles.css'

const validationSchema = Yup.object({
  maintenanceDate: Yup.string().required('مطلوب'),
  dumpTruckId: Yup.string().required('مطلوب'),
  maintenanceTypeId: Yup.string().required('مطلوب'),
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
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const { dumpTrucks } = useDumpTrucks()
  const { maintenanceTypes } = useMaintenanceTypes()

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [pageSize] = useState(10)

  const initialValues = {
    maintenanceDate: '',
    dumpTruckId: '',
    maintenanceTypeId: '',
    partsCost: '',
    laborCost: '',
    totalCost: '',
    doneBy: '',
    notes: ''
  }

  useEffect(() => {
    loadData()
  }, [currentPage])

  async function loadData() {
    try {
      setLoading(true)
      setError(null)
      const data = await maintenanceRecordApi.getAll({
        pageNumber: currentPage,
        pageSize: pageSize
      })
      const paginatedData = extractPaginatedData(data)
      setItems(paginatedData.items)
      setTotalPages(paginatedData.totalPages)
    } catch (err) {
      setError(err.message || 'حدث خطأ أثناء تحميل البيانات')
      console.error('Error loading maintenance records:', err)
    } finally {
      setLoading(false)
    }
  }

  function calculateTotal(partsCost, laborCost, setFieldValue) {
    const parts = Number.parseFloat(partsCost) || 0
    const labor = Number.parseFloat(laborCost) || 0
    const total = parts + labor
    setFieldValue('totalCost', total.toFixed(2))
  }

  async function handleSubmit(values, { resetForm, setSubmitting }) {
    try {
      setError(null)
      const submitData = {
        ...values,
        partsCost: Number.parseFloat(values.partsCost) || 0,
        laborCost: Number.parseFloat(values.laborCost) || 0,
        totalCost: Number.parseFloat(values.totalCost) || 0
      }
      if (editingItem) {
        await maintenanceRecordApi.update({ ...submitData, id: editingItem.id })
      } else {
        await maintenanceRecordApi.create(submitData)
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
      const response = await maintenanceRecordApi.getById(item.id)
      const fullItem = extractObjectFromResponse(response)

      const itemToEdit = {
        id: fullItem?.id || item.id,
        maintenanceDate: fullItem?.maintenanceDate || item.maintenanceDate || '',
        dumpTruckId: fullItem?.dumpTruckId || item.dumpTruckId || '',
        maintenanceTypeId: fullItem?.maintenanceTypeId || item.maintenanceTypeId || '',
        partsCost: fullItem?.partsCost !== undefined ? fullItem.partsCost : (item.partsCost !== undefined ? item.partsCost : ''),
        laborCost: fullItem?.laborCost !== undefined ? fullItem.laborCost : (item.laborCost !== undefined ? item.laborCost : ''),
        totalCost: fullItem?.totalCost !== undefined ? fullItem.totalCost : (item.totalCost !== undefined ? item.totalCost : ''),
        doneBy: fullItem?.doneBy || item.doneBy || '',
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
        await maintenanceRecordApi.delete(id)
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

  function getDumpTruckName(dumpTruckId) {
    const truck = dumpTrucks.find(t => t.id === dumpTruckId)
    return truck ? truck.truckNumber : dumpTruckId
  }

  function getMaintenanceTypeName(maintenanceTypeId) {
    const maintenanceType = maintenanceTypes.find(m => m.id === maintenanceTypeId)
    return maintenanceType ? maintenanceType.name : maintenanceTypeId
  }

  return (
    <div className="page">
      <h2>سجلات الصيانة — Maintenance Records</h2>

      {error && (
        <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
          {error}
        </div>
      )}

      <div className="table-section">
        <div className="table-section-title">
          <span>قائمة سجلات الصيانة</span>
          <button onClick={handleAdd} disabled={loading} className="primary">
            + إضافة سجل صيانة جديد
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
                      <td>{getDumpTruckName(it.dumpTruckId)}</td>
                      <td>{getMaintenanceTypeName(it.maintenanceTypeId)}</td>
                      <td>{it.partsCost}</td>
                      <td>{it.laborCost}</td>
                      <td>{it.totalCost}</td>
                      <td>{it.doneBy}</td>
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
                <FormField name="dumpTruckId" type="select" label="القلاب / Dump Truck">
                  <option value="">اختر القلاب</option>
                  {dumpTrucks.map(truck => (
                    <option key={truck.id} value={truck.id}>{truck.truckNumber}</option>
                  ))}
                </FormField>
                <FormField name="maintenanceTypeId" type="select" label="نوع الصيانة / Maintenance Type">
                  <option value="">اختر نوع الصيانة</option>
                  {maintenanceTypes.map(maintenanceType => (
                    <option key={maintenanceType.id} value={maintenanceType.id}>{maintenanceType.name}</option>
                  ))}
                </FormField>
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

              <div className="form-grid" style={{ gridTemplateColumns: '1fr', marginTop: '1rem' }}>
                <FormField name="notes" type="textarea" label="ملاحظات / Notes" rows="3" />
              </div>

              <div className="actions">
                <button type="button" onClick={handleClose} className="secondary">
                  إلغاء
                </button>
                <button type="submit" disabled={isSubmitting} className="primary">
                  {editingItem ? 'حفظ التعديلات' : 'إضافة سجل صيانة'}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </Modal>
    </div>
  )
}
