// -----------------------------------------------------------------------
//  <copyright file="ViewModelExBase.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-12 10:03</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using GalaSoft.MvvmLight;

using Newtonsoft.Json;


namespace Liuliu.MouseClicker.Mvvm
{
    /// <summary>
    /// 实现了属性设置值(SetProperty)功能的ViewModelBase，可以使用表达式触发RaisePropertyChanged事件，免去硬编码属性名的编写
    /// </summary>
    public abstract class ViewModelExBase : ViewModelBase, IDataErrorInfo
    {
        protected void SetProperty<T>(ref T field, T value, Expression<Func<T>> expression)
        {
            if (field != null && field.Equals(value))
            {
                return;
            }
            if (!(expression.Body is MemberExpression body))
            {
                throw new ArgumentException(@"表达式类型必须为 MemberExpression", nameof(expression));
            }
            PropertyInfo property = body.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(@"表达式类型必须为 PropertyExpression", nameof(expression));
            }
            string propertyName = property.Name;
            field = value;
            RaisePropertyChanged(propertyName);
        }

        #region Implementation of IDataErrorInfo

        /// <summary>
        /// 获取具有给定名称的属性的错误信息。
        /// </summary>
        /// <returns>
        /// 该属性的错误信息。默认值为空字符串 ("")。
        /// </returns>
        /// <param name="columnName">要获取其错误信息的属性的名称。</param>
        [JsonIgnore]
        public virtual string this[string columnName]
        {
            get
            {
                ValidationContext context = new ValidationContext(this, null, null) { MemberName = columnName };
                List<ValidationResult> results = new List<ValidationResult>();
                PropertyInfo prop = GetType().GetProperty(columnName);
                if (prop == null)
                {
                    return string.Empty;
                }
                Validator.TryValidateProperty(prop.GetValue(this, null), context, results);
                if (results.Count <= 0)
                {
                    return string.Empty;
                }
                return string.Join(Environment.NewLine, results.Select(m => m.ErrorMessage).ToArray());
            }
        }

        /// <summary>
        /// 获取指示对象何处出错的错误信息。
        /// </summary>
        /// <returns>
        /// 指示对象何处出错的错误信息。默认值为空字符串 ("")。
        /// </returns>
        [JsonIgnore]
        public virtual string Error
        {
            get { return string.Empty; }
        }

        #endregion
    }
}