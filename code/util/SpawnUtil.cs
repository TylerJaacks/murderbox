using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
    public static class SpawnUtil
    {
        static List<Vector3> GetSpawnPoints(String MapName)
        {
            var mapJson = JSON.Parse(jsonFileString);

            string fileJson = File.ReadAllText("maps/" + MapName + "/" + MapName + ".json");

            List<object> spawnList = fileJson.FromJSON<List<object>>()[1];

            foreach (object obj in spawnList)
            {
                object posObj = obj[0];

                int x = (int) posObj[0];
                int y = (int) posObj[1];
                int z = (int) posObj[2];

                spawnList.Add(new Vector3(x, y, z));
            }

            return spawnList;
        }

        static List<Clue> GetClueSpawnPoints(String MapName)
        {
            var mapJson = JSON.Parse(jsonFileString);

            string fileJson = File.ReadAllText("maps/" + MapName + "/" + MapName + ".json");

            List<object> spawnList = fileJson.FromJSON<List<object>>()[0];

            int i = 0;

            foreach (object obj in spawnList)
            {
                object posObj = obj[0];

                int x = (int) posObj[0];
                int y = (int) posObj[1];
                int z = (int) posObj[2];

                object angleObj = obj[1];

                int x = (int) angleObj = angleObj[0];
                int y = (int) angleObj = angleObj[1];
                int z = (int) angleObj = angleObj[2];
                int q = (int) angleObj = angleObj[3];

                object modelObj = obj[2];

                Clue clue1 = new Clue(i, modelObj, new Vector3(x, y, z), new Rotation(x, y, z, q));

                spawnList.Add(new Vector3(x, y, z));

                i++;
            }

            return spawnList;
        }
        
        // TODO: Spawn the Clue Props
        public static void SpawnClue(List<ClueEntity> clues, ClueEntity clueEntity, string mapName)
        {
            List<Vector3> spawnLocations = GetClueSpawnPoints(mapName);

            foreach (Vector3 spawn in spawnLocations) 
            {
                if (isClueAtLocation(clues, location)) 
                {
                    continue;
                } 
                else 
                {
                    // TODO: Spawn ClueEntity at the location.
                }
            }
        }

        // TODO: Spawn the Clue Props
        public static void SpawnPlayer(List<Player> players, Player player, string mapName)
        {
            List<Vector3> spawnLocations = GetSpawnPoints(mapName);

            foreach (Vector3 spawn in spawnLocations) 
            {
                if (isPlayerAtLocation(players, location)) 
                {
                    continue;
                } 
                else {
                    // TODO: Spawn Player at the location.
                }
            }
        }

        public static bool isPlayerAtLocation(List<Player> players, Vector3 location) {
            foreach (Player player in players)
            {
                // TODO: Actually get there location.
                Vector3 playersLocation = new Vector3(0, 0, 0);

                // TODO: Do a proper equals.
                if (playersLocation == location) return true;
            }

            return false;
        }

        public static bool isClueAtLocation(List<ClueEntity> clues, Vector3 location) {
            foreach (ClueEntity clue in clues)
            {
                // TODO: Actually get there location.
                Vector3 cluesLocation = new Vector3(0, 0, 0);

                // TODO: Do a proper equals.
                if (cluesLocation == location) return true;
            }

            return false;
        }
    }
}