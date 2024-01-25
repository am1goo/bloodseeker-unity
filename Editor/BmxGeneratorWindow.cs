using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
#if UNITY_ANDROID
using UnityEditor.Android;
#endif
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class BmxGeneratorWindow : EditorWindow
{
    private const string PACKAGE_NAME = "com.am1goo.bloodseeker";

    private const string PP_PATH_TO_PROJECT_FILE = "pp_path_to_project_file";
    private const string PP_PATH_TO_RESULT_DIR = "pp_path_to_result_dir";
    private const string PP_PATH_TO_RESULT_FILENAME = "pp_path_to_result_filename";
    private const string PP_REVEAL_IN_FINDER_AFTER_GENERATION = "pp_reveal_in_finder_after_generation";

    private const string PROJECT_FILENAME = "project.json";
    private const string DEFAULT_FILENAME = "generated.bmx";

    private TextField _pathToProjectField;
    private TextField _pathToResultField;
    private Toggle _revealInFinderCheckbox;
    private Button _generateButton;
    private Button _testButton;
    private Label _logs;

    private bool _lastGenerateSuccess = false;

    [MenuItem("Bloodseeker/*.bmx generator")]
    public static void ShowWindow()
    {
        BmxGeneratorWindow wnd = GetWindow<BmxGeneratorWindow>();
        wnd.titleContent = new GUIContent("*.bmx generator");
    }

    private void Update()
    {
        var prevRevealInFinder = EditorPrefs.GetBool(PP_REVEAL_IN_FINDER_AFTER_GENERATION);
        if (prevRevealInFinder != _revealInFinderCheckbox.value)
            EditorPrefs.SetBool(PP_REVEAL_IN_FINDER_AFTER_GENERATION, _revealInFinderCheckbox.value);

        _testButton.SetEnabled(_lastGenerateSuccess);

        var generateDenied = string.IsNullOrEmpty(_pathToProjectField.value) || string.IsNullOrEmpty(_pathToResultField.value);
        var generateAllowed = !generateDenied;
        _generateButton.SetEnabled(generateAllowed);
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        Label empty = new Label(" ");
        root.Add(empty);

        Button pathToProjectButton = new Button(OnPathToProjectClick);
        pathToProjectButton.text = "Open..";

        _pathToProjectField = new TextField($"Path to {PROJECT_FILENAME}");
        _pathToProjectField.isReadOnly = true;
        _pathToProjectField.value = string.Empty;
        _pathToProjectField.Add(pathToProjectButton);
        root.Add(_pathToProjectField);

        Button pathToResultButton = new Button(OnPathToResultClick);
        pathToResultButton.text = "Select..";

        _pathToResultField = new TextField("Path to result *.bmx");
        _pathToResultField.isReadOnly = true;
        _pathToResultField.value = string.Empty;
        _pathToResultField.Add(pathToResultButton);
        root.Add(_pathToResultField);

        _revealInFinderCheckbox = new Toggle("Open file after generation");
        root.Add(_revealInFinderCheckbox);

        _generateButton = new Button(OnGenerateClick);
        _generateButton.text = "Generate";
        root.Add(_generateButton);

        _testButton = new Button(OnTestClick);
        _testButton.text = "Test";
        root.Add(_testButton);

        _logs = new Label("");
        root.Add(_logs);

        var pathToProjectFile = EditorPrefs.GetString(PP_PATH_TO_PROJECT_FILE, string.Empty);
        OnPathToProjectChanged(pathToProjectFile);

        var pathToResultDir = EditorPrefs.GetString(PP_PATH_TO_RESULT_DIR, string.Empty);
        var pathToResultFilename = EditorPrefs.GetString(PP_PATH_TO_RESULT_FILENAME, string.Empty);
        var pathToResultFile = Path.Combine(pathToResultDir, pathToResultFilename);
        OnPathToResultChanged(pathToResultFile);

        var revealInFinder = EditorPrefs.GetBool(PP_REVEAL_IN_FINDER_AFTER_GENERATION);
        OnRevealInFinderChanged(revealInFinder);
    }

    private void OnPathToProjectClick()
    {
        var filepath = EditorPrefs.GetString(PP_PATH_TO_PROJECT_FILE, Environment.CurrentDirectory);
        var path = EditorUtility.OpenFilePanelWithFilters($"Select {PROJECT_FILENAME} file", filepath, new string[] { PROJECT_FILENAME, "json" });
        if (string.IsNullOrWhiteSpace(path))
            return;

        EditorPrefs.SetString(PP_PATH_TO_PROJECT_FILE, path);
        OnPathToProjectChanged(path);
    }

    private void OnPathToProjectChanged(string path)
    {
        _pathToProjectField.value = path;
    }

    private void OnPathToResultClick()
    {
        var dirpath = EditorPrefs.GetString(PP_PATH_TO_RESULT_DIR, Environment.CurrentDirectory);
        var filename = EditorPrefs.GetString(PP_PATH_TO_RESULT_FILENAME, DEFAULT_FILENAME);
        var path = EditorUtility.SaveFilePanel("Select place to save *.bmx file", dirpath, filename, "bmx");
        if (string.IsNullOrWhiteSpace(path))
            return;

        FileInfo fi = new FileInfo(path);
        EditorPrefs.SetString(PP_PATH_TO_RESULT_DIR, fi.DirectoryName);
        EditorPrefs.SetString(PP_PATH_TO_RESULT_FILENAME, fi.Name);
        OnPathToResultChanged(path);
    }

    private void OnPathToResultChanged(string path)
    {
        _pathToResultField.value = path;
    }

    private void OnRevealInFinderChanged(bool value)
    {
        _revealInFinderCheckbox.value = value;
    }

    private void OnGenerateClick()
    {
        _lastGenerateSuccess = false;
        ClearLog();

        var pathToProjectFileInfo = new FileInfo(_pathToProjectField.value);
        if (!pathToProjectFileInfo.Exists)
        {
            AddLogInfo($"{PROJECT_FILENAME} doesn't exists at path {pathToProjectFileInfo.FullName}");
            return;
        }

        var pathToResultFileInfo = new FileInfo(_pathToResultField.value);
        AddLogInfo($"Prepare to save to {pathToResultFileInfo.FullName}");

        var exec = ExecGenerate(pathToProjectFileInfo.DirectoryName, pathToResultFileInfo.FullName);
        if (exec.retCode != 0)
        {
            AddLogError(exec.output);
            AddLogError("Failed!");
            return;
        }

        _lastGenerateSuccess = true;
        AddLogInfo(exec.output);
        AddLogInfo("Success!");
        
        if (!_revealInFinderCheckbox.value)
            return;

        EditorUtility.RevealInFinder(pathToResultFileInfo.FullName);
    }

    private void OnTestClick()
    {
        var pathToProjectFileInfo = new FileInfo(_pathToProjectField.value);
        if (!pathToProjectFileInfo.Exists)
        {
            ShowDialogError($"{PROJECT_FILENAME} doesn't exists at path {pathToProjectFileInfo.FullName}");
            return;
        }

        var bmxFileInfo = new FileInfo(_pathToResultField.value);
        var exec = ExecTest(pathToProjectFileInfo.DirectoryName, bmxFileInfo.FullName);
        if (exec.retCode != 0)
        {
            ShowDialogError(exec.output);
            return;
        }

        ShowDialogInfo(exec.output);
    }

    private void ClearLog()
    {
        _logs.text = string.Empty;
    }

    private void AddLogInfo(string text)
    {
        AddLog(text, "INFO");
    }

    private void AddLogError(string text)
    {
        AddLog(text, "ERROR");
    }

    private void ShowDialogInfo(string text)
    {
        ShowDialog(text, "INFO");
    }

    private void ShowDialogError(string text)
    {
        ShowDialog(text, "ERROR");
    }

    private void ShowDialog(string text, string prefix)
    {
        EditorUtility.DisplayDialog(prefix, text, "Okay");
    }

    private void AddLog(string text, string prefix)
    {
        if (_logs.text.Length > 0)
            _logs.text += Environment.NewLine;

        _logs.text += $"[{prefix}] {text}";
    }

    private static (int retCode, string output) ExecGenerate(string pathToProject, string pathToResult)
    {
        var toolsPath = GetToolPath();
        if (string.IsNullOrWhiteSpace(toolsPath))
            return (-1, "can't file tools path");

        var javaPath = GetJavaPath();
        var generatorPath = Path.Combine(toolsPath, "Editor/Tools/generator.jar");
        return Exec(javaPath, "-jar", generatorPath, "-c", pathToProject, pathToResult);
    }

    private static (int retCode, string output) ExecTest(string pathToProject, string pathToBmxFile)
    {
        var toolsPath = GetToolPath();
        if (string.IsNullOrWhiteSpace(toolsPath))
            return (-1, "can't file tools path");

        var javaPath = GetJavaPath();
        var generatorPath = Path.Combine(toolsPath, "Editor/Tools/generator.jar");
        return Exec(javaPath, "-jar", generatorPath, "-t", pathToProject, pathToBmxFile);
    }

    private static (int retCode, string output) Exec(string fileName, params string[] args)
    {
        try
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = string.Join(" ", args);
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            bool started = proc.Start();
            if (!started)
                return (-1, $"process {fileName} cannot be started");

            proc.WaitForExit();
            int retCode = proc.ExitCode;
            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();
            return (retCode, string.Join(Environment.NewLine, output, error).Trim());
        }
        catch (Exception ex)
        {
            return (-1, ex.ToString());
        }
    }

    private static string GetJavaPath()
    {
        var jdkPath = Environment.GetEnvironmentVariable("JAVA_HOME");
#if BLOODSEEKER_USE_UNITY_JDK
#if UNITY_ANDROID
        if (string.IsNullOrWhiteSpace(jdkPath))
            jdkPath = AndroidExternalToolsSettings.jdkRootPath;
#endif
#endif
        if (string.IsNullOrWhiteSpace(jdkPath))
            return "java";
        else
            return Path.Combine(jdkPath, "bin", "java.exe");
    }

    private static string GetToolPath()
    {
        if (TryGetToolsPathFromPackages(out var pathFromPackages))
        {
            return pathFromPackages;
        }
        else if (TryGetToolPathFromSources(out var pathFromSources))
        {
            return pathFromSources;
        }
        else
        {
            return "";
        }
    }

    private static bool TryGetToolsPathFromPackages(out string result)
    {
        var req = Client.List();
        while (!req.IsCompleted)
            System.Threading.Thread.Sleep(10);

        var packages = req.Result;
        if (packages == null)
        {
            result = default;
            return false;
        }

        foreach (var package in packages)
        {
            if (package.name == PACKAGE_NAME)
            {
                result = package.resolvedPath;
                return true;
            }
        }

        result = default;
        return false;
    }

    private static bool TryGetToolPathFromSources(out string result)
    {
        var path = Path.GetFullPath("Assets/Package");
        var di = new DirectoryInfo(path);
        if (!di.Exists)
        {
            result = default;
            return false;
        }

        result = path;
        return true;
    }
}
