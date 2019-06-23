using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ExpressionEval.ExpressionEvaluation;
using ExpressionEval.MethodState;

namespace ExpressionEval.TestApp
{
    public partial class MainForm : Form
    {
        List<XmlDocument> functionArrayList;
       
        public MainForm()
        {
            InitializeComponent();
            //setup some return types
            ReturnTypeCombo.Items.Add(typeof(string));
            ReturnTypeCombo.Items.Add(typeof(double));
            ReturnTypeCombo.Items.Add(typeof(DateTime));

            functionList.View = View.Details;
            functionList.FullRowSelect = true;
            functionArrayList = readAllXmlFilesFromPath("C:\\Users\\elisa\\source\\repos\\tryRunFromXML\\source_codes");
            foreach (XmlDocument xmlDoc in functionArrayList) {

                functionList.Items.Add(new ListViewItem(new string[] { xmlDoc.GetElementsByTagName("name").Item(0).InnerText, xmlDoc.GetElementsByTagName("input").Count.ToString(), xmlDoc.GetElementsByTagName("returnType").Item(0).InnerText }));
            }
            functionList.GridLines = true;
            ReturnTypeCombo.SelectedIndex = 1;
        }
        private static List<XmlDocument> readAllXmlFilesFromPath(string path) {
            List<XmlDocument> functionArrayList = new List<XmlDocument>();
            foreach (string file in Directory.GetFiles(path, "*.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                functionArrayList.Add(doc);
               
            }
            return functionArrayList;

        }
        private void EvaluateButton_Click(object sender, EventArgs e)
        {
            string result = string.Empty;

            //make sure expression is not blank
            if (ExpressionText.Text.Trim().Length > 0)
            {
                //Get the Expression and add some parameters
                string expressionText = ExpressionText.Text;
                pullParametersInFunction();
                //create new evaluator
                IExpressionEvaluator eval = new ExpressionEvaluator(ExpressionLanguage.CSharp);

                //create instance of function class with some instance data
                FunctionClass functions = new FunctionClass();

                //set the instance data
                functions.X = 10;
                functions.Y = 15;
                functions.Z = 3.3;

                object evalResult = null;

                //performance metrics
                Stopwatch stopWatch = new Stopwatch();

                stopWatch.Start();

                try
                {
                    //based on the return type evaluate the expression
                    switch (((Type)ReturnTypeCombo.SelectedItem).Name)
                    {
                        case "String":
                            evalResult = eval.Evaluate<string, FunctionClass>(ExpressionText.Text, functions);
                            break;
                        case "Double":
                            evalResult = eval.Evaluate<double, FunctionClass>(ExpressionText.Text, functions);
                            break;
                        case "DateTime":
                            evalResult = eval.Evaluate<DateTime, FunctionClass>(ExpressionText.Text, functions);
                            break;
                    }
                }
                catch (ApplicationException exp)
                {
                    evalResult = exp;
                }

                //get the result as a string for display
                result = evalResult.ToString();

                stopWatch.Stop();

                //show the elapsed time - this is the time it takes to compile and run once
                ElapsedTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " ms";
            }

            ResultLabel.Text = result.ToString();
        }

        private void LoopButton_Click(object sender, EventArgs e)
        {
            //evaluate in a loop based on return type
            switch (((Type)ReturnTypeCombo.SelectedItem).Name)
            {
                case "String":
                    CalcLoop<string>();
                    break;
                case "Double":
                    CalcLoop<double>();
                    break;
                case "DateTime":
                    CalcLoop<DateTime>();
                    break;
            }
        }

        private void CalcLoop<R>()
        {
            
            string result = string.Empty;

            //make sure expression is not empty
            if (ExpressionText.Text.Trim().Length > 0)
            {   
               
                Stopwatch stopWatch = new Stopwatch();
                IExpressionEvaluator eval = new ExpressionEvaluator(ExpressionLanguage.CSharp);

                try
                {
                    //DynamicMethodState could be serialized and saved
                    DynamicMethodState methodState = eval.GetMethodState<R, FunctionClass>(ExpressionText.Text);

                    //get delegate for saved compiled expression
                    EvalExpression<R, FunctionClass> expressionDelegate = eval.GetDelegate<R, FunctionClass>(methodState);

                    //create instance of function class
                    FunctionClass functions = new FunctionClass();

                    functions.X = 10;
                    functions.Y = 15;
                    functions.Z = 3.3;

                    object evalResult = null;

                    try
                    {

                        stopWatch.Start();

                        //loop enough times to be above 1 ms
                        for (int i = 0; i <= 100000; i++)
                        {
                            //we could set the instance properties of the function class for each iteration
                            //if we had a data set, each iteration would be a row of data to process
                            evalResult = expressionDelegate(functions);
                        }

                        stopWatch.Stop();

                        result = evalResult.ToString();
                    }
                    catch (ApplicationException exp)
                    {
                        evalResult = exp;
                    }

                    ResultLabel.Text = result.ToString();

                    //show the elapsed time for the loop only - no compilation is included in this time.
                    //simulates having a saved expression that is rarely changed
                    ElapsedTime.Text = stopWatch.ElapsedMilliseconds.ToString() + " ms";

                    System.Diagnostics.Trace.WriteLine(ElapsedTime.Text);

                }
                catch (Exception e)
                {
                    ResultLabel.Text = e.ToString();
                }
            }

        }

        private void edit_MouseClick(object sender, MouseEventArgs e)
        {
            int selectedItem = functionList.SelectedItems[0].Index;
            XmlDocument selectedFun = functionArrayList[selectedItem];
            funcName.Text = selectedFun.GetElementsByTagName("name")[0].InnerText;
            ExpressionText.Text = selectedFun.GetElementsByTagName("body")[0].InnerText;
            List<Parameter> paramList = new List<Parameter>();
            foreach (XmlNode node in selectedFun.GetElementsByTagName("input"))
            {
                Type paramType = Type.GetType(node.LastChild.InnerText);
                Parameter param = new Parameter(node.FirstChild.InnerText, Type.GetType(node.LastChild.InnerText), "0");
                paramList.Add(param);
            }
            possibleParams.View = View.Details;
            possibleParams.FullRowSelect = true;
            foreach (Parameter p in paramList)
            {
                MethodInfo method = typeof(Parameter).GetMethod("getValue");             
                MethodInfo genericMethod = method.MakeGenericMethod(p.type);
                var obj = genericMethod.Invoke(p, null);
                 // No target, no arguments
                possibleParams.Items.Add(new ListViewItem(new string[] { p.type.ToString() , p.name , obj.ToString()}));
            }
            functionList.GridLines = true;
            ReturnTypeCombo.SelectedIndex = 1;
        }
        public string pullParametersInFunction() {
            string parameters = "";
            int parameterCount = possibleParams.Items.Count;
            
            for (int i = 0; i < parameterCount; i++) {
                if (i == (parameterCount - 1)) {
                    parameters += possibleParams.Items[i].Text;
                }
                else parameters+= possibleParams.Items[i].Text+", ";
            }
            return parameters;
        }
        private void addToXML() { }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}