﻿// <copyright file="EmailLog.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineSales.Entities
{
    public enum EmailStatus
    {
        NOTSENT = 0,
        SENT = 1,
    }

    [Table("email_log")]
    public class EmailLog : BaseEntity
    {
        /// <summary>
        /// Gets or sets reference to the CustomerEmailSchedule table.
        /// </summary>
        public int ScheduleId { get; set; }

        [JsonIgnore]
        [ForeignKey("CustomerEmailScheduleId")]
        public CustomerEmailSchedule? Schedule { get; set; }

        /// <summary>
        /// Gets or sets reference to the customer table.
        /// </summary>
        public int CustomerId { get; set; }

        [JsonIgnore]
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        /// <summary>
        /// Gets or sets reference to the EmailTemplate table.
        /// </summary>
        public int TemplateId { get; set; }

        [JsonIgnore]
        [ForeignKey("EmailTemplateId")]
        public EmailTemplate? Template { get; set; }

        [Required]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the email recipient.
        /// </summary>
        [Required]
        public string Recipient { get; set; } = string.Empty;

        [Required]
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email body.
        /// </summary>
        [Required]
        public string Body { get; set; } = string.Empty;

        public EmailStatus Status { get; set; }
    }
}