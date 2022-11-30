﻿// <copyright file="IEmailWithLogService.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

namespace OnlineSales.Interfaces;

public interface IEmailWithLogService
{
    Task SendAsync(string subject, string fromEmail, string fromName, string recipient, string body, List<IFormFile>? attachments);

    Task SendToCustomerAsync(int customerId, string subject, string fromEmail, string fromName, string body, List<IFormFile>? attachments);
}