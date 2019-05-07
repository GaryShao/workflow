using SFood.BackendService.Common.Consts;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace SFood.BackendService.Application.ServiceImplements
{
    public class RedisService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<bool> ResetOrderCodeIndex()
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            return await redisDb.StringSetAsync(AppConst.RedisKey_OrderDailyIndex, 1);
        }
    }
}
