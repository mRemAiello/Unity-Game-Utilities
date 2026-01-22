using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(-100)]
    public class GameEventDataManager : GenericDataManager<GameEventDataManager, GameEventAssetBase>
    {
        //
        protected override void OnPostAwake()
        {
            base.OnPostAwake();
            ResetAllEventData();
        }

        protected override void OnPostDestroy()
        {
            base.OnPostDestroy();
            ResetAllEventData();
        }

        private void ResetAllEventData()
        {
            foreach (var item in Items)
            {
                item.ResetData();
            }
        }
    }
}
