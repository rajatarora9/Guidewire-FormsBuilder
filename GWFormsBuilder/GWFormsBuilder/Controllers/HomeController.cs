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

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(XmlDocument));
            string alltext = System.IO.File.ReadAllText(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");

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
            XmlNode pcfDoc = doc.AppendChild(pcf);


            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(XmlDocument));
            string alltext = System.IO.File.ReadAllText(@"C:\\Temp\\FNOLVehicleIncidentPopup.pcf");

            XmlDocument olddoc = new XmlDocument();
            olddoc.LoadXml(alltext);
            XmlNodeList nodes = olddoc.GetElementsByTagName("InputColumn");
            StringBuilder builder = new StringBuilder();
            if (GWControlList.Any())
            {

                for (int i = 1; i <= nodes.Count; i++)
                {
                    XmlElement inputcolumn = doc.CreateElement("InputColumn");
                    XmlNode input = pcfDoc.AppendChild(inputcolumn);
                    foreach (GWControl gwControl in GWControlList)
                    {
                        if (gwControl.inputColumnIdentifier == i)
                        {
                            XmlElement controltype =  doc.CreateElement(gwControl.ControlType);

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
    }
}