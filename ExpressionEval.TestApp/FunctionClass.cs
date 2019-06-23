using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEval.TestApp
{
    public class FunctionClass
    {

        public FunctionClass()
        {

        }

        //some instance data, could be another object
        //maybe a datarow or custom data object
        public double X;
        public double Y;
        public double Z;

        //method that can be called
        public double Round(double number)
        {
            return Math.Round(number);
        }
    }
}
