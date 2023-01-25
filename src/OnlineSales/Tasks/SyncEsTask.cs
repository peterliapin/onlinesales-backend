﻿// <copyright file="SyncEsTask.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

using System.Reflection;
using System.Text;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Nest;
using OnlineSales.Configuration;
using OnlineSales.Data;
using OnlineSales.DataAnnotations;
using OnlineSales.Entities;
using OnlineSales.Helpers;
using OnlineSales.Services;

namespace OnlineSales.Tasks
{
    public class SyncEsTask : ChangeLogTask
    {
        private readonly ChangeLogTaskConfig? taskConfig = new ChangeLogTaskConfig();
        private readonly ElasticClient elasticClient;
        private readonly string prefix = string.Empty;

        public SyncEsTask(IConfiguration configuration, ApiDbContext dbContext, IEnumerable<PluginDbContextBase> pluginDbContexts, ElasticClient elasticClient, TaskStatusService taskStatusService)
            : base(dbContext, pluginDbContexts, taskStatusService)
        {
            var config = configuration.GetSection("Tasks:SyncEsTask") !.Get<ChangeLogTaskConfig>();
            if (config is not null)
            {
                taskConfig = config;
            }

            var elasticPrefix = configuration.GetSection("Elasticsearch:IndexPrefix").Get<string>();

            if (!string.IsNullOrEmpty(elasticPrefix))
            {
                prefix = elasticPrefix + "-";
            }

            this.elasticClient = elasticClient;
        }

        public override int ChangeLogBatchSize => taskConfig!.BatchSize;

        public override string CronSchedule => taskConfig!.CronSchedule;

        public override int RetryCount => taskConfig!.RetryCount;

        public override int RetryInterval => taskConfig!.RetryInterval;

        internal override void ExecuteLogTask(List<ChangeLog> nextBatch)
        {
            var bulkPayload = new StringBuilder();
            
            foreach (var item in nextBatch)
            {
                var entityState = item.EntityState;

                if (entityState == EntityState.Added || entityState == EntityState.Modified)
                {
                    var createItem = new { index = new { _index = prefix + item.ObjectType.ToLower(), _id = item.ObjectId } };
                    bulkPayload.AppendLine(JsonHelper.Serialize(createItem));
                    bulkPayload.AppendLine(item.Data);
                }

                if (entityState == EntityState.Deleted)
                {
                    var deleteItem = new { delete = new { _index = prefix + item.ObjectType.ToLower(), _id = item.ObjectId } };
                    bulkPayload.AppendLine(JsonHelper.Serialize(deleteItem));
                }
            }

            var bulkResponse = elasticClient.LowLevel.Bulk<StringResponse>(bulkPayload.ToString());

            Log.Information("ES Sync Bulk Saved : {0}", bulkResponse.ToString());
        }

        protected override bool IsTypeSupported(Type type)
        {
            return type.GetCustomAttribute<SupportsElasticSearchAttribute>() != null;
        }
    }
}
