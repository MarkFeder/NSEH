using UnityEngine;
using nseh.Managers.Main;

public class TakeablePowerUp : MonoBehaviour {
	CustomizablePowerUp customPowerUp;

	void Start() {
		customPowerUp = (CustomizablePowerUp)transform.parent.gameObject.GetComponent<CustomizablePowerUp>();
		//this.audio.clip = customPowerUp.pickUpSound;
	}

	void OnTriggerEnter (Collider collider) {
		if(collider.tag == "Player") {
			PowerUpManager.Instance.Add(customPowerUp);
			if(customPowerUp.pickUpSound != null){
                GameManager.Instance.SoundManager.PlayAudioFX(customPowerUp.pickUpSound, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            }
			Destroy(transform.parent.gameObject);
		}
	}
}
