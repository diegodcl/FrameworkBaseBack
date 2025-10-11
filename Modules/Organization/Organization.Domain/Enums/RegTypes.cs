using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.ValueObjects;

namespace Organization.Domain.Enums
{
    public enum RegTypes
    {
        Cnpj,
        Cpf,
        InscricaoEstadual,
        InscricaoMunicipal,
        RG
    }
}