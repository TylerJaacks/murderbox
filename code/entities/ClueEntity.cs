using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderBoxGamemode
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

        [Input]
        public void DoSomethingElse(string str)
        {
            
        }

        [Input(Name = "DoSomething", Help = "Help text for input")]
        public void SomeInput()
        {

        }

        [Sandbox.Internal.Description("Fires when something happens")]
        public Output OnSomethingHappened;

        public static void MakeItDoSomething(SomeEntity ent)
        {
            ent.FireOutput("OnSomethingHappened", null);
        }
    }
}