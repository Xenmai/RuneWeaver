using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using System.Linq;

namespace RuneWeaver.GameProperties.GameControllers
{
    public class UnitActionHandlerProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.KeyDown += Window_KeyDown;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.MouseDown -= Window_MouseDown;
            Engine.Window.KeyDown -= Window_KeyDown;
        }
        
        /// <summary>
        /// The current unit action.
        /// </summary>
        public BasicActionProperty Action;
                
        /// <summary>
        /// Tracks mouse presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                if (Action != null && Action.Preparing)
                {
                    Action.Execute();
                }
            }
            else if (e.Button == MouseButton.Right)
            {
                if (Action != null && Action.Preparing)
                {
                    Action.Cancel();
                }
            }
        }

        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Number1)
            {
                ClientEntity selected = Game.UnitSelector.GetProperty<UnitSelectorProperty>().Selected;
                if (selected != null && selected.GetAllSubTypes<BasicUnitProperty>().First().Ally)
                {
                    Action = selected.GetAllSubTypes<BasicActionProperty>().First();
                    if (!Action.Executing)
                    {
                        Action.Prepare();
                    }
                }
            }
        }
    }
}
