using System.Xml;
using System;
using System.Collections;
using System.Reflection;
using System.Security;
using UnityEngine;

public class VOUtil
{
    public static XmlElement MakeXML<T>(T vo, XmlDocument Root)
    {
        Type t = typeof(T);
        FieldInfo[] infoList = t.GetFields();
        PropertyInfo[] PropertyInfoList = t.GetProperties();
        XmlElement element = Root.CreateElement(t.Name);

        for (int i = 0; i < infoList.Length; i++)
        {
            // object value=null;
            // infoList[i].GetValue(value);
            // string value = PropertyInfoList[i].ToString();
            string value = infoList[i].GetValue(vo).ToString();
            element.SetAttribute(infoList[i].Name, infoList[i].GetValue(vo).ToString());
        }
        return element;
    }
    
    public static T MakeVO<T>(T vo, XmlElement xml)
    {

        //IEnumerator enumerator = xml.Attributes.Keys.GetEnumerator();
        Type t = typeof(T);

        FieldInfo[] infoList = t.GetFields();
        for (int i = 0; i < infoList.Length; i++)
        {
            SetValue(vo, infoList[i], xml.GetAttribute(infoList[i].Name));
        }
        return vo;
    }

    public static object GetValue(object tar, string name)
    {
        return tar.GetType().GetField(name).GetValue(tar);
    }



    public static void SetValue(object tar, FieldInfo fInfo, object value)
    {
        // FieldInfo fInfo = tar.GetType().GetField(name);
        if (fInfo == null || value == null || (string)value == "")
        {
            //Debug.Log("SetValue：" + tar + "找不到这个字段" + name);
            return;
        }
        try
        {
            Type type = fInfo.FieldType;
            string[] arg;
            string valueStr = value + "";
            if (type == typeof(Int32))
            {
                value = string.IsNullOrEmpty(valueStr) ? 0 : int.Parse(valueStr);
            }
            else if (type == typeof(Int64))
            {
                value = string.IsNullOrEmpty(valueStr) ? 0 : long.Parse(valueStr);
            }
            else if (type == typeof(float))
            {
                value = string.IsNullOrEmpty(valueStr) ? 0 : float.Parse(valueStr);
            }
            else if (type == typeof(double))
            {
                value = string.IsNullOrEmpty(valueStr) ? 0 : double.Parse(valueStr);
            }
            else if (type == typeof(bool))
            {
                //var b = false;
                if (valueStr == "False")
                {
                    value = false;
                }
                else
                {
                    value = true;
                }
                //value = (!string.IsNullOrEmpty(valueStr));
            }
            else if (type == typeof(Vector2))
            {
                arg = valueStr.Split(',');
                value = new Vector2(float.Parse(arg[0]), float.Parse(arg[1]));
            }
            else if (type == typeof(Vector3))
            {
                string sub = valueStr.Substring(1, valueStr.Length - 2);
                arg = (sub).Split(',');
                value = new Vector3(float.Parse(arg[0]), float.Parse(arg[1]), float.Parse(arg[2]));
            }
            else if (type == typeof(Vector4))
            {
                value = GetVector4(valueStr);
            }
            else if (type == typeof(Quaternion))
            {
                arg = (valueStr).Split(',');
                value = new Quaternion(float.Parse(arg[0]), float.Parse(arg[1]), float.Parse(arg[2]), float.Parse(arg[3]));
            }
            else if (type == typeof(int[]))
            {
                if (string.IsNullOrEmpty(valueStr) == false)
                {
                    arg = (valueStr).Split(',');
                    value = Array.ConvertAll<string, int>(arg, s => int.Parse(s));
                }
                else
                {
                    value = null;
                }

            }
            else if (type == typeof(long[]))
            {
                if (string.IsNullOrEmpty(valueStr) == false)
                {
                    arg = (valueStr).Split(',');
                    value = Array.ConvertAll<string, long>(arg, s => long.Parse(s));
                }
                else
                {
                    value = null;
                }
            }
            else if (type == typeof(string[]))
            {

                arg = (valueStr).Split(',');
                value = arg;
            }
            else if (type.BaseType == typeof(System.Enum))
            {
                value = Enum.Parse(type, value.ToString());
            }
            else
            {
            }
            fInfo.SetValue(tar, value);
        }
        catch (Exception ex)
        {
            //Debug.Log("SetValue name：" + name + " type：" + tar + ex.ToString());
        }

    }

    public static void SetValue(Type tar, string name, object value)
    {
        FieldInfo fInfo = tar.GetField(name);
        if (fInfo == null)
        {
            Debug.Log("SetValue：" + tar + "找不到这个字段" + name);
            return;
        }
        fInfo.SetValue(tar, value);
    }

    private static Vector4 GetVector4(string msg)
    {
        string[] arg;
        var q = new Vector4();
        if (msg.IndexOf("{") != -1)
        {
            msg = msg.Substring(msg.IndexOf("{"), msg.IndexOf("}") - msg.IndexOf("{"));
            arg = msg.Split(',');
            for (var i = 0; i < arg.Length; i++)
            {
                var msgList2 = arg[i].Split(':');
                FieldInfo fInfo = q.GetType().GetField(msgList2[0].Trim());
                fInfo.SetValue(q, float.Parse(msgList2[1].Trim()));
            }
        }
        else
        {
            arg = msg.Split(',');
            q = new Vector4(float.Parse(arg[0]), float.Parse(arg[1]), float.Parse(arg[2]), float.Parse(arg[3]));
        }
        return q;
    }
}
