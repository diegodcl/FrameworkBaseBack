using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization.Domain.Enums;

namespace Organization.Domain.Entities
{
    public class LegalPerson : Person
    {
        // public override PersonType PersonType => PersonType.LegalPerson;


        public LegalPerson(string name, string email, string phoneNumber, DateTime? dateOfBirth = null,
            string? reg1 = null,
            string? reg2 = null,
            string? reg3 = null,
            string? reg4 = null,
            string? reg5 = null) : base(name, email, phoneNumber, dateOfBirth, reg1, reg2, reg3, reg4, reg5)
        {
            
        }
        public static LegalPerson Create(string name, string email, string phoneNumber, DateTime? dateOfBirth = null,
            string? reg1 = null,
            string? reg2 = null,
            string? reg3 = null,
            string? reg4 = null,
            string? reg5 = null)
        {
            // Add validation logic here if needed
            return new LegalPerson(name, email, phoneNumber, dateOfBirth, reg1, reg2, reg3, reg4, reg5);
        }
    }
}