using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Reflection;
using System;
public abstract class CXmlElementParserBase 
{
	public List<CXmlElementParserBase> ChildXmlElementParser = new List<CXmlElementParserBase>();
	//------------------------------------------------------------
	public void DoScriptDataLoad(XmlElement pElem, string strNameSpace, Assembly pAssembly)
	{
		Type pClassType = GetType();
		XmlAttributeCollection pAttList = pElem.Attributes;
		for (int i = 0; i < pAttList.Count; i++)
		{
			PrivXmlAttribute(pAttList[i]);
		}

		OnScriptXMLLoadedAttribute();

		XmlNodeList pNodeList = pElem.ChildNodes;
		for (int i = 0; i < pNodeList.Count; i++)
		{
			XmlNode pNode = pNodeList[i];
			if (pNode.NodeType == XmlNodeType.Element)
			{
				PrivXmlElement(pNode as XmlElement, strNameSpace, pAssembly);
			}
		}
	}

	//--------------------------------------------------------------
	private void PrivXmlAttribute(XmlAttribute pXmlAttribute)
	{
		CScriptDataBase.GlobalScriptDataReadField(this, pXmlAttribute.Name, pXmlAttribute.Value);
	}

	private void PrivXmlElement(XmlElement pElement, string strNameSpace, Assembly pAssembly)
	{
		string strClassName = string.Format("{0}.{1}", strNameSpace, pElement.Name);
		CXmlElementParserBase pChildLoader = pAssembly.CreateInstance(strClassName) as CXmlElementParserBase;
		if (pChildLoader == null)
		{
			Debug.LogError($"[XmlData Loader] Element invalid class Instance {pElement.Name} ");
			return;
		}

		ChildXmlElementParser.Add(pChildLoader);
		pChildLoader.DoScriptDataLoad(pElement, strNameSpace, pAssembly);
	}

	//------------------------------------------------------------------------------------
	protected virtual void OnScriptXMLLoadedAttribute() { }

}
