// Contain declaration for Conditional attribute
using System.Diagnostics;
using UnityEngine;

// Prevent Type conflict with System.Diagnostics.Log
using Debug = UnityEngine.Debug;

namespace LC.Ultility
{
    /// <summary>
    /// Debug log (only run on Editor)
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Default debug message
        /// </summary>
        /// <param name="message"></param>
        [Conditional("ENABLE_LOG")]
        public static void Info(object message)
        {
#if UNITY_EDITOR
            Debug.Log("Info : " + message);
#endif
        }

        /// <summary>
        /// Default debug message
        /// </summary>
        /// <param name="message"></param>
        [Conditional("ENABLE_LOG")]
        public static void InfoFormat(object message, params object[] args)
        {
#if UNITY_EDITOR
            Debug.LogFormat("InfoFormat: " + message, args);
#endif
        }

        [Conditional("ENABLE_LOG")]
        public static void Warning(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Warning : " + message);
#endif
        }

        [Conditional("ENABLE_LOG")]
        public static void Warning(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Warning : " + message, context);
#endif
        }

        [Conditional("ENABLE_LOG")]
        public static void Error(object message)
        {
#if UNITY_EDITOR
            Debug.LogError("Error : " + message);
#endif
        }

        [Conditional("DEBUG")]
        public static void Warning(bool condition, object message)
        {
            if (!condition) Debug.LogWarning(message);
        }

        [Conditional("DEBUG")]
        public static void Warning(bool condition, object message, Object context)
        {
            if (!condition) Debug.LogWarning(message, context);
        }

        [Conditional("DEBUG")]
        public static void Warning(bool condition, Object context, string format, params object[] args)
        {
            if (!condition) Debug.LogWarning(string.Format(format, args), context);
        }

        //---------------------------------------------
        //------------- Assert ------------------------

        /// Thown an exception if condition = false
        [Conditional("ASSERT")]
        public static void Assert(bool condition)
        {
            if (!condition) throw new UnityException();
        }

        /// Thown an exception if condition = false, show message on console's log
        [Conditional("ASSERT")]
        public static void Assert(bool condition, string message)
        {
            if (!condition) throw new UnityException(message);
        }

        /// Thown an exception if condition = false, show message on console's log
        [Conditional("ASSERT")]
        public static void Assert(bool condition, string format, params object[] args)
        {
            if (!condition) throw new UnityException(string.Format(format, args));
        }
    }
}