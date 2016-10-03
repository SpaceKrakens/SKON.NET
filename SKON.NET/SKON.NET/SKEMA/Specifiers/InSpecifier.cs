
namespace SKON.SKEMA.Specifiers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ValueType = ValueType;

    class InSpecifier : SKEMASpecifier
    {
        private ValueType Type;

        private List<string> stringValues;
        private List<int> intValues;
        private List<double> doubleValues;
        private List<bool> booleanValues;
        private List<DateTime> dateTimeValues;
        
        public InSpecifier(List<string> values)
        {
            Type = ValueType.STRING;

            stringValues = values;
        }

        public InSpecifier(List<int> values)
        {
            Type = ValueType.INTEGER;

            intValues = values;
        }

        public InSpecifier(List<double> values)
        {
            Type = ValueType.DOUBLE;

            doubleValues = values;
        }

        public InSpecifier(List<bool> values)
        {
            Type = ValueType.BOOLEAN;

            booleanValues = values;
        }

        public InSpecifier(List<DateTime> values)
        {
            Type = ValueType.DATETIME;

            dateTimeValues = values;
        }

        public override bool Match(SKONObject obj)
        {
            switch (Type)
            {
                case ValueType.STRING:
                    return stringValues.Contains(obj.String);
                case ValueType.INTEGER:
                    if (obj.Int == null)
                    {
                        throw new ArgumentException("A SKONOnject of type INTEGER cannot have a null int value!");
                    }
                    return intValues.Contains(obj.Int ?? default(int));
                case ValueType.DOUBLE:
                    if (obj.Double == null)
                    {
                        throw new ArgumentException("A SKONOnject of type DOUBLE cannot have a null double value!");
                    }
                    return doubleValues.Contains(obj.Double ?? default(double));
                case ValueType.BOOLEAN:
                    if (obj.Boolean == null)
                    {
                        throw new ArgumentException("A SKONOnject of type BOOLEAN cannot have a null boolean value!");
                    }
                    return booleanValues.Contains(obj.Boolean ?? default(bool));
                case ValueType.DATETIME:
                    if (obj.DateTime == null)
                    {
                        throw new ArgumentException("A SKONOnject of type DATETIME cannot have a null DateTime value!");
                    }
                    return dateTimeValues.Contains(obj.DateTime ?? default(DateTime));
                default:
                    return false;
            }
        }

        public override string GetSKEMAString()
        {
            StringBuilder skemaStringBuilder = new StringBuilder("in [");
            
            switch (Type)
            {
                case ValueType.STRING:
                    for (int i = 0; i < stringValues.Count; i++)
                    {
                        skemaStringBuilder.Append("\"");
                        skemaStringBuilder.Append(stringValues[i]);
                        skemaStringBuilder.Append("\", ");
                    }
                    break;
                case ValueType.INTEGER:
                    for (int i = 0; i < intValues.Count; i++)
                    {
                        skemaStringBuilder.Append(intValues[i]);
                        skemaStringBuilder.Append(", ");
                    }
                    break;
                case ValueType.DOUBLE:
                    for (int i = 0; i < doubleValues.Count; i++)
                    {
                        skemaStringBuilder.Append(doubleValues[i]);
                        skemaStringBuilder.Append(", ");
                    }
                    break;
                case ValueType.BOOLEAN:
                    for (int i = 0; i < booleanValues.Count; i++)
                    {
                        skemaStringBuilder.Append(booleanValues[i]);
                        skemaStringBuilder.Append(", ");
                    }
                    break;
                case ValueType.DATETIME:
                    for (int i = 0; i < dateTimeValues.Count; i++)
                    {
                        skemaStringBuilder.Append(dateTimeValues[i]);
                        skemaStringBuilder.Append(", ");
                    }
                    break;
                default:
                    throw new InvalidOperationException("This Specifier does not support the SKONObject ValueType: " + Type + "!");
            }

            skemaStringBuilder.Append("]");

            return skemaStringBuilder.ToString();
        }
    }
}
