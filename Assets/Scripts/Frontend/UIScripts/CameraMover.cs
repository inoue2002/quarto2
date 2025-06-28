using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraMover : MonoBehaviour//MainCameraにアタッチ
{
    // WASD/十字キー：前後左右の移動
    // QE：上昇・降下
    // 右ドラッグ：カメラの回転
    // 左ドラッグ：前後左右の移動
    // スペース：カメラ操作の有効・無効の切り替え
    // P：回転を実行時の状態に初期化する

	//カメラの移動量
	[SerializeField, Range(10.0f, 20.0f)]
	private float _positionStep = 10.0f;

    //マウス感度
    [SerializeField, Range(30.0f, 150.0f)]
    private float _mouseSensitive = 90.0f;

    //十字キー回転速度
    [SerializeField, Range(30.0f, 120.0f)]
    private float _rotationSpeed = 60.0f;

    //十字キー距離調整速度
    [SerializeField, Range(5.0f, 20.0f)]
    private float _distanceSpeed = 10.0f;

    //ボードの中心座標
    [SerializeField]
    private Vector3 _boardCenter = Vector3.zero;

    //カメラ距離の最小・最大値
    [SerializeField, Range(5.0f, 15.0f)]
    private float _minDistance = 8.0f;
    [SerializeField, Range(15.0f, 30.0f)]
    private float _maxDistance = 25.0f;

    //カメラ操作の有効無効
	private bool _cameraMoveActive = true;
    //カメラのtransform  
    private Transform _camTransform;
    //マウスの始点 
    private Vector3 _startMousePos;
    //カメラ回転の始点情報
    private Vector3 _presentCamRotation;
    private Vector3 _presentCamPos;
    //初期状態 Rotation
    private Quaternion _initialCamRotation;
    //UIメッセージの表示
    private bool _uiMessageActiv;

    void Start ()
	{
		_camTransform = this.gameObject.transform;
		
		//初期回転の保存
		_initialCamRotation = this.gameObject.transform.rotation;
	}
	
	void Update () {
		
		CamControlIsActive(); //カメラ操作の有効無効

        if (_cameraMoveActive)
		{
			ResetCameraRotation(); //回転角度のみリセット
            CameraRotationMouseControl(); //カメラの回転 マウス
            CameraSlideMouseControl(); //カメラの縦横移動 マウス
            CameraPositionKeyControl(); //カメラのローカル移動 キー
            CameraOrbitControl(); //カメラの軌道制御 十字キー
        }
	}
	
	//カメラ操作の有効無効
	public void CamControlIsActive()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_cameraMoveActive = !_cameraMoveActive;

            if (_uiMessageActiv == false)
            {
                StartCoroutine(DisplayUiMessage());
            }            
			Debug.Log("CamControl : " + _cameraMoveActive);
		}
	}
	
	//回転を初期状態にする
	private void ResetCameraRotation()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			this.gameObject.transform.rotation = _initialCamRotation;
			Debug.Log("Cam Rotate : " + _initialCamRotation.ToString());	
		}
	}
	
	//カメラの回転 マウス
	private void CameraRotationMouseControl()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_startMousePos = Input.mousePosition;
			_presentCamRotation.x = _camTransform.transform.eulerAngles.x;
			_presentCamRotation.y = _camTransform.transform.eulerAngles.y;
		}
		
		if (Input.GetMouseButton(0))
		{
			//(移動開始座標 - マウスの現在座標) / 解像度 で正規化
			float x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
			float y = (_startMousePos.y - Input.mousePosition.y) / Screen.height;

			//回転開始角度 ＋ マウスの変化量 * マウス感度
			float eulerX = _presentCamRotation.x + y * _mouseSensitive;
			float eulerY = _presentCamRotation.y + x * _mouseSensitive;

			_camTransform.rotation = Quaternion.Euler(eulerX, eulerY, 0);
		}
	}
	
	//カメラの移動 マウス
	private void CameraSlideMouseControl()
	{
		if (Input.GetMouseButtonDown(1))
		{
			_startMousePos = Input.mousePosition;
			_presentCamPos = _camTransform.position;
		}

		if (Input.GetMouseButton(1))
		{
			//(移動開始座標 - マウスの現在座標) / 解像度 で正規化
			float x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
			float y = (_startMousePos.y - Input.mousePosition.y) / Screen.height;

            x = x * _positionStep;
            y = y * _positionStep;

            Vector3 velocity = _camTransform.rotation * new Vector3(x, y, 0);
            velocity = velocity + _presentCamPos;
            _camTransform.position = velocity;
		}
	}
		
	//カメラのローカル移動 キー（現在は無効化）
	private void CameraPositionKeyControl()
	{
		// WASDキーとQEキーは軌道制御に統合されたため、この関数は無効化
	}

    //カメラの軌道制御 十字キー・WASD・QE
    private void CameraOrbitControl()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // 左右入力でY軸回転（水平方向の軌道回転）
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) horizontalInput = -1f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) horizontalInput = 1f;

        // 前後・上下入力で距離調整
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) verticalInput = -1f; // 近づく
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) verticalInput = 1f; // 遠ざかる
        if (Input.GetKey(KeyCode.Q)) verticalInput = -1f; // 近づく（上昇）
        if (Input.GetKey(KeyCode.E)) verticalInput = 1f; // 遠ざかる（降下）

        // 水平回転の処理
        if (horizontalInput != 0f)
        {
            // ボード中心を起点にカメラを水平回転
            Vector3 direction = _camTransform.position - _boardCenter;
            Quaternion rotation = Quaternion.AngleAxis(horizontalInput * _rotationSpeed * Time.deltaTime, Vector3.up);
            Vector3 newDirection = rotation * direction;
            _camTransform.position = _boardCenter + newDirection;
            
            // カメラをボード中心に向ける
            _camTransform.LookAt(_boardCenter);
        }

        // 距離調整の処理
        if (verticalInput != 0f)
        {
            Vector3 direction = (_camTransform.position - _boardCenter).normalized;
            float currentDistance = Vector3.Distance(_camTransform.position, _boardCenter);
            float newDistance = currentDistance + verticalInput * _distanceSpeed * Time.deltaTime;
            
            // 距離を制限
            newDistance = Mathf.Clamp(newDistance, _minDistance, _maxDistance);
            
            _camTransform.position = _boardCenter + direction * newDistance;
        }
    }

    //UIメッセージの表示
    private IEnumerator DisplayUiMessage()
    {
        _uiMessageActiv = true;
        float time = 0;
        while (time < 2)
        {
            time = time + Time.deltaTime;
            yield return null;
        }
        _uiMessageActiv = false;
    }

    void OnGUI()
    {
        if (_uiMessageActiv == false) { return; }
        GUI.color = Color.black;
        if (_cameraMoveActive == true)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 30, 100, 20), "カメラ操作 有効");
        }

        if (_cameraMoveActive == false)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 30, 100, 20), "カメラ操作 無効");
        }
    }

}

