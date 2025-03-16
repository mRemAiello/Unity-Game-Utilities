using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// UnityEngine.Camera extension methods
    /// </summary>
    public static class CameraExtensions
    {
        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="r">Red color to set.</param>
        public static void SetBackgroundColorR(this Camera camera, float r)
        {
            camera.backgroundColor = camera.backgroundColor.WithR(r);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="g">Green color to set.</param>
        public static void SetBackgroundColorG(this Camera camera, float g)
        {
            camera.backgroundColor = camera.backgroundColor.WithG(g);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="b">Blue color to set.</param>
        public static void SetBackgroundColorB(this Camera camera, float b)
        {
            camera.backgroundColor = camera.backgroundColor.WithB(b);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="a">Alpha color to set.</param>
        public static void SetBackgroundColorA(this Camera camera, float a)
        {
            camera.backgroundColor = camera.backgroundColor.WithA(a);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="r">Red color to set.</param>
        /// <param name="g">Green color to set.</param>
        public static void SetBackgroundColorRG(this Camera camera, float r, float g)
        {
            camera.backgroundColor = camera.backgroundColor.WithRG(r, g);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="r">Red color to set.</param>
        /// <param name="b">Blue color to set.</param>
        public static void SetBackgroundColorRB(this Camera camera, float r, float b)
        {
            camera.backgroundColor = camera.backgroundColor.WithRB(r, b);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="r">Red color to set.</param>
        /// <param name="a">Alpha color to set.</param>
        public static void SetBackgroundColorRA(this Camera camera, float r, float a)
        {
            camera.backgroundColor = camera.backgroundColor.WithGA(r, a);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="g">Green color to set.</param>
        /// <param name="b">Blue color to set.</param>
        public static void SetBackgroundColorGB(this Camera camera, float g, float b)
        {
            camera.backgroundColor = camera.backgroundColor.WithGB(g, b);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="g">Green color to set.</param>
        /// <param name="a">Alpha color to set.</param>
        public static void SetBackgroundColorGA(this Camera camera, float g, float a)
        {
            camera.backgroundColor = camera.backgroundColor.WithGA(g, a);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="b">Blue color to set.</param>
        /// <param name="a">Alpha color to set.</param>
        public static void SetBackgroundColorBA(this Camera camera, float b, float a)
        {
            camera.backgroundColor = camera.backgroundColor.WithBA(b, a);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="r">Red color to set.</param>
        /// <param name="g">Green color to set.</param>
        /// <param name="b">Blue color to set.</param>
        public static void SetBackgroundColorRGB(this Camera camera, float r, float g, float b)
        {
            camera.backgroundColor = camera.backgroundColor.WithRGB(r, g, b);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="r">Red color to set.</param>
        /// <param name="g">Green color to set.</param>
        /// <param name="a">Alpha color to set.</param>
        public static void SetBackgroundColorRGA(this Camera camera, float r, float g, float a)
        {
            camera.backgroundColor = camera.backgroundColor.WithRGA(r, g, a);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="r">Red color to set.</param>
        /// <param name="b">Blue color to set.</param>
        /// <param name="a">Alpha color to set.</param>
        public static void SetBackgroundColorRBA(this Camera camera, float r, float b, float a)
        {
            camera.backgroundColor = camera.backgroundColor.WithRBA(r, b, a);
        }

        /// <summary>
        /// Sets the background color of a camera.
        /// </summary>
        /// <param name="camera">Camera to modify.</param>
        /// <param name="g">Green color to set.</param>
        /// <param name="b">Blue color to set.</param>
        /// <param name="a">Alpha color to set.</param>
        public static void SetBackgroundColorGBA(this Camera camera, float g, float b, float a)
        {
            camera.backgroundColor = camera.backgroundColor.WithGBA(g, b, a);
        }
    }
}
