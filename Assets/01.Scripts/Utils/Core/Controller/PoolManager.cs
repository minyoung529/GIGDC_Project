using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static Dictionary<string, Stack<GameObject>> pools = new Dictionary<string, Stack<GameObject>>();
    private static Transform poolTransform;

    public static void Awake()
    {
        poolTransform = new GameObject("@Pool").transform;
    }

    public static void Push(GameObject obj)
    {
        obj.name = obj.name.Trim();
        obj.transform.position = Vector3.one;

        if (!pools.ContainsKey(obj.name))
        {
            pools.Add(obj.name, new Stack<GameObject>());
        }

        obj.transform.SetParent(poolTransform);
        pools[obj.name].Push(obj);
        obj.SetActive(false);
    }

    public static GameObject Pop(GameObject item)
    {
        item.name = item.name.Trim();
        GameObject value = null;

        if (pools.ContainsKey(item.name))
        {
            if (pools[item.name].Count > 0)
                value = pools[item.name].Pop();
        }

        value ??= GameObject.Instantiate(item, null);
        value.name = item.name;
        value.SetActive(true);
        return value;
    }

    public static GameObject Pop(string item)
    {
        GameObject value = null;

        if (pools.ContainsKey(item))
        {
            if (pools[item].Count > 0)
                value = pools[item].Pop();
        }

        value ??= Object.Instantiate(Resources.Load<GameObject>(item));
        value.name = item;
        value.SetActive(true);
        return value;
    }
}
