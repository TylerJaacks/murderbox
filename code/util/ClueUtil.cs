using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
    public static class Clue
    {
        public int ClueId { get; set; }
        public String ClueModel { get; set; }
        public Vector3 CluePosition { get; set; }
        public Rotation ClueRotation { get; set; }

        public Clue(int id, string modelName, Vector3 position, Rotation rotation)
        {
            this.ClueId = id;
            this.ClueModel = modelName;
            this.CluePosition = position;
            this.ClueRotation = rotation;
        }
    }

    public static class ClueUtil
    {
        // TODO: Load these from the map/<map_name>/<map_name>.json
        public List<Clue> GetClues()
        {
            List<Clue> clues = new List<Clue>();
            
            Clue clue1 = new Clue(0, "", new Vector3(0, 15, 10), new Rotation(0, 0, 0, 0));
            Clue clue1 = new Clue(1, "", new Vector3(5, 10, 10), new Rotation(0, 0, 0, 0));
            Clue clue1 = new Clue(2, "", new Vector3(10, 5, 10), new Rotation(0, 0, 0, 0));
            Clue clue1 = new Clue(3, "", new Vector3(15, 0, 10), new Rotation(0, 0, 0, 0));

            return clues;
        }
    }
}