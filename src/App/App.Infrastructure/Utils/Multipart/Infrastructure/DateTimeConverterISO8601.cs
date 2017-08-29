﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace App.Infrastructure.Utils.Multipart.Infrastructure
{
    /// <summary>
    /// convert datetime to ISO 8601 format string
    /// </summary>
    public class DateTimeConverterIso8601 : DateTimeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is DateTime && destinationType == typeof (string))
            {
                return ((DateTime)value).ToString("O"); // ISO 8601
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
