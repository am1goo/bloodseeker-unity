using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BmxGeneratorWindow : EditorWindow
{
    private const string PP_PATH_TO_PROJECT_FILE = "pp_path_to_project_file";
    private const string PP_PATH_TO_RESULT_DIR = "pp_path_to_result_dir";
    private const string PP_PATH_TO_RESULT_FILENAME = "pp_path_to_result_filename";

    private const string PROJECT_FILENAME = "project.json";
    private const string DEFAULT_FILENAME = "generated.bmx";

    private TextField _pathToProjectField;
    private TextField _pathToResultField;
    private Button _generateButton;
    private Label _logs;

    [MenuItem("Bloodseeker/*.bmx generator")]
    public static void ShowWindow()
    {
        BmxGeneratorWindow wnd = GetWindow<BmxGeneratorWindow>();
        wnd.titleContent = new GUIContent("*.bmx generator");
    }

    private void Update()
    {
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

        _generateButton = new Button(OnGenerateClick);
        _generateButton.text = "Generate";
        root.Add(_generateButton);

        _logs = new Label("");
        root.Add(_logs);

        var pathToProjectFile = EditorPrefs.GetString(PP_PATH_TO_PROJECT_FILE, string.Empty);
        OnPathToProjectChanged(pathToProjectFile);

        var pathToResultDir = EditorPrefs.GetString(PP_PATH_TO_RESULT_DIR, string.Empty);
        var pathToResultFilename = EditorPrefs.GetString(PP_PATH_TO_RESULT_FILENAME, string.Empty);
        var pathToResultFile = Path.Combine(pathToResultDir, pathToResultFilename);
        OnPathToResultChanged(pathToResultFile);
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

    private void OnGenerateClick()
    {
        ClearLog();

        var pathToProjectFileInfo = new FileInfo(_pathToProjectField.value);
        if (!pathToProjectFileInfo.Exists)
        {
            AddLogInfo($"{PROJECT_FILENAME} doesn't exists at path {pathToProjectFileInfo.FullName}");
            return;
        }

        var pathToResultFileInfo = new FileInfo(_pathToResultField.value);
        AddLogInfo($"Prepare to save to {pathToResultFileInfo.FullName}");
        //TODO: invoke generator here

        int retCode = UnityEngine.Random.Range(0, 2);
        if (retCode != 0)
        {
            AddLogError("Failed!");
            return;
        }
        AddLogInfo("Success!");
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

    private void AddLog(string text, string prefix)
    {
        if (_logs.text.Length > 0)
            _logs.text += Environment.NewLine;

        _logs.text = $"[{prefix}] {text}";
    }
}
