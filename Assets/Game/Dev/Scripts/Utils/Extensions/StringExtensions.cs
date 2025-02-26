using System.Globalization;
using System.Linq;

namespace RunTogether.Extensions{

  public static class StringExtensions{

    public static int ConvertToInt(this string value){ // !: Remove chars and convert to int
      if (string.IsNullOrEmpty(value)) return default;

      var charArray = value.ToCharArray();

      string nonNumericString = charArray.Where(c => char.IsNumber(c)).Aggregate(string.Empty, (current, c) => current + c);

      int convertedValue = int.Parse(nonNumericString);

      return convertedValue;
    }
    
    public static string RemoveSpecialCharacters(this string value){
      if (string.IsNullOrEmpty(value)) return value;
      return new string(value.Where(char.IsLetterOrDigit).ToArray());
    }

  #region Cases
    public static string ToCamelCase(this string value){
      if (string.IsNullOrEmpty(value)) return value;
      return char.ToUpper(value[0]) + value.Substring(1);
    }

    public static string ToPascalCase(this string value){
      if (string.IsNullOrEmpty(value)) return value;
      return char.ToUpper(value[0]) + value.Substring(1);
    }

    public static string ToSnakeCase(this string value){
      if (string.IsNullOrEmpty(value)) return value;
      return value.Replace(" ", "_");
    }

    public static string ToKebabCase(this string value){
      if (string.IsNullOrEmpty(value)) return value;
      return value.Replace(" ", "-");
    }

    public static string ToTitleCase(this string value){
      if (string.IsNullOrEmpty(value)) return value;
      return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }
  #endregion

  }

}