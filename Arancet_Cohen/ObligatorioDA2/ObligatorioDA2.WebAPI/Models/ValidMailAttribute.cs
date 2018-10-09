using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;


namespace ObligatorioDA2.WebAPI.Models
{
    public class ValidMailAttribute: ValidationAttribute
    {
        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email =((UserModelIn)validationContext.ObjectInstance).Email;
            
            ValidationResult result;

            try
            {
                MailAddress m = new MailAddress(email);
                result= ValidationResult.Success;
            }
            catch (FormatException)
            {
                result = new ValidationResult(GetErrorMessage());
            }

            return result;
        }

        private string GetErrorMessage()
        {
            return "Email is not valid";
        }
    }
}
