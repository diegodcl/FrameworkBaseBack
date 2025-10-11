using System;
using System.Text.RegularExpressions;

namespace Core.Domain.ValueObjects
{
    public sealed class RegistroGeral
    {
        public string Value { get; }

        public RegistroGeral(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Invalid Registro Geral.", nameof(value));
            Value = value;
        }

        public static bool IsValid(string rg)
        {
            if (string.IsNullOrWhiteSpace(rg)) return false;
            rg = rg.Trim();
            // RG can have digits and sometimes a letter at the end, length usually 7-9
            rg = Regex.Replace(rg, "[^0-9A-Za-z]", "");
            if (rg.Length < 7 || rg.Length > 9) return false;
            // Optionally, add more rules for your system
            return true;
        }

        public override string ToString() => Value;
        public override bool Equals(object? obj) => obj is RegistroGeral other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
