using Airalnes.Models.ValidationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Airalnes.Validators
{
    internal class PaymentValidator
    {
        public (bool IsValid, string ErrorMessage) Validate(PaymentValidationModel form)
        {
            if (string.IsNullOrWhiteSpace(form.CardNumber) ||
                string.IsNullOrWhiteSpace(form.CVV) ||
                string.IsNullOrWhiteSpace(form.DateCard))              
            {
                return (false, "Fields cannot be blank");
            }
            if (!Regex.IsMatch(form.CardNumber.Replace(" ", ""), @"^\d{16}$"))
            {
                return (false, "Card must contain 16 digits");
            }

            if (!Regex.IsMatch(form.CVV, @"^\d{3}$"))
            {
                return (false, "CVV must consist of 3 digits");
            }

            if (!Regex.IsMatch(form.DateCard, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                return (false, "Card date must be in MM/YY format");
            }

            return (true, string.Empty);
        }
    }
}
