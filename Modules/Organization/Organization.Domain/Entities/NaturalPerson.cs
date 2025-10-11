using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Domain.Enums;

namespace Organization.Domain.Entities
{
    public class NaturalPerson : Person
    {
        // public override PersonType PersonType => PersonType.NaturalPerson;

        public NaturalPerson(string name, string email, string phoneNumber, PersonType personType = PersonType.NaturalPerson, DateTime? dateOfBirth = null,
            string? reg1 = null,
            string? reg2 = null,
            string? reg3 = null,
            string? reg4 = null,
            string? reg5 = null
            ) : base(name, email, phoneNumber, dateOfBirth, reg1, reg2, reg3, reg4, reg5)
        {
            
        }

        public static NaturalPerson Create(string name, string email, string phoneNumber, DateTime? dateOfBirth = null,
            string? reg1 = null,
            string? reg2 = null,
            string? reg3 = null,
            string? reg4 = null,
            string? reg5 = null)
        {
            // Add validation logic here if needed
            return new NaturalPerson(name, email, phoneNumber, PersonType.NaturalPerson, dateOfBirth, reg1, reg2, reg3, reg4, reg5);
        }
    }
}