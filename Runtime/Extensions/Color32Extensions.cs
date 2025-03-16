using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// UnityEngine.Color32 extension methods
    /// </summary>
    public static class Color32Extensions
    {
        /// <summary>
        /// Sets the red component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <returns>Color with modified red component.</returns>
        public static Color32 WithR(this Color32 color, byte r)
        {
            return new Color32(r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Sets the green component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <returns>Color with modified green component.</returns>
        public static Color32 WithG(this Color32 color, byte g)
        {
            return new Color32(color.r, g, color.b, color.a);
        }

        /// <summary>
        /// Sets the blue component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified blue component.</returns>
        public static Color32 WithB(this Color32 color, byte b)
        {
            return new Color32(color.r, color.g, b, color.a);
        }

        /// <summary>
        /// Sets the alpha component of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified alpha component.</returns>
        public static Color32 WithA(this Color32 color, byte a)
        {
            return new Color32(color.r, color.g, color.b, a);
        }

        /// <summary>
        /// Sets the red and green components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithRG(this Color32 color, byte r, byte g)
        {
            return new Color32(r, g, color.b, color.a);
        }

        /// <summary>
        /// Sets the red and blue components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithRB(this Color32 color, byte r, byte b)
        {
            return new Color32(r, color.g, b, color.a);
        }

        /// <summary>
        /// Sets the red and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithRA(this Color32 color, byte r, byte a)
        {
            return new Color32(r, color.g, color.b, a);
        }

        /// <summary>
        /// Sets the green and blue components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithGB(this Color32 color, byte g, byte b)
        {
            return new Color32(color.r, g, b, color.a);
        }

        /// <summary>
        /// Sets the green and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithGA(this Color32 color, byte g, byte a)
        {
            return new Color32(color.r, g, color.b, a);
        }

        /// <summary>
        /// Sets the blue and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithBA(this Color32 color, byte b, byte a)
        {
            return new Color32(color.r, color.g, b, a);
        }

        /// <summary>
        /// Sets the red, green, and blue components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithRGB(this Color32 color, byte r, byte g, byte b)
        {
            return new Color32(r, g, b, color.a);
        }

        /// <summary>
        /// Sets the red, green, and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithRGA(this Color32 color, byte r, byte g, byte a)
        {
            return new Color32(r, g, color.b, a);
        }

        /// <summary>
        /// Sets the red, blue, and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="r">Red component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithRBA(this Color32 color, byte r, byte b, byte a)
        {
            return new Color32(r, color.g, b, a);
        }

        /// <summary>
        /// Sets the green, blue, and alpha components of a color.
        /// </summary>
        /// <param name="color">Color to modify.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        /// <returns>Color with modified components.</returns>
        public static Color32 WithGBA(this Color32 color, byte g, byte b, byte a)
        {
            return new Color32(color.r, g, b, a);
        }

        /// <summary>
        /// Converts a color to grayscale.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns>Grayscale value.</returns>
        public static float ToGray(this Color32 color)
        {
            return color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
        }

        /// <summary>
        /// Converts a color to grayscale.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns>Grayscale color.</returns>
        public static Color ToGrayColor(this Color32 color)
        {
            float gray = color.ToGray();
            return new Color(gray, gray, gray, color.a);
        }
    }
}
