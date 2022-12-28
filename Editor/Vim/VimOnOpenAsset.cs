using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class VimOnOpenAsset : MonoBehaviour
{
    private static Process _process = null;
#if ENABLE_VSTU
    [MenuItem("Vim/Regenerate Project Files")]
    public static void RegenerateProjectFiles() {
      SyntaxTree.VisualStudio.Unity.Bridge.ProjectFilesGenerator.GenerateProject();
    }
#endif

    [OnOpenAssetAttribute()]
    public static bool OnOpenAsset(int instanceID, int line, int column)
    {
        if (!VimSettings.Enabled)
        {
            return false;
        }

        UnityEngine.Object selectedObject = EditorUtility.InstanceIDToObject(instanceID);
        string filePath = AssetDatabase.GetAssetPath(selectedObject);

        if (!Regex.Match(filePath, VimSettings.FileMatchPattern, RegexOptions.IgnoreCase).Success)
        {
            return false;
        }

        if (line == -1)
        {
            line = 0;
        }
        if (column == -1)
        {
            column = 0;
        }
        string projectPath = System.IO.Path.GetDirectoryName(UnityEngine.Application.dataPath);
        string completeFilePath = Path.Combine(projectPath, filePath);

        try
        {
            string command = $"nvim +'call cursor({line},{column})' \"{completeFilePath}\"";
            _process = Process.Start(@$"{VimSettings.VimClientPath}", command);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Could not start Vim. Check your Preferences. {ex.Message}");
        }
        return true;
    }

    private static void ErrorReceived(object sender, DataReceivedEventArgs e)
    {
        UnityEngine.Debug.LogError(e.Data);
    }

    private static void DataReceived(object sender, DataReceivedEventArgs e)
    {
        UnityEngine.Debug.Log(e.Data);

    }

    [OnOpenAssetAttribute()]
    public static bool OnOpenAsset(int instanceID, int line) {
      return OnOpenAsset(instanceID, line, -1);
    }

}
