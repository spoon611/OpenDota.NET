﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OpenDota.NET.Players
{
    public class PlayerManager
    {
        public IEnumerable<ProPlayer> GetProPlayers()
        {
            return GetProPlayersAsync().GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<ProPlayer>> GetProPlayersAsync()
        {
            var client = OpenDotaAPIWrapper.Client;
            using (var response = await client.GetAsync("proplayers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var proPlayers = new List<ProPlayer>();
                    foreach(var proPlayer in JArray.Parse(result))
                    {
                        proPlayers.Add(ProPlayer.Deserialize(proPlayer));
                    }

                    return proPlayers;
                }
            }
            throw new Exception("Could not successfully get proplayers data");
        }

        public Player GetPlayer(int accountId)
        {
            return GetPlayerAsync(accountId).GetAwaiter().GetResult();
        }

        public async Task<Player> GetPlayerAsync(int accountId)
        {
            var client = OpenDotaAPIWrapper.Client;
            using (var response = await client.GetAsync(string.Format("players/{0}", accountId)))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    
                    return Player.Deserialize(result);
                }
            }
            throw new Exception("Could not successfully get player data");
        }

        public WinLoss GetPlayerWinLossCount(int accountId, PlayerWinLossQuery query = null)
        {
            return GetPlayerWinLossCountAsync(accountId, query).GetAwaiter().GetResult();
        }

        public async Task<WinLoss> GetPlayerWinLossCountAsync(int accountId, PlayerWinLossQuery query = null)
        {
            var client = OpenDotaAPIWrapper.Client;
            var queryStringParam = PlayerWinLossQuery.GetQueryString(query);
            using (var response = await client.GetAsync(string.Format("players/{0}/wl?{1}", accountId, queryStringParam)))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return WinLoss.Deserialize(result);
                }
            }
            throw new Exception("Could not successfully get player win loss data");
        }
    }
}
