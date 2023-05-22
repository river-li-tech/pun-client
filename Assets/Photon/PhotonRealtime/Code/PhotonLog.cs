
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System;
    using System.Collections;
	using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY
    using UnityEngine;
    using Debug = UnityEngine.Debug;
    #endif
    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif


    /// <summary>
    /// This static class defines some useful extension methods for several existing classes (e.g. Vector3, float and others).
    /// </summary>
    public static class PhotonLog
    {
        public static string GetFilePath()
        {
            Process pro = Process.GetCurrentProcess();
            int pid = pro.Id;
            return Path.Combine(Application.persistentDataPath, string.Format("pun-client-{0}.txt", pid));
        }

        public static void LogFormat(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            UnityEngine.Debug.Log(msg);
            LogToFile(msg);
        }
        
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(msg);
            LogToFile(msg);
        }

        private static void LogToFile(string msg)
        {
            try
            {
                string filePath = GetFilePath();
                string tmsg = string.Format("[{0}] {1}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"), msg);
                File.AppendAllText(filePath, tmsg);
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
        }
    }
}

