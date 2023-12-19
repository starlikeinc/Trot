using System.Collections.Generic;
using Newtonsoft.Json.Linq;


abstract public class CScriptDataJsonBase : CScriptDataBase
{
   
	//---------------------------------------------------------
    protected void ProtDataJsonRead<TEMPLATE>(string strTextData, ref List<TEMPLATE> listJsonData) where TEMPLATE : class
    {
        JArray pJsonArray = JArray.Parse(strTextData);
        for (int i = 0; i < pJsonArray.Count; i++)
        {
            TEMPLATE pInstance = pJsonArray[i].ToObject<TEMPLATE>();
            listJsonData.Add(pInstance);
        }
    }
}