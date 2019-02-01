using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.UISystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.MainGame;
using System.Linq;
using FreneticGameCore.CoreSystems;

namespace RuneWeaver.GameScreens
{
    /// <summary>
    /// Represents the main menu.
    /// </summary>
    public class GameScreen : UIScreen
    {
        /// <summary>
        /// The "Reset Turn" button.
        /// </summary>
        public UIButton ResetButton;

        /// <summary>
        /// The selected unit name label.
        /// </summary>
        public UIButton UnitNameLabel;

        /// <summary>
        /// The selected unit energy label.
        /// </summary>
        public UILabel UnitEnergyLabel;

        /// <summary>
        /// Constructs a new main menu screen.
        /// </summary>
        /// <param name="view">The associated view.</param>
        public GameScreen(ViewUI2D view) : base(view)
        {
            Game game = Engine.Source as Game;
            int height = game.Client.WindowHeight;
            int width = game.Client.WindowWidth - height;
            ResetButton = new UIButton("White", "^!Reset Turn", Client.FontSets.SlightlyBigger, ResetEnergy, new UIPositionHelper(view).Anchor(UIAnchor.BOTTOM_RIGHT).ConstantWidthHeight(width, 70));
            UnitNameLabel = new UIButton("White", string.Empty, Client.FontSets.SlightlyBigger, Nothing, new UIPositionHelper(view).Anchor(UIAnchor.TOP_RIGHT).ConstantWidthHeight(width, 70));
            UnitEnergyLabel = new UILabel(string.Empty, Client.FontSets.SlightlyBigger, new UIPositionHelper(view).Anchor(UIAnchor.CENTER_RIGHT).ConstantWidthHeight(width, 70));
            AddChild(ResetButton);
            AddChild(UnitNameLabel);
            AddChild(UnitEnergyLabel);
        }

        /// <summary>
        /// Resets every unit's energy.
        /// </summary>
        public void ResetEnergy()
        {
            Game game = Engine.Source as Game;
            foreach (BasicUnitProperty unit in game.Units)
            {
                unit.UpdateEnergy(unit.MaxEnergy);
            }
        }

        /// <summary>
        /// Does nothing at all.
        /// </summary>
        private void Nothing()
        {
        }
    }
}

