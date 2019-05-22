using FGECore.MathHelpers;
using OpenTK;

namespace RuneWeaver.Utilities
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this Location loc)
        {
            return new Vector2((float)loc.X, (float)loc.Y);
        }

        public static Location ToLocation(this Vector2 v)
        {
            return new Location(v.X, v.Y, 0);
        }
    }
}
