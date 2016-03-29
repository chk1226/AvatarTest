using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image containerImage;
	public Image receivingImage;
	private Color normalColor;
	public Color highlightColor = Color.yellow;
	
	public void OnEnable ()
	{
		if (containerImage != null)
			normalColor = containerImage.color;
	}
	
	public void OnDrop(PointerEventData data)
	{
		containerImage.color = normalColor;
		
		if (receivingImage == null)
			return;
		
		Sprite dropSprite = GetDropSprite (data);
		if (dropSprite != null)
			receivingImage.overrideSprite = dropSprite;




		if(data.pointerDrag)
		{
			var sourceMesh = data.pointerDrag.GetComponent<SourceMesh>();
			var avatar = GameObject.FindWithTag("TestAvatar");
			if(!avatar) 
				return;

			var avatarTest = avatar.GetComponent<AvatarTest>();
			if(sourceMesh && sourceMesh.Source)
			{
				switch(sourceMesh.Place)
				{
				case SourceMesh.BodyPlace.Hat:
					avatarTest.HatSource = sourceMesh.Source;
					break;
				case SourceMesh.BodyPlace.Top:
					avatarTest.TopSource = sourceMesh.Source;					
					break;
				case SourceMesh.BodyPlace.Hair:
					avatarTest.HairSource = sourceMesh.Source;
					break;
				case SourceMesh.BodyPlace.Bottom:
					avatarTest.BottomSource = sourceMesh.Source;
					break;
				
				}

				avatarTest.UpdataAvatarMesh();
			}
		}


	}

	public void OnPointerEnter(PointerEventData data)
	{
		if (containerImage == null)
			return;
		
		Sprite dropSprite = GetDropSprite (data);
		if (dropSprite != null)
			containerImage.color = highlightColor;
	}

	public void OnPointerExit(PointerEventData data)
	{
		if (containerImage == null)
			return;
		
		containerImage.color = normalColor;
	}
	
	private Sprite GetDropSprite(PointerEventData data)
	{
		var originalObj = data.pointerDrag;
		if (originalObj == null)
			return null;

		var srcImage = originalObj.GetComponent<Image>();
		if (srcImage == null)
			return null;
		
		return srcImage.sprite;
	}
}
