﻿using System;
using System.Linq;

namespace EnumValidator
{
    public static class EnumValidator
    {
        public static bool IsValid<TEnum>(this TEnum @enum) where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                throw new ArgumentException("TEnum must be an enum parameter");
            
            var attributeList = enumType.GetCustomAttributes(true).OfType<Attribute>().ToList();
            if (attributeList.Any(i => i.GetType() == typeof(FlagsAttribute)))
            {
                return !@enum.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Any(i =>
                {
                    var enumValue = (TEnum) Enum.Parse(enumType, i.Trim());
                    return !Enum.IsDefined(enumType, enumValue);
                }); // Trying to find non-defined value in flag enum
            }

            return @enum.IsDefined();
        }

        private static bool IsDefined<TEnum>(this TEnum @enum) where TEnum : Enum
        {
            return Enum.IsDefined(typeof(TEnum), @enum);
        }
    }
}