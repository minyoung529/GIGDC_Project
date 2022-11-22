using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static Dictionary<short, Action> eventDictionary = new Dictionary<short, Action>();

    public static void StartListening(short eventName, Action listener)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(short eventName, Action listener)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(short eventName)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }
}

public static class EventManager<T>
{
    private static Dictionary<short, Action<T>> eventDictionary = new Dictionary<short, Action<T>>();

    public static void StartListening(short eventName, Action<T> listener)
    {
        Action<T> thisEvent;
         
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(short eventName, Action<T> listener)
    {
        Action<T> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(short eventName, T param)
    {
        Action<T> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
    }
}

public struct EventParam
{
    public bool boolean;
    public Character character;
    public VerbType verbType;
    public Verb verb;
    public ItemObject itemObject;
}