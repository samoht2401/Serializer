using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Serializer
{
    public abstract class Reader
    {
        protected StreamReader reader;
        protected Stack<Group> groupStack;

        public Reader(StreamReader r)
        {
            reader = r;
            groupStack = new Stack<Group>();
        }

        public virtual void Start() { }
        public virtual void End() { }

        public abstract Object Read(String fieldName, Type type);

        public bool FindAndPushGroup(String groupName)
        {
            if (findAndPushGroup(groupName))
            {
                groupStack.Push(new Group(groupName, ""));
                return true;
            }
            return false;
        }
        public Group PopGroup()
        {
            Group g = groupStack.Pop();
            popGroup(g.Name);
            return g;
        }

        protected virtual bool findAndPushGroup(String groupName) { return false; }
        protected virtual void popGroup(String groupName) { }
    }
}
