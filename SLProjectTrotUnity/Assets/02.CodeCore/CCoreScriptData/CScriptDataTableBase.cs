using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
// 엑셀 CSV - UTF8 포멧 기반
abstract public class CScriptDataTableBase : CScriptDataBase
{	
	private List<Dictionary<string, string>> m_listIDictionarytem = new List<Dictionary<string, string>>();
	//---------------------------------------------------------
	protected override sealed void OnScriptDataLoad(string strTextData)
	{
		m_listIDictionarytem.AddRange(CSVReader.Read(strTextData));
	}

    protected sealed override void OnScriptDataLoadFinish()
    {
        OnScriptDataTableCSVParseComplete();
    }

    //---------------------------------------------------------
    protected List<INSTANCE> ProtDataTableRead<INSTANCE>() where INSTANCE : class, new()
	{
		List<INSTANCE> pListInstance = new List<INSTANCE>();

		for (int i = 0; i < m_listIDictionarytem.Count; i++)
		{
			INSTANCE pInstance = new INSTANCE();
			Dictionary<string, string>.Enumerator it = m_listIDictionarytem[i].GetEnumerator();
			while(it.MoveNext())
			{
				GlobalScriptDataReadField(pInstance, it.Current.Key, it.Current.Value);
			}
			pListInstance.Add(pInstance);
		}

		return pListInstance;
	}

	//---------------------------------------------------------------
	protected virtual void OnScriptDataTableCSVParseComplete() { }
}

//static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";


public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r";
	static char[] TRIM_CHARS = { '\"' };

	public static List<Dictionary<string, string>> Read(string strTextAsset)
	{
		var list = new List<Dictionary<string, string>>();

		var lines = Regex.Split(strTextAsset, LINE_SPLIT_RE);

		if (lines.Length <= 1) return list;

		var header = Regex.Split(lines[0], SPLIT_RE);
		for (var i = 1; i < lines.Length; i++)
		{

			var values = Regex.Split(lines[i], SPLIT_RE);
			if (values.Length == 0 || values[0] == "") continue;

			var entry = new Dictionary<string, string>();
			for (var j = 0; j < header.Length && j < values.Length; j++)
			{
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS);
				entry[header[j]] = value;
			}
			list.Add(entry);
		}
		return list;
	}
}