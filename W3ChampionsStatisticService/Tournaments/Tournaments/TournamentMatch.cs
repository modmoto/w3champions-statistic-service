﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace W3ChampionsStatisticService.Tournaments.Tournaments
{
    public class TournamentMatch
    {
        public string Id { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public List<TournamentPlayer> Players { get; set; }

        [BsonIgnore]
        public string Winner
        {
            get
            {
                if (Players.All(p => p.Score == null || p.Score == null)) return null;
                if (Players.GroupBy(p => p.Score).Count() == 1) return null;

                return Players.OrderByDescending(p => p.Score).FirstOrDefault()?.BattleTag;
            }
        }
    }
}