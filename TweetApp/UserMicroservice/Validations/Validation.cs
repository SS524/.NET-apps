using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserMicroservice.Exceptions;

namespace UserMicroservice.Validations
{
    public class Validation:IValidation
    {
        public bool DateValidation(string date)
        {
            try
            {
                DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EmailValidation(string email)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                    return true;
                else
                    return false;
                //throw new CustomException(email + " is not Valid Email Address");
            }
            catch
            {
                return false;
            }

        }

        public bool PasswordValidation(string password)
        {
            try
            {
                var input = password;


                if (string.IsNullOrWhiteSpace(input))
                {
                    //throw new Exception("\nPassword should not be empty");
                    return false;
                }

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasMiniMaxChars = new Regex(@".{8,15}");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (!hasLowerChar.IsMatch(input))
                {
                    //throw new Exception("\nPassword should contain at least one lower case letter.");
                    return false;

                }
                else if (!hasUpperChar.IsMatch(input))
                {
                    // throw new Exception("\nPassword should contain at least one upper case letter.");
                    return false;

                }
                else if (!hasMiniMaxChars.IsMatch(input))
                {
                    //throw new Exception("\nPassword should not be lesser than 8 or greater than 15 characters.");
                    return false;
                }
                else if (!hasNumber.IsMatch(input))
                {
                    //  throw new Exception("\nPassword should contain at least one numeric value.");
                    return false;
                }

                else if (!hasSymbols.IsMatch(input))
                {
                    //throw new Exception("\nPassword should contain at least one special case character.");
                    return false;

                }
                else
                {
                    return true;
                }




            }
            catch (CustomException ex)
            {
                Console.WriteLine($"\nException Occured: {ex.Message}");
                return true;
            }
        }


    }
}
