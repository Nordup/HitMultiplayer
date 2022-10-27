using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace HitScriptableObjects
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/PlayersData", fileName = "PlayersData")]
    public class PlayersData : ScriptableObject
    {
        private class Player
        {
            public string Name;
            public int Score;
        }
        
        private readonly Dictionary<NetworkIdentity, Player> _players = new ();
        
        public void AddPlayer(NetworkIdentity playerId, string playerName)
        {
            if (_players.ContainsKey(playerId))
            {
                Debug.LogError($"Player {playerId} already added");
                return;
            }
            
            _players.Add(playerId, new Player()
            {
                Name = playerName,
                Score = 0
            });
        }
        
        public void RemovePlayer(NetworkIdentity playerId)
        {
            if (!_players.ContainsKey(playerId))
            {
                Debug.LogError($"Not found player {playerId}");
                return;
            }
        
            _players.Remove(playerId);
        }

        public List<NetworkIdentity> GetPlayers()
        {
            return _players.Keys.ToList();
        }
        
        public string GetName(NetworkIdentity playerId)
        {
            return _players.TryGetValue(playerId, out var player) ? player.Name : "";
        }

        public int GetScore(NetworkIdentity playerId)
        {
            return _players.TryGetValue(playerId, out var player) ? player.Score : -1;
        }

        public void SetScore(NetworkIdentity playerId, int score)
        {
            if (!_players.ContainsKey(playerId))
            {
                Debug.LogError($"Not found player {playerId}");
                return;
            }
        
            _players[playerId].Score = score;
        }
    }
}