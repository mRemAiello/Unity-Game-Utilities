using UnityEngine;
using UnityEditor;

namespace DungeonsOfSurvival
{
    public class AnimationEventRemover
    {
        [MenuItem("Assets/Remove Animation Events", false, 2000)]
        private static void RemoveAnimationEvents()
        {
            Object[] selectedObjects = Selection.objects;

            int modifiedCount = 0;

            foreach (Object obj in selectedObjects)
            {
                if (obj is AnimationClip clip)
                {
                    // Registra undo
                    Undo.RecordObject(clip, "Remove Animation Events");

                    // Rimuove tutti gli eventi
                    AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[0]);

                    EditorUtility.SetDirty(clip);
                    modifiedCount++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Rimossi Animation Events da {modifiedCount} clip.");
        }

        // Mostra l'opzione solo se è selezionata almeno una AnimationClip
        [MenuItem("Assets/Remove Animation Events", true)]
        private static bool ValidateRemoveAnimationEvents()
        {
            foreach (Object obj in Selection.objects)
            {
                if (obj is AnimationClip)
                    return true;
            }
            return false;
        }
    }
}
