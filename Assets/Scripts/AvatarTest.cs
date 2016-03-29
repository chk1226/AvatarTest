using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarTest : MonoBehaviour {

	public Transform target;

	//public Transform FaceSource;

	public int FaceNumber = 4;


	public Transform HatSource;
	public Transform HairSource;
	public Transform TopSource;
	public Transform BottomSource;

	public Transform WeaponSource;

	public int RWeapon = 0;

	public int LWeapon = 0;


	//模型資源資料
	//private Dictionary<string , Dictionary<string , SkinnedMeshRenderer>> data = new Dictionary<string , Dictionary<string , SkinnedMeshRenderer>>();

	//目標物件的骨架
	private Transform[] targetbones;

	//目標物件各部位的 SkinnedMeshRenderer 資料(參照)
	private Dictionary<string , SkinnedMeshRenderer> targetSmr = new Dictionary<string , SkinnedMeshRenderer>();
	


	void Start () {

		//取得Target的SkinnedMeshRenderer資料
		GetSkinnedMeshRenderer ();

		UpdataAvatarMesh ();


	}

	private void ChangePart(string part, SkinnedMeshRenderer item) {

		//從資料中取得各部位指定編號的 SkinnedMeshRenderer
		SkinnedMeshRenderer smr = item;

		/*
		//取得相對應名稱的骨架物件來建立新的骨架列表
		List<Transform> bones = new List<Transform>();
		foreach(Transform bone in smr.bones){

			foreach(Transform targetbone in targetbones){

				if(targetbone.name!= bone.name) continue;
				bones.Add(targetbone);
				break;

			}
		}
		*/
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 內容
		targetSmr[part].sharedMesh = smr.sharedMesh;
		//targetSmr[part].bones = bones.ToArray();
		targetSmr[part].materials = smr.sharedMaterials;


	}

	private void GetSkinnedMeshRenderer()
	{
		//取得Target的SkinnedMeshRenderer資料
		var SmrParts = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		foreach(SkinnedMeshRenderer part in SmrParts){
			string[] partName = part.name.Split('_');
			if(targetSmr.ContainsKey(partName[0]))
			{
				targetSmr[partName[0]] = part;
			}
			else
			{
				targetSmr.Add(partName[0],part);
			}
		}
	}

	public void NextFacial()
	{
		FaceNumber = (FaceNumber + 1) % 9 + 1;

		UpdataAvatarMesh ();
	}

	public void UpdataAvatarMesh()
	{

		//換Face
		Material[] Facials = new Material[2];
		
		Material mat1 = Resources.Load("Materials/FemaleFacial" , typeof(Material))as Material;
		Material mat2 = Resources.Load("Materials/FemaleBody_Nude" , typeof(Material))as Material;
		Facials[0] = mat1;
		Facials[1] = mat2;
		
		//UV Offset
		int B = (1 - ((FaceNumber-1) / 4));
		int A = ((FaceNumber - 1)%4);
		Vector2 Offset = new Vector2 (A *0.25f , B*0.25f);
		
		targetSmr["Face"].sharedMaterials = Facials;
		targetSmr["Face"].sharedMaterials[0].SetTextureOffset("_MainTex", Offset);

		
		


		SkinnedMeshRenderer[] SmrParts;

		//換Hat
		if(HatSource != null){
			SmrParts = HatSource.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach(SkinnedMeshRenderer part in SmrParts){
				
				string[] partName = part.name.Split('_');
				
				if(partName[0] == "Hat"){
					ChangePart("Hat" , part);
				}
			}
		}

		//換Hair
		if(HairSource != null){
			SmrParts = HairSource.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach(SkinnedMeshRenderer part in SmrParts){
				
				string[] partName = part.name.Split('_');
				
				if(partName[0] == "Hair"){
					ChangePart("Hair" , part);
				}
			}
		}

		//換Top
		if(TopSource != null){
			SmrParts = TopSource.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach(SkinnedMeshRenderer part in SmrParts){
				
				string[] partName = part.name.Split('_');
				
				if(partName[0] == "Top"){
					ChangePart("Top" , part);
				}
			}
		}

		//換Bottom
		if(BottomSource != null){
			SmrParts = BottomSource.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach(SkinnedMeshRenderer part in SmrParts){
				
				string[] partName = part.name.Split('_');
				
				if(partName[0] == "Bottom"){
					ChangePart("Bottom" , part);
				}
			}
		}
	}


	// Update is called once per frame
	void Update () {

//		GetSkinnedMeshRenderer ();

		//從目標物件取得骨架資料
		targetbones = target.GetComponentsInChildren<Transform>();

		
		/*
		int children_count = WeaponSource.transform.childCount;

		for (int i = 0; i < children_count; ++i)
		{
			Transform tm = WeaponSource.transform.GetChild(i);

			MeshFilter filter = tm.GetComponent<MeshFilter>();
			if (null != filter)
			{
			}
		}
		Transform tm_axe = WeaponSource.FindChild("Axe");
		if (null != tm_axe)
		{
			MeshFilter filter = tm_axe.GetComponent<MeshFilter>();
			if (null != filter)
			{
				targetSmr["RWeapon"].sharedMesh = filter.sharedMesh;
			}
		}
		*/
		//換Weapon
		int children_count = WeaponSource.transform.childCount;
		
		for (int i = 0; i < children_count; ++i)
		{
			Transform tm = WeaponSource.transform.GetChild(i);
			
			MeshFilter filter = tm.GetComponent<MeshFilter>();
			if (null != filter)
			{
				//右手
				if (i+1 == RWeapon)
				{
					targetSmr["RWeapon"].enabled = true;
					targetSmr["RWeapon"].sharedMesh = filter.sharedMesh;
					targetSmr["RWeapon"].materials = tm.GetComponent<MeshRenderer>().sharedMaterials;
				}else if (0 == RWeapon)
				{
					targetSmr["RWeapon"].enabled = false;
				}
				//左手
				if (i+1 == LWeapon)
				{
					targetSmr["LWeapon"].enabled = true;
					targetSmr["LWeapon"].sharedMesh = filter.sharedMesh;
					targetSmr["LWeapon"].materials = tm.GetComponent<MeshRenderer>().sharedMaterials;
				}else if (0 ==LWeapon)
				{
					targetSmr["LWeapon"].enabled = false;
				}
			}
		}
	}
}
