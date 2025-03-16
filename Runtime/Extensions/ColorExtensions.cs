using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// UnityEngine.Color extension methods
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Sets the red component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <returns>Color with modified red component.</returns>
        public static Color WithR(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Sets the green component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <returns>Color with modified green component.</returns>
        public static Color WithG(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }

        /// <summary>
        /// Sets the blue component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified blue component.</returns>
        public static Color WithB(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }

        /// <summary>
        /// Sets the alpha component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified alpha component.</returns>
        public static Color WithA(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }

        /// <summary>
        /// Sets the red and green components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithRG(this Color color, float r, float g)
        {
            return new Color(r, g, color.b, color.a);
        }

        /// <summary>
        /// Sets the red and blue components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithRB(this Color color, float r, float b)
        {
            return new Color(r, color.g, b, color.a);
        }

        /// <summary>
        /// Sets the red and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithRA(this Color color, float r, float a)
        {
            return new Color(r, color.g, color.b, a);
        }

        /// <summary>
        /// Sets the green and blue components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithGB(this Color color, float g, float b)
        {
            return new Color(color.r, g, b, color.a);
        }

        /// <summary>
        /// Sets the green and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithGA(this Color color, float g, float a)
        {
            return new Color(color.r, g, color.b, a);
        }

        /// <summary>
        /// Sets the blue and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithBA(this Color color, float b, float a)
        {
            return new Color(color.r, color.g, b, a);
        }

        /// <summary>
        /// Sets the red, green, and blue components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithRGB(this Color color, float r, float g, float b)
        {
            return new Color(r, g, b, color.a);
        }

        /// <summary>
        /// Sets the red, green, and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithRGA(this Color color, float r, float g, float a)
        {
            return new Color(r, g, color.b, a);
        }

        /// <summary>
        /// Sets the red, blue, and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithRBA(this Color color, float r, float b, float a)
        {
            return new Color(r, color.g, b, a);
        }

        /// <summary>
        /// Sets the green, blue, and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color WithGBA(this Color color, float g, float b, float a)
        {
            return new Color(color.r, g, b, a);
        }

        /// <summary>
        /// Converts a color to grayscale.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns>Grayscale value.</returns>
        public static float ToGray(this Color color)
        {
            return color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
        }

        /// <summary>
        /// Converts a color to grayscale.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns>Grayscale color.</returns>
        public static Color ToGrayColor(this Color color)
        {
            float gray = color.ToGray();
            return new Color(gray, gray, gray, color.a);
        }
    }
}
