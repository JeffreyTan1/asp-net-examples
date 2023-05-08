using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WebApi.Validators;

// Reference: https://stackoverflow.com/a/40808293
public class CustomerNameAttribute : ValidationAttribute
{
    public CustomerNameAttribute()
    {
        ErrorMessage = "The {0} field is required";
    }

    public override bool IsValid(object value)
    {
        // check if the value is a string
        if (value is not string name)
        {
            ErrorMessage = "value must be a string";
            return false;
        }
            

        // check if string starts with an uppercase letter and throw an error if not
        if (name.Length > 0 && char.IsLower(name[0]))
        {
            ErrorMessage = "Customer name must start with an uppercase letter";
            return false;
        }
           

        // check if string only has alphabetic characters and spaces
        foreach (char c in name)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                ErrorMessage = "Customer name must only contain alphabetic characters and spaces";
                return false;
            }
                
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
    }
}
