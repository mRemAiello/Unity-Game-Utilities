namespace GameUtils
{
    /// <summary>
    /// Rappresenta lo stato di una quest nel gioco
    /// </summary>
    public enum QuestStatus
    {
        /// <summary>
        /// La quest è disponibile ma non ancora iniziata
        /// </summary>
        Available,
        
        /// <summary>
        /// La quest è attualmente attiva
        /// </summary>
        Active,
        
        /// <summary>
        /// La quest è completata con successo
        /// </summary>
        Completed,
        
        /// <summary>
        /// La quest è fallita
        /// </summary>
        Failed,
        
        /// <summary>
        /// La quest è stata abbandonata
        /// </summary>
        Abandoned
    }
}