using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iPaxStudio.RubbyThief
{
    /// <summary>
    /// Help to serialize objects GSData -> Object and Obj -> GSData
    /// </summary>
    public class GSDataHelpers
    {

        /** Converts a serializable object into GameSparks compatible GSData. */
        public static GSRequestData ObjectToGSData(object obj)
        {
            GSRequestData gsData = new GSRequestData();
            gsData.AddString("type", obj.GetType().ToString());

            foreach (var field in obj.GetType().GetFields())
            {
                if (!field.IsNotSerialized && field.GetValue(obj) != null)
                {
                    if (field.FieldType == typeof(string))
                    {
                        gsData.AddString(field.Name, field.GetValue(obj).ToString());
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        gsData.AddBoolean(field.Name, bool.Parse(field.GetValue(obj).ToString()));
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        gsData.AddNumber(field.Name, (int)Convert.ToInt32(field.GetValue(obj)));
                    }
                    else if (field.FieldType == typeof(float) || field.GetValue(obj).GetType() == typeof(double))
                    {
                        gsData.AddNumber(field.Name, Double.Parse(field.GetValue(obj).ToString()));
                    }
                    else if (field.FieldType == typeof(List<string>) || field.GetValue(obj).GetType() == typeof(string[]))
                    {
                        gsData.AddStringList(field.Name, (field.FieldType == typeof(List<string>)) ? field.GetValue(obj) as List<string> : new List<string>(field.GetValue(obj) as string[]));
                    }
                    else if (field.FieldType == typeof(List<int>) || field.GetValue(obj).GetType() == typeof(int[]))
                    {
                        gsData.AddNumberList(field.Name, (field.FieldType == typeof(List<int>)) ? field.GetValue(obj) as List<int> : new List<int>(field.GetValue(obj) as int[]));
                    }
                    else if (field.FieldType == typeof(List<float>) || field.GetValue(obj).GetType() == typeof(float[]))
                    {
                        gsData.AddNumberList(field.Name, (field.FieldType == typeof(List<float>)) ? field.GetValue(obj) as List<float> : new List<float>(field.GetValue(obj) as float[]));
                    }
                    else if (field.FieldType == typeof(DateTime))
                    {
                        gsData.AddDate(field.Name, (DateTime)field.GetValue(obj));
                    }
                    else if (field.FieldType.IsClass && !field.FieldType.IsGenericType)
                    {
                        gsData.AddObject(field.Name, ObjectToGSData(field.GetValue(obj)));
                    }
                    else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>) || field.FieldType.IsArray)
                    {
                        List<GSData> gsDataList = new List<GSData>();
                        foreach (var elem in field.GetValue(obj) as IList)
                        {
                            gsDataList.Add(ObjectToGSData(elem));
                        }
                        gsData.AddObjectList(field.Name, gsDataList);
                    }
                }
            }

            Debug.LogWarning(gsData.JSON);
            return gsData;
        }

        /** Converts GSData to the corresponding class. */
        public static object GSDataToObject(GSData gsData)
        {
            Debug.LogWarning(gsData.JSON);

            //Type objType = Type.GetType(gsData.GetString("type"));
            Type objType = gsData.GetType();
            object obj = Activator.CreateInstance(objType);
            foreach (var typeField in objType.GetFields())
            {
                if (!typeField.IsNotSerialized && gsData.ContainsKey(typeField.Name))
                {
                    if (typeField.FieldType == typeof(string))
                    {
                        typeField.SetValue(obj, gsData.GetString(typeField.Name));
                    }
                    else if (typeField.FieldType == typeof(int))
                    {
                        typeField.SetValue(obj, (int)gsData.GetNumber(typeField.Name).Value);
                    }
                    else if (typeField.FieldType == typeof(float))
                    {
                        typeField.SetValue(obj, (float)gsData.GetFloat(typeField.Name).Value);
                    }
                    else if (typeField.FieldType == typeof(bool))
                    {
                        typeField.SetValue(obj, gsData.GetBoolean(typeField.Name));
                    }
                    else if (typeField.FieldType == typeof(DateTime))
                    {
                        typeField.SetValue(obj, gsData.GetDate(typeField.Name));
                    }
                    else if ((typeField.FieldType == typeof(List<string>) || typeField.FieldType == typeof(string[])))
                    {
                        typeField.SetValue(obj, (typeField.FieldType == typeof(List<string>)) ? (object)gsData.GetStringList(typeField.Name) : gsData.GetStringList(typeField.Name).ToArray());
                    }
                    else if ((typeField.FieldType == typeof(List<int>) || typeField.FieldType == typeof(int[])))
                    {
                        typeField.SetValue(obj, (typeField.FieldType == typeof(List<int>)) ? (object)gsData.GetIntList(typeField.Name) : gsData.GetIntList(typeField.Name).ToArray());
                    }
                    else if ((typeField.FieldType == typeof(List<float>) || typeField.FieldType == typeof(float[])))
                    {
                        typeField.SetValue(obj, (typeField.FieldType == typeof(List<float>)) ? (object)gsData.GetFloatList(typeField.Name) : gsData.GetFloatList(typeField.Name).ToArray());
                    }
                    else if (typeField.FieldType.IsClass && !typeField.FieldType.IsGenericType && !typeField.FieldType.IsArray)
                    {
                        typeField.SetValue(obj, GSDataToObject(gsData.GetGSData(typeField.Name)));
                    }
                    else if (!typeField.FieldType.IsArray && typeof(IList).IsAssignableFrom(typeField.FieldType))
                    {
                        IList genericList = Activator.CreateInstance(typeField.FieldType) as IList;
                        foreach (GSData gsDataElem in gsData.GetGSDataList(typeField.Name))
                        {
                            object elem = GSDataToObject(gsDataElem);
                            genericList.Add(elem);
                        }
                        typeField.SetValue(obj, genericList);
                    }
                    else if (typeField.FieldType.IsArray)
                    {
                        List<GSData> gsArrayData = gsData.GetGSDataList(typeField.Name);
                        // create a new instance of the array. The Activator class cannot do this for arrays //
                        // so this will create a new array of types inside the array, with the count of what is in the gsdata list //
                        Array newArray = Array.CreateInstance(typeField.FieldType.GetElementType(), gsArrayData.Count);
                        object[] objArray = new object[gsArrayData.Count]; // create a new array of objects where the serialized objects will be kept
                        for (int i = 0; i < gsArrayData.Count; i++)
                        {
                            objArray[i] = GSDataToObject(gsArrayData[i]); // convert the JSON data inside the list to an object
                        }
                        Array.Copy(objArray, newArray, objArray.Length); //covert the object[] to the original type
                        typeField.SetValue(obj, newArray);
                    }
                }
            }

            return obj;
        }

        /** Converts GSData to the corresponding class. */
        /// <summary>
        /// Use when don't have type
        /// </summary>
        public static object GSDataToObject(GSData gsData, Type type)
        {
            Debug.LogWarning(gsData.JSON);

            Type objType = type;
            object obj = Activator.CreateInstance(objType);
            foreach (var typeField in objType.GetFields())
            {
                if (!typeField.IsNotSerialized && gsData.ContainsKey(typeField.Name))
                {
                    if (typeField.FieldType == typeof(string))
                    {
                        typeField.SetValue(obj, gsData.GetString(typeField.Name));
                    }
                    else if (typeField.FieldType == typeof(int))
                    {
                        typeField.SetValue(obj, (int)gsData.GetNumber(typeField.Name).Value);
                    }
                    else if (typeField.FieldType == typeof(float))
                    {
                        typeField.SetValue(obj, (float)gsData.GetFloat(typeField.Name).Value);
                    }
                    else if (typeField.FieldType == typeof(bool))
                    {
                        typeField.SetValue(obj, gsData.GetBoolean(typeField.Name));
                    }
                    else if (typeField.FieldType == typeof(DateTime))
                    {
                        typeField.SetValue(obj, gsData.GetDate(typeField.Name));
                    }
                    else if ((typeField.FieldType == typeof(List<string>) || typeField.FieldType == typeof(string[])))
                    {
                        typeField.SetValue(obj, (typeField.FieldType == typeof(List<string>)) ? (object)gsData.GetStringList(typeField.Name) : gsData.GetStringList(typeField.Name).ToArray());
                    }
                    else if ((typeField.FieldType == typeof(List<int>) || typeField.FieldType == typeof(int[])))
                    {
                        typeField.SetValue(obj, (typeField.FieldType == typeof(List<int>)) ? (object)gsData.GetIntList(typeField.Name) : gsData.GetIntList(typeField.Name).ToArray());
                    }
                    else if ((typeField.FieldType == typeof(List<float>) || typeField.FieldType == typeof(float[])))
                    {
                        typeField.SetValue(obj, (typeField.FieldType == typeof(List<float>)) ? (object)gsData.GetFloatList(typeField.Name) : gsData.GetFloatList(typeField.Name).ToArray());
                    }
                    else if (typeField.FieldType.IsClass && !typeField.FieldType.IsGenericType && !typeField.FieldType.IsArray)
                    {
                        typeField.SetValue(obj, GSDataToObject(gsData.GetGSData(typeField.Name)));
                    }
                    else if (!typeField.FieldType.IsArray && typeof(IList).IsAssignableFrom(typeField.FieldType))
                    {
                        IList genericList = Activator.CreateInstance(typeField.FieldType) as IList;
                        foreach (GSData gsDataElem in gsData.GetGSDataList(typeField.Name))
                        {
                            object elem = GSDataToObject(gsDataElem);
                            genericList.Add(elem);
                        }
                        typeField.SetValue(obj, genericList);
                    }
                    else if (typeField.FieldType.IsArray)
                    {
                        List<GSData> gsArrayData = gsData.GetGSDataList(typeField.Name);
                        // create a new instance of the array. The Activator class cannot do this for arrays //
                        // so this will create a new array of types inside the array, with the count of what is in the gsdata list //
                        Array newArray = Array.CreateInstance(typeField.FieldType.GetElementType(), gsArrayData.Count);
                        object[] objArray = new object[gsArrayData.Count]; // create a new array of objects where the serialized objects will be kept
                        for (int i = 0; i < gsArrayData.Count; i++)
                        {
                            objArray[i] = GSDataToObject(gsArrayData[i]); // convert the JSON data inside the list to an object
                        }
                        Array.Copy(objArray, newArray, objArray.Length); //covert the object[] to the original type
                        typeField.SetValue(obj, newArray);
                    }
                }
            }

            return obj;
        }

        /** Returns the size of the data in bytes. */
        public static int SizeOfGSData(GSData data)
        {
            return data.JSON.Length * sizeof(char);
        }
    }
}