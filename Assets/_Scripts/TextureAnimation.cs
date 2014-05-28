using UnityEngine;
using System.Collections;

public class TextureAnimation : MonoBehaviour {

	public float _nrOfColumns;
	public float _nrOfRows;
	public float _frameTime;

	public Material _sheet;
	public bool _startFromScript = false;
	public Rect _aniRect = new Rect(4,5,6,7);
	bool _activateOnGUI = false;
	public float _clock = 0;
	float _currentFrameColumn;
	float _currentFrameRow;


	// Use this for initialization
	void Start () {
	
		_currentFrameColumn = 0;
		_currentFrameRow = 0;
		//_sheet.SetTextureScale("_MainTex",new Vector2(1/_nrOfColumns,1/_nrOfRows));


	}
	
	// Update is called once per frame
	void Update () {
	
		if(Time.time - _clock >= _frameTime){
			if(_currentFrameColumn <  _nrOfColumns-1){
				_currentFrameColumn += 1;
			}
			else{
				if(_currentFrameRow > 0){
					_currentFrameRow -= 1;
				}
				else{
					_currentFrameRow = _nrOfRows -1;
				}
				_currentFrameColumn = 0;
			}
			_sheet.SetTextureOffset("_MainTex",new Vector2(_currentFrameColumn/_nrOfColumns,(_currentFrameRow/_nrOfRows)));
			_clock = Time.time;

		}
		if(_startFromScript){
			setSheet(_sheet, _nrOfColumns, _nrOfRows, _frameTime);
			_activateOnGUI = true;
			_startFromScript = false;
		}
	}


	public void setSheet(Material sheet,float nrOfColumns, float nrOfRows, float frameTime){
		_sheet = sheet;
		_nrOfColumns = nrOfColumns;
		_nrOfRows = nrOfRows;
		_frameTime = frameTime;
		_sheet.SetTextureScale("_MainTex",new Vector2(1/_nrOfColumns,1/_nrOfRows));
		_sheet.SetTextureOffset("_MainTex",new Vector2(0,1 - (1/_nrOfRows)));
		_currentFrameRow = _nrOfRows-1;
		_clock = Time.time;
	}

	public Material getSheet(){
		return _sheet;
	}

	public Rect horunge(){
		return new Rect(_sheet.GetTextureOffset("_MainTex").x,
		                _sheet.GetTextureOffset("_MainTex").y,
		                _sheet.GetTextureScale("_MainTex").x,
		                _sheet.GetTextureScale("_MainTex").y);
	}

	void OnGUI(){
		if(_activateOnGUI){
			GUI.DrawTextureWithTexCoords(scaleRect(_aniRect),_sheet.mainTexture,horunge());
		}
	}

	Rect scaleRect(Rect r){
		return new Rect(Screen.width/r.x,Screen.height/r.y,Screen.width/r.width,Screen.height/r.height);
	}
}
