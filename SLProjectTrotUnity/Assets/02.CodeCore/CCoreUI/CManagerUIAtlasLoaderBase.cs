using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

//모든 아틀라스는 메모리에 상주한다. 규모와 갯수를 통재해야 한다.
//

public abstract class CManagerUIAtlasLoaderBase : CManagerTemplateBase<CManagerUIAtlasLoaderBase>
{
	private List<SpriteAtlas> m_listAtlasInstance = new List<SpriteAtlas>();
	private Dictionary<string, Sprite> m_mapAtlasSprite = new Dictionary<string, Sprite>();
	//--------------------------------------------------------
	protected void ProtAtlasLoaderAdd(SpriteAtlas pAtlas) // 모든 스프라이트는 Clone이며 메모리에 상주한다.
    {       
		if (m_listAtlasInstance.Contains(pAtlas) == false)
		{
			Sprite[] aSprite = new Sprite[pAtlas.spriteCount];
			pAtlas.GetSprites(aSprite);

			for (int i = 0; i < aSprite.Length; i++)
			{
				m_mapAtlasSprite[RemoveCloneObjectName(aSprite[i].name)] = aSprite[i];
			}
		}
	}

	public Sprite FindAtlasSprite(string strSpriteName) 
	{
		Sprite pFindSprite = null;
		if (m_mapAtlasSprite.ContainsKey(strSpriteName))
		{
			pFindSprite = m_mapAtlasSprite[strSpriteName];
		}

		return pFindSprite;
	}


} 
