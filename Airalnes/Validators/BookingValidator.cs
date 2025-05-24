using Airalnes.Models.ValidationModels;
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
                string.IsNullOrWhiteSpace(form.FlightNumber))
            {
                return (false, "Fields cannot be blank");
            }           
            return (true, string.Empty);
        }
    }
}
