using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.UISystem;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.MainGame;

namespace RuneWeaver.GameScreens
{
    /// <summary>
    /// Represents the main menu.
    /// </summary>
    public class GameScreen : UIScreen
    {
        /// <summary>
        /// The main GUI container box.
        /// </summary>
        public UIColoredBox Box;

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
            Box = new UIColoredBox(new OpenTK.Vector4(0.3f, 0.3f, 0.8f, 1.0f),
                new UIPositionHelper(view).Anchor(UIAnchor.CENTER_RIGHT).GetterWidthHeight(() => (int)(game.Client.WindowWidth * 0.25), () => game.Client.WindowHeight));
            ResetButton = new UIButton("White", "^!Reset Turn", Client.FontSets.SlightlyBigger, ResetEnergy,
                new UIPositionHelper(view).Anchor(UIAnchor.BOTTOM_CENTER).GetterWidthHeight(() => (int)(game.Client.WindowWidth * 0.25), () => (int)(game.Client.WindowHeight * 0.15)));
            UnitNameLabel = new UIButton("White", string.Empty, Client.FontSets.SlightlyBigger, Nothing,
                new UIPositionHelper(view).Anchor(UIAnchor.TOP_CENTER).GetterWidthHeight(() => (int)(game.Client.WindowWidth * 0.25), () => (int)(game.Client.WindowHeight * 0.15)));
            UnitEnergyLabel = new UILabel(string.Empty, Client.FontSets.SlightlyBigger,
                new UIPositionHelper(view).Anchor(UIAnchor.CENTER).GetterWidthHeight(() => (int)(game.Client.WindowWidth * 0.25), () => (int)(game.Client.WindowHeight * 0.15)));
            Box.AddChild(ResetButton);
            Box.AddChild(UnitNameLabel);
            Box.AddChild(UnitEnergyLabel);
            AddChild(Box);
        }

        /// <summary>
        /// Resets every unit's energy.
        /// </summary>
        public void ResetEnergy()
        {
            Game game = Engine.Source as Game;
            foreach (BasicUnitProperty unit in game.Units)
            {
                unit.Energy = unit.MaxEnergy;
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

