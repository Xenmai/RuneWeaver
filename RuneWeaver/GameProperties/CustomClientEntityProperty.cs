using FreneticGameGraphics.ClientSystem.EntitySystem;
using RuneWeaver.MainGame;

namespace RuneWeaver.GameProperties
{
    public abstract class CustomClientEntityProperty : ClientEntityProperty
    {
        public Game Game
        {
            get
            {
                return Engine.Source as Game;
            }
        }
    }
}
