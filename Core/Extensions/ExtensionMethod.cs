using Core.Entity;

namespace Core.Extensions
{
    public static class ExtensionMethod
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static DateTime ConvertToUtc(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime();
        }
        public static DateTime ToBangladeshTime(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            TimeZoneInfo zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime bangladeshTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), zoneInfo);
            return bangladeshTime;
        }

        public static string ToDisplayString(this DateTime dateTime)
        {
            return dateTime.ToString("dd-MM-yyyy");
        }

        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };

        public static DateTime FromBangladeshTimeToUtc(this DateTime dateTime)
        {
            dateTime = dateTime.AddHours(-6);
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return dateTime;
        }

        public static DateTime BangladeshDateInUtc(this DateTime dateTime)
        {
            return dateTime.ToBangladeshTime().Date.FromBangladeshTimeToUtc();
        }

        public static string? GetPropertyValue<T>(this T obj, string name) where T : class
        {
            var propName = name.Split(".");
            var dataObj = new List<object> {obj};

            Type t = typeof(T);

            int index = 0;
            foreach (var prop in propName)
            {
                var data = t.GetProperty(prop.FirstCharToUpper())?.GetValue(dataObj[index], null);

                if (data != null)
                {
                    if (data is string)
                    {
                        return data.ToString();
                    }

                    t = data.GetType();
                    dataObj.Add(data);
                    index = index + 1;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public static bool HasProperty(this IQueryable objectToCheck, string propertyName)
        {
            var type = objectToCheck.ElementType;
            return type.GetProperty(propertyName.FirstCharToUpper()) != null;
        }

        public static void UpdateObjectWithoutId<TFirst, TSecond>(this TFirst obj1, TSecond obj2)
        {
            foreach (var property in obj2?.GetType().GetProperties()!)
            {
                if (property.Name == nameof(BaseEntity.Id)) continue;
                obj1?.GetType().GetProperty(property.Name)?.SetValue(obj1, property.GetValue(obj2));
            }

        }
        public static void UpdateObject<TFirst, TSecond>(this TFirst obj1, TSecond obj2)
        {
            foreach (var property in obj2?.GetType().GetProperties()!)
                obj1?.GetType().GetProperty(property.Name)?.SetValue(obj1, property.GetValue(obj2));
        }
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }
    }
}
