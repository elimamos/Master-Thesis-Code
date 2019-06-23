using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEval.TestApp
{
    class Parameter
    {
        public string name { get; }
        public Type type { get;  }
        private string value;
       public Parameter(string Name, Type Type, string Value) {
            name = Name;
            type = Type;
            value = Value;
         }
        public void setNewValue(string Value) {
            value = Value;
        }
        public T getValue<T>(){
            T valueTyped = default(T);
            try
            {
                valueTyped = (T)Convert.ChangeType(value, typeof(T));
            }
            catch {
                Console.WriteLine("Unable to cast parameter value to given type!");
            }
            return valueTyped;
        }

    }
}
