using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NWebPacket
{

	public interface IJsonPacketBase
    {
        public void ReturnInstance();
    }

    public interface IJsonPacketRequest : IJsonPacketBase
    {

    }

    public interface IJsonPacketResponse : IJsonPacketBase
    {

    }


    [System.Serializable]
    public abstract class CJsonPacketTemplateRequest<TEMPLATE> : CObjectInstancePoolBase<TEMPLATE>, IJsonPacketRequest  where TEMPLATE : CJsonPacketTemplateRequest<TEMPLATE>
    {
        public string account_id;
        public string authToken;
        //-----------------------------------------------------------

        public void ReturnInstance()
        {
            InstancePoolDisable(this as TEMPLATE);
        }
    }

    [System.Serializable]
    public abstract class CJsonPacketTemplateResponse<TEMPLATE> : CObjectInstancePoolBase<TEMPLATE>, IJsonPacketResponse where TEMPLATE : CJsonPacketTemplateResponse<TEMPLATE>
    {
        public int result = 0;
        public int timestemp = 0;
        //---------------------------------------------------
        public void ReturnInstance()
        {
            InstancePoolDisable(this as TEMPLATE);
        }
    }

}




