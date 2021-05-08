﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Adnc.Infra.Caching;
using Adnc.Infra.Common.Extensions;
using Adnc.Infra.Common.Helper.IdGeneraterInternal;
using Adnc.Application.Shared.Consts;

namespace Adnc.Application.Shared.IdGeneraterWorkerNode
{
    public class WorkerNode
    {
        private readonly ILogger<WorkerNode> _logger;
        private readonly IRedisProvider _redisProvider;
        private readonly IDistributedLocker _distributedLocker;

        public WorkerNode(ILogger<WorkerNode> logger
           , IRedisProvider redisProvider
           , IDistributedLocker distributedLocker)
        {
            _redisProvider = redisProvider;
            _distributedLocker = distributedLocker;
            _logger = logger;
        }

        internal async Task InitWorkerNodesAsync(string serviceName)
        {
            var workerIdSortedSetCacheKey = string.Format(SharedCachingConsts.WorkerIdSortedSetCacheKey, serviceName);

            if (!_redisProvider.KeyExists(workerIdSortedSetCacheKey))
            {
                _logger.LogInformation("Starting InitWorkerNodes:{0}", workerIdSortedSetCacheKey);

                var flag = await _distributedLocker.LockAsync(workerIdSortedSetCacheKey);

                if (!flag.Success)
                {
                    await Task.Delay(300);
                    await InitWorkerNodesAsync(serviceName);
                }

                long count = 0;
                try
                {
                    var set = new Dictionary<long, double>();
                    for (long index = 0; index <= YitterSnowFlake.MaxWorkerId; index++)
                    {
                        set.Add(index, DateTime.Now.GetTotalMilliseconds());
                    }
                    count = await _redisProvider.ZAddAsync(workerIdSortedSetCacheKey, set);
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await _distributedLocker.SafedUnLockAsync(workerIdSortedSetCacheKey,flag.LockValue);
                }

                _logger.LogInformation("Finlished InitWorkerNodes:{0}:{1}", workerIdSortedSetCacheKey, count);
            }
            else
                _logger.LogInformation("Exists WorkerNodes:{0}", workerIdSortedSetCacheKey);
        }

        internal async Task<long> GetWorkerIdAsync(string serviceName)
        {
            var workerIdSortedSetCacheKey = string.Format(SharedCachingConsts.WorkerIdSortedSetCacheKey, serviceName);

            var scirpt = @"local workerids = redis.call('ZRANGE', @key, @start,@stop)
                                    redis.call('ZADD',@key,@score,workerids[1])
                                    return workerids[1]";

            var parameters = new { key = workerIdSortedSetCacheKey, start = 0, stop = 0, score = DateTime.Now.GetTotalMilliseconds() };
            var luaResult = (byte[]) await _redisProvider.ScriptEvaluateAsync(scirpt, parameters);
            var workerId = _redisProvider.Serializer.Deserialize<long>(luaResult);

            _logger.LogInformation("Get WorkerNodes:{0}", workerId);

            return workerId;
        }

        internal async Task RefreshWorkerIdScoreAsync(string serviceName, long workerId, double? workerIdScore = null)
        {
            if (workerId < 0 || workerId > YitterSnowFlake.MaxWorkerId)
                throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0", YitterSnowFlake.MaxWorkerId));

            var workerIdSortedSetCacheKey = string.Format(SharedCachingConsts.WorkerIdSortedSetCacheKey, serviceName);

            var score = workerIdScore == null ? DateTime.Now.GetTotalMilliseconds() : workerIdScore.Value;
            await _redisProvider.ZAddAsync(workerIdSortedSetCacheKey, new Dictionary<long, double> { { workerId, score } });
            _logger.LogInformation("Refresh WorkerNodes:{0}:{1}", workerId, score);
        }
    }
}
