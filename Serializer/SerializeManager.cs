using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Serializer
{
    public class SerializeManager
    {
        MethodInfo serializeMethod = null;
        MethodInfo unserializeMethod = null;

        public SerializeManager()
        {
            serializeMethod = this.GetType().GetMethod("serialize", BindingFlags.NonPublic | BindingFlags.Instance);
            unserializeMethod = this.GetType().GetMethod("unserialize", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public void Serialize<T>(T obj, String name, Writer writer)
        {
            writer.Start();
            writer.PushGroup(new Group(name, ""));
            serialize<T>(obj, writer);
            writer.PopGroup();
            writer.End();
        }

        private void serialize<T>(T obj, Writer writer)
        {
            if (obj == null)
                return;
            Type ty = obj.GetType();
            FieldInfo[] fields = ty.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                ToSerializeAttribute[] attris = (ToSerializeAttribute[])field.GetCustomAttributes(typeof(ToSerializeAttribute), false);
                if (attris.Length == 0)
                    continue;
                writeToWriter(new Field<Object>(attris[0].SpecialName == null ? field.Name : attris[0].SpecialName, field.GetValue(obj), attris[0].Commentaire), writer);
            }
        }

        private void writeToWriter(Field<Object> field, Writer writer)
        {
            if (field == null)
                return;
            if (field.Value == null)
            {
                writer.Write(field);
                return;
            }
            Type type = field.Value.GetType(); ;
            if (type.IsPrimitive || type.FullName == "System.String")
                writer.Write(field);
            else if (type.GetInterface("ICollection") != null)
            {
                writer.PushGroup(new Group(field.Name, field.Commentaire));
                foreach (Object o in (ICollection)field.Value)
                    writeToWriter(new Field<Object>(o.GetType().Name, o), writer);
                writer.PopGroup();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<Object, Object>).GetGenericTypeDefinition())
            {
                writeToWriter(new Field<Object>("Key", type.GetProperty("Key").GetValue(field.Value, null)), writer);
                writeToWriter(new Field<Object>("Value", type.GetProperty("Value").GetValue(field.Value, null)), writer);
            }
            else// Rappel Serialize<T> si la propriété n'est pas un type primitif.
            {
                writer.PushGroup(new Group(field.Name, field.Commentaire));
                serializeMethod.MakeGenericMethod(type).Invoke(this, new Object[] { field.Value, writer });
                writer.PopGroup();
            }
        }

        public void Unserialize<T>(T obj, String name, Reader reader)
        {
            reader.Start();
            if (reader.FindAndPushGroup(name))
            {
                unserialize<T>(obj, reader);
                reader.PopGroup();
            }
            reader.End();
        }

        private T unserialize<T>(T obj, Reader reader)
        {
            if (obj == null)
            {
                ConstructorInfo construct = typeof(T).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
                obj = (T)construct.Invoke(new Object[] { });
            }
            Type ty = obj.GetType();
            FieldInfo[] fields = ty.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                ToSerializeAttribute[] attris = (ToSerializeAttribute[])field.GetCustomAttributes(typeof(ToSerializeAttribute), false);
                if (attris.Length == 0)
                    continue;
                Field<Object> f = new Field<Object>(attris[0].SpecialName == null ? field.Name : attris[0].SpecialName, field.GetValue(obj), attris[0].Commentaire);
                readFromReader(f, field.FieldType, reader);
                field.SetValue(obj, f.Value);
            }
            return obj;
        }

        private void readFromReader(Field<Object> field, Type type, Reader reader)
        {
            if (field == null)
                return;
            if (type.IsPrimitive || type.FullName == "System.String")
            {
                Object val = reader.Read(field.Name, type);
                if (val != null)
                {
                    if (val != "" || type.FullName == "System.String")
                        field.Value = val;
                    else
                        field.Value = null;
                }

            }
            else if (type.GetInterface("ICollection") != null)
            {
                if (reader.FindAndPushGroup(field.Name))
                {
                    Type listType = type.GetInterfaces().First(i => i.Name.StartsWith("ICollection`")).GetGenericArguments()[0];
                    field.Value = type.GetConstructor(new Type[] { }).Invoke(new Object[] { });
                    while (true)
                    {
                        Field<Object> f = new Field<Object>(listType.Name, null);
                        readFromReader(f, listType, reader);
                        if (f.Value == null)
                            break;
                        if (type.GetInterface("IDictionary") != null)
                            type.GetMethod("Add").Invoke(field.Value, new Object[] { f.Value.GetType().GetProperty("Key").GetValue(f.Value, null), f.Value.GetType().GetProperty("Value").GetValue(f.Value, null) });
                        else
                            type.GetMethod("Add").Invoke(field.Value, new Object[] { f.Value });
                    }
                    reader.PopGroup();
                }
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<Object, Object>).GetGenericTypeDefinition())
            {
                Field<Object> keyField = new Field<Object>("Key", null);
                Field<Object> valueField = new Field<Object>("Value", null);
                readFromReader(keyField, type.GetGenericArguments()[0], reader);
                readFromReader(valueField, type.GetGenericArguments()[1], reader);
                if (keyField.Value != null && valueField.Value != null)
                    field.Value = type.GetConstructor(new Type[] { type.GetGenericArguments()[0], type.GetGenericArguments()[1] }).Invoke(new Object[] { keyField.Value, valueField.Value });
            }
            else// Rappel Serialize<T> si la propriété n'est pas un type primitif.
            {
                if (reader.FindAndPushGroup(field.Name))
                {
                    field.Value = unserializeMethod.MakeGenericMethod(type).Invoke(this, new Object[] { field.Value, reader });
                    reader.PopGroup();
                }
            }
        }
    }
}
