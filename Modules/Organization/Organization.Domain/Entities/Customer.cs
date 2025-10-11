using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Organization.Domain.Enums;
using Organization.Shared.Interfaces;

namespace Organization.Domain.Entities
{
    public class Customer : Base, ICustomer
    {
        public Person Person { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public static Customer CreateCustomer(string name, string email, string phoneNumber, DateTime? dateOfBirth, string reg1, string reg2, string reg3, string reg4, string reg5, string alias, PersonType personType)
        {
            if (personType == PersonType.NaturalPerson)
            {
                // Person person = NaturalPerson.Create(
                //     name,
                //     email,
                //     phoneNumber,
                //     dateOfBirth,
                //     reg1,
                //     reg2,
                //     reg3,
                //     reg4,
                //     reg5
                // );
                return new Customer
                {
                    Person = NaturalPerson.Create(
                        name,
                        email,
                        phoneNumber,
                        dateOfBirth,
                        reg1,
                        reg2,
                        reg3,
                        reg4,
                        reg5
                    ),
                    IsActive = true,
                    StartDate = DateTime.UtcNow,
                    Alias = alias
                };
            }
            else
            {
                // Person person = LegalPerson.Create(
                //     name,
                //     email,
                //     phoneNumber,
                //     dateOfBirth,
                //     reg1,
                //     reg2,
                //     reg3,
                //     reg4,
                //     reg5
                // );
                return new Customer
                {
                    Person = LegalPerson.Create(
                        name,
                        email,
                        phoneNumber,
                        dateOfBirth,
                        reg1,
                        reg2,
                        reg3,
                        reg4,
                        reg5
                    ),
                    IsActive = true,
                    StartDate = DateTime.UtcNow,
                    Alias = alias
                };
            }
        }
    }
}