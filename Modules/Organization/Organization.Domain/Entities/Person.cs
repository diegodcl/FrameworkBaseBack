using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Organization.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Domain.Entities
{
    public abstract class Person : Base
    {
        protected Person() { }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? Reg1 { get; private set; }
        public string? LabelReg1 { get; private set; }
        public string? Reg2 { get; private set; }
        public string? LabelReg2 { get; private set; }
        public string? Reg3 { get; private set; }
        public string? LabelReg3 { get; private set; }
        public string? Reg4 { get; private set; }
        public string? LabelReg4 { get; private set; }
        public string? Reg5 { get; private set; }
        public string? LabelReg5 { get; private set; }
        public DateTime? DateOfBirth { get; private set; }

        // [NotMapped]
        // public abstract PersonType PersonType { get; }
        // Outros campos genéricos
        protected Person(
            string name,
            string email,
            string phoneNumber,
            DateTime? dateOfBirth = null,
            string? reg1 = null,
            string? reg2 = null,
            string? reg3 = null,
            string? reg4 = null,
            string? reg5 = null
            )
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Reg1 = reg1;
            Reg2 = reg2;
            Reg3 = reg3;
            Reg4 = reg4;
            Reg5 = reg5;
        }

        // public static Person Create(string name, string email, string phoneNumber, DateTime? dateOfBirth = null,
        //     string? reg1 = null, string? labelReg1 = null,
        //     string? reg2 = null, string? labelReg2 = null,
        //     string? reg3 = null, string? labelReg3 = null,
        //     string? reg4 = null, string? labelReg4 = null,
        //     string? reg5 = null, string? labelReg5 = null)
        // {
        //     // Add validation logic here if needed
        //     return new Person(name, email, phoneNumber, dateOfBirth, reg1, labelReg1, reg2, labelReg2, reg3, labelReg3, reg4, labelReg4, reg5, labelReg5);
        // }
    }


}