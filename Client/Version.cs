#region Copyright (c)2003 Juanjo < http://lphant.sourceforge.net >
/*
* This file is part of eAnt
* Copyright (C)2003 Juanjo < j_u_a_n_j_o@users.sourceforge.net / http://lphant.sourceforge.net >
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either
* version 2 of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Net;
using System.Windows.Forms;

namespace eAnt.Classes
{
	/// <summary>
	/// The Config class is a file storage for propertys.
	/// </summary>
	public class Version
	{
		public struct VersionComp
		{
			public Versione Actual;
			public Versione New;
			public VersionComp(Versione Attuale, Versione Nuova)
			{
				Actual=Attuale;
				New=Nuova;
			}
		}
		public VersionComp ComparatoreDiVersione;
		public struct Versione
		{
			public string Version; public string URL; public string Changes;
			public Versione(string versione, string url, string changes)
			{
				Version=versione;
				URL=url;
				Changes=changes;
			}
		}

		public void LoadConfig(string url/*, string user, string pass, string proxyURL, bool proxyEnabled*/)
		{
			try 
			{
			//Load the xml config file
			XmlDocument XmlDoc = new XmlDocument();
			HttpWebResponse Response;
			HttpWebRequest Request;
			//Retrieve the File

			Request = (HttpWebRequest)HttpWebRequest.Create(url);
			//Request.Headers.Add("Translate: f");
			/*if(user != null && user != "")
				Request.Credentials = new NetworkCredential(user, pass);
			else
				Request.Credentials = CredentialCache.DefaultCredentials;*/

			/*
			if(proxyEnabled == true)
				Request.Proxy = new WebProxy(proxyURL,true);*/

			Response = (HttpWebResponse)Request.GetResponse();

			Stream respStream = null;
			respStream = Response.GetResponseStream();
			//Load the XML from the stream
			XmlDoc.Load(respStream);

			//Parse out the AvailableVersion
			XmlNode AvailableVersionNode = XmlDoc.SelectSingleNode(@"//AvailableVersion");
			string AvailableVersion = AvailableVersionNode.InnerText;

			//Parse out the AppFileURL
			XmlNode AppFileURLNode = XmlDoc.SelectSingleNode(@"//AppFileURL");
			string AppFileURL = AppFileURLNode.InnerText;

			//Parse out the LatestChanges
			XmlNode LatestChangesNode = XmlDoc.SelectSingleNode(@"//LatestChanges");
			string LatestChanges;
			if(LatestChangesNode != null)
				LatestChanges = LatestChangesNode.InnerText;
			else
				LatestChanges = "";
			
			Versione VersioneAttuale=new Versione("1.4", null, null);
			Versione VersioneNuova=new Versione(AvailableVersion, AppFileURL, LatestChanges);
			ComparatoreDiVersione=new VersionComp(VersioneAttuale, VersioneNuova);
			} 
			catch (Exception e)
			{
				Debug.WriteLine("Failed to read the config file at: " + url ); 
				Debug.WriteLine("Make sure that the config file is present and has a valid format");
				MessageBox.Show("Failed to read the config file at: " + url + "\nIf you are behind a proxy, check manually at:\nhttp://www.salatti.net/eAnt" ); 
				
				Versione VersioneAttuale=new Versione("1.4", null, null);
				Versione VersioneNuova=new Versione("1.4", null, null);
				ComparatoreDiVersione=new VersionComp(VersioneAttuale, VersioneNuova);			
			}
			
		}//LoadConfig(string url, string user, string pass)
	}

}
