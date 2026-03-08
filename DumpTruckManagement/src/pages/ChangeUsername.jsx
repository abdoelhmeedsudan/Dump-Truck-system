import React, { useState, useEffect } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import { userApi, authApi } from '../services/apiService'
import '../pages/styles.css'

const validationSchema = Yup.object({
    newUsername: Yup.string().required('مطلوب')
})

export default function ChangeUsername() {
    const [error, setError] = useState(null)
    const [success, setSuccess] = useState(null)
    const [currentUser, setCurrentUser] = useState({ username: '' })

    useEffect(() => {
        const user = authApi.getCurrentUser()
        if (user) {
            setCurrentUser(user)
        }
    }, [])

    const initialValues = {
        newUsername: currentUser.username || ''
    }

    async function handleSubmit(values, { setSubmitting }) {
        setError(null)
        setSuccess(null)
        try {
            await userApi.changeUsername({
                newUsername: values.newUsername
            })
            setSuccess('تم تغيير اسم المستخدم بنجاح. سيتم تحديث الاسم عند تسجيل الدخول القادم.')
            // Update local storage manually just for immediate UI reflect if needed, 
            // but usually re-login is best. For now we just show success.
        } catch (err) {
            setError(err.message || 'حدث خطأ أثناء تغيير الاسم')
        } finally {
            setSubmitting(false)
        }
    }

    return (
        <div className="page" style={{ maxWidth: 500, margin: '0 auto' }}>
            <h2>تغيير اسم المستخدم</h2>

            {error && (
                <div className="badge badge-error" style={{ display: 'block', marginBottom: '1rem', padding: '1rem' }}>
                    {error}
                </div>
            )}

            {success && (
                <div className="badge" style={{ display: 'block', marginBottom: '1rem', padding: '1rem', background: 'var(--success-bg)', color: 'var(--success-text)', border: '1px solid var(--success-border)' }}>
                    {success}
                </div>
            )}

            <div className="form-card" style={{ marginBottom: '2rem' }}>
                <p style={{ color: 'var(--muted)', marginBottom: '1rem' }}>
                    اسم المستخدم الحالي: <strong>{currentUser.username || '...'}</strong>
                </p>
            </div>

            <Formik
                initialValues={initialValues}
                validationSchema={validationSchema}
                onSubmit={handleSubmit}
                enableReinitialize
            >
                {({ isSubmitting }) => (
                    <Form className="form-card">
                        <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                            <FormField name="newUsername" type="text" label="اسم المستخدم الجديد" />
                        </div>

                        <div className="actions" style={{ marginTop: '2rem' }}>
                            <button type="submit" className="primary" disabled={isSubmitting}>
                                {isSubmitting ? 'جاري الحفظ...' : 'حفظ التغييرات'}
                            </button>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    )
}
