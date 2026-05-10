namespace GameUtils
{
    public class SaveEncryptionSettings
    {
        public SaveEncryptionMode EncryptionMode { get; set; }
        public string Password { get; set; }

        public SaveEncryptionSettings()
        {
            EncryptionMode = SaveEncryptionMode.None;
            Password = string.Empty;
        }
    }
}
