using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class VimSettings
{
    private const string _enabledName = "Vim/Enable";
    private const string _vimClientPathName = "Vim/Vim Client Path";
    private const string _fileMatchPatternName = "Vim/FileMatchPattern";

#if UNITY_EDITOR_WIN
    private const string _defaultVimClientPath =
        @"C:\Program Files\Vim\x86_64\bin\vimclientw.exe";
#elif UNITY_EDITOR_OSX
    private const string _defaultVimClientPath =
        "/Applications/Vim.app/Contents/MacOS/bin/vimclient";
#else
    private const string _defaultVimClientPath = "vimclient";
#endif

    private const string _defaultFileMatchPattern =
        "(.cs|.txt|.js|.javascript|.json|.html|.shader|.template|.proto|.xml)$";

    public static bool Enabled
    {
        get { return EditorPrefs.GetBool(_enabledName, false); }
        set { EditorPrefs.SetBool(_enabledName, value); }
    }

    public static string VimClientPath
    {
        get { return EditorPrefs.GetString(_vimClientPathName, _defaultVimClientPath); }
        set { EditorPrefs.SetString(_vimClientPathName, value); }
    }

    public static string FileMatchPattern
    {
        get { return EditorPrefs.GetString(_fileMatchPatternName, _defaultFileMatchPattern); }
        set { EditorPrefs.SetString(_fileMatchPatternName, value); }
    }

    private class VimSettingsProvider : SettingsProvider
    {
        public VimSettingsProvider() : base("Preferences/Vim", SettingsScope.User) { }

        public override void OnGUI(string searchContext)
        {
            VimSettings.OnGUI();
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new VimSettingsProvider();
        }
    }

    private static void OnGUI()
    {
        Enabled = EditorGUILayout.Toggle("Enabled", Enabled);
        VimClientPath = EditorGUILayout.TextField("Vim Client Path", VimClientPath);
        FileMatchPattern = EditorGUILayout.TextField("File Extensions", FileMatchPattern);
    }
}

