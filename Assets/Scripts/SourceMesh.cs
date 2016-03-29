using UnityEngine;
using System.Collections;

public class SourceMesh : MonoBehaviour {

	public enum BodyPlace
	{
		Hat,
		Hair,
		Top,
		Bottom,
	}

	public BodyPlace Place;

	public Transform Source;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
