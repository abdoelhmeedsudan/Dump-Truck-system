import React from 'react'
import { useField } from 'formik'

export default function FormField({ label, name, type = 'text', ...props }) {
  const [field, meta] = useField({
    name,
    type: type === 'checkbox' ? 'checkbox' : undefined
  })

  if (type === 'checkbox') {
    return (
      <div className="form-field">
        <div className="form-control" style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', paddingTop: '0.5rem' }}>
          <input
            {...field}
            {...props}
            type="checkbox"
            id={name}
            checked={field.value}
            className={meta.touched && meta.error ? 'error' : ''}
          />
          <label className="form-label" htmlFor={name} style={{ margin: 0, cursor: 'pointer' }}>
            {label}
          </label>
        </div>
        {meta.touched && meta.error && (
          <div className="form-error">{meta.error}</div>
        )}
      </div>
    )
  }

  return (
    <div className="form-field">
      <label className="form-label" htmlFor={name}>
        {label}
      </label>
      <div className="form-control">
        {type === 'textarea' ? (
          <textarea
            {...field}
            {...props}
            id={name}
            className={meta.touched && meta.error ? 'error' : ''}
          />
        ) : type === 'select' ? (
          <select
            {...field}
            {...props}
            id={name}
            className={meta.touched && meta.error ? 'error' : ''}
          >
            {props.children}
          </select>
        ) : (
          <input
            {...field}
            {...props}
            type={type}
            id={name}
            className={meta.touched && meta.error ? 'error' : ''}
          />
        )}
        {meta.touched && meta.error && (
          <div className="form-error">{meta.error}</div>
        )}
      </div>
    </div>
  )
}
