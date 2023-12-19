using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CManagerGameDBBase : CManagerTemplateBase<CManagerGameDBBase>
{
	private Dictionary<string, CGameDBSheetBase> m_mapGameDBCategory = new Dictionary<string, CGameDBSheetBase>();
	//---------------------------------------------------------
	protected void ProtMgrGameDBSheetAdd(CGameDBSheetBase pSheet)
	{
		string strClassName = pSheet.GetType().Name;

		if (m_mapGameDBCategory.ContainsKey(strClassName))
		{
			Debug.LogWarningFormat("[GameDB] Duplicate SheetID : {0}", strClassName);
		}
		else
		{
			m_mapGameDBCategory[strClassName] = pSheet;
		}
	}

	protected TEMPLATE FindMgrGameDBSheet<TEMPLATE>() where TEMPLATE : CGameDBSheetBase
	{
		string strClassName = typeof(TEMPLATE).Name;
		TEMPLATE pFindSheet = null;
		if (m_mapGameDBCategory.ContainsKey(strClassName))
		{
			pFindSheet = m_mapGameDBCategory[strClassName] as TEMPLATE;
		}
		return pFindSheet;
	}

    protected void ProtMgrGameDBSheetRefresh()
    {
        Dictionary<string, CGameDBSheetBase>.Enumerator it = m_mapGameDBCategory.GetEnumerator();
        while(it.MoveNext())
        {
            it.Current.Value.InterGameDBSheetRefresh();
        }
    }

    protected void ProtMgrGameDBSheetLocalDB() // 모든 DB를 개발자용 로컬 데이터로 채운다. 릴리즈 할때 문제가 생길수 있으므로 하위에서 전처리기로 호출을 통제하자
    {
        Dictionary<string, CGameDBSheetBase>.Enumerator it = m_mapGameDBCategory.GetEnumerator();
        while (it.MoveNext())
        {
            it.Current.Value.InterGameDBSheetLocalDB();
        }
    }

}
