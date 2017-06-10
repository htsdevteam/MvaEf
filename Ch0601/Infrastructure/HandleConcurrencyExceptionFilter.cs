using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Ch0601.Infrastructure
{
    public class HandleConcurrencyExceptionFilter : FilterAttribute, IExceptionFilter
    {
        private PropertyMatchingMode _propertyMatchingMode;
        /// <summary>
        /// This defines when the concurrencyexception happens, 
        /// </summary>
        public enum PropertyMatchingMode
        {
            /// <summary>
            /// Uses only the field names in the model to check against the entity. This option is best when you are using 
            /// View Models with limited fields as opposed to an entity that has many fields. The ViewModel (or model) field names will
            /// be used to check current posted values vs. db values on the entity itself.
            /// </summary>
            UseViewModelNamesToCheckEntity = 0,
            /// <summary>
            /// Use any non-matching value fields on the entity (except timestamp fields) to add errors to the ModelState.
            /// </summary>
            UseEntityFieldsOnly = 1,
            /// <summary>
            /// Tells the filter to not attempt to add field differences to the model state.
            /// This means the end user will not see the specifics of which fields caused issues
            /// </summary>
            DontDisplayFieldClashes = 2
        }


        public HandleConcurrencyExceptionFilter()
        {
            _propertyMatchingMode = PropertyMatchingMode.UseViewModelNamesToCheckEntity;
        }

        public HandleConcurrencyExceptionFilter(PropertyMatchingMode propertyMatchingMode)
        {
            _propertyMatchingMode = propertyMatchingMode;
        }


        public void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception as DbUpdateConcurrencyException;
            if (!filterContext.ExceptionHandled && ex != null)
            {
                var entry = ex.Entries.Single();
                var clientValues = entry.CurrentValues.Clone().ToObject();
                entry.Reload();
                var databaseValues = entry.CurrentValues.ToObject();

                List<string> propertyNames;
                filterContext.Controller.ViewData.ModelState.AddModelError("",
                    "The record you are trying to edit"
                    + " was modified by another user after you got the original value."
                    + " The edit operation was cancelled and the current values"
                    + " in the database have been displayed. If you still want"
                    + " to edit this record, click the Save button again to"
                    + " cause your changes to be current saved values");
                PropertyInfo[] entityFromDbProperties = databaseValues.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

                if (_propertyMatchingMode == PropertyMatchingMode.UseViewModelNamesToCheckEntity)
                {
                    propertyNames = filterContext.Controller.ViewData.ModelState.Keys.ToList();
                }
                else if (_propertyMatchingMode == PropertyMatchingMode.UseEntityFieldsOnly)
                {
                    propertyNames = databaseValues.GetType().GetProperties(BindingFlags.Public).Select(p => p.Name).ToList();
                }
                else
                {
                    filterContext.ExceptionHandled = true;
                    UpdateTimestampField(filterContext, entityFromDbProperties, databaseValues);
                    filterContext.Result = new ViewResult { ViewData = filterContext.Controller.ViewData };
                    return;
                }

                UpdateTimestampField(filterContext, entityFromDbProperties, databaseValues);

                foreach (var propertyInfo in entityFromDbProperties)
                {
                    if (propertyNames.Contains(propertyInfo.Name))
                    {
                        if (propertyInfo.GetValue(databaseValues, null)
                            != propertyInfo.GetValue(clientValues, null))
                        {
                            var currentValue = propertyInfo.GetValue(databaseValues, null);
                            if (currentValue == null || string.IsNullOrEmpty(currentValue.ToString()))
                            {
                                currentValue = "Empty";
                            }
                            filterContext.Controller.ViewData.ModelState.AddModelError(
                                propertyInfo.Name, "Current value: " + currentValue);
                        }
                    }
                }
                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult { ViewData = filterContext.Controller.ViewData };
            }
        }

        private void UpdateTimestampField(ExceptionContext filterContext, PropertyInfo[] entityFromDbProperties, object databaseValues)
        {
            foreach (var propertyInfo in entityFromDbProperties)
            {
                var attributes = propertyInfo.GetCustomAttributesData();

                //If this is a timestamp field, we need to set the current value.
                foreach (CustomAttributeData attr in attributes)
                {
                    if (typeof(System.ComponentModel.DataAnnotations.TimestampAttribute).IsAssignableFrom(attr.Constructor.DeclaringType))
                    {
                        //This currently works only with byte[] timestamps. You can use dates as timestamps, but support is not provided here.
                        byte[] timestampValue = (byte[])propertyInfo.GetValue(databaseValues, null);
                        //we've found the timestamp. Add it to the model.
                        filterContext.Controller.ViewData.ModelState.Add(propertyInfo.Name, new ModelState());
                        filterContext.Controller.ViewData.ModelState.SetModelValue(propertyInfo.Name,
                            new ValueProviderResult(Convert.ToBase64String(timestampValue), Convert.ToBase64String(timestampValue), null));
                        break;
                    }
                }
            }
        }

    }
}