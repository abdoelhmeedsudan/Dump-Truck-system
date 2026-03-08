import React, { useState } from 'react'
import { Formik, Form } from 'formik'
import * as Yup from 'yup'
import FormField from '../components/FormField'
import { userApi } from '../services/apiService'
import '../pages/styles.css'

const validationSchema = Yup.object({
    email: Yup.string().email('بريد إلكتروني غير صالح').required('مطلوب'),
    username: Yup.string().required('مطلوب'),
    password: Yup.string().required('مطلوب').min(6, 'يجب أن تكون كلمة المرور 6 أحرف على الأقل'),
    confirmPassword: Yup.string()
        .oneOf([Yup.ref('password'), null], 'كلمات المرور غير متطابقة')
        .required('مطلوب')
})

export default function AddUser() {
    const [error, setError] = useState(null)
    const [success, setSuccess] = useState(null)

    const initialValues = {
        email: '',
        username: '',
        password: '',
        confirmPassword: ''
    }

    async function handleSubmit(values, { resetForm, setSubmitting }) {
        setError(null)
        setSuccess(null)
        try {
            await userApi.register({
                email: values.email,
                username: values.username,
                password: values.password
            })
            setSuccess('تم إنشاء المستخدم بنجاح')
            resetForm()
        } catch (err) {
            setError(err.message || 'حدث خطأ أثناء إنشاء المستخدم')
        } finally {
            setSubmitting(false)
        }
    }

    return (
        <div className="page" style={{ maxWidth: 600, margin: '0 auto' }}>
            <h2>إضافة مستخدم جديد</h2>

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

            <Formik
                initialValues={initialValues}
                validationSchema={validationSchema}
                onSubmit={handleSubmit}
            >
                {({ isSubmitting }) => (
                    <Form className="form-card">
                        <div className="form-grid" style={{ gridTemplateColumns: '1fr' }}>
                            <FormField name="username" type="text" label="اسم المستخدم" />
                            <FormField name="email" type="email" label="البريد الإلكتروني" dir="ltr" />
                            <FormField name="password" type="password" label="كلمة المرور" dir="ltr" />
                            <FormField name="confirmPassword" type="password" label="تأكيد كلمة المرور" dir="ltr" />
                        </div>

                        <div className="actions" style={{ marginTop: '2rem' }}>
                            <button type="submit" className="primary" disabled={isSubmitting}>
                                {isSubmitting ? 'جاري الإضافة...' : 'إضافة المستخدم'}
                            </button>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    )
}
