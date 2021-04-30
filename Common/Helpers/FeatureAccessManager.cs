using System;
using System.Collections.Generic;
using System.IO;

namespace Common.Helpers
{
    public class FeatureAccessManager
    {
        public const string FlatOvelFeature = "DandR_FEAT_SHOW_FLAT_OVAL";
        public const string SelectLighModelFeature = "DandR_FEAT_SELECT_LIGHT_MODEL";
        public const string CopyMultipleFeature = "DandR_FEAT_COPY_MULTIPLE";


        private static readonly HashSet<string> FeaturesLockFileMap = new HashSet<string>
        {
            FlatOvelFeature,
            SelectLighModelFeature,
            CopyMultipleFeature,
        };


        public static bool FeatureAvailable(string featureKey)
        {
            if (FeaturesLockFileMap.Contains(featureKey))
            {
                return !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(featureKey, EnvironmentVariableTarget.User)) || LockFileExists(featureKey);
            }

            return false;
        }

        private static bool LockFileExists(string filename)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{filename}.lock");
            return File.Exists(file);
        }
    }
}