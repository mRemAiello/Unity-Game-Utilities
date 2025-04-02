using System.Collections.Generic;

namespace GameUtils
{
   public interface IModifiersOperations
   {
      //void AddModifier(Modifier modifier);
      //bool TryRemoveModifier(Modifier modifier);
      //ModifiersCollection GetAllModifiers();
      float CalculateModifiersValue(float baseValue, float currentValue);
   }
}