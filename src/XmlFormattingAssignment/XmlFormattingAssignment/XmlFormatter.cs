using System.Collections;
using System.Reflection;
using System.Text;


namespace XmlFormattingAssignment
{
    public static class XmlFormatter
    {
        static string xmlString = null; //for storing XML string

        // Generating a string of spaces for indentation based on the given number of spaces.
        public static string Indent(int tab)
        {
            string str = null;
            for (int i = 0; i < tab; i++)
            {
                str += " ";
            }
            return str;
        }
        public static void ObjectToXML(object obj, int ind)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();


            foreach (var field in propertyInfos)
            {   // Checking if the field is a collection (List, Array) but not a string        
                if (field.GetValue(obj) != null && typeof(IEnumerable).IsAssignableFrom(field.PropertyType) && field.PropertyType != typeof(string))
                {

                    #region ListOrArrayHandeling

                    // Attempt to cast the field's value to IEnumerable to check if it is a collection
                    var enumerable = field.GetValue(obj) as IEnumerable;
                    xmlString += $"{Indent(ind)}<{field.Name}>\n";

                    if (enumerable != null)
                    {
                        foreach (var field1 in enumerable)
                        {   // Checking again if the field1 is a class but not a string
                            if (field1.GetType().IsClass && field1.GetType() != typeof(string))
                            {   
                                xmlString += $"{Indent(ind + 4)}<{field1.GetType().Name}>\n";
                                ObjectToXML(field1, ind + 8); // Handle nested objects by recursively converting their fields to XML
                                xmlString += $"{Indent(ind + 4)}</{field1.GetType().Name}>\n";
                            }
                            else
                                //Handle Primitive or String type
                                xmlString += $"{Indent(ind + 4)}<{field1.GetType().Name}>{field1}</{field1.GetType().Name}>\n";
                        }
                    }
                    xmlString += $"{Indent(ind)}</{field.Name}>\n";
                    #endregion

                }
                // Checking if the field is a class but not a string
                else if (field.GetValue(obj) != null && field.PropertyType.IsClass && field.PropertyType != typeof(string))
                {

                    #region ClassHandeling
                    xmlString += $"{Indent(ind)}<{field.Name}>\n";

                    ObjectToXML(field.GetValue(obj), ind + 4);

                    xmlString += $"{Indent(ind)}</{field.Name}>\n";
                    #endregion
                }
                // otherwise field is a null or primitive or string type
                else
                    xmlString += $"{Indent(ind)}<{field.Name}>{field.GetValue(obj)}</{field.Name}>\n";

            }
            
        }

        public static void XML(object obj, string variableName)
        {
            if (obj == null)
                return;
            // Checking if the obj is a collection of obj(List, Array) but not a string 
            if (obj != null && typeof(IEnumerable).IsAssignableFrom(obj.GetType()) && obj.GetType() != typeof(string))
            {
                #region ListOrArrayHandeling

                var enumerable = obj as IEnumerable;
                xmlString += $"<{variableName}>";
                int ind = 0;
                if (enumerable != null)
                {
                    foreach (var field1 in enumerable)
                    {
                        if (field1.GetType().IsClass && field1.GetType() != typeof(string))
                        {
                            xmlString += $"{Indent(ind + 4)}<{field1.GetType().Name}>";
                            ObjectToXML(field1, ind + 8);
                            xmlString += $"{Indent(ind + 4)}</{field1.GetType().Name}>";
                        }
                        else
                            xmlString += $"{Indent(ind + 4)}<{field1.GetType().Name}>{field1}</{field1.GetType().Name}>";
                    }
                }
                xmlString += $"</{variableName}>";
                #endregion

            }
            // Checking if the obj is a class but not a string
            else if (obj != null && obj.GetType().IsClass && obj.GetType() != typeof(string))
            {
                #region ClassHandeling
                xmlString += $"<{obj.GetType().Name}>";
                ObjectToXML(obj, 4);
                xmlString += $"</{obj.GetType().Name}>";
                #endregion
            }
            else            
                xmlString += $"<{obj.GetType().Name}>{obj}</{obj.GetType().Name}>"; // otherwise field is a null or primitive or string type
            
        }

        public static string Convert(object obj)
        {                        
           
            XML(obj, nameof(obj));

            return xmlString;

           
        }
    }
}