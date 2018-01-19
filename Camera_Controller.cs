using UnityEngine;
using System.Collections;

public class Camera_Controller : MonoBehaviour {

	public GameObject target;
	public Property_Informer selector;

	private Vector3 offset;
	private Vector3 selectorposition;

	public float speed =1;

	private bool started = false;
	private bool FreeCamera = false;

	// Use this for initialization
	public void Start () {
		if (!started) {
			//offset = transform.position - target.transform.position;
			started = true;
			selector.gameObject.SetActive (FreeCamera);
			selectorposition = selector.transform.localPosition;
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		if (!FreeCamera) {
			transform.position = target.transform.position + offset;
			selector.transform.localPosition = selectorposition;
		} else {
			if (!Menu_Manager.singleton.isOpenMenu) {
				float horizontal = Input.GetAxis ("Horizontal");
				if (horizontal > .5) {
					horizontal = 1f;
				} else if (horizontal < -.5) {
					horizontal = -1f;
				} else {
					horizontal = 0f;
				}
				horizontal *= -1;

				float vertical = Input.GetAxis ("Vertical");
				if (vertical > .5) {
					vertical = 1f;
				} else if (vertical < -.5) {
					vertical = -1f;
				} else {
					vertical = 0f;
				}
				vertical *= -1;

				horizontal *= speed;
				vertical *= speed;

				Vector3 positionMove = new Vector3 (horizontal, 0, vertical);
				transform.position += positionMove;
				selector.transform.localPosition = selectorposition;
			}
		}
	}

	public void SetTarget(GameObject n_target){
		target = n_target;
		offset = transform.position - target.transform.position;
	}

	public void ChangeTarget(GameObject n_target){
		target = n_target;
	}

	public void SwitchFreeCamera(){
		FreeCamera = !FreeCamera;
		selector.gameObject.SetActive (FreeCamera);
		selector.transform.localPosition = selectorposition;
	}

	public void StaticCamera(){
		FreeCamera = false;
		selector.gameObject.SetActive (FreeCamera);
		selector.transform.localPosition = selectorposition;
	}

	public void FreeCameraMovement(){
		FreeCamera = true;
		selector.gameObject.SetActive (FreeCamera);
		selector.transform.localPosition = selectorposition;
	}

	public bool GetCameraMode(){
		return FreeCamera;
	}
}
