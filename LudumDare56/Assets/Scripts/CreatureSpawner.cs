using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour 
{

public GameObject[] creaturePrefabs;

public int gridRowSize = 10;
public float gridHorizontalSpacing = 1.0f;
public int gridColumnSize = 10;
public float gridVerticalSpacing = 1.0f;

public void SpawnCreatures()
{
	int rows = gridRowSize;
	int columns = gridColumnSize;

	float offsetX = rows * gridHorizontalSpacing / 2f;
	float offsetY = columns * gridVerticalSpacing / 2f;

	for (int i = 0; i < rows; i++)
	{
		for (int j = 0; j < columns; j++)
		{
			Vector3 position = transform.position + new Vector3(i * gridHorizontalSpacing, j * -gridVerticalSpacing, 0);
			position.x -= offsetX;
			position.y += offsetY;

			int prefabIndex = 0;
			GameObject creaturePrefab = creaturePrefabs[prefabIndex];
			Instantiate(creaturePrefab, position, Quaternion.identity);
		}
	}
}
	
}
