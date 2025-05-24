using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Airalnes.Helpers
{
    public static class InputFormatter
    {
        public static bool IsDigitInput(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        public static void FormatCardNumber(TextBox textBox)
        {
            int cursorPosition = textBox.SelectionStart;
            string text = Regex.Replace(textBox.Text, @"\s+", "");

            if (text.Length > 16)
                text = text.Substring(0, 16);

            string formatted = string.Join(" ", Regex.Matches(text, @"\d{1,4}")
                                                    .Cast<Match>()
                                                    .Select(m => m.Value));

            if (textBox.Text != formatted)
            {
                textBox.Text = formatted;
                textBox.SelectionStart = Math.Min(cursorPosition + (formatted.Length - text.Length), formatted.Length);
            }
        }

        public static void FormatCardDate(TextBox textBox)
        {
            string raw = textBox.Text.Replace("/", "");
            int selectionStart = textBox.SelectionStart;

            if (raw.Length > 4)
                raw = raw.Substring(0, 4);

            if (raw.Length >= 2)
            {
                string monthPart = raw.Substring(0, 2);
                if (!int.TryParse(monthPart, out int month) || month < 1 || month > 12)
                {
                    raw = raw.Substring(0, 1);
                }
            }

            string formatted = raw;
            if (raw.Length >= 3)
                formatted = raw.Insert(2, "/");

            if (textBox.Text != formatted)
            {
                textBox.Text = formatted;
                textBox.SelectionStart = Math.Min(formatted.Length, selectionStart);
            }
        }
    }
}
