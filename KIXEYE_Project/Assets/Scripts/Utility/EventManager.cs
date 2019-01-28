using System;
using System.Collections.Generic;
using UnityEngine;

namespace LC.Ultility
{
    public class EventManager : MonoSingletonExt<EventManager>
    {
        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(this);
        }

        protected void OnDestroy()
        {
            // reset this static var to null if it's the singleton instance
            ClearAllListener();
            //mInstance = null;
        }

        #region Fields

        /// Store all "listener"
        private Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();

        #endregion Fields

        #region Add Listeners, Post events, Remove listener

        /// <summary>
        /// Register to listen for eventID
        /// </summary>
        /// <param name="eventID">EventID that object want to listen</param>
        /// <param name="callback">Callback will be invoked when this eventID be raised</para	m>
        public void RegisterListener(EventID eventID, Action<object> callback)
        {
            // checking params
            Log.Assert(callback != null, "AddListener, event {0}, callback = null !!", eventID.ToString());
            Log.Assert(eventID != EventID.None, "RegisterListener, event = None !!");

            // check if listener exist in distionary
            if (IsContainsEventID(eventID))
            {
                // add callback to our collection
                _listeners[eventID] += callback;
            }
            else
            {
                // add new key-value pair
                _listeners.Add(eventID, null);
                _listeners[eventID] += callback;
            }
        }

        /// <summary>
        /// Posts the event. This will notify all listener that register for this event
        /// </summary>
        /// <param name="eventID">EventID.</param>
        /// <param name="sender">Sender, in some case, the Listener will need to know who send this message.</param>
        /// <param name="param">Parameter. Can be anything (struct, class ...), Listener will make a cast to get the data</param>
        public void PostEvent(EventID eventID, object param = null)
        {
            if (!IsContainsEventID(eventID))
            {
                Log.InfoFormat("No listeners for this event : {0}", eventID);
                return;
            }

            // posting event
            var callbacks = _listeners[eventID];
            // if there's no listener remain, then do nothing
            if (callbacks != null)
            {
                callbacks(param);
            }
            else
            {
                Log.InfoFormat("PostEvent {0}, but no listener remain, Remove this key", eventID);
                _listeners.Remove(eventID);
            }
        }

        /// <summary>
        /// Removes the listener. Use to Unregister listener
        /// </summary>
        /// <param name="eventID">EventID.</param>
        /// <param name="callback">Callback.</param>
        public void RemoveListener(EventID eventID, Action<object> callback)
        {
            // checking params
            Log.Assert(callback != null, "RemoveListener, event {0}, callback = null !!", eventID.ToString());
            Log.Assert(eventID != EventID.None, "AddListener, event = None !!");

            if (_listeners.ContainsKey(eventID))
            {
                _listeners[eventID] -= callback;
            }
            else
            {
                Log.Warning(false, "RemoveListener, not found key : " + eventID);
            }
        }

        /// <summary>
        /// Clears all the listener.
        /// </summary>
        public void ClearAllListener()
        {
            _listeners.Clear();
        }

        public bool IsContainsEventID(EventID eventID)
        {
            return _listeners.ContainsKey(eventID);
        }

        #endregion Add Listeners, Post events, Remove listener
    }
}

#region Extension class

namespace LC.Ultility
{
    /// <summary>
    /// Delare some "shortcut" for using EventDispatcher easier
    /// </summary>
    public static class EventManagerExtension
    {
        /// /// <summary>
        /// Use for registering with EventsManager
        /// </summary>
        /// "this MonoBehaviour listener" for all MonoBehaviour can access
        public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
        {
            EventManager.Instance.RegisterListener(eventID, callback);
        }

        /// <summary>
        /// Post event with param
        /// </summary>
        public static void PostEvent(this MonoBehaviour listener, EventID eventID, object param)
        {
            EventManager.Instance.PostEvent(eventID, param);
        }

        /// <summary>
        /// Post event with no param (param = null)
        /// </summary>
        public static void PostEvent(this MonoBehaviour sender, EventID eventID)
        {
            EventManager.Instance.PostEvent(eventID, null);
        }
    }
}

#endregion Extension class