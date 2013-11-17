using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ZyGames.GamesReportService;

namespace ZyGames.OA.BLL.Remote.Ddz
{
    public class DdzDataService : DataService
    {
        public DdzDataService(int gameId, int serverId)
            : base(gameId, serverId)
        {
        }

        public override IDataGetter Get<T>(string filter, int pageIndex, int pageSize, List<SqlParameter> parameters)
        {
            if (typeof(T).Equals(typeof(UserGetter)))
            {
                return new UserGetter(GameId, ServerId, filter, parameters);
            }
            else if (typeof(T).Equals(typeof(PrizeGetter)))
            {
                return new PrizeGetter(GameId, ServerId, filter, parameters);
            }
            return null;
        }
    }
}
