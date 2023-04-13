using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DVG.WIS.Utilities.Base
{
    public abstract class EntityBase
    {
        private readonly Dictionary<string, object> _extendedProperties = new Dictionary<string, object>();

        public object this[string propertyName]
        {
            get
            {
                return (_extendedProperties.ContainsKey(propertyName) ? _extendedProperties[propertyName] : null);
            }
            set
            {
                if (_extendedProperties.ContainsKey(propertyName))
                {
                    _extendedProperties[propertyName] = value;
                }
                else
                {
                    _extendedProperties.Add(propertyName, value);
                }
            }
        }

        public static bool SetObjectValue<T>(IDataRecord reader, ref T entity) where T : class
        {
            Type type = typeof(T);

            if (type.BaseType == typeof(EntityBase) && entity != null)
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var fieldName = reader.GetName(i);
                    try
                    {

                        var propertyInfo =
                            type.GetProperties().FirstOrDefault(
                                info => info.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
                        if (propertyInfo != null)
                        {
                            if ((reader[i] != null) && (reader[i] != DBNull.Value))
                            {
                                propertyInfo.SetValue(entity, reader[i], null);
                            }
                            else
                            {
                                if (propertyInfo.PropertyType == typeof(DateTime) ||
                                    propertyInfo.PropertyType == typeof(DateTime?))
                                {
                                    propertyInfo.SetValue(entity, DateTime.MinValue, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(string))
                                {
                                    propertyInfo.SetValue(entity, string.Empty, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(bool) ||
                                    propertyInfo.PropertyType == typeof(bool?))
                                {
                                    propertyInfo.SetValue(entity, false, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(decimal) ||
                                    propertyInfo.PropertyType == typeof(decimal?))
                                {
                                    propertyInfo.SetValue(entity, decimal.Zero, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(double) ||
                                propertyInfo.PropertyType == typeof(double?))
                                {
                                    propertyInfo.SetValue(entity, double.Parse("0"), null);
                                }
                                else if (propertyInfo.PropertyType == typeof(float) ||
                           propertyInfo.PropertyType == typeof(float?))
                                {
                                    propertyInfo.SetValue(entity, 0, null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(entity, 0, null);
                                }
                            }
                        }
                        else
                        {
                            (entity as EntityBase)[fieldName] = reader[i];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
