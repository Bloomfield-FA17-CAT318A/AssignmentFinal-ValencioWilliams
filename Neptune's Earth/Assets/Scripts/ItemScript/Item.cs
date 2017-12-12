using UnityEngine;

public class Item : MonoBehaviour
{
	public int lifeHp = 10;

	public int foodAmmo = 1;

	public bool isHealing;
	public bool isAmmo;

	public bool wasPickedUp = false;
//
//	void OnTriggerEnter2D(Collider2D other)
//	{
//		if (other.gameObject.tag == "Player")
//		{
//			Player player = other.GetComponent<Player> ();
//
//			player.ammo.CurrentVal += foodAmmo;
//
//			Destroy (this);
//		}
//	}
}
