using System;
using System.Text.RegularExpressions;

public class ValidatorPass
{
    public static bool IsValidPassword(string password)
    {
        if (password.Length < 8)
        {
            return false;
        }

       
        bool hasUpperChar = false;
        bool hasLowerChar = false;
        bool hasDigit = false;
        bool hasSpecialChar = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUpperChar = true;
            if (char.IsLower(c)) hasLowerChar = true;
            if (char.IsDigit(c)) hasDigit = true;
            if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
        }

        return hasUpperChar && hasLowerChar && hasDigit && hasSpecialChar;
    }
}
