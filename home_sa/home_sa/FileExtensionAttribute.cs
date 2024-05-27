using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FileExtensionAttribute : ValidationAttribute
{
    private readonly string[] _validExtensions;

    public FileExtensionAttribute(string validExtensions)
    {
        _validExtensions = validExtensions.Split(',').Select(e => e.Trim().ToLower()).ToArray();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_validExtensions.Contains(extension))
            {
                return new ValidationResult(GetErrorMessage());
            }
        }
        return ValidationResult.Success;
    }

    private string GetErrorMessage()
    {
        return $"Invalid file type. Allowed types are: {string.Join(", ", _validExtensions)}";
    }
}
