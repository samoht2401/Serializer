using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Serializer
{
    public abstract class Writer
    {
        protected StreamWriter writer;
        protected Stack<Group> groupStack;

        public Writer(StreamWriter w)
        {
            writer = w;
            groupStack = new Stack<Group>();
        }

        public virtual void Start() { }
        public virtual void End() { }

        public abstract void Write(Field<Object> val);

        public void PushGroup(Group group)
        {
            pushGroup(group);
            groupStack.Push(group);
        }
        public Group PopGroup()
        {
            Group g = groupStack.Pop();
            popGroup(g);
            return g;
        }

        protected virtual void pushGroup(Group group) { }
        protected virtual void popGroup(Group group) { }
    }
}
