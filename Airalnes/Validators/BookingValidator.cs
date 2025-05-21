using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Airalnes.Validators
{
    public class BookingValidator
    {
        public (bool IsValid, string ErrorMessage) Validate(BookingFormModel form)
        {
            if (string.IsNullOrWhiteSpace(form.Name) ||
                string.IsNullOrWhiteSpace(form.Surname) ||
                string.IsNullOrWhiteSpace(form.DateOfBirth) ||
                string.IsNullOrWhiteSpace(form.Sex) ||
                string.IsNullOrWhiteSpace(form.FlightNumber) ||
                string.IsNullOrWhiteSpace(form.CardNumber) ||
                string.IsNullOrWhiteSpace(form.Cost) ||
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
