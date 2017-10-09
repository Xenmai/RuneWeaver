using OpenTK.Input;
using RuneWeaver.GameProperties.GameEntities.UnitActions;
using System;
using System.Linq;

namespace RuneWeaver.GameProperties.GameControllers
{
    class UnitActionHandlerProperty : CustomClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseDown += Window_MouseDown;
            Engine.Window.KeyDown += Window_KeyDown;
            Game.HitboxRenderable = Engine.SpawnEntity();
            Selector = Entity.GetProperty<UnitSelectorProperty>();
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
        /// The main selector.
        /// </summary>
        public UnitSelectorProperty Selector;
        
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
                if (Selector.Selected != null)
                {
                    Action = Selector.Selected.GetAllSubTypes<BasicActionProperty>().First<BasicActionProperty>();
                    if (Action is MoveActionProperty)
                    {
                        Action.Prepare();
                    }
                }
            }
        }
    }
}
