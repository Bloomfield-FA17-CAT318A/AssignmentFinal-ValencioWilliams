using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{

	void Enter (Enemy enemy);

	void Starting ();

	void Ending();

	void OnTriggerEnter2D (Collider2D other);

}
