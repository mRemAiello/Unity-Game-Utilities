using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GameUtils
{
    public abstract class SaveFileStoreBase
    {
        private const string SaveFolderName = "Save";

        protected readonly string _root;
        protected readonly SaveEncryptionSettings _settings;
        protected Dictionary<string, string> _items;

        //
        protected SaveFileStoreBase(string root, SaveEncryptionSettings settings)
        {
            _root = root;
            _settings = settings ?? new SaveEncryptionSettings();
            _items = new Dictionary<string, string>();
        }

        public static bool RootExists(string root)
        {
            return File.Exists(GetFilePath(root));
        }

        protected void Load()
        {
            _items = LoadItems(_root, _settings);
        }

        protected void Save()
        {
            SaveItems(_root, _settings, _items);
        }

        protected static string GetFilePath(string root)
        {
            var safeRoot = string.Concat(root.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
            var folderPath = Path.Combine(Application.persistentDataPath, SaveFolderName);
            return Path.Combine(folderPath, safeRoot + ".json");
        }

        private static Dictionary<string, string> LoadItems(string root, SaveEncryptionSettings settings)
        {
            var filePath = GetFilePath(root);
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>();
            }

            var content = File.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(content))
            {
                return new Dictionary<string, string>();
            }

            var json = Decode(content, settings);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dict ?? new Dictionary<string, string>();
        }

        private static void SaveItems(string root, SaveEncryptionSettings settings, Dictionary<string, string> items)
        {
            var filePath = GetFilePath(root);
            var folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var json = JsonConvert.SerializeObject(items);
            var encoded = Encode(json, settings);
            File.WriteAllText(filePath, encoded, Encoding.UTF8);
        }

        private static string Encode(string raw, SaveEncryptionSettings settings)
        {
            return settings.EncryptionMode switch
            {
                SaveEncryptionMode.Base64 => Convert.ToBase64String(Encoding.UTF8.GetBytes(raw)),
                SaveEncryptionMode.Aes => EncryptAes(raw, settings.Password),
                _ => raw,
            };
        }

        private static string Decode(string encoded, SaveEncryptionSettings settings)
        {
            return settings.EncryptionMode switch
            {
                SaveEncryptionMode.Base64 => Encoding.UTF8.GetString(Convert.FromBase64String(encoded)),
                SaveEncryptionMode.Aes => DecryptAes(encoded, settings.Password),
                _ => encoded,
            };
        }

        private static string EncryptAes(string plainText, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Encryption password is required when AES is enabled.");
            }

            using var aes = Aes.Create();
            aes.Key = BuildAesKey(password);
            aes.GenerateIV();

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            var payload = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, payload, 0, aes.IV.Length);
            Buffer.BlockCopy(cipherBytes, 0, payload, aes.IV.Length, cipherBytes.Length);
            return Convert.ToBase64String(payload);
        }

        private static string DecryptAes(string cipherText, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Encryption password is required when AES is enabled.");
            }

            var payload = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = BuildAesKey(password);

            var ivLength = aes.BlockSize / 8;
            var iv = new byte[ivLength];
            var cipherBytes = new byte[payload.Length - ivLength];

            Buffer.BlockCopy(payload, 0, iv, 0, ivLength);
            Buffer.BlockCopy(payload, ivLength, cipherBytes, 0, cipherBytes.Length);

            using var decryptor = aes.CreateDecryptor(aes.Key, iv);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

        private static byte[] BuildAesKey(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
