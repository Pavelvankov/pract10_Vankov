using System;
using System.IO;
using System.Windows;

namespace PriemPatient
{
    public static class ThemeHelper
    {
        private static readonly string[] ThemeList =
        {
            "/Styles/Colors/LightTheme.xaml",
            "/Styles/Colors/DarkTheme.xaml"
        };

        private static readonly string SaveFile = "theme.txt";

        public static void ApplySaved()
        {
            string theme = ThemeList[0];

            try
            {
                if (File.Exists(SaveFile))
                {
                    string saved = File.ReadAllText(SaveFile).Trim();
                    if (saved == ThemeList[0] || saved == ThemeList[1])
                        theme = saved;
                }
            }
            catch
            {
            }

            Apply(theme);
        }

        public static void Toggle()
        {
            string current = GetCurrentTheme();
            string next = current == ThemeList[0] ? ThemeList[1] : ThemeList[0];
            Apply(next);
        }

        private static string GetCurrentTheme()
        {
            for (int i = 0; i < Application.Current.Resources.MergedDictionaries.Count; i++)
            {
                var d = Application.Current.Resources.MergedDictionaries[i];
                if (d.Source == null) continue;

                string s = d.Source.OriginalString;
                if (s.EndsWith(ThemeList[0], StringComparison.OrdinalIgnoreCase)) return ThemeList[0];
                if (s.EndsWith(ThemeList[1], StringComparison.OrdinalIgnoreCase)) return ThemeList[1];
            }

            return ThemeList[0];
        }

        public static void Apply(string themePath)
        {
            if (themePath != ThemeList[0] && themePath != ThemeList[1])
                themePath = ThemeList[0];

            var newDict = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            ResourceDictionary oldDict = null;

            for (int i = 0; i < Application.Current.Resources.MergedDictionaries.Count; i++)
            {
                var d = Application.Current.Resources.MergedDictionaries[i];
                if (d.Source == null) continue;

                string s = d.Source.OriginalString;
                if (s.EndsWith("/Styles/Colors/LightTheme.xaml", StringComparison.OrdinalIgnoreCase) ||
                    s.EndsWith("/Styles/Colors/DarkTheme.xaml", StringComparison.OrdinalIgnoreCase))
                {
                    oldDict = d;
                    break;
                }
            }

            if (oldDict != null)
            {
                int index = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                Application.Current.Resources.MergedDictionaries[index] = newDict;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Insert(0, newDict);
            }

            try
            {
                File.WriteAllText(SaveFile, themePath);
            }
            catch
            {
            }
        }
    }
}
