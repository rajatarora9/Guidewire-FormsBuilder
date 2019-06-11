using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Windows;
using GWFormsBuilder.Models;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;

namespace GWFormsBuilder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            GWControlModel model = new GWControlModel();
            model = InitializeModel(model);

            //model = GenerateXMLDataFromFiles(model);
            if (!model.GWControlList.Any())
            {
                List<GWControl> newCollection = new List<GWControl>()
                {
                       new GWControl{ IsDeleted = false}
                };
                model.GWControlList = newCollection;

            }

            if (!model.LevelOneGWControl.Any())
            {
                List<LevelOneGWControl> leveloneCollection = new List<LevelOneGWControl>()
                {
                    new LevelOneGWControl{ IsDeleted = false}
                };
                model.LevelOneGWControl = leveloneCollection;
            }
            model = GetDirectories(model);

            return View(model);
        }

        public GWControlModel InitializeModel(GWControlModel model)
        {
            model.GWControlList = new List<GWControl>();
            model.LevelOneGWControl = new List<LevelOneGWControl>();
            model.LevelZeroControlGrid = new List<LevelZeroControls>();
            model.InputType = new Dictionary<string, string>();
            model.InputType.Add("TextInput", "TextInput");
            model.InputType.Add("TypeKeyInput", "TypeKeyInput");
            model.InputType.Add("CurrencyInput", "CurrencyInput");
            model.InputType.Add("BooleanRadioInput", "BooleanRadioInput");
            model.InputType.Add("Label", "Label");
            model.InputType.Add("TextAreaInput", "TextAreaInput");
            model.InputType.Add("DateInput", "DateInput");
            model.LevelOneInputType = new List<string> { "Label", "ToolBar", "RowIterator", "PostOnChange", "InputDivider", "MenuItem", "Reflect" };
            model.LevelZeroInputType = new List<string> { "DetailViewPanel" };
            model.YesNo = new List<string> { "True", "False" };
            model.ValueType = new List<string> { "java.lang.String", "java.lang.Integer" };
            return model;
        }

        [HttpPost]
        public ActionResult Index(List<GWControl> GWControlList, List<LevelOneGWControl> LevelOneGWControl, List<LevelZeroControls> LevelZeroControlGrid, string module, string file)
        {
            bool CreatedSuccesfully = false;
            string Data = "";
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);// Create the root element
            XmlElement pcf = doc.CreateElement("PCF");
            pcf.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            pcf.SetAttribute("xsi:noNamespaceSchemaLocation", "pcf.xsd");
            XmlNode pcfDoc = doc.AppendChild(pcf);


            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(XmlDocument));
            string alltext = System.IO.File.ReadAllText(@"C:\\GWGuidewire\\" + module + "\\" + file + "");
            //string alltext = System.IO.File.ReadAllText(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");

            XmlDocument olddoc = new XmlDocument();
            olddoc.LoadXml(alltext);
            XmlNodeList nodes = olddoc.GetElementsByTagName("InputColumn");
            StringBuilder builder = new StringBuilder();
            if (GWControlList.Any())
            {
                foreach (GWControl gwControl in GWControlList)
                {
                    gwControl.inputColumnIdentifier = gwControl.inputColumnIdentifier == 0 ? 1 : gwControl.inputColumnIdentifier;
                }
                for (int i = 1; i <= nodes.Count; i++)
                {
                    XmlElement inputcolumn = doc.CreateElement("InputColumn");
                    XmlNode input = pcfDoc.AppendChild(inputcolumn);
                    foreach (GWControl gwControl in GWControlList)
                    {
                        if (gwControl.inputColumnIdentifier == i)
                        {
                            XmlElement controltype = doc.CreateElement(gwControl.ControlType);

                            if (!String.IsNullOrEmpty(gwControl.IsEditable))
                            {
                                controltype.SetAttribute("editable", gwControl.IsEditable);
                            }

                            if (!String.IsNullOrEmpty(gwControl.ID))
                            {
                                controltype.SetAttribute("id", gwControl.ID);
                            }

                            if (!String.IsNullOrEmpty(gwControl.Label))
                            {
                                controltype.SetAttribute("label", gwControl.Label);
                            }

                            if (!String.IsNullOrEmpty(gwControl.IsVisible))
                            {
                                controltype.SetAttribute("visible", gwControl.IsVisible);
                            }

                            if (!String.IsNullOrEmpty(gwControl.ValueType))
                            {
                                controltype.SetAttribute("valueType", gwControl.ValueType);
                            }
                            else if (!String.IsNullOrEmpty(gwControl.CustomValueType))
                            {
                                controltype.SetAttribute("valueType", gwControl.CustomValueType);
                            }

                            if (!String.IsNullOrEmpty(gwControl.Value))
                            {
                                controltype.SetAttribute("value", gwControl.Value);
                            }

                            if (!String.IsNullOrEmpty(gwControl.IsRequired))
                            {
                                controltype.SetAttribute("required", gwControl.IsRequired);
                            }


                            input.AppendChild(controltype);

                        }
                    }
                }
                using (StringWriter stringWriter = new StringWriter(builder))
                {
                    // We will use the Formatting of our xmlTextWriter to provide our indentation.
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                    {
                        xmlTextWriter.Formatting = Formatting.Indented;
                        pcfDoc.WriteTo(xmlTextWriter);
                    }
                }
            }
            Session["DocContent"] = doc;
            //string filename = "File1" + ".pcf";
            //var path = @"C:\GWGuidewire\" + module + @"\" + filename;
            //doc.Save(path);
            return Json(new { isSuccess = true, data = builder.ToString() });
        }


        public void SetAttributeforLevelOne(XmlElement nestedElement, LevelOneGWControl control)
        {
            if (!String.IsNullOrEmpty(control.Actions))
            {
                nestedElement.SetAttribute("action", control.Actions);
            }

            if (!String.IsNullOrEmpty(control.ID))
            {
                nestedElement.SetAttribute("id", control.ID);
            }

            if (!String.IsNullOrEmpty(control.Label))
            {
                nestedElement.SetAttribute("label", control.Label);
            }

            if (!String.IsNullOrEmpty(control.IsVisible))
            {
                nestedElement.SetAttribute("visible", control.IsVisible);
            }

            if (!String.IsNullOrEmpty(control.ValueType))
            {
                nestedElement.SetAttribute("valueType", control.ValueType);
            }
            else if (!String.IsNullOrEmpty(control.CustomValueType))
            {
                nestedElement.SetAttribute("valueType", control.CustomValueType);
            }

            if (!String.IsNullOrEmpty(control.Value))
            {
                nestedElement.SetAttribute("value", control.Value);
            }

            if (!String.IsNullOrEmpty(control.IsRequired))
            {
                nestedElement.SetAttribute("required", control.IsRequired);
            }

            if (!String.IsNullOrEmpty(control.TriggerIds))
            {
                nestedElement.SetAttribute("TriggerIds", control.TriggerIds);
            }

            if (!String.IsNullOrEmpty(control.Available))
            {
                nestedElement.SetAttribute("available", control.Available);
            }

            if (!String.IsNullOrEmpty(control.BoldValue))
            {
                nestedElement.SetAttribute("boldValue", control.BoldValue);
            }
        }

        public GWControlModel GetDropDownDataFromDatabase(GWControlModel model)
        {
            DBConnect db = new DBConnect();
            db.OpenConnection();
            db.SelectModules();
            db.CloseConnection();
            model.ModuleName = new Dictionary<string, string>();
            model.FileNames = new Dictionary<string, string>();
            model.ModuleName.Add("Module1", "Module 1");
            model.FileNames.Add("File1", "File 1");
            return model;
        }

        public GWControlModel GetDirectories(GWControlModel model)
        {
            string root = @"C:\GWGuidewire";
            // Get all subdirectories
            string[] subdirectoryEntries = Directory.GetDirectories(root);
            // Loop through them to see if they have any other subdirectories
            int moduleCounter = 0;
            foreach (string subdirectory in subdirectoryEntries)
            {
                SelectListItem moduleItem = new SelectListItem();
                moduleItem.Text = subdirectory.Replace("C:\\GWGuidewire\\", "");
                moduleItem.Value = moduleCounter.ToString();
                model.Modules.Add(moduleItem);
                //LoadSubDirs(subdirectory, moduleCounter);
                moduleCounter++;

            }
            return model;
        }

        public GWControlModel GenerateXMLDataFromFiles(GWControlModel model, string module, string file)
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(XmlDocument));
            string alltext = System.IO.File.ReadAllText(@"C:\\GWGuidewire\\" + module + "\\" + file + "");

            // StreamWriter sw = new StreamWriter(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");

            XmlDocument doc = new XmlDocument();
            //doc.Load(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");
            doc.LoadXml(alltext);


            XmlNodeList nodes = doc.GetElementsByTagName("InputColumn");

            int counter = 1;
            foreach (XmlNode node in nodes)
            {
                XmlNodeList childnodes = node.ChildNodes;

                foreach (XmlNode childnode in childnodes)
                {
                    GWControl control = new GWControl();
                    XmlAttributeCollection attributes;
                    if (childnode.Attributes != null)
                    {
                        attributes = childnode.Attributes;
                        control.ControlType = childnode.Name;
                        control.inputColumnIdentifier = counter;
                        foreach (XmlAttribute attribute in attributes)
                        {


                            if (attribute.Name == "label")
                            {
                                control.Label = attribute.Value;
                            }
                            if (attribute.Name == "editable")
                            {
                                control.IsEditable = attribute.Value;
                            }
                            if (attribute.Name == "action")
                            {
                                control.Action = attribute.Value;
                            }
                            if (attribute.Name == "id")
                            {
                                control.ID = attribute.Value;
                            }
                            if (attribute.Name == "required")
                            {
                                control.IsRequired = attribute.Value;
                            }
                            if (attribute.Name == "visible")
                            {
                                control.IsVisible = attribute.Value;
                            }
                            if (attribute.Name == "value")
                            {
                                control.Value = attribute.Value;
                            }
                            if (attribute.Name == "valueType")
                            {
                                control.ValueType = attribute.Value;
                            }
                            else if (attribute.Name == "CustomValueType")
                            {
                                control.CustomValueType = attribute.Value;
                            }
                        }

                        model.GWControlList.Add(control);
                    }
                    else
                    {

                    }
                }
                counter++;

            }

            List<GWControl> newCollection = new List<GWControl>()
            {
                   new GWControl{ IsDeleted = false}
            };
            if (!model.GWControlList.Any())
            {
                model.GWControlList = newCollection;

            }

            List<LevelOneGWControl> leveloneCollection = new List<LevelOneGWControl>()
            {
                   new LevelOneGWControl{ IsDeleted = false}
            };
            if (!model.LevelOneGWControl.Any())
            {
                model.LevelOneGWControl = leveloneCollection;

            }

            List<LevelZeroControls> levelZeroCollection = new List<LevelZeroControls>()
            {
                   new LevelZeroControls{ IsDeleted = false}
            };

            if (!model.LevelZeroControlGrid.Any())
            {
                model.LevelZeroControlGrid = levelZeroCollection;

            }
            return model;
        }

        [HttpPost]
        public ActionResult LoadSubDirs(string module)
        {
            List<SelectListItem> Files = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(module) && module != "Select")
            {
                string[] subdirectoryEntries = Directory.GetFiles(@"C:\GWGuidewire\" + module);
                int fileCounter = 0;
                foreach (string subdirectory in subdirectoryEntries)
                {
                    SelectListItem fileItem = new SelectListItem();
                    fileItem.Text = subdirectory.Replace("C:\\GWGuidewire\\" + module + "\\", ""); ;
                    fileItem.Value = fileCounter.ToString();
                    Files.Add(fileItem);
                    fileCounter++;
                }
            }
            return Json(Files, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerateXML(string module, string file)
        {
            GWControlModel model = new GWControlModel();
            model = InitializeModel(model);
            model = GenerateXMLDataFromFiles(model, module, file);
            return PartialView("~/Views/Shared/_GWGrid.cshtml", model);
        }

        [HttpPost]
        public ActionResult DownloadFile(string module, string file)
        {
            var doc = Session["DocContent"] as XmlDocument;
            string filename = file.Replace(".pcf", "");
            int fileExtension = 1;
            var checkIfFileExisits = true;
            string toBeSavedFilename = "";
            do
            {
                toBeSavedFilename = filename + "_V" + fileExtension + ".pcf";
                checkIfFileExisits = CustomFileManager.CheckIfFileExisits(@"C:\GWGuidewire\" + module + @"\" + toBeSavedFilename);
                fileExtension++;
            } while (checkIfFileExisits);
            var path = @"C:\GWGuidewire\" + module + @"\" + toBeSavedFilename;
            doc.Save(path);
            return Json(new { isSuccess = true, Path = path });
        }


    }


    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "guidewire";
            uid = "root";
            password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";";
            //database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }


        //Select statement
        public List<string> SelectModules()
        {
            string query = "SELECT ModuleNames FROM tbldemotable";

            //Create a list to store the result
            List<string> list = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list.Add(dataReader["ModuleNames"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }



    }

    public static class CustomFileManager
    {
        public static bool CheckIfFileExisits(string file)
        {
            var fileExists = File.Exists(file);
            return fileExists;
        }

    }
}