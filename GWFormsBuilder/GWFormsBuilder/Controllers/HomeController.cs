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

namespace GWFormsBuilder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            GWControlModel model = new GWControlModel();
            model.GWControlList = new List<GWControl>();
            model.LevelOneGWControl = new List<LevelOneGWControl>();
            model.LevelZeroControlGrid = new List<LevelZeroControls>();
            model.InputType = new Dictionary<string, string>();
            model.InputType .Add("TextInput", "TextInput");
            model.InputType.Add("TypeKeyInput","TypeKeyInput");
            model.InputType.Add("CurrencyInput", "CurrencyInput");
            model.InputType.Add("BooleanRadioInput", "BooleanRadioInput");
            model.InputType.Add("Label", "Label");
            model.InputType.Add("TextAreaInput", "TextAreaInput");
            model.InputType.Add("DateInput", "DateInput");
            model.LevelOneInputType = new List<string> {"Label", "ToolBar", "RowIterator", "PostOnChange", "InputDivider", "MenuItem", "Reflect"};
            model.LevelZeroInputType = new List<string> { "DetailViewPanel" };
            model.YesNo = new List<string> { "True", "False" };
            model.ValueType = new List<string> { "java.lang.String", "java.lang.Integer" };

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(XmlDocument));
            string alltext =  System.IO.File.ReadAllText(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");

            // StreamWriter sw = new StreamWriter(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");

            XmlDocument doc = new XmlDocument();
            //doc.Load(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");
            doc.LoadXml(alltext);


            XmlNodeList nodes = doc.GetElementsByTagName("InputColumn");
            
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

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(List<GWControl> GWControlList, List<LevelOneGWControl> LevelOneGWControl, List<LevelZeroControls> LevelZeroControlGrid)
        {
            bool CreatedSuccesfully = false;
            string Data = "";
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);// Create the root element
            XmlElement pcf = doc.CreateElement("PCF");
            pcf.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            pcf.SetAttribute("xsi:noNamespaceSchemaLocation", "pcf.xsd");
            XmlNode pcfDoc =  doc.AppendChild(pcf);

            if (LevelZeroControlGrid.Any())
            {
                foreach (var L0Control in LevelZeroControlGrid)
                {
                    XmlElement parent = doc.CreateElement(L0Control.LevelZeroControl);
                    XmlNode LevelZeroControl =  pcfDoc.AppendChild(parent);
                    XmlElement inputcolumn = doc.CreateElement("InputColumn");
                    
                    XmlNode control = LevelZeroControl.AppendChild(inputcolumn);

                    if (GWControlList.Any())
                    {
                        foreach (var getControl in GWControlList)
                        {
                            if (!String.IsNullOrEmpty(getControl.ControlType) && getControl.IsDeleted == false)
                            {
                                XmlElement nestedElementL1 = doc.CreateElement(getControl.ControlType);
                                control.AppendChild(nestedElementL1);

                                if (!String.IsNullOrEmpty(getControl.IsEditable))
                                {
                                    nestedElementL1.SetAttribute("editable", getControl.IsEditable);
                                }

                                if (!String.IsNullOrEmpty(getControl.ID))
                                {
                                    nestedElementL1.SetAttribute("id", getControl.ID);
                                }

                                if (!String.IsNullOrEmpty(getControl.Label))
                                {
                                    nestedElementL1.SetAttribute("label", getControl.Label);
                                }

                                if (!String.IsNullOrEmpty(getControl.IsVisible))
                                {
                                    nestedElementL1.SetAttribute("visible", getControl.IsVisible);
                                }

                                if (!String.IsNullOrEmpty(getControl.ValueType))
                                {
                                    nestedElementL1.SetAttribute("valueType", getControl.ValueType);
                                }
                                else if (!String.IsNullOrEmpty(getControl.CustomValueType))
                                {
                                    nestedElementL1.SetAttribute("valueType", getControl.CustomValueType);
                                }

                                if (!String.IsNullOrEmpty(getControl.Value))
                                {
                                    nestedElementL1.SetAttribute("value", getControl.Value);
                                }

                                if (!String.IsNullOrEmpty(getControl.IsRequired))
                                {
                                    nestedElementL1.SetAttribute("required", getControl.IsRequired);
                                }

                                if (getControl.IsParent)
                                {
                                    foreach (var item in LevelOneGWControl)
                                    {
                                        if (getControl.ID == item.ParentID)
                                        {
                                            XmlElement nestedElement = doc.CreateElement(item.LevelOneControl);
                                            SetAttributeforLevelOne(nestedElement, item);
                                            nestedElementL1.AppendChild(nestedElement);
                                        }
                                    }
                                }

                                //pcf.AppendChild(control);
                            }
                        }

                        //MessageBox.Show(Filename + ".pcf has been generated successfully");
                    }

                        Data = doc.InnerXml;
                        CreatedSuccesfully = true;
                }
            }
            StringBuilder builder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(builder))
            {
                // We will use the Formatting of our xmlTextWriter to provide our indentation.
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    doc.WriteTo(xmlTextWriter);
                }
            }

            return Json(new { isSuccess = CreatedSuccesfully, data = builder.ToString()});

        }

        public void SetAttributeforLevelOne(XmlElement nestedElement , LevelOneGWControl control)
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
    }
}