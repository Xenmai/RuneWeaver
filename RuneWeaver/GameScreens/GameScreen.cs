using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.UISystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using RuneWeaver.GameProperties.GameEntities;
using RuneWeaver.MainGame;
using System.Linq;

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
        /// The "Selected Unit Name" label.
        /// </summary>
        public UIButton UnitNameLabel;

        /// <summary>
        /// Constructs a new main menu screen.
        /// </summary>
        /// <param name="view">The associated view.</param>
        public GameScreen(ViewUI2D view) : base(view)
        {
            Game game = Engine.Source as Game;
            int height = game.Client.WindowHeight;
            int width = game.Client.WindowWidth - height;
            ResetButton = new UIButton("White", "^!Reset Turn", Client.FontSets.SlightlyBigger, Nothing, new UIPositionHelper(view).Anchor(UIAnchor.BOTTOM_RIGHT).ConstantWidthHeight(width, 70));
            UnitNameLabel = new UIButton("White", string.Empty, Client.FontSets.SlightlyBigger, Nothing, new UIPositionHelper(view).Anchor(UIAnchor.TOP_RIGHT).ConstantWidthHeight(width, 70));
            AddChild(ResetButton);
            AddChild(UnitNameLabel);
        }

        /// <summary>
        /// Does nothing at all.
        /// </summary>
        private void Nothing()
        {
        }
    }
}

