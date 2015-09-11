/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ProtoBuf;

namespace Scut.SMS
{

    public static class ProtobufChecker
    {
        private static int errorCount;
        private static int warnCount;
        private static int tagErrCount;

        public static string Check(Assembly assembly)
        {
            bool includePrent = false;
            tagErrCount = 0;
            warnCount = 0;
            errorCount = 0;
            StringBuilder writeBody = new StringBuilder();
            writeBody.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            writeBody.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            writeBody.AppendLine("<head>");
            writeBody.AppendLine("\t<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            writeBody.AppendLine("\t<title>Check Protobuf Attribute</title>");
            writeBody.AppendLine(@"
	<style>
	body{
		font: 12px/1.6 Tahoma, Verdana, Arial, ""宋体"";background: #fff; color: #333;
    }
	table 
	{ 
		border-collapse: collapse; 
		border: none; 
		width: 200px; 
	} 
	td 
	{ 
		border: solid #000 1px; 
		padding: 3px 5px;
        white-space:nowrap; 
	} 
	</style>");
            writeBody.AppendLine("</head>");
            writeBody.AppendLine("<body>");

            StringBuilder writeText = new StringBuilder();
            if (assembly != null)
            {
                var types = assembly.GetTypes().Where(
                        p => p.GetCustomAttributes(typeof(ProtoContractAttribute), false).Count() > 0).ToList();
                for (int i = 0; i < types.Count; i++)
                {
                    var myEntity = types[i];
                    //获得所有属性
                    var properties =
                        myEntity.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    var fields =
                        myEntity.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
                                           BindingFlags.IgnoreCase);
                    if (properties.Length == 0 && fields.Length == 0)
                    {
                        continue;
                    }

                    writeText.AppendFormat("<h2>Class:{0}</h2>", myEntity.FullName);
                    writeText.AppendLine();

                    DoField(writeText, fields, includePrent);

                    DoProperty(writeText, properties, includePrent);

                    writeText.AppendLine("<hr />");
                }

                var notypes = assembly.GetTypes().Where(
                        p => !p.IsEnum && p.GetCustomAttributes(typeof(ProtoContractAttribute), false).Count() == 0).
                        ToList();

                writeBody.AppendFormat("<h2>Check {0} error:</h2>", FormatToHtml(assembly.FullName));
                writeBody.AppendFormat("<ul><li style=\"color:#FF0000;\">Tag repeat: {0};</li>", tagErrCount);
                writeBody.AppendFormat("<li style=\"color:#FF0000\">Exception: {0}</li>", errorCount);
                writeBody.AppendFormat("<li style=\"color:#0000FF;\">No Proto class warn: {0};</li>", notypes.Count);
                writeBody.AppendFormat("<li style=\"color:#0000FF\">No get or set warn: {0};</li></ul>", warnCount);
                writeBody.AppendLine("<hr />");

                writeBody.AppendLine("<h3>No ProtoContract Attribute:</h3>");

                writeBody.AppendLine("<table><tr>");
                writeBody.AppendLine("\t<td>Type</td>");
                writeBody.AppendLine("</tr>");

                foreach (var notype in notypes)
                {
                    writeBody.AppendLine("\t<tr>");
                    writeBody.AppendFormat("\t\t<td style=\"color:#0000FF;\">{0}</td>\r\n",
                                           FormatToHtml(notype.FullName));
                    writeBody.AppendLine("\t</tr>");
                }
                writeBody.AppendLine("</table>");
                writeBody.AppendLine();
            }
            writeBody.Append(writeText);
            writeBody.AppendLine("</body>");
            writeBody.AppendLine("</html>");

            return writeBody.ToString();

        }

        private static void DoProperty(StringBuilder writeText, PropertyInfo[] properties, bool includePrent)
        {
            if (properties.Length == 0)
            {
                return;
            }
            writeText.AppendLine("<h3>Property:</h3>");
            writeText.AppendLine("<table><tr>");
            writeText.AppendLine("\t<td>Type</td>");
            writeText.AppendLine("\t<td>Property</td>");
            writeText.AppendLine("\t<td>Remark</td>");
            writeText.AppendLine("</tr>");
            HashSet<int> memberSet = new HashSet<int>();
            foreach (var property in properties)
            {
                try
                {
                    string typeName = WriteFormat(property.PropertyType.Name);
                    string canReadWrite = "";
                    if (property.CanRead && property.CanWrite)
                    {
                        canReadWrite = string.Format("{0}{1}", (property.CanRead ? "get;" : ""), (property.CanWrite ? "set;" : ""));
                    }
                    else
                    {
                        warnCount++;
                        canReadWrite = string.Format("<span style=\"color:#0000FF;\">{0}{1}</span>", (property.CanRead ? "get;" : ""), (property.CanWrite ? "set;" : ""));
                    }

                    var tempList = property.GetCustomAttributes(typeof(ProtoMemberAttribute), true);
                    if (tempList.Length == 0)
                    {
                        if (property.DeclaringType.FullName != null &&
                            property.DeclaringType.FullName.StartsWith("ZyGames.Framework."))
                        {
                            if (includePrent)
                            {
                                writeText.AppendLine("\t<tr>");
                                writeText.AppendFormat("\t\t<td>Parent:{0}</td>\r\n", FormatToHtml(typeName));
                                writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(property.Name));
                                writeText.AppendFormat("\t\t<td>{0}</td>\r\n", canReadWrite);
                                writeText.AppendLine("\t</tr>");
                            }
                        }
                        else
                        {
                            writeText.AppendLine("\t<tr>");
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(typeName));
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(property.Name));
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", canReadWrite);
                            writeText.AppendLine("\t</tr>");
                        }
                    }
                    else
                    {
                        var member = tempList[0] as ProtoMemberAttribute;
                        if (member == null) continue;
                        if (!memberSet.Contains(member.Tag))
                        {
                            memberSet.Add(member.Tag);
                        }
                        else
                        {
                            tagErrCount++;
                            writeText.AppendLine("\t<tr>");
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(typeName));
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(property.Name));
                            writeText.AppendFormat("\t\t<td style=\"color:#FF0000\">Protomember tag:\"{0}\"is exist.</td>\r\n", member.Tag);
                            writeText.AppendLine("\t</tr>");

                        }
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    writeText.AppendLine("\t<tr>");
                    writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(property.PropertyType.Name));
                    writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(property.Name));
                    writeText.AppendFormat("\t\t<td style=\"color:#FF0000\">Error:{0}</td>\r\n", FormatToHtml(ex.Message));
                    writeText.AppendLine("\t</tr>");
                }

            }

            writeText.AppendLine("</table>");
        }

        private static string WriteFormat(string typeName)
        {
            return typeName != null && typeName.Length >= 8 ? typeName : typeName + "\t";
        }

        private static void DoField(StringBuilder writeText, FieldInfo[] fields, bool includePrent)
        {
            if (fields.Length == 0)
            {
                return;
            }
            writeText.AppendLine("<h3>Field:</h3>");
            writeText.AppendLine("<table><tr>");
            writeText.AppendLine("\t<td>Type</td>");
            writeText.AppendLine("\t<td>Field</td>");
            writeText.AppendLine("\t<td>Remark</td>");
            writeText.AppendLine("</tr>");
            HashSet<int> memberSet = new HashSet<int>();
            foreach (var field in fields)
            {
                try
                {
                    string typeName = WriteFormat(field.FieldType.Name);
                    var tempList = field.GetCustomAttributes(typeof(ProtoMemberAttribute), true);
                    if (tempList.Length == 0)
                    {

                        if (field.DeclaringType.FullName != null &&
                            field.DeclaringType.FullName.StartsWith("ZyGames.Framework."))
                        {
                            if (includePrent)
                            {
                                writeText.AppendLine("\t<tr>");
                                writeText.AppendFormat("\t\t<td>Parent:{0}</td>\r\n", FormatToHtml(typeName));
                                writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(field.Name));
                                writeText.AppendFormat("\t\t<td></td>\r\n");
                                writeText.AppendLine("\t</tr>");
                            }
                        }
                        else
                        {
                            writeText.AppendLine("\t<tr>");
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(typeName));
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(field.Name));
                            writeText.AppendFormat("\t\t<td></td>\r\n");
                            writeText.AppendLine("\t</tr>");
                        }
                    }
                    else
                    {
                        var member = tempList[0] as ProtoMemberAttribute;
                        if (member == null) continue;
                        if (!memberSet.Contains(member.Tag))
                        {
                            memberSet.Add(member.Tag);
                        }
                        else
                        {
                            tagErrCount++;
                            writeText.AppendLine("\t<tr>");
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(typeName));
                            writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(field.Name));
                            writeText.AppendFormat("\t\t<td style=\"color:#FF0000\">Protomember tag:\"{0}\"is exist.</td>\r\n", member.Tag);
                            writeText.AppendLine("\t</tr>");

                        }
                    }

                }
                catch (Exception ex)
                {
                    errorCount++;
                    writeText.AppendLine("\t<tr>");
                    writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(field.FieldType.Name));
                    writeText.AppendFormat("\t\t<td>{0}</td>\r\n", FormatToHtml(field.Name));
                    writeText.AppendFormat("\t\t<td style=\"color:#FF0000\">Error:{0}</td>\r\n", FormatToHtml(ex.Message));
                    writeText.AppendLine("\t</tr>");
                }
            }
            writeText.AppendLine("</table>");
        }

        private static string FormatToHtml(string str)
        {
            return str
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace(" ", "&nbsp;")
                .Replace("\"", "&quot;")
                .Replace("©", "&copy;");
        }
    }
}