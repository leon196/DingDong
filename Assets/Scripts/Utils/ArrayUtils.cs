using UnityEngine;
using System.Collections;

public class ArrayUtils
{
	static public Type[] Shuffle<Type> (Type[] array)
	{
	 for (int i = array.Length - 1; i > 0; i--)
	 {
	     int j = (int)Mathf.Floor(Random.Range(0f, 1f) * (i + 1));
	     Type temp = array[i];
	     array[i] = array[j];
	     array[j] = temp;
	 }
	 return array;
	}
}