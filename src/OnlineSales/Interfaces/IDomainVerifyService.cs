﻿// <copyright file="IDomainVerifyService.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

using OnlineSales.DTOs;
using OnlineSales.Entities;

namespace OnlineSales.Interfaces
{
    public interface IDomainVerifyService
    {
        public Task Verify(Domain domain);

        public Task VerifyHttp(Domain domain);

        public Task VerifyDns(Domain domain);
    }
}