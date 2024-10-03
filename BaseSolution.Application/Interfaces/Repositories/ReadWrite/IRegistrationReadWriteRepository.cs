﻿using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface IRegistrationReadWriteRepository
    {
        Task<RequestResult<int>> AddRegistrationAsync(Registration entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateRegistrationAsync(Registration entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteRegistrationAsync(int id, CancellationToken cancellationToken);
    }
}
