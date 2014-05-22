using UnityEngine;
using System.Collections;

public class randomOffset : MonoBehaviour {
    #region Public variables
	public float manualTime = 1.0f;
	public float frequency = 24.0f;
	public int offsetX = 10;
	public int offsetY = 10;
	public Texture2D[] textures;
	public float timeForSwap = 1.0f;
    #endregion

    #region Private variables
	private float count = 0;
	private int index = 0;
    #endregion

    #region Structs 
    #endregion

    #region Enums
    #endregion

    #region Inits
    //Called even if script component is not enabled
    //best used for references between scripts and inits
    void Awake() {

    }
    
    //Called if script component is enabled
	IEnumerator Start () {
		float customtime = manualTime / frequency;
		renderer.materials[0].SetTexture("_MainTex",textures[index]);
		yield return StartCoroutine("UpdateTexture", customtime);
		
	}
    #endregion

    #region Update funtions
    //every frame (1)
	void Update () {
	
	}

    //after Update every frame (2)
    void LateUpdate() {

    }

    //physics updates (3) does not happen every frame
    void FixedUpdate() {

    }
    #endregion

    #region Private functions
	IEnumerator UpdateTexture ( float waitTime ){
		while(true){
			count += Time.deltaTime;
			if(count>timeForSwap) {
				count = 0;
				index++;
				
				index = index%(textures.Length);
				renderer.materials[0].SetTexture("_MainTex",textures[index]);
			}
			Vector2 offset = new Vector2(offsetX * Random.value,offsetY * Random.value);
			renderer.materials[0].SetTextureOffset ("_GrainTex", offset);
			//print(renderer.material + " < material");
			yield return new WaitForSeconds(waitTime);
		}
	}
    #endregion

    #region Public functions
    #endregion
}
