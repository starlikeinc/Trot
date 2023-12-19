using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 케릭터 귀속 이펙트가 아닌 공용 이펙트 관리 로직
// 동적으로 할당받고 Expire가 되면 풀로 자동 반환 

public abstract class CManagerEffectBase : CManagerTemplateBase<CManagerEffectBase>
{
    public enum EEffectOriginType
    {
        FromPrefabPool,     // 메모리 풀로 자동 회수된다.
        FromInstance,
    }

    private struct SEffectInfo
    {
        public CEffectBase       EffectInstance;     
        public EEffectOriginType OriginType;    
    }


    private CLinkedList<SEffectInfo> m_listActiveEffectInstance = new CLinkedList<SEffectInfo>();
	//-------------------------------------------------------------
	protected override void OnUnityUpdate()
	{
		base.OnUnityUpdate();
        UpdateMgrEffectExpire();
    }

    //---------------------------------------------------------------
    protected void ProtMgrEffectRegistByInstance(CEffectBase pEffectInstance)
    {
        SEffectInfo rEffectInfo = new SEffectInfo();
        rEffectInfo.EffectInstance = pEffectInstance;      
        rEffectInfo.OriginType = EEffectOriginType.FromInstance;
        m_listActiveEffectInstance.AddLast(rEffectInfo);
        pEffectInstance.transform.SetParent(transform, true);
    }

    protected void ProtMgrEffectRegistByName(string strPrefabName, UnityAction<CEffectBase> delFinish)
    {
        CManagerPrefabPoolUsageBase.Instance.LoadComponent(EAssetPoolType.Effect, strPrefabName, (CEffectBase pLoadedEffect) =>
        {
            SEffectInfo rEffectInfo = new SEffectInfo();
            rEffectInfo.EffectInstance = pLoadedEffect;
            rEffectInfo.OriginType = EEffectOriginType.FromPrefabPool;
            m_listActiveEffectInstance.AddLast(rEffectInfo);
            pLoadedEffect.transform.SetParent(transform, false);
            pLoadedEffect.transform.position = Vector3.zero;
            pLoadedEffect.transform.localPosition = Vector3.zero;

            delFinish?.Invoke(pLoadedEffect);
        });
    }

    //---------------------------------------------------------------
    private void UpdateMgrEffectExpire()
    {
        CLinkedList<SEffectInfo>.Enumerator it = m_listActiveEffectInstance.GetEnumerator();
        while(it.MoveNext())
        {
            if (it.Current.EffectInstance.IsActive == false)
            {
                PrivMgrEffectExpire(it.Current);
                it.Remove();
            }
        }
    }
     
    private void PrivMgrEffectExpire(SEffectInfo rEffectInfo)
    {
        if (rEffectInfo.OriginType == EEffectOriginType.FromPrefabPool)
        {
            CManagerPrefabPoolUsageBase.Instance.Return(rEffectInfo.EffectInstance.gameObject);
            OnEffectExpire(rEffectInfo.EffectInstance);
        }
        else if (rEffectInfo.OriginType == EEffectOriginType.FromInstance)
        {
            rEffectInfo.EffectInstance.transform.SetParent(rEffectInfo.EffectInstance.GetEffectParentOrigin(), false);
            rEffectInfo.EffectInstance.transform.localPosition = Vector3.zero;
            rEffectInfo.EffectInstance.transform.rotation = Quaternion.identity;
            OnEffectExpire(rEffectInfo.EffectInstance);
        }
    }

    //----------------------------------------------------------------
    protected virtual void OnEffectExpire(CEffectBase pEffect) {}
}
