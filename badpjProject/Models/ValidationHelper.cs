using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public static class ValidationHelper
    {
        public static string ValidateContent(string content, int maxLength = 500)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "Content cannot be empty.";
            }

            if (content.Length > maxLength)
            {
                return $"Content cannot exceed {maxLength} characters.";
            }

            return null; // No validation errors
        }
    }
}