using CI.QuickSave;

namespace GameUtils
{
    public class GameSaveManager : Singleton<GameSaveManager>
    {
        private void CheckFileSave(int saveSlot = 0)
        {
            //
            if (!QuickSaveBase.RootExists("Save" + saveSlot))
            {
                var saveWriter = QuickSaveWriter.Create("Save" + saveSlot);
                saveWriter.Commit();
            }
        }
    }
}