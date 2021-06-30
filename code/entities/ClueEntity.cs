using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
    [Library("clue", Description = "Represents a Clue.")]
    public partial class ClueEntity : Entity
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

        // TODO: Figure out how to manage the state per player i.e. if one player picks up a clue it only effects them.
        // When the Bystander touches this entity add it to the there list of clues and remove it from there game.
        [Sandbox.Internal.Description("OnStartTouch Event")]
        public Output OnStartTouch;

        public static void FireOnStartTouchEvent(SomeEntity ent)
        {
            ent.FireOutput("OnStartTouch", ent);
        }
    }
}