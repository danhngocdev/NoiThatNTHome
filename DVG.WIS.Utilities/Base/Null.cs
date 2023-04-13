using System;
using System.Reflection;

namespace DVG.WIS.Utilities.Base
{
    public class Null
    {
        public static short NullShort
        {
            get
            {
                return -1;
            }
        }

        public static int NullInteger
        {
            get
            {
                return -1;
            }
        }

        public static byte NullByte
        {
            get
            {
                return 255;
            }
        }

        public static float NullSingle
        {
            get
            {
                return float.MinValue;
            }
        }

        public static double NullDouble
        {
            get
            {
                return double.MinValue;
            }
        }

        public static decimal NullDecimal
        {
            get
            {
                return decimal.MinValue;
            }
        }

        public static DateTime NullDate
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        public static string NullString
        {
            get
            {
                return "";
            }
        }

        public static bool NullBoolean
        {
            get
            {
                return false;
            }
        }

        public static Guid NullGuid
        {
            get
            {
                return Guid.Empty;
            }
        }

        public static object SetNull(object value, object field)
        {
            object returnValue;
            if (value == DBNull.Value)
            {
                if (field is short)
                {
                    returnValue = NullShort;
                }
                else if (field is byte)
                {
                    returnValue = NullByte;
                }
                else if (field is int)
                {
                    returnValue = NullInteger;
                }
                else if (field is float)
                {
                    returnValue = NullSingle;
                }
                else if (field is double)
                {
                    returnValue = NullDouble;
                }
                else if (field is decimal)
                {
                    returnValue = NullDecimal;
                }
                else if (field is DateTime)
                {
                    returnValue = NullDate;
                }
                else if (field is string)
                {
                    returnValue = NullString;
                }
                else if (field is bool)
                {
                    returnValue = NullBoolean;
                }
                else if (field is Guid)
                {
                    returnValue = NullGuid;
                }
                else
                {
                    returnValue = null;
                }
            }
            else
            {
                returnValue = value;
            }
            return returnValue;
        }
        public static object SetNull(PropertyInfo propertyInfo)
        {
            object returnValue = null;
            switch (propertyInfo.PropertyType.ToString())
            {
                case "System.Int16":
                    returnValue = NullShort;
                    break;
                case "System.Int32":
                case "System.Int64":
                    returnValue = NullInteger;
                    break;
                case "system.Byte":
                    returnValue = NullByte;
                    break;
                case "System.Single":
                    returnValue = NullSingle;
                    break;
                case "System.Double":
                    returnValue = NullDouble;
                    break;
                case "System.Decimal":
                    returnValue = NullDecimal;
                    break;
                case "System.DateTime":
                    returnValue = NullDate;
                    break;
                case "System.String":
                case "System.Char":
                    returnValue = NullString;
                    break;
                case "System.Boolean":
                    returnValue = NullBoolean;
                    break;
                case "System.Guid":
                    returnValue = NullGuid;
                    break;
                default:
                    var type = propertyInfo.PropertyType;
                    if (type.BaseType != null && type.BaseType == typeof(Enum))
                    {
                        var objEnumValues = Enum.GetValues(type);
                        Array.Sort(objEnumValues);
                        returnValue = Enum.ToObject(type, objEnumValues.GetValue(0));
                    }
                    break;
            }
            return returnValue;
        }
        public static bool SetNullBoolean(object value)
        {
            var retValue = NullBoolean;
            if (value != DBNull.Value)
            {
                retValue = Convert.ToBoolean(value);
            }
            return retValue;
        }
        public static DateTime SetNullDateTime(object value)
        {
            var retValue = NullDate;
            if (value != DBNull.Value)
            {
                retValue = Convert.ToDateTime(value);
            }
            return retValue;
        }
        public static int SetNullInteger(object value)
        {
            var retValue = NullInteger;
            if (value != DBNull.Value)
            {
                retValue = Convert.ToInt32(value);
            }
            return retValue;
        }
        public static float SetNullSingle(object value)
        {
            var retValue = NullSingle;
            if (value != DBNull.Value)
            {
                retValue = Convert.ToSingle(value);
            }
            return retValue;
        }
        public static string SetNullString(object value)
        {
            var retValue = NullString;
            if (value != DBNull.Value)
            {
                retValue = Convert.ToString(value);
            }
            return retValue;
        }
        public static Guid SetNullGuid(object value)
        {
            var retValue = Guid.Empty;
            if ((value != DBNull.Value) & !string.IsNullOrEmpty(value.ToString()))
            {
                retValue = new Guid(value.ToString());
            }
            return retValue;
        }
        public static object GetNull(object field, object dbNull)
        {
            var returnValue = field;
            if (field == null)
            {
                returnValue = dbNull;
            }
            else if (field is byte)
            {
                if (Convert.ToByte(field) == NullByte)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is short)
            {
                if (Convert.ToInt16(field) == NullShort)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is int)
            {
                if (Convert.ToInt32(field) == NullInteger)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is float)
            {
                if (Convert.ToSingle(field) == NullSingle)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is double)
            {
                if (Convert.ToDouble(field) == NullDouble)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is decimal)
            {
                if (Convert.ToDecimal(field) == NullDecimal)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is DateTime)
            {
                if (Convert.ToDateTime(field).Date == NullDate.Date)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is string)
            {
                if (field.ToString() == NullString)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is bool)
            {
                if (Convert.ToBoolean(field) == NullBoolean)
                {
                    returnValue = dbNull;
                }
            }
            else if (field is Guid)
            {
                if (((Guid)field).Equals(NullGuid))
                {
                    returnValue = dbNull;
                }
            }
            return returnValue;
        }
        public static bool IsNull(object field)
        {
            bool isNull;
            if (field != null)
            {
                if (field is int)
                {
                    isNull = field.Equals(NullInteger);
                }
                else if (field is short)
                {
                    isNull = field.Equals(NullShort);
                }
                else if (field is byte)
                {
                    isNull = field.Equals(NullByte);
                }
                else if (field is float)
                {
                    isNull = field.Equals(NullSingle);
                }
                else if (field is double)
                {
                    isNull = field.Equals(NullDouble);
                }
                else if (field is decimal)
                {
                    isNull = field.Equals(NullDecimal);
                }
                else if (field is DateTime)
                {
                    var date = (DateTime)field;
                    isNull = date.Date.Equals(NullDate.Date);
                }
                else if (field is string)
                {
                    isNull = field.Equals(NullString);
                }
                else if (field is bool)
                {
                    isNull = field.Equals(NullBoolean);
                }
                else if (field is Guid)
                {
                    isNull = field.Equals(NullGuid);
                }
                else
                {
                    isNull = false;
                }
            }
            else
            {
                isNull = true;
            }
            return isNull;
        }
    }
}
