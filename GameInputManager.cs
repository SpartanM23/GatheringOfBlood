using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GameInputManager
{
    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[4]
    {

        "Forward",
        "Backward",
        "Left",
        "Right"
    };

    static KeyCode[] QWERTY = new KeyCode[4]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D
    };

    static KeyCode[] AZERTY = new KeyCode[4]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D
    };

    static GameInputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], AZERTY[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }
}