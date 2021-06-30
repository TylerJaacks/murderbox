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
        public static void SpawnClue(Clue clue)
        {

        }

        // TODO: Spawn the Clue Props
        public static void SpawnPlayer(Player player)
        {

        }
    }
}