using System;
using Microsoft.Extensions.Configuration;
using Refactoring.Conway.Common;

namespace Refactoring.Conway
{
    public static class Utilities
    {
        public static int GetUserInputInt(string prompt, int? defaultValue)
        {
            string userInput = string.Empty;
            int intOutput;
            Console.WriteLine(prompt);
            if (defaultValue.HasValue)
            {
                Console.WriteLine(Localization.DefaultValueLabel, defaultValue);
            }
            while (!int.TryParse(userInput, out intOutput))
            {
                userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput) && defaultValue.HasValue)
                    return defaultValue.Value;
            }
            return intOutput;
        }

        public static int ConfigValueOrDefault(IConfigurationRoot configuration, ApplicationSettingNames appSettingName, int defaultValue)
        {
            if (int.TryParse(configuration[appSettingName.ToString()], out int returnValue))
            {
                return returnValue;
            }
            return defaultValue;
        }

        public static string ConfigValueOrDefault(IConfigurationRoot configuration, ApplicationSettingNames appSettingName, string defaultValue)
        {
            if (!string.IsNullOrEmpty(configuration[appSettingName.ToString()]))
            {
                return configuration[appSettingName.ToString()];
            }
            return defaultValue;
        }
    }
}