using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Domain.ValueObjects
{
    [ComplexType]
    public sealed class Cnpj
    {
        [MaxLength(14)]
        public string Value { get; }

        public Cnpj(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Invalid CNPJ.", nameof(value));
            Value = value;
        }

        public static bool IsValid(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");
            if (cnpj.Length != 14) return false;

            // Invalid known CNPJs
            if (new string(cnpj[0], cnpj.Length) == cnpj) return false;

            int[] multiplicator1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicator2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCnpj, digit;
            int sum, rest;

            tempCnpj = cnpj.Substring(0, 12);
            sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplicator1[i];

            rest = (sum % 11);
            
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = rest.ToString();
            tempCnpj += digit;
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplicator2[i];

            rest = (sum % 11);

            if (rest < 2)
                rest = 0;
            else
            rest = 11 - rest;

            digit += rest.ToString();
            return cnpj.EndsWith(digit);
        }

        public override string ToString() => Value;
        public override bool Equals(object? obj) => obj is Cnpj other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}