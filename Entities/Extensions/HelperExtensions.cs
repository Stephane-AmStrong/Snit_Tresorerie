using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Entities.Extensions
{
    public static class StringExtentions
    {
        public static bool Contains(this string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (value.Length > maxLength) return value.Substring(0, maxLength) + " ...";
            return value;
        }

        public static bool Contains(this string source, string value, CompareOptions options, CultureInfo culture)
        {
            return culture.CompareInfo.IndexOf(source, value, options) >= 0;
        }

        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }


    public class ExcludeChar : ValidationAttribute
    {
        private readonly string _chars;
        public ExcludeChar(string chars)
            : base("{0} contains invalid character.")
        {
            _chars = chars;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                for (int i = 0; i < _chars.Length; i++)
                {
                    var valueAsString = value.ToString();
                    if (valueAsString.Contains(_chars[i]))
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    public class EmailField : ValidationAttribute
    {
        //private readonly string _chars;
        public EmailField()
            : base("{0} invalid email")
        {
            //_chars = chars;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMessage = FormatErrorMessage(validationContext.DisplayName);

            if (value == null) return new ValidationResult(errorMessage);

            try
            {
                MailAddress m = new MailAddress(value.ToString());
                return ValidationResult.Success;
            }
            catch (FormatException)
            {
                validationContext.MemberName = validationContext.DisplayName;
                return new ValidationResult(errorMessage);
            }
        }
    }

    //public static class CloneExtension
    //{


    //    public static T Clone<T>(this object item)
    //    {
    //        if (item != null)
    //        {
    //            BinaryFormatter formatter = new BinaryFormatter();
    //            MemoryStream stream = new MemoryStream();

    //            formatter.Serialize(stream, item);
    //            stream.Seek(0, SeekOrigin.Begin);

    //            T result = (T)formatter.Deserialize(stream);

    //            stream.Close();

    //            return result;
    //        }
    //        else
    //            return default(T);
    //    }
    //}
}
