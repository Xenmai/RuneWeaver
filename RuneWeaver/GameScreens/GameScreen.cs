using System.Collections.Generic;
using System.Linq;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.UISystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using RuneWeaver.GameProperties.GameEntities;

namespace RuneWeaver.GameScreens
{
    /// <summary>
    /// Represents the main menu.
    /// </summary>
    public class GameScreen : UIScreen
    {
        /// <summary>
        /// The relevant play button.
        /// </summary>
        public UIButton PlayButton;

        /// <summary>
        /// Constructs a new main menu screen.
        /// </summary>
        /// <param name="view">The associated view.</param>
        public GameScreen(ViewUI2D view) : base(view)
        {
            PlayButton = new UIButton("White", "Reset Turn", Client.FontSets.SlightlyBigger, ResetEnergy, new UIPositionHelper(view).Anchor(UIAnchor.BOTTOM_RIGHT).ConstantWidthHeight(350, 70));
            AddChild(PlayButton);
        }

        void ResetEnergy()
        {
            foreach (ClientEntity ent in Engine.EntityList)
            {
                IEnumerable<BasicUnitProperty> props = ent.GetAllSubTypes<BasicUnitProperty>();
                if (props.Count() > 0)
                {
                    props.First().Energy = props.First().MaxEnergy;
                }
            }
        }
    }
}

