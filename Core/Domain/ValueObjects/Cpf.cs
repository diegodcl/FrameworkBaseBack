using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core.Domain.ValueObjects
{
    public sealed class Cpf
    {
        [MaxLength(11)]
        public string Value { get; }

        public Cpf(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Invalid CPF.", nameof(value));
            Value = value;
        }

        public static bool IsValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            cpf = Regex.Replace(cpf, "[^0-9]", "");
            if (cpf.Length != 11) return false;

            // Invalid known CPFs
            if (new string(cpf[0], cpf.Length) == cpf) return false;

            int[] multiplicator1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicator2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf, digit;
            int sum, rest;

            tempCpf = cpf.Substring(0, 9);
            sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplicator1[i];
            rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;
            digit = rest.ToString();
            tempCpf += digit;
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplicator2[i];
            rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;
            digit += rest.ToString();
            return cpf.EndsWith(digit);
        }

        public override string ToString() => Value;
        public override bool Equals(object? obj) => obj is Cpf other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
