using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
    [Library("clue", Description = "Represents a Clue.")]
    public partial class ClueEntity : PhysicsBase
    {
        [Property(Name="is_picked_up", Title="Is Picked Up.")]
        [Sandbox.Internal.DefaultValue(false)]
        public bool IsPickedUp { get; set; } = false;

        [Property(Name="model_name", Title="Model of the clue.")]
        [Sandbox.Internal.DefaultValue("")]
        public string ModelName { get; set; } = "";

        [Property(Name="position", Title="Position of the clue.")]
        [Sandbox.Internal.DefaultValue(0, 0, 0)]
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

        [Property(Name="rotation", Title="Rotation of the clue.")]
        [Sandbox.Internal.DefaultValue(0, 0, 0)]
        public Rotation Rotation { get; set; } = new Vector3(0, 0, 0);

        [Property(Name="clue_id", Title="Id of the clue.")]
        [Sandbox.Internal.DefaultValue(-1)]
        public int ClueId { get; set; } = -1;

        // TODO: Get Documentation on this Function.
        public override OnTouch(Player player)
        {
            if (!player._isMurderer || !player._hasGun)
            {
                player._clues.Add(this);
            }
        }
    }
}