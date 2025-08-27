using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script helps improve performance and memory usage when working with Coroutines
public static class Yielders
{

    /*
     Caching WaitForSeconds Objects:
         _timeInterval dictionary is used to cache 'WaitForSeconds' objects.
        WaitForSeconds is a class in Unity used within coroutines to wait for a specific duration.
        By caching these objects, the script aims to avoid creating new instances every time you need a wait in your coroutines
     */
    static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);
    static Dictionary<float, WaitForSecondsRealtime> _timeIntervalRealtime = new Dictionary<float, WaitForSecondsRealtime>(100);

    //Pauses the coroutine until the current frame has finished rendering.
    static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame EndOfFrame
    {
        get { return _endOfFrame; }
    }

    //Pauses a coroutine until the next FixedUpdate is called. This is useful for tasks that need to be synchronized with physics updates.
    static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate FixedUpdate
    {
        get { return _fixedUpdate; }
    }

    public static WaitForSeconds Get(float seconds)
    {
        if (!_timeInterval.ContainsKey(seconds))
            _timeInterval.Add(seconds, new WaitForSeconds(seconds));
        return _timeInterval[seconds];
    }
    
    public static WaitForSecondsRealtime GetRealtime(float seconds)
    {
        if (!_timeIntervalRealtime.ContainsKey(seconds))
            _timeIntervalRealtime.Add(seconds, new WaitForSecondsRealtime(seconds));
        return _timeIntervalRealtime[seconds];
    }

    /*
    Example:
    IEnumerator MyCoroutine()
    {
        yield return Yielders.Get(2f); // Waits for 2 seconds
        Debug.Log("Two seconds have passed");

        yield return Yielders.EndOfFrame; // Waits until the end of the current frame
        Debug.Log("End of frame reached");
    }
    */

}

