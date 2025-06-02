using UnityEngine;
using System.Diagnostics;
using System.IO;

public class PythonRunner : MonoBehaviour
{
    public void RunPythonScript()
    {
        string pythonExe = @"C:\Program Files\Python312\python.exe";
        string scriptPath = Path.Combine(Application.dataPath, "Scripts/api_excel.py");

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonExe;
        start.Arguments = $"\"{scriptPath}\"";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true;
        start.CreateNoWindow = true;

        using (Process process = Process.Start(start))
        {
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            UnityEngine.Debug.Log("Çıktı:\n" + output);
            if (!string.IsNullOrEmpty(error))
                UnityEngine.Debug.LogError("Hata:\n" + error);
        }
    }
}
