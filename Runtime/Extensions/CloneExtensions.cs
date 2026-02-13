using Newtonsoft.Json;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Provides cloning helpers for classes and ScriptableObjects.
    /// </summary>
    public static class CloneExtensions
    {
        /// <summary>
        /// Clones a plain class instance via JSON serialization.
        /// </summary>
        /// <typeparam name="T">Type of the class to clone.</typeparam>
        /// <param name="self">Instance to clone.</param>
        /// <returns>Cloned instance.</returns>
        public static T Clone<T>(this T self) where T : class
        {
            // Return null when there's nothing to clone.
            if (self == null)
            {
                return null;
            }

            // Serialize then deserialize to produce a deep clone.
            var serializedObject = JsonConvert.SerializeObject(self);

            // Deserialize into a new instance of the same type.
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        /// <summary>
        /// Clones a ScriptableObject instance via JSON serialization.
        /// </summary>
        /// <typeparam name="T">Type of the ScriptableObject to clone.</typeparam>
        /// <param name="self">Instance to clone.</param>
        /// <returns>Cloned ScriptableObject instance.</returns>
        public static T Clone<T>(this T self) where T : ScriptableObject
        {
            // Return null when there's nothing to clone.
            if (self == null)
            {
                return null;
            }

            // Serialize then deserialize to produce a deep clone.
            var serializedObject = JsonConvert.SerializeObject(self);

            // Create a Unity-managed instance before populating it.
            var clone = ScriptableObject.CreateInstance<T>();

            // Populate fields on the Unity-managed instance.
            JsonConvert.PopulateObject(serializedObject, clone);

            // Return the populated clone instance.
            return clone;
        }
    }
}
