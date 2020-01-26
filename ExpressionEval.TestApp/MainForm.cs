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
        bool isEdited = false;
        string souceCoudePath = "C:\\Users\\elisa\\source\\repos\\tryRunFromXML\\source_codes";
        public MainForm()
        {
            //initialize the components of the form 
            InitializeComponent();
            // set parameter type
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("C:\\Users\\elisa\\source\\repos\\tryRunFromXML\\config_data\\possible_types.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    while (sr.Peek() >= 0)
                    {
                        String line = sr.ReadLine();
                        Param_type.Items.Add(line);
                        Console.WriteLine(line);
                    }

                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }
            //setup some return types
            ReturnTypeCombo.Items.Add(typeof(string));
            ReturnTypeCombo.Items.Add(typeof(double));
            ReturnTypeCombo.Items.Add(typeof(DateTime));
            //<- possibly the same as the dopdown menu for the parametes 

            functionList.View = View.Details;
            functionList.FullRowSelect = true;
            //read all prepared XML files which contain functions 
            functionArrayList = readAllXmlFilesFromPath(souceCoudePath);
            foreach (XmlDocument xmlDoc in functionArrayList)
            {
                functionList.Items.Add(new ListViewItem(new string[] { xmlDoc.GetElementsByTagName("name").Item(0).InnerText, xmlDoc.GetElementsByTagName("input").Count.ToString(), xmlDoc.GetElementsByTagName("returnType").Item(0).InnerText }));
            }
            functionList.GridLines = true;
            functionList.Items[0].Selected = true;
            functionList.Select();
            ReturnTypeCombo.SelectedIndex = 1;
        }
        private static List<XmlDocument> readAllXmlFilesFromPath(string path)
        {
            List<XmlDocument> functionArrayList = new List<XmlDocument>();
            foreach (string file in Directory.GetFiles(path, "*.xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                functionArrayList.Add(doc);

            }
            return functionArrayList;

        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            isEdited = false;
            
            string result = string.Empty;

            //get the function body 
            if (FunctionBody.Text.Trim().Length > 0)
            {
                //Get the Expression and add some parameters
                string expressionText = FunctionBody.Text;
                string parameters = pullParametersInFunction();
                Console.Out.WriteLine(pullParametersInFunction());
                //create new evaluator
                ICodeCompiler evaluator = new CodeCompiler(ExpressionLanguage.CSharp);

                //create instance of function class with some instance data
                FunctionClass functions = new FunctionClass();

                //set the instance data
                functions.X = 10;
                functions.Y = 15;
                functions.Z = 3.3;

                object evalResult = null;

                //performance metrics
                Stopwatch stopWatch = new Stopwatch();
                int a = 1;
                int b = 2;
                int c = 1;

                stopWatch.Start();
                /* object[] paramArray = new object[3];
                 paramArray[0]=a;
                 paramArray[1] = b;
                 paramArray[2] = c;*/

                //string paramArray = "int a, int b";

                try
                {
                    //based on the return type evaluate the expression
                    switch (((Type)ReturnTypeCombo.SelectedItem).Name)
                    {
                        case "String":
                            evalResult = evaluator.CompileToResult<string, FunctionClass>(FunctionBody.Text, functions);
                            break;
                        case "Double":
                            evalResult = evaluator.CompileToResult<double, FunctionClass>(FunctionBody.Text, functions);
                            break;
                        case "DateTime":
                            evalResult = evaluator.CompileToResult<DateTime, FunctionClass>(FunctionBody.Text, functions);
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
            cleanAllFields();
            ResultLabel.Text = result.ToString();
        }

       /* private void LoopButton_Click(object sender, EventArgs e)
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
        }*/

      /*  private void CalcLoop<R>()
        {

            string result = string.Empty;

            //make sure expression is not empty
            if (FunctionBody.Text.Trim().Length > 0)
            {

                Stopwatch stopWatch = new Stopwatch();
                ICodeCompiler compiler = new CodeCompiler(ExpressionLanguage.CSharp);

                try
                {
                    //DynamicMethodState could be serialized and saved
                    DynamicMethodState methodState = compiler.GetMethodState<R, FunctionClass>(FunctionBody.Text);

                    //get delegate for saved compiled expression
                    CompiledCode<R, FunctionClass> expressionDelegate = compiler.GetDelegate<R, FunctionClass>(methodState);

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
                        evalResult = exp.ToString();
                    }

                    ResultLabel.Text = result;

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
        */
        private void edit_MouseClick(object sender, MouseEventArgs e)
        {
            cleanAllFields();
            if (functionList.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                int selectedItem = functionList.SelectedItems[0].Index;
                isEdited = true;
                XmlDocument selectedFun = functionArrayList[selectedItem];
                funcName.Text = selectedFun.GetElementsByTagName("name")[0].InnerText;
                FunctionBody.Text = selectedFun.GetElementsByTagName("body")[0].InnerText;
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
                    possibleParams.Items.Add(new ListViewItem(new string[] { p.type.ToString(), p.name, obj.ToString() }));
                }
                functionList.GridLines = true;
                ReturnTypeCombo.SelectedIndex = 1;
            }
        }
        public string pullParametersInFunction()
        {
            string parameters = "";
            int parameterCount = possibleParams.Items.Count;

            for (int i = 0; i < parameterCount; i++)
            {
                if (i == (parameterCount - 1))
                {
                    parameters += possibleParams.Items[i].Text;
                }
                else parameters += possibleParams.Items[i].Text + ", ";
            }
            return parameters;
        }
        private void cleanAllFields() {
            Param_name.Clear();
            funcName.Clear();
            possibleParams.Items.Clear();
            Param_type.SelectedIndex = -1;
            FunctionBody.Clear();
            ReturnTypeCombo.SelectedIndex = -1;
        }
        private void addParamToXML(string type, string name, XmlDocument function) {
            XmlNode root = function.GetElementsByTagName("inputs")[0];
            XmlElement input = function.CreateElement("input");
            XmlElement nameTag = function.CreateElement("name");
            nameTag.InnerText = name;
            XmlElement typeTag = function.CreateElement("type");
            typeTag.InnerText = type;
            input.AppendChild(nameTag);
            input.AppendChild(typeTag);
            root.AppendChild(input);
            System.Console.Out.WriteLine(function.BaseURI.ToString());
            function.Save(function.BaseURI.ToString().Replace("file:///",""));
        }

        /*private void button2_Click(object sender, EventArgs e)
        {


            List<Type> possibleTypes = new List<Type>();
            Assembly mscorlib = typeof(string).Assembly;
                      // Set a variable to the Documents path.
            /*string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "all_types.txt")))
            {
                foreach (Type type in mscorlib.GetTypes())
                {
                    /* if (type.ToString().Split('.').Length <= 2) {
                         Console.WriteLine(type.FullName);
                     }
                   // outputFile.WriteLine(type.ToString());
                    //Console.WriteLine(type.FullName);
                }
           
           
            //  }


            System.Console.Out.WriteLine("XD");

        }*/

        private void Add_param_Click(object sender, EventArgs e)
        {
            if (isEdited) {
                int selectedItem = functionList.SelectedItems[0].Index;
                XmlDocument selectedFun = functionArrayList[selectedItem];
                string selected_function_name= selectedFun.GetElementsByTagName("name")[0].InnerText;
                System.Console.Out.WriteLine("I WILL BE EDITING FUNCTION " + selected_function_name);
                string selectedType = (string)Param_type.SelectedItem;
                string paramName = Param_name.Text.Trim();
                System.Console.Out.WriteLine(selectedType);
                System.Console.Out.WriteLine(paramName);
                if (selectedType == "" || selectedType == null || paramName == "")
                {
                    System.Console.Out.WriteLine("CAN NOT ADD PARAM - EMPTY FIELDS");
                }
                else
                {
                    addParamToXML(selectedType, paramName, selectedFun);
                }
            }
            else
            {
                System.Console.Out.WriteLine("Could not edit because was not in edit mode!");
            }
        }

        
    }
}