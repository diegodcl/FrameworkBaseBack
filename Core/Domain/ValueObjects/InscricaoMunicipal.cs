using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core.Domain.ValueObjects
{
    public sealed class InscricaoMunicipal
    {
        [MaxLength(56)]
        public string Value { get; }

        public InscricaoMunicipal(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Invalid Inscrição Municipal.", nameof(value));
            Value = value;
        }

        public static bool IsValid(string inscricao)
        {
            if (string.IsNullOrWhiteSpace(inscricao)) return false;
            inscricao = inscricao.Trim();
            // Usually digits, sometimes with separators, length varies by municipality
            inscricao = Regex.Replace(inscricao, "[^0-9]", "");
            // Accept 6 to 12 digits (adjust as needed for your municipality)
            if (inscricao.Length < 6 || inscricao.Length > 12) return false;
            return true;
        }

        public override string ToString() => Value;
        public override bool Equals(object? obj) => obj is InscricaoMunicipal other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
